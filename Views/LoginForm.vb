Option Strict On
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public NotInheritable Class LoginForm
    Inherits Form

    Private ReadOnly _authentification As AuthentificationDepot
    Private ReadOnly txtLogin As New TextBox()
    Private ReadOnly txtMotDePasse As New TextBox()
    Private ReadOnly lblMessage As New Label()
    Private ReadOnly btnConnexion As New Button()
    Private ReadOnly lnkInscription As New LinkLabel()
    Private _formulairePrincipal As MainForm
    Public Property Utilisateur As UtilisateurSession

    Public Sub New(authentification As AuthentificationDepot)
        If authentification Is Nothing Then Throw New ArgumentNullException(NameOf(authentification))
        _authentification = authentification
        Text = "Connexion"
        Name = "LoginForm"
        StartPosition = FormStartPosition.CenterScreen
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        ClientSize = New Size(566, 390)
        BackColor = Color.FromArgb(240, 240, 240)
        Font = New Font("Georgia", 12.0F, FontStyle.Regular)

        Dim lblLogo As New Label With {
            .Text = "Pricieux",
            .Font = New Font("Georgia", 42.0F, FontStyle.Regular),
            .ForeColor = ThemePricieux.Foret,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Location = New Point(0, 14),
            .Size = New Size(ClientSize.Width, 66)
        }
        Controls.Add(lblLogo)

        Dim lblSousTitre As New Label With {
            .Text = ThemePricieux.Slogan,
            .Font = New Font("Georgia", 11.0F, FontStyle.Regular),
            .ForeColor = ThemePricieux.Foret,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Location = New Point(0, 77),
            .Size = New Size(ClientSize.Width, 24)
        }
        Controls.Add(lblSousTitre)

        AjouterEtiquette("Nom d'utilisateur :", 35, 139)
        txtLogin.Name = "txtLogin"
        txtLogin.Location = New Point(295, 131)
        txtLogin.Size = New Size(238, 29)
        txtLogin.Font = New Font("Georgia", 11.0F)
        Controls.Add(txtLogin)

        AjouterEtiquette("Mot de passe :", 35, 193)
        txtMotDePasse.Name = "txtMotDePasse"
        txtMotDePasse.Location = New Point(295, 185)
        txtMotDePasse.Size = New Size(238, 29)
        txtMotDePasse.Font = New Font("Georgia", 11.0F)
        txtMotDePasse.UseSystemPasswordChar = True
        Controls.Add(txtMotDePasse)

        lblMessage.Name = "lblMessage"
        lblMessage.Location = New Point(28, 225)
        lblMessage.Size = New Size(510, 34)
        lblMessage.ForeColor = Color.Firebrick
        lblMessage.Font = New Font("Georgia", 9.0F)
        lblMessage.TextAlign = ContentAlignment.MiddleCenter
        Controls.Add(lblMessage)

        btnConnexion.Name = "btnConnexion"
        btnConnexion.Text = "Se connecter"
        btnConnexion.Location = New Point(171, 271)
        btnConnexion.Size = New Size(224, 48)
        btnConnexion.BackColor = ThemePricieux.Action
        btnConnexion.ForeColor = Color.White
        btnConnexion.Font = New Font("Georgia", 20.0F, FontStyle.Regular)
        btnConnexion.FlatStyle = FlatStyle.Flat
        btnConnexion.FlatAppearance.BorderColor = ThemePricieux.Foret
        AddHandler btnConnexion.Click, AddressOf ConnecterAsync
        Controls.Add(btnConnexion)

        lnkInscription.Name = "lnkInscription"
        lnkInscription.Text = "S'inscrire"
        lnkInscription.Location = New Point(0, 325)
        lnkInscription.Size = New Size(ClientSize.Width, 27)
        lnkInscription.Font = New Font("Georgia", 12.0F)
        lnkInscription.LinkColor = ThemePricieux.Foret
        lnkInscription.ActiveLinkColor = ThemePricieux.Action
        lnkInscription.VisitedLinkColor = ThemePricieux.Foret
        lnkInscription.TextAlign = ContentAlignment.MiddleCenter
        AddHandler lnkInscription.LinkClicked, AddressOf OuvrirInscription
        Controls.Add(lnkInscription)

        AcceptButton = btnConnexion
        CancelButton = Nothing
    End Sub

    Private Sub AjouterEtiquette(texte As String, x As Integer, y As Integer)
        Controls.Add(New Label With {
            .Text = texte,
            .Location = New Point(x, y),
            .Size = New Size(245, 27),
            .TextAlign = ContentAlignment.MiddleLeft,
            .Font = New Font("Georgia", 13.0F, FontStyle.Regular),
            .ForeColor = ThemePricieux.Texte
        })
    End Sub

    Private Sub OuvrirInscription(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Using formulaire As New InscriptionForm(_authentification)
            If formulaire.ShowDialog(Me) = DialogResult.OK AndAlso formulaire.Utilisateur IsNot Nothing Then
                txtLogin.Text = formulaire.Utilisateur.Login
                txtMotDePasse.Clear()
                lblMessage.Text = "Compte créé. Connectez-vous avec vos identifiants."
                lblMessage.ForeColor = ThemePricieux.Action
                txtMotDePasse.Focus()
            End If
        End Using
    End Sub

    Private Async Sub ConnecterAsync(sender As Object, e As EventArgs)
        Await ExecuterAsync(ConnecterAvecReconfigurationAsync())
    End Sub

    Private Async Function ConnecterAvecReconfigurationAsync() As Threading.Tasks.Task
        Dim utilisateurConnecte As UtilisateurSession = Nothing
        Dim erreurConnexion As Exception = Nothing
        Try
            utilisateurConnecte = Await _authentification.ConnecterAsync(txtLogin.Text, txtMotDePasse.Text)
        Catch ex As Exception
            erreurConnexion = ex
        End Try

        If erreurConnexion IsNot Nothing Then
            Dim choix As DialogResult = MessageBox.Show(Me,
                "La connexion à la base de données a échoué. Voulez-vous vérifier ou modifier sa configuration maintenant ?",
                "Base de données inaccessible", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
            If choix <> DialogResult.Yes Then
                lblMessage.Text = erreurConnexion.Message
                Return
            End If

            Using formulaire As New ConfigurationBaseForm()
                If formulaire.ShowDialog(Me) <> DialogResult.OK Then
                    lblMessage.Text = "La connexion à la base n'a pas été modifiée."
                    Return
                End If
            End Using

            Try
                utilisateurConnecte = Await _authentification.ConnecterAsync(txtLogin.Text, txtMotDePasse.Text)
            Catch nouvelleException As Exception
                lblMessage.Text = nouvelleException.Message
                Return
            End Try
        End If

        If utilisateurConnecte Is Nothing Then
            lblMessage.Text = "Identifiant ou mot de passe incorrect."
            Return
        End If

        Utilisateur = utilisateurConnecte
        AfficherPrincipal()
    End Function

    Private Sub AfficherPrincipal()
        Hide()
        Try
            _formulairePrincipal = New MainForm(Utilisateur)
            AddHandler _formulairePrincipal.FormClosed, AddressOf PrincipalFerme
            _formulairePrincipal.Show()
            _formulairePrincipal.Activate()
        Catch ex As Exception
            Show()
            lblMessage.Text = ex.Message
        End Try
    End Sub

    Private Sub PrincipalFerme(sender As Object, e As FormClosedEventArgs)
        Dim formulaire As MainForm = DirectCast(sender, MainForm)
        RemoveHandler formulaire.FormClosed, AddressOf PrincipalFerme
        _formulairePrincipal = Nothing
        If formulaire.DemandeDeconnexion Then
            Utilisateur = Nothing
            txtMotDePasse.Clear()
            lblMessage.Text = String.Empty
            Show()
            Activate()
        Else
            Close()
        End If
    End Sub

    Private Async Function ExecuterAsync(operation As Threading.Tasks.Task) As Threading.Tasks.Task
        btnConnexion.Enabled = False
        lnkInscription.Enabled = False
        lblMessage.ForeColor = Color.Firebrick
        lblMessage.Text = String.Empty
        Try
            Await operation
        Catch ex As Exception
            lblMessage.Text = ex.Message
        Finally
            If Not IsDisposed Then
                btnConnexion.Enabled = True
                lnkInscription.Enabled = True
            End If
        End Try
    End Function
End Class
