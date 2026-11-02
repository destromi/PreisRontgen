
Option Strict On
Option Explicit On

Imports MySql.Data.MySqlClient
Imports System.Data

Public Class ProduitRepository

    Private Shared Function VersDb(texte As String) As Object
        If String.IsNullOrWhiteSpace(texte) Then
            Return DBNull.Value
        End If

        Return texte.Trim()
    End Function

    Private Const SelectDetails As String =
        "SELECT p.id, p.nom, IFNULL(p.marque,'') AS marque, " &
        "p.id_categorie, c.nom AS categorie, " &
        "p.unite_base, IFNULL(p.code_barres,'') AS code_barres " &
        "FROM produit p " &
        "LEFT JOIN categorie c ON c.id = p.id_categorie "

    ' Charger les catégories
    Public Function ChargerCategories() As DataTable

        Dim table As New DataTable()

        Using connexion As MySqlConnection = ConnexionBD.ObtenirConnexion()
            connexion.Open()

            Using commande As New MySqlCommand(
                "SELECT id, nom FROM categorie ORDER BY nom", connexion)

                Using adaptateur As New MySqlDataAdapter(commande)
                    adaptateur.Fill(table)
                End Using

            End Using
        End Using

        Return table

    End Function

    ' Charger tous les produits
    Public Function ChargerProduits() As DataTable

        Dim table As New DataTable()

        Using connexion As MySqlConnection = ConnexionBD.ObtenirConnexion()
            connexion.Open()

            Using commande As New MySqlCommand(
                SelectDetails & "ORDER BY p.nom", connexion)

                Using adaptateur As New MySqlDataAdapter(commande)
                    adaptateur.Fill(table)
                End Using

            End Using
        End Using

        Return table

    End Function

    ' Charger les détails pour le tableau du formulaire
    Public Function ListerDetails() As DataTable
        Return ChargerProduits()
    End Function

    ' Rechercher un produit par code-barres
    Public Function ObtenirParCodeBarres(code As String) As DataTable

        Dim table As New DataTable()

        Using connexion As MySqlConnection = ConnexionBD.ObtenirConnexion()
            connexion.Open()

            Using commande As New MySqlCommand(
                SelectDetails & "WHERE p.code_barres = @cb", connexion)

                commande.Parameters.AddWithValue("@cb", code.Trim())

                Using adaptateur As New MySqlDataAdapter(commande)
                    adaptateur.Fill(table)
                End Using

            End Using
        End Using

        Return table

    End Function

    ' Obtenir l'unité de base d'un produit
    Public Function ObtenirUniteBase(idProduit As Integer) As String

        Using connexion As MySqlConnection = ConnexionBD.ObtenirConnexion()
            connexion.Open()

            Using commande As New MySqlCommand(
                "SELECT unite_base FROM produit WHERE id = @id", connexion)

                commande.Parameters.AddWithValue("@id", idProduit)

                Dim resultat As Object = commande.ExecuteScalar()

                If resultat Is Nothing OrElse IsDBNull(resultat) Then
                    Return ""
                End If

                Return Convert.ToString(resultat)

            End Using
        End Using

    End Function

    ' Vérifier les doublons
    Public Function MessageDoublon(
        id As Integer,
        nom As String,
        marque As String,
        code As String
    ) As String

        Using connexion As MySqlConnection = ConnexionBD.ObtenirConnexion()
            connexion.Open()

            Dim sql As String =
                "SELECT " &
                "(SELECT COUNT(*) FROM produit " &
                "WHERE id <> @id AND nom = @nom " &
                "AND IFNULL(marque,'') = @marque) AS dn, " &
                "(SELECT COUNT(*) FROM produit " &
                "WHERE id <> @id AND @cb IS NOT NULL " &
                "AND code_barres = @cb) AS dc"

            Using commande As New MySqlCommand(sql, connexion)

                commande.Parameters.AddWithValue("@id", id)
                commande.Parameters.AddWithValue("@nom", nom.Trim())
                commande.Parameters.AddWithValue("@marque", marque.Trim())
                commande.Parameters.AddWithValue("@cb", VersDb(code))

                Using lecteur As MySqlDataReader = commande.ExecuteReader()

                    If lecteur.Read() Then

                        If Convert.ToInt64(lecteur("dn")) > 0 Then
                            Return "Un produit avec ce nom et cette marque existe déjà."
                        End If

                        If Convert.ToInt64(lecteur("dc")) > 0 Then
                            Return "Ce code-barres est déjà utilisé."
                        End If

                    End If

                End Using
            End Using
        End Using

        Return ""

    End Function

    ' Ajouter un produit
    Public Function Ajouter(
        nom As String,
        marque As String,
        idCategorie As Integer,
        uniteBase As String,
        code As String
    ) As Integer

        Using connexion As MySqlConnection = ConnexionBD.ObtenirConnexion()
            connexion.Open()

            Dim sql As String =
                "INSERT INTO produit " &
                "(nom, marque, id_categorie, unite_base, code_barres) " &
                "VALUES (@nom, @marque, @cat, @unite, @cb)"

            Using commande As New MySqlCommand(sql, connexion)

                commande.Parameters.AddWithValue("@nom", nom.Trim())
                commande.Parameters.AddWithValue("@marque", VersDb(marque))
                commande.Parameters.AddWithValue("@cat", idCategorie)
                commande.Parameters.AddWithValue("@unite", uniteBase)
                commande.Parameters.AddWithValue("@cb", VersDb(code))

                commande.ExecuteNonQuery()

                Return CInt(commande.LastInsertedId)

            End Using
        End Using

    End Function

    ' Modifier un produit
    Public Sub Modifier(
        id As Integer,
        nom As String,
        marque As String,
        idCategorie As Integer,
        uniteBase As String,
        code As String
    )

        Using connexion As MySqlConnection = ConnexionBD.ObtenirConnexion()
            connexion.Open()

            Dim sql As String =
                "UPDATE produit SET nom=@nom, marque=@marque, " &
                "id_categorie=@cat, unite_base=@unite, " &
                "code_barres=@cb WHERE id=@id"

            Using commande As New MySqlCommand(sql, connexion)

                commande.Parameters.AddWithValue("@nom", nom.Trim())
                commande.Parameters.AddWithValue("@marque", VersDb(marque))
                commande.Parameters.AddWithValue("@cat", idCategorie)
                commande.Parameters.AddWithValue("@unite", uniteBase)
                commande.Parameters.AddWithValue("@cb", VersDb(code))
                commande.Parameters.AddWithValue("@id", id)

                commande.ExecuteNonQuery()

            End Using
        End Using

    End Sub

    ' Supprimer un produit
    Public Sub Supprimer(id As Integer)

        Using connexion As MySqlConnection = ConnexionBD.ObtenirConnexion()
            connexion.Open()

            Using commande As New MySqlCommand(
                "DELETE FROM produit WHERE id = @id", connexion)

                commande.Parameters.AddWithValue("@id", id)
                commande.ExecuteNonQuery()

            End Using
        End Using

    End Sub

End Class
