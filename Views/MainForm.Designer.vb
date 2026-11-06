<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        components = New ComponentModel.Container()
        Panel1 = New Panel()
        BtnLogOut = New Button()
        LabelAlerte = New Label()
        LabelNomRole = New Label()
        LabelAcronyme = New Label()
        Label1 = New Label()
        ToolStrip1 = New ToolStrip()
        ToolStripSeparator7 = New ToolStripSeparator()
        BtnAccueil = New ToolStripButton()
        ToolStripSeparator1 = New ToolStripSeparator()
        BtnSaisieTicket = New ToolStripButton()
        ToolStripSeparator2 = New ToolStripSeparator()
        BtnHistorique = New ToolStripButton()
        ToolStripSeparator3 = New ToolStripSeparator()
        BtnAlertes = New ToolStripButton()
        ToolStripSeparator4 = New ToolStripSeparator()
        BtnAnalyses = New ToolStripButton()
        ToolStripSeparator5 = New ToolStripSeparator()
        BtnReferentiels = New ToolStripButton()
        ToolStripSeparator6 = New ToolStripSeparator()
        BtnCSV = New ToolStripButton()
        ToolStripSeparator8 = New ToolStripSeparator()
        PanelAffichage = New Panel()
        Timer1 = New Timer(components)
        Panel1.SuspendLayout()
        ToolStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' Panel1
        ' 
        Panel1.BackColor = Color.ForestGreen
        Panel1.Controls.Add(BtnLogOut)
        Panel1.Controls.Add(LabelAlerte)
        Panel1.Controls.Add(LabelNomRole)
        Panel1.Controls.Add(LabelAcronyme)
        Panel1.Controls.Add(Label1)
        Panel1.Dock = DockStyle.Top
        Panel1.Location = New Point(0, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1211, 125)
        Panel1.TabIndex = 0
        ' 
        ' BtnLogOut
        ' 
        BtnLogOut.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        BtnLogOut.Font = New Font("Bookman Old Style", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnLogOut.ForeColor = Color.ForestGreen
        BtnLogOut.Location = New Point(987, 42)
        BtnLogOut.Name = "BtnLogOut"
        BtnLogOut.Size = New Size(183, 41)
        BtnLogOut.TabIndex = 4
        BtnLogOut.Text = "Se deconnecter"
        BtnLogOut.UseVisualStyleBackColor = True
        ' 
        ' LabelAlerte
        ' 
        LabelAlerte.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        LabelAlerte.AutoSize = True
        LabelAlerte.Font = New Font("Bookman Old Style", 10.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        LabelAlerte.ForeColor = Color.Yellow
        LabelAlerte.Location = New Point(746, 30)
        LabelAlerte.Name = "LabelAlerte"
        LabelAlerte.Size = New Size(183, 20)
        LabelAlerte.TabIndex = 3
        LabelAlerte.Text = "Alertes non lues : 3"
        ' 
        ' LabelNomRole
        ' 
        LabelNomRole.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        LabelNomRole.AutoSize = True
        LabelNomRole.Font = New Font("Bookman Old Style", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        LabelNomRole.ForeColor = Color.White
        LabelNomRole.Location = New Point(560, 30)
        LabelNomRole.Name = "LabelNomRole"
        LabelNomRole.Size = New Size(116, 23)
        LabelNomRole.TabIndex = 2
        LabelNomRole.Text = "Ajel.Admin"
        ' 
        ' LabelAcronyme
        ' 
        LabelAcronyme.AutoSize = True
        LabelAcronyme.Font = New Font("Bookman Old Style", 9F)
        LabelAcronyme.ForeColor = Color.White
        LabelAcronyme.Location = New Point(30, 75)
        LabelAcronyme.Name = "LabelAcronyme"
        LabelAcronyme.Size = New Size(253, 19)
        LabelAcronyme.TabIndex = 1
        LabelAcronyme.Text = "Suivi des prix et hausse des prix"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Bookman Old Style", 24F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.ForeColor = Color.White
        Label1.Location = New Point(30, 30)
        Label1.Name = "Label1"
        Label1.Size = New Size(189, 47)
        Label1.TabIndex = 0
        Label1.Text = "Pricieux"
        ' 
        ' ToolStrip1
        ' 
        ToolStrip1.BackColor = SystemColors.ControlLight
        ToolStrip1.ImageScalingSize = New Size(20, 20)
        ToolStrip1.Items.AddRange(New ToolStripItem() {ToolStripSeparator7, BtnAccueil, ToolStripSeparator1, BtnSaisieTicket, ToolStripSeparator2, BtnHistorique, ToolStripSeparator3, BtnAlertes, ToolStripSeparator4, BtnAnalyses, ToolStripSeparator5, BtnReferentiels, ToolStripSeparator6, BtnCSV, ToolStripSeparator8})
        ToolStrip1.Location = New Point(0, 125)
        ToolStrip1.Name = "ToolStrip1"
        ToolStrip1.Size = New Size(1211, 37)
        ToolStrip1.TabIndex = 1
        ToolStrip1.Text = "ToolStrip1"
        ' 
        ' ToolStripSeparator7
        ' 
        ToolStripSeparator7.Name = "ToolStripSeparator7"
        ToolStripSeparator7.Size = New Size(6, 37)
        ' 
        ' BtnAccueil
        ' 
        BtnAccueil.DisplayStyle = ToolStripItemDisplayStyle.Text
        BtnAccueil.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnAccueil.ImageTransparentColor = Color.Magenta
        BtnAccueil.Margin = New Padding(2, 0, 2, 0)
        BtnAccueil.Name = "BtnAccueil"
        BtnAccueil.Padding = New Padding(10, 6, 10, 6)
        BtnAccueil.Size = New Size(96, 37)
        BtnAccueil.Text = "Accueil"
        ' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Margin = New Padding(2, 0, 2, 0)
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Padding = New Padding(10, 6, 10, 6)
        ToolStripSeparator1.Size = New Size(6, 37)
        ' 
        ' BtnSaisieTicket
        ' 
        BtnSaisieTicket.DisplayStyle = ToolStripItemDisplayStyle.Text
        BtnSaisieTicket.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnSaisieTicket.ImageTransparentColor = Color.Magenta
        BtnSaisieTicket.Margin = New Padding(2, 0, 2, 0)
        BtnSaisieTicket.Name = "BtnSaisieTicket"
        BtnSaisieTicket.Padding = New Padding(10, 6, 10, 6)
        BtnSaisieTicket.Size = New Size(136, 37)
        BtnSaisieTicket.Text = "Saisie ticket"
        ' 
        ' ToolStripSeparator2
        ' 
        ToolStripSeparator2.Margin = New Padding(2, 0, 2, 0)
        ToolStripSeparator2.Name = "ToolStripSeparator2"
        ToolStripSeparator2.Padding = New Padding(10, 6, 10, 6)
        ToolStripSeparator2.Size = New Size(6, 37)
        ' 
        ' BtnHistorique
        ' 
        BtnHistorique.DisplayStyle = ToolStripItemDisplayStyle.Text
        BtnHistorique.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnHistorique.ImageTransparentColor = Color.Magenta
        BtnHistorique.Margin = New Padding(2, 0, 2, 0)
        BtnHistorique.Name = "BtnHistorique"
        BtnHistorique.Padding = New Padding(10, 6, 10, 6)
        BtnHistorique.Size = New Size(122, 37)
        BtnHistorique.Text = "Historique"
        ' 
        ' ToolStripSeparator3
        ' 
        ToolStripSeparator3.Margin = New Padding(2, 0, 2, 0)
        ToolStripSeparator3.Name = "ToolStripSeparator3"
        ToolStripSeparator3.Padding = New Padding(10, 6, 10, 6)
        ToolStripSeparator3.Size = New Size(6, 37)
        ' 
        ' BtnAlertes
        ' 
        BtnAlertes.DisplayStyle = ToolStripItemDisplayStyle.Text
        BtnAlertes.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnAlertes.ImageTransparentColor = Color.Magenta
        BtnAlertes.Margin = New Padding(2, 0, 2, 0)
        BtnAlertes.Name = "BtnAlertes"
        BtnAlertes.Padding = New Padding(10, 6, 10, 6)
        BtnAlertes.Size = New Size(93, 37)
        BtnAlertes.Text = "Alertes"
        ' 
        ' ToolStripSeparator4
        ' 
        ToolStripSeparator4.Margin = New Padding(2, 0, 2, 0)
        ToolStripSeparator4.Name = "ToolStripSeparator4"
        ToolStripSeparator4.Padding = New Padding(10, 6, 10, 6)
        ToolStripSeparator4.Size = New Size(6, 37)
        ' 
        ' BtnAnalyses
        ' 
        BtnAnalyses.DisplayStyle = ToolStripItemDisplayStyle.Text
        BtnAnalyses.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnAnalyses.ImageTransparentColor = Color.Magenta
        BtnAnalyses.Margin = New Padding(2, 0, 2, 0)
        BtnAnalyses.Name = "BtnAnalyses"
        BtnAnalyses.Padding = New Padding(10, 6, 10, 6)
        BtnAnalyses.Size = New Size(109, 37)
        BtnAnalyses.Text = "Analyses"
        ' 
        ' ToolStripSeparator5
        ' 
        ToolStripSeparator5.Margin = New Padding(2, 0, 2, 0)
        ToolStripSeparator5.Name = "ToolStripSeparator5"
        ToolStripSeparator5.Padding = New Padding(10, 6, 10, 6)
        ToolStripSeparator5.Size = New Size(6, 37)
        ' 
        ' BtnReferentiels
        ' 
        BtnReferentiels.DisplayStyle = ToolStripItemDisplayStyle.Text
        BtnReferentiels.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnReferentiels.ImageTransparentColor = Color.Magenta
        BtnReferentiels.Margin = New Padding(2, 0, 2, 0)
        BtnReferentiels.Name = "BtnReferentiels"
        BtnReferentiels.Padding = New Padding(10, 6, 10, 6)
        BtnReferentiels.Size = New Size(134, 37)
        BtnReferentiels.Text = "Référentiels"
        ' 
        ' ToolStripSeparator6
        ' 
        ToolStripSeparator6.Margin = New Padding(2, 0, 2, 0)
        ToolStripSeparator6.Name = "ToolStripSeparator6"
        ToolStripSeparator6.Padding = New Padding(10, 6, 10, 6)
        ToolStripSeparator6.Size = New Size(6, 37)
        ' 
        ' BtnCSV
        ' 
        BtnCSV.DisplayStyle = ToolStripItemDisplayStyle.Text
        BtnCSV.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnCSV.ImageTransparentColor = Color.Magenta
        BtnCSV.Margin = New Padding(2, 0, 2, 0)
        BtnCSV.Name = "BtnCSV"
        BtnCSV.Padding = New Padding(10, 6, 10, 6)
        BtnCSV.Size = New Size(177, 37)
        BtnCSV.Text = "CSV et dépenses"
        ' 
        ' ToolStripSeparator8
        ' 
        ToolStripSeparator8.Name = "ToolStripSeparator8"
        ToolStripSeparator8.Size = New Size(6, 37)
        ' 
        ' PanelAffichage
        ' 
        PanelAffichage.BackColor = Color.White
        PanelAffichage.Dock = DockStyle.Fill
        PanelAffichage.Location = New Point(0, 162)
        PanelAffichage.Name = "PanelAffichage"
        PanelAffichage.Size = New Size(1211, 288)
        PanelAffichage.TabIndex = 2
        ' 
        ' Timer1
        ' 
        Timer1.Interval = 30
        ' 
        ' MainForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1211, 450)
        Controls.Add(PanelAffichage)
        Controls.Add(ToolStrip1)
        Controls.Add(Panel1)
        Name = "MainForm"
        Text = "Pricieux : Suivi des prix et hausse des prix"
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ToolStrip1.ResumeLayout(False)
        ToolStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents LabelAcronyme As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents LabelAlerte As Label
    Friend WithEvents LabelNomRole As Label
    Friend WithEvents BtnLogOut As Button
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents BtnAccueil As ToolStripButton
    Friend WithEvents BtnSaisieTicket As ToolStripButton
    Friend WithEvents BtnHistorique As ToolStripButton
    Friend WithEvents BtnAlertes As ToolStripButton
    Friend WithEvents BtnAnalyses As ToolStripButton
    Friend WithEvents BtnReferentiels As ToolStripButton
    Friend WithEvents BtnCSV As ToolStripButton
    Friend WithEvents PanelAffichage As Panel
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents Timer1 As Timer
End Class
