Option Strict On
Option Explicit On

Imports MySqlConnector

Public Class Authentification
    Public Async Function CreationCompte(username As String, Pwd As String, condition As Boolean) As Task(Of Utilisateurs)
        Dim verifUsername As String = If(username, String.Empty).Trim()
        If verifUsername.Length < 3 OrElse verifUsername.Length > 30 Then Throw New ArgumentException("Le nom de l'utilisateur doit avoir entre 3 et 30 caractères")
        If Pwd Is Nothing OrElse Pwd.Length < 8 Then Throw New ArgumentException("Le mot de passe doit contenir au moins 08 caractères")
        If Not condition Then Throw New ArgumentException("Veuillez acceptez les conditions d'utilisations de Pricieux.")

        Dim resultatPwd As String() = Hachage_Mot_de_passe.PwdHash(Pwd).Split(".") 'hashage du mot de passe 
        Using connection As MySqlConnection = GetConnection 'connection avec la BD
            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
                'MessageBox.Show("connexion avec la base reussi")
                sql = "Select Get_Lock('nou_compte', 30);"
                Using commandVerrou As New MySqlCommand(sql, connection)
                    Dim verrou As Object = commandVerrou.ExecuteScalar
                    If verrou Is Nothing OrElse Convert.ToInt32(verrou) <> 1 Then Throw New InvalidOperationException("Erreur lors de la creation du compte. Reéssayer!!!")
                End Using
                Try
                    Using Transaction As MySqlTransaction = DirectCast(Await connection.BeginTransactionAsync, MySqlTransaction)
                        Dim nombreCompte As Integer
                        sql = "SELECT COUNT(*) From utilisateur"
                        Using compte As New MySqlCommand(sql, connection, Transaction)
                            nombreCompte = Convert.ToInt32(Await compte.ExecuteScalarAsync)
                        End Using
                        Dim Role As String = If(nombreCompte = 0, "Admin", "User") 'Attribution du rôle
                        sql = "INSERT INTO Utilisateur (login, mot_de_passe_hash, sel, role) value (@login, @hash, @sel, @role)"
                        Using insertion As New MySqlCommand(sql, connection, Transaction) 'insertion en Base de données 
                            insertion.Parameters.AddWithValue("@login", verifUsername)
                            insertion.Parameters.Add("@hash", MySqlDbType.VarChar, resultatPwd(2).Length).Value = resultatPwd(2)
                            insertion.Parameters.Add("@sel", MySqlDbType.VarChar, resultatPwd(1).Length).Value = resultatPwd(1)
                            insertion.Parameters.AddWithValue("role", Role)
                            Await insertion.ExecuteNonQueryAsync()
                            Dim id As Integer = CInt(insertion.LastInsertedId)
                            Await Transaction.CommitAsync()
                            Return New Utilisateurs With {.Id = id, .Login = verifUsername, .Role = Role}
                        End Using
                    End Using
                Finally
                    sql = "Select Release_lock('nou_compte');"
                    Using LiberationConnection As New MySqlCommand(sql, connection)
                        LiberationConnection.ExecuteScalar()
                    End Using
                End Try
            End If
        End Using
    End Function

    Public Async Function ConnectionCompte(username As String, Pwd As String) As Task(Of Utilisateurs)
        Dim verifUsername = If(username, String.Empty).Trim()
        If verifUsername Is String.Empty Then Throw New ArgumentException("Le nom de l'utilisateur ne peut être vide")

        Using connection As MySqlConnection = GetConnection 'Connexion avec la Base de données
            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync
                'MessageBox.Show("connexion avec la base reussi")
                sql = "Select * from utilisateur where login = @login"
                Using selection As New MySqlCommand(sql, connection)
                    selection.Parameters.AddWithValue("@login", verifUsername)
                    Using resultat As MySqlDataReader = DirectCast(Await selection.ExecuteReaderAsync, MySqlDataReader)

                        If Await resultat.ReadAsync Then
                            Dim sel As String = DirectCast(resultat("sel"), String)
                            Dim hash As String = DirectCast(resultat("mot_de_passe_hash"), String)
                            Dim verification As Boolean = Hachage_Mot_de_passe.VerifPasd(Pwd, sel, hash)
                            If Not verification Then Throw New ArgumentException("le mot de passe est incorrect")
                            Return New Utilisateurs With {
                                .Id = CInt(resultat("Id")),
                                .Login = verifUsername,
                                .Role = DirectCast(resultat("role"), String)
                                }
                        Else
                            Throw New ArgumentException("Le nom de l'utilisateur est invalide")
                        End If
                    End Using
                End Using
            End If
        End Using
    End Function

    Private sql As String
End Class
