Option Explicit On
Option Strict On
Imports MySqlConnector

Public Class MainForm
    Private tabBord As TableauBord
    Private TB As New TableauBordMySql
    Private nombreAlerte As Integer
    Private referentiel As Referentiels
    Private alerte As frmAlertes
    Private ticket As FrmTickets
    Private historique As frmHistorique
    Private analyse As frmAnalyses
    Private Sub BtnAccueil_Click(sender As Object, e As EventArgs) Handles BtnAccueil.Click
        'BtnAccueil.BackColor = Color.White
        'BtnAccueil.ForeColor = Color.ForestGreen

        If tabBord IsNot Nothing Then
            BtnColor("BtnAccueil")
            tabBord.BringToFront()
        End If
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LabelNomRole.Text = Session.user.Login & "." & Session.user.Role
        'Dim sql As String = "Select count(lue) from alerte where id_utilisateur = @id and lue = false; "
        'Using connection As MySqlConnection = GetConnection
        '    If connection.State <> ConnectionState.Open Then
        '        connection.Open()

        '        Using alerte As New MySqlCommand(sql, connection)
        '            alerte.Parameters.AddWithValue("@id", Session.user.Id)
        '            nombreAlerte = Convert.ToInt32(alerte.ExecuteScalar)
        '        End Using
        '    End If
        'End Using
        nombreAlerte = TB.AlerteNonLue(Session.user.Id)
        LabelAlerte.Text = "Alertes non lues : " & nombreAlerte
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

    Private Sub BtnLogOut_Click(sender As Object, e As EventArgs) Handles BtnLogOut.Click
        Dim LogIn As New Connexion
        Hide()
        Session.Deconnecter()
        LogIn.Show()
    End Sub

    Private Sub timer_tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        nombreAlerte = TB.AlerteNonLue(Session.user.Id)
        LabelAlerte.Text = "Alertes non lues : " & nombreAlerte
    End Sub

    Private Sub BtnAlertes_Click(sender As Object, e As EventArgs) Handles BtnAlertes.Click
        BtnColor("BtnAlertes")
        If alerte Is Nothing Then
            alerte = New frmAlertes
            alerte.TopLevel = False
            alerte.FormBorderStyle = FormBorderStyle.None
            alerte.Dock = DockStyle.Fill
            PanelAffichage.Controls.Add(alerte)
            alerte.Show()
            alerte.BringToFront()
        Else
            alerte.BringToFront()
        End If
    End Sub

    Private Sub BtnSaisieTicket_Click(sender As Object, e As EventArgs) Handles BtnSaisieTicket.Click
        BtnColor("BtnSaisieTicket")
        If ticket Is Nothing Then
            ticket = New FrmTickets
            ticket.TopLevel = False
            ticket.FormBorderStyle = FormBorderStyle.None
            ticket.Dock = DockStyle.Fill
            PanelAffichage.Controls.Add(ticket)
            ticket.Show()
            ticket.BringToFront()
        Else
            ticket.BringToFront()
        End If
    End Sub

    Private Sub BtnHistorique_Click(sender As Object, e As EventArgs) Handles BtnHistorique.Click
        BtnColor("BtnHistorique")
        If historique Is Nothing Then
            historique = New frmHistorique
            historique.TopLevel = False
            historique.FormBorderStyle = FormBorderStyle.None
            historique.Dock = DockStyle.Fill
            PanelAffichage.Controls.Add(historique)
            historique.Show()
            historique.BringToFront()
        Else
            historique.BringToFront()
        End If
    End Sub

    Private Sub BtnAnalyses_Click(sender As Object, e As EventArgs) Handles BtnAnalyses.Click
        BtnColor("BtnAnalyses")
        If analyse Is Nothing Then
            analyse = New frmAnalyses
            analyse.TopLevel = False
            analyse.FormBorderStyle = FormBorderStyle.None
            analyse.Dock = DockStyle.Fill
            PanelAffichage.Controls.Add(analyse)
            analyse.Show()
            analyse.BringToFront()
        Else
            analyse.BringToFront()
        End If
    End Sub
End Class