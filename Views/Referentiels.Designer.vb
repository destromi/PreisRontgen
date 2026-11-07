<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Referentiels
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
        Label1 = New Label()
        TabControls1 = New TabControl()
        TabMagasins = New TabPage()
        SplitContainer1 = New SplitContainer()
        ListBoxMagasins = New ListBox()
        Label3 = New Label()
        BtnDesactiver = New Button()
        BtnNouveau = New Button()
        BtnEnregistrer = New Button()
        TextBoxMagasin = New TextBox()
        Label2 = New Label()
        TabCategories = New TabPage()
        SplitContainer2 = New SplitContainer()
        ListBoxCateg = New ListBox()
        BtnDelCateg = New Button()
        Label4 = New Label()
        BtnNewCateg = New Button()
        TextBoxCateg = New TextBox()
        BtnEnregistrerCateg = New Button()
        TabProduits = New TabPage()
        TabControls1.SuspendLayout()
        TabMagasins.SuspendLayout()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        TabCategories.SuspendLayout()
        CType(SplitContainer2, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer2.Panel1.SuspendLayout()
        SplitContainer2.Panel2.SuspendLayout()
        SplitContainer2.SuspendLayout()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.BackColor = SystemColors.Control
        Label1.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.ForeColor = Color.ForestGreen
        Label1.Location = New Point(27, 22)
        Label1.Name = "Label1"
        Label1.Size = New Size(442, 38)
        Label1.TabIndex = 0
        Label1.Text = "Magasins, catégories et produits"
        ' 
        ' TabControls1
        ' 
        TabControls1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        TabControls1.Controls.Add(TabMagasins)
        TabControls1.Controls.Add(TabCategories)
        TabControls1.Controls.Add(TabProduits)
        TabControls1.Location = New Point(27, 77)
        TabControls1.Name = "TabControls1"
        TabControls1.SelectedIndex = 0
        TabControls1.Size = New Size(1392, 614)
        TabControls1.TabIndex = 1
        ' 
        ' TabMagasins
        ' 
        TabMagasins.BackColor = Color.White
        TabMagasins.Controls.Add(SplitContainer1)
        TabMagasins.Location = New Point(4, 29)
        TabMagasins.Margin = New Padding(2, 0, 2, 0)
        TabMagasins.Name = "TabMagasins"
        TabMagasins.Padding = New Padding(10, 6, 10, 6)
        TabMagasins.Size = New Size(1384, 581)
        TabMagasins.TabIndex = 0
        TabMagasins.Text = "Magasins"
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.Location = New Point(10, 6)
        SplitContainer1.Name = "SplitContainer1"
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(ListBoxMagasins)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.Controls.Add(Label3)
        SplitContainer1.Panel2.Controls.Add(BtnDesactiver)
        SplitContainer1.Panel2.Controls.Add(BtnNouveau)
        SplitContainer1.Panel2.Controls.Add(BtnEnregistrer)
        SplitContainer1.Panel2.Controls.Add(TextBoxMagasin)
        SplitContainer1.Panel2.Controls.Add(Label2)
        SplitContainer1.Size = New Size(1364, 569)
        SplitContainer1.SplitterDistance = 454
        SplitContainer1.TabIndex = 0
        ' 
        ' ListBoxMagasins
        ' 
        ListBoxMagasins.BorderStyle = BorderStyle.FixedSingle
        ListBoxMagasins.DisplayMember = "nom"
        ListBoxMagasins.Dock = DockStyle.Fill
        ListBoxMagasins.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        ListBoxMagasins.FormattingEnabled = True
        ListBoxMagasins.IntegralHeight = False
        ListBoxMagasins.ItemHeight = 23
        ListBoxMagasins.Location = New Point(0, 0)
        ListBoxMagasins.Margin = New Padding(10, 0, 10, 0)
        ListBoxMagasins.Name = "ListBoxMagasins"
        ListBoxMagasins.Size = New Size(454, 569)
        ListBoxMagasins.TabIndex = 0
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label3.ForeColor = Color.Gray
        Label3.Location = New Point(43, 189)
        Label3.Name = "Label3"
        Label3.Size = New Size(459, 20)
        Label3.TabIndex = 4
        Label3.Text = "Un magasin déjà utilisé est désactivé afin de préserver l'historique"
        ' 
        ' BtnDesactiver
        ' 
        BtnDesactiver.BackColor = Color.White
        BtnDesactiver.FlatAppearance.BorderColor = Color.ForestGreen
        BtnDesactiver.FlatStyle = FlatStyle.Flat
        BtnDesactiver.Font = New Font("Segoe UI", 10F)
        BtnDesactiver.ForeColor = Color.ForestGreen
        BtnDesactiver.Location = New Point(327, 116)
        BtnDesactiver.Name = "BtnDesactiver"
        BtnDesactiver.Size = New Size(114, 38)
        BtnDesactiver.TabIndex = 3
        BtnDesactiver.Text = "Désactiver"
        BtnDesactiver.UseVisualStyleBackColor = False
        ' 
        ' BtnNouveau
        ' 
        BtnNouveau.BackColor = Color.White
        BtnNouveau.FlatAppearance.BorderColor = Color.ForestGreen
        BtnNouveau.FlatStyle = FlatStyle.Flat
        BtnNouveau.Font = New Font("Segoe UI", 10F)
        BtnNouveau.ForeColor = Color.ForestGreen
        BtnNouveau.Location = New Point(202, 116)
        BtnNouveau.Name = "BtnNouveau"
        BtnNouveau.Size = New Size(94, 38)
        BtnNouveau.TabIndex = 3
        BtnNouveau.Text = "Nouveau"
        BtnNouveau.UseVisualStyleBackColor = False
        ' 
        ' BtnEnregistrer
        ' 
        BtnEnregistrer.BackColor = Color.ForestGreen
        BtnEnregistrer.FlatStyle = FlatStyle.Flat
        BtnEnregistrer.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnEnregistrer.ForeColor = Color.White
        BtnEnregistrer.Location = New Point(43, 116)
        BtnEnregistrer.Name = "BtnEnregistrer"
        BtnEnregistrer.Size = New Size(129, 38)
        BtnEnregistrer.TabIndex = 2
        BtnEnregistrer.Text = "Enregistrer"
        BtnEnregistrer.UseVisualStyleBackColor = False
        ' 
        ' TextBoxMagasin
        ' 
        TextBoxMagasin.Font = New Font("Segoe UI", 11F)
        TextBoxMagasin.Location = New Point(43, 56)
        TextBoxMagasin.Name = "TextBoxMagasin"
        TextBoxMagasin.Size = New Size(451, 32)
        TextBoxMagasin.TabIndex = 1
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(43, 22)
        Label2.Name = "Label2"
        Label2.Size = New Size(129, 20)
        Label2.TabIndex = 0
        Label2.Text = "Nom du magasin"
        ' 
        ' TabCategories
        ' 
        TabCategories.Controls.Add(SplitContainer2)
        TabCategories.Location = New Point(4, 29)
        TabCategories.Name = "TabCategories"
        TabCategories.Padding = New Padding(3)
        TabCategories.Size = New Size(1384, 581)
        TabCategories.TabIndex = 1
        TabCategories.Text = "Catégories"
        TabCategories.UseVisualStyleBackColor = True
        ' 
        ' SplitContainer2
        ' 
        SplitContainer2.Dock = DockStyle.Fill
        SplitContainer2.Location = New Point(3, 3)
        SplitContainer2.Name = "SplitContainer2"
        ' 
        ' SplitContainer2.Panel1
        ' 
        SplitContainer2.Panel1.Controls.Add(ListBoxCateg)
        ' 
        ' SplitContainer2.Panel2
        ' 
        SplitContainer2.Panel2.Controls.Add(BtnDelCateg)
        SplitContainer2.Panel2.Controls.Add(Label4)
        SplitContainer2.Panel2.Controls.Add(BtnNewCateg)
        SplitContainer2.Panel2.Controls.Add(TextBoxCateg)
        SplitContainer2.Panel2.Controls.Add(BtnEnregistrerCateg)
        SplitContainer2.Size = New Size(1378, 575)
        SplitContainer2.SplitterDistance = 459
        SplitContainer2.TabIndex = 0
        ' 
        ' ListBoxCateg
        ' 
        ListBoxCateg.BorderStyle = BorderStyle.FixedSingle
        ListBoxCateg.DisplayMember = "nom"
        ListBoxCateg.Dock = DockStyle.Fill
        ListBoxCateg.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        ListBoxCateg.FormattingEnabled = True
        ListBoxCateg.IntegralHeight = False
        ListBoxCateg.ItemHeight = 23
        ListBoxCateg.Location = New Point(0, 0)
        ListBoxCateg.Margin = New Padding(10, 0, 10, 0)
        ListBoxCateg.Name = "ListBoxCateg"
        ListBoxCateg.Size = New Size(459, 575)
        ListBoxCateg.TabIndex = 1
        ' 
        ' BtnDelCateg
        ' 
        BtnDelCateg.BackColor = Color.White
        BtnDelCateg.FlatAppearance.BorderColor = Color.ForestGreen
        BtnDelCateg.FlatStyle = FlatStyle.Flat
        BtnDelCateg.Font = New Font("Segoe UI", 10F)
        BtnDelCateg.ForeColor = Color.ForestGreen
        BtnDelCateg.Location = New Point(308, 116)
        BtnDelCateg.Name = "BtnDelCateg"
        BtnDelCateg.Size = New Size(114, 38)
        BtnDelCateg.TabIndex = 6
        BtnDelCateg.Text = "Désactiver"
        BtnDelCateg.UseVisualStyleBackColor = False
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label4.Location = New Point(24, 22)
        Label4.Name = "Label4"
        Label4.Size = New Size(150, 20)
        Label4.TabIndex = 4
        Label4.Text = "Nom de la categorie"
        ' 
        ' BtnNewCateg
        ' 
        BtnNewCateg.BackColor = Color.White
        BtnNewCateg.FlatAppearance.BorderColor = Color.ForestGreen
        BtnNewCateg.FlatStyle = FlatStyle.Flat
        BtnNewCateg.Font = New Font("Segoe UI", 10F)
        BtnNewCateg.ForeColor = Color.ForestGreen
        BtnNewCateg.Location = New Point(183, 116)
        BtnNewCateg.Name = "BtnNewCateg"
        BtnNewCateg.Size = New Size(94, 38)
        BtnNewCateg.TabIndex = 7
        BtnNewCateg.Text = "Nouveau"
        BtnNewCateg.UseVisualStyleBackColor = False
        ' 
        ' TextBoxCateg
        ' 
        TextBoxCateg.Font = New Font("Segoe UI", 11F)
        TextBoxCateg.Location = New Point(24, 56)
        TextBoxCateg.Name = "TextBoxCateg"
        TextBoxCateg.Size = New Size(451, 32)
        TextBoxCateg.TabIndex = 4
        ' 
        ' BtnEnregistrerCateg
        ' 
        BtnEnregistrerCateg.BackColor = Color.ForestGreen
        BtnEnregistrerCateg.FlatStyle = FlatStyle.Flat
        BtnEnregistrerCateg.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnEnregistrerCateg.ForeColor = Color.White
        BtnEnregistrerCateg.Location = New Point(24, 116)
        BtnEnregistrerCateg.Name = "BtnEnregistrerCateg"
        BtnEnregistrerCateg.Size = New Size(129, 38)
        BtnEnregistrerCateg.TabIndex = 5
        BtnEnregistrerCateg.Text = "Enregistrer"
        BtnEnregistrerCateg.UseVisualStyleBackColor = False
        ' 
        ' TabProduits
        ' 
        TabProduits.Location = New Point(4, 29)
        TabProduits.Name = "TabProduits"
        TabProduits.Padding = New Padding(3)
        TabProduits.Size = New Size(1384, 581)
        TabProduits.TabIndex = 2
        TabProduits.Text = "Produits"
        TabProduits.UseVisualStyleBackColor = True
        ' 
        ' Referentiels
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1448, 685)
        Controls.Add(TabControls1)
        Controls.Add(Label1)
        Name = "Referentiels"
        Text = "Referentiels"
        TabControls1.ResumeLayout(False)
        TabMagasins.ResumeLayout(False)
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        SplitContainer1.Panel2.PerformLayout()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        TabCategories.ResumeLayout(False)
        SplitContainer2.Panel1.ResumeLayout(False)
        SplitContainer2.Panel2.ResumeLayout(False)
        SplitContainer2.Panel2.PerformLayout()
        CType(SplitContainer2, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer2.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents TabControls1 As TabControl
    Friend WithEvents TabMagasins As TabPage
    Friend WithEvents TabCategories As TabPage
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents ListBoxMagasins As ListBox
    Friend WithEvents BtnEnregistrer As Button
    Friend WithEvents TextBoxMagasin As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents BtnNouveau As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents BtnDesactiver As Button
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents ListBoxCateg As ListBox
    Friend WithEvents Label4 As Label
    Friend WithEvents BtnDelCateg As Button
    Friend WithEvents BtnNewCateg As Button
    Friend WithEvents TextBoxCateg As TextBox
    Friend WithEvents BtnEnregistrerCateg As Button
    Friend WithEvents TabProduits As TabPage
End Class
