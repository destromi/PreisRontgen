Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports MySqlConnector

Public NotInheritable Class ReferentielsDepot
    Public Async Function ListerMagasinsAsync() As Task(Of List(Of Magasin))
        Dim resultats As New List(Of Magasin)()
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand("SELECT id, nom FROM magasin WHERE actif = TRUE ORDER BY nom;", connexion)
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    While Await lecteur.ReadAsync()
                        resultats.Add(New Magasin With {.Id = lecteur.GetInt64("id"), .Nom = lecteur.GetString("nom")})
                    End While
                End Using
            End Using
        End Using
        Return resultats
    End Function

    Public Async Function ListerCategoriesAsync() As Task(Of List(Of Categorie))
        Dim resultats As New List(Of Categorie)()
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand("SELECT id, nom FROM categorie WHERE actif = TRUE ORDER BY nom;", connexion)
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    While Await lecteur.ReadAsync()
                        resultats.Add(New Categorie With {.Id = lecteur.GetInt64("id"), .Nom = lecteur.GetString("nom")})
                    End While
                End Using
            End Using
        End Using
        Return resultats
    End Function

    Public Async Function ListerProduitsAsync(Optional recherche As String = Nothing, Optional categorieId As Long? = Nothing) As Task(Of List(Of Produit))
        Dim resultats As New List(Of Produit)()
        Dim sql As String = "SELECT p.id, p.nom, p.marque, p.id_categorie, c.nom AS categorie_nom, p.unite_base, COALESCE(p.code_barres, '') AS code_barres FROM produit p INNER JOIN categorie c ON c.id = p.id_categorie WHERE p.actif = TRUE"
        If Not String.IsNullOrWhiteSpace(recherche) Then sql &= " AND (p.nom LIKE @recherche OR p.marque LIKE @recherche OR p.code_barres LIKE @recherche)"
        If categorieId.HasValue Then sql &= " AND p.id_categorie = @categorie"
        sql &= " ORDER BY p.nom, p.marque LIMIT 500;"
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand(sql, connexion)
                If Not String.IsNullOrWhiteSpace(recherche) Then commande.Parameters.AddWithValue("@recherche", "%" & recherche.Trim() & "%")
                If categorieId.HasValue Then commande.Parameters.AddWithValue("@categorie", categorieId.Value)
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    While Await lecteur.ReadAsync()
                        resultats.Add(New Produit With {
                            .Id = lecteur.GetInt64("id"),
                            .Nom = lecteur.GetString("nom"),
                            .Marque = lecteur.GetString("marque"),
                            .CategorieId = lecteur.GetInt64("id_categorie"),
                            .CategorieNom = lecteur.GetString("categorie_nom"),
                            .UniteBase = LireUnite(lecteur.GetString("unite_base")),
                            .CodeBarres = lecteur.GetString("code_barres")
                        })
                    End While
                End Using
            End Using
        End Using
        Return resultats
    End Function

    Public Async Function EnregistrerMagasinAsync(id As Long?, nom As String) As Task
        Dim valeur As String = ValiderNom(nom, "magasin")
        Await ExecuterEnregistrementAsync("magasin", id, valeur)
    End Function

    Public Async Function EnregistrerCategorieAsync(id As Long?, nom As String) As Task
        Dim valeur As String = ValiderNom(nom, "catégorie")
        Await ExecuterEnregistrementAsync("categorie", id, valeur)
    End Function

    Public Async Function EnregistrerProduitAsync(produit As Produit) As Task
        If produit Is Nothing Then Throw New ArgumentNullException(NameOf(produit))
        Dim nom As String = ValiderNom(produit.Nom, "produit")
        Dim marque As String = If(produit.Marque, String.Empty).Trim()
        If produit.CategorieId <= 0 Then Throw New ArgumentException("Sélectionnez une catégorie.")
        Dim codeBarres As String = If(String.IsNullOrWhiteSpace(produit.CodeBarres), Nothing, produit.CodeBarres.Trim())
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            If produit.Id > 0 Then
                Using commandeUnite As New MySqlCommand("SELECT p.unite_base, EXISTS(SELECT 1 FROM observation_prix o WHERE o.id_produit=p.id) AS possede_historique FROM produit p WHERE p.id=@id;", connexion)
                    commandeUnite.Parameters.AddWithValue("@id", produit.Id)
                    Using lecteur As MySqlDataReader = DirectCast(Await commandeUnite.ExecuteReaderAsync(), MySqlDataReader)
                        If Not Await lecteur.ReadAsync() Then Throw New InvalidOperationException("Le produit n'existe plus.")
                        If lecteur.GetBoolean("possede_historique") AndAlso Not String.Equals(lecteur.GetString("unite_base"), EcrireUnite(produit.UniteBase), StringComparison.Ordinal) Then
                            Throw New InvalidOperationException("L'unité de base ne peut pas être modifiée après l'enregistrement d'observations de prix.")
                        End If
                    End Using
                End Using
            End If
            Dim sql As String = If(produit.Id > 0,
                "UPDATE produit SET nom=@nom, marque=@marque, id_categorie=@categorie, unite_base=@unite, code_barres=@code, actif=TRUE WHERE id=@id;",
                "INSERT INTO produit (nom, marque, id_categorie, unite_base, code_barres) VALUES (@nom, @marque, @categorie, @unite, @code);")
            Using commande As New MySqlCommand(sql, connexion)
                commande.Parameters.AddWithValue("@nom", nom)
                commande.Parameters.AddWithValue("@marque", marque)
                commande.Parameters.AddWithValue("@categorie", produit.CategorieId)
                commande.Parameters.AddWithValue("@unite", EcrireUnite(produit.UniteBase))
                commande.Parameters.AddWithValue("@code", If(codeBarres, CType(DBNull.Value, Object)))
                If produit.Id > 0 Then commande.Parameters.AddWithValue("@id", produit.Id)
                Await commande.ExecuteNonQueryAsync()
            End Using
        End Using
    End Function

    Public Async Function SupprimerMagasinAsync(id As Long) As Task
        Await DesactiverAsync("magasin", id)
    End Function

    Public Async Function SupprimerCategorieAsync(id As Long) As Task
        Await DesactiverAsync("categorie", id)
    End Function

    Public Async Function SupprimerProduitAsync(id As Long) As Task
        Await DesactiverAsync("produit", id)
    End Function

    Public Async Function TrouverProduitAsync(nom As String, marque As String) As Task(Of Produit)
        Dim produits As List(Of Produit) = Await ListerProduitsAsync(nom)
        Return produits.Find(Function(item) String.Equals(item.Nom, nom, StringComparison.OrdinalIgnoreCase) AndAlso String.Equals(item.Marque, If(marque, String.Empty), StringComparison.OrdinalIgnoreCase))
    End Function

    Public Async Function TrouverMagasinAsync(nom As String) As Task(Of Magasin)
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand("SELECT id, nom FROM magasin WHERE nom=@nom AND actif=TRUE LIMIT 1;", connexion)
                commande.Parameters.AddWithValue("@nom", nom.Trim())
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    If Await lecteur.ReadAsync() Then Return New Magasin With {.Id = lecteur.GetInt64("id"), .Nom = lecteur.GetString("nom")}
                End Using
            End Using
        End Using
        Return Nothing
    End Function

    Public Async Function TrouverCategorieAsync(nom As String) As Task(Of Categorie)
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand("SELECT id, nom FROM categorie WHERE nom=@nom AND actif=TRUE LIMIT 1;", connexion)
                commande.Parameters.AddWithValue("@nom", nom.Trim())
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    If Await lecteur.ReadAsync() Then Return New Categorie With {.Id = lecteur.GetInt64("id"), .Nom = lecteur.GetString("nom")}
                End Using
            End Using
        End Using
        Return Nothing
    End Function

    Private Shared Async Function ExecuterEnregistrementAsync(table As String, id As Long?, nom As String) As Task
        Dim sql As String = If(id.HasValue, "UPDATE " & table & " SET nom=@nom, actif=TRUE WHERE id=@id;", "INSERT INTO " & table & " (nom) VALUES (@nom);")
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand(sql, connexion)
                commande.Parameters.AddWithValue("@nom", nom)
                If id.HasValue Then commande.Parameters.AddWithValue("@id", id.Value)
                Await commande.ExecuteNonQueryAsync()
            End Using
        End Using
    End Function

    Private Shared Async Function DesactiverAsync(table As String, id As Long) As Task
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand("UPDATE " & table & " SET actif=FALSE WHERE id=@id;", connexion)
                commande.Parameters.AddWithValue("@id", id)
                Await commande.ExecuteNonQueryAsync()
            End Using
        End Using
    End Function

    Private Shared Function ValiderNom(nom As String, type As String) As String
        Dim valeur As String = If(nom, String.Empty).Trim()
        If valeur.Length = 0 OrElse valeur.Length > 180 Then Throw New ArgumentException("Le nom du " & type & " doit contenir entre 1 et 180 caractères.")
        Return valeur
    End Function

    Private Shared Function LireUnite(valeur As String) As UniteProduit
        Select Case valeur
            Case "kg"
                Return UniteProduit.Kilogramme
            Case "L"
                Return UniteProduit.Litre
            Case Else
                Return UniteProduit.Piece
        End Select
    End Function

    Private Shared Function EcrireUnite(valeur As UniteProduit) As String
        Select Case valeur
            Case UniteProduit.Kilogramme
                Return "kg"
            Case UniteProduit.Litre
                Return "L"
            Case Else
                Return "pièce"
        End Select
    End Function
End Class
