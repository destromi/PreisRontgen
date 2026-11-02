Option Strict On
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports MySqlConnector

Public NotInheritable Class ConfigurationBaseForm
    Inherits Form

    Private ReadOnly txtServeur As New TextBox()
    Private ReadOnly txtPort As New TextBox()
    Private ReadOnly txtBase As New TextBox()
    Private ReadOnly txtUtilisateur As New TextBox()
    Private ReadOnly txtMotDePasse As New TextBox()
    Private ReadOnly btnTesterEnregistrer As New Button()
    Private ReadOnly btnAnnuler As New Button()

    Public Sub New()
        Text = "Pricieux — Connexion à la base"
        StartPosition = FormStartPosition.CenterParent
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        ShowInTaskbar = False
        ClientSize = New Size(520, 430)
        BackColor = ThemePricieux.FondPage
        Font = New Font("Segoe UI", 9)

        Dim panneau As New Panel With {.Dock = DockStyle.Fill, .Padding = New Padding(28), .BackColor = Color.White}
        Controls.Add(panneau)
        panneau.Controls.Add(New Label With {.Text = "Connexion MySQL / MariaDB", .Font = New Font("Segoe UI", 17, FontStyle.Bold), .ForeColor = ThemePricieux.Foret, .AutoSize = True, .Location = New Point(26, 20)})

        AjouterChamp(panneau, "Serveur", txtServeur, 28, 72, 300)
        AjouterChamp(panneau, "Port", txtPort, 350, 72, 112)
        AjouterChamp(panneau, "Base de données", txtBase, 28, 142, 434)
        AjouterChamp(panneau, "Utilisateur MySQL", txtUtilisateur, 28, 212, 434)
        AjouterChamp(panneau, "Mot de passe", txtMotDePasse, 28, 282, 434)
        txtMotDePasse.UseSystemPasswordChar = True

        panneau.Controls.Add(New Label With {
            .Text = "Entrez le compte applicatif de votre base (vous n'avez pas besoin d'utiliser root). Le mot de passe sera chiffré pour votre compte Windows.",
            .Location = New Point(28, 342), .Size = New Size(434, 42), .ForeColor = Color.DimGray
        })

        btnTesterEnregistrer.Text = "Tester et enregistrer"
        btnTesterEnregistrer.Location = New Point(28, 386)
        btnTesterEnregistrer.Size = New Size(210, 34)
        btnTesterEnregistrer.BackColor = ThemePricieux.Action
        btnTesterEnregistrer.ForeColor = Color.White
        btnTesterEnregistrer.FlatStyle = FlatStyle.Flat
        AddHandler btnTesterEnregistrer.Click, AddressOf TesterEtEnregistrerAsync
        panneau.Controls.Add(btnTesterEnregistrer)

        btnAnnuler.Text = "Annuler"
        btnAnnuler.Location = New Point(342, 386)
        btnAnnuler.Size = New Size(120, 34)
        btnAnnuler.DialogResult = DialogResult.Cancel
        panneau.Controls.Add(btnAnnuler)

        AcceptButton = btnTesterEnregistrer
        CancelButton = btnAnnuler
        txtServeur.Text = "localhost"
        txtPort.Text = "3306"
        txtBase.Text = "pricieux_app"
        ChargerConnexionEnregistree()
    End Sub

    Private Shared Sub AjouterChamp(parent As Control, titre As String, champ As TextBox, x As Integer, y As Integer, largeur As Integer)
        parent.Controls.Add(New Label With {
            .Text = titre, .Location = New Point(x, y), .AutoSize = True,
            .Font = New Font("Segoe UI", 9, FontStyle.Bold), .ForeColor = ThemePricieux.TexteSecondaire
        })
        champ.Location = New Point(x, y + 23)
        champ.Size = New Size(largeur, 30)
        champ.Font = New Font("Segoe UI", 10)
        parent.Controls.Add(champ)
    End Sub

    Private Sub ChargerConnexionEnregistree()
        Dim connectionString As String = Configuration.LireChaineConnexionConfigurable()
        If String.IsNullOrWhiteSpace(connectionString) Then Return
        Try
            Dim builder As New MySqlConnectionStringBuilder(connectionString)
            txtServeur.Text = builder.Server
            txtPort.Text = builder.Port.ToString(CultureInfo.InvariantCulture)
            txtBase.Text = If(String.Equals(builder.Database, "pricieux", StringComparison.OrdinalIgnoreCase), "pricieux_app", builder.Database)
            txtUtilisateur.Text = builder.UserID
            txtMotDePasse.Text = builder.Password
        Catch
            ' Une configuration illisible peut être remplacée par les valeurs saisies ici.
        End Try
    End Sub

    Private Async Sub TesterEtEnregistrerAsync(sender As Object, e As EventArgs)
        btnTesterEnregistrer.Enabled = False
        btnAnnuler.Enabled = False
        UseWaitCursor = True
        Try
            Dim builder As MySqlConnectionStringBuilder = ConstruireChaineConnexion()
            Using connexion As New MySqlConnection(builder.ConnectionString)
                Await connexion.OpenAsync()
            End Using
            Configuration.EnregistrerConnexion(builder.ConnectionString)
            MessageBox.Show(Me, "Connexion vérifiée et enregistrée. Vous pourrez la modifier depuis cet écran.", "Base de données configurée", MessageBoxButtons.OK, MessageBoxIcon.Information)
            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            MessageBox.Show(Me, "La connexion n'a pas pu être vérifiée :" & Environment.NewLine & ex.Message, "Connexion à la base", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            UseWaitCursor = False
            If Not IsDisposed Then
                btnTesterEnregistrer.Enabled = True
                btnAnnuler.Enabled = True
            End If
        End Try
    End Sub

    Private Function ConstruireChaineConnexion() As MySqlConnectionStringBuilder
        Dim serveur As String = txtServeur.Text.Trim()
        Dim nomBase As String = txtBase.Text.Trim()
        Dim utilisateur As String = txtUtilisateur.Text.Trim()
        Dim port As UInteger
        If String.IsNullOrWhiteSpace(serveur) Then Throw New InvalidOperationException("Saisissez le nom du serveur MySQL.")
        If Not UInteger.TryParse(txtPort.Text.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, port) OrElse port = 0UI Then Throw New InvalidOperationException("Le port doit être un nombre supérieur à zéro.")
        If String.IsNullOrWhiteSpace(nomBase) Then Throw New InvalidOperationException("Saisissez le nom de la base de données.")
        If String.Equals(nomBase, "pricieux", StringComparison.OrdinalIgnoreCase) Then Throw New InvalidOperationException("Le nom 'pricieux' est réservé à votre base existante. Utilisez 'pricieux_app' pour l'application.")
        If String.IsNullOrWhiteSpace(utilisateur) Then Throw New InvalidOperationException("Saisissez le nom du compte MySQL créé pour l'application.")

        Return New MySqlConnectionStringBuilder With {
            .Server = serveur,
            .Port = port,
            .Database = nomBase,
            .UserID = utilisateur,
            .Password = txtMotDePasse.Text
        }
    End Function
End Class
