Option Strict On
Option Explicit On

Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports MySqlConnector

Public NotInheritable Class Configuration
    Private Const NomVariableConnexion As String = "PRICIEUX_CONNECTION_STRING"
    Private Const NomAncienneBase As String = "pricieux"
    Private Shared ReadOnly Entropie As Byte() = Encoding.UTF8.GetBytes("Pricieux.Database.Connection.v1")

    Private Sub New()
    End Sub

    Public Shared Function CreerConnexion() As MySqlConnection
        Dim connectionString As String = LireChaineConnexionConfigurable()
        If String.IsNullOrWhiteSpace(connectionString) Then
            Throw New InvalidOperationException("Configurez d'abord la connexion à la base depuis l'écran de connexion.")
        End If
        Dim builder As New MySqlConnectionStringBuilder(connectionString)
        If String.Equals(builder.Database, NomAncienneBase, StringComparison.OrdinalIgnoreCase) Then
            Throw New InvalidOperationException("La connexion pointe encore vers votre base 'pricieux'. Reconfigurez l'application pour utiliser 'pricieux_app'.")
        End If
        Return New MySqlConnection(builder.ConnectionString)
    End Function

    Public Shared Function ConnexionConfiguree() As Boolean
        Dim connectionString As String = LireChaineConnexionConfigurable()
        If String.IsNullOrWhiteSpace(connectionString) Then Return False
        Try
            Dim builder As New MySqlConnectionStringBuilder(connectionString)
            Return Not String.Equals(builder.Database, NomAncienneBase, StringComparison.OrdinalIgnoreCase)
        Catch
            Return True
        End Try
    End Function

    Public Shared Function LireChaineConnexionConfigurable() As String
        Dim connectionString As String = LireConnexionEnregistree()
        If String.IsNullOrWhiteSpace(connectionString) Then connectionString = Environment.GetEnvironmentVariable(NomVariableConnexion)
        Return If(connectionString, String.Empty)
    End Function

    Public Shared Function LireConnexionEnregistree() As String
        Dim chemin As String = CheminFichierConnexion()
        If Not File.Exists(chemin) Then Return String.Empty
        Try
            Dim donneesChiffrees As Byte() = File.ReadAllBytes(chemin)
            Dim donnees As Byte() = ProtectedData.Unprotect(donneesChiffrees, Entropie, DataProtectionScope.CurrentUser)
            Return Encoding.UTF8.GetString(donnees)
        Catch ex As CryptographicException
            Return String.Empty
        End Try
    End Function

    Public Shared Sub EnregistrerConnexion(connectionString As String)
        If String.IsNullOrWhiteSpace(connectionString) Then Throw New ArgumentException("La chaîne de connexion est obligatoire.", NameOf(connectionString))
        Dim builder As New MySqlConnectionStringBuilder(connectionString)
        Dim donnees As Byte() = Encoding.UTF8.GetBytes(builder.ConnectionString)
        Dim donneesChiffrees As Byte() = ProtectedData.Protect(donnees, Entropie, DataProtectionScope.CurrentUser)
        Dim chemin As String = CheminFichierConnexion()
        Dim dossier As String = Path.GetDirectoryName(chemin)
        Directory.CreateDirectory(dossier)
        Dim temporaire As String = chemin & ".tmp"
        File.WriteAllBytes(temporaire, donneesChiffrees)
        File.Move(temporaire, chemin, True)
    End Sub

    Private Shared Function CheminFichierConnexion() As String
        Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Pricieux", "connexion.bin")
    End Function

    Public Shared Function CheminTesseract As String
        Return Environment.GetEnvironmentVariable("PRICIEUX_TESSERACT_PATH")
    End Function
End Class
