Option Strict On
Option Explicit On
Public Class Inscription

    ' TODO: insérez le code pour effectuer une authentification personnalisée à l'aide du nom d'utilisateur et du mot de passe fournis 
    ' (Consultez https://go.microsoft.com/fwlink/?LinkId=35339).  
    ' L'objet Principal personnalisé peut ensuite être associé à l'objet Principal du thread actuel comme suit : 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' CustomPrincipal est l'implémentation IPrincipal utilisée pour effectuer l'authentification. 
    ' Par la suite, My.User retournera les informations d'identité encapsulées dans l'objet CustomPrincipal
    ' telles que le nom d'utilisateur, le nom complet, etc.


    Private Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles LabelConnecter.Click
        Dim connect As New Connexion()
        connect.Show()
        Hide()
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBoxPwdC.TextChanged
        If TextBoxPwd.Text <> TextBoxPwdC.Text Then
            LabelError.Text = "les mots de passes ne correspondent pas"
            PwdConfirm = False
        Else
            LabelError.Text = ""
            PwdConfirm = True
        End If
    End Sub

    Private Sub BtnInscrire_Click(sender As Object, e As EventArgs) Handles BtnInscrire.Click
        'If String.IsNullOrWhiteSpace(TextBoxUser.Text) Then
        '    Throw New ArgumentException("Le nom d'utilisateur ne peut être vide")
        'End If
        If Not CheckBox1.Checked Then
            LabelError.Text = "Veuillez acceptez les conditions de Pricieux"
            Return
        End If
        If PwdConfirm And CheckBox1.Checked Then
            CreationCompte()
        End If
    End Sub
    Private Async Sub CreationCompte()
        Dim NouvelUtilisateur = Await _authentification.CreationCompte(TextBoxUser.Text, TextBoxPwd.Text)

    End Sub

    Public Property Utilisateur As Utilisateurs

    Private _authentification As New Authentification()
    Private PwdConfirm As Boolean
End Class
