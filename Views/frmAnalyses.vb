' ============================================================================
' FICHIER : Views/frmAnalyses.vb  (partie « code » du formulaire)
'           frmAnalyses.Designer.vb contient la partie « dessin » ; les deux forment UNE classe.
' RÔLE    : onglet « Analyses » de la maquette :
'           - EF-10 indice de prix personnel (base 100) entre deux dates ;
'           - EF-11 comparaison de magasins (par produit ou par catégorie) ;
'           - EF-12 produits équivalents moins chers.
'           Les résultats sont une information, jamais un conseil d'achat (rappel affiché en bas).
' RÈGLE   : le formulaire ne contient que la logique d'interface ; les calculs sont dans
'           IndiceService et ComparaisonService.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Linq                ' Pour Where, ToDictionary...
Imports PreisRontgen.Data          ' Pour IAnalyseRepository
Imports PreisRontgen.Models        ' Pour ProduitRef, LignePrixMagasin, Equivalent
Imports PreisRontgen.Services      ' Pour IndiceService, ComparaisonService, FormatageAnalyse

''' <summary>EF-10, EF-11 et EF-12 : indice personnel, comparaison de magasins, équivalents.</summary>
Public Class frmAnalyses                           ' Début de la classe (Partial dans le fichier Designer)

    ''' <summary>Ligne du classement des magasins.</summary>
    Friend Class LigneMagasin                      ' Une ligne déjà mise en forme
        Public Property Produit As String = String.Empty       ' Colonne « Produit »
        Public Property Magasin As String = String.Empty       ' Colonne « Magasin »
        Public Property PrixTexte As String = String.Empty     ' Colonne « Prix unitaire »
        Public Property DateTexte As String = String.Empty     ' Colonne « Dernière obs. »
        Public Property EcartTexte As String = String.Empty    ' Colonne « Écart » (« Moins cher » ou « + 12,5 % »)
        Public Property EstLeMoinsCher As Boolean              ' Sert à colorer la ligne
    End Class                                      ' Fin de LigneMagasin

    ''' <summary>Ligne de la liste des équivalents moins chers.</summary>
    Friend Class LigneEquivalent                   ' Une ligne déjà mise en forme
        Public Property Produit As String = String.Empty         ' Colonne « Produit »
        Public Property Magasin As String = String.Empty         ' Colonne « Magasin »
        Public Property PrixTexte As String = String.Empty       ' Colonne « Prix unitaire »
        Public Property EconomieTexte As String = String.Empty   ' Colonne « Économie »
    End Class                                      ' Fin de LigneEquivalent

    Private ReadOnly _depot As IAnalyseRepository          ' Accès aux données
    Private ReadOnly _indice As IndiceService              ' Calcul de l'indice
    Private ReadOnly _comparaison As ComparaisonService    ' Comparaisons et équivalents

    ''' <summary>Constructeur sans paramètre, réservé au concepteur de Visual Studio.</summary>
    Public Sub New()                               ' Constructeur du concepteur
        InitializeComponent()                      ' Construit les contrôles
    End Sub                                        ' Fin du constructeur

    Public Sub New(depot As IAnalyseRepository)    ' Constructeur utilisé par l'application
        Me.New()                                   ' Construit l'écran
        If depot Is Nothing Then Throw New ArgumentNullException(NameOf(depot))   ' Refuse un dépôt absent
        _depot = depot                             ' Mémorise le dépôt
        _indice = New IndiceService(depot)         ' Service de l'indice
        _comparaison = New ComparaisonService(depot)   ' Service des comparaisons
    End Sub                                        ' Fin du constructeur

    Private Sub frmAnalyses_Load(sender As Object, e As EventArgs) Handles MyBase.Load   ' À l'ouverture
        If DesignMode OrElse _depot Is Nothing Then Return   ' Rien à faire dans le concepteur
        dtpDepart.Value = Date.Today.AddMonths(-3)   ' Départ proposé : il y a 3 mois
        dtpArrivee.Value = Date.Today                ' Arrivée proposée : aujourd'hui
        lblIndiceNote.Text = IndiceService.Avertissement   ' Rappel sous l'indice
        lblAvertissement.Text = ComparaisonService.Avertissement   ' Rappel en bas de l'écran
        RechargerListes()                            ' Remplit les listes de produits et de catégories
    End Sub                                          ' Fin de frmAnalyses_Load

    ''' <summary>Recharge les listes déroulantes (à appeler après l'ajout de produits).</summary>
    Public Sub RechargerListes()                     ' Remplit les listes
        If _depot Is Nothing Then Return             ' Mode conception
        Try                                          ' Début du bloc surveillé
            Dim produits As New List(Of ProduitRef)()   ' Produits proposés
            produits.Add(New ProduitRef With {.Id = 0, .Nom = "(Choisir un produit)"})   ' Premier choix : aucun
            produits.AddRange(_depot.ListerProduits())   ' Produits de la base
            cmbProduit.DisplayMember = "Nom"         ' Affiche le nom
            cmbProduit.ValueMember = "Id"            ' Valeur interne : l'identifiant
            cmbProduit.DataSource = produits         ' Remplit la liste

            Dim categories As New List(Of KeyValuePair(Of Integer, String))()   ' Catégories proposées
            categories.Add(New KeyValuePair(Of Integer, String)(0, "(Aucune)"))   ' Premier choix : aucune
            categories.AddRange(_depot.ListerCategories())   ' Catégories de la base
            cmbCategorie.DisplayMember = "Value"     ' Affiche le nom
            cmbCategorie.ValueMember = "Key"         ' Valeur interne : l'identifiant
            cmbCategorie.DataSource = categories     ' Remplit la liste
        Catch ex As InvalidOperationException        ' Erreur d'accès aux données
            MessageBox.Show(ex.Message, "Analyses", MessageBoxButtons.OK, MessageBoxIcon.Error)   ' Affiche l'erreur
        End Try                                      ' Fin du bloc surveillé
    End Sub                                          ' Fin de RechargerListes

    Private Function ProduitChoisi() As ProduitRef   ' Produit sélectionné (Nothing si « Choisir un produit »)
        Dim p As ProduitRef = TryCast(cmbProduit.SelectedItem, ProduitRef)   ' Élément sélectionné
        If p Is Nothing OrElse p.Id = 0 Then Return Nothing   ' Aucun produit choisi
        Return p                                     ' Rend le produit
    End Function                                     ' Fin de ProduitChoisi

    Private Function CategorieChoisie() As Integer   ' Identifiant de la catégorie choisie (0 = aucune)
        If cmbCategorie.SelectedItem Is Nothing Then Return 0   ' Rien de sélectionné
        Return CType(cmbCategorie.SelectedItem, KeyValuePair(Of Integer, String)).Key   ' Identifiant
    End Function                                     ' Fin de CategorieChoisie

    ' ---------------------------------------------------------------- EF-10 : indice

    Private Sub btnCalculer_Click(sender As Object, e As EventArgs) Handles btnCalculer.Click   ' Clic sur « Calculer »
        If dtpArrivee.Value.Date <= dtpDepart.Value.Date Then   ' Dates dans le mauvais ordre
            MessageBox.Show("La date d'arrivée doit être postérieure à la date de départ.", "Indice de prix",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)   ' Message de validation clair
            dtpArrivee.Focus()                       ' Replace le curseur
            Return                                   ' On ne calcule pas
        End If                                       ' Fin de la validation

        Try                                          ' Début du bloc surveillé
            Dim r As ResultatIndice = _indice.Calculer(dtpDepart.Value.Date, dtpArrivee.Value.Date)   ' Calcule l'indice
            If r.Indice.HasValue Then                ' Calcul réussi
                Dim valeur As Decimal = r.Indice.Value   ' Indice (base 100)
                lblIndiceValeur.Text = valeur.ToString("0.00")   ' Affiche la valeur
                If valeur > 100D Then                ' Les prix ont augmenté
                    lblIndiceValeur.ForeColor = Color.FromArgb(150, 50, 20)   ' Brun-rouge
                ElseIf valeur < 100D Then            ' Les prix ont baissé
                    lblIndiceValeur.ForeColor = Color.FromArgb(30, 77, 58)    ' Vert forêt
                Else                                 ' Stable
                    lblIndiceValeur.ForeColor = Color.FromArgb(23, 38, 31)    ' Texte sombre
                End If                               ' Fin du choix de couleur
                Dim variation As String = (valeur - 100D).ToString("+0.00;-0.00;0.00")   ' Variation en pourcentage
                lblIndiceTexte.Text = $"Panier de {r.NombreProduits} produit(s) : {FormatageAnalyse.FormaterMontant(r.CoutDepart)} FCFA au départ, " &
                                      $"{FormatageAnalyse.FormaterMontant(r.CoutArrivee)} FCFA à l'arrivée ({variation} %)."   ' Détail du calcul
            Else                                     ' Calcul impossible
                lblIndiceValeur.Text = "—"           ' Pas de valeur
                lblIndiceValeur.ForeColor = Color.FromArgb(90, 106, 98)   ' Gris
                lblIndiceTexte.Text = r.Message      ' Explication
            End If                                   ' Fin du choix
        Catch ex As InvalidOperationException        ' Erreur d'accès aux données
            MessageBox.Show(ex.Message, "Indice de prix", MessageBoxButtons.OK, MessageBoxIcon.Error)   ' Affiche l'erreur
        End Try                                      ' Fin du bloc surveillé
    End Sub                                          ' Fin du gestionnaire

    ' ---------------------------------------------------------------- EF-11 : comparaison

    Private Sub btnComparer_Click(sender As Object, e As EventArgs) Handles btnComparer.Click   ' Clic sur « Comparer magasins »
        Dim produit As ProduitRef = ProduitChoisi()  ' Produit choisi
        Dim idCategorie As Integer = CategorieChoisie()   ' Catégorie choisie
        If produit Is Nothing AndAlso idCategorie = 0 Then   ' Rien de choisi
            MessageBox.Show("Choisissez un produit ou une catégorie.", "Comparaison de magasins",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)   ' Message clair
            cmbProduit.Focus()                       ' Replace le curseur
            Return                                   ' On ne compare pas
        End If                                       ' Fin de la validation

        Try                                          ' Début du bloc surveillé
            Dim lignes As List(Of LignePrixMagasin) = If(produit IsNot Nothing,
                _comparaison.ComparerMagasins(produit.Id), _comparaison.ComparerCategorie(idCategorie))   ' Le produit prime sur la catégorie
            Dim moinsCher As Dictionary(Of Integer, Decimal) = lignes.GroupBy(Function(l) l.IdProduit).ToDictionary(Function(gr) gr.Key, Function(gr) gr.Min(Function(l) l.PrixUnitaire))   ' Prix le plus bas de chaque produit
            Dim affichage As New List(Of LigneMagasin)(lignes.Count)   ' Lignes du tableau
            For Each l As LignePrixMagasin In lignes     ' Pour chaque magasin
                Dim minimum As Decimal = moinsCher(l.IdProduit)   ' Meilleur prix de ce produit
                Dim estMin As Boolean = (l.PrixUnitaire = minimum)   ' Cette ligne est la moins chère
                affichage.Add(New LigneMagasin With {
                    .Produit = l.NomProduit,         ' Produit
                    .Magasin = l.NomMagasin,         ' Magasin
                    .PrixTexte = FormatageAnalyse.FormaterPrix(l.PrixUnitaire, l.UniteBase),   ' Prix unitaire
                    .DateTexte = l.DateObs.ToString("d"),   ' Date de l'observation
                    .EcartTexte = If(estMin, "Moins cher", "+ " & ((l.PrixUnitaire - minimum) / minimum * 100D).ToString("0.0") & " %"),   ' Écart avec le moins cher
                    .EstLeMoinsCher = estMin         ' Pour la couleur
                })                                   ' Ajoute la ligne
            Next                                     ' Fin de la boucle
            dgvMagasins.DataSource = affichage       ' Affiche le tableau
            dgvMagasins.ClearSelection()             ' Pas de ligne sélectionnée d'office
            lblMessage.Text = If(affichage.Count = 0, "Aucune observation de prix pour ce choix.", $"{affichage.Count} prix comparé(s).")   ' Message d'information
        Catch ex As InvalidOperationException        ' Erreur d'accès aux données
            MessageBox.Show(ex.Message, "Comparaison de magasins", MessageBoxButtons.OK, MessageBoxIcon.Error)   ' Affiche l'erreur
        End Try                                      ' Fin du bloc surveillé
    End Sub                                          ' Fin du gestionnaire

    Private Sub dgvMagasins_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvMagasins.CellFormatting   ' Avant l'affichage de chaque cellule
        If e.RowIndex < 0 Then Return                ' En-tête : rien à faire
        Dim ligne As LigneMagasin = TryCast(dgvMagasins.Rows(e.RowIndex).DataBoundItem, LigneMagasin)   ' Objet affiché sur cette ligne
        If ligne IsNot Nothing AndAlso ligne.EstLeMoinsCher Then   ' La ligne la moins chère
            e.CellStyle.BackColor = Color.FromArgb(232, 244, 236)  ' Fond vert très clair
        End If                                       ' Fin du contrôle
    End Sub                                          ' Fin du gestionnaire

    ' ---------------------------------------------------------------- EF-12 : équivalents

    Private Sub btnEquivalents_Click(sender As Object, e As EventArgs) Handles btnEquivalents.Click   ' Clic sur « Voir équivalents moins chers »
        Dim produit As ProduitRef = ProduitChoisi()  ' Produit choisi
        If produit Is Nothing Then                   ' Aucun produit
            MessageBox.Show("Choisissez d'abord un produit.", "Produits équivalents",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)   ' Message clair
            cmbProduit.Focus()                       ' Replace le curseur
            Return                                   ' On ne cherche pas
        End If                                       ' Fin de la validation

        Try                                          ' Début du bloc surveillé
            Dim liste As List(Of Equivalent) = _comparaison.TrouverEquivalents(produit.Id)   ' Cherche les équivalents
            Dim affichage As New List(Of LigneEquivalent)(liste.Count)   ' Lignes du tableau
            For Each q As Equivalent In liste        ' Pour chaque équivalent
                affichage.Add(New LigneEquivalent With {
                    .Produit = q.NomProduit,         ' Produit
                    .Magasin = q.NomMagasin,         ' Magasin
                    .PrixTexte = FormatageAnalyse.FormaterPrix(q.PrixUnitaire, q.UniteBase),   ' Prix unitaire
                    .EconomieTexte = (q.EconomiePct * 100D).ToString("0.0") & " %"             ' Économie par rapport au produit choisi
                })                                   ' Ajoute la ligne
            Next                                     ' Fin de la boucle
            dgvEquivalents.DataSource = affichage    ' Affiche le tableau
            dgvEquivalents.ClearSelection()          ' Pas de ligne sélectionnée d'office
            lblMessage.Text = If(affichage.Count = 0, "Aucun équivalent moins cher trouvé pour ce produit.", $"{affichage.Count} équivalent(s) moins cher(s) trouvé(s).")   ' Message d'information
        Catch ex As InvalidOperationException        ' Erreur d'accès aux données
            MessageBox.Show(ex.Message, "Produits équivalents", MessageBoxButtons.OK, MessageBoxIcon.Error)   ' Affiche l'erreur
        End Try                                      ' Fin du bloc surveillé
    End Sub                                          ' Fin du gestionnaire

End Class                                            ' Fin de frmAnalyses
