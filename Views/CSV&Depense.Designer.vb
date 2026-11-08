<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CSV_Depense
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CSV_Depense))
        TabControl1 = New TabControl()
        TabImportExport = New TabPage()
        LabelResultat = New Label()
        Label3 = New Label()
        BtnExport = New Button()
        CmBMagasins = New ComboBox()
        CmBProduit = New ComboBox()
        Label2 = New Label()
        BtnImport = New Button()
        Label1 = New Label()
        TabDepense = New TabPage()
        LabelTitle = New Label()
        TabControl1.SuspendLayout()
        TabImportExport.SuspendLayout()
        SuspendLayout()
        ' 
        ' TabControl1
        ' 
        TabControl1.Controls.Add(TabImportExport)
        TabControl1.Controls.Add(TabDepense)
        TabControl1.Location = New Point(12, 82)
        TabControl1.Name = "TabControl1"
        TabControl1.SelectedIndex = 0
        TabControl1.Size = New Size(1423, 728)
        TabControl1.TabIndex = 0
        ' 
        ' TabImportExport
        ' 
        TabImportExport.Controls.Add(LabelResultat)
        TabImportExport.Controls.Add(Label3)
        TabImportExport.Controls.Add(BtnExport)
        TabImportExport.Controls.Add(CmBMagasins)
        TabImportExport.Controls.Add(CmBProduit)
        TabImportExport.Controls.Add(Label2)
        TabImportExport.Controls.Add(BtnImport)
        TabImportExport.Controls.Add(Label1)
        TabImportExport.Location = New Point(4, 29)
        TabImportExport.Name = "TabImportExport"
        TabImportExport.Padding = New Padding(3)
        TabImportExport.Size = New Size(1415, 695)
        TabImportExport.TabIndex = 0
        TabImportExport.Text = "Import & Export CSV"
        TabImportExport.UseVisualStyleBackColor = True
        ' 
        ' LabelResultat
        ' 
        LabelResultat.AutoSize = True
        LabelResultat.Font = New Font("Segoe UI", 8.0F)
        LabelResultat.ForeColor = Color.SeaGreen
        LabelResultat.Location = New Point(36, 195)
        LabelResultat.Name = "LabelResultat"
        LabelResultat.Size = New Size(207, 19)
        LabelResultat.TabIndex = 9
        LabelResultat.Text = "3 ligne(s) importée(s). 0 erreur(s)"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 8.0F)
        Label3.ForeColor = Color.DimGray
        Label3.Location = New Point(36, 492)
        Label3.Name = "Label3"
        Label3.Size = New Size(1230, 19)
        Label3.TabIndex = 8
        Label3.Text = resources.GetString("Label3.Text")
        ' 
        ' BtnExport
        ' 
        BtnExport.FlatAppearance.BorderColor = Color.ForestGreen
        BtnExport.FlatStyle = FlatStyle.Flat
        BtnExport.Font = New Font("Segoe UI", 10.0F)
        BtnExport.ForeColor = Color.ForestGreen
        BtnExport.Location = New Point(807, 404)
        BtnExport.Name = "BtnExport"
        BtnExport.Size = New Size(151, 49)
        BtnExport.TabIndex = 6
        BtnExport.Text = "Exporter en CSV"
        BtnExport.UseVisualStyleBackColor = True
        ' 
        ' CmBMagasins
        ' 
        CmBMagasins.FormattingEnabled = True
        CmBMagasins.Items.AddRange(New Object() {"Tous les magasins"})
        CmBMagasins.Location = New Point(453, 416)
        CmBMagasins.Name = "CmBMagasins"
        CmBMagasins.Size = New Size(280, 28)
        CmBMagasins.TabIndex = 5
        ' 
        ' CmBProduit
        ' 
        CmBProduit.FormattingEnabled = True
        CmBProduit.Location = New Point(36, 416)
        CmBProduit.Name = "CmBProduit"
        CmBProduit.Size = New Size(363, 28)
        CmBProduit.TabIndex = 4
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Label2.Location = New Point(36, 345)
        Label2.Name = "Label2"
        Label2.Size = New Size(332, 28)
        Label2.TabIndex = 2
        Label2.Text = "Exporter l'historique d'un produit"
        ' 
        ' BtnImport
        ' 
        BtnImport.BackColor = Color.ForestGreen
        BtnImport.FlatAppearance.BorderColor = Color.ForestGreen
        BtnImport.FlatStyle = FlatStyle.Flat
        BtnImport.Font = New Font("Segoe UI", 10.0F)
        BtnImport.ForeColor = Color.White
        BtnImport.Location = New Point(36, 112)
        BtnImport.Name = "BtnImport"
        BtnImport.Size = New Size(151, 49)
        BtnImport.TabIndex = 1
        BtnImport.Text = "Importer un CSV"
        BtnImport.UseVisualStyleBackColor = False
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.ForeColor = Color.Black
        Label1.Location = New Point(36, 38)
        Label1.Name = "Label1"
        Label1.Size = New Size(753, 23)
        Label1.TabIndex = 0
        Label1.Text = "Importer des relevés de prix au format CSV. Les tickets sont regroupés par des dates et magasins."
        ' 
        ' TabDepense
        ' 
        TabDepense.Location = New Point(4, 29)
        TabDepense.Name = "TabDepense"
        TabDepense.Padding = New Padding(3)
        TabDepense.Size = New Size(1415, 695)
        TabDepense.TabIndex = 1
        TabDepense.Text = "Dépenses récurrentes"
        TabDepense.UseVisualStyleBackColor = True
        ' 
        ' LabelTitle
        ' 
        LabelTitle.AutoSize = True
        LabelTitle.Font = New Font("Segoe UI", 18.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LabelTitle.ForeColor = Color.ForestGreen
        LabelTitle.Location = New Point(12, 9)
        LabelTitle.Name = "LabelTitle"
        LabelTitle.Size = New Size(649, 41)
        LabelTitle.TabIndex = 1
        LabelTitle.Text = "Import et Export CSV / Dépenses récurrentes"
        ' 
        ' CSV_Depense
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1435, 822)
        Controls.Add(LabelTitle)
        Controls.Add(TabControl1)
        Name = "CSV_Depense"
        Text = "CSV_Depense"
        TabControl1.ResumeLayout(False)
        TabImportExport.ResumeLayout(False)
        TabImportExport.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabImportExport As TabPage
    Friend WithEvents TabDepense As TabPage
    Friend WithEvents LabelTitle As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents BtnImport As Button
    Friend WithEvents CmBMagasins As ComboBox
    Friend WithEvents CmBProduit As ComboBox
    Friend WithEvents BtnExport As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents LabelResultat As Label
End Class
