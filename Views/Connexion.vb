Option Strict On
Option Explicit On
Imports MySqlConnector

Public Class Connexion
    Public Sub New()
        Text = "Pricieux - Connexion"
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().

    End Sub

    ' TODO: insérez le code pour effectuer une authentification personnalisée à l'aide du nom d'utilisateur et du mot de passe fournis 
    ' (Consultez https://go.microsoft.com/fwlink/?LinkId=35339).  
    ' L'objet Principal personnalisé peut ensuite être associé à l'objet Principal du thread actuel comme suit : 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' CustomPrincipal est l'implémentation IPrincipal utilisée pour effectuer l'authentification. 
    ' Par la suite, My.User retournera les informations d'identité encapsulées dans l'objet CustomPrincipal
    ' telles que le nom d'utilisateur, le nom complet, etc.

    Private Sub OK_Click(ByVal sender As Object, ByVal e As EventArgs)
        Close()
    End Sub

    Private Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        Close()
    End Sub

    Private Sub labelInsc(sender As Object, e As EventArgs) Handles LabelInscrire.MouseEnter
        LabelInscrire.Font = New Font(LabelInscrire.Font, FontStyle.Underline)
        LabelInscrire.Cursor = Cursors.Hand
    End Sub

    Private Sub labelInscI(sender As Object, e As EventArgs) Handles LabelInscrire.MouseLeave
        LabelInscrire.Font = New Font(LabelInscrire.Font, FontStyle.Regular)
    End Sub

    Private Sub Conn(sender As Object, e As EventArgs) Handles BtnConnect.MouseEnter
        BtnConnect.BackColor = Color.ForestGreen
        BtnConnect.Cursor = Cursors.Hand
    End Sub

    Private Sub Connleave(sender As Object, e As EventArgs) Handles BtnConnect.MouseLeave
        BtnConnect.BackColor = Color.LimeGreen
    End Sub

    Private Async Sub BtnConnect_Click(sender As Object, e As EventArgs) Handles BtnConnect.Click
        Try
            Dim user As Utilisateurs = Await _authentification.ConnectionCompte(TextBoxUser.Text, TextBoxPwd.Text)
            Hide()
            MessageBox.Show("Bienvenue Monsieur " & user.Login)
        Catch ex As ArgumentException
            LabelError.Text = ex.Message
        Catch ex As MySqlException
            LabelError.Text = "Impossible de connecter la base de données"
        End Try
    End Sub
    Public Property Utilisateur As Utilisateurs

    Private _authentification As New Authentification()
End Class
