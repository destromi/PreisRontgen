Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Pt = System.Drawing.Point
Imports Sz = System.Drawing.Size
Imports Fnt = System.Drawing.Font

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

        CType(dgvLignes, ISupportInitialize).BeginInit()
        CType(errProvider, ISupportInitialize).BeginInit()
        pnlEntete.SuspendLayout()
        grpEntete.SuspendLayout()
        grpLigne.SuspendLayout()
        SuspendLayout()

        ' ----- Bandeau vert forêt -----
        With pnlEntete
            .Name = "pnlEntete"
            .BackColor = Color.FromArgb(34, 85, 51)
            .Dock = DockStyle.Top
            .Size = New Sz(1000, 50)
            .Controls.Add(lblTitre)
        End With
        With lblTitre
            .Name = "lblTitre"
            .AutoSize = True
            .Font = New Fnt("Segoe UI", 14.0F, FontStyle.Bold)
            .ForeColor = Color.White
            .Location = New Pt(16, 11)
            .Text = "Nouveau ticket"
        End With

        ' ----- En-tête du ticket -----
        With grpEntete
            .Name = "grpEntete"
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            .Location = New Pt(12, 62)
            .Size = New Sz(976, 64)
            .TabIndex = 0
            .TabStop = False
            .Text = "En-tête du ticket"
            .Controls.Add(lblMagasin)
            .Controls.Add(cmbMagasin)
            .Controls.Add(lblDate)
            .Controls.Add(dtpDate)
        End With
        With lblMagasin
            .Name = "lblMagasin"
            .AutoSize = True
            .Location = New Pt(16, 30)
            .Text = "Magasin"
        End With
        With cmbMagasin
            .Name = "cmbMagasin"
            .DropDownStyle = ComboBoxStyle.DropDownList
            .Location = New Pt(76, 26)
            .Size = New Sz(300, 23)
            .TabIndex = 0
        End With
        With lblDate
            .Name = "lblDate"
            .AutoSize = True
            .Location = New Pt(410, 30)
            .Text = "Date d'achat"
        End With
        With dtpDate
            .Name = "dtpDate"
            .Format = DateTimePickerFormat.Short
            .Location = New Pt(490, 26)
            .Size = New Sz(140, 23)
            .TabIndex = 1
        End With

        ' ----- Ajout d'un article -----
        With grpLigne
            .Name = "grpLigne"
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            .Location = New Pt(12, 134)
            .Size = New Sz(976, 118)
            .TabIndex = 1
            .TabStop = False
            .Text = "Ajout d'un article"
            .Controls.Add(lblProduit)
            .Controls.Add(txtProduit)
            .Controls.Add(lblConditionnement)
            .Controls.Add(txtConditionnement)
            .Controls.Add(lblUnite)
            .Controls.Add(cmbUnite)
            .Controls.Add(lblPrix)
            .Controls.Add(txtPrix)
            .Controls.Add(lblNombre)
            .Controls.Add(txtNombre)
            .Controls.Add(btnAjouter)
            .Controls.Add(lblProduitInfo)
        End With
        With lblProduit
            .Name = "lblProduit"
            .AutoSize = True
            .Location = New Pt(16, 26)
            .Text = "Produit"
        End With
        With txtProduit
            .Name = "txtProduit"
            .Location = New Pt(16, 46)
            .Size = New Sz(300, 23)
            .TabIndex = 0
        End With
        With lblConditionnement
            .Name = "lblConditionnement"
            .AutoSize = True
            .Location = New Pt(330, 26)
            .Text = "Conditionnement"
        End With
        With txtConditionnement
            .Name = "txtConditionnement"
            .Location = New Pt(330, 46)
            .Size = New Sz(110, 23)
            .TabIndex = 1
        End With
        With lblUnite
            .Name = "lblUnite"
            .AutoSize = True
            .Location = New Pt(452, 26)
            .Text = "Unité"
        End With
        With cmbUnite
            .Name = "cmbUnite"
            .DropDownStyle = ComboBoxStyle.DropDownList
            .Location = New Pt(452, 46)
            .Size = New Sz(90, 23)
            .TabIndex = 2
        End With
        With lblPrix
            .Name = "lblPrix"
            .AutoSize = True
            .Location = New Pt(554, 26)
            .Text = "Prix du paquet (FCFA)"
        End With
        With txtPrix
            .Name = "txtPrix"
            .Location = New Pt(554, 46)
            .Size = New Sz(130, 23)
            .TabIndex = 3
        End With
        With lblNombre
            .Name = "lblNombre"
            .AutoSize = True
            .Location = New Pt(696, 26)
            .Text = "Nombre acheté"
        End With
        With txtNombre
            .Name = "txtNombre"
            .Location = New Pt(696, 46)
            .Size = New Sz(90, 23)
            .TabIndex = 4
            .Text = "1"
        End With
        With btnAjouter
            .Name = "btnAjouter"
            .Location = New Pt(800, 42)
            .Size = New Sz(160, 30)
            .TabIndex = 5
            .Text = "Ajouter une ligne"
            .UseVisualStyleBackColor = True
        End With
        With lblProduitInfo
            .Name = "lblProduitInfo"
            .AutoSize = False
            .ForeColor = Color.FromArgb(34, 85, 51)
            .Location = New Pt(16, 82)
            .Size = New Sz(600, 22)
            .Text = ""
        End With

        ' ----- Grille des lignes -----
        With dgvLignes
            .Name = "dgvLignes"
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .RowHeadersVisible = False
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
            .Location = New Pt(12, 262)
            .Size = New Sz(976, 270)
            .TabIndex = 2
        End With

        ' ----- Pied : totaux et boutons -----
        With lblSomme
            .Name = "lblSomme"
            .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
            .AutoSize = True
            .Font = New Fnt("Segoe UI", 9.0F, FontStyle.Bold)
            .Location = New Pt(12, 551)
            .Text = "Somme des lignes : 0 FCFA"
        End With
        With lblTotalSaisi
            .Name = "lblTotalSaisi"
            .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
            .AutoSize = True
            .Location = New Pt(340, 551)
            .Text = "Total du ticket (FCFA)"
        End With
        With txtTotalSaisi
            .Name = "txtTotalSaisi"
            .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
            .Location = New Pt(480, 547)
            .Size = New Sz(130, 23)
            .TabIndex = 3
        End With
        With btnSupprimerLigne
            .Name = "btnSupprimerLigne"
            .Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
            .Location = New Pt(640, 541)
            .Size = New Sz(160, 34)
            .TabIndex = 4
            .Text = "Supprimer la ligne"
            .UseVisualStyleBackColor = True
        End With
        With btnEnregistrer
            .Name = "btnEnregistrer"
            .Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
            .Location = New Pt(810, 541)
            .Size = New Sz(178, 34)
            .TabIndex = 5
            .Text = "Enregistrer le ticket"
            .UseVisualStyleBackColor = True
        End With

        With errProvider
            .ContainerControl = Me
            .BlinkStyle = ErrorBlinkStyle.NeverBlink
        End With

        ' ----- Formulaire -----
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New Sz(1000, 590)
        Me.Controls.Add(grpEntete)
        Me.Controls.Add(grpLigne)
        Me.Controls.Add(dgvLignes)
        Me.Controls.Add(lblSomme)
        Me.Controls.Add(lblTotalSaisi)
        Me.Controls.Add(txtTotalSaisi)
        Me.Controls.Add(btnSupprimerLigne)
        Me.Controls.Add(btnEnregistrer)
        Me.Controls.Add(pnlEntete)
        Me.Font = New Fnt("Segoe UI", 9.0F)
        Me.MinimumSize = New Sz(1016, 629)
        Me.Name = "FrmTickets"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "Pricieux - Saisie d'un ticket"

        CType(dgvLignes, ISupportInitialize).EndInit()
        CType(errProvider, ISupportInitialize).EndInit()
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        grpEntete.ResumeLayout(False)
        grpEntete.PerformLayout()
        grpLigne.ResumeLayout(False)
        grpLigne.PerformLayout()
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
End Class