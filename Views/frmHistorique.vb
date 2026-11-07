' ============================================================================
' FICHIER : Views/frmHistorique.vb  (partie « code » du formulaire)
'           frmHistorique.Designer.vb contient la partie « dessin » ; les deux forment UNE classe.
' RÔLE    : EF-06, historique des prix d'un produit : tableau (Date, Magasin, Conditionnement,
'           Prix unitaire) et graphique dessiné par l'événement Paint, une courbe par magasin.
'           Présentation conforme à la maquette (onglet « Historique », vert forêt).
' RÈGLE   : le formulaire ne contient que la logique d'interface ; les calculs sont dans
'           HistoriqueService et les données arrivent par IAnalyseRepository.
' NOTION  : l'événement Paint d'un contrôle est déclenché chaque fois que Windows doit le
'           redessiner ; on y dessine avec l'objet Graphics (GDI+). Invalidate() demande un
'           nouveau dessin.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Drawing.Drawing2D   ' Pour SmoothingMode (lignes lissées)
Imports System.Linq                ' Pour Select, ToList...
Imports PreisRontgen.Data          ' Pour IAnalyseRepository
Imports PreisRontgen.Models        ' Pour ProduitRef, PointPrix
Imports PreisRontgen.Services      ' Pour HistoriqueService et FormatageAnalyse

''' <summary>EF-06 : historique d'un produit (tableau + graphique par magasin).</summary>
Public Class frmHistorique                         ' Début de la classe (Partial dans le fichier Designer)

    ''' <summary>Ligne affichée dans la grille (liaison aux colonnes par DataPropertyName).</summary>
    Friend Class LigneHistorique                   ' Une ligne déjà mise en forme
        Public Property DateTexte As String = String.Empty         ' Colonne « Date »
        Public Property Magasin As String = String.Empty           ' Colonne « Magasin »
        Public Property Conditionnement As String = String.Empty   ' Colonne « Conditionnement »
        Public Property PrixTexte As String = String.Empty         ' Colonne « Prix unitaire »
    End Class                                      ' Fin de LigneHistorique

    Private Const ChoixTous As String = "Tous"     ' Choix « pas de filtre » de la liste des magasins

    Private Shared ReadOnly CouleursCourbes As Color() = {   ' Une couleur par magasin (la première est le vert forêt)
        Color.FromArgb(30, 77, 58), Color.FromArgb(200, 90, 30), Color.FromArgb(40, 100, 170),
        Color.FromArgb(150, 50, 120), Color.FromArgb(120, 120, 30), Color.FromArgb(160, 40, 40)}

    Private ReadOnly _depot As IAnalyseRepository  ' Accès aux données (reçu à la création)
    Private _courbe As New List(Of PointPrix)()    ' Points du graphique (ancien vers récent)
    Private _chargement As Boolean                 ' Vrai pendant le remplissage des listes (évite les rechargements inutiles)

    ''' <summary>Constructeur sans paramètre, réservé au concepteur de Visual Studio.</summary>
    Public Sub New()                               ' Constructeur du concepteur
        InitializeComponent()                      ' Construit les contrôles
    End Sub                                        ' Fin du constructeur

    Public Sub New(depot As IAnalyseRepository)    ' Constructeur utilisé par l'application
        Me.New()                                   ' Construit l'écran
        If depot Is Nothing Then Throw New ArgumentNullException(NameOf(depot))   ' Refuse un dépôt absent
        _depot = depot                             ' Mémorise le dépôt
    End Sub                                        ' Fin du constructeur

    Private Sub frmHistorique_Load(sender As Object, e As EventArgs) Handles MyBase.Load   ' À l'ouverture
        If DesignMode OrElse _depot Is Nothing Then Return   ' Rien à faire dans le concepteur
        _chargement = True                         ' On remplit les listes : pas de rechargement
        Try                                        ' Début du bloc surveillé
            cmbAfficher.Items.AddRange(New Object() {"Tout l'historique", "3 derniers mois", "12 derniers mois"})   ' Périodes proposées
            cmbAfficher.SelectedIndex = 0          ' Tout l'historique par défaut

            cmbMagasin.Items.Add(New KeyValuePair(Of Integer, String)(0, ChoixTous))   ' « Tous » (identifiant 0)
            For Each m As KeyValuePair(Of Integer, String) In _depot.ListerMagasins()   ' Pour chaque magasin
                cmbMagasin.Items.Add(m)            ' L'ajoute à la liste
            Next                                   ' Fin de la boucle
            cmbMagasin.DisplayMember = "Value"     ' Affiche le nom
            cmbMagasin.ValueMember = "Key"         ' Valeur interne : l'identifiant
            cmbMagasin.SelectedIndex = 0           ' « Tous »

            cmbProduit.DisplayMember = "Nom"       ' Affiche le nom du produit
            cmbProduit.ValueMember = "Id"          ' Valeur interne : l'identifiant
            cmbProduit.DataSource = _depot.ListerProduits()   ' Remplit la liste des produits
        Catch ex As InvalidOperationException      ' Erreur d'accès aux données
            MessageBox.Show(ex.Message, "Historique", MessageBoxButtons.OK, MessageBoxIcon.Error)   ' Affiche l'erreur
        Finally                                    ' Toujours exécuté
            _chargement = False                    ' Fin du remplissage
        End Try                                    ' Fin du bloc surveillé
        Rafraichir()                               ' Affiche l'historique du premier produit
    End Sub                                        ' Fin de frmHistorique_Load

    ''' <summary>Recharge le tableau, le résumé et le graphique. Peut être appelée depuis un autre thread.</summary>
    Public Sub Rafraichir()                        ' Recharge l'écran
        If _depot Is Nothing Then Return           ' Mode conception
        If InvokeRequired Then                     ' Appelé depuis un autre thread
            Invoke(New Action(AddressOf Rafraichir))   ' Se ré-exécute sur le thread de l'écran
            Return                                 ' La version exécutée sur le bon thread fait le travail
        End If                                     ' Fin du contrôle de thread

        Dim produit As ProduitRef = TryCast(cmbProduit.SelectedItem, ProduitRef)   ' Produit choisi (Nothing si aucun)
        If produit Is Nothing Then                 ' Aucun produit connu
            dgvHistorique.DataSource = New List(Of LigneHistorique)()   ' Tableau vide
            _courbe = New List(Of PointPrix)()     ' Graphique vide
            lblResume.Text = "Aucun produit enregistré."   ' Message
            pnlGraphique.Invalidate()              ' Redessine
            Return                                 ' Rien d'autre à faire
        End If                                     ' Fin du contrôle

        Try                                        ' Début du bloc surveillé
            Dim idMagasin As Integer? = Nothing    ' Magasin choisi (Nothing = tous)
            Dim choix As KeyValuePair(Of Integer, String) = CType(cmbMagasin.SelectedItem, KeyValuePair(Of Integer, String))   ' Élément sélectionné
            If choix.Key <> 0 Then idMagasin = choix.Key   ' Filtre sur ce magasin

            Dim points As List(Of PointPrix) = _depot.ListerObservations(produit.Id, idMagasin)   ' Lit l'historique
            Dim limite As Date = Date.MinValue     ' Date minimale affichée
            If cmbAfficher.SelectedIndex = 1 Then limite = Date.Today.AddMonths(-3)        ' 3 derniers mois
            If cmbAfficher.SelectedIndex = 2 Then limite = Date.Today.AddMonths(-12)       ' 12 derniers mois
            points = points.Where(Function(p) p.DateObs.Date >= limite).ToList()           ' Applique la période

            Dim lignes As New List(Of LigneHistorique)(points.Count)   ' Lignes du tableau
            For Each p As PointPrix In HistoriqueService.Trier(points)   ' Du plus récent au plus ancien
                lignes.Add(New LigneHistorique With {
                    .DateTexte = p.DateObs.ToString("d"),                                    ' Date courte
                    .Magasin = p.NomMagasin,                                                  ' Magasin
                    .Conditionnement = FormatageAnalyse.FormaterQuantite(p.QuantiteBase, p.UniteBase),   ' Ex. « 1 L »
                    .PrixTexte = FormatageAnalyse.FormaterPrix(p.PrixUnitaire, p.UniteBase)   ' Ex. « 1 250 / L »
                })                                 ' Ajoute la ligne
            Next                                   ' Fin de la boucle
            dgvHistorique.DataSource = lignes      ' Affiche le tableau
            dgvHistorique.ClearSelection()         ' Pas de ligne sélectionnée d'office

            _courbe = HistoriqueService.PreparerCourbe(points)   ' Données du graphique
            lblResume.Text = ConstruireResume(points)            ' Résumé chiffré
        Catch ex As InvalidOperationException      ' Erreur d'accès aux données
            MessageBox.Show(ex.Message, "Historique", MessageBoxButtons.OK, MessageBoxIcon.Error)   ' Affiche l'erreur
        End Try                                    ' Fin du bloc surveillé
        pnlGraphique.Invalidate()                  ' Demande le redessin du graphique (déclenche Paint)
    End Sub                                        ' Fin de Rafraichir

    Private Shared Function ConstruireResume(points As List(Of PointPrix)) As String   ' Texte sous le graphique
        If points.Count = 0 Then Return "Aucune observation pour ce choix."   ' Rien à résumer
        Dim r As ResumeHistorique = HistoriqueService.Resumer(points)         ' Calculs
        Dim unite As UniteBase = points(0).UniteBase                          ' Unité du produit
        Dim texte As String = $"{r.Nombre} observation(s)   ·   Min {FormatageAnalyse.FormaterPrix(r.Minimum, unite)}   ·   Max {FormatageAnalyse.FormaterPrix(r.Maximum, unite)}   ·   Moyenne {FormatageAnalyse.FormaterPrix(r.Moyenne, unite)}"   ' Début du texte
        If r.VariationTotale.HasValue Then         ' Variation calculable
            texte &= "   ·   Variation " & (r.VariationTotale.Value * 100D).ToString("+0.0;-0.0;0.0") & " %"   ' Ajoute la variation
        End If                                     ' Fin du test
        Return texte                               ' Rend le texte
    End Function                                   ' Fin de ConstruireResume

    ' ---------------------------------------------------------------- événements

    Private Sub Filtre_Change(sender As Object, e As EventArgs) Handles cmbProduit.SelectedIndexChanged, cmbMagasin.SelectedIndexChanged, cmbAfficher.SelectedIndexChanged, btnActualiser.Click   ' Un filtre change ou clic sur « Actualiser »
        If _chargement Then Return                 ' Les listes sont en cours de remplissage
        Rafraichir()                               ' Recharge l'écran
    End Sub                                        ' Fin du gestionnaire

    Private Sub pnlGraphique_Paint(sender As Object, e As PaintEventArgs) Handles pnlGraphique.Paint   ' Windows demande de dessiner le graphique
        Dessiner(e.Graphics, pnlGraphique.ClientRectangle)   ' Dessine dans la zone du panneau
    End Sub                                        ' Fin du gestionnaire

    ''' <summary>Dessine le graphique : axes, quadrillage, une courbe par magasin et la légende.</summary>
    Private Sub Dessiner(g As Graphics, zoneTotale As Rectangle)   ' Dessin GDI+
        g.SmoothingMode = SmoothingMode.AntiAlias  ' Lignes lissées
        g.Clear(Color.White)                       ' Fond blanc

        Dim texteFonce As Color = Color.FromArgb(23, 38, 31)   ' Couleur du texte
        Dim zone As New Rectangle(zoneTotale.Left + 70, zoneTotale.Top + 48, zoneTotale.Width - 90, zoneTotale.Height - 92)   ' Zone de tracé (marges pour les axes)
        If zone.Width < 60 OrElse zone.Height < 60 Then Return   ' Trop petit pour dessiner

        Using police As New Font("Segoe UI", 8.0F),
              policeTitre As New Font("Segoe UI", 9.0F, FontStyle.Bold),
              pinceauTexte As New SolidBrush(texteFonce),
              stylo As New Pen(Color.FromArgb(195, 208, 200)),
              styloAxe As New Pen(Color.FromArgb(90, 106, 98), 1.5F)

            g.DrawString("Prix unitaire dans le temps (FCFA)", policeTitre, pinceauTexte, 12.0F, 8.0F)   ' Titre

            If _courbe.Count = 0 Then              ' Aucune donnée
                g.DrawString("Aucune observation pour ce choix.", police, pinceauTexte, zone.Left, zone.Top + 20)   ' Message
                Return                             ' Fin du dessin
            End If                                 ' Fin du contrôle

            Dim prix As List(Of Decimal) = _courbe.Select(Function(p) p.PrixUnitaire).ToList()   ' Prix des points
            Dim dates As List(Of Date) = _courbe.Select(Function(p) p.DateObs).ToList()          ' Dates des points
            Dim xs As List(Of Double) = HistoriqueService.PositionsX(dates)   ' Positions horizontales (0 à 1)
            Dim ys As List(Of Double) = HistoriqueService.PositionsY(prix)    ' Positions verticales (0 à 1)
            Dim minimum As Decimal = prix.Min()    ' Prix le plus bas
            Dim maximum As Decimal = prix.Max()    ' Prix le plus haut

            ' --- quadrillage horizontal et graduations du prix
            For i As Integer = 0 To 4              ' Cinq lignes
                Dim y As Single = CSng(zone.Bottom - (i / 4.0R) * zone.Height)   ' Hauteur de la ligne
                g.DrawLine(stylo, zone.Left, y, zone.Right, y)   ' Trait du quadrillage
                If maximum <> minimum OrElse i = 2 Then   ' Courbe plate : une seule graduation, au milieu
                    Dim valeur As Decimal = If(maximum = minimum, minimum, minimum + (maximum - minimum) * i / 4D)   ' Prix de cette ligne
                    g.DrawString(FormatageAnalyse.FormaterMontant(valeur), police, pinceauTexte, 6.0F, y - 8.0F)   ' Graduation à gauche
                End If                             ' Fin du test
            Next                                   ' Fin de la boucle

            ' --- axes
            g.DrawLine(styloAxe, zone.Left, zone.Top, zone.Left, zone.Bottom)         ' Axe vertical
            g.DrawLine(styloAxe, zone.Left, zone.Bottom, zone.Right, zone.Bottom)     ' Axe horizontal

            ' --- dates aux extrémités
            Dim premiere As String = dates.Min().ToString("d")   ' Date la plus ancienne
            Dim derniere As String = dates.Max().ToString("d")   ' Date la plus récente
            g.DrawString(premiere, police, pinceauTexte, zone.Left - 20.0F, zone.Bottom + 6.0F)   ' À gauche
            If derniere <> premiere Then           ' Deux dates différentes
                g.DrawString(derniere, police, pinceauTexte, zone.Right - 55.0F, zone.Bottom + 6.0F)   ' À droite
            End If                                 ' Fin du test

            ' --- une courbe par magasin
            Dim idsMagasins As List(Of Integer) = _courbe.Select(Function(p) p.IdMagasin).Distinct().ToList()   ' Magasins présents
            Dim xLegende As Single = 12.0F         ' Position de la légende
            For k As Integer = 0 To idsMagasins.Count - 1   ' Pour chaque magasin
                Dim idMag As Integer = idsMagasins(k)       ' Magasin traité
                Dim couleur As Color = CouleursCourbes(k Mod CouleursCourbes.Length)   ' Sa couleur
                Dim pointsEcran As New List(Of PointF)()    ' Positions en pixels
                Dim nomMagasin As String = String.Empty     ' Nom pour la légende
                For i As Integer = 0 To _courbe.Count - 1   ' Parcourt les points
                    If _courbe(i).IdMagasin = idMag Then    ' Point de ce magasin
                        nomMagasin = _courbe(i).NomMagasin  ' Mémorise le nom
                        pointsEcran.Add(New PointF(CSng(zone.Left + xs(i) * zone.Width), CSng(zone.Bottom - ys(i) * zone.Height)))   ' Convertit 0-1 en pixels
                    End If                                  ' Fin du test
                Next                                        ' Fin de la boucle
                Using stylet As New Pen(couleur, 2.0F), pinceau As New SolidBrush(couleur)   ' Outils de dessin
                    If pointsEcran.Count >= 2 Then g.DrawLines(stylet, pointsEcran.ToArray())   ' Relie les points
                    If pointsEcran.Count <= 60 Then         ' Peu de points : on les marque
                        For Each pe As PointF In pointsEcran
                            g.FillEllipse(pinceau, pe.X - 3.5F, pe.Y - 3.5F, 7.0F, 7.0F)   ' Petit disque
                        Next                                ' Fin de la boucle
                    End If                                  ' Fin du test
                    If idsMagasins.Count > 1 Then           ' Plusieurs magasins : légende
                        g.FillRectangle(pinceau, xLegende, 29.0F, 10.0F, 10.0F)   ' Carré de couleur
                        g.DrawString(nomMagasin, police, pinceauTexte, xLegende + 14.0F, 26.0F)   ' Nom du magasin
                        xLegende += 24.0F + g.MeasureString(nomMagasin, police).Width   ' Décale la légende suivante
                    End If                                  ' Fin du test
                End Using                                   ' Libère le stylo et le pinceau
            Next                                            ' Fin de la boucle
        End Using                                           ' Libère les outils
    End Sub                                                 ' Fin de Dessiner

End Class                                                   ' Fin de frmHistorique
