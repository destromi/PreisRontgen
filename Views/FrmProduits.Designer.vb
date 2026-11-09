Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmProduits
    Inherits Form

    <DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New Container()
        grpCatalogue = New GroupBox()
        lblRecherche = New Label()
        txtRecherche = New TextBox()
        dgvProduits = New DataGridView()
        grpFiche = New GroupBox()
        Label1 = New Label()
        lblCodeBarres = New Label()
        txtCodeBarres = New TextBox()
        btnRechercherOFF = New Button()
        lblInfoOFF = New Label()
        lblNom = New Label()
        txtNom = New TextBox()
        lblMarque = New Label()
        txtMarque = New TextBox()
        lblCategorie = New Label()
        cmbCategorie = New ComboBox()
        lblUnite = New Label()
        cmbUnite = New ComboBox()
        btnEnregistrer = New Button()
        btnNouveau = New Button()
        btnDesactiver = New Button()
        errProvider = New ErrorProvider(components)
        SplitContainer1 = New SplitContainer()
        grpCatalogue.SuspendLayout()
        CType(dgvProduits, ISupportInitialize).BeginInit()
        grpFiche.SuspendLayout()
        CType(errProvider, ISupportInitialize).BeginInit()
        CType(SplitContainer1, ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        SuspendLayout()
        ' 
        ' grpCatalogue
        ' 
        grpCatalogue.Controls.Add(lblRecherche)
        grpCatalogue.Controls.Add(txtRecherche)
        grpCatalogue.Controls.Add(dgvProduits)
        grpCatalogue.Dock = DockStyle.Fill
        grpCatalogue.Location = New Point(0, 0)
        grpCatalogue.Margin = New Padding(3, 4, 3, 4)
        grpCatalogue.Name = "grpCatalogue"
        grpCatalogue.Padding = New Padding(3, 4, 3, 4)
        grpCatalogue.Size = New Size(860, 747)
        grpCatalogue.TabIndex = 2
        grpCatalogue.TabStop = False
        grpCatalogue.Text = "Catalogue des produits"
        ' 
        ' lblRecherche
        ' 
        lblRecherche.AutoSize = True
        lblRecherche.Location = New Point(14, 39)
        lblRecherche.Name = "lblRecherche"
        lblRecherche.Size = New Size(89, 20)
        lblRecherche.TabIndex = 0
        lblRecherche.Text = "Rechercher :"
        ' 
        ' txtRecherche
        ' 
        txtRecherche.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtRecherche.Location = New Point(114, 35)
        txtRecherche.Margin = New Padding(3, 4, 3, 4)
        txtRecherche.Name = "txtRecherche"
        txtRecherche.Size = New Size(732, 27)
        txtRecherche.TabIndex = 0
        ' 
        ' dgvProduits
        ' 
        dgvProduits.AllowUserToAddRows = False
        dgvProduits.AllowUserToDeleteRows = False
        dgvProduits.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvProduits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvProduits.ColumnHeadersHeight = 29
        dgvProduits.Location = New Point(14, 80)
        dgvProduits.Margin = New Padding(3, 4, 3, 4)
        dgvProduits.MultiSelect = False
        dgvProduits.Name = "dgvProduits"
        dgvProduits.ReadOnly = True
        dgvProduits.RowHeadersVisible = False
        dgvProduits.RowHeadersWidth = 51
        dgvProduits.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvProduits.Size = New Size(833, 651)
        dgvProduits.TabIndex = 1
        ' 
        ' grpFiche
        ' 
        grpFiche.Controls.Add(Label1)
        grpFiche.Controls.Add(lblCodeBarres)
        grpFiche.Controls.Add(txtCodeBarres)
        grpFiche.Controls.Add(btnRechercherOFF)
        grpFiche.Controls.Add(lblInfoOFF)
        grpFiche.Controls.Add(lblNom)
        grpFiche.Controls.Add(txtNom)
        grpFiche.Controls.Add(lblMarque)
        grpFiche.Controls.Add(txtMarque)
        grpFiche.Controls.Add(lblCategorie)
        grpFiche.Controls.Add(cmbCategorie)
        grpFiche.Controls.Add(lblUnite)
        grpFiche.Controls.Add(cmbUnite)
        grpFiche.Controls.Add(btnEnregistrer)
        grpFiche.Controls.Add(btnNouveau)
        grpFiche.Controls.Add(btnDesactiver)
        grpFiche.Dock = DockStyle.Fill
        grpFiche.Location = New Point(0, 0)
        grpFiche.Margin = New Padding(3, 4, 3, 4)
        grpFiche.Name = "grpFiche"
        grpFiche.Padding = New Padding(3, 4, 3, 4)
        grpFiche.Size = New Size(678, 747)
        grpFiche.TabIndex = 1
        grpFiche.TabStop = False
        grpFiche.Text = "Fiche produit"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 8.0F)
        Label1.ForeColor = Color.DimGray
        Label1.Location = New Point(18, 569)
        Label1.Name = "Label1"
        Label1.Size = New Size(637, 38)
        Label1.TabIndex = 9
        Label1.Text = "La recherche est facultative et envoie uniquement le code barre après confirmation. Vérifier les champs " & vbCrLf & " importés; la catégorie et l'unité restent à confirmer "
        ' 
        ' lblCodeBarres
        ' 
        lblCodeBarres.AutoSize = True
        lblCodeBarres.Location = New Point(18, 191)
        lblCodeBarres.Name = "lblCodeBarres"
        lblCodeBarres.Size = New Size(164, 20)
        lblCodeBarres.TabIndex = 0
        lblCodeBarres.Text = "Code-barres (facultatif)"
        ' 
        ' txtCodeBarres
        ' 
        txtCodeBarres.Location = New Point(18, 217)
        txtCodeBarres.Margin = New Padding(3, 4, 3, 4)
        txtCodeBarres.MaxLength = 50
        txtCodeBarres.Name = "txtCodeBarres"
        txtCodeBarres.Size = New Size(519, 27)
        txtCodeBarres.TabIndex = 0
        ' 
        ' btnRechercherOFF
        ' 
        btnRechercherOFF.BackColor = Color.White
        btnRechercherOFF.FlatAppearance.BorderColor = Color.ForestGreen
        btnRechercherOFF.FlatStyle = FlatStyle.Flat
        btnRechercherOFF.ForeColor = Color.ForestGreen
        btnRechercherOFF.Location = New Point(18, 271)
        btnRechercherOFF.Margin = New Padding(3, 4, 3, 4)
        btnRechercherOFF.Name = "btnRechercherOFF"
        btnRechercherOFF.Size = New Size(222, 37)
        btnRechercherOFF.TabIndex = 1
        btnRechercherOFF.Text = "Rechercher Open Food Facts"
        btnRechercherOFF.UseVisualStyleBackColor = False
        ' 
        ' lblInfoOFF
        ' 
        lblInfoOFF.ForeColor = Color.ForestGreen
        lblInfoOFF.Location = New Point(18, 509)
        lblInfoOFF.Name = "lblInfoOFF"
        lblInfoOFF.Size = New Size(402, 39)
        lblInfoOFF.TabIndex = 2
        ' 
        ' lblNom
        ' 
        lblNom.AutoSize = True
        lblNom.Location = New Point(18, 44)
        lblNom.Name = "lblNom"
        lblNom.Size = New Size(116, 20)
        lblNom.TabIndex = 3
        lblNom.Text = "Nom du produit"
        ' 
        ' txtNom
        ' 
        txtNom.Location = New Point(18, 71)
        txtNom.Margin = New Padding(3, 4, 3, 4)
        txtNom.MaxLength = 150
        txtNom.Name = "txtNom"
        txtNom.Size = New Size(519, 27)
        txtNom.TabIndex = 2
        ' 
        ' lblMarque
        ' 
        lblMarque.AutoSize = True
        lblMarque.Location = New Point(18, 116)
        lblMarque.Name = "lblMarque"
        lblMarque.Size = New Size(60, 20)
        lblMarque.TabIndex = 4
        lblMarque.Text = "Marque"
        ' 
        ' txtMarque
        ' 
        txtMarque.Location = New Point(18, 143)
        txtMarque.Margin = New Padding(3, 4, 3, 4)
        txtMarque.MaxLength = 100
        txtMarque.Name = "txtMarque"
        txtMarque.Size = New Size(519, 27)
        txtMarque.TabIndex = 3
        ' 
        ' lblCategorie
        ' 
        lblCategorie.AutoSize = True
        lblCategorie.Location = New Point(18, 342)
        lblCategorie.Name = "lblCategorie"
        lblCategorie.Size = New Size(74, 20)
        lblCategorie.TabIndex = 5
        lblCategorie.Text = "Catégorie"
        ' 
        ' cmbCategorie
        ' 
        cmbCategorie.DropDownStyle = ComboBoxStyle.DropDownList
        cmbCategorie.Location = New Point(18, 369)
        cmbCategorie.Margin = New Padding(3, 4, 3, 4)
        cmbCategorie.Name = "cmbCategorie"
        cmbCategorie.Size = New Size(295, 28)
        cmbCategorie.TabIndex = 4
        ' 
        ' lblUnite
        ' 
        lblUnite.AutoSize = True
        lblUnite.Location = New Point(360, 342)
        lblUnite.Name = "lblUnite"
        lblUnite.Size = New Size(100, 20)
        lblUnite.TabIndex = 6
        lblUnite.Text = "Unité de base"
        ' 
        ' cmbUnite
        ' 
        cmbUnite.DropDownStyle = ComboBoxStyle.DropDownList
        cmbUnite.Location = New Point(360, 369)
        cmbUnite.Margin = New Padding(3, 4, 3, 4)
        cmbUnite.Name = "cmbUnite"
        cmbUnite.Size = New Size(177, 28)
        cmbUnite.TabIndex = 5
        ' 
        ' btnEnregistrer
        ' 
        btnEnregistrer.BackColor = Color.ForestGreen
        btnEnregistrer.FlatAppearance.BorderColor = Color.ForestGreen
        btnEnregistrer.FlatStyle = FlatStyle.Flat
        btnEnregistrer.ForeColor = Color.White
        btnEnregistrer.Location = New Point(18, 438)
        btnEnregistrer.Margin = New Padding(3, 4, 3, 4)
        btnEnregistrer.Name = "btnEnregistrer"
        btnEnregistrer.Size = New Size(194, 48)
        btnEnregistrer.TabIndex = 6
        btnEnregistrer.Text = "Enregistrer le produit"
        btnEnregistrer.UseVisualStyleBackColor = False
        ' 
        ' btnNouveau
        ' 
        btnNouveau.CausesValidation = False
        btnNouveau.FlatAppearance.BorderColor = Color.ForestGreen
        btnNouveau.FlatStyle = FlatStyle.Flat
        btnNouveau.ForeColor = Color.ForestGreen
        btnNouveau.Location = New Point(238, 438)
        btnNouveau.Margin = New Padding(3, 4, 3, 4)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(143, 48)
        btnNouveau.TabIndex = 7
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' btnDesactiver
        ' 
        btnDesactiver.CausesValidation = False
        btnDesactiver.FlatAppearance.BorderColor = Color.ForestGreen
        btnDesactiver.FlatStyle = FlatStyle.Flat
        btnDesactiver.ForeColor = Color.ForestGreen
        btnDesactiver.Location = New Point(401, 438)
        btnDesactiver.Margin = New Padding(3, 4, 3, 4)
        btnDesactiver.Name = "btnDesactiver"
        btnDesactiver.Size = New Size(136, 48)
        btnDesactiver.TabIndex = 8
        btnDesactiver.Text = "Désactiver"
        btnDesactiver.UseVisualStyleBackColor = True
        ' 
        ' errProvider
        ' 
        errProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink
        errProvider.ContainerControl = Me
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.Location = New Point(0, 0)
        SplitContainer1.Name = "SplitContainer1"
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(grpCatalogue)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.Controls.Add(grpFiche)
        SplitContainer1.Size = New Size(1542, 747)
        SplitContainer1.SplitterDistance = 860
        SplitContainer1.TabIndex = 3
        ' 
        ' FrmProduits
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1542, 747)
        Controls.Add(SplitContainer1)
        Font = New Font("Segoe UI", 9.0F)
        Margin = New Padding(3, 4, 3, 4)
        MinimumSize = New Size(1026, 731)
        Name = "FrmProduits"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Pricieux - Référentiels : Produits"
        grpCatalogue.ResumeLayout(False)
        grpCatalogue.PerformLayout()
        CType(dgvProduits, ISupportInitialize).EndInit()
        grpFiche.ResumeLayout(False)
        grpFiche.PerformLayout()
        CType(errProvider, ISupportInitialize).EndInit()
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        CType(SplitContainer1, ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        ResumeLayout(False)
    End Sub
    Friend WithEvents grpCatalogue As GroupBox
    Friend WithEvents lblRecherche As Label
    Friend WithEvents txtRecherche As TextBox
    Friend WithEvents dgvProduits As DataGridView
    Friend WithEvents grpFiche As GroupBox
    Friend WithEvents lblCodeBarres As Label
    Friend WithEvents txtCodeBarres As TextBox
    Friend WithEvents btnRechercherOFF As Button
    Friend WithEvents lblInfoOFF As Label
    Friend WithEvents lblNom As Label
    Friend WithEvents txtNom As TextBox
    Friend WithEvents lblMarque As Label
    Friend WithEvents txtMarque As TextBox
    Friend WithEvents lblCategorie As Label
    Friend WithEvents cmbCategorie As ComboBox
    Friend WithEvents lblUnite As Label
    Friend WithEvents cmbUnite As ComboBox
    Friend WithEvents btnEnregistrer As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents btnDesactiver As Button
    Friend WithEvents errProvider As ErrorProvider
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents Label1 As Label
End Class