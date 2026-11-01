Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        MessageBox.Show("Le formulaire fonctionne !")

        If ConnexionBD.TesterConnexion() Then
            MessageBox.Show("Connexion à la base Pricieux réussie !")
        End If

    End Sub
End Class
