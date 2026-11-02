' ============================================================================
' FICHIER : Views/frmAlertes.vb  (partie « code » du formulaire)
'           Le fichier frmAlertes.Designer.vb contient la partie « dessin »
'           (création et placement des contrôles) ; les deux forment UNE classe.
' RÔLE    : EF-09, liste des alertes avec filtres (période, magasin, type), case
'           « Non lues uniquement » et statut lu / non lu. La présentation suit
'           la maquette du groupe (onglet « Alertes », vert forêt).
' RÈGLE   : un formulaire ne contient QUE la logique d'interface (gestionnaires
'           d'événements). Les accès aux données passent par IAlerteRepository.
' NOTION  : « Handles X.Evenement » relie une méthode à un événement : quand
'           l'utilisateur clique, la méthode s'exécute. C'est la programmation
'           événementielle.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports PreisRontgen.Data      ' Pour utiliser IAlerteRepository
Imports PreisRontgen.Models    ' Pour utiliser Alerte, FiltreAlerte, TypeAlerte

''' <summary>
''' EF-09 : liste des alertes avec filtres (période, magasin, type) et statut lu / non lu.
''' Le formulaire ne contient que la logique d'interface : les accès aux données passent
''' par IAlerteRepository.
''' </summary>
Public Class frmAlertes        ' Début de la classe du formulaire (Partial dans le fichier Designer)

    ''' <summary>Ligne affichée dans la grille (liaison aux colonnes par DataPropertyName).</summary>
    Friend Class LigneAlerte   ' Classe imbriquée : une ligne de la grille, déjà mise en forme pour l'affichage
        Public Property Id As Integer                          ' Identifiant de l'alerte (pour la marquer comme lue)
        Public Property Lue As Boolean                         ' Sert à afficher les non lues en gras
        Public Property DateTexte As String = String.Empty     ' Date formatée (colonne « Date »)
        Public Property Produit As String = String.Empty       ' Nom du produit (colonne « Produit »)
        Public Property Magasin As String = String.Empty       ' Nom du magasin (colonne « Magasin »)
        Public Property TypeTexte As String = String.Empty     ' « HAUSSE » ou « SHRINKFLATION » (colonne « Type »)
        Public Property VariationTexte As String = String.Empty   ' Variation en pourcentage (colonne « Variation prix unitaire »)
        Public Property Detail As String = String.Empty        ' Phrase explicative (affichée dans le panneau « Détail de l'alerte sélectionnée »)
        Public Property StatutTexte As String = String.Empty   ' « Lue » ou « Non lue » (colonne « Statut »)
    End Class                                                  ' Fin de LigneAlerte

    ''' <summary>
    ''' Déclenché après chaque rechargement de la liste ; le formulaire principal s'y abonne
    ''' pour mettre à jour son compteur d'alertes non lues.
    ''' </summary>
    Public Event CompteurChange(nombreNonLues As Integer)      ' Événement personnalisé : transmet le nombre d'alertes non lues

    Private Const ChoixTous As String = "Tous"                 ' Texte du choix « pas de filtre » de la liste des magasins
    Private Const ChoixToutes As String = "Toutes"             ' Texte du choix « pas de filtre » de la liste des types (comme la maquette)
    Private Const TexteHausse As String = "HAUSSE"             ' Libellé affiché pour une hausse
    Private Const TexteShrinkflation As String = "SHRINKFLATION"   ' Libellé affiché pour une shrinkflation

    Private ReadOnly _depot As IAlerteRepository               ' Accès aux données (reçu à la création du formulaire)
    Private _policeGras As Font                                ' Police en gras pour les alertes non lues (créée au chargement)

    ''' <summary>Constructeur sans paramètre, réservé au concepteur de Visual Studio.</summary>
    Public Sub New()               ' Le concepteur Visual Studio a besoin d'un constructeur sans paramètre
        InitializeComponent()      ' Construit les contrôles (méthode du fichier Designer)
    End Sub                        ' Fin du constructeur

    Public Sub New(depot As IAlerteRepository)   ' Constructeur utilisé par l'application
        Me.New()                                 ' Appelle d'abord le constructeur sans paramètre (construit l'écran)
        If depot Is Nothing Then Throw New ArgumentNullException(NameOf(depot))   ' Refuse un dépôt absent
        _depot = depot                           ' Mémorise le dépôt
    End Sub                                      ' Fin du constructeur

    Private Sub frmAlertes_Load(sender As Object, e As EventArgs) Handles MyBase.Load   ' Exécuté à l'ouverture du formulaire (événement Load)
        If DesignMode OrElse _depot Is Nothing Then   ' En mode conception Visual Studio, ou sans dépôt
            Return                                    ' On ne fait rien (évite une erreur dans le concepteur)
        End If                                        ' Fin du contrôle

        _policeGras = New Font(dgvAlertes.Font, FontStyle.Bold)   ' Crée une version grasse de la police de la grille
        dtpDebut.Value = Date.Today.AddMonths(-3)     ' Période proposée : début = il y a 3 mois
        dtpFin.Value = Date.Today                     ' Période proposée : fin = aujourd'hui

        cmbType.Items.AddRange(New Object() {ChoixToutes, "Hausse", "Shrinkflation"})   ' Remplit la liste des types
        cmbType.SelectedIndex = 0                     ' Sélectionne « Toutes »
        chkNonLues.Checked = False                    ' Par défaut, on voit les alertes lues et non lues

        ChargerMagasins()                             ' Remplit la liste des magasins depuis la base
        Rafraichir()                                  ' Affiche les alertes avec les filtres par défaut
    End Sub                                           ' Fin de frmAlertes_Load

    Private Sub frmAlertes_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed   ' Exécuté à la fermeture
        If _policeGras IsNot Nothing Then             ' Si la police grasse a été créée
            _policeGras.Dispose()                     ' Libère cette ressource graphique (bonne pratique)
            _policeGras = Nothing                     ' Oublie la référence
        End If                                        ' Fin du contrôle
    End Sub                                           ' Fin de frmAlertes_FormClosed

    ''' <summary>Recharge la liste avec les filtres courants. Peut être appelée depuis un autre thread.</summary>
    Public Sub Rafraichir()                           ' Recharge la grille
        If _depot Is Nothing Then                     ' Pas de dépôt (mode conception)
            Return                                    ' Rien à faire
        End If                                        ' Fin du contrôle
        If InvokeRequired Then                        ' Vrai si on est appelé depuis un autre thread que celui de l'écran
            Invoke(New Action(AddressOf Rafraichir))  ' Se fait ré-exécuter sur le thread de l'écran (obligatoire pour toucher aux contrôles)
            Return                                    ' La version exécutée sur le bon thread fera le travail
        End If                                        ' Fin du contrôle de thread

        Try                                           ' Début du bloc surveillé
            Dim alertes As List(Of Alerte) = _depot.Rechercher(ConstruireFiltre())   ' Lit les alertes qui respectent les filtres de l'écran
            Dim lignes As New List(Of LigneAlerte)(alertes.Count)   ' Liste des lignes à afficher (capacité prévue = nombre d'alertes)
            For Each a As Alerte In alertes           ' Pour chaque alerte trouvée
                lignes.Add(VersLigne(a))              ' La convertit en ligne d'affichage
            Next                                      ' Fin de la boucle
            dgvAlertes.DataSource = lignes            ' Donne la liste à la grille : elle s'affiche

            Dim nonLues As Integer = _depot.CompterNonLues()   ' Nombre total d'alertes non lues
            lblCompteur.Text = $"{lignes.Count} alerte(s) affichée(s) - {nonLues} non lue(s) au total"   ' Texte récapitulatif en bas de l'écran
            RaiseEvent CompteurChange(nonLues)        ' Prévient le formulaire principal du nouveau compteur
        Catch ex As InvalidOperationException         ' Erreur d'accès aux données (signalée par le dépôt)
            MessageBox.Show(ex.Message, "Alertes", MessageBoxButtons.OK, MessageBoxIcon.Error)   ' Affiche le message d'erreur à l'utilisateur
        End Try                                       ' Fin du bloc surveillé
    End Sub                                           ' Fin de Rafraichir

    Private Sub ChargerMagasins()                     ' Remplit la liste déroulante des magasins
        cmbMagasin.Items.Clear()                      ' Vide la liste
        cmbMagasin.Items.Add(New KeyValuePair(Of Integer, String)(0, ChoixTous))   ' Premier choix : « Tous » (identifiant 0 = pas de filtre)
        Try                                           ' Début du bloc surveillé
            For Each m As KeyValuePair(Of Integer, String) In _depot.ListerMagasins()   ' Pour chaque magasin de la base
                cmbMagasin.Items.Add(m)               ' L'ajoute à la liste
            Next                                      ' Fin de la boucle
        Catch ex As InvalidOperationException         ' Erreur d'accès aux données
            MessageBox.Show(ex.Message, "Alertes", MessageBoxButtons.OK, MessageBoxIcon.Error)   ' Affiche l'erreur
        End Try                                       ' Fin du bloc surveillé
        cmbMagasin.DisplayMember = "Value"            ' La liste affiche le nom (propriété Value du couple)
        cmbMagasin.ValueMember = "Key"                ' La valeur interne est l'identifiant (propriété Key du couple)
        cmbMagasin.SelectedIndex = 0                  ' Sélectionne « Tous »
    End Sub                                           ' Fin de ChargerMagasins

    Private Function ConstruireFiltre() As FiltreAlerte   ' Lit l'écran et en déduit le filtre
        Dim filtre As New FiltreAlerte()              ' Crée un filtre vide (aucun critère)

        filtre.DateDebut = dtpDebut.Value.Date        ' Date de début choisie (« Du »)
        filtre.DateFin = dtpFin.Value.Date            ' Date de fin choisie (« Au »)

        If cmbMagasin.SelectedItem IsNot Nothing Then   ' Un élément est sélectionné dans la liste des magasins
            Dim choix As KeyValuePair(Of Integer, String) = CType(cmbMagasin.SelectedItem, KeyValuePair(Of Integer, String))   ' Convertit l'élément en couple (id, nom)
            If choix.Key <> 0 Then                    ' Ce n'est pas « Tous »
                filtre.IdMagasin = choix.Key          ' Filtre sur ce magasin
            End If                                    ' Fin du contrôle
        End If                                        ' Fin du magasin

        Select Case cmbType.SelectedIndex             ' Selon la position choisie dans la liste des types
            Case 1                                    ' Position 1 = « Hausse »
                filtre.Type = TypeAlerte.Hausse       ' Filtre sur les hausses
            Case 2                                    ' Position 2 = « Shrinkflation »
                filtre.Type = TypeAlerte.Shrinkflation   ' Filtre sur la shrinkflation
        End Select                                    ' Position 0 = « Tous » : aucun filtre

        If chkNonLues.Checked Then                    ' La case « Non lues uniquement » est cochée
            filtre.Lue = False                        ' Filtre sur les non lues
        End If                                        ' Sinon : aucun filtre sur le statut

        Return filtre                                 ' Rend le filtre construit
    End Function                                      ' Fin de ConstruireFiltre

    Private Shared Function VersLigne(a As Alerte) As LigneAlerte   ' Convertit une Alerte en ligne d'affichage
        Return New LigneAlerte With {                 ' Crée la ligne et remplit ses champs
            .Id = a.Id,                               ' Identifiant
            .Lue = a.Lue,                             ' État lu / non lu
            .DateTexte = a.DateAlerte.ToString("d"),  ' Date courte selon la langue de Windows (ex. 15/10/2026)
            .Produit = a.NomProduit,                  ' Nom du produit
            .Magasin = a.NomMagasin,                  ' Nom du magasin
            .TypeTexte = If(a.Type = TypeAlerte.Hausse, TexteHausse, TexteShrinkflation),   ' Libellé du type (en majuscules comme la maquette)
            .VariationTexte = a.Valeur.ToString("+ 0.0 %;- 0.0 %;0.0 %"),   ' Format : + 5,3 % si positif ; - 2,0 % si négatif ; 0,0 % si nul
            .Detail = a.Detail,                       ' Phrase explicative
            .StatutTexte = If(a.Lue, "Lue", "Non lue")   ' Libellé du statut
        }                                             ' Fin de l'initialisation
    End Function                                      ' Fin de VersLigne

    ' ---------------------------------------------------------------- événements

    Private Sub btnFiltrer_Click(sender As Object, e As EventArgs) Handles btnFiltrer.Click   ' Clic sur « Filtrer »
        If dtpDebut.Value.Date > dtpFin.Value.Date Then   ' La date « Du » est après la date « Au »
            MessageBox.Show("La date de début doit précéder la date de fin.", "Alertes",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)   ' Message de validation clair
            dtpDebut.Focus()                          ' Replace le curseur sur la date de début
            Return                                    ' On n'applique pas le filtre
        End If                                        ' Fin de la validation
        Rafraichir()                                  ' Recharge la liste avec les filtres choisis
    End Sub                                           ' Fin du gestionnaire

    Private Sub dgvAlertes_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvAlertes.CellFormatting   ' Avant l'affichage de chaque cellule
        If e.RowIndex < 0 OrElse _policeGras Is Nothing Then   ' Ligne d'en-tête (indice -1) ou police pas encore créée
            Return                                    ' Rien à faire
        End If                                        ' Fin du contrôle
        Dim ligne As LigneAlerte = TryCast(dgvAlertes.Rows(e.RowIndex).DataBoundItem, LigneAlerte)   ' Retrouve l'objet affiché sur cette ligne (Nothing si impossible)
        If ligne Is Nothing Then                      ' Pas d'objet lié à cette ligne
            Return                                    ' Rien à faire
        End If                                        ' Fin du contrôle
        If Not ligne.Lue Then                         ' L'alerte n'est pas lue
            e.CellStyle.Font = _policeGras            ' Une alerte non lue s'affiche en gras
        End If                                        ' Fin du contrôle
        If dgvAlertes.Columns(e.ColumnIndex).Name = "colType" Then   ' La cellule appartient à la colonne « Type »
            If ligne.TypeTexte = TexteHausse Then     ' Hausse
                e.CellStyle.ForeColor = Color.FromArgb(122, 51, 0)    ' Texte brun-orangé
            Else                                      ' Shrinkflation
                e.CellStyle.ForeColor = Color.FromArgb(75, 42, 133)   ' Texte violet
            End If                                    ' Fin du choix de couleur
        End If                                        ' Fin du contrôle de colonne
    End Sub                                           ' Fin du gestionnaire

    Private Sub dgvAlertes_SelectionChanged(sender As Object, e As EventArgs) Handles dgvAlertes.SelectionChanged   ' Quand la sélection change dans la grille
        If dgvAlertes.SelectedRows.Count = 0 Then     ' Aucune ligne sélectionnée
            lblDetailTitre.Text = "Détail de l'alerte sélectionnée"   ' Titre par défaut
            lblDetail.Text = "Sélectionnez une alerte pour afficher son détail."   ' Invite l'utilisateur à choisir
            Return                                    ' On s'arrête là
        End If                                        ' Fin du contrôle
        Dim ligne As LigneAlerte = TryCast(dgvAlertes.SelectedRows(0).DataBoundItem, LigneAlerte)   ' Objet de la ligne sélectionnée
        If ligne Is Nothing Then                      ' Pas d'objet lié
            Return                                    ' Rien à afficher
        End If                                        ' Fin du contrôle
        lblDetailTitre.Text = $"Détail de l'alerte sélectionnée : {ligne.Produit}, {ligne.Magasin}, {ligne.DateTexte}"   ' Titre avec produit, magasin et date
        lblDetail.Text = ligne.Detail                 ' Phrase explicative produite par la détection
    End Sub                                           ' Fin du gestionnaire

    Private Sub dgvAlertes_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAlertes.CellDoubleClick   ' Double-clic sur une cellule
        If e.RowIndex >= 0 Then                       ' On a cliqué sur une vraie ligne (pas sur l'en-tête)
            MarquerSelectionCommeLue()                ' Marque la sélection comme lue
        End If                                        ' Fin du contrôle
    End Sub                                           ' Fin du gestionnaire

    Private Sub btnMarquerLue_Click(sender As Object, e As EventArgs) Handles btnMarquerLue.Click   ' Clic sur « Marquer comme lue »
        MarquerSelectionCommeLue()                    ' Marque la sélection comme lue
    End Sub                                           ' Fin du gestionnaire

    Private Sub MarquerSelectionCommeLue()            ' Marque comme lues les lignes sélectionnées
        If dgvAlertes.SelectedRows.Count = 0 Then     ' Aucune ligne sélectionnée
            MessageBox.Show("Sélectionnez au moins une alerte.", "Alertes",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)   ' Invite l'utilisateur à sélectionner
            Return                                    ' On s'arrête là
        End If                                        ' Fin du contrôle

        Try                                           ' Début du bloc surveillé
            For Each ligneGrille As DataGridViewRow In dgvAlertes.SelectedRows   ' Pour chaque ligne sélectionnée
                Dim ligne As LigneAlerte = TryCast(ligneGrille.DataBoundItem, LigneAlerte)   ' Retrouve l'objet affiché
                If ligne IsNot Nothing AndAlso Not ligne.Lue Then   ' Il existe et n'est pas déjà lu
                    _depot.MarquerCommeLue(ligne.Id)  ' Le marque comme lu en base
                End If                                ' Fin du contrôle
            Next                                      ' Fin de la boucle
        Catch ex As InvalidOperationException         ' Erreur d'accès aux données
            MessageBox.Show(ex.Message, "Alertes", MessageBoxButtons.OK, MessageBoxIcon.Error)   ' Affiche l'erreur
            Return                                    ' On s'arrête là
        End Try                                       ' Fin du bloc surveillé
        Rafraichir()                                  ' Recharge la liste (les lignes lues passent en normal ou disparaissent selon le filtre)
    End Sub                                           ' Fin de MarquerSelectionCommeLue

    Private Sub dgvAlertes_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAlertes.CellContentClick

    End Sub
End Class                                             ' Fin de la classe frmAlertes
