Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmTickets
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
        grpEntete = New GroupBox()
        LblInfo = New Label()
        BtnOCR = New Button()
        lblMagasin = New Label()
        cmbMagasin = New ComboBox()
        lblDate = New Label()
        dtpDate = New DateTimePicker()
        grpLigne = New GroupBox()
        lblProduit = New Label()
        txtProduit = New TextBox()
        lblConditionnement = New Label()
        txtConditionnement = New TextBox()
        lblUnite = New Label()
        cmbUnite = New ComboBox()
        lblPrix = New Label()
        txtPrix = New TextBox()
        lblNombre = New Label()
        txtNombre = New TextBox()
        btnAjouter = New Button()
        lblProduitInfo = New Label()
        dgvLignes = New DataGridView()
        lblSomme = New Label()
        lblTotalSaisi = New Label()
        txtTotalSaisi = New TextBox()
        btnSupprimerLigne = New Button()
        btnEnregistrer = New Button()
        errProvider = New ErrorProvider(components)
        pnlEntete.SuspendLayout()
        grpEntete.SuspendLayout()
        grpLigne.SuspendLayout()
        CType(dgvLignes, ISupportInitialize).BeginInit()
        CType(errProvider, ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' pnlEntete
        ' 
        pnlEntete.BackColor = SystemColors.Control
        pnlEntete.Controls.Add(lblTitre)
        pnlEntete.Dock = DockStyle.Top
        pnlEntete.Location = New Point(0, 0)
        pnlEntete.Margin = New Padding(3, 4, 3, 4)
        pnlEntete.Name = "pnlEntete"
        pnlEntete.Size = New Size(1143, 67)
        pnlEntete.TabIndex = 6
        ' 
        ' lblTitre
        ' 
        lblTitre.AutoSize = True
        lblTitre.Font = New Font("Segoe UI", 18.0F, FontStyle.Bold)
        lblTitre.ForeColor = Color.ForestGreen
        lblTitre.Location = New Point(18, 15)
        lblTitre.Name = "lblTitre"
        lblTitre.Size = New Size(232, 41)
        lblTitre.TabIndex = 0
        lblTitre.Text = "Nouveau ticket"
        ' 
        ' grpEntete
        ' 
        grpEntete.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        grpEntete.BackColor = Color.White
        grpEntete.Controls.Add(LblInfo)
        grpEntete.Controls.Add(BtnOCR)
        grpEntete.Controls.Add(lblMagasin)
        grpEntete.Controls.Add(cmbMagasin)
        grpEntete.Controls.Add(lblDate)
        grpEntete.Controls.Add(dtpDate)
        grpEntete.Location = New Point(14, 83)
        grpEntete.Margin = New Padding(3, 4, 3, 4)
        grpEntete.Name = "grpEntete"
        grpEntete.Padding = New Padding(3, 4, 3, 4)
        grpEntete.Size = New Size(1115, 120)
        grpEntete.TabIndex = 0
        grpEntete.TabStop = False
        grpEntete.Text = "En-tête du ticket"
        ' 
        ' LblInfo
        ' 
        LblInfo.AutoSize = True
        LblInfo.Location = New Point(877, 76)
        LblInfo.Name = "LblInfo"
        LblInfo.Size = New Size(53, 20)
        LblInfo.TabIndex = 3
        LblInfo.Text = "Label1"
        ' 
        ' BtnOCR
        ' 
        BtnOCR.FlatAppearance.BorderColor = Color.ForestGreen
        BtnOCR.FlatStyle = FlatStyle.Flat
        BtnOCR.ForeColor = Color.ForestGreen
        BtnOCR.Location = New Point(833, 33)
        BtnOCR.Name = "BtnOCR"
        BtnOCR.Size = New Size(177, 29)
        BtnOCR.TabIndex = 2
        BtnOCR.Text = "Lire une photo (OCR)"
        BtnOCR.UseVisualStyleBackColor = True
        ' 
        ' lblMagasin
        ' 
        lblMagasin.AutoSize = True
        lblMagasin.Location = New Point(18, 40)
        lblMagasin.Name = "lblMagasin"
        lblMagasin.Size = New Size(65, 20)
        lblMagasin.TabIndex = 0
        lblMagasin.Text = "Magasin"
        ' 
        ' cmbMagasin
        ' 
        cmbMagasin.DropDownStyle = ComboBoxStyle.DropDownList
        cmbMagasin.Location = New Point(87, 35)
        cmbMagasin.Margin = New Padding(3, 4, 3, 4)
        cmbMagasin.Name = "cmbMagasin"
        cmbMagasin.Size = New Size(342, 28)
        cmbMagasin.TabIndex = 0
        ' 
        ' lblDate
        ' 
        lblDate.AutoSize = True
        lblDate.Location = New Point(469, 40)
        lblDate.Name = "lblDate"
        lblDate.Size = New Size(93, 20)
        lblDate.TabIndex = 1
        lblDate.Text = "Date d'achat"
        ' 
        ' dtpDate
        ' 
        dtpDate.Format = DateTimePickerFormat.Short
        dtpDate.Location = New Point(560, 35)
        dtpDate.Margin = New Padding(3, 4, 3, 4)
        dtpDate.Name = "dtpDate"
        dtpDate.Size = New Size(159, 27)
        dtpDate.TabIndex = 1
        ' 
        ' grpLigne
        ' 
        grpLigne.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        grpLigne.BackColor = Color.White
        grpLigne.Controls.Add(lblProduit)
        grpLigne.Controls.Add(txtProduit)
        grpLigne.Controls.Add(lblConditionnement)
        grpLigne.Controls.Add(txtConditionnement)
        grpLigne.Controls.Add(lblUnite)
        grpLigne.Controls.Add(cmbUnite)
        grpLigne.Controls.Add(lblPrix)
        grpLigne.Controls.Add(txtPrix)
        grpLigne.Controls.Add(lblNombre)
        grpLigne.Controls.Add(txtNombre)
        grpLigne.Controls.Add(btnAjouter)
        grpLigne.Controls.Add(lblProduitInfo)
        grpLigne.Location = New Point(14, 227)
        grpLigne.Margin = New Padding(3, 4, 3, 4)
        grpLigne.Name = "grpLigne"
        grpLigne.Padding = New Padding(3, 4, 3, 4)
        grpLigne.Size = New Size(1115, 109)
        grpLigne.TabIndex = 1
        grpLigne.TabStop = False
        grpLigne.Text = "Ajout d'un article"
        ' 
        ' lblProduit
        ' 
        lblProduit.AutoSize = True
        lblProduit.Location = New Point(8, 37)
        lblProduit.Name = "lblProduit"
        lblProduit.Size = New Size(57, 20)
        lblProduit.TabIndex = 0
        lblProduit.Text = "Produit"
        ' 
        ' txtProduit
        ' 
        txtProduit.Location = New Point(8, 63)
        txtProduit.Margin = New Padding(3, 4, 3, 4)
        txtProduit.Name = "txtProduit"
        txtProduit.Size = New Size(342, 27)
        txtProduit.TabIndex = 0
        ' 
        ' lblConditionnement
        ' 
        lblConditionnement.AutoSize = True
        lblConditionnement.Location = New Point(367, 37)
        lblConditionnement.Name = "lblConditionnement"
        lblConditionnement.Size = New Size(124, 20)
        lblConditionnement.TabIndex = 1
        lblConditionnement.Text = "Conditionnement"
        ' 
        ' txtConditionnement
        ' 
        txtConditionnement.Location = New Point(367, 63)
        txtConditionnement.Margin = New Padding(3, 4, 3, 4)
        txtConditionnement.Name = "txtConditionnement"
        txtConditionnement.Size = New Size(125, 27)
        txtConditionnement.TabIndex = 1
        ' 
        ' lblUnite
        ' 
        lblUnite.AutoSize = True
        lblUnite.Location = New Point(507, 37)
        lblUnite.Name = "lblUnite"
        lblUnite.Size = New Size(44, 20)
        lblUnite.TabIndex = 2
        lblUnite.Text = "Unité"
        ' 
        ' cmbUnite
        ' 
        cmbUnite.DropDownStyle = ComboBoxStyle.DropDownList
        cmbUnite.Location = New Point(507, 63)
        cmbUnite.Margin = New Padding(3, 4, 3, 4)
        cmbUnite.Name = "cmbUnite"
        cmbUnite.Size = New Size(102, 28)
        cmbUnite.TabIndex = 2
        ' 
        ' lblPrix
        ' 
        lblPrix.AutoSize = True
        lblPrix.Location = New Point(623, 37)
        lblPrix.Name = "lblPrix"
        lblPrix.Size = New Size(151, 20)
        lblPrix.TabIndex = 3
        lblPrix.Text = "Prix du paquet (FCFA)"
        ' 
        ' txtPrix
        ' 
        txtPrix.Location = New Point(623, 63)
        txtPrix.Margin = New Padding(3, 4, 3, 4)
        txtPrix.Name = "txtPrix"
        txtPrix.Size = New Size(148, 27)
        txtPrix.TabIndex = 3
        ' 
        ' lblNombre
        ' 
        lblNombre.AutoSize = True
        lblNombre.Location = New Point(785, 37)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(112, 20)
        lblNombre.TabIndex = 4
        lblNombre.Text = "Nombre acheté"
        ' 
        ' txtNombre
        ' 
        txtNombre.Location = New Point(785, 63)
        txtNombre.Margin = New Padding(3, 4, 3, 4)
        txtNombre.Name = "txtNombre"
        txtNombre.Size = New Size(102, 27)
        txtNombre.TabIndex = 4
        txtNombre.Text = "1"
        ' 
        ' btnAjouter
        ' 
        btnAjouter.BackColor = Color.White
        btnAjouter.FlatAppearance.BorderColor = Color.ForestGreen
        btnAjouter.FlatStyle = FlatStyle.Flat
        btnAjouter.ForeColor = Color.ForestGreen
        btnAjouter.Location = New Point(904, 58)
        btnAjouter.Margin = New Padding(3, 4, 3, 4)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(183, 40)
        btnAjouter.TabIndex = 5
        btnAjouter.Text = "Ajouter une ligne"
        btnAjouter.UseVisualStyleBackColor = False
        ' 
        ' lblProduitInfo
        ' 
        lblProduitInfo.ForeColor = Color.FromArgb(CByte(34), CByte(85), CByte(51))
        lblProduitInfo.Location = New Point(10, 66)
        lblProduitInfo.Name = "lblProduitInfo"
        lblProduitInfo.Size = New Size(686, 29)
        lblProduitInfo.TabIndex = 6
        ' 
        ' dgvLignes
        ' 
        dgvLignes.AllowUserToAddRows = False
        dgvLignes.AllowUserToDeleteRows = False
        dgvLignes.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvLignes.ColumnHeadersHeight = 29
        dgvLignes.Location = New Point(14, 349)
        dgvLignes.Margin = New Padding(3, 4, 3, 4)
        dgvLignes.MultiSelect = False
        dgvLignes.Name = "dgvLignes"
        dgvLignes.RowHeadersVisible = False
        dgvLignes.RowHeadersWidth = 51
        dgvLignes.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvLignes.Size = New Size(1115, 517)
        dgvLignes.TabIndex = 2
        ' 
        ' lblSomme
        ' 
        lblSomme.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblSomme.AutoSize = True
        lblSomme.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        lblSomme.Location = New Point(14, 892)
        lblSomme.Name = "lblSomme"
        lblSomme.Size = New Size(195, 20)
        lblSomme.TabIndex = 3
        lblSomme.Text = "Somme des lignes : 0 FCFA"
        ' 
        ' lblTotalSaisi
        ' 
        lblTotalSaisi.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblTotalSaisi.AutoSize = True
        lblTotalSaisi.Location = New Point(389, 892)
        lblTotalSaisi.Name = "lblTotalSaisi"
        lblTotalSaisi.Size = New Size(149, 20)
        lblTotalSaisi.TabIndex = 4
        lblTotalSaisi.Text = "Total du ticket (FCFA)"
        ' 
        ' txtTotalSaisi
        ' 
        txtTotalSaisi.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtTotalSaisi.Location = New Point(549, 886)
        txtTotalSaisi.Margin = New Padding(3, 4, 3, 4)
        txtTotalSaisi.Name = "txtTotalSaisi"
        txtTotalSaisi.Size = New Size(148, 27)
        txtTotalSaisi.TabIndex = 3
        ' 
        ' btnSupprimerLigne
        ' 
        btnSupprimerLigne.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnSupprimerLigne.FlatAppearance.BorderColor = Color.ForestGreen
        btnSupprimerLigne.FlatStyle = FlatStyle.Flat
        btnSupprimerLigne.ForeColor = Color.ForestGreen
        btnSupprimerLigne.Location = New Point(731, 878)
        btnSupprimerLigne.Margin = New Padding(3, 4, 3, 4)
        btnSupprimerLigne.Name = "btnSupprimerLigne"
        btnSupprimerLigne.Size = New Size(183, 45)
        btnSupprimerLigne.TabIndex = 4
        btnSupprimerLigne.Text = "Supprimer la ligne"
        btnSupprimerLigne.UseVisualStyleBackColor = True
        ' 
        ' btnEnregistrer
        ' 
        btnEnregistrer.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnEnregistrer.BackColor = Color.ForestGreen
        btnEnregistrer.FlatAppearance.BorderColor = Color.ForestGreen
        btnEnregistrer.FlatStyle = FlatStyle.Flat
        btnEnregistrer.ForeColor = Color.White
        btnEnregistrer.Location = New Point(926, 878)
        btnEnregistrer.Margin = New Padding(3, 4, 3, 4)
        btnEnregistrer.Name = "btnEnregistrer"
        btnEnregistrer.Size = New Size(203, 45)
        btnEnregistrer.TabIndex = 5
        btnEnregistrer.Text = "Enregistrer le ticket"
        btnEnregistrer.UseVisualStyleBackColor = False
        ' 
        ' errProvider
        ' 
        errProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink
        errProvider.ContainerControl = Me
        ' 
        ' FrmTickets
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1143, 944)
        Controls.Add(grpEntete)
        Controls.Add(grpLigne)
        Controls.Add(dgvLignes)
        Controls.Add(lblSomme)
        Controls.Add(lblTotalSaisi)
        Controls.Add(txtTotalSaisi)
        Controls.Add(btnSupprimerLigne)
        Controls.Add(btnEnregistrer)
        Controls.Add(pnlEntete)
        Font = New Font("Segoe UI", 9.0F)
        Margin = New Padding(3, 4, 3, 4)
        MinimumSize = New Size(1159, 823)
        Name = "FrmTickets"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Pricieux - Saisie d'un ticket"
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        grpEntete.ResumeLayout(False)
        grpEntete.PerformLayout()
        grpLigne.ResumeLayout(False)
        grpLigne.PerformLayout()
        CType(dgvLignes, ISupportInitialize).EndInit()
        CType(errProvider, ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents pnlEntete As Panel
    Friend WithEvents lblTitre As Label
    Friend WithEvents grpEntete As GroupBox
    Friend WithEvents lblMagasin As Label
    Friend WithEvents cmbMagasin As ComboBox
    Friend WithEvents lblDate As Label
    Friend WithEvents dtpDate As DateTimePicker
    Friend WithEvents grpLigne As GroupBox
    Friend WithEvents lblProduit As Label
    Friend WithEvents txtProduit As TextBox
    Friend WithEvents lblConditionnement As Label
    Friend WithEvents txtConditionnement As TextBox
    Friend WithEvents lblUnite As Label
    Friend WithEvents cmbUnite As ComboBox
    Friend WithEvents lblPrix As Label
    Friend WithEvents txtPrix As TextBox
    Friend WithEvents lblNombre As Label
    Friend WithEvents txtNombre As TextBox
    Friend WithEvents btnAjouter As Button
    Friend WithEvents lblProduitInfo As Label
    Friend WithEvents dgvLignes As DataGridView
    Friend WithEvents lblSomme As Label
    Friend WithEvents lblTotalSaisi As Label
    Friend WithEvents txtTotalSaisi As TextBox
    Friend WithEvents btnSupprimerLigne As Button
    Friend WithEvents btnEnregistrer As Button
    Friend WithEvents errProvider As ErrorProvider
    Friend WithEvents BtnOCR As Button
    Friend WithEvents LblInfo As Label
End Class