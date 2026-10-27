Option Strict On
Option Explicit On

Imports MySqlConnector

Public Class Authentification
    Private sql As String
    Public Async Function CreationCompte(username As String, Pwsd As String) As Task(Of Utilisateurs)
        Using connection As MySqlConnection = GetConnection
            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
                MessageBox.Show("connexion avec la base reussi")
                sql = "Select Get_Lock('nou_compte', 30);"
                Using commandVerrou As New MySqlCommand(sql, connection)
                    Dim verrou As Object = commandVerrou.ExecuteScalar
                    If verrou Is Nothing OrElse Convert.ToInt32(verrou) <> 1 Then

                    End If
                End Using
            End If
        End Using
    End Function
End Class
