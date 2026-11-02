<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmTickets
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        lblMagasin = New Label()
        lblDateAchat = New Label()
        cmbMagasin = New ComboBox()
        dtpDataAchat = New DateTimePicker()
        lblNumeroTicket = New Label()
        txtNumeroTicket = New TextBox()
        lblProduit = New Label()
        cmbProduit = New ComboBox()
        lblNombre = New Label()
        nudNombre = New NumericUpDown()
        lblUnite = New Label()
        txtQuantite = New TextBox()
        cmbUnite = New ComboBox()
        lblPrix = New Label()
        txtPrix = New TextBox()
        btnAjouterProduit = New Button()
        dgvLignesTickets = New DataGridView()
        lblTotalTicket = New Label()
        TotalSaisi = New TextBox()
        btnEnregistrerTicket = New Button()
        btnVider = New Button()
        Label1 = New Label()
        CType(nudNombre, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvLignesTickets, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblMagasin
        ' 
        lblMagasin.AutoSize = True
        lblMagasin.Location = New Point(16, 43)
        lblMagasin.Margin = New Padding(4, 0, 4, 0)
        lblMagasin.Name = "lblMagasin"
        lblMagasin.Size = New Size(65, 20)
        lblMagasin.TabIndex = 0
        lblMagasin.Text = "Magasin"
        ' 
        ' lblDateAchat
        ' 
        lblDateAchat.AutoSize = True
        lblDateAchat.Location = New Point(248, 43)
        lblDateAchat.Margin = New Padding(4, 0, 4, 0)
        lblDateAchat.Name = "lblDateAchat"
        lblDateAchat.Size = New Size(79, 20)
        lblDateAchat.TabIndex = 1
        lblDateAchat.Text = "DateAchat"
        ' 
        ' cmbMagasin
        ' 
        cmbMagasin.FormattingEnabled = True
        cmbMagasin.Location = New Point(16, 68)
        cmbMagasin.Margin = New Padding(4, 5, 4, 5)
        cmbMagasin.Name = "cmbMagasin"
        cmbMagasin.Size = New Size(160, 28)
        cmbMagasin.TabIndex = 2
        ' 
        ' dtpDataAchat
        ' 
        dtpDataAchat.Location = New Point(252, 69)
        dtpDataAchat.Margin = New Padding(4, 5, 4, 5)
        dtpDataAchat.Name = "dtpDataAchat"
        dtpDataAchat.Size = New Size(177, 27)
        dtpDataAchat.TabIndex = 3
        ' 
        ' lblNumeroTicket
        ' 
        lblNumeroTicket.AutoSize = True
        lblNumeroTicket.Location = New Point(503, 43)
        lblNumeroTicket.Margin = New Padding(4, 0, 4, 0)
        lblNumeroTicket.Name = "lblNumeroTicket"
        lblNumeroTicket.Size = New Size(102, 20)
        lblNumeroTicket.TabIndex = 4
        lblNumeroTicket.Text = "NumeroTicket"
        ' 
        ' txtNumeroTicket
        ' 
        txtNumeroTicket.Location = New Point(507, 74)
        txtNumeroTicket.Margin = New Padding(4, 5, 4, 5)
        txtNumeroTicket.Name = "txtNumeroTicket"
        txtNumeroTicket.Size = New Size(132, 27)
        txtNumeroTicket.TabIndex = 5
        ' 
        ' lblProduit
        ' 
        lblProduit.AutoSize = True
        lblProduit.Location = New Point(25, 174)
        lblProduit.Margin = New Padding(4, 0, 4, 0)
        lblProduit.Name = "lblProduit"
        lblProduit.Size = New Size(57, 20)
        lblProduit.TabIndex = 6
        lblProduit.Text = "Produit"
        ' 
        ' cmbProduit
        ' 
        cmbProduit.FormattingEnabled = True
        cmbProduit.Location = New Point(20, 198)
        cmbProduit.Margin = New Padding(4, 5, 4, 5)
        cmbProduit.Name = "cmbProduit"
        cmbProduit.Size = New Size(172, 28)
        cmbProduit.TabIndex = 7
        ' 
        ' lblNombre
        ' 
        lblNombre.AutoSize = True
        lblNombre.Location = New Point(348, 180)
        lblNombre.Margin = New Padding(4, 0, 4, 0)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(64, 20)
        lblNombre.TabIndex = 6
        lblNombre.Text = "Nombre"
        ' 
        ' nudNombre
        ' 
        nudNombre.Location = New Point(352, 205)
        nudNombre.Margin = New Padding(4, 5, 4, 5)
        nudNombre.Name = "nudNombre"
        nudNombre.Size = New Size(180, 27)
        nudNombre.TabIndex = 8
        ' 
        ' lblUnite
        ' 
        lblUnite.AutoSize = True
        lblUnite.Location = New Point(980, 180)
        lblUnite.Margin = New Padding(4, 0, 4, 0)
        lblUnite.Name = "lblUnite"
        lblUnite.Size = New Size(44, 20)
        lblUnite.TabIndex = 6
        lblUnite.Text = "Unite"
        ' 
        ' txtQuantite
        ' 
        txtQuantite.Location = New Point(661, 205)
        txtQuantite.Margin = New Padding(4, 5, 4, 5)
        txtQuantite.Name = "txtQuantite"
        txtQuantite.Size = New Size(165, 27)
        txtQuantite.TabIndex = 9
        ' 
        ' cmbUnite
        ' 
        cmbUnite.FormattingEnabled = True
        cmbUnite.Location = New Point(984, 205)
        cmbUnite.Margin = New Padding(4, 5, 4, 5)
        cmbUnite.Name = "cmbUnite"
        cmbUnite.Size = New Size(160, 28)
        cmbUnite.TabIndex = 10
        ' 
        ' lblPrix
        ' 
        lblPrix.AutoSize = True
        lblPrix.Location = New Point(1305, 180)
        lblPrix.Margin = New Padding(4, 0, 4, 0)
        lblPrix.Name = "lblPrix"
        lblPrix.Size = New Size(33, 20)
        lblPrix.TabIndex = 6
        lblPrix.Text = "Prix"
        ' 
        ' txtPrix
        ' 
        txtPrix.Location = New Point(1309, 206)
        txtPrix.Margin = New Padding(4, 5, 4, 5)
        txtPrix.Name = "txtPrix"
        txtPrix.Size = New Size(168, 27)
        txtPrix.TabIndex = 11
        ' 
        ' btnAjouterProduit
        ' 
        btnAjouterProduit.BackColor = Color.ForestGreen
        btnAjouterProduit.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnAjouterProduit.Location = New Point(1387, 1040)
        btnAjouterProduit.Margin = New Padding(4, 5, 4, 5)
        btnAjouterProduit.Name = "btnAjouterProduit"
        btnAjouterProduit.Size = New Size(173, 51)
        btnAjouterProduit.TabIndex = 12
        btnAjouterProduit.Text = "Ajouter Produit"
        btnAjouterProduit.UseVisualStyleBackColor = False
        ' 
        ' dgvLignesTickets
        ' 
        dgvLignesTickets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvLignesTickets.Location = New Point(16, 242)
        dgvLignesTickets.Margin = New Padding(4, 5, 4, 5)
        dgvLignesTickets.Name = "dgvLignesTickets"
        dgvLignesTickets.RowHeadersWidth = 51
        dgvLignesTickets.Size = New Size(1547, 703)
        dgvLignesTickets.TabIndex = 13
        ' 
        ' lblTotalTicket
        ' 
        lblTotalTicket.AutoSize = True
        lblTotalTicket.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalTicket.Location = New Point(72, 1020)
        lblTotalTicket.Margin = New Padding(4, 0, 4, 0)
        lblTotalTicket.Name = "lblTotalTicket"
        lblTotalTicket.Size = New Size(98, 23)
        lblTotalTicket.TabIndex = 14
        lblTotalTicket.Text = "TotalTicket"
        ' 
        ' TotalSaisi
        ' 
        TotalSaisi.Location = New Point(76, 1045)
        TotalSaisi.Margin = New Padding(4, 5, 4, 5)
        TotalSaisi.Name = "TotalSaisi"
        TotalSaisi.Size = New Size(172, 27)
        TotalSaisi.TabIndex = 15
        ' 
        ' btnEnregistrerTicket
        ' 
        btnEnregistrerTicket.BackColor = Color.ForestGreen
        btnEnregistrerTicket.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnEnregistrerTicket.Location = New Point(1387, 977)
        btnEnregistrerTicket.Margin = New Padding(4, 5, 4, 5)
        btnEnregistrerTicket.Name = "btnEnregistrerTicket"
        btnEnregistrerTicket.Size = New Size(173, 54)
        btnEnregistrerTicket.TabIndex = 16
        btnEnregistrerTicket.Text = "EnregistrerTicket"
        btnEnregistrerTicket.UseVisualStyleBackColor = False
        ' 
        ' btnVider
        ' 
        btnVider.BackColor = Color.ForestGreen
        btnVider.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnVider.Location = New Point(1124, 1040)
        btnVider.Margin = New Padding(4, 5, 4, 5)
        btnVider.Name = "btnVider"
        btnVider.Size = New Size(193, 51)
        btnVider.TabIndex = 16
        btnVider.Text = "Vider le formulaire"
        btnVider.UseVisualStyleBackColor = False
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(657, 180)
        Label1.Margin = New Padding(4, 0, 4, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(66, 20)
        Label1.TabIndex = 6
        Label1.Text = "Quantite"
        ' 
        ' FrmTickets
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1579, 1055)
        Controls.Add(btnVider)
        Controls.Add(btnEnregistrerTicket)
        Controls.Add(TotalSaisi)
        Controls.Add(lblTotalTicket)
        Controls.Add(dgvLignesTickets)
        Controls.Add(btnAjouterProduit)
        Controls.Add(txtPrix)
        Controls.Add(cmbUnite)
        Controls.Add(txtQuantite)
        Controls.Add(nudNombre)
        Controls.Add(cmbProduit)
        Controls.Add(lblPrix)
        Controls.Add(Label1)
        Controls.Add(lblUnite)
        Controls.Add(lblNombre)
        Controls.Add(lblProduit)
        Controls.Add(txtNumeroTicket)
        Controls.Add(lblNumeroTicket)
        Controls.Add(dtpDataAchat)
        Controls.Add(cmbMagasin)
        Controls.Add(lblDateAchat)
        Controls.Add(lblMagasin)
        Margin = New Padding(4, 5, 4, 5)
        Name = "FrmTickets"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Tickets"
        CType(nudNombre, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvLignesTickets, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lblMagasin As Label
    Friend WithEvents lblDateAchat As Label
    Friend WithEvents cmbMagasin As ComboBox
    Friend WithEvents dtpDataAchat As DateTimePicker
    Friend WithEvents lblNumeroTicket As Label
    Friend WithEvents txtNumeroTicket As TextBox
    Friend WithEvents lblProduit As Label
    Friend WithEvents cmbProduit As ComboBox
    Friend WithEvents lblNombre As Label
    Friend WithEvents nudNombre As NumericUpDown
    Friend WithEvents lblUnite As Label
    Friend WithEvents txtQuantite As TextBox
    Friend WithEvents cmbUnite As ComboBox
    Friend WithEvents lblPrix As Label
    Friend WithEvents txtPrix As TextBox
    Friend WithEvents btnAjouterProduit As Button
    Friend WithEvents dgvLignesTickets As DataGridView
    Friend WithEvents lblTotalTicket As Label
    Friend WithEvents TotalSaisi As TextBox
    Friend WithEvents btnEnregistrerTicket As Button
    Friend WithEvents btnVider As Button
    Friend WithEvents Label1 As Label
End Class
