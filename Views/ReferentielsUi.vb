Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Globalization
Imports System.Linq
Imports System.Windows.Forms

Public NotInheritable Partial Class MainForm
    Private ReadOnly ongletsReferentiels As New TabControl()
    Private ReadOnly lstMagasins As New ListBox()
    Private ReadOnly txtNomMagasin As New TextBox()
    Private ReadOnly lstCategories As New ListBox()
    Private ReadOnly txtNomCategorie As New TextBox()
    Private ReadOnly lstProduits As New ListBox()
    Private ReadOnly txtNomProduit As New TextBox()
    Private ReadOnly txtMarqueProduit As New TextBox()
    Private ReadOnly txtCodeBarresProduit As New TextBox()
    Private ReadOnly btnRechercherCatalogue As Button = CreerBouton("Rechercher Open Food Facts", False)
    Private ReadOnly lblResultatCatalogue As New Label()
    Private ReadOnly cmbCategorieProduit As New ComboBox()
    Private ReadOnly cmbUniteBaseProduit As New ComboBox()
    Private ReadOnly txtSeuilHausse As New TextBox()
    Private ReadOnly txtSeuilShrink As New TextBox()
    Private ReadOnly dgvUtilisateurs As New DataGridView()
    Private ReadOnly cmbRoleUtilisateur As New ComboBox()
    Private ReadOnly chkActifUtilisateur As New CheckBox()
    Private ReadOnly pnlAdministration As New Panel()
    Private _magasins As New List(Of Magasin)()
    Private _categories As New List(Of Categorie)()
    Private _produits As New List(Of Produit)()

    Private Sub ConstruireOngletReferentiels()
        Dim page As New TabPage("Référentiels") With {.BackColor = ThemePricieux.FondPage, .Padding = New Padding(16)}
        _onglets.TabPages.Add(page)
        page.Controls.Add(New Label With {.Text = "Magasins, catégories et produits", .Font = New Font("Segoe UI", 17, FontStyle.Bold), .AutoSize = True, .Location = New Point(18, 16), .ForeColor = ThemePricieux.Foret})
        pnlAdministration.Name = "pnlAdministration"
        pnlAdministration.Location = New Point(18, 62)
        pnlAdministration.Size = New Size(1195, 625)
        pnlAdministration.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        page.Controls.Add(pnlAdministration)
        ongletsReferentiels.Dock = DockStyle.Fill
        ongletsReferentiels.DrawMode = TabDrawMode.OwnerDrawFixed
        ongletsReferentiels.ItemSize = New Size(125, 28)
        AddHandler ongletsReferentiels.DrawItem, AddressOf ThemePricieux.DessinerOnglet
        pnlAdministration.Controls.Add(ongletsReferentiels)
        ConstruireGestionMagasins()
        ConstruireGestionCategories()
        ConstruireGestionProduits()
        ConstruireParametres()
        ConstruireGestionUtilisateurs()
        If Not _utilisateur.EstAdministrateur Then
            pnlAdministration.Enabled = False
            page.Controls.Add(New Label With {.Text = "La gestion des référentiels et seuils est réservée au compte administrateur.", .AutoSize = True, .Location = New Point(32, 80), .Font = New Font("Segoe UI", 11), .ForeColor = Color.DimGray})
        End If
    End Sub

    Private Sub ConstruireGestionMagasins()
        Dim page As New TabPage("Magasins")
        ongletsReferentiels.TabPages.Add(page)
        lstMagasins.Name = "lstMagasins"
        lstMagasins.Location = New Point(18, 18)
        lstMagasins.Size = New Size(510, 520)
        lstMagasins.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        page.Controls.Add(lstMagasins)
        AddHandler lstMagasins.SelectedIndexChanged, AddressOf MagasinSelectionne
        page.Controls.Add(New Label With {.Text = "Nom du magasin", .Location = New Point(560, 20), .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold)})
        txtNomMagasin.Name = "txtNomMagasin"
        txtNomMagasin.Location = New Point(560, 50)
        txtNomMagasin.Size = New Size(440, 30)
        page.Controls.Add(txtNomMagasin)
        Dim btnEnregistrer As Button = CreerBouton("Enregistrer", True)
        btnEnregistrer.Location = New Point(560, 103)
        AddHandler btnEnregistrer.Click, AddressOf EnregistrerMagasinAsync
        page.Controls.Add(btnEnregistrer)
        Dim btnNouveau As Button = CreerBouton("Nouveau", False)
        btnNouveau.Location = New Point(690, 103)
        AddHandler btnNouveau.Click, AddressOf NouveauMagasin
        page.Controls.Add(btnNouveau)
        Dim btnSupprimer As Button = CreerBouton("Désactiver", False)
        btnSupprimer.Location = New Point(820, 103)
        AddHandler btnSupprimer.Click, AddressOf SupprimerMagasinAsync
        page.Controls.Add(btnSupprimer)
        page.Controls.Add(New Label With {.Text = "Un magasin déjà utilisé est désactivé afin de préserver l'historique.", .Location = New Point(560, 165), .Size = New Size(480, 60), .ForeColor = Color.DimGray})
    End Sub

    Private Sub ConstruireGestionCategories()
        Dim page As New TabPage("Catégories")
        ongletsReferentiels.TabPages.Add(page)
        lstCategories.Name = "lstCategories"
        lstCategories.Location = New Point(18, 18)
        lstCategories.Size = New Size(510, 520)
        lstCategories.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        page.Controls.Add(lstCategories)
        AddHandler lstCategories.SelectedIndexChanged, AddressOf CategorieSelectionnee
        page.Controls.Add(New Label With {.Text = "Nom de la catégorie", .Location = New Point(560, 20), .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold)})
        txtNomCategorie.Name = "txtNomCategorie"
        txtNomCategorie.Location = New Point(560, 50)
        txtNomCategorie.Size = New Size(440, 30)
        page.Controls.Add(txtNomCategorie)
        Dim btnEnregistrer As Button = CreerBouton("Enregistrer", True)
        btnEnregistrer.Location = New Point(560, 103)
        AddHandler btnEnregistrer.Click, AddressOf EnregistrerCategorieAsync
        page.Controls.Add(btnEnregistrer)
        Dim btnNouveau As Button = CreerBouton("Nouveau", False)
        btnNouveau.Location = New Point(690, 103)
        AddHandler btnNouveau.Click, AddressOf NouvelleCategorie
        page.Controls.Add(btnNouveau)
        Dim btnSupprimer As Button = CreerBouton("Désactiver", False)
        btnSupprimer.Location = New Point(820, 103)
        AddHandler btnSupprimer.Click, AddressOf SupprimerCategorieAsync
        page.Controls.Add(btnSupprimer)
    End Sub

    Private Sub ConstruireGestionProduits()
        Dim page As New TabPage("Produits")
        ongletsReferentiels.TabPages.Add(page)
        lstProduits.Name = "lstProduits"
        lstProduits.Location = New Point(18, 18)
        lstProduits.Size = New Size(450, 520)
        lstProduits.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        page.Controls.Add(lstProduits)
        AddHandler lstProduits.SelectedIndexChanged, AddressOf ProduitReferenceSelectionne
        Dim pnlChamps As New Panel With {.Location = New Point(490, 18), .Size = New Size(630, 530), .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right}
        page.Controls.Add(pnlChamps)
        AjouterControle(pnlChamps, "Nom du produit", txtNomProduit, 10, 15, 570)
        AjouterControle(pnlChamps, "Marque", txtMarqueProduit, 10, 84, 570)
        AjouterControle(pnlChamps, "Code-barres (facultatif)", txtCodeBarresProduit, 10, 153, 570)
        AddHandler txtNomProduit.Validating, AddressOf ValiderChampProduit
        AddHandler txtMarqueProduit.Validating, AddressOf ValiderChampProduit
        AddHandler txtCodeBarresProduit.Validating, AddressOf ValiderChampProduit
        btnRechercherCatalogue.Name = "btnRechercherOpenFoodFacts"
        btnRechercherCatalogue.Location = New Point(10, 213)
        btnRechercherCatalogue.Size = New Size(235, 36)
        AddHandler btnRechercherCatalogue.Click, AddressOf RechercherProduitOpenFoodFactsAsync
        pnlChamps.Controls.Add(btnRechercherCatalogue)
        pnlChamps.Controls.Add(New Label With {.Text = "Catégorie", .Location = New Point(10, 260), .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold)})
        cmbCategorieProduit.Name = "cmbCategorieProduit"
        cmbCategorieProduit.DropDownStyle = ComboBoxStyle.DropDownList
        cmbCategorieProduit.Location = New Point(10, 287)
        cmbCategorieProduit.Size = New Size(270, 30)
        pnlChamps.Controls.Add(cmbCategorieProduit)
        pnlChamps.Controls.Add(New Label With {.Text = "Unité de base", .Location = New Point(312, 260), .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold)})
        cmbUniteBaseProduit.Name = "cmbUniteBaseProduit"
        cmbUniteBaseProduit.DropDownStyle = ComboBoxStyle.DropDownList
        cmbUniteBaseProduit.Items.AddRange(New Object() {"kg", "L", "pièce"})
        cmbUniteBaseProduit.Location = New Point(312, 287)
        cmbUniteBaseProduit.Size = New Size(150, 30)
        pnlChamps.Controls.Add(cmbUniteBaseProduit)
        Dim btnEnregistrer As Button = CreerBouton("Enregistrer le produit", True)
        btnEnregistrer.Location = New Point(10, 342)
        AddHandler btnEnregistrer.Click, AddressOf EnregistrerProduitAsync
        pnlChamps.Controls.Add(btnEnregistrer)
        Dim btnNouveau As Button = CreerBouton("Nouveau", False)
        btnNouveau.Location = New Point(210, 342)
        AddHandler btnNouveau.Click, Sub(sender As Object, args As EventArgs) ViderProduit()
        pnlChamps.Controls.Add(btnNouveau)
        Dim btnSupprimer As Button = CreerBouton("Désactiver", False)
        btnSupprimer.Location = New Point(340, 342)
        AddHandler btnSupprimer.Click, AddressOf SupprimerProduitAsync
        pnlChamps.Controls.Add(btnSupprimer)
        lblResultatCatalogue.Name = "lblResultatOpenFoodFacts"
        lblResultatCatalogue.Location = New Point(10, 390)
        lblResultatCatalogue.Size = New Size(570, 42)
        lblResultatCatalogue.ForeColor = ThemePricieux.Action
        pnlChamps.Controls.Add(lblResultatCatalogue)
        pnlChamps.Controls.Add(New Label With {.Text = "La recherche est facultative et envoie uniquement le code-barres après confirmation. Vérifiez les champs importés ; la catégorie et l'unité restent à confirmer.", .Location = New Point(10, 438), .Size = New Size(570, 55), .ForeColor = Color.DimGray})
    End Sub

    Private Sub ConstruireParametres()
        Dim page As New TabPage("Seuils")
        ongletsReferentiels.TabPages.Add(page)
        page.Controls.Add(New Label With {.Text = "Seuil de hausse du prix unitaire (%)", .Location = New Point(26, 28), .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold)})
        txtSeuilHausse.Name = "txtSeuilHausse"
        txtSeuilHausse.Location = New Point(30, 59)
        txtSeuilHausse.Size = New Size(130, 30)
        txtSeuilHausse.Text = "5"
        page.Controls.Add(txtSeuilHausse)
        page.Controls.Add(New Label With {.Text = "Baisse minimale du conditionnement (%)", .Location = New Point(210, 28), .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold)})
        txtSeuilShrink.Name = "txtSeuilShrink"
        txtSeuilShrink.Location = New Point(214, 59)
        txtSeuilShrink.Size = New Size(130, 30)
        txtSeuilShrink.Text = "3"
        page.Controls.Add(txtSeuilShrink)
        Dim btnSauver As Button = CreerBouton("Enregistrer les seuils", True)
        btnSauver.Location = New Point(30, 118)
        AddHandler btnSauver.Click, AddressOf EnregistrerSeuilsAsync
        page.Controls.Add(btnSauver)
        page.Controls.Add(New Label With {.Text = "Les règles sont appliquées à chaque nouvelle observation du même produit et magasin.", .Location = New Point(30, 177), .Size = New Size(600, 50), .ForeColor = Color.DimGray})
    End Sub

    Private Sub ConstruireGestionUtilisateurs()
        Dim page As New TabPage("Utilisateurs")
        ongletsReferentiels.TabPages.Add(page)
        ConfigurerGrille(dgvUtilisateurs)
        dgvUtilisateurs.Name = "dgvUtilisateurs"
        dgvUtilisateurs.Location = New Point(18, 18)
        dgvUtilisateurs.Size = New Size(700, 520)
        dgvUtilisateurs.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        page.Controls.Add(dgvUtilisateurs)
        dgvUtilisateurs.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "id", .HeaderText = "Id", .Visible = False})
        dgvUtilisateurs.Columns.Add("login", "Identifiant")
        dgvUtilisateurs.Columns.Add("role", "Rôle")
        dgvUtilisateurs.Columns.Add("actif", "Statut")
        dgvUtilisateurs.Columns("login").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvUtilisateurs.Columns("role").Width = 150
        dgvUtilisateurs.Columns("actif").Width = 130
        AddHandler dgvUtilisateurs.SelectionChanged, AddressOf UtilisateurSelectionne
        Dim panneau As New Panel With {.Location = New Point(750, 18), .Size = New Size(380, 520), .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right}
        page.Controls.Add(panneau)
        panneau.Controls.Add(New Label With {.Text = "Rôle", .Location = New Point(10, 15), .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold)})
        cmbRoleUtilisateur.Name = "cmbRoleUtilisateur"
        cmbRoleUtilisateur.DropDownStyle = ComboBoxStyle.DropDownList
        cmbRoleUtilisateur.Items.AddRange(New Object() {"ADMIN", "UTILISATEUR"})
        cmbRoleUtilisateur.Location = New Point(10, 42)
        cmbRoleUtilisateur.Size = New Size(220, 30)
        panneau.Controls.Add(cmbRoleUtilisateur)
        chkActifUtilisateur.Name = "chkActifUtilisateur"
        chkActifUtilisateur.Text = "Compte actif"
        chkActifUtilisateur.Location = New Point(10, 96)
        chkActifUtilisateur.AutoSize = True
        panneau.Controls.Add(chkActifUtilisateur)
        Dim btnEnregistrer As Button = CreerBouton("Enregistrer", True)
        btnEnregistrer.Location = New Point(10, 146)
        AddHandler btnEnregistrer.Click, AddressOf EnregistrerUtilisateurAsync
        panneau.Controls.Add(btnEnregistrer)
        Dim btnActualiser As Button = CreerBouton("Actualiser", False)
        btnActualiser.Location = New Point(145, 146)
        AddHandler btnActualiser.Click, AddressOf ChargerUtilisateursAsync
        panneau.Controls.Add(btnActualiser)
        Dim btnSupprimerUtilisateur As Button = CreerBouton("Supprimer définitivement", False)
        btnSupprimerUtilisateur.Name = "btnSupprimerUtilisateur"
        btnSupprimerUtilisateur.Location = New Point(10, 190)
        btnSupprimerUtilisateur.Size = New Size(190, 36)
        btnSupprimerUtilisateur.ForeColor = Color.Firebrick
        btnSupprimerUtilisateur.FlatAppearance.BorderColor = Color.Firebrick
        AddHandler btnSupprimerUtilisateur.Click, AddressOf SupprimerUtilisateurAsync
        panneau.Controls.Add(btnSupprimerUtilisateur)
        panneau.Controls.Add(New Label With {.Text = "Votre compte et le dernier administrateur actif sont protégés. Un compte lié à des tickets, alertes ou dépenses doit être désactivé pour conserver son historique.", .Location = New Point(10, 240), .Size = New Size(340, 115), .ForeColor = Color.DimGray})
    End Sub

    Private Shared Sub AjouterControle(parent As Control, titre As String, champ As TextBox, x As Integer, y As Integer, largeur As Integer)
        parent.Controls.Add(New Label With {.Text = titre, .Location = New Point(x, y), .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold)})
        Select Case titre
            Case "Nom du produit"
                champ.Name = "txtNomProduit"
            Case "Marque"
                champ.Name = "txtMarqueProduit"
            Case "Code-barres (facultatif)"
                champ.Name = "txtCodeBarresProduit"
            Case "Nom de la dépense"
                champ.Name = "txtLibelleRecurrent"
            Case "Montant (FCFA)"
                champ.Name = "txtMontantRecurrent"
        End Select
        champ.Location = New Point(x, y + 27)
        champ.Size = New Size(largeur, 30)
        parent.Controls.Add(champ)
    End Sub

    Private Sub MettreAJourDonneesReferentiel(magasins As List(Of Magasin), categories As List(Of Categorie), produits As List(Of Produit))
        _magasins = magasins
        _categories = categories
        _produits = produits
        _produitsTicket = New List(Of Produit)(produits)

        cmbMagasinTicket.DataSource = Nothing
        cmbMagasinTicket.DataSource = New List(Of Magasin)(magasins)
        cmbMagasinTicket.DisplayMember = "Nom"
        cmbMagasinTicket.SelectedIndex = If(magasins.Count > 0, 0, -1)

        cmbMagasinHistorique.Items.Clear()
        cmbMagasinHistorique.Items.Add("Tous les magasins")
        For Each magasin As Magasin In magasins
            cmbMagasinHistorique.Items.Add(magasin)
        Next
        cmbMagasinHistorique.SelectedIndex = 0
        cmbMagasinAlerte.Items.Clear()
        cmbMagasinAlerte.Items.Add("Tous les magasins")
        For Each magasin As Magasin In magasins
            cmbMagasinAlerte.Items.Add(magasin)
        Next
        cmbMagasinAlerte.SelectedIndex = 0

        ChargerComboProduits(cmbProduitHistorique, produits)
        ChargerComboProduits(cmbProduitComparaison, produits)
        cmbCategorieComparaison.DataSource = Nothing
        cmbCategorieComparaison.DataSource = New List(Of Categorie)(categories)
        cmbCategorieComparaison.DisplayMember = "Nom"
        cmbCategorieComparaison.SelectedIndex = If(categories.Count > 0, 0, -1)
        cmbCategorieProduit.DataSource = Nothing
        cmbCategorieProduit.DataSource = New List(Of Categorie)(categories)
        cmbCategorieProduit.DisplayMember = "Nom"
        cmbCategorieProduit.SelectedIndex = If(categories.Count > 0, 0, -1)

        cmbProduitExport.DataSource = Nothing
        cmbProduitExport.DataSource = New List(Of Produit)(produits)
        cmbProduitExport.DisplayMember = "Libelle"
        cmbProduitExport.SelectedIndex = If(produits.Count > 0, 0, -1)
        cmbMagasinExport.Items.Clear()
        cmbMagasinExport.Items.Add("Tous les magasins")
        For Each magasin As Magasin In magasins
            cmbMagasinExport.Items.Add(magasin)
        Next
        cmbMagasinExport.SelectedIndex = 0

        cmbProduitTicket.DataSource = Nothing
        cmbProduitTicket.DataSource = New List(Of Produit)(produits)
        cmbProduitTicket.DisplayMember = "Libelle"
        cmbProduitTicket.SelectedIndex = -1
        cmbProduitTicket.AutoCompleteCustomSource = New AutoCompleteStringCollection()
        For Each produit As Produit In produits
            cmbProduitTicket.AutoCompleteCustomSource.Add(produit.Libelle)
        Next
        If dgvLignesTicket.Columns.Contains("produitId") Then
            DirectCast(dgvLignesTicket.Columns("produitId"), DataGridViewComboBoxColumn).DataSource = New List(Of Produit)(produits)
        End If

        lstMagasins.DataSource = Nothing
        lstMagasins.DataSource = New List(Of Magasin)(magasins)
        lstCategories.DataSource = Nothing
        lstCategories.DataSource = New List(Of Categorie)(categories)
        lstProduits.DataSource = Nothing
        lstProduits.DataSource = New List(Of Produit)(produits)
        cmbCategorieProduit.DataSource = Nothing
        cmbCategorieProduit.DataSource = New List(Of Categorie)(categories)
        cmbCategorieProduit.DisplayMember = "Nom"
        cmbCategorieProduit.SelectedIndex = If(categories.Count > 0, 0, -1)
    End Sub

    Private Shared Sub ChargerComboProduits(combo As ComboBox, produits As List(Of Produit))
        combo.DataSource = Nothing
        combo.DataSource = New List(Of Produit)(produits)
        combo.DisplayMember = "Libelle"
        combo.SelectedIndex = If(produits.Count > 0, 0, -1)
    End Sub

    Private Sub MagasinSelectionne(sender As Object, e As EventArgs)
        Dim magasin As Magasin = TryCast(lstMagasins.SelectedItem, Magasin)
        If magasin IsNot Nothing Then txtNomMagasin.Text = magasin.Nom
    End Sub

    Private Sub NouveauMagasin(sender As Object, e As EventArgs)
        lstMagasins.ClearSelected()
        txtNomMagasin.Clear()
        txtNomMagasin.Focus()
    End Sub

    Private Sub CategorieSelectionnee(sender As Object, e As EventArgs)
        Dim categorie As Categorie = TryCast(lstCategories.SelectedItem, Categorie)
        If categorie IsNot Nothing Then txtNomCategorie.Text = categorie.Nom
    End Sub

    Private Sub NouvelleCategorie(sender As Object, e As EventArgs)
        lstCategories.ClearSelected()
        txtNomCategorie.Clear()
        txtNomCategorie.Focus()
    End Sub

    Private Sub ProduitReferenceSelectionne(sender As Object, e As EventArgs)
        Dim produit As Produit = TryCast(lstProduits.SelectedItem, Produit)
        If produit Is Nothing Then Return
        lblResultatCatalogue.Text = String.Empty
        txtNomProduit.Text = produit.Nom
        txtMarqueProduit.Text = produit.Marque
        txtCodeBarresProduit.Text = produit.CodeBarres
        For Each categorie As Categorie In cmbCategorieProduit.Items
            If categorie.Id = produit.CategorieId Then cmbCategorieProduit.SelectedItem = categorie
        Next
        cmbUniteBaseProduit.SelectedItem = UniteProduitVersTexte(produit.UniteBase)
    End Sub

    Private Async Sub RechercherProduitOpenFoodFactsAsync(sender As Object, e As EventArgs)
        Dim codeBarres As String = txtCodeBarresProduit.Text.Trim()
        If String.IsNullOrWhiteSpace(codeBarres) Then
            MessageBox.Show("Saisissez d'abord le code-barres du produit.", "Recherche Open Food Facts", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim consentement As DialogResult = MessageBox.Show(
            "Cette recherche transmettra le code-barres et les informations réseau habituelles (notamment l'adresse IP) à Open Food Facts. Aucun ticket, prix payé ou compte utilisateur ne sera envoyé. Continuer ?",
            "Recherche externe facultative", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If consentement <> DialogResult.Yes Then Return

        Try
            btnRechercherCatalogue.Enabled = False
            lblResultatCatalogue.Text = "Recherche en cours…"
            Dim resultat As ProduitCatalogueExterne = Await New OpenFoodFactsService().RechercherParCodeBarresAsync(codeBarres)
            If resultat Is Nothing Then
                lblResultatCatalogue.Text = "Aucune fiche trouvée pour ce code-barres."
                Return
            End If

            If Not String.IsNullOrWhiteSpace(resultat.Nom) Then txtNomProduit.Text = resultat.Nom
            If Not String.IsNullOrWhiteSpace(resultat.Marque) Then txtMarqueProduit.Text = resultat.Marque
            Dim categorieCorrespondante As Categorie = _categories.Find(Function(categorie As Categorie) resultat.Categories.Any(Function(nom As String) String.Equals(categorie.Nom.Trim(), nom.Trim(), StringComparison.OrdinalIgnoreCase)))
            If categorieCorrespondante IsNot Nothing Then cmbCategorieProduit.SelectedItem = categorieCorrespondante

            Dim details As New List(Of String)()
            If Not String.IsNullOrWhiteSpace(resultat.QuantiteIndiquee) Then details.Add("quantité catalogue : " & resultat.QuantiteIndiquee)
            If resultat.Categories.Count > 0 Then details.Add("catégories : " & String.Join(", ", resultat.Categories.Take(3)))
            lblResultatCatalogue.Text = "Fiche Open Food Facts collaborative — contrôlez les champs." & If(details.Count > 0, " " & String.Join(" · ", details), String.Empty)
        Catch ex As Exception
            lblResultatCatalogue.Text = "Recherche indisponible ; vous pouvez saisir le produit manuellement."
            AfficherErreur("Recherche Open Food Facts impossible", ex)
        Finally
            btnRechercherCatalogue.Enabled = True
        End Try
    End Sub

    Private Sub ValiderChampProduit(sender As Object, e As System.ComponentModel.CancelEventArgs)
        Dim champ As TextBox = DirectCast(sender, TextBox)
        Dim message As String = String.Empty
        If champ Is txtNomProduit AndAlso String.IsNullOrWhiteSpace(champ.Text) Then message = "Le nom du produit est obligatoire."
        Dim longueurMax As Integer = If(champ Is txtMarqueProduit, 120, If(champ Is txtCodeBarresProduit, 64, 180))
        If champ.Text.Length > longueurMax Then message = "La valeur saisie est trop longue."
        If champ Is txtCodeBarresProduit AndAlso champ.Text.Length > 0 AndAlso Not champ.Text.All(Function(caractere As Char) Char.IsDigit(caractere)) Then message = "Le code-barres doit contenir uniquement des chiffres."
        e.Cancel = message.Length > 0
        champ.BackColor = If(e.Cancel, Color.MistyRose, Color.White)
        If e.Cancel Then MessageBox.Show(message, "Validation du produit", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Async Sub EnregistrerMagasinAsync(sender As Object, e As EventArgs)
        Try
            Dim selection As Magasin = TryCast(lstMagasins.SelectedItem, Magasin)
            Await _referentiels.EnregistrerMagasinAsync(If(selection Is Nothing, CType(Nothing, Long?), selection.Id), txtNomMagasin.Text)
            Await RechargerReferentielsAsync()
        Catch ex As Exception
            AfficherErreur("Enregistrement du magasin impossible", ex)
        End Try
    End Sub

    Private Async Sub SupprimerMagasinAsync(sender As Object, e As EventArgs)
        Try
            Dim selection As Magasin = TryCast(lstMagasins.SelectedItem, Magasin)
            If selection Is Nothing Then Throw New InvalidOperationException("Sélectionnez un magasin.")
            If MessageBox.Show("Désactiver le magasin « " & selection.Nom & " » ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
            Await _referentiels.SupprimerMagasinAsync(selection.Id)
            Await RechargerReferentielsAsync()
        Catch ex As Exception
            AfficherErreur("Désactivation impossible", ex)
        End Try
    End Sub

    Private Async Sub EnregistrerCategorieAsync(sender As Object, e As EventArgs)
        Try
            Dim selection As Categorie = TryCast(lstCategories.SelectedItem, Categorie)
            Await _referentiels.EnregistrerCategorieAsync(If(selection Is Nothing, CType(Nothing, Long?), selection.Id), txtNomCategorie.Text)
            Await RechargerReferentielsAsync()
        Catch ex As Exception
            AfficherErreur("Enregistrement de la catégorie impossible", ex)
        End Try
    End Sub

    Private Async Sub SupprimerCategorieAsync(sender As Object, e As EventArgs)
        Try
            Dim selection As Categorie = TryCast(lstCategories.SelectedItem, Categorie)
            If selection Is Nothing Then Throw New InvalidOperationException("Sélectionnez une catégorie.")
            If MessageBox.Show("Désactiver la catégorie « " & selection.Nom & " » ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
            Await _referentiels.SupprimerCategorieAsync(selection.Id)
            Await RechargerReferentielsAsync()
        Catch ex As Exception
            AfficherErreur("Désactivation impossible", ex)
        End Try
    End Sub

    Private Async Sub EnregistrerProduitAsync(sender As Object, e As EventArgs)
        Try
            Dim selection As Produit = TryCast(lstProduits.SelectedItem, Produit)
            Dim categorie As Categorie = TryCast(cmbCategorieProduit.SelectedItem, Categorie)
            If categorie Is Nothing Then Throw New InvalidOperationException("Créez ou sélectionnez une catégorie.")
            Dim unite As UniteProduit
            Select Case Convert.ToString(cmbUniteBaseProduit.SelectedItem, CultureInfo.CurrentCulture)
                Case "kg"
                    unite = UniteProduit.Kilogramme
                Case "L"
                    unite = UniteProduit.Litre
                Case "pièce"
                    unite = UniteProduit.Piece
                Case Else
                    Throw New InvalidOperationException("Sélectionnez une unité de base.")
            End Select
            Dim produit As New Produit With {
                .Id = If(selection Is Nothing, 0L, selection.Id), .Nom = txtNomProduit.Text, .Marque = txtMarqueProduit.Text,
                .CategorieId = categorie.Id, .UniteBase = unite, .CodeBarres = txtCodeBarresProduit.Text
            }
            Await _referentiels.EnregistrerProduitAsync(produit)
            Await RechargerReferentielsAsync()
        Catch ex As Exception
            AfficherErreur("Enregistrement du produit impossible", ex)
        End Try
    End Sub

    Private Async Sub SupprimerProduitAsync(sender As Object, e As EventArgs)
        Try
            Dim selection As Produit = TryCast(lstProduits.SelectedItem, Produit)
            If selection Is Nothing Then Throw New InvalidOperationException("Sélectionnez un produit.")
            If MessageBox.Show("Désactiver le produit « " & selection.Libelle & " » ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
            Await _referentiels.SupprimerProduitAsync(selection.Id)
            Await RechargerReferentielsAsync()
            ViderProduit()
        Catch ex As Exception
            AfficherErreur("Désactivation impossible", ex)
        End Try
    End Sub

    Private Async Sub EnregistrerSeuilsAsync(sender As Object, e As EventArgs)
        Try
            Dim hausse As Decimal = LireDecimal(txtSeuilHausse.Text, "seuil de hausse")
            Dim shrink As Decimal = LireDecimal(txtSeuilShrink.Text, "seuil de shrinkflation")
            Await _depensesDepot.EnregistrerSeuilsAsync(hausse, shrink)
            MessageBox.Show("Seuils enregistrés.", "Pricieux", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            AfficherErreur("Enregistrement des seuils impossible", ex)
        End Try
    End Sub

    Private Async Function RechargerReferentielsAsync() As System.Threading.Tasks.Task
        Dim magasins = Await _referentiels.ListerMagasinsAsync()
        Dim categories = Await _referentiels.ListerCategoriesAsync()
        Dim produits = Await _referentiels.ListerProduitsAsync()
        MettreAJourDonneesReferentiel(magasins, categories, produits)
    End Function

    Private Async Function ChargerAdministrationAsync() As System.Threading.Tasks.Task
        If Not _utilisateur.EstAdministrateur Then Return
        Dim seuils = Await _depensesDepot.LireSeuilsAsync()
        txtSeuilHausse.Text = seuils.Hausse.ToString(CultureInfo.InvariantCulture)
        txtSeuilShrink.Text = seuils.Shrinkflation.ToString(CultureInfo.InvariantCulture)
        Await ChargerUtilisateursAsyncTask()
    End Function

    Private Async Sub ChargerUtilisateursAsync(sender As Object, e As EventArgs)
        Try
            Await ChargerUtilisateursAsyncTask()
        Catch ex As Exception
            AfficherErreur("Liste des utilisateurs indisponible", ex)
        End Try
    End Sub

    Private Async Function ChargerUtilisateursAsyncTask() As System.Threading.Tasks.Task
        Dim utilisateurs = Await New AuthentificationDepot().ListerUtilisateursAsync()
        dgvUtilisateurs.Rows.Clear()
        For Each utilisateur As UtilisateurSession In utilisateurs
            dgvUtilisateurs.Rows.Add(utilisateur.Id, utilisateur.Login, utilisateur.Role, If(utilisateur.Actif, "Actif", "Désactivé"))
        Next
    End Function

    Private Sub UtilisateurSelectionne(sender As Object, e As EventArgs)
        If dgvUtilisateurs.CurrentRow Is Nothing Then Return
        cmbRoleUtilisateur.SelectedItem = Convert.ToString(dgvUtilisateurs.CurrentRow.Cells("role").Value, CultureInfo.CurrentCulture)
        chkActifUtilisateur.Checked = Convert.ToString(dgvUtilisateurs.CurrentRow.Cells("actif").Value, CultureInfo.CurrentCulture) = "Actif"
    End Sub

    Private Async Sub EnregistrerUtilisateurAsync(sender As Object, e As EventArgs)
        Try
            If dgvUtilisateurs.CurrentRow Is Nothing Then Throw New InvalidOperationException("Sélectionnez un compte utilisateur.")
            Dim id As Long = Convert.ToInt64(dgvUtilisateurs.CurrentRow.Cells("id").Value)
            Dim role As String = Convert.ToString(cmbRoleUtilisateur.SelectedItem, CultureInfo.CurrentCulture)
            Await New AuthentificationDepot().EnregistrerUtilisateurAsync(id, role, chkActifUtilisateur.Checked, _utilisateur.Id)
            Await ChargerUtilisateursAsyncTask()
        Catch ex As Exception
            AfficherErreur("Modification du compte impossible", ex)
        End Try
    End Sub

    Private Async Sub SupprimerUtilisateurAsync(sender As Object, e As EventArgs)
        Try
            If dgvUtilisateurs.CurrentRow Is Nothing Then Throw New InvalidOperationException("Sélectionnez un compte utilisateur.")
            Dim id As Long = Convert.ToInt64(dgvUtilisateurs.CurrentRow.Cells("id").Value)
            Dim login As String = Convert.ToString(dgvUtilisateurs.CurrentRow.Cells("login").Value, CultureInfo.CurrentCulture)
            If id = _utilisateur.Id Then Throw New InvalidOperationException("Vous ne pouvez pas supprimer le compte avec lequel vous êtes connecté.")

            Dim confirmation As DialogResult = MessageBox.Show(Me,
                "Supprimer définitivement le compte '" & login & "' ?" & Environment.NewLine & Environment.NewLine &
                "Les comptes liés à des tickets, alertes ou dépenses ne peuvent pas être supprimés. Désactivez-les pour conserver leur historique. Le dernier administrateur actif est protégé.",
                "Suppression définitive d'un compte", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            If confirmation <> DialogResult.Yes Then Return

            Await New AuthentificationDepot().SupprimerUtilisateurAsync(id, _utilisateur.Id)
            Await ChargerUtilisateursAsyncTask()
            dgvUtilisateurs.ClearSelection()
            cmbRoleUtilisateur.SelectedIndex = -1
            chkActifUtilisateur.Checked = False
        Catch ex As Exception
            AfficherErreur("Suppression du compte impossible", ex)
        End Try
    End Sub

    Private Sub ViderProduit()
        lstProduits.ClearSelected()
        lblResultatCatalogue.Text = String.Empty
        txtNomProduit.Clear()
        txtMarqueProduit.Clear()
        txtCodeBarresProduit.Clear()
        If cmbCategorieProduit.Items.Count > 0 Then cmbCategorieProduit.SelectedIndex = 0
        If cmbUniteBaseProduit.Items.Count > 0 Then cmbUniteBaseProduit.SelectedIndex = 0
    End Sub

    Private Shared Function UniteProduitVersTexte(unite As UniteProduit) As String
        Select Case unite
            Case UniteProduit.Kilogramme
                Return "kg"
            Case UniteProduit.Litre
                Return "L"
            Case Else
                Return "pièce"
        End Select
    End Function
End Class
