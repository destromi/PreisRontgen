Imports System.Globalization
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.FileIO
Imports MySqlConnector

' =====================================================================
' ImportExportService : import / export CSV des relevés de prix.
' Aucune référence à un formulaire : CSV_Depense appelle ces méthodes
' et ne s'occupe que de l'affichage.
' Si ImportExportService.vb existe déjà, copiez-y ResultatImport et le
' contenu du Module (et fusionnez les Imports).
' =====================================================================

' Résultat d'un import (lu par le formulaire)
Public Class ResultatImport
    Public Importees As Integer
    Public Tickets As Integer
    Public ReadOnly Erreurs As New List(Of String)()
End Class

Public Module ImportExportService

    Private Class ProduitInfo
        Public Id As Integer              ' 0 = produit à créer pendant l'import
        Public UniteBase As String
        Public Categorie As String
    End Class

    Private Class LigneCsv
        Public Numero As Integer
        Public DateAchat As Date
        Public Magasin As String
        Public Produit As String
        Public Marque As String
        Public Categorie As String
        Public UniteBaseCsv As String
        Public Conditionnement As Decimal
        Public Unite As String
        Public Prix As Decimal
        Public Nombre As Integer
        ' calculés pendant l'import
        Public UniteBase As String
        Public QuantiteBase As Decimal
        Public Info As ProduitInfo
    End Class

    Private ReadOnly FormatsDate As String() =
        {"dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "yyyy-MM-dd", "dd/MM/yy"}

    ' Lit le fichier, rejette les lignes invalides, enregistre les autres en une seule transaction
    Public Function ImporterReleves(chemin As String, idUtilisateur As Integer) As ResultatImport
        Dim resultat As New ResultatImport()
        Dim lignes As List(Of LigneCsv) = LireFichier(chemin, resultat)
        If lignes.Count = 0 Then Return resultat

        Using cn As MySqlConnection = GetConnection
            cn.Open()
            Using tx As MySqlTransaction = cn.BeginTransaction()
                Try
                    EnregistrerLignes(cn, tx, idUtilisateur, lignes, resultat)
                    tx.Commit()
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using
        Return resultat
    End Function

    ' ----- Lecture et contrôle du fichier -----

    Private Function LireFichier(chemin As String, resultat As ResultatImport) As List(Of LigneCsv)
        Dim lignes As New List(Of LigneCsv)()

        ' UTF-8 si possible, sinon Windows (Excel enregistre souvent en ANSI)
        Dim octets As Byte() = File.ReadAllBytes(chemin)
        Dim texte As String
        Try
            texte = New UTF8Encoding(False, True).GetString(octets)
        Catch ex As DecoderFallbackException
            texte = Encoding.Latin1.GetString(octets)
        End Try
        texte = texte.TrimStart(ChrW(&HFEFF))
        If texte.Trim() = "" Then Throw New InvalidDataException("Le fichier est vide.")

        ' Séparateur : détecté sur la première ligne (; , ou tabulation)
        Dim premiere As String = texte.Split(New Char() {ChrW(10), ChrW(13)}, StringSplitOptions.RemoveEmptyEntries)(0)
        Dim nbPointVirgule As Integer = premiere.Count(Function(c) c = ";"c)
        Dim nbVirgule As Integer = premiere.Count(Function(c) c = ","c)
        Dim nbTab As Integer = premiere.Count(Function(c) c = ChrW(9))
        Dim separateur As String = ";"
        If nbTab > nbPointVirgule AndAlso nbTab > nbVirgule Then
            separateur = vbTab
        ElseIf nbVirgule > nbPointVirgule Then
            separateur = ","
        End If

        Using lecteur As New StringReader(texte)
            Using parser As New TextFieldParser(lecteur)
                parser.TextFieldType = FieldType.Delimited
                parser.SetDelimiters(separateur)
                parser.HasFieldsEnclosedInQuotes = True
                parser.TrimWhiteSpace = True

                ' En-tête : on repère les colonnes par leur nom (sans accents ni majuscules)
                Dim entete As String() = parser.ReadFields()
                If entete Is Nothing Then Throw New InvalidDataException("Le fichier est vide.")
                Dim idx As New Dictionary(Of String, Integer)()
                For i As Integer = 0 To entete.Length - 1
                    Dim k As String = Cle(entete(i))
                    If k <> "" AndAlso Not idx.ContainsKey(k) Then idx(k) = i
                Next

                Dim requises As String()() = {
                    New String() {"date"}, New String() {"magasins", "magasin"},
                    New String() {"produits", "produit"}, New String() {"conditionnement"},
                    New String() {"prix"}, New String() {"unite"}, New String() {"nombre"}}
                For Each groupe As String() In requises
                    If Not groupe.Any(Function(n) idx.ContainsKey(n)) Then
                        Throw New InvalidDataException("Colonne manquante dans l'en-tête : « " & groupe(0) & " ».")
                    End If
                Next

                While Not parser.EndOfData
                    Dim champs As String() = Nothing
                    Try
                        champs = parser.ReadFields()
                    Catch ex As MalformedLineException
                        resultat.Erreurs.Add("Ligne " & ex.LineNumber.ToString(CultureInfo.InvariantCulture) & " : format invalide (guillemets mal fermés).")
                        Continue While
                    End Try

                    If champs Is Nothing OrElse champs.All(Function(c) c.Trim() = "") Then Continue While

                    Dim numero As Integer = CInt(parser.LineNumber) - 1
                    Dim ligne As LigneCsv = AnalyserLigne(numero, champs, idx, resultat)
                    If ligne IsNot Nothing Then lignes.Add(ligne)
                End While
            End Using
        End Using

        Return lignes
    End Function

    Private Function AnalyserLigne(numero As Integer, champs As String(), idx As Dictionary(Of String, Integer),
                                   resultat As ResultatImport) As LigneCsv
        Dim l As New LigneCsv() With {.Numero = numero}
        Dim txt As String

        txt = Lire(champs, idx, "date")
        If Not Date.TryParseExact(txt, FormatsDate, CultureInfo.InvariantCulture, DateTimeStyles.None, l.DateAchat) Then
            Return Rejeter(resultat, numero, "date invalide (« " & txt & " »), format attendu jj/mm/aaaa.")
        End If
        If l.DateAchat.Date > Date.Today Then Return Rejeter(resultat, numero, "date dans le futur.")

        l.Magasin = Lire(champs, idx, "magasins", "magasin")
        If l.Magasin = "" Then Return Rejeter(resultat, numero, "magasin manquant.")
        If l.Magasin.Length > 150 Then Return Rejeter(resultat, numero, "nom de magasin trop long (150 caractères maximum).")

        l.Produit = Lire(champs, idx, "produits", "produit")
        If l.Produit = "" Then Return Rejeter(resultat, numero, "produit manquant.")
        If l.Produit.Length > 150 Then Return Rejeter(resultat, numero, "nom de produit trop long (150 caractères maximum).")

        l.Marque = Lire(champs, idx, "marque")
        If l.Marque.Length > 100 Then Return Rejeter(resultat, numero, "marque trop longue (100 caractères maximum).")

        l.Categorie = Lire(champs, idx, "categorie")
        If l.Categorie.Length > 100 Then Return Rejeter(resultat, numero, "catégorie trop longue (100 caractères maximum).")

        l.UniteBaseCsv = ""
        txt = Lire(champs, idx, "unite de base", "unite base")
        If txt <> "" Then
            Dim ub As String = NormaliserUnite(txt)
            If ub Is Nothing OrElse ub = "g" OrElse ub = "cL" OrElse ub = "mL" Then
                Return Rejeter(resultat, numero, "unité de base invalide (« " & txt & " »), attendu : kg, L ou piece.")
            End If
            l.UniteBaseCsv = ub
        End If

        If Not TryLireDecimal(Lire(champs, idx, "conditionnement"), l.Conditionnement) OrElse l.Conditionnement <= 0D Then
            Return Rejeter(resultat, numero, "conditionnement invalide (nombre supérieur à 0 attendu).")
        End If

        Dim u As String = NormaliserUnite(Lire(champs, idx, "unite"))
        If u Is Nothing Then Return Rejeter(resultat, numero, "unité invalide (attendu : kg, g, L, cL, mL ou piece).")
        l.Unite = u

        If Not TryLireDecimal(Lire(champs, idx, "prix"), l.Prix) OrElse l.Prix < 0D Then
            Return Rejeter(resultat, numero, "prix invalide (nombre positif attendu).")
        End If
        If l.Prix > 9999999999D Then Return Rejeter(resultat, numero, "prix trop élevé.")

        If Not Integer.TryParse(Lire(champs, idx, "nombre"), NumberStyles.Integer, CultureInfo.InvariantCulture, l.Nombre) _
           OrElse l.Nombre <= 0 Then
            Return Rejeter(resultat, numero, "nombre invalide (entier supérieur à 0 attendu).")
        End If

        Return l
    End Function

    Private Function Rejeter(resultat As ResultatImport, numero As Integer, motif As String) As LigneCsv
        resultat.Erreurs.Add("Ligne " & numero.ToString(CultureInfo.InvariantCulture) & " : " & motif)
        Return Nothing
    End Function

    ' ----- Enregistrement en base -----

    Private Sub EnregistrerLignes(cn As MySqlConnection, tx As MySqlTransaction, idUtilisateur As Integer,
                                  lignes As List(Of LigneCsv), resultat As ResultatImport)
        ' 1. Ce qui existe déjà (comparaison sans accents ni majuscules, comme MySQL)
        Dim magasins As Dictionary(Of String, Integer) = ChargerCles(cn, tx, "SELECT id, nom FROM magasin")
        Dim categories As Dictionary(Of String, Integer) = ChargerCles(cn, tx, "SELECT id, nom FROM categorie")
        Dim produits As New Dictionary(Of String, ProduitInfo)()
        Using cmd As New MySqlCommand("SELECT id, nom, marque, unite_base FROM produit", cn, tx)
            Using rd As MySqlDataReader = cmd.ExecuteReader()
                While rd.Read()
                    produits(Cle(rd.GetString(1)) & "|" & Cle(rd.GetString(2))) =
                        New ProduitInfo() With {.Id = rd.GetInt32(0), .UniteBase = rd.GetString(3)}
                End While
            End Using
        End Using

        ' 2. Contrôles qui dépendent du produit (unité de base, conversion)
        Dim retenues As New List(Of LigneCsv)()
        For Each l As LigneCsv In lignes
            Dim cleProduit As String = Cle(l.Produit) & "|" & Cle(l.Marque)
            Dim info As ProduitInfo = Nothing

            If produits.TryGetValue(cleProduit, info) Then
                If l.UniteBaseCsv <> "" AndAlso l.UniteBaseCsv <> info.UniteBase Then
                    Rejeter(resultat, l.Numero, "unité de base « " & l.UniteBaseCsv & " » différente de celle du produit (« " & info.UniteBase & " »).")
                    Continue For
                End If
            Else
                If l.Categorie = "" Then
                    Rejeter(resultat, l.Numero, "produit inconnu : la catégorie est obligatoire pour le créer.")
                    Continue For
                End If
                If l.UniteBaseCsv = "" Then
                    Rejeter(resultat, l.Numero, "produit inconnu : l'unité de base est obligatoire pour le créer.")
                    Continue For
                End If
                info = New ProduitInfo() With {.Id = 0, .UniteBase = l.UniteBaseCsv, .Categorie = l.Categorie}
                produits(cleProduit) = info
            End If
            l.UniteBase = info.UniteBase

            Dim qb As Decimal? = ConvertirVersBase(l.Conditionnement, l.Unite, l.UniteBase)
            If Not qb.HasValue Then
                Rejeter(resultat, l.Numero, "unité « " & l.Unite & " » incompatible avec l'unité de base du produit (" & l.UniteBase & ").")
                Continue For
            End If
            Dim qbArrondi As Decimal = Math.Round(qb.Value, 3)
            If qbArrondi <= 0D Then
                Rejeter(resultat, l.Numero, "quantité trop petite.")
                Continue For
            End If
            If l.Prix / qbArrondi > 99999999D Then
                Rejeter(resultat, l.Numero, "prix unitaire trop élevé.")
                Continue For
            End If

            l.QuantiteBase = qbArrondi
            l.Info = info
            retenues.Add(l)
        Next

        ' 3. Un ticket par couple date + magasin
        Dim groupes = retenues.GroupBy(
            Function(x) x.DateAchat.ToString("yyyyMMdd", CultureInfo.InvariantCulture) & "|" & Cle(x.Magasin))

        For Each groupe In groupes
            Dim premiere As LigneCsv = groupe.First()

            Dim cleMagasin As String = Cle(premiere.Magasin)
            Dim idMagasin As Integer
            If Not magasins.TryGetValue(cleMagasin, idMagasin) Then
                idMagasin = CInt(Inserer(cn, tx, "INSERT INTO magasin (nom) VALUES (@nom)",
                                         New MySqlParameter("@nom", premiere.Magasin)))
                magasins(cleMagasin) = idMagasin
            End If

            Dim total As Decimal = Math.Round(groupe.Sum(Function(x) x.Prix * x.Nombre), 2)
            Dim idTicket As Long = Inserer(cn, tx,
                "INSERT INTO ticket (id_utilisateur, id_magasin, date_achat, total_saisi) VALUES (@u, @m, @d, @t)",
                New MySqlParameter("@u", idUtilisateur), New MySqlParameter("@m", idMagasin),
                New MySqlParameter("@d", premiere.DateAchat.Date), New MySqlParameter("@t", total))
            resultat.Tickets += 1

            For Each l As LigneCsv In groupe
                Dim info As ProduitInfo = l.Info

                ' Produit (et catégorie) créés à la première ligne qui les utilise
                If info.Id = 0 Then
                    Dim cleCategorie As String = Cle(info.Categorie)
                    Dim idCategorie As Integer
                    If Not categories.TryGetValue(cleCategorie, idCategorie) Then
                        idCategorie = CInt(Inserer(cn, tx, "INSERT INTO categorie (nom) VALUES (@nom)",
                                                   New MySqlParameter("@nom", info.Categorie)))
                        categories(cleCategorie) = idCategorie
                    End If
                    info.Id = CInt(Inserer(cn, tx,
                        "INSERT INTO produit (nom, marque, id_categorie, unite_base) VALUES (@nom, @marque, @cat, @unite)",
                        New MySqlParameter("@nom", l.Produit), New MySqlParameter("@marque", l.Marque),
                        New MySqlParameter("@cat", idCategorie), New MySqlParameter("@unite", info.UniteBase)))
                End If

                Dim idLigne As Long = Inserer(cn, tx,
                    "INSERT INTO ligne_ticket (id_ticket, id_produit, quantite_conditionnement, unite, prix, nb_achete) " &
                    "VALUES (@t, @p, @q, @u, @prix, @nb)",
                    New MySqlParameter("@t", idTicket), New MySqlParameter("@p", info.Id),
                    New MySqlParameter("@q", Math.Round(l.Conditionnement, 3)), New MySqlParameter("@u", l.Unite),
                    New MySqlParameter("@prix", Math.Round(l.Prix, 2)), New MySqlParameter("@nb", l.Nombre))

                ' Observation de prix : prix par unité de base (kg, L ou pièce)
                Inserer(cn, tx,
                    "INSERT INTO observation_prix (id_utilisateur, id_produit, id_magasin, id_ligne_ticket, date_obs, prix_unitaire, quantite_base) " &
                    "VALUES (@u, @p, @m, @l, @d, @pu, @qb)",
                    New MySqlParameter("@u", idUtilisateur), New MySqlParameter("@p", info.Id),
                    New MySqlParameter("@m", idMagasin), New MySqlParameter("@l", idLigne),
                    New MySqlParameter("@d", l.DateAchat.Date),
                    New MySqlParameter("@pu", Math.Round(l.Prix / l.QuantiteBase, 4)),
                    New MySqlParameter("@qb", l.QuantiteBase))

                resultat.Importees += 1
            Next
        Next
    End Sub

    Private Function ChargerCles(cn As MySqlConnection, tx As MySqlTransaction, sql As String) As Dictionary(Of String, Integer)
        Dim d As New Dictionary(Of String, Integer)()
        Using cmd As New MySqlCommand(sql, cn, tx)
            Using rd As MySqlDataReader = cmd.ExecuteReader()
                While rd.Read()
                    d(Cle(rd.GetString(1))) = rd.GetInt32(0)
                End While
            End Using
        End Using
        Return d
    End Function

    Private Function Inserer(cn As MySqlConnection, tx As MySqlTransaction, sql As String,
                                    ParamArray parametres As MySqlParameter()) As Long
        Using cmd As New MySqlCommand(sql, cn, tx)
            cmd.Parameters.AddRange(parametres)
            cmd.ExecuteNonQuery()
            Return cmd.LastInsertedId
        End Using
    End Function

    ' Un texte qui contient ; " ou un retour à la ligne doit être entouré de guillemets
    Private Function Champ(valeur As Object) As String
        Dim t As String = Convert.ToString(valeur, CultureInfo.InvariantCulture)
        If t Is Nothing Then Return ""
        If t.IndexOfAny(New Char() {";"c, """"c, ChrW(10), ChrW(13)}) >= 0 Then
            Return """" & t.Replace("""", """""") & """"
        End If
        Return t
    End Function

    ' Nombre avec virgule décimale (Excel en français)
    Private Function Nombre(valeur As Object) As String
        Return Convert.ToDecimal(valeur, CultureInfo.InvariantCulture).ToString("0.####", CultureInfo.InvariantCulture).Replace(".", ",")
    End Function

    ' ================= Outils =================

    ' Texte sans accents, sans majuscules, espaces simplifiés : sert à comparer des noms
    Private Function Cle(texte As String) As String
        If texte Is Nothing Then Return ""
        Dim decompose As String = texte.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD)
        Dim sb As New StringBuilder()
        For Each c As Char In decompose
            If CharUnicodeInfo.GetUnicodeCategory(c) <> UnicodeCategory.NonSpacingMark Then sb.Append(c)
        Next
        Dim propre As String = sb.ToString().Replace("_", " ").Replace("-", " ")
        Dim mots As String() = propre.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        Return String.Join(" ", mots)
    End Function

    Private Function Lire(champs As String(), idx As Dictionary(Of String, Integer), ParamArray noms As String()) As String
        For Each nom As String In noms
            Dim i As Integer
            If idx.TryGetValue(nom, i) AndAlso i < champs.Length Then Return champs(i).Trim()
        Next
        Return ""
    End Function

    Private Function NormaliserUnite(texte As String) As String
        Select Case Cle(texte)
            Case "kg" : Return "kg"
            Case "g" : Return "g"
            Case "l", "litre", "litres" : Return "L"
            Case "cl" : Return "cL"
            Case "ml" : Return "mL"
            Case "piece", "pieces", "pc", "unite", "u" : Return "piece"
        End Select
        Return Nothing
    End Function

    ' Accepte "1500", "1 500", "1500,50" ou "1500.50"
    Private Function TryLireDecimal(texte As String, ByRef valeur As Decimal) As Boolean
        Dim t As String = texte.Trim().Replace(" ", "").Replace(ChrW(160), "").Replace(",", ".")
        Return Decimal.TryParse(t, NumberStyles.Number, CultureInfo.InvariantCulture, valeur)
    End Function

    ' Convertit vers l'unité de base du produit (Nothing si incompatible)
    Private Function ConvertirVersBase(quantite As Decimal, unite As String, uniteBase As String) As Decimal?
        Select Case uniteBase
            Case "kg"
                If unite = "kg" Then Return quantite
                If unite = "g" Then Return quantite / 1000D
            Case "L"
                If unite = "L" Then Return quantite
                If unite = "cL" Then Return quantite / 100D
                If unite = "mL" Then Return quantite / 1000D
            Case "piece"
                If unite = "piece" Then Return quantite
        End Select
        Return Nothing
    End Function

    ' ================= Export (lecture et écriture) =================

    Public Function ListerProduits() As DataTable
        Dim dt As New DataTable()
        Using cn As MySqlConnection = GetConnection
            Using da As New MySqlDataAdapter(
                "SELECT id, CASE WHEN marque = '' THEN nom ELSE CONCAT(nom, ' - ', marque) END AS libelle " &
                "FROM produit ORDER BY nom, marque", cn)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    Public Function ListerMagasins() As DataTable
        Dim dt As New DataTable()
        Using cn As MySqlConnection = GetConnection
            Using da As New MySqlDataAdapter("SELECT id, nom FROM magasin ORDER BY nom", cn)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    ' Historique d'un produit pour un utilisateur (idMagasin = 0 : tous les magasins).
    ' Mêmes colonnes que l'import : le fichier exporté peut être corrigé puis réimporté.
    Public Function LireHistorique(idUtilisateur As Integer, idProduit As Integer, idMagasin As Integer) As DataTable
        Dim dt As New DataTable()
        Dim sql As String =
            "SELECT o.date_obs, m.nom AS magasin, p.nom AS produit, p.marque, c.nom AS categorie, p.unite_base, " &
            "COALESCE(l.quantite_conditionnement, o.quantite_base) AS conditionnement, " &
            "COALESCE(l.prix, ROUND(o.prix_unitaire * o.quantite_base, 2)) AS prix, " &
            "COALESCE(l.unite, p.unite_base) AS unite, " &
            "COALESCE(l.nb_achete, 1) AS nombre " &
            "FROM observation_prix o " &
            "INNER JOIN produit p ON p.id = o.id_produit " &
            "INNER JOIN categorie c ON c.id = p.id_categorie " &
            "INNER JOIN magasin m ON m.id = o.id_magasin " &
            "LEFT JOIN ligne_ticket l ON l.id = o.id_ligne_ticket " &
            "WHERE o.id_utilisateur = @u AND o.id_produit = @p AND (@m = 0 OR o.id_magasin = @m) " &
            "ORDER BY o.date_obs, m.nom"
        Using cn As MySqlConnection = GetConnection
            Using cmd As New MySqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("@u", idUtilisateur)
                cmd.Parameters.AddWithValue("@p", idProduit)
                cmd.Parameters.AddWithValue("@m", idMagasin)
                Using da As New MySqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using
        Return dt
    End Function

    ' Écrit le résultat de LireHistorique dans un fichier CSV (séparateur ;, UTF-8 avec marque pour Excel)
    Public Sub EcrireCsv(dt As DataTable, chemin As String)
        Dim sb As New StringBuilder()
        sb.AppendLine("date;magasins;produits;marque;catégorie;unité de base;conditionnement;prix;unité;nombre")
        For Each r As DataRow In dt.Rows
            sb.AppendLine(String.Join(";", New String() {
                Convert.ToDateTime(r("date_obs")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Champ(r("magasin")), Champ(r("produit")), Champ(r("marque")), Champ(r("categorie")),
                Champ(r("unite_base")), Nombre(r("conditionnement")), Nombre(r("prix")),
                Champ(r("unite")), Convert.ToString(r("nombre"), CultureInfo.InvariantCulture)}))
        Next
        File.WriteAllText(chemin, sb.ToString(), New UTF8Encoding(True))
    End Sub

End Module