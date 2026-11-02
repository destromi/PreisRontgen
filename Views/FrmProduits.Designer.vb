<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmProduits
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
        Me.lblTitre = New System.Windows.Forms.Label()
        Me.lblRecherche = New System.Windows.Forms.Label()
        Me.txtRechercheCode = New System.Windows.Forms.TextBox()
        Me.btnRechercherOFF = New System.Windows.Forms.Button()
        Me.lblMarque = New System.Windows.Forms.Label()
        Me.txtNom = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.lblNom = New System.Windows.Forms.Label()
        Me.lblCategorie = New System.Windows.Forms.Label()
        Me.cmbCategorie = New System.Windows.Forms.ComboBox()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.lblUnite = New System.Windows.Forms.Label()
        Me.cmbUnite = New System.Windows.Forms.ComboBox()
        Me.txtCodeBarres = New System.Windows.Forms.TextBox()
        Me.btnEnregistrer = New System.Windows.Forms.Button()
        Me.btnActualiser = New System.Windows.Forms.Button()
        Me.dgvProduits = New System.Windows.Forms.DataGridView()
        CType(Me.dgvProduits, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitre
        '
        Me.lblTitre.AutoSize = True
        Me.lblTitre.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitre.Location = New System.Drawing.Point(30, 20)
        Me.lblTitre.Name = "lblTitre"
        Me.lblTitre.Size = New System.Drawing.Size(293, 32)
        Me.lblTitre.TabIndex = 0
        Me.lblTitre.Text = "GESTION DES PRODUITS"
        '
        'lblRecherche
        '
        Me.lblRecherche.AutoSize = True
        Me.lblRecherche.Location = New System.Drawing.Point(33, 67)
        Me.lblRecherche.Name = "lblRecherche"
        Me.lblRecherche.Size = New System.Drawing.Size(137, 13)
        Me.lblRecherche.TabIndex = 1
        Me.lblRecherche.Text = "Recherche par code-barres"
        '
        'txtRechercheCode
        '
        Me.txtRechercheCode.Location = New System.Drawing.Point(173, 64)
        Me.txtRechercheCode.Name = "txtRechercheCode"
        Me.txtRechercheCode.Size = New System.Drawing.Size(421, 20)
        Me.txtRechercheCode.TabIndex = 2
        '
        'btnRechercherOFF
        '
        Me.btnRechercherOFF.Location = New System.Drawing.Point(637, 62)
        Me.btnRechercherOFF.Name = "btnRechercherOFF"
        Me.btnRechercherOFF.Size = New System.Drawing.Size(75, 23)
        Me.btnRechercherOFF.TabIndex = 3
        Me.btnRechercherOFF.Text = "Rechercher sur Open Food Facts"
        Me.btnRechercherOFF.UseVisualStyleBackColor = True
        '
        'lblMarque
        '
        Me.lblMarque.AutoSize = True
        Me.lblMarque.Location = New System.Drawing.Point(211, 117)
        Me.lblMarque.Name = "lblMarque"
        Me.lblMarque.Size = New System.Drawing.Size(43, 13)
        Me.lblMarque.TabIndex = 4
        Me.lblMarque.Text = "Marque"
        '
        'txtNom
        '
        Me.txtNom.Location = New System.Drawing.Point(23, 133)
        Me.txtNom.Name = "txtNom"
        Me.txtNom.Size = New System.Drawing.Size(100, 20)
        Me.txtNom.TabIndex = 5
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(214, 133)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 20)
        Me.TextBox1.TabIndex = 5
        '
        'lblNom
        '
        Me.lblNom.AutoSize = True
        Me.lblNom.Location = New System.Drawing.Point(20, 117)
        Me.lblNom.Name = "lblNom"
        Me.lblNom.Size = New System.Drawing.Size(79, 13)
        Me.lblNom.TabIndex = 6
        Me.lblNom.Text = "Nom du produit"
        '
        'lblCategorie
        '
        Me.lblCategorie.AutoSize = True
        Me.lblCategorie.Location = New System.Drawing.Point(404, 117)
        Me.lblCategorie.Name = "lblCategorie"
        Me.lblCategorie.Size = New System.Drawing.Size(52, 13)
        Me.lblCategorie.TabIndex = 7
        Me.lblCategorie.Text = "Catégorie"
        '
        'cmbCategorie
        '
        Me.cmbCategorie.FormattingEnabled = True
        Me.cmbCategorie.Location = New System.Drawing.Point(407, 133)
        Me.cmbCategorie.Name = "cmbCategorie"
        Me.cmbCategorie.Size = New System.Drawing.Size(121, 21)
        Me.cmbCategorie.TabIndex = 8
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.Location = New System.Drawing.Point(867, 117)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(64, 13)
        Me.lblCode.TabIndex = 9
        Me.lblCode.Text = "Code-barres"
        '
        'lblUnite
        '
        Me.lblUnite.AutoSize = True
        Me.lblUnite.Location = New System.Drawing.Point(655, 117)
        Me.lblUnite.Name = "lblUnite"
        Me.lblUnite.Size = New System.Drawing.Size(73, 13)
        Me.lblUnite.TabIndex = 10
        Me.lblUnite.Text = "Unité de base"
        '
        'cmbUnite
        '
        Me.cmbUnite.FormattingEnabled = True
        Me.cmbUnite.Location = New System.Drawing.Point(658, 132)
        Me.cmbUnite.Name = "cmbUnite"
        Me.cmbUnite.Size = New System.Drawing.Size(121, 21)
        Me.cmbUnite.TabIndex = 11
        '
        'txtCodeBarres
        '
        Me.txtCodeBarres.Location = New System.Drawing.Point(870, 134)
        Me.txtCodeBarres.Name = "txtCodeBarres"
        Me.txtCodeBarres.Size = New System.Drawing.Size(100, 20)
        Me.txtCodeBarres.TabIndex = 12
        '
        'btnEnregistrer
        '
        Me.btnEnregistrer.BackColor = System.Drawing.Color.ForestGreen
        Me.btnEnregistrer.ForeColor = System.Drawing.Color.White
        Me.btnEnregistrer.Location = New System.Drawing.Point(944, 593)
        Me.btnEnregistrer.Name = "btnEnregistrer"
        Me.btnEnregistrer.Size = New System.Drawing.Size(128, 23)
        Me.btnEnregistrer.TabIndex = 13
        Me.btnEnregistrer.Text = "Enregistrer le produit"
        Me.btnEnregistrer.UseVisualStyleBackColor = False
        '
        'btnActualiser
        '
        Me.btnActualiser.BackColor = System.Drawing.Color.ForestGreen
        Me.btnActualiser.ForeColor = System.Drawing.Color.White
        Me.btnActualiser.Location = New System.Drawing.Point(944, 635)
        Me.btnActualiser.Name = "btnActualiser"
        Me.btnActualiser.Size = New System.Drawing.Size(128, 23)
        Me.btnActualiser.TabIndex = 14
        Me.btnActualiser.Text = "Actualiser la liste"
        Me.btnActualiser.UseVisualStyleBackColor = False
        '
        'dgvProduits
        '
        Me.dgvProduits.AllowUserToAddRows = False
        Me.dgvProduits.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvProduits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvProduits.Location = New System.Drawing.Point(1, 172)
        Me.dgvProduits.MultiSelect = False
        Me.dgvProduits.Name = "dgvProduits"
        Me.dgvProduits.ReadOnly = True
        Me.dgvProduits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvProduits.Size = New System.Drawing.Size(1071, 408)
        Me.dgvProduits.TabIndex = 15
        '
        'FrmProduits
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1084, 661)
        Me.Controls.Add(Me.lblCode)
        Me.Controls.Add(Me.txtCodeBarres)
        Me.Controls.Add(Me.lblUnite)
        Me.Controls.Add(Me.cmbUnite)
        Me.Controls.Add(Me.lblNom)
        Me.Controls.Add(Me.txtNom)
        Me.Controls.Add(Me.lblCategorie)
        Me.Controls.Add(Me.cmbCategorie)
        Me.Controls.Add(Me.lblMarque)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.dgvProduits)
        Me.Controls.Add(Me.btnActualiser)
        Me.Controls.Add(Me.btnEnregistrer)
        Me.Controls.Add(Me.btnRechercherOFF)
        Me.Controls.Add(Me.txtRechercheCode)
        Me.Controls.Add(Me.lblRecherche)
        Me.Controls.Add(Me.lblTitre)
        Me.Name = "FrmProduits"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pricieux-Gestion des produits"
        CType(Me.dgvProduits, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitre As Label
    Friend WithEvents lblRecherche As Label
    Friend WithEvents txtRechercheCode As TextBox
    Friend WithEvents btnRechercherOFF As Button
    Friend WithEvents lblMarque As Label
    Friend WithEvents txtNom As TextBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents lblNom As Label
    Friend WithEvents lblCategorie As Label
    Friend WithEvents cmbCategorie As ComboBox
    Friend WithEvents lblCode As Label
    Friend WithEvents lblUnite As Label
    Friend WithEvents cmbUnite As ComboBox
    Friend WithEvents txtCodeBarres As TextBox
    Friend WithEvents btnEnregistrer As Button
    Friend WithEvents btnActualiser As Button
    Friend WithEvents dgvProduits As DataGridView
End Class
