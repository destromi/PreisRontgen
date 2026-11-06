Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Pt = System.Drawing.Point
Imports Sz = System.Drawing.Size
Imports Fnt = System.Drawing.Font

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
        pnlEntete = New Panel()
        lblTitre = New Label()
        grpCatalogue = New GroupBox()
        lblRecherche = New Label()
        txtRecherche = New TextBox()
        dgvProduits = New DataGridView()
        grpFiche = New GroupBox()
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

        CType(dgvProduits, ISupportInitialize).BeginInit()
        CType(errProvider, ISupportInitialize).BeginInit()
        pnlEntete.SuspendLayout()
        grpCatalogue.SuspendLayout()
        grpFiche.SuspendLayout()
        SuspendLayout()

        ' ----- Bandeau vert forêt -----
        With pnlEntete
            .Name = "pnlEntete"
            .BackColor = Color.FromArgb(34, 85, 51)
            .Dock = DockStyle.Top
            .Size = New Sz(940, 50)
            .Controls.Add(lblTitre)
        End With
        With lblTitre
            .Name = "lblTitre"
            .AutoSize = True
            .Font = New Fnt("Segoe UI", 14.0F, FontStyle.Bold)
            .ForeColor = Color.White
            .Location = New Pt(16, 11)
            .Text = "Référentiels - Produits"
        End With

        ' ----- Catalogue (à gauche) -----
        With grpCatalogue
            .Name = "grpCatalogue"
            .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
            .Location = New Pt(12, 62)
            .Size = New Sz(520, 486)
            .TabIndex = 2
            .TabStop = False
            .Text = "Catalogue des produits"
            .Controls.Add(lblRecherche)
            .Controls.Add(txtRecherche)
            .Controls.Add(dgvProduits)
        End With
        With lblRecherche
            .Name = "lblRecherche"
            .AutoSize = True
            .Location = New Pt(12, 29)
            .Text = "Rechercher :"
        End With
        With txtRecherche
            .Name = "txtRecherche"
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            .Location = New Pt(100, 26)
            .Size = New Sz(408, 23)
            .TabIndex = 0
        End With
        With dgvProduits
            .Name = "dgvProduits"
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .MultiSelect = False
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
            .Location = New Pt(12, 60)
            .Size = New Sz(496, 414)
            .TabIndex = 1
        End With

        ' ----- Fiche produit (à droite) -----
        With grpFiche
            .Name = "grpFiche"
            .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
            .Location = New Pt(544, 62)
            .Size = New Sz(384, 486)
            .TabIndex = 1
            .TabStop = False
            .Text = "Fiche produit"
            .Controls.Add(lblCodeBarres)
            .Controls.Add(txtCodeBarres)
            .Controls.Add(btnRechercherOFF)
            .Controls.Add(lblInfoOFF)
            .Controls.Add(lblNom)
            .Controls.Add(txtNom)
            .Controls.Add(lblMarque)
            .Controls.Add(txtMarque)
            .Controls.Add(lblCategorie)
            .Controls.Add(cmbCategorie)
            .Controls.Add(lblUnite)
            .Controls.Add(cmbUnite)
            .Controls.Add(btnEnregistrer)
            .Controls.Add(btnNouveau)
            .Controls.Add(btnDesactiver)
        End With

        With lblCodeBarres
            .Name = "lblCodeBarres"
            .AutoSize = True
            .Location = New Pt(16, 32)
            .Text = "Code-barres (facultatif)"
        End With
        With txtCodeBarres
            .Name = "txtCodeBarres"
            .Location = New Pt(16, 52)
            .MaxLength = 50
            .Size = New Sz(150, 23)
            .TabIndex = 0
        End With
        With btnRechercherOFF
            .Name = "btnRechercherOFF"
            .Location = New Pt(174, 50)
            .Size = New Sz(194, 28)
            .TabIndex = 1
            .Text = "Rechercher Open Food Facts"
            .UseVisualStyleBackColor = True
        End With
        With lblInfoOFF
            .Name = "lblInfoOFF"
            .AutoSize = False
            .ForeColor = Color.FromArgb(34, 85, 51)
            .Location = New Pt(16, 84)
            .Size = New Sz(352, 44)
            .Text = ""
        End With
        With lblNom
            .Name = "lblNom"
            .AutoSize = True
            .Location = New Pt(16, 136)
            .Text = "Nom du produit"
        End With
        With txtNom
            .Name = "txtNom"
            .Location = New Pt(16, 156)
            .MaxLength = 150
            .Size = New Sz(352, 23)
            .TabIndex = 2
        End With
        With lblMarque
            .Name = "lblMarque"
            .AutoSize = True
            .Location = New Pt(16, 190)
            .Text = "Marque"
        End With
        With txtMarque
            .Name = "txtMarque"
            .Location = New Pt(16, 210)
            .MaxLength = 100
            .Size = New Sz(352, 23)
            .TabIndex = 3
        End With
        With lblCategorie
            .Name = "lblCategorie"
            .AutoSize = True
            .Location = New Pt(16, 244)
            .Text = "Catégorie"
        End With
        With cmbCategorie
            .Name = "cmbCategorie"
            .DropDownStyle = ComboBoxStyle.DropDownList
            .Location = New Pt(16, 264)
            .Size = New Sz(352, 23)
            .TabIndex = 4
        End With
        With lblUnite
            .Name = "lblUnite"
            .AutoSize = True
            .Location = New Pt(16, 298)
            .Text = "Unité de base"
        End With
        With cmbUnite
            .Name = "cmbUnite"
            .DropDownStyle = ComboBoxStyle.DropDownList
            .Location = New Pt(16, 318)
            .Size = New Sz(352, 23)
            .TabIndex = 5
        End With
        With btnEnregistrer
            .Name = "btnEnregistrer"
            .Location = New Pt(16, 380)
            .Size = New Sz(170, 36)
            .TabIndex = 6
            .Text = "Enregistrer le produit"
            .UseVisualStyleBackColor = True
        End With
        With btnNouveau
            .Name = "btnNouveau"
            .CausesValidation = False
            .Location = New Pt(194, 380)
            .Size = New Sz(80, 36)
            .TabIndex = 7
            .Text = "Nouveau"
            .UseVisualStyleBackColor = True
        End With
        With btnDesactiver
            .Name = "btnDesactiver"
            .CausesValidation = False
            .Location = New Pt(282, 380)
            .Size = New Sz(86, 36)
            .TabIndex = 8
            .Text = "Désactiver"
            .UseVisualStyleBackColor = True
        End With

        With errProvider
            .ContainerControl = Me
            .BlinkStyle = ErrorBlinkStyle.NeverBlink
        End With

        ' ----- Formulaire -----
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New Sz(940, 560)
        Me.Controls.Add(grpFiche)
        Me.Controls.Add(grpCatalogue)
        Me.Controls.Add(pnlEntete)
        Me.Font = New Fnt("Segoe UI", 9.0F)
        Me.MinimumSize = New Sz(900, 560)
        Me.Name = "FrmProduits"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "Pricieux - Référentiels : Produits"

        CType(dgvProduits, ISupportInitialize).EndInit()
        CType(errProvider, ISupportInitialize).EndInit()
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        grpCatalogue.ResumeLayout(False)
        grpCatalogue.PerformLayout()
        grpFiche.ResumeLayout(False)
        grpFiche.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlEntete As Panel
    Friend WithEvents lblTitre As Label
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
End Class