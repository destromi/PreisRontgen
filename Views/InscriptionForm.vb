Option Strict On
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public NotInheritable Class InscriptionForm
    Inherits Form

    Private ReadOnly _authentification As AuthentificationDepot
    Private ReadOnly txtLogin As New TextBox()
    Private ReadOnly txtMotDePasse As New TextBox()
    Private ReadOnly txtConfirmation As New TextBox()
    Private ReadOnly chkConditions As New CheckBox()
    Private ReadOnly lblMessage As New Label()
    Private ReadOnly btnInscrire As New Button()
    Private ReadOnly lnkConnexion As New LinkLabel()

    Public Property Utilisateur As UtilisateurSession

    Public Sub New(authentification As AuthentificationDepot)
        If authentification Is Nothing Then Throw New ArgumentNullException(NameOf(authentification))
        _authentification = authentification
        Text = "Inscription"
        Name = "InscriptionForm"
        StartPosition = FormStartPosition.CenterParent
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        ClientSize = New Size(580, 488)
        BackColor = Color.FromArgb(240, 240, 240)
        Font = New Font("Georgia", 12.0F, FontStyle.Regular)

        Dim lblLogo As New Label With {
            .Text = "Pricieux",
            .Font = New Font("Georgia", 42.0F, FontStyle.Regular),
            .ForeColor = ThemePricieux.Foret,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Location = New Point(0, 20),
            .Size = New Size(ClientSize.Width, 66)
        }
        Controls.Add(lblLogo)

        Dim lblSousTitre As New Label With {
            .Text = ThemePricieux.Slogan,
            .Font = New Font("Georgia", 11.0F, FontStyle.Regular),
            .ForeColor = ThemePricieux.Foret,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Location = New Point(0, 84),
            .Size = New Size(ClientSize.Width, 24)
        }
        Controls.Add(lblSousTitre)

        AjouterEtiquette("Nom d'utilisateur :", 36, 151)
        txtLogin.Name = "txtInscriptionLogin"
        txtLogin.Location = New Point(299, 143)
        txtLogin.Size = New Size(237, 29)
        txtLogin.Font = New Font("Georgia", 11.0F)
        txtLogin.MaxLength = 80
        Controls.Add(txtLogin)

        AjouterEtiquette("Mot de passe :", 36, 204)
        txtMotDePasse.Name = "txtInscriptionMotDePasse"
        txtMotDePasse.Location = New Point(299, 196)
        txtMotDePasse.Size = New Size(237, 29)
        txtMotDePasse.Font = New Font("Georgia", 11.0F)
        txtMotDePasse.UseSystemPasswordChar = True
        Controls.Add(txtMotDePasse)

        AjouterEtiquette("Confirmer le mot de passe :", 36, 257)
        txtConfirmation.Name = "txtConfirmationMotDePasse"
        txtConfirmation.Location = New Point(299, 249)
        txtConfirmation.Size = New Size(237, 29)
        txtConfirmation.Font = New Font("Georgia", 11.0F)
        txtConfirmation.UseSystemPasswordChar = True
        Controls.Add(txtConfirmation)

        chkConditions.Name = "chkConditions"
        chkConditions.Text = "Acceptez les conditions d'utilisations de Pricieux"
        chkConditions.Location = New Point(34, 318)
        chkConditions.Size = New Size(510, 30)
        chkConditions.Font = New Font("Georgia", 11.0F)
        chkConditions.ForeColor = ThemePricieux.Texte
        Controls.Add(chkConditions)

        lblMessage.Name = "lblMessageInscription"
        lblMessage.Location = New Point(25, 351)
        lblMessage.Size = New Size(530, 31)
        lblMessage.Font = New Font("Georgia", 9.0F)
        lblMessage.ForeColor = Color.Firebrick
        lblMessage.TextAlign = ContentAlignment.MiddleCenter
        Controls.Add(lblMessage)

        btnInscrire.Name = "btnInscrire"
        btnInscrire.Text = "S'inscrire"
        btnInscrire.Location = New Point(211, 389)
        btnInscrire.Size = New Size(158, 47)
        btnInscrire.BackColor = ThemePricieux.Action
        btnInscrire.ForeColor = Color.White
        btnInscrire.Font = New Font("Georgia", 20.0F, FontStyle.Regular)
        btnInscrire.FlatStyle = FlatStyle.Flat
        btnInscrire.FlatAppearance.BorderColor = ThemePricieux.Foret
        AddHandler btnInscrire.Click, AddressOf InscrireAsync
        Controls.Add(btnInscrire)

        lnkConnexion.Name = "lnkConnexion"
        lnkConnexion.Text = "Se connecter"
        lnkConnexion.Location = New Point(0, 443)
        lnkConnexion.Size = New Size(ClientSize.Width, 28)
        lnkConnexion.Font = New Font("Georgia", 12.0F)
        lnkConnexion.LinkColor = ThemePricieux.Foret
        lnkConnexion.ActiveLinkColor = ThemePricieux.Action
        lnkConnexion.VisitedLinkColor = ThemePricieux.Foret
        lnkConnexion.TextAlign = ContentAlignment.MiddleCenter
        AddHandler lnkConnexion.LinkClicked, Sub(sender As Object, e As LinkLabelLinkClickedEventArgs) Close()
        Controls.Add(lnkConnexion)

        AcceptButton = btnInscrire
    End Sub

    Private Sub AjouterEtiquette(texte As String, x As Integer, y As Integer)
        Controls.Add(New Label With {
            .Text = texte,
            .Location = New Point(x, y),
            .Size = New Size(255, 27),
            .TextAlign = ContentAlignment.MiddleLeft,
            .Font = New Font("Georgia", 13.0F, FontStyle.Regular),
            .ForeColor = ThemePricieux.Texte
        })
    End Sub

    Private Async Sub InscrireAsync(sender As Object, e As EventArgs)
        lblMessage.Text = String.Empty
        If Not chkConditions.Checked Then
            lblMessage.Text = "Veuillez accepter les conditions d'utilisation."
            Return
        End If
        If Not String.Equals(txtMotDePasse.Text, txtConfirmation.Text, StringComparison.Ordinal) Then
            lblMessage.Text = "Les deux mots de passe ne correspondent pas."
            Return
        End If

        btnInscrire.Enabled = False
        lnkConnexion.Enabled = False
        Try
            Utilisateur = Await _authentification.CreerCompteAsync(txtLogin.Text, txtMotDePasse.Text)
            Dim message As String = If(Utilisateur.EstAdministrateur,
                "Le premier compte dispose du rôle administrateur.",
                "Votre compte est créé. Vous pouvez maintenant vous connecter.")
            MessageBox.Show(Me, message, "Compte créé", MessageBoxButtons.OK, MessageBoxIcon.Information)
            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            lblMessage.Text = ex.Message
        Finally
            If Not IsDisposed Then
                btnInscrire.Enabled = True
                lnkConnexion.Enabled = True
            End If
        End Try
    End Sub
End Class
