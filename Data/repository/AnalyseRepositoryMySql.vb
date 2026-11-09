Option Strict On
Option Explicit On

Imports System.Text
Imports MySqlConnector
Imports PreisRontgen.Models

Namespace Data

    ''' <summary>
    ''' Dépôt d'analyse qui lit MySQL (à passer à frmHistorique et frmAnalyses à la place de DepotAnalyseDemo).
    ''' Les relevés de prix et les achats sont ceux de l'utilisateur connecté (Session.user).
    ''' Les produits, magasins et catégories sont communs à tous.
    ''' </summary>
    Public Class AnalyseRepositoryMySql
        Implements IAnalyseRepository


        Private Shared Function IdUtilisateur() As Integer
            If Session.user Is Nothing Then Throw New InvalidOperationException("Aucun utilisateur connecté.")
            Return Session.user.Id
        End Function

        Private Shared Function Echec(ex As MySqlException) As InvalidOperationException
            Return New InvalidOperationException("Accès aux données impossible : " & ex.Message, ex)
        End Function

        ' kg / L / piece (texte de la base) vers l'énumération du projet
        Private Shared Function VersUniteBase(texte As String) As UniteBase
            If texte = "kg" Then Return UniteBase.Kg
            If texte = "L" Then Return UniteBase.Litre
            Return UniteBase.Piece
        End Function

        Private Shared Function NomComplet(nom As String, marque As String) As String
            Return If(marque = "", nom, nom & " " & marque)
        End Function

        Private Shared Function ListerCles(sql As String) As List(Of KeyValuePair(Of Integer, String))
            Dim resultat As New List(Of KeyValuePair(Of Integer, String))()
            Try
                Using cn As MySqlConnection = GetConnection
                    cn.Open()
                    Using cmd As New MySqlCommand(sql, cn)
                        Using rd As MySqlDataReader = cmd.ExecuteReader()
                            While rd.Read()
                                resultat.Add(New KeyValuePair(Of Integer, String)(rd.GetInt32(0), rd.GetString(1)))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As MySqlException
                Throw Echec(ex)
            End Try
            Return resultat
        End Function


        Public Function ListerProduits() As List(Of ProduitRef) Implements IAnalyseRepository.ListerProduits
            Dim resultat As New List(Of ProduitRef)()
            Try
                Using cn As MySqlConnection = GetConnection
                    cn.Open()
                    Using cmd As New MySqlCommand(
                        "SELECT id, nom, marque, id_categorie, unite_base FROM produit ORDER BY nom, marque", cn)
                        Using rd As MySqlDataReader = cmd.ExecuteReader()
                            While rd.Read()
                                resultat.Add(New ProduitRef With {
                                    .Id = rd.GetInt32(0),
                                    .Nom = NomComplet(rd.GetString(1), rd.GetString(2)),
                                    .IdCategorie = rd.GetInt32(3),
                                    .UniteBase = VersUniteBase(rd.GetString(4))
                                })
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As MySqlException
                Throw Echec(ex)
            End Try
            Return resultat
        End Function

        Public Function ListerMagasins() As List(Of KeyValuePair(Of Integer, String)) Implements IAnalyseRepository.ListerMagasins
            Return ListerCles("SELECT id, nom FROM magasin ORDER BY nom")
        End Function

        Public Function ListerCategories() As List(Of KeyValuePair(Of Integer, String)) Implements IAnalyseRepository.ListerCategories
            Return ListerCles("SELECT id, nom FROM categorie ORDER BY nom")
        End Function


        ' Historique d'un produit, du plus récent au plus ancien (idMagasin = Nothing : tous les magasins)
        Public Function ListerObservations(idProduit As Integer, idMagasin As Integer?) As List(Of PointPrix) _
            Implements IAnalyseRepository.ListerObservations

            Dim resultat As New List(Of PointPrix)()
            Try
                Dim sql As String =
                    "SELECT o.date_obs, o.id_magasin, m.nom, o.quantite_base, o.prix_unitaire, p.unite_base " &
                    "FROM observation_prix o " &
                    "INNER JOIN magasin m ON m.id = o.id_magasin " &
                    "INNER JOIN produit p ON p.id = o.id_produit " &
                    "WHERE o.id_utilisateur = @u AND o.id_produit = @p" &
                    If(idMagasin.HasValue, " AND o.id_magasin = @m", "") &
                    " ORDER BY o.date_obs DESC, o.id DESC"

                Using cn As MySqlConnection = GetConnection
                    cn.Open()
                    Using cmd As New MySqlCommand(sql, cn)
                        cmd.Parameters.AddWithValue("@u", IdUtilisateur())
                        cmd.Parameters.AddWithValue("@p", idProduit)
                        If idMagasin.HasValue Then cmd.Parameters.AddWithValue("@m", idMagasin.Value)
                        Using rd As MySqlDataReader = cmd.ExecuteReader()
                            While rd.Read()
                                resultat.Add(New PointPrix With {
                                    .DateObs = rd.GetDateTime(0),
                                    .IdMagasin = rd.GetInt32(1),
                                    .NomMagasin = rd.GetString(2),
                                    .QuantiteBase = rd.GetDecimal(3),
                                    .PrixUnitaire = rd.GetDecimal(4),
                                    .UniteBase = VersUniteBase(rd.GetString(5))
                                })
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As MySqlException
                Throw Echec(ex)
            End Try
            Return resultat
        End Function

        ' Dernier prix observé pour chaque couple produit / magasin (filtres facultatifs)
        Public Function ListerDernieresObservations(idProduit As Integer?, idCategorie As Integer?) As List(Of LignePrixMagasin) _
            Implements IAnalyseRepository.ListerDernieresObservations

            Dim resultat As New List(Of LignePrixMagasin)()
            Try
                Dim sql As New StringBuilder(
                    "SELECT o.id_produit, p.nom, p.marque, o.id_magasin, m.nom, o.prix_unitaire, o.date_obs, p.unite_base " &
                    "FROM observation_prix o " &
                    "INNER JOIN produit p ON p.id = o.id_produit " &
                    "INNER JOIN magasin m ON m.id = o.id_magasin " &
                    "WHERE o.id_utilisateur = @u")
                If idProduit.HasValue Then sql.Append(" AND o.id_produit = @p")
                If idCategorie.HasValue Then sql.Append(" AND p.id_categorie = @c")
                sql.Append(
                    " AND o.id = (SELECT o2.id FROM observation_prix o2 " &
                    "              WHERE o2.id_utilisateur = o.id_utilisateur AND o2.id_produit = o.id_produit " &
                    "                AND o2.id_magasin = o.id_magasin " &
                    "              ORDER BY o2.date_obs DESC, o2.id DESC LIMIT 1) " &
                    "ORDER BY p.nom, m.nom")

                Using cn As MySqlConnection = GetConnection
                    cn.Open()
                    Using cmd As New MySqlCommand(sql.ToString(), cn)
                        cmd.Parameters.AddWithValue("@u", IdUtilisateur())
                        If idProduit.HasValue Then cmd.Parameters.AddWithValue("@p", idProduit.Value)
                        If idCategorie.HasValue Then cmd.Parameters.AddWithValue("@c", idCategorie.Value)
                        Using rd As MySqlDataReader = cmd.ExecuteReader()
                            While rd.Read()
                                resultat.Add(New LignePrixMagasin With {
                                    .IdProduit = rd.GetInt32(0),
                                    .NomProduit = NomComplet(rd.GetString(1), rd.GetString(2)),
                                    .IdMagasin = rd.GetInt32(3),
                                    .NomMagasin = rd.GetString(4),
                                    .PrixUnitaire = rd.GetDecimal(5),
                                    .DateObs = rd.GetDateTime(6),
                                    .UniteBase = VersUniteBase(rd.GetString(7))
                                })
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As MySqlException
                Throw Echec(ex)
            End Try
            Return resultat
        End Function

        ' Produits achetés entre deux dates (bornes incluses) : quantité totale en unité de base et nombre de tickets
        Public Function ListerAchats(debut As Date, fin As Date) As List(Of AchatProduit) Implements IAnalyseRepository.ListerAchats
            Dim resultat As New List(Of AchatProduit)()
            Try
                Dim sql As String =
                    "SELECT p.id, p.nom, p.marque, SUM(o.quantite_base * l.nb_achete) AS quantite, COUNT(DISTINCT t.id) AS achats " &
                    "FROM ligne_ticket l " &
                    "INNER JOIN ticket t ON t.id = l.id_ticket " &
                    "INNER JOIN observation_prix o ON o.id_ligne_ticket = l.id " &
                    "INNER JOIN produit p ON p.id = l.id_produit " &
                    "WHERE t.id_utilisateur = @u AND t.date_achat >= @debut AND t.date_achat <= @fin " &
                    "GROUP BY p.id, p.nom, p.marque " &
                    "ORDER BY achats DESC, quantite DESC"

                Using cn As MySqlConnection = GetConnection
                    cn.Open()
                    Using cmd As New MySqlCommand(sql, cn)
                        cmd.Parameters.AddWithValue("@u", IdUtilisateur())
                        cmd.Parameters.AddWithValue("@debut", debut.Date)
                        cmd.Parameters.AddWithValue("@fin", fin.Date)
                        Using rd As MySqlDataReader = cmd.ExecuteReader()
                            While rd.Read()
                                resultat.Add(New AchatProduit With {
                                    .IdProduit = rd.GetInt32(0),
                                    .NomProduit = NomComplet(rd.GetString(1), rd.GetString(2)),
                                    .QuantiteBase = rd.GetDecimal(3),
                                    .NombreAchats = Convert.ToInt32(rd.GetInt64(4))
                                })
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As MySqlException
                Throw Echec(ex)
            End Try
            Return resultat
        End Function

        ' Dernier prix unitaire connu de chaque produit à la date donnée (ou avant) ; absent s'il n'existe pas
        Public Function ObtenirPrixALaDate(idsProduits As IEnumerable(Of Integer), dateLimite As Date) _
            As Dictionary(Of Integer, Decimal) Implements IAnalyseRepository.ObtenirPrixALaDate

            Dim resultat As New Dictionary(Of Integer, Decimal)()
            Dim ids As List(Of Integer) = idsProduits.Distinct().ToList()
            If ids.Count = 0 Then Return resultat

            Try
                Dim noms As New List(Of String)()
                For i As Integer = 0 To ids.Count - 1
                    noms.Add("@i" & i.ToString())
                Next

                Dim sql As String =
                    "SELECT o.id_produit, o.prix_unitaire FROM observation_prix o " &
                    "WHERE o.id_utilisateur = @u AND o.id_produit IN (" & String.Join(", ", noms) & ") " &
                    "  AND o.id = (SELECT o2.id FROM observation_prix o2 " &
                    "               WHERE o2.id_utilisateur = o.id_utilisateur AND o2.id_produit = o.id_produit " &
                    "                 AND o2.date_obs < @fin " &
                    "               ORDER BY o2.date_obs DESC, o2.id DESC LIMIT 1)"

                Using cn As MySqlConnection = GetConnection
                    cn.Open()
                    Using cmd As New MySqlCommand(sql, cn)
                        cmd.Parameters.AddWithValue("@u", IdUtilisateur())
                        cmd.Parameters.AddWithValue("@fin", dateLimite.Date.AddDays(1))   ' toute la journée limite est incluse
                        For i As Integer = 0 To ids.Count - 1
                            cmd.Parameters.AddWithValue(noms(i), ids(i))
                        Next
                        Using rd As MySqlDataReader = cmd.ExecuteReader()
                            While rd.Read()
                                resultat(rd.GetInt32(0)) = rd.GetDecimal(1)
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As MySqlException
                Throw Echec(ex)
            End Try
            Return resultat
        End Function

    End Class

End Namespace