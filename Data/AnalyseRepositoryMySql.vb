' ============================================================================
' FICHIER : Data/AnalyseRepositoryMySql.vb
' RÔLE    : implémentation MySQL de IAnalyseRepository (schéma final du chef, base pricieux_app).
'           Toutes les lectures passent par la table ticket (ticket.id_utilisateur)
'           afin de ne montrer que les données de l'utilisateur connecté.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Linq            ' Pour Distinct et ToList
Imports MySqlConnector         ' Bibliothèque pour parler à MySQL
Imports PreisRontgen.Models    ' Pour ProduitRef, PointPrix, ...
Imports PreisRontgen.Services  ' Pour FormatageAnalyse.UniteDepuisBase

Namespace Data                 ' Espace de noms : PreisRontgen.Data

    ''' <summary>Lectures MySQL pour l'historique, l'indice, la comparaison et les équivalents.</summary>
    Public Class AnalyseRepositoryMySql          ' Début de la classe
        Implements IAnalyseRepository            ' S'engage à fournir toutes les lectures du contrat

        Private ReadOnly _chaineConnexion As String   ' Texte de connexion à MySQL
        Private ReadOnly _idUtilisateur As Integer    ' Utilisateur connecté

        Public Sub New(chaineConnexion As String, idUtilisateur As Integer)   ' Constructeur
            If String.IsNullOrWhiteSpace(chaineConnexion) Then   ' Texte absent
                Throw New ArgumentException("La chaîne de connexion est obligatoire.", NameOf(chaineConnexion))   ' Refus explicite
            End If                                    ' Fin du contrôle
            If idUtilisateur <= 0 Then                ' Identifiant invalide
                Throw New ArgumentOutOfRangeException(NameOf(idUtilisateur), "L'identifiant de l'utilisateur doit être positif.")   ' Refus explicite
            End If                                    ' Fin du contrôle
            _chaineConnexion = chaineConnexion        ' Mémorise la chaîne
            _idUtilisateur = idUtilisateur            ' Mémorise l'utilisateur
        End Sub                                       ' Fin du constructeur

        ' ---------------------------------------------------------------- Produits, magasins, catégories

        Public Function ListerProduits() As List(Of ProduitRef) Implements IAnalyseRepository.ListerProduits   ' Liste des produits actifs
            Const sql As String =
                "SELECT p.id, p.nom, p.marque, p.id_categorie, p.unite_base " &
                "FROM produit p WHERE p.actif = 1 ORDER BY p.nom, p.marque"   ' Produits actifs par ordre alphabétique
            Dim resultat As New List(Of ProduitRef)()   ' Résultat
            Executer(sql, Nothing, Sub(lecteur As MySqlDataReader)   ' Lit chaque ligne
                                       Dim nom As String = lecteur.GetString(1)   ' Nom du produit
                                       If Not lecteur.IsDBNull(2) AndAlso lecteur.GetString(2).Trim().Length > 0 Then   ' Une marque existe
                                           nom &= " " & lecteur.GetString(2).Trim()   ' Nom + marque
                                       End If                                         ' Fin du test
                                       resultat.Add(New ProduitRef With {
                                           .Id = lecteur.GetInt32(0),                                           ' Identifiant
                                           .Nom = nom,                                                          ' Nom affiché
                                           .IdCategorie = If(lecteur.IsDBNull(3), 0, lecteur.GetInt32(3)),      ' Catégorie (0 si absente)
                                           .UniteBase = Services.FormatageAnalyse.UniteDepuisBase(lecteur.GetString(4))   ' Unité de base
                                       })                                                                       ' Ajoute le produit
                                   End Sub)                                                                     ' Fin de la lecture
            Return resultat                           ' Rend la liste
        End Function                                  ' Fin de ListerProduits

        Public Function ListerMagasins() As List(Of KeyValuePair(Of Integer, String)) Implements IAnalyseRepository.ListerMagasins   ' Liste des magasins actifs
            Return ListerIdNom("SELECT id, nom FROM magasin WHERE actif = 1 ORDER BY nom")   ' Magasins par ordre alphabétique
        End Function                                  ' Fin de ListerMagasins

        Public Function ListerCategories() As List(Of KeyValuePair(Of Integer, String)) Implements IAnalyseRepository.ListerCategories   ' Liste des catégories actives
            Return ListerIdNom("SELECT id, nom FROM categorie WHERE actif = 1 ORDER BY nom")   ' Catégories par ordre alphabétique
        End Function                                  ' Fin de ListerCategories

        ' ---------------------------------------------------------------- Historique (EF-06)

        Public Function ListerObservations(idProduit As Integer, idMagasin As Integer?) As List(Of PointPrix) Implements IAnalyseRepository.ListerObservations   ' Historique d'un produit
            Dim sql As String =
                "SELECT o.date_obs, o.id_magasin, m.nom, o.quantite_base, o.prix_unitaire, p.unite_base " &
                "FROM observation_prix o " &
                "INNER JOIN ticket t ON t.id = o.id_ticket " &
                "INNER JOIN magasin m ON m.id = o.id_magasin " &
                "INNER JOIN produit p ON p.id = o.id_produit " &
                "WHERE t.id_utilisateur = @utilisateur AND o.id_produit = @produit"   ' Observations de l'utilisateur pour ce produit
            If idMagasin.HasValue Then sql &= " AND o.id_magasin = @magasin"          ' Filtre facultatif par magasin
            sql &= " ORDER BY o.date_obs DESC, o.id DESC"                              ' De la plus récente à la plus ancienne

            Dim resultat As New List(Of PointPrix)()   ' Résultat
            Executer(sql, Sub(cmd As MySqlCommand)     ' Paramètres de la requête
                              cmd.Parameters.AddWithValue("@produit", idProduit)   ' Produit choisi
                              If idMagasin.HasValue Then cmd.Parameters.AddWithValue("@magasin", idMagasin.Value)   ' Magasin choisi
                          End Sub,
                     Sub(lecteur As MySqlDataReader)   ' Lit chaque ligne
                         resultat.Add(New PointPrix With {
                             .DateObs = lecteur.GetDateTime(0),        ' Date
                             .IdMagasin = lecteur.GetInt32(1),         ' Magasin
                             .NomMagasin = lecteur.GetString(2),       ' Nom du magasin
                             .QuantiteBase = lecteur.GetDecimal(3),    ' Quantité du conditionnement
                             .PrixUnitaire = lecteur.GetDecimal(4),    ' Prix unitaire
                             .UniteBase = Services.FormatageAnalyse.UniteDepuisBase(lecteur.GetString(5))   ' Unité
                         })                                            ' Ajoute le point
                     End Sub)                                          ' Fin de la lecture
            Return resultat                           ' Rend la liste
        End Function                                  ' Fin de ListerObservations

        ' ---------------------------------------------------------------- Comparaisons (EF-11, EF-12)

        Public Function ListerDernieresObservations(idProduit As Integer?, idCategorie As Integer?) As List(Of LignePrixMagasin) Implements IAnalyseRepository.ListerDernieresObservations   ' Dernier prix par produit et magasin
            Dim sql As String =
                "SELECT o.id_produit, p.nom, o.id_magasin, m.nom, o.prix_unitaire, o.date_obs, p.unite_base " &
                "FROM observation_prix o " &
                "INNER JOIN ticket t ON t.id = o.id_ticket " &
                "INNER JOIN produit p ON p.id = o.id_produit " &
                "INNER JOIN magasin m ON m.id = o.id_magasin " &
                "WHERE t.id_utilisateur = @utilisateur AND p.actif = 1 " &
                "AND o.id = (SELECT o2.id FROM observation_prix o2 " &
                "            INNER JOIN ticket t2 ON t2.id = o2.id_ticket " &
                "            WHERE t2.id_utilisateur = @utilisateur AND o2.id_produit = o.id_produit AND o2.id_magasin = o.id_magasin " &
                "            ORDER BY o2.date_obs DESC, o2.id DESC LIMIT 1)"   ' Garde seulement la dernière observation de chaque couple produit/magasin
            If idProduit.HasValue Then sql &= " AND o.id_produit = @produit"          ' Restriction à un produit
            If idCategorie.HasValue Then sql &= " AND p.id_categorie = @categorie"    ' Restriction à une catégorie
            sql &= " ORDER BY p.nom, o.prix_unitaire"                                  ' Ordre de lecture

            Dim resultat As New List(Of LignePrixMagasin)()   ' Résultat
            Executer(sql, Sub(cmd As MySqlCommand)            ' Paramètres de la requête
                              If idProduit.HasValue Then cmd.Parameters.AddWithValue("@produit", idProduit.Value)         ' Produit
                              If idCategorie.HasValue Then cmd.Parameters.AddWithValue("@categorie", idCategorie.Value)   ' Catégorie
                          End Sub,
                     Sub(lecteur As MySqlDataReader)          ' Lit chaque ligne
                         resultat.Add(New LignePrixMagasin With {
                             .IdProduit = lecteur.GetInt32(0),        ' Produit
                             .NomProduit = lecteur.GetString(1),      ' Nom du produit
                             .IdMagasin = lecteur.GetInt32(2),        ' Magasin
                             .NomMagasin = lecteur.GetString(3),      ' Nom du magasin
                             .PrixUnitaire = lecteur.GetDecimal(4),   ' Prix unitaire
                             .DateObs = lecteur.GetDateTime(5),       ' Date
                             .UniteBase = Services.FormatageAnalyse.UniteDepuisBase(lecteur.GetString(6))   ' Unité
                         })                                           ' Ajoute la ligne
                     End Sub)                                         ' Fin de la lecture
            Return resultat                           ' Rend la liste
        End Function                                  ' Fin de ListerDernieresObservations

        ' ---------------------------------------------------------------- Indice personnel (EF-10)

        Public Function ListerAchats(debut As Date, fin As Date) As List(Of AchatProduit) Implements IAnalyseRepository.ListerAchats   ' Achats de la période
            Const sql As String =
                "SELECT l.id_produit, p.nom, SUM(l.quantite_base_totale), COUNT(DISTINCT l.id_ticket) " &
                "FROM ligne_ticket l " &
                "INNER JOIN ticket t ON t.id = l.id_ticket " &
                "INNER JOIN produit p ON p.id = l.id_produit " &
                "WHERE t.id_utilisateur = @utilisateur AND t.date_achat >= @debut AND t.date_achat <= @fin " &
                "GROUP BY l.id_produit, p.nom"        ' Quantité totale et nombre de tickets par produit
            Dim resultat As New List(Of AchatProduit)()   ' Résultat
            Executer(sql, Sub(cmd As MySqlCommand)        ' Paramètres de la requête
                               cmd.Parameters.AddWithValue("@debut", debut.Date)   ' Début de la période
                               cmd.Parameters.AddWithValue("@fin", fin.Date)       ' Fin de la période
                           End Sub,
                     Sub(lecteur As MySqlDataReader)      ' Lit chaque ligne
                         resultat.Add(New AchatProduit With {
                             .IdProduit = lecteur.GetInt32(0),          ' Produit
                             .NomProduit = lecteur.GetString(1),        ' Nom
                             .QuantiteBase = lecteur.GetDecimal(2),     ' Quantité totale
                             .NombreAchats = CInt(lecteur.GetInt64(3))  ' Nombre de tickets
                         })                                             ' Ajoute l'achat
                     End Sub)                                           ' Fin de la lecture
            Return resultat                           ' Rend la liste
        End Function                                  ' Fin de ListerAchats

        Public Function ObtenirPrixALaDate(idsProduits As IEnumerable(Of Integer), dateLimite As Date) As Dictionary(Of Integer, Decimal) Implements IAnalyseRepository.ObtenirPrixALaDate   ' Prix à une date
            Dim resultat As New Dictionary(Of Integer, Decimal)()   ' Résultat
            Dim ids As List(Of Integer) = idsProduits.Distinct().ToList()   ' Identifiants sans doublon
            If ids.Count = 0 Then Return resultat                           ' Rien à chercher

            Dim noms As New List(Of String)()                       ' Noms des paramètres @p0, @p1...
            For i As Integer = 0 To ids.Count - 1                   ' Un paramètre par produit
                noms.Add("@p" & i.ToString())                       ' Ajoute le nom
            Next                                                    ' Fin de la boucle
            Dim sql As String =
                "SELECT o.id_produit, o.prix_unitaire " &
                "FROM observation_prix o " &
                "INNER JOIN ticket t ON t.id = o.id_ticket " &
                "WHERE t.id_utilisateur = @utilisateur AND o.date_obs <= @date " &
                "AND o.id_produit IN (" & String.Join(",", noms) & ") " &
                "ORDER BY o.date_obs ASC, o.id ASC"                 ' Du plus ancien au plus récent : la dernière ligne lue l'emporte
            Executer(sql, Sub(cmd As MySqlCommand)                  ' Paramètres de la requête
                              cmd.Parameters.AddWithValue("@date", dateLimite.Date)   ' Date limite incluse
                              For i As Integer = 0 To ids.Count - 1                   ' Chaque produit
                                  cmd.Parameters.AddWithValue(noms(i), ids(i))        ' Valeur du paramètre
                              Next                                                    ' Fin de la boucle
                          End Sub,
                     Sub(lecteur As MySqlDataReader)                ' Lit chaque ligne
                         resultat(lecteur.GetInt32(0)) = lecteur.GetDecimal(1)   ' Écrase par le prix le plus récent
                     End Sub)                                       ' Fin de la lecture
            Return resultat                           ' Rend le dictionnaire
        End Function                                  ' Fin de ObtenirPrixALaDate

        ' ---------------------------------------------------------------- Outils internes

        Private Function ListerIdNom(sql As String) As List(Of KeyValuePair(Of Integer, String))   ' Lit des couples identifiant / nom
            Dim resultat As New List(Of KeyValuePair(Of Integer, String))()   ' Résultat
            Executer(sql, Nothing, Sub(lecteur As MySqlDataReader)            ' Lit chaque ligne
                                       resultat.Add(New KeyValuePair(Of Integer, String)(lecteur.GetInt32(0), lecteur.GetString(1)))   ' Ajoute le couple
                                   End Sub)                                   ' Fin de la lecture
            Return resultat                           ' Rend la liste
        End Function                                  ' Fin de ListerIdNom

        ''' <summary>Exécute un SELECT : ajoute @utilisateur, laisse l'appelant ajouter ses paramètres puis lire les lignes.</summary>
        Private Sub Executer(sql As String, parametres As Action(Of MySqlCommand), lireLigne As Action(Of MySqlDataReader))   ' Évite de répéter le code de connexion
            Try                                       ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion (fermée automatiquement)
                    cn.Open()                         ' Ouvre
                    Using cmd As New MySqlCommand(sql, cn)          ' Commande SQL
                        cmd.Parameters.AddWithValue("@utilisateur", _idUtilisateur)   ' Utilisateur connecté (ignoré si absent du texte SQL)
                        If parametres IsNot Nothing Then parametres(cmd)              ' Paramètres propres à la requête
                        Using lecteur As MySqlDataReader = cmd.ExecuteReader()        ' Exécute
                            While lecteur.Read()                                      ' Pour chaque ligne
                                lireLigne(lecteur)                                    ' Convertit la ligne
                            End While                                                 ' Fin de la boucle
                        End Using                                                     ' Libère le lecteur
                    End Using                                                         ' Libère la commande
                End Using                                                             ' Ferme la connexion
            Catch ex As MySqlException                                                ' Erreur MySQL
                Throw New InvalidOperationException("Impossible de lire les données d'analyse : " & ex.Message, ex)   ' Message clair
            End Try                                                                   ' Fin du bloc surveillé
        End Sub                                                                       ' Fin de Executer

        Private Sub Executer(sql As String, lireLigne As Action(Of MySqlDataReader))   ' Variante sans paramètre supplémentaire
            Executer(sql, Nothing, lireLigne)         ' Délègue
        End Sub                                       ' Fin de Executer

    End Class                                         ' Fin de AnalyseRepositoryMySql

End Namespace                                         ' Fin de l'espace de noms Data
