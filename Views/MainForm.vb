Option Explicit On
Option Strict On
Public Class MainForm
    Private tabBord As TableauBord
    Private referentiel As Referentiels
    Private Sub BtnAccueil_Click(sender As Object, e As EventArgs) Handles BtnAccueil.Click
        'BtnAccueil.BackColor = Color.White
        'BtnAccueil.ForeColor = Color.ForestGreen

        If tabBord IsNot Nothing Then
            BtnColor("BtnAccueil")
            tabBord.BringToFront()
        End If
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tabBord = New TableauBord
        'BtnAccueil.BackColor = Color.White
        'BtnAccueil.ForeColor = Color.ForestGreen
        BtnColor("BtnAccueil")
        tabBord.TopLevel = False
        tabBord.FormBorderStyle = FormBorderStyle.None
        tabBord.Dock = DockStyle.Fill
        PanelAffichage.Controls.Add(tabBord)
        tabBord.Show()
    End Sub


    Private Sub BtnReferentiels_Click(sender As Object, e As EventArgs) Handles BtnReferentiels.Click
        'BtnReferentiels.BackColor = Color.White
        'BtnReferentiels.ForeColor = Color.ForestGreen
        'BtnAccueil.BackColor = SystemColors.ControlLight
        'BtnAccueil.ForeColor = SystemColors.ControlText
        BtnColor("BtnReferentiels")
        'MessageBox.Show("la valeur de referentiel" & referentiel.ToString)
        If referentiel Is Nothing Then
            referentiel = New Referentiels
            referentiel.TopLevel = False
            referentiel.FormBorderStyle = FormBorderStyle.None
            referentiel.Dock = DockStyle.Fill
            PanelAffichage.Controls.Add(referentiel)
            referentiel.Show()
            referentiel.BringToFront()
        Else
            referentiel.BringToFront()
        End If
    End Sub
    Private Sub BtnColor(Bt As String)
        For Each items As ToolStripItem In ToolStrip1.Items
            If TypeOf items Is ToolStripButton Then
                items.BackColor = SystemColors.ControlLight
                items.ForeColor = SystemColors.ControlText
            End If
        Next
        Dim btn As ToolStripButton = DirectCast(ToolStrip1.Items(Bt), ToolStripButton)
        btn.BackColor = Color.White
        btn.ForeColor = Color.ForestGreen
    End Sub
End Class