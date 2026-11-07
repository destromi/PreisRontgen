<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TableauBord
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
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As DataGridViewCellStyle = New DataGridViewCellStyle()
        DataGridView1 = New DataGridView()
        ColCateg = New DataGridViewTextBoxColumn()
        ColMont1 = New DataGridViewTextBoxColumn()
        Label1 = New Label()
        BtnActualiser = New Button()
        PanelDepense = New Panel()
        LabelDepenseTotal = New Label()
        LabelTitleDepense = New Label()
        Panel1 = New Panel()
        LabelAlertesTotal = New Label()
        LabelAlertes = New Label()
        Panel2 = New Panel()
        LabelIndice = New Label()
        LabelIndiceTitre = New Label()
        DataGridView2 = New DataGridView()
        ColDep = New DataGridViewTextBoxColumn()
        ColMont2 = New DataGridViewTextBoxColumn()
        ColEchean = New DataGridViewTextBoxColumn()
        Label2 = New Label()
        Label3 = New Label()
        TableLayoutPanel1 = New TableLayoutPanel()
        TableLayoutPanel2 = New TableLayoutPanel()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        PanelDepense.SuspendLayout()
        Panel1.SuspendLayout()
        Panel2.SuspendLayout()
        CType(DataGridView2, ComponentModel.ISupportInitialize).BeginInit()
        TableLayoutPanel1.SuspendLayout()
        TableLayoutPanel2.SuspendLayout()
        SuspendLayout()
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToOrderColumns = True
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.BackgroundColor = Color.White
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Columns.AddRange(New DataGridViewColumn() {ColCateg, ColMont1})
        DataGridView1.Dock = DockStyle.Fill
        DataGridView1.Location = New Point(10, 50)
        DataGridView1.Margin = New Padding(10, 0, 10, 0)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.RowHeadersVisible = False
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.Size = New Size(640, 249)
        DataGridView1.TabIndex = 3
        ' 
        ' ColCateg
        ' 
        ColCateg.HeaderText = "Catégorie"
        ColCateg.MinimumWidth = 6
        ColCateg.Name = "ColCateg"
        ' 
        ' ColMont1
        ' 
        DataGridViewCellStyle4.Format = "#,##0 ""FCFA"""
        ColMont1.DefaultCellStyle = DataGridViewCellStyle4
        ColMont1.HeaderText = "Montant"
        ColMont1.MinimumWidth = 6
        ColMont1.Name = "ColMont1"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.ForeColor = Color.ForestGreen
        Label1.Location = New Point(12, 24)
        Label1.Name = "Label1"
        Label1.Size = New Size(246, 41)
        Label1.TabIndex = 0
        Label1.Text = "Tableau de bord"
        ' 
        ' BtnActualiser
        ' 
        BtnActualiser.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        BtnActualiser.BackColor = Color.ForestGreen
        BtnActualiser.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnActualiser.ForeColor = Color.White
        BtnActualiser.Location = New Point(1174, 23)
        BtnActualiser.Name = "BtnActualiser"
        BtnActualiser.Size = New Size(150, 46)
        BtnActualiser.TabIndex = 1
        BtnActualiser.Text = "Actualiser"
        BtnActualiser.UseVisualStyleBackColor = False
        ' 
        ' PanelDepense
        ' 
        PanelDepense.BackColor = Color.White
        PanelDepense.BorderStyle = BorderStyle.FixedSingle
        PanelDepense.Controls.Add(LabelDepenseTotal)
        PanelDepense.Controls.Add(LabelTitleDepense)
        PanelDepense.Dock = DockStyle.Fill
        PanelDepense.Font = New Font("Bookman Old Style", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        PanelDepense.Location = New Point(10, 0)
        PanelDepense.Margin = New Padding(10, 0, 10, 0)
        PanelDepense.Name = "PanelDepense"
        PanelDepense.Size = New Size(420, 150)
        PanelDepense.TabIndex = 2
        ' 
        ' LabelDepenseTotal
        ' 
        LabelDepenseTotal.AutoSize = True
        LabelDepenseTotal.Font = New Font("Segoe UI", 22.2F, FontStyle.Bold)
        LabelDepenseTotal.ForeColor = Color.ForestGreen
        LabelDepenseTotal.Location = New Point(-1, 62)
        LabelDepenseTotal.Name = "LabelDepenseTotal"
        LabelDepenseTotal.Size = New Size(264, 50)
        LabelDepenseTotal.TabIndex = 1
        LabelDepenseTotal.Text = "1000000 FCFA"
        ' 
        ' LabelTitleDepense
        ' 
        LabelTitleDepense.AutoSize = True
        LabelTitleDepense.BackColor = Color.White
        LabelTitleDepense.Font = New Font("Segoe UI", 9F)
        LabelTitleDepense.ForeColor = Color.DimGray
        LabelTitleDepense.Location = New Point(3, 13)
        LabelTitleDepense.Name = "LabelTitleDepense"
        LabelTitleDepense.Size = New Size(124, 20)
        LabelTitleDepense.TabIndex = 0
        LabelTitleDepense.Text = "Dépense du mois"
        ' 
        ' Panel1
        ' 
        Panel1.BackColor = Color.White
        Panel1.BorderStyle = BorderStyle.FixedSingle
        Panel1.Controls.Add(LabelAlertesTotal)
        Panel1.Controls.Add(LabelAlertes)
        Panel1.Dock = DockStyle.Fill
        Panel1.Font = New Font("Bookman Old Style", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Panel1.Location = New Point(450, 0)
        Panel1.Margin = New Padding(10, 0, 10, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(420, 150)
        Panel1.TabIndex = 2
        ' 
        ' LabelAlertesTotal
        ' 
        LabelAlertesTotal.AutoSize = True
        LabelAlertesTotal.Font = New Font("Segoe UI", 22.2F, FontStyle.Bold)
        LabelAlertesTotal.ForeColor = Color.ForestGreen
        LabelAlertesTotal.Location = New Point(3, 59)
        LabelAlertesTotal.Name = "LabelAlertesTotal"
        LabelAlertesTotal.Size = New Size(43, 50)
        LabelAlertesTotal.TabIndex = 1
        LabelAlertesTotal.Text = "3"
        ' 
        ' LabelAlertes
        ' 
        LabelAlertes.AutoSize = True
        LabelAlertes.BackColor = Color.White
        LabelAlertes.Font = New Font("Segoe UI", 9F)
        LabelAlertes.ForeColor = Color.DimGray
        LabelAlertes.Location = New Point(3, 13)
        LabelAlertes.Name = "LabelAlertes"
        LabelAlertes.Size = New Size(114, 20)
        LabelAlertes.TabIndex = 0
        LabelAlertes.Text = "Alertes non lues"
        ' 
        ' Panel2
        ' 
        Panel2.BackColor = Color.White
        Panel2.BorderStyle = BorderStyle.FixedSingle
        Panel2.Controls.Add(LabelIndice)
        Panel2.Controls.Add(LabelIndiceTitre)
        Panel2.Dock = DockStyle.Fill
        Panel2.Font = New Font("Bookman Old Style", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Panel2.Location = New Point(890, 0)
        Panel2.Margin = New Padding(10, 0, 10, 0)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(421, 150)
        Panel2.TabIndex = 2
        ' 
        ' LabelIndice
        ' 
        LabelIndice.AutoSize = True
        LabelIndice.Font = New Font("Segoe UI", 22.2F, FontStyle.Bold)
        LabelIndice.ForeColor = Color.ForestGreen
        LabelIndice.Location = New Point(3, 59)
        LabelIndice.Name = "LabelIndice"
        LabelIndice.Size = New Size(116, 50)
        LabelIndice.TabIndex = 1
        LabelIndice.Text = "103,5"
        ' 
        ' LabelIndiceTitre
        ' 
        LabelIndiceTitre.AutoSize = True
        LabelIndiceTitre.BackColor = Color.White
        LabelIndiceTitre.Font = New Font("Segoe UI", 9F)
        LabelIndiceTitre.ForeColor = Color.DimGray
        LabelIndiceTitre.Location = New Point(3, 13)
        LabelIndiceTitre.Name = "LabelIndiceTitre"
        LabelIndiceTitre.Size = New Size(118, 20)
        LabelIndiceTitre.TabIndex = 0
        LabelIndiceTitre.Text = "Indice personnel"
        ' 
        ' DataGridView2
        ' 
        DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView2.BackgroundColor = Color.White
        DataGridView2.ColumnHeadersHeight = 29
        DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridView2.Columns.AddRange(New DataGridViewColumn() {ColDep, ColMont2, ColEchean})
        DataGridView2.Dock = DockStyle.Fill
        DataGridView2.Location = New Point(670, 50)
        DataGridView2.Margin = New Padding(10, 0, 10, 0)
        DataGridView2.Name = "DataGridView2"
        DataGridView2.RowHeadersVisible = False
        DataGridView2.RowHeadersWidth = 51
        DataGridView2.Size = New Size(641, 249)
        DataGridView2.TabIndex = 3
        ' 
        ' ColDep
        ' 
        ColDep.HeaderText = "Dépense"
        ColDep.MinimumWidth = 6
        ColDep.Name = "ColDep"
        ' 
        ' ColMont2
        ' 
        DataGridViewCellStyle5.Format = "#,##0 ""FCFA"""
        ColMont2.DefaultCellStyle = DataGridViewCellStyle5
        ColMont2.HeaderText = "Montant"
        ColMont2.MinimumWidth = 6
        ColMont2.Name = "ColMont2"
        ' 
        ' ColEchean
        ' 
        DataGridViewCellStyle6.Format = """dd/MM/yyyy"""
        ColEchean.DefaultCellStyle = DataGridViewCellStyle6
        ColEchean.HeaderText = "Echéance"
        ColEchean.MinimumWidth = 6
        ColEchean.Name = "ColEchean"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Dock = DockStyle.Fill
        Label2.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label2.ForeColor = Color.ForestGreen
        Label2.Location = New Point(3, 0)
        Label2.Name = "Label2"
        Label2.Size = New Size(654, 50)
        Label2.TabIndex = 4
        Label2.Text = "Dépense par catégorie - mois en cours"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Dock = DockStyle.Fill
        Label3.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label3.ForeColor = Color.ForestGreen
        Label3.Location = New Point(663, 0)
        Label3.Name = "Label3"
        Label3.Size = New Size(655, 50)
        Label3.TabIndex = 4
        Label3.Text = "Echéance à venir"
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel1.ColumnCount = 3
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333359F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333359F))
        TableLayoutPanel1.Controls.Add(PanelDepense, 0, 0)
        TableLayoutPanel1.Controls.Add(Panel1, 1, 0)
        TableLayoutPanel1.Controls.Add(Panel2, 2, 0)
        TableLayoutPanel1.Location = New Point(12, 105)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 1
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanel1.Size = New Size(1321, 150)
        TableLayoutPanel1.TabIndex = 5
        ' 
        ' TableLayoutPanel2
        ' 
        TableLayoutPanel2.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel2.ColumnCount = 2
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.Controls.Add(DataGridView2, 1, 1)
        TableLayoutPanel2.Controls.Add(DataGridView1, 0, 1)
        TableLayoutPanel2.Controls.Add(Label2, 0, 0)
        TableLayoutPanel2.Controls.Add(Label3, 1, 0)
        TableLayoutPanel2.Location = New Point(12, 283)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.RowCount = 2
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Absolute, 50F))
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanel2.Size = New Size(1321, 299)
        TableLayoutPanel2.TabIndex = 6
        ' 
        ' TableauBord
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.WhiteSmoke
        ClientSize = New Size(1469, 594)
        Controls.Add(TableLayoutPanel2)
        Controls.Add(TableLayoutPanel1)
        Controls.Add(BtnActualiser)
        Controls.Add(Label1)
        Name = "TableauBord"
        Text = "TableauBord"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        PanelDepense.ResumeLayout(False)
        PanelDepense.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        CType(DataGridView2, ComponentModel.ISupportInitialize).EndInit()
        TableLayoutPanel1.ResumeLayout(False)
        TableLayoutPanel2.ResumeLayout(False)
        TableLayoutPanel2.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents BtnActualiser As Button
    Friend WithEvents PanelDepense As Panel
    Friend WithEvents LabelTitleDepense As Label
    Friend WithEvents LabelDepenseTotal As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents LabelAlertesTotal As Label
    Friend WithEvents LabelAlertes As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents LabelIndice As Label
    Friend WithEvents LabelIndiceTitre As Label
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents ColCateg As DataGridViewTextBoxColumn
    Friend WithEvents ColMont1 As DataGridViewTextBoxColumn
    Friend WithEvents ColDep As DataGridViewTextBoxColumn
    Friend WithEvents ColMont2 As DataGridViewTextBoxColumn
    Friend WithEvents ColEchean As DataGridViewTextBoxColumn
End Class
