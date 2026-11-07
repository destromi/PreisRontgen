Option Strict On
Option Explicit On

Imports MySql.Data.MySqlClient

Public Module ConnexionBD

    ''' <summary>
    ''' Récupère la chaîne de connexion depuis les variables
    ''' d'environnement Windows.
    ''' </summary>
    Private Function ObtenirChaineConnexion() As String

        Dim chaine As String =
            Environment.GetEnvironmentVariable("PRICIEUX_DB_CONNECTION")

        If String.IsNullOrWhiteSpace(chaine) Then
            Throw New InvalidOperationException(
                "La variable d'environnement PRICIEUX_DB_CONNECTION " &
                "n'est pas configurée.")
        End If

        Return chaine

    End Function

    ''' <summary>
    ''' Crée une nouvelle connexion MySQL.
    ''' </summary>
    Public Function CreerConnexion() As MySqlConnection

        Return New MySqlConnection(ObtenirChaineConnexion())

    End Function

    ''' <summary>
    ''' Vérifie que la connexion à la base de données fonctionne.
    ''' </summary>
    Public Function TesterConnexion() As Boolean

        Try
            Using connexion As MySqlConnection = CreerConnexion()

                connexion.Open()

                Return connexion.State = ConnectionState.Open

            End Using

        Catch ex As MySqlException
            Throw New ApplicationException(
                "Impossible de se connecter à la base MySQL. " &
                "Vérifiez le serveur et les paramètres de connexion.", ex)

        End Try

    End Function

End Module