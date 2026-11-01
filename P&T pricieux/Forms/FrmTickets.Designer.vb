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
        Me.lblMagasin = New System.Windows.Forms.Label()
        Me.lblDateAchat = New System.Windows.Forms.Label()
        Me.cmbMagasin = New System.Windows.Forms.ComboBox()
        Me.dtpDataAchat = New System.Windows.Forms.DateTimePicker()
        Me.lblNumeroTicket = New System.Windows.Forms.Label()
        Me.txtNumeroTicket = New System.Windows.Forms.TextBox()
        Me.lblProduit = New System.Windows.Forms.Label()
        Me.cmbProduit = New System.Windows.Forms.ComboBox()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.nudNombre = New System.Windows.Forms.NumericUpDown()
        Me.lblUnite = New System.Windows.Forms.Label()
        Me.txtQuantite = New System.Windows.Forms.TextBox()
        Me.cmbUnite = New System.Windows.Forms.ComboBox()
        Me.lblPrix = New System.Windows.Forms.Label()
        Me.txtPrix = New System.Windows.Forms.TextBox()
        Me.btnAjouterProduit = New System.Windows.Forms.Button()
        Me.dgvLignesTickets = New System.Windows.Forms.DataGridView()
        Me.lblTotalTicket = New System.Windows.Forms.Label()
        Me.TotalSaisi = New System.Windows.Forms.TextBox()
        Me.btnEnregistrerTicket = New System.Windows.Forms.Button()
        Me.btnVider = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.nudNombre, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvLignesTickets, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblMagasin
        '
        Me.lblMagasin.AutoSize = True
        Me.lblMagasin.Location = New System.Drawing.Point(12, 28)
        Me.lblMagasin.Name = "lblMagasin"
        Me.lblMagasin.Size = New System.Drawing.Size(47, 13)
        Me.lblMagasin.TabIndex = 0
        Me.lblMagasin.Text = "Magasin"
        '
        'lblDateAchat
        '
        Me.lblDateAchat.AutoSize = True
        Me.lblDateAchat.Location = New System.Drawing.Point(186, 28)
        Me.lblDateAchat.Name = "lblDateAchat"
        Me.lblDateAchat.Size = New System.Drawing.Size(58, 13)
        Me.lblDateAchat.TabIndex = 1
        Me.lblDateAchat.Text = "DateAchat"
        '
        'cmbMagasin
        '
        Me.cmbMagasin.FormattingEnabled = True
        Me.cmbMagasin.Location = New System.Drawing.Point(12, 44)
        Me.cmbMagasin.Name = "cmbMagasin"
        Me.cmbMagasin.Size = New System.Drawing.Size(121, 21)
        Me.cmbMagasin.TabIndex = 2
        '
        'dtpDataAchat
        '
        Me.dtpDataAchat.Location = New System.Drawing.Point(189, 45)
        Me.dtpDataAchat.Name = "dtpDataAchat"
        Me.dtpDataAchat.Size = New System.Drawing.Size(134, 20)
        Me.dtpDataAchat.TabIndex = 3
        '
        'lblNumeroTicket
        '
        Me.lblNumeroTicket.AutoSize = True
        Me.lblNumeroTicket.Location = New System.Drawing.Point(377, 28)
        Me.lblNumeroTicket.Name = "lblNumeroTicket"
        Me.lblNumeroTicket.Size = New System.Drawing.Size(74, 13)
        Me.lblNumeroTicket.TabIndex = 4
        Me.lblNumeroTicket.Text = "NumeroTicket"
        '
        'txtNumeroTicket
        '
        Me.txtNumeroTicket.Location = New System.Drawing.Point(380, 48)
        Me.txtNumeroTicket.Name = "txtNumeroTicket"
        Me.txtNumeroTicket.Size = New System.Drawing.Size(100, 20)
        Me.txtNumeroTicket.TabIndex = 5
        '
        'lblProduit
        '
        Me.lblProduit.AutoSize = True
        Me.lblProduit.Location = New System.Drawing.Point(19, 113)
        Me.lblProduit.Name = "lblProduit"
        Me.lblProduit.Size = New System.Drawing.Size(40, 13)
        Me.lblProduit.TabIndex = 6
        Me.lblProduit.Text = "Produit"
        '
        'cmbProduit
        '
        Me.cmbProduit.FormattingEnabled = True
        Me.cmbProduit.Location = New System.Drawing.Point(15, 129)
        Me.cmbProduit.Name = "cmbProduit"
        Me.cmbProduit.Size = New System.Drawing.Size(130, 21)
        Me.cmbProduit.TabIndex = 7
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Location = New System.Drawing.Point(261, 117)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(44, 13)
        Me.lblNombre.TabIndex = 6
        Me.lblNombre.Text = "Nombre"
        '
        'nudNombre
        '
        Me.nudNombre.Location = New System.Drawing.Point(264, 133)
        Me.nudNombre.Name = "nudNombre"
        Me.nudNombre.Size = New System.Drawing.Size(135, 20)
        Me.nudNombre.TabIndex = 8
        '
        'lblUnite
        '
        Me.lblUnite.AutoSize = True
        Me.lblUnite.Location = New System.Drawing.Point(735, 117)
        Me.lblUnite.Name = "lblUnite"
        Me.lblUnite.Size = New System.Drawing.Size(32, 13)
        Me.lblUnite.TabIndex = 6
        Me.lblUnite.Text = "Unite"
        '
        'txtQuantite
        '
        Me.txtQuantite.Location = New System.Drawing.Point(496, 133)
        Me.txtQuantite.Name = "txtQuantite"
        Me.txtQuantite.Size = New System.Drawing.Size(125, 20)
        Me.txtQuantite.TabIndex = 9
        '
        'cmbUnite
        '
        Me.cmbUnite.FormattingEnabled = True
        Me.cmbUnite.Location = New System.Drawing.Point(738, 133)
        Me.cmbUnite.Name = "cmbUnite"
        Me.cmbUnite.Size = New System.Drawing.Size(121, 21)
        Me.cmbUnite.TabIndex = 10
        '
        'lblPrix
        '
        Me.lblPrix.AutoSize = True
        Me.lblPrix.Location = New System.Drawing.Point(979, 117)
        Me.lblPrix.Name = "lblPrix"
        Me.lblPrix.Size = New System.Drawing.Size(24, 13)
        Me.lblPrix.TabIndex = 6
        Me.lblPrix.Text = "Prix"
        '
        'txtPrix
        '
        Me.txtPrix.Location = New System.Drawing.Point(982, 134)
        Me.txtPrix.Name = "txtPrix"
        Me.txtPrix.Size = New System.Drawing.Size(127, 20)
        Me.txtPrix.TabIndex = 11
        '
        'btnAjouterProduit
        '
        Me.btnAjouterProduit.BackColor = System.Drawing.Color.ForestGreen
        Me.btnAjouterProduit.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAjouterProduit.Location = New System.Drawing.Point(1040, 676)
        Me.btnAjouterProduit.Name = "btnAjouterProduit"
        Me.btnAjouterProduit.Size = New System.Drawing.Size(130, 33)
        Me.btnAjouterProduit.TabIndex = 12
        Me.btnAjouterProduit.Text = "Ajouter Produit"
        Me.btnAjouterProduit.UseVisualStyleBackColor = False
        '
        'dgvLignesTickets
        '
        Me.dgvLignesTickets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLignesTickets.Location = New System.Drawing.Point(12, 157)
        Me.dgvLignesTickets.Name = "dgvLignesTickets"
        Me.dgvLignesTickets.Size = New System.Drawing.Size(1160, 457)
        Me.dgvLignesTickets.TabIndex = 13
        '
        'lblTotalTicket
        '
        Me.lblTotalTicket.AutoSize = True
        Me.lblTotalTicket.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalTicket.Location = New System.Drawing.Point(54, 663)
        Me.lblTotalTicket.Name = "lblTotalTicket"
        Me.lblTotalTicket.Size = New System.Drawing.Size(76, 17)
        Me.lblTotalTicket.TabIndex = 14
        Me.lblTotalTicket.Text = "TotalTicket"
        '
        'TotalSaisi
        '
        Me.TotalSaisi.Location = New System.Drawing.Point(57, 679)
        Me.TotalSaisi.Name = "TotalSaisi"
        Me.TotalSaisi.Size = New System.Drawing.Size(130, 20)
        Me.TotalSaisi.TabIndex = 15
        '
        'btnEnregistrerTicket
        '
        Me.btnEnregistrerTicket.BackColor = System.Drawing.Color.ForestGreen
        Me.btnEnregistrerTicket.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEnregistrerTicket.Location = New System.Drawing.Point(1040, 635)
        Me.btnEnregistrerTicket.Name = "btnEnregistrerTicket"
        Me.btnEnregistrerTicket.Size = New System.Drawing.Size(130, 35)
        Me.btnEnregistrerTicket.TabIndex = 16
        Me.btnEnregistrerTicket.Text = "EnregistrerTicket"
        Me.btnEnregistrerTicket.UseVisualStyleBackColor = False
        '
        'btnVider
        '
        Me.btnVider.BackColor = System.Drawing.Color.ForestGreen
        Me.btnVider.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnVider.Location = New System.Drawing.Point(843, 676)
        Me.btnVider.Name = "btnVider"
        Me.btnVider.Size = New System.Drawing.Size(145, 33)
        Me.btnVider.TabIndex = 16
        Me.btnVider.Text = "Vider le formulaire"
        Me.btnVider.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(493, 117)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Quantite"
        '
        'FrmTickets
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1184, 711)
        Me.Controls.Add(Me.btnVider)
        Me.Controls.Add(Me.btnEnregistrerTicket)
        Me.Controls.Add(Me.TotalSaisi)
        Me.Controls.Add(Me.lblTotalTicket)
        Me.Controls.Add(Me.dgvLignesTickets)
        Me.Controls.Add(Me.btnAjouterProduit)
        Me.Controls.Add(Me.txtPrix)
        Me.Controls.Add(Me.cmbUnite)
        Me.Controls.Add(Me.txtQuantite)
        Me.Controls.Add(Me.nudNombre)
        Me.Controls.Add(Me.cmbProduit)
        Me.Controls.Add(Me.lblPrix)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblUnite)
        Me.Controls.Add(Me.lblNombre)
        Me.Controls.Add(Me.lblProduit)
        Me.Controls.Add(Me.txtNumeroTicket)
        Me.Controls.Add(Me.lblNumeroTicket)
        Me.Controls.Add(Me.dtpDataAchat)
        Me.Controls.Add(Me.cmbMagasin)
        Me.Controls.Add(Me.lblDateAchat)
        Me.Controls.Add(Me.lblMagasin)
        Me.Name = "FrmTickets"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tickets"
        CType(Me.nudNombre, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvLignesTickets, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
