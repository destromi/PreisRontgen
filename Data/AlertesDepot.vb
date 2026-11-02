Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports MySqlConnector

Public NotInheritable Class AlertesDepot
    Public Async Function ListerAsync(utilisateurId As Long, dateDebut As Date?, dateFin As Date?, magasinId As Long?, type As String, nonLuesSeulement As Boolean) As Task(Of List(Of AlertePrix))
        Dim resultats As New List(Of AlertePrix)()
        Dim sql As String = "SELECT a.id, p.nom AS produit, p.marque, m.nom AS magasin, a.type, a.valeur, a.prix_precedent, a.prix_actuel, a.date_alerte, a.lue FROM alerte a INNER JOIN produit p ON p.id=a.id_produit INNER JOIN magasin m ON m.id=a.id_magasin WHERE a.id_utilisateur=@utilisateur"
        If dateDebut.HasValue Then sql &= " AND a.date_alerte >= @debut"
        If dateFin.HasValue Then sql &= " AND a.date_alerte < @fin"
        If magasinId.HasValue Then sql &= " AND a.id_magasin=@magasin"
        If Not String.IsNullOrWhiteSpace(type) AndAlso type <> "Tous" Then sql &= " AND a.type=@type"
        If nonLuesSeulement Then sql &= " AND a.lue=FALSE"
        sql &= " ORDER BY a.date_alerte DESC LIMIT 2000;"
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand(sql, connexion)
                commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                If dateDebut.HasValue Then commande.Parameters.AddWithValue("@debut", dateDebut.Value.Date)
                If dateFin.HasValue Then commande.Parameters.AddWithValue("@fin", dateFin.Value.Date.AddDays(1))
                If magasinId.HasValue Then commande.Parameters.AddWithValue("@magasin", magasinId.Value)
                If Not String.IsNullOrWhiteSpace(type) AndAlso type <> "Tous" Then commande.Parameters.AddWithValue("@type", type)
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    While Await lecteur.ReadAsync()
                        resultats.Add(New AlertePrix With {
                            .Id = lecteur.GetInt64("id"), .ProduitNom = lecteur.GetString("produit"), .Marque = lecteur.GetString("marque"),
                            .MagasinNom = lecteur.GetString("magasin"), .Type = lecteur.GetString("type"), .Valeur = lecteur.GetDecimal("valeur"),
                            .PrixPrecedent = lecteur.GetDecimal("prix_precedent"), .PrixActuel = lecteur.GetDecimal("prix_actuel"),
                            .DateAlerte = lecteur.GetDateTime("date_alerte"), .Lue = lecteur.GetBoolean("lue")
                        })
                    End While
                End Using
            End Using
        End Using
        Return resultats
    End Function

    Public Async Function CompterNonLuesAsync(utilisateurId As Long) As Task(Of Integer)
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand("SELECT COUNT(*) FROM alerte WHERE id_utilisateur=@utilisateur AND lue=FALSE;", connexion)
                commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                Return Convert.ToInt32(Await commande.ExecuteScalarAsync())
            End Using
        End Using
    End Function

    Public Async Function MarquerLueAsync(utilisateurId As Long, alerteId As Long) As Task
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand("UPDATE alerte SET lue=TRUE WHERE id=@id AND id_utilisateur=@utilisateur;", connexion)
                commande.Parameters.AddWithValue("@id", alerteId)
                commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                Await commande.ExecuteNonQueryAsync()
            End Using
        End Using
    End Function
End Class
