Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports Microsoft.VisualBasic.FileIO

Public NotInheritable Class ImportExportService
    Private ReadOnly _referentiels As ReferentielsDepot
    Private ReadOnly _tickets As TicketService
    Private ReadOnly _historique As TicketDepot

    Public Sub New(referentiels As ReferentielsDepot, tickets As TicketService, historique As TicketDepot)
        _referentiels = referentiels
        _tickets = tickets
        _historique = historique
    End Sub

    Public Async Function ImporterAsync(chemin As String, utilisateur As UtilisateurSession) As Task(Of ResultatImport)
        Dim resultat As New ResultatImport()
        If Not File.Exists(chemin) Then Throw New FileNotFoundException("Le fichier CSV est introuvable.", chemin)
        Dim drafts As New Dictionary(Of String, List(Of LigneTicket))(StringComparer.OrdinalIgnoreCase)
        Dim dates As New Dictionary(Of String, Date)(StringComparer.OrdinalIgnoreCase)
        Dim magasins As New Dictionary(Of String, Magasin)(StringComparer.OrdinalIgnoreCase)
        Dim magasinsConnus As List(Of Magasin) = Await _referentiels.ListerMagasinsAsync()
        Dim categoriesConnues As List(Of Categorie) = Await _referentiels.ListerCategoriesAsync()
        Dim produitsConnus As List(Of Produit) = Await _referentiels.ListerProduitsAsync()
        Dim numeroLigne As Integer = 0

        Using parser As New TextFieldParser(chemin, Encoding.UTF8)
            parser.TextFieldType = FieldType.Delimited
            parser.HasFieldsEnclosedInQuotes = True
            Dim premiereLigne As String = parser.ReadLine()
            numeroLigne += 1
            If premiereLigne Is Nothing Then Throw New InvalidDataException("Le fichier CSV est vide.")
            parser.SetDelimiters(If(premiereLigne.Contains(";"), ";", ","))
            Dim colonnes As String() = premiereLigne.Split(If(premiereLigne.Contains(";"c), ";"c, ","c)).Select(Function(item) item.Trim().TrimStart(ChrW(&HFEFF)).ToLowerInvariant()).ToArray()
            Dim indices As Dictionary(Of String, Integer) = ConstruireIndex(colonnes)
            Dim obligatoires As String() = {"date", "magasin", "produit", "marque", "categorie", "unite_base", "conditionnement", "unite", "prix", "nombre"}
            Dim manquantes As IEnumerable(Of String) = obligatoires.Where(Function(nom) Not indices.ContainsKey(nom))
            If manquantes.Any() Then Throw New InvalidDataException("Colonnes CSV absentes : " & String.Join(", ", manquantes))

            While Not parser.EndOfData
                numeroLigne += 1
                Try
                    Dim champs As String() = parser.ReadFields()
                    If champs Is Nothing Then Continue While
                    Dim ligne As LigneImportee = LireLigne(champs, indices)
                    Dim magasin As Magasin = magasinsConnus.Find(Function(item) String.Equals(item.Nom, ligne.Magasin, StringComparison.OrdinalIgnoreCase))
                    Dim categorie As Categorie = categoriesConnues.Find(Function(item) String.Equals(item.Nom, ligne.Categorie, StringComparison.OrdinalIgnoreCase))
                    Dim produit As Produit = produitsConnus.Find(Function(item) String.Equals(item.Nom, ligne.Produit, StringComparison.OrdinalIgnoreCase) AndAlso String.Equals(item.Marque, ligne.Marque, StringComparison.OrdinalIgnoreCase))
                    If magasin Is Nothing Then Throw New InvalidDataException("Magasin inconnu : " & ligne.Magasin)
                    If categorie Is Nothing Then Throw New InvalidDataException("Catégorie inconnue : " & ligne.Categorie)
                    If produit Is Nothing Then Throw New InvalidDataException("Produit inconnu : " & ligne.Produit & " / " & ligne.Marque)
                    If produit.CategorieId <> categorie.Id OrElse UniteVersTexte(produit.UniteBase) <> ligne.UniteBase Then Throw New InvalidDataException("La catégorie ou l'unité de base ne correspond pas au produit enregistré.")

                    Dim ligneTicket As LigneTicket = _tickets.PreparerLigne(produit, ligne.Conditionnement, ligne.Unite, ligne.Prix, ligne.Nombre)
                    Dim cle As String = ligne.DateAchat.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) & "|" & magasin.Id.ToString(CultureInfo.InvariantCulture)
                    If Not drafts.ContainsKey(cle) Then
                        drafts.Add(cle, New List(Of LigneTicket)())
                        dates.Add(cle, ligne.DateAchat)
                        magasins.Add(cle, magasin)
                    End If
                    drafts(cle).Add(ligneTicket)
                Catch ex As Exception
                    resultat.Erreurs.Add("Ligne " & numeroLigne.ToString(CultureInfo.InvariantCulture) & " : " & ex.Message)
                End Try
            End While
        End Using

        For Each cle As String In drafts.Keys
            Try
                Dim total As Decimal = Decimal.Round(drafts(cle).Sum(Function(ligne) ligne.Total), 2)
                Await _tickets.EnregistrerAsync(utilisateur, magasins(cle).Id, dates(cle), total, drafts(cle))
                resultat.LignesImportees += drafts(cle).Count
            Catch ex As Exception
                resultat.Erreurs.Add("Ticket " & cle & " : " & ex.Message)
            End Try
        Next
        Return resultat
    End Function

    Public Async Function ExporterHistoriqueAsync(chemin As String, utilisateurId As Long, produitId As Long, magasinId As Long?) As Task
        Dim observations As List(Of ObservationPrix) = Await _historique.ListerHistoriqueAsync(utilisateurId, produitId, magasinId)
        Using writer As New StreamWriter(chemin, False, New UTF8Encoding(True))
            Await writer.WriteLineAsync("date;magasin;produit;marque;categorie;unite_base;conditionnement;unite;prix;nombre")
            For Each observation As ObservationPrix In observations
                Dim champs As String() = {
                    observation.DateObservation.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    observation.MagasinNom, observation.ProduitNom, observation.Marque, observation.CategorieNom,
                    observation.UniteBase, observation.QuantiteBase.ToString(CultureInfo.InvariantCulture), observation.UniteBase,
                    observation.PrixConditionnement.ToString(CultureInfo.InvariantCulture), "1"
                }
                Await writer.WriteLineAsync(String.Join(";", champs.Select(Function(champ) Csv(champ))))
            Next
        End Using
    End Function

    Private Shared Function ConstruireIndex(colonnes As String()) As Dictionary(Of String, Integer)
        Dim resultat As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
        For index As Integer = 0 To colonnes.Length - 1
            resultat(colonnes(index)) = index
        Next
        Return resultat
    End Function

    Private Shared Function LireLigne(champs As String(), indices As Dictionary(Of String, Integer)) As LigneImportee
        Dim dateAchat As Date
        Dim prix As Decimal
        Dim conditionnement As Decimal
        Dim nombre As Decimal
        Dim formats As CultureInfo() = {CultureInfo.InvariantCulture, CultureInfo.GetCultureInfo("fr-FR")}
        If Not Date.TryParse(Champ(champs, indices("date")), formats(1), DateTimeStyles.None, dateAchat) AndAlso Not Date.TryParse(Champ(champs, indices("date")), formats(0), DateTimeStyles.None, dateAchat) Then Throw New InvalidDataException("Date invalide.")
        If Not Decimal.TryParse(Champ(champs, indices("prix")), NumberStyles.Number, formats(1), prix) AndAlso Not Decimal.TryParse(Champ(champs, indices("prix")), NumberStyles.Number, formats(0), prix) Then Throw New InvalidDataException("Prix invalide.")
        If Not Decimal.TryParse(Champ(champs, indices("conditionnement")), NumberStyles.Number, formats(1), conditionnement) AndAlso Not Decimal.TryParse(Champ(champs, indices("conditionnement")), NumberStyles.Number, formats(0), conditionnement) Then Throw New InvalidDataException("Conditionnement invalide.")
        If Not Decimal.TryParse(Champ(champs, indices("nombre")), NumberStyles.Number, formats(1), nombre) AndAlso Not Decimal.TryParse(Champ(champs, indices("nombre")), NumberStyles.Number, formats(0), nombre) Then Throw New InvalidDataException("Nombre acheté invalide.")
        If prix < 0D OrElse conditionnement <= 0D OrElse nombre <= 0D Then Throw New InvalidDataException("Les valeurs numériques doivent être positives (le prix peut être nul).")
        Return New LigneImportee With {
            .DateAchat = dateAchat.Date, .Magasin = Champ(champs, indices("magasin")),
            .Produit = Champ(champs, indices("produit")), .Marque = Champ(champs, indices("marque")),
            .Categorie = Champ(champs, indices("categorie")), .UniteBase = Champ(champs, indices("unite_base")),
            .Conditionnement = conditionnement, .Unite = Champ(champs, indices("unite")), .Prix = prix, .Nombre = nombre
        }
    End Function

    Private Shared Function Champ(champs As String(), index As Integer) As String
        If index < 0 OrElse index >= champs.Length Then Throw New InvalidDataException("Nombre de champs incorrect.")
        Return champs(index).Trim()
    End Function

    Private Shared Function UniteVersTexte(unite As UniteProduit) As String
        Select Case unite
            Case UniteProduit.Kilogramme
                Return "kg"
            Case UniteProduit.Litre
                Return "L"
            Case Else
                Return "pièce"
        End Select
    End Function

    Private Shared Function Csv(valeur As String) As String
        Return ChrW(34) & If(valeur, String.Empty).Replace(ChrW(34).ToString(), New String(ChrW(34), 2)) & ChrW(34)
    End Function
End Class
