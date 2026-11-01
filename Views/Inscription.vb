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
        'If Not CheckBox1.Checked Then
        '    LabelError.Text = "Veuillez acceptez les conditions de Pricieux"
        '    Return
        'End If
        If PwdConfirm Then
            CreationCompte()
        End If
    End Sub
    Private Async Sub CreationCompte()
        Try
            Dim NouvelUtilisateur As Utilisateurs = Await _authentification.CreationCompte(TextBoxUser.Text, TextBoxPwd.Text, CheckBox1.Checked)
            Hide()
            MessageBox.Show("Bienvenue Monsieur " & NouvelUtilisateur.Login)
        Catch ex As ArgumentException
            LabelError.Text = ex.Message
        End Try

    End Sub

    Private Sub labelConn(sender As Object, e As EventArgs) Handles LabelConnecter.MouseEnter
        LabelConnecter.Font = New Font(LabelConnecter.Font, FontStyle.Underline)
        LabelConnecter.Cursor = Cursors.Hand
    End Sub

    Private Sub labelConne(sender As Object, e As EventArgs) Handles LabelConnecter.MouseLeave
        LabelConnecter.Font = New Font(LabelConnecter.Font, FontStyle.Regular)
    End Sub

    Private Sub BtnIns(sender As Object, e As EventArgs) Handles BtnInscrire.MouseEnter
        BtnInscrire.BackColor = Color.ForestGreen
        BtnInscrire.Cursor = Cursors.Hand
    End Sub

    Private Sub BtnInse(sender As Object, e As EventArgs) Handles BtnInscrire.MouseLeave
        BtnInscrire.BackColor = Color.LimeGreen
    End Sub

    Public Property Utilisateur As Utilisateurs

    Private _authentification As New Authentification()
    Private PwdConfirm As Boolean

End Class
