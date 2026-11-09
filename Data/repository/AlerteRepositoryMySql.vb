Option Strict On
Option Explicit On

Imports System.Globalization
Imports System.Text
Imports MySqlConnector
Imports PreisRontgen.Models

Namespace Data

    ''' <summary>
    ''' Dépôt d'alertes qui lit et écrit dans MySQL (à passer à frmAlertes à la place de DepotDemo).
    ''' Approche B : la table alerte reste inchangée. Le magasin, les prix et le texte explicatif
    ''' sont reconstruits à la lecture à partir de l'observation de prix liée à l'alerte.
    ''' Toutes les requêtes portent sur l'utilisateur connecté (Session.user).
    ''' </summary>
    Public Class AlerteRepositoryMySql
        Implements IAlerteRepository

        Private Shared ReadOnly Fr As NumberFormatInfo = CreerFormat()

        Private Shared Function CreerFormat() As NumberFormatInfo
            Dim nf As NumberFormatInfo = CType(CultureInfo.GetCultureInfo("fr-FR").NumberFormat.Clone(), NumberFormatInfo)
            nf.NumberGroupSeparator = " "      ' espace simple entre les milliers
            Return nf
        End Function


        Private Shared Function IdUtilisateur() As Integer
            If Session.user Is Nothing Then Throw New InvalidOperationException("Aucun utilisateur connecté.")
            Return Session.user.Id
        End Function

        ' Les écrans n'attrapent que InvalidOperationException : on y convertit les erreurs MySQL
        Private Shared Function Echec(ex As MySqlException) As InvalidOperationException
            Return New InvalidOperationException("Accès aux données impossible : " & ex.Message, ex)
        End Function

        Private Shared Function TypeEnTexte(t As TypeAlerte) As String
            Return If(t = TypeAlerte.Hausse, "HAUSSE", "SHRINKFLATION")
        End Function

        Private Shared Function TexteEnType(s As String) As TypeAlerte
            Return If(String.Equals(s, "HAUSSE", StringComparison.OrdinalIgnoreCase), TypeAlerte.Hausse, TypeAlerte.Shrinkflation)
        End Function


        Public Function Rechercher(filtre As FiltreAlerte) As List(Of Alerte) Implements IAlerteRepository.Rechercher
            Dim resultat As New List(Of Alerte)()
            Try
                Dim conditions As New List(Of String) From {"a.id_utilisateur = @u"}
                Dim valeurs As New List(Of KeyValuePair(Of String, Object)) From {
                    New KeyValuePair(Of String, Object)("@u", IdUtilisateur())}

                If filtre IsNot Nothing Then
                    If filtre.DateDebut.HasValue Then
                        conditions.Add("a.date_alerte >= @debut")
                        valeurs.Add(New KeyValuePair(Of String, Object)("@debut", filtre.DateDebut.Value.Date))
                    End If
                    If filtre.DateFin.HasValue Then
                        conditions.Add("a.date_alerte < @fin")       ' fin incluse : strictement avant le lendemain
                        valeurs.Add(New KeyValuePair(Of String, Object)("@fin", filtre.DateFin.Value.Date.AddDays(1)))
                    End If
                    If filtre.IdMagasin.HasValue Then
                        conditions.Add("o.id_magasin = @mag")
                        valeurs.Add(New KeyValuePair(Of String, Object)("@mag", filtre.IdMagasin.Value))
                    End If
                    If filtre.Type.HasValue Then
                        conditions.Add("a.type = @type")
                        valeurs.Add(New KeyValuePair(Of String, Object)("@type", TypeEnTexte(filtre.Type.Value)))
                    End If
                    If filtre.Lue.HasValue Then
                        conditions.Add("a.lue = @lue")
                        valeurs.Add(New KeyValuePair(Of String, Object)("@lue", filtre.Lue.Value))
                    End If
                End If

                Dim sql As String =
                    "SELECT a.id, a.id_produit, a.id_observation, o.id_magasin, a.type, a.valeur, a.date_alerte, a.lue, " &
                    "       p.nom, p.marque, p.unite_base, m.nom, o.prix_unitaire, o.quantite_base, " &
                    "       (SELECT o2.quantite_base FROM observation_prix o2 " &
                    "         WHERE o2.id_utilisateur = o.id_utilisateur AND o2.id_produit = o.id_produit " &
                    "           AND o2.id_magasin = o.id_magasin " &
                    "           AND (o2.date_obs < o.date_obs OR (o2.date_obs = o.date_obs AND o2.id < o.id)) " &
                    "         ORDER BY o2.date_obs DESC, o2.id DESC LIMIT 1) AS qte_precedente " &
                    "FROM alerte a " &
                    "INNER JOIN observation_prix o ON o.id = a.id_observation " &
                    "INNER JOIN produit p ON p.id = a.id_produit " &
                    "INNER JOIN magasin m ON m.id = o.id_magasin " &
                    "WHERE " & String.Join(" AND ", conditions) & " " &
                    "ORDER BY a.date_alerte DESC, a.id DESC"

                Using cn As MySqlConnection = GetConnection
                    cn.Open()
                    Dim seuilHausse As Decimal = 5D
                    Dim seuilShrink As Decimal = 3D
                    ChargerSeuils(cn, seuilHausse, seuilShrink)

                    Using cmd As New MySqlCommand(sql, cn)
                        For Each kv As KeyValuePair(Of String, Object) In valeurs
                            cmd.Parameters.AddWithValue(kv.Key, kv.Value)
                        Next
                        Using rd As MySqlDataReader = cmd.ExecuteReader()
                            While rd.Read()
                                Dim type As TypeAlerte = TexteEnType(rd.GetString(4))
                                Dim valeur As Decimal = rd.GetDecimal(5)
                                Dim marque As String = rd.GetString(9)
                                Dim unite As String = rd.GetString(10)
                                Dim prixActuel As Decimal = rd.GetDecimal(12)
                                Dim quantiteActuelle As Decimal = rd.GetDecimal(13)
                                Dim quantitePrecedente As Decimal? = Nothing
                                If Not rd.IsDBNull(14) Then quantitePrecedente = rd.GetDecimal(14)

                                Dim prixPrecedent As Decimal = Math.Round(prixActuel / (1D + valeur), 4)

                                resultat.Add(New Alerte With {
                                    .Id = rd.GetInt32(0),
                                    .IdProduit = rd.GetInt32(1),
                                    .IdObservation = rd.GetInt32(2),
                                    .IdMagasin = rd.GetInt32(3),
                                    .Type = type,
                                    .Valeur = valeur,
                                    .DateAlerte = rd.GetDateTime(6),
                                    .Lue = rd.GetBoolean(7),
                                    .NomProduit = If(marque = "", rd.GetString(8), rd.GetString(8) & " " & marque),
                                    .NomMagasin = rd.GetString(11),
                                    .PrixActuel = prixActuel,
                                    .PrixPrecedent = prixPrecedent,
                                    .Detail = ConstruireDetail(type, valeur, prixPrecedent, prixActuel,
                                                               quantitePrecedente, quantiteActuelle, unite,
                                                               If(type = TypeAlerte.Hausse, seuilHausse, seuilShrink))
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

        Public Function CompterNonLues() As Integer Implements IAlerteRepository.CompterNonLues
            Try
                Using cn As MySqlConnection = GetConnection
                    cn.Open()
                    Using cmd As New MySqlCommand("SELECT COUNT(*) FROM alerte WHERE id_utilisateur = @u AND lue = 0", cn)
                        cmd.Parameters.AddWithValue("@u", IdUtilisateur())
                        Return Convert.ToInt32(cmd.ExecuteScalar())
                    End Using
                End Using
            Catch ex As MySqlException
                Throw Echec(ex)
            End Try
        End Function

        Public Function ListerMagasins() As List(Of KeyValuePair(Of Integer, String)) Implements IAlerteRepository.ListerMagasins
            Dim resultat As New List(Of KeyValuePair(Of Integer, String))()
            Try
                Using cn As MySqlConnection = GetConnection
                    cn.Open()
                    Using cmd As New MySqlCommand("SELECT id, nom FROM magasin ORDER BY nom", cn)
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


        Public Function Ajouter(alerte As Alerte) As Integer Implements IAlerteRepository.Ajouter
            Try
                Using cn As MySqlConnection = GetConnection
                    cn.Open()
                    Dim idObservation As Integer = TrouverObservation(cn, alerte)
                    If idObservation = 0 Then
                        Throw New InvalidOperationException("Aucune observation de prix trouvée pour cette alerte.")
                    End If

                    Dim dateAlerte As Date = If(alerte.DateAlerte = Date.MinValue, Date.Now, alerte.DateAlerte)
                    Using cmd As New MySqlCommand(
                        "INSERT INTO alerte (id_utilisateur, id_produit, id_observation, type, valeur, date_alerte, lue) " &
                        "VALUES (@u, @p, @o, @t, @v, @d, @l)", cn)
                        cmd.Parameters.AddWithValue("@u", IdUtilisateur())
                        cmd.Parameters.AddWithValue("@p", alerte.IdProduit)
                        cmd.Parameters.AddWithValue("@o", idObservation)
                        cmd.Parameters.AddWithValue("@t", TypeEnTexte(alerte.Type))
                        cmd.Parameters.AddWithValue("@v", Math.Round(alerte.Valeur, 4))
                        cmd.Parameters.AddWithValue("@d", dateAlerte)
                        cmd.Parameters.AddWithValue("@l", alerte.Lue)
                        cmd.ExecuteNonQuery()
                        alerte.Id = CInt(cmd.LastInsertedId)
                    End Using
                End Using
            Catch ex As MySqlException
                Throw Echec(ex)
            End Try
            Return alerte.Id
        End Function

        Public Sub MarquerCommeLue(idAlerte As Integer) Implements IAlerteRepository.MarquerCommeLue
            Try
                Using cn As MySqlConnection = GetConnection
                    cn.Open()
                    Using cmd As New MySqlCommand("UPDATE alerte SET lue = 1 WHERE id = @id AND id_utilisateur = @u", cn)
                        cmd.Parameters.AddWithValue("@id", idAlerte)
                        cmd.Parameters.AddWithValue("@u", IdUtilisateur())
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As MySqlException
                Throw Echec(ex)
            End Try
        End Sub

        Public Sub MarquerToutesCommeLues() Implements IAlerteRepository.MarquerToutesCommeLues
            Try
                Using cn As MySqlConnection = GetConnection
                    cn.Open()
                    Using cmd As New MySqlCommand("UPDATE alerte SET lue = 1 WHERE id_utilisateur = @u AND lue = 0", cn)
                        cmd.Parameters.AddWithValue("@u", IdUtilisateur())
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As MySqlException
                Throw Echec(ex)
            End Try
        End Sub

        ' Anti-doublon : une alerte de ce type existe déjà pour la même observation de prix
        Public Function ExisteDeja(alerte As Alerte) As Boolean Implements IAlerteRepository.ExisteDeja
            Try
                Using cn As MySqlConnection = GetConnection
                    cn.Open()
                    Dim idObservation As Integer = TrouverObservation(cn, alerte)
                    If idObservation = 0 Then Return False
                    Using cmd As New MySqlCommand(
                        "SELECT COUNT(*) FROM alerte WHERE id_utilisateur = @u AND id_observation = @o AND type = @t", cn)
                        cmd.Parameters.AddWithValue("@u", IdUtilisateur())
                        cmd.Parameters.AddWithValue("@o", idObservation)
                        cmd.Parameters.AddWithValue("@t", TypeEnTexte(alerte.Type))
                        Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
                    End Using
                End Using
            Catch ex As MySqlException
                Throw Echec(ex)
            End Try
        End Function


        ' Observation liée à l'alerte : celle indiquée, sinon la dernière du produit dans ce magasin à cette date
        Private Shared Function TrouverObservation(cn As MySqlConnection, alerte As Alerte) As Integer
            If alerte.IdObservation > 0 Then Return alerte.IdObservation

            Using cmd As New MySqlCommand(
                "SELECT id FROM observation_prix " &
                "WHERE id_utilisateur = @u AND id_produit = @p AND id_magasin = @m AND date_obs < @fin " &
                "ORDER BY date_obs DESC, id DESC LIMIT 1", cn)
                cmd.Parameters.AddWithValue("@u", IdUtilisateur())
                cmd.Parameters.AddWithValue("@p", alerte.IdProduit)
                cmd.Parameters.AddWithValue("@m", alerte.IdMagasin)
                cmd.Parameters.AddWithValue("@fin", alerte.DateAlerte.Date.AddDays(1))
                Dim r As Object = cmd.ExecuteScalar()
                Return If(r Is Nothing OrElse r Is DBNull.Value, 0, Convert.ToInt32(r))
            End Using
        End Function

        ' Seuils (en %) de la table parametre ; valeurs par défaut conservées s'ils sont absents
        Private Shared Sub ChargerSeuils(cn As MySqlConnection, ByRef seuilHausse As Decimal, ByRef seuilShrink As Decimal)
            Using cmd As New MySqlCommand(
                "SELECT nom, valeur FROM parametre WHERE nom IN ('seuil_hausse_prix', 'seuil_shrinkflation')", cn)
                Using rd As MySqlDataReader = cmd.ExecuteReader()
                    While rd.Read()
                        If rd.GetString(0) = "seuil_hausse_prix" Then seuilHausse = rd.GetDecimal(1)
                        If rd.GetString(0) = "seuil_shrinkflation" Then seuilShrink = rd.GetDecimal(1)
                    End While
                End Using
            End Using
        End Sub


        Private Shared Function ConstruireDetail(type As TypeAlerte, valeur As Decimal, prixPrecedent As Decimal,
                                                 prixActuel As Decimal, quantitePrecedente As Decimal?,
                                                 quantiteActuelle As Decimal, unite As String, seuil As Decimal) As String
            Dim libelle As String = LibelleUnite(unite)
            Dim variationPrix As String = Montant(prixPrecedent) & " vers " & Montant(prixActuel) &
                                          " FCFA/" & libelle & " (+ " & Pourcentage(valeur) & " %)"

            If type = TypeAlerte.Hausse Then
                Return "Prix unitaire : " & Montant(prixPrecedent) & " FCFA/" & libelle & " vers " & Montant(prixActuel) &
                       " FCFA/" & libelle & " (+ " & Pourcentage(valeur) & " %), au-dessus du seuil de " &
                       seuil.ToString("0.##", Fr) & " %."
            End If

            If quantitePrecedente.HasValue AndAlso quantitePrecedente.Value > 0D Then
                Dim baisse As Decimal = (quantitePrecedente.Value - quantiteActuelle) / quantitePrecedente.Value
                Return "Quantité : " & Quantite(quantitePrecedente.Value, unite) & " vers " & Quantite(quantiteActuelle, unite) &
                       " (- " & Pourcentage(baisse) & " %) sans baisse de prix (" & Montant(prixActuel * quantiteActuelle) &
                       " FCFA) ; prix unitaire réel : " & variationPrix & "."
            End If
            Return "Quantité en baisse sans baisse de prix ; prix unitaire réel : " & variationPrix & "."
        End Function

        Private Shared Function LibelleUnite(unite As String) As String
            Return If(unite = "piece", "pièce", unite)
        End Function

        Private Shared Function Montant(valeur As Decimal) As String
            Return Math.Round(valeur, 0).ToString("#,##0", Fr)
        End Function

        Private Shared Function Pourcentage(fraction As Decimal) As String
            Return Math.Abs(fraction * 100D).ToString("0.0", Fr)
        End Function

        Private Shared Function Quantite(q As Decimal, unite As String) As String
            If unite = "kg" Then
                Return If(q < 1D, (q * 1000D).ToString("0.##", Fr) & " g", q.ToString("0.###", Fr) & " kg")
            End If
            If unite = "L" Then
                Return If(q < 1D, (q * 1000D).ToString("0.##", Fr) & " ml", q.ToString("0.###", Fr) & " L")
            End If
            Return q.ToString("0.###", Fr) & " pièce(s)"
        End Function

    End Class

End Namespace