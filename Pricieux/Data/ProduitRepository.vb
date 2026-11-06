
Option Strict On
Option Explicit On

Imports System.Data
Imports MySql.Data.MySqlClient

Public Class ProduitRepository

    ''' <summary>
    ''' Liste tous les produits avec leur catégorie.
    ''' </summary>
    Public Function Lister() As DataTable

        Const sql As String =
            "SELECT p.id, p.nom, p.marque, p.id_categorie, " &
            "c.nom AS categorie, p.unite_base, p.code_barres " &
            "FROM produit p " &
            "INNER JOIN categorie c ON c.id = p.id_categorie " &
            "ORDER BY p.nom"

        Using cn = ConnexionBD.CreerConnexion()

            cn.Open()

            Using cmd As New MySqlCommand(sql, cn)
                Using da As New MySqlDataAdapter(cmd)

                    Dim dt As New DataTable()
                    da.Fill(dt)
                    Return dt

                End Using
            End Using
        End Using

    End Function

    ''' <summary>
    ''' La base ne possède pas de colonne actif.
    ''' Tous les produits existants sont donc retournés.
    ''' </summary>
    Public Function ListerActifs() As DataTable
        Return Lister()
    End Function

    ''' <summary>
    ''' Recherche les produits par nom ou marque.
    ''' </summary>
    Public Function Rechercher(terme As String) As DataTable

        Const sql As String =
            "SELECT p.id, p.nom, p.marque, p.id_categorie, " &
            "c.nom AS categorie, p.unite_base, p.code_barres " &
            "FROM produit p " &
            "INNER JOIN categorie c ON c.id = p.id_categorie " &
            "WHERE p.nom LIKE @terme OR p.marque LIKE @terme " &
            "ORDER BY p.nom"

        Using cn = ConnexionBD.CreerConnexion()

            cn.Open()

            Using cmd As New MySqlCommand(sql, cn)

                cmd.Parameters.AddWithValue(
                    "@terme", "%" & If(terme, String.Empty).Trim() & "%")

                Using da As New MySqlDataAdapter(cmd)

                    Dim dt As New DataTable()
                    da.Fill(dt)
                    Return dt

                End Using
            End Using
        End Using

    End Function

    ''' <summary>
    ''' Ajoute un produit.
    ''' </summary>
    Public Function Ajouter(
        nom As String,
        marque As String,
        idCategorie As Integer,
        uniteBase As String,
        codeBarres As String
    ) As Integer

        ValiderProduit(nom, idCategorie, uniteBase)

        Const sql As String =
            "INSERT INTO produit " &
            "(nom, marque, id_categorie, unite_base, code_barres) " &
            "VALUES (@nom, @marque, @categorie, @unite, @code)"

        Using cn = ConnexionBD.CreerConnexion()

            cn.Open()

            Using cmd As New MySqlCommand(sql, cn)

                cmd.Parameters.AddWithValue("@nom", nom.Trim())
                cmd.Parameters.AddWithValue(
                    "@marque", If(marque, String.Empty).Trim())
                cmd.Parameters.AddWithValue("@categorie", idCategorie)
                cmd.Parameters.AddWithValue("@unite", uniteBase)

                cmd.Parameters.AddWithValue(
                    "@code",
                    If(String.IsNullOrWhiteSpace(codeBarres),
                       CType(DBNull.Value, Object),
                       codeBarres.Trim()))

                cmd.ExecuteNonQuery()

                Return CInt(cmd.LastInsertedId)

            End Using
        End Using

    End Function

    ''' <summary>
    ''' Modifie un produit existant.
    ''' </summary>
    Public Sub Modifier(
        id As Integer,
        nom As String,
        marque As String,
        idCategorie As Integer,
        uniteBase As String,
        codeBarres As String
    )

        If id <= 0 Then
            Throw New ArgumentException("Identifiant invalide.")
        End If

        ValiderProduit(nom, idCategorie, uniteBase)

        Const sql As String =
            "UPDATE produit SET nom=@nom, marque=@marque, " &
            "id_categorie=@categorie, unite_base=@unite, " &
            "code_barres=@code WHERE id=@id"

        Using cn = ConnexionBD.CreerConnexion()

            cn.Open()

            Using cmd As New MySqlCommand(sql, cn)

                cmd.Parameters.AddWithValue("@id", id)
                cmd.Parameters.AddWithValue("@nom", nom.Trim())
                cmd.Parameters.AddWithValue(
                    "@marque", If(marque, String.Empty).Trim())
                cmd.Parameters.AddWithValue("@categorie", idCategorie)
                cmd.Parameters.AddWithValue("@unite", uniteBase)

                cmd.Parameters.AddWithValue(
                    "@code",
                    If(String.IsNullOrWhiteSpace(codeBarres),
                       CType(DBNull.Value, Object),
                       codeBarres.Trim()))

                cmd.ExecuteNonQuery()

            End Using
        End Using

    End Sub

    ''' <summary>
    ''' Supprime un produit.
    ''' La base refuse la suppression s'il est déjà utilisé.
    ''' </summary>
    Public Sub Supprimer(id As Integer)

        If id <= 0 Then
            Throw New ArgumentException("Identifiant invalide.")
        End If

        Using cn = ConnexionBD.CreerConnexion()

            cn.Open()

            Using cmd As New MySqlCommand(
                "DELETE FROM produit WHERE id=@id", cn)

                cmd.Parameters.AddWithValue("@id", id)
                cmd.ExecuteNonQuery()

            End Using
        End Using

    End Sub

    Private Sub ValiderProduit(
        nom As String,
        idCategorie As Integer,
        uniteBase As String)

        If String.IsNullOrWhiteSpace(nom) Then
            Throw New ArgumentException(
                "Le nom du produit est obligatoire.")
        End If

        If idCategorie <= 0 Then
            Throw New ArgumentException(
                "Sélectionnez une catégorie valide.")
        End If

        If uniteBase <> "kg" AndAlso
           uniteBase <> "L" AndAlso
           uniteBase <> "piece" Then

            Throw New ArgumentException(
                "L'unité de base doit être kg, L ou piece.")

        End If

    End Sub

End Class
