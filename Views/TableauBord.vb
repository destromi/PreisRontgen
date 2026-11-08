Imports MySqlConnector

Public Class TableauBord
    Private sql As String
    Private depense As Decimal
    Private nombrealerte As Integer
    Private TB As New TableauBordMySql
    Private Sub TableauBord_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        depense = TB.DepenseTotal(Session.user.Id)
        nombrealerte = TB.AlerteNonLue(Session.user.Id)
        DataGridView1.AutoGenerateColumns = False
        DataGridView2.AutoGenerateColumns = False
        DataGridView1.DataSource = TB.DepenseCategorie(Session.user.Id, Date.Now)
        DataGridView2.DataSource = TB.EcheanceTotal(Session.user.Id, Date.Now)
        LabelDepenseTotal.Text = depense & " FCFA"
        LabelAlertesTotal.Text = nombrealerte
        Dim r = SessionIndice.resultat
        If r IsNot Nothing AndAlso r.Indice.HasValue Then
            LabelIndice.Text = r.Indice.Value.ToString("0.00")
        Else
            LabelIndice.Text = "—"
        End If
    End Sub


    Private Sub BtnActualiser_Click(sender As Object, e As EventArgs) Handles BtnActualiser.Click
        depense = TB.DepenseTotal(Session.user.Id)
        nombrealerte = TB.AlerteNonLue(Session.user.Id)
        DataGridView1.DataSource = TB.DepenseCategorie(Session.user.Id, Date.Now)
        DataGridView2.DataSource = TB.EcheanceTotal(Session.user.Id, Date.Now)
        LabelIndice.Text = SessionIndice.resultat.Indice.Value.ToString("0.00")
    End Sub
End Class