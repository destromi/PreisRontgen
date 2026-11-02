Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports MySqlConnector

Public NotInheritable Class AuthentificationDepot
    Public Async Function ListerUtilisateursAsync() As Task(Of List(Of UtilisateurSession))
        Dim resultat As New List(Of UtilisateurSession)()
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand("SELECT id, login, role, actif FROM utilisateur ORDER BY login;", connexion)
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    While Await lecteur.ReadAsync()
                        resultat.Add(New UtilisateurSession With {.Id = lecteur.GetInt64("id"), .Login = lecteur.GetString("login"), .Role = lecteur.GetString("role"), .Actif = lecteur.GetBoolean("actif")})
                    End While
                End Using
            End Using
        End Using
        Return resultat
    End Function

    Public Async Function EnregistrerUtilisateurAsync(id As Long, role As String, actif As Boolean, administrateurConnecteId As Long) As Task
        If role <> "ADMIN" AndAlso role <> "UTILISATEUR" Then Throw New ArgumentException("Rôle utilisateur invalide.")
        If id = administrateurConnecteId Then Throw New InvalidOperationException("Le compte actuellement connecté ne peut pas modifier son propre rôle ou son statut.")
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using transaction As MySqlTransaction = DirectCast(Await connexion.BeginTransactionAsync(), MySqlTransaction)
                Dim administrateurActifDifferent As Boolean = False
                Using commande As New MySqlCommand("SELECT id FROM utilisateur WHERE role='ADMIN' AND actif=TRUE ORDER BY id FOR UPDATE;", connexion, transaction)
                    Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                        While Await lecteur.ReadAsync()
                            If lecteur.GetInt64("id") <> id Then administrateurActifDifferent = True
                        End While
                    End Using
                End Using
                If (role <> "ADMIN" OrElse Not actif) AndAlso Not administrateurActifDifferent Then Throw New InvalidOperationException("Au moins un compte administrateur actif doit être conservé.")
                Using commande As New MySqlCommand("UPDATE utilisateur SET role=@role, actif=@actif WHERE id=@id;", connexion, transaction)
                    commande.Parameters.AddWithValue("@role", role)
                    commande.Parameters.AddWithValue("@actif", actif)
                    commande.Parameters.AddWithValue("@id", id)
                    If Await commande.ExecuteNonQueryAsync() <> 1 Then Throw New InvalidOperationException("Le compte n'existe plus.")
                End Using
                Await transaction.CommitAsync()
            End Using
        End Using
    End Function

    Public Async Function ConnecterAsync(login As String, motDePasse As String) As Task(Of UtilisateurSession)
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Const sql As String = "SELECT id, login, mot_de_passe_hash, sel, role FROM utilisateur WHERE login = @login AND actif = TRUE LIMIT 1;"
            Using commande As New MySqlCommand(sql, connexion)
                commande.Parameters.AddWithValue("@login", login.Trim())
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    If Not Await lecteur.ReadAsync() Then Return Nothing
                    Dim sel As Byte() = DirectCast(lecteur("sel"), Byte())
                    Dim hash As Byte() = DirectCast(lecteur("mot_de_passe_hash"), Byte())
                    If Not HachageMotDePasse.Verifier(motDePasse, sel, hash) Then Return Nothing
                    Return New UtilisateurSession With {
                        .Id = Convert.ToInt64(lecteur("id")),
                        .Login = Convert.ToString(lecteur("login")),
                        .Role = Convert.ToString(lecteur("role"))
                    }
                End Using
            End Using
        End Using
    End Function

    Public Async Function CreerCompteAsync(login As String, motDePasse As String) As Task(Of UtilisateurSession)
        Dim loginNettoye As String = If(login, String.Empty).Trim()
        If loginNettoye.Length < 3 OrElse loginNettoye.Length > 80 Then Throw New ArgumentException("L'identifiant doit contenir entre 3 et 80 caractères.")
        If motDePasse Is Nothing OrElse motDePasse.Length < 10 Then Throw New ArgumentException("Le mot de passe doit contenir au moins 10 caractères.")

        Dim resultat = HachageMotDePasse.Creer(motDePasse)
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commandeVerrou As New MySqlCommand("SELECT GET_LOCK('pricieux-premier-compte', 10);", connexion)
                Dim verrou As Object = Await commandeVerrou.ExecuteScalarAsync()
                If verrou Is Nothing OrElse Convert.ToInt32(verrou) <> 1 Then Throw New InvalidOperationException("Impossible de sécuriser la création du compte. Réessayez.")
            End Using
            Try
                Using transaction As MySqlTransaction = DirectCast(Await connexion.BeginTransactionAsync(), MySqlTransaction)
                    Dim nombreComptes As Long
                    Using compte As New MySqlCommand("SELECT COUNT(*) FROM utilisateur;", connexion, transaction)
                        nombreComptes = Convert.ToInt64(Await compte.ExecuteScalarAsync())
                    End Using
                    Dim role As String = If(nombreComptes = 0, "ADMIN", "UTILISATEUR")
                    Const sql As String = "INSERT INTO utilisateur (login, mot_de_passe_hash, sel, role) VALUES (@login, @hash, @sel, @role);"
                    Using insertion As New MySqlCommand(sql, connexion, transaction)
                        insertion.Parameters.AddWithValue("@login", loginNettoye)
                        insertion.Parameters.Add("@hash", MySqlDbType.VarBinary, resultat.Hachage.Length).Value = resultat.Hachage
                        insertion.Parameters.Add("@sel", MySqlDbType.VarBinary, resultat.Sel.Length).Value = resultat.Sel
                        insertion.Parameters.AddWithValue("@role", role)
                        Await insertion.ExecuteNonQueryAsync()
                        Dim id As Long = insertion.LastInsertedId
                        Await transaction.CommitAsync()
                        Return New UtilisateurSession With {.Id = id, .Login = loginNettoye, .Role = role}
                    End Using
                End Using
            Finally
                Using commandeLiberation As New MySqlCommand("SELECT RELEASE_LOCK('pricieux-premier-compte');", connexion)
                    commandeLiberation.ExecuteScalar()
                End Using
            End Try
        End Using
    End Function
End Class
