Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms

Public NotInheritable Partial Class MainForm
    Private ReadOnly dtpDateDepartIndice As New DateTimePicker()
    Private ReadOnly dtpDateArriveeIndice As New DateTimePicker()
    Private ReadOnly lblResultatIndice As New Label()
    Private ReadOnly cmbProduitComparaison As New ComboBox()
    Private ReadOnly cmbCategorieComparaison As New ComboBox()
    Private ReadOnly dgvComparaisonMagasins As New DataGridView()
    Private ReadOnly dgvProduitsEquivalents As New DataGridView()

    Private Sub ConstruireOngletAnalyses()
        Dim page As New TabPage("Analyses") With {.BackColor = ThemePricieux.FondPage, .Padding = New Padding(18)}
        _onglets.TabPages.Add(page)
        page.Controls.Add(New Label With {.Text = "Indices et comparaisons", .Font = New Font("Segoe UI", 17, FontStyle.Bold), .AutoSize = True, .Location = New Point(20, 18), .ForeColor = ThemePricieux.Foret})

        Dim panneauIndice As New Panel With {.Location = New Point(22, 66), .Size = New Size(1190, 148), .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right, .BackColor = Color.White, .Padding = New Padding(14)}
        page.Controls.Add(panneauIndice)
        panneauIndice.Controls.Add(New Label With {.Text = "Indice personnel — panier des 30 jours avant la date de départ", .Font = New Font("Segoe UI", 11, FontStyle.Bold), .AutoSize = True, .Location = New Point(14, 12)})
        dtpDateDepartIndice.Name = "dtpDateDepartIndice"
        dtpDateDepartIndice.Format = DateTimePickerFormat.Short
        dtpDateDepartIndice.Value = Date.Today.AddMonths(-1)
        dtpDateDepartIndice.Location = New Point(18, 60)
        dtpDateDepartIndice.Width = 150
        panneauIndice.Controls.Add(New Label With {.Text = "Départ", .Location = New Point(18, 40), .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold)})
        panneauIndice.Controls.Add(dtpDateDepartIndice)
        dtpDateArriveeIndice.Name = "dtpDateArriveeIndice"
        dtpDateArriveeIndice.Format = DateTimePickerFormat.Short
        dtpDateArriveeIndice.Value = Date.Today
        dtpDateArriveeIndice.Location = New Point(282, 60)
        dtpDateArriveeIndice.Width = 150
        panneauIndice.Controls.Add(New Label With {.Text = "Arrivée", .Location = New Point(282, 40), .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold)})
        panneauIndice.Controls.Add(dtpDateArriveeIndice)
        Dim btnCalculerIndice As Button = CreerBouton("Calculer", True)
        btnCalculerIndice.Location = New Point(465, 56)
        AddHandler btnCalculerIndice.Click, AddressOf CalculerIndiceAsync
        panneauIndice.Controls.Add(btnCalculerIndice)
        lblResultatIndice.Text = "Base 100 — informations descriptives, sans recommandation d'achat."
        lblResultatIndice.Location = New Point(620, 55)
        lblResultatIndice.Size = New Size(540, 65)
        lblResultatIndice.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblResultatIndice.ForeColor = ThemePricieux.Foret
        panneauIndice.Controls.Add(lblResultatIndice)

        Dim panneauComparaison As New Panel With {.Location = New Point(22, 229), .Size = New Size(1190, 438), .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right, .BackColor = Color.White, .Padding = New Padding(12)}
        page.Controls.Add(panneauComparaison)
        panneauComparaison.Controls.Add(New Label With {.Text = "Classement des magasins et produits de même catégorie/unité", .Font = New Font("Segoe UI", 11, FontStyle.Bold), .AutoSize = True, .Location = New Point(14, 12)})
        cmbProduitComparaison.Name = "cmbProduitComparaison"
        cmbProduitComparaison.DropDownStyle = ComboBoxStyle.DropDownList
        cmbProduitComparaison.Width = 300
        cmbProduitComparaison.FormattingEnabled = True
        AddHandler cmbProduitComparaison.Format, AddressOf FormaterProduit
        cmbProduitComparaison.Location = New Point(18, 49)
        panneauComparaison.Controls.Add(cmbProduitComparaison)
        cmbCategorieComparaison.Name = "cmbCategorieComparaison"
        cmbCategorieComparaison.DropDownStyle = ComboBoxStyle.DropDownList
        cmbCategorieComparaison.Width = 220
        cmbCategorieComparaison.Location = New Point(330, 49)
        panneauComparaison.Controls.Add(cmbCategorieComparaison)
        Dim btnComparer As Button = CreerBouton("Comparer magasins", True)
        btnComparer.Location = New Point(566, 45)
        AddHandler btnComparer.Click, AddressOf ComparerMagasinsAsync
        panneauComparaison.Controls.Add(btnComparer)
        Dim btnEquivalents As Button = CreerBouton("Voir équivalents moins chers", False)
        btnEquivalents.Location = New Point(744, 45)
        AddHandler btnEquivalents.Click, AddressOf ChargerEquivalentsAsync
        panneauComparaison.Controls.Add(btnEquivalents)

        ConfigurerGrille(dgvComparaisonMagasins)
        dgvComparaisonMagasins.Name = "dgvComparaisonMagasins"
        dgvComparaisonMagasins.Location = New Point(18, 103)
        dgvComparaisonMagasins.Size = New Size(500, 306)
        dgvComparaisonMagasins.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        panneauComparaison.Controls.Add(dgvComparaisonMagasins)
        dgvComparaisonMagasins.Columns.Add("produit", "Produit")
        dgvComparaisonMagasins.Columns.Add("magasin", "Magasin")
        dgvComparaisonMagasins.Columns.Add("prix", "Prix unitaire")
        dgvComparaisonMagasins.Columns.Add("date", "Date")
        dgvComparaisonMagasins.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvComparaisonMagasins.Columns(1).Width = 140
        dgvComparaisonMagasins.Columns(2).Width = 145
        dgvComparaisonMagasins.Columns(3).Width = 100

        ConfigurerGrille(dgvProduitsEquivalents)
        dgvProduitsEquivalents.Name = "dgvProduitsEquivalents"
        dgvProduitsEquivalents.Location = New Point(540, 103)
        dgvProduitsEquivalents.Size = New Size(625, 306)
        dgvProduitsEquivalents.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panneauComparaison.Controls.Add(dgvProduitsEquivalents)
        dgvProduitsEquivalents.Columns.Add("produit", "Produit équivalent")
        dgvProduitsEquivalents.Columns.Add("magasin", "Magasin observé")
        dgvProduitsEquivalents.Columns.Add("prix", "Prix unitaire")
        dgvProduitsEquivalents.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvProduitsEquivalents.Columns(1).Width = 170
        dgvProduitsEquivalents.Columns(2).Width = 140
    End Sub

    Private Async Sub CalculerIndiceAsync(sender As Object, e As EventArgs)
        Try
            Dim resultat As ResultatIndice = Await _analysesDepot.CalculerIndiceAsync(_utilisateur.Id, dtpDateDepartIndice.Value.Date, dtpDateArriveeIndice.Value.Date)
            If Not resultat.Indice.HasValue Then
                lblResultatIndice.Text = "Données insuffisantes pour calculer le panier de référence."
                Return
            End If
            lblResultatIndice.Text = "Indice " & resultat.Indice.Value.ToString("N2", CultureInfo.CurrentCulture) & " · " & resultat.ProduitsCompares.ToString(CultureInfo.CurrentCulture) & " produits" & Environment.NewLine &
                "Coût de référence : " & resultat.CoutDepart.ToString("N0") & " FCFA → " & resultat.CoutArrivee.ToString("N0") & " FCFA"
        Catch ex As Exception
            AfficherErreur("Calcul de l'indice impossible", ex)
        End Try
    End Sub

    Private Async Sub ComparerMagasinsAsync(sender As Object, e As EventArgs)
        Try
            Dim produit As Produit = TryCast(cmbProduitComparaison.SelectedItem, Produit)
            Dim categorie As Categorie = TryCast(cmbCategorieComparaison.SelectedItem, Categorie)
            If produit Is Nothing AndAlso categorie Is Nothing Then Throw New InvalidOperationException("Choisissez un produit ou une catégorie.")
            Dim observations = Await _analysesDepot.ComparerMagasinsAsync(_utilisateur.Id, If(produit Is Nothing, CType(Nothing, Long?), produit.Id), If(produit IsNot Nothing OrElse categorie Is Nothing, CType(Nothing, Long?), categorie.Id))
            dgvComparaisonMagasins.Rows.Clear()
            For Each item As PrixMagasin In observations
                dgvComparaisonMagasins.Rows.Add(item.ProduitNom, item.MagasinNom, item.PrixUnitaire.ToString("N2") & " / " & item.UniteBase, item.DateObservation.ToString("dd/MM/yyyy"))
            Next
            If observations.Count = 0 Then dgvComparaisonMagasins.Rows.Add("Aucune observation enregistrée", String.Empty, String.Empty, String.Empty)
        Catch ex As Exception
            AfficherErreur("Comparaison indisponible", ex)
        End Try
    End Sub

    Private Async Sub ChargerEquivalentsAsync(sender As Object, e As EventArgs)
        Try
            Dim produit As Produit = TryCast(cmbProduitComparaison.SelectedItem, Produit)
            If produit Is Nothing Then Throw New InvalidOperationException("Sélectionnez un produit à comparer.")
            Dim equivalents = Await _analysesDepot.ProduitsMoinsChersAsync(_utilisateur.Id, produit.Id)
            dgvProduitsEquivalents.Rows.Clear()
            For Each item As ProduitEquivalent In equivalents
                dgvProduitsEquivalents.Rows.Add(item.ProduitNom & If(String.IsNullOrWhiteSpace(item.Marque), String.Empty, " — " & item.Marque), item.MagasinNom,
                    item.PrixUnitaire.ToString("N2") & " / " & item.UniteBase)
            Next
            If equivalents.Count = 0 Then dgvProduitsEquivalents.Rows.Add("Aucun prix inférieur observé dans cette catégorie et unité.", String.Empty, String.Empty)
        Catch ex As Exception
            AfficherErreur("Comparaison des produits impossible", ex)
        End Try
    End Sub
End Class
