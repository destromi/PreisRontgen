Imports MySql.Data.MySqlClient
Imports System.Data
Public Class ProduitRepository

    ' Charger toutes les catégories
    Public Function ChargerCategories() As DataTable

        Dim table As New DataTable()

        Using connexion As MySqlConnection = ConnexionBD.ObtenirConnexion()

            Dim sql As String = "SELECT id, nom FROM categorie ORDER BY nom"

            Using commande As New MySqlCommand(sql, connexion)

                Using adaptateur As New MySqlDataAdapter(commande)
                    adaptateur.Fill(table)
                End Using

            End Using

        End Using

        Return table

    End Function


    ' Charger les produits avec leurs catégories
    Public Function ChargerProduits() As DataTable

        Dim table As New DataTable()

        Using connexion As MySqlConnection = ConnexionBD.ObtenirConnexion()

            Dim sql As String =
                "SELECT p.id, p.nom, p.marque, " &
                "c.nom AS categorie, p.unite_base, p.code_barres " &
                "FROM produit p " &
                "LEFT JOIN categorie c ON p.id_categorie = c.id " &
                "ORDER BY p.nom"

            Using commande As New MySqlCommand(sql, connexion)

                Using adaptateur As New MySqlDataAdapter(commande)
                    adaptateur.Fill(table)
                End Using

            End Using

        End Using

        Return table

    End Function


    ' Ajouter un nouveau produit
    Public Sub AjouterProduit(nom As String,
                              marque As String,
                              idCategorie As Integer,
                              uniteBase As String,
                              codeBarres As String)

        Using connexion As MySqlConnection = ConnexionBD.ObtenirConnexion()

            connexion.Open()

            Dim sql As String =
                "INSERT INTO produit " &
                "(nom, marque, id_categorie, unite_base, code_barres) " &
                "VALUES (@nom, @marque, @categorie, @unite, @code)"

            Using commande As New MySqlCommand(sql, connexion)

                commande.Parameters.AddWithValue("@nom", nom)
                commande.Parameters.AddWithValue("@marque", marque)
                commande.Parameters.AddWithValue("@categorie", idCategorie)
                commande.Parameters.AddWithValue("@unite", uniteBase)
                commande.Parameters.AddWithValue("@code", codeBarres)

                commande.ExecuteNonQuery()

            End Using

        End Using

    End Sub

End Class

