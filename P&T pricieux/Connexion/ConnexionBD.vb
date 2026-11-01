Imports MySql.Data.MySqlClient
Imports System.Configuration
Public Class ConnexionBD
    Private Shared ReadOnly chaineConnexion As String =
       ConfigurationManager.ConnectionStrings("PricieuxDB").ConnectionString

    Public Shared Function ObtenirConnexion() As MySqlConnection
        Return New MySqlConnection(chaineConnexion)
    End Function

    Public Shared Function TesterConnexion() As Boolean

        Try
            Using connexion As MySqlConnection = ObtenirConnexion()
                connexion.Open()
                Return True
            End Using

        Catch ex As Exception
            MessageBox.Show("Erreur de connexion : " & ex.Message)
            Return False
        End Try

    End Function
End Class

