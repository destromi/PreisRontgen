Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks

Public NotInheritable Class TesseractOcrService
    Private Shared ReadOnly RegexMontants As New Regex(
        "(?<![\p{L}\d])(\d{1,3}(?:[ \u00A0\u202F.]\d{3})+(?:,\d{1,2})?|\d{1,6}(?:[,.]\d{1,2})?)(?:\s*(?:FCFA|CFA|XAF)\b)?(?![\p{L}\d])(?!\s*(?:kg|mg|ml|cl|g|l|pcs?|unit[eé]s?)\b)(?!\s*[x×])",
        RegexOptions.Compiled Or RegexOptions.CultureInvariant Or RegexOptions.IgnoreCase)
    Private Shared ReadOnly RegexConditionnement As New Regex(
        "(?<![\p{L}\d])(?<quantite>\d+(?:[,.]\d{1,3})?)\s*(?<unite>kg|mg|g|lt|l|cl|ml)\b",
        RegexOptions.Compiled Or RegexOptions.CultureInvariant Or RegexOptions.IgnoreCase)
    Private Shared ReadOnly RegexNombreAchete As New Regex(
        "(?:\b(?:qte|qty|quantite|quantité)\s*[:=]?\s*(?<nombre>\d+(?:[,.]\d+)?)|\b(?<nombre>\d+(?:[,.]\d+)?)\s*[x×]\s*\d+(?:[,.]\d+)?\s*(?:kg|mg|g|lt|l|cl|ml)\b|[x×]\s*(?<nombre>\d+(?:[,.]\d+)?)\b)",
        RegexOptions.Compiled Or RegexOptions.CultureInvariant Or RegexOptions.IgnoreCase)

    Public Async Function LireTicketAsync(cheminImage As String, produits As IList(Of Produit)) As Task(Of List(Of LigneOcr))
        If Not File.Exists(cheminImage) Then Throw New FileNotFoundException("La photographie du ticket est introuvable.", cheminImage)
        Dim executable As String = Configuration.CheminTesseract
        If String.IsNullOrWhiteSpace(executable) OrElse Not File.Exists(executable) Then Throw New InvalidOperationException("Configurez PRICIEUX_TESSERACT_PATH avec le chemin de tesseract.exe pour activer l'OCR.")
        Dim nomTemporaire As String = Path.Combine(Path.GetTempPath(), "pricieux-ocr-" & Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))
        Dim fichierTexte As String = nomTemporaire & ".txt"
        Try
            Dim demarrage As New ProcessStartInfo With {.FileName = executable, .UseShellExecute = False, .CreateNoWindow = True}
            demarrage.ArgumentList.Add(cheminImage)
            demarrage.ArgumentList.Add(nomTemporaire)
            demarrage.ArgumentList.Add("-l")
            demarrage.ArgumentList.Add("fra+eng")
            demarrage.ArgumentList.Add("--psm")
            demarrage.ArgumentList.Add("6")
            demarrage.ArgumentList.Add("-c")
            demarrage.ArgumentList.Add("preserve_interword_spaces=1")
            Using processus As New Process With {.StartInfo = demarrage}
                If Not processus.Start() Then Throw New InvalidOperationException("Le moteur OCR n'a pas pu démarrer.")
                Await processus.WaitForExitAsync()
                If processus.ExitCode <> 0 OrElse Not File.Exists(fichierTexte) Then Throw New InvalidOperationException("Tesseract n'a pas réussi à lire l'image. Vérifiez le format et les données de langue.")
            End Using
            Dim lignesTexte As String() = Await File.ReadAllLinesAsync(fichierTexte)
            Return ExtraireCandidats(lignesTexte, produits)
        Finally
            If File.Exists(fichierTexte) Then File.Delete(fichierTexte)
        End Try
    End Function

    Private Shared Function ExtraireCandidats(lignesTexte As IEnumerable(Of String), produits As IList(Of Produit)) As List(Of LigneOcr)
        Dim lignes As String() = lignesTexte.Select(Function(ligne As String) If(ligne, String.Empty).Trim()).Where(Function(ligne As String) ligne.Length > 0).ToArray()
        Dim resultat As New List(Of LigneOcr)()
        Dim index As Integer = 0

        While index < lignes.Length
            Dim texte As String = lignes(index)
            Dim correspondance As CorrespondanceProduit = TrouverProduit(texte, produits)
            Dim prix As Decimal
            Dim contientPrix As Boolean = EssayerLireMontant(texte, prix)
            Dim lignesConsommees As Integer = 1

            If correspondance IsNot Nothing AndAlso contientPrix Then
                resultat.Add(CreerLigne(correspondance, prix, texte))
            ElseIf index + 1 < lignes.Length Then
                Dim texteSuivant As String = lignes(index + 1)
                Dim produitSuivant As CorrespondanceProduit = TrouverProduit(texteSuivant, produits)
                Dim prixSuivant As Decimal
                Dim contientPrixSuivant As Boolean = EssayerLireMontant(texteSuivant, prixSuivant)

                If correspondance IsNot Nothing AndAlso Not contientPrix AndAlso produitSuivant Is Nothing AndAlso contientPrixSuivant Then
                    resultat.Add(CreerLigne(correspondance, prixSuivant, texte & " " & texteSuivant))
                    lignesConsommees = 2
                ElseIf correspondance Is Nothing AndAlso contientPrix AndAlso produitSuivant IsNot Nothing AndAlso Not contientPrixSuivant Then
                    resultat.Add(CreerLigne(produitSuivant, prix, texte & " " & texteSuivant))
                    lignesConsommees = 2
                ElseIf correspondance Is Nothing AndAlso produitSuivant Is Nothing Then
                    Dim texteCombine As String = texte & " " & texteSuivant
                    Dim correspondanceCombinee As CorrespondanceProduit = TrouverProduit(texteCombine, produits)
                    If correspondanceCombinee IsNot Nothing AndAlso (contientPrix OrElse contientPrixSuivant) Then
                        resultat.Add(CreerLigne(correspondanceCombinee, If(contientPrix, prix, prixSuivant), texteCombine))
                        lignesConsommees = 2
                    End If
                End If
            End If

            index += lignesConsommees
        End While

        Return resultat
    End Function

    Private Shared Function CreerLigne(correspondance As CorrespondanceProduit, prix As Decimal, texte As String) As LigneOcr
        Dim ligne As New LigneOcr With {
            .Produit = correspondance.Produit,
            .PrixLu = prix,
            .TexteOriginal = texte.Trim(),
            .Confiance = correspondance.Score
        }
        LireConditionnementEtNombre(ligne)
        Return ligne
    End Function

    Private Shared Sub LireConditionnementEtNombre(ligne As LigneOcr)
        For Each correspondance As Match In RegexConditionnement.Matches(ligne.TexteOriginal)
            Dim uniteLue As String = correspondance.Groups("unite").Value.ToLowerInvariant()
            Dim uniteBase As UniteProduit = ligne.Produit.UniteBase
            Dim uniteCompatible As Boolean =
                (uniteBase = UniteProduit.Kilogramme AndAlso New String() {"kg", "g", "mg"}.Contains(uniteLue)) OrElse
                (uniteBase = UniteProduit.Litre AndAlso New String() {"l", "lt", "cl", "ml"}.Contains(uniteLue))
            If Not uniteCompatible Then Continue For

            Dim quantite As Decimal
            If Not LireMontant(correspondance.Groups("quantite").Value, quantite) OrElse quantite <= 0D Then Continue For
            ligne.QuantiteConditionnement = quantite
            ligne.UniteConditionnement = If(uniteLue = "lt", "L", If(uniteLue = "l", "L", uniteLue))
            Exit For
        Next

        Dim quantiteAchetee As Match = RegexNombreAchete.Match(ligne.TexteOriginal)
        If quantiteAchetee.Success Then
            Dim nombre As Decimal
            If LireMontant(quantiteAchetee.Groups("nombre").Value, nombre) AndAlso nombre > 0D Then ligne.NombreAchete = nombre
        End If
    End Sub

    Private Shared Function TrouverProduit(texte As String, produits As IList(Of Produit)) As CorrespondanceProduit
        If String.IsNullOrWhiteSpace(texte) OrElse produits Is Nothing OrElse produits.Count = 0 Then Return Nothing
        Dim jetonsTexte As String() = Jetons(Normaliser(texte))
        If jetonsTexte.Length = 0 Then Return Nothing

        Dim meilleure As CorrespondanceProduit = Nothing
        For Each produit As Produit In produits
            If produit Is Nothing Then Continue For
            Dim jetonsProduit As String() = Jetons(Normaliser(produit.Nom)).Where(Function(jeton As String) jeton.Length >= 2).ToArray()
            If jetonsProduit.Length = 0 Then Continue For

            Dim correspondances As Integer = 0
            Dim scoreTotal As Decimal = 0D
            For Each jetonProduit As String In jetonsProduit
                Dim meilleurScoreJeton As Decimal = 0D
                For Each jetonTexte As String In jetonsTexte
                    meilleurScoreJeton = Math.Max(meilleurScoreJeton, SimilariteJetons(jetonProduit, jetonTexte))
                    If meilleurScoreJeton >= 1D Then Exit For
                Next
                If meilleurScoreJeton >= 0.75D Then correspondances += 1
                scoreTotal += meilleurScoreJeton
            Next

            Dim minimumCorrespondances As Integer = If(jetonsProduit.Length = 1, 1, CInt(Math.Ceiling(jetonsProduit.Length * 0.6D)))
            Dim score As Decimal = scoreTotal / jetonsProduit.Length
            If correspondances < minimumCorrespondances OrElse score < 0.58D Then Continue For
            If jetonsProduit.Length = 1 AndAlso score < 0.75D Then Continue For

            If meilleure Is Nothing OrElse score > meilleure.Score OrElse
               (score = meilleure.Score AndAlso produit.Nom.Length > meilleure.Produit.Nom.Length) Then
                meilleure = New CorrespondanceProduit With {.Produit = produit, .Score = score}
            End If
        Next
        Return meilleure
    End Function

    Private Shared Function SimilariteJetons(attendu As String, lu As String) As Decimal
        If String.Equals(attendu, lu, StringComparison.Ordinal) Then Return 1D
        If attendu.Length < 4 OrElse lu.Length < 4 Then Return 0D
        If Math.Abs(attendu.Length - lu.Length) > 1 Then Return 0D
        If DistanceEdition(attendu, lu) = 1 Then Return 0.8D
        Return 0D
    End Function

    Private Shared Function DistanceEdition(premier As String, second As String) As Integer
        Dim distances(premier.Length, second.Length) As Integer
        For index As Integer = 0 To premier.Length
            distances(index, 0) = index
        Next
        For index As Integer = 0 To second.Length
            distances(0, index) = index
        Next
        For indexPremier As Integer = 1 To premier.Length
            For indexSecond As Integer = 1 To second.Length
                Dim cout As Integer = If(premier(indexPremier - 1) = second(indexSecond - 1), 0, 1)
                distances(indexPremier, indexSecond) = Math.Min(
                    Math.Min(distances(indexPremier - 1, indexSecond) + 1, distances(indexPremier, indexSecond - 1) + 1),
                    distances(indexPremier - 1, indexSecond - 1) + cout)
            Next
        Next
        Return distances(premier.Length, second.Length)
    End Function

    Private Shared Function EssayerLireMontant(texte As String, ByRef montant As Decimal) As Boolean
        Dim texteComplet As String = If(texte, String.Empty)
        Dim correspondances As MatchCollection = RegexMontants.Matches(texteComplet)
        For index As Integer = correspondances.Count - 1 To 0 Step -1
            Dim correspondance As Match = correspondances(index)
            Dim avant As String = texteComplet.Substring(0, correspondance.Index)
            If Regex.IsMatch(avant, "(?:\bqte|\bqty|\bquantit[eé])\s*[:=]?\s*$", RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant) Then Continue For
            Dim apresIndex As Integer = correspondance.Index + correspondance.Length
            If Regex.IsMatch(texteComplet.Substring(apresIndex), "^\s*[x×]", RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant) Then Continue For
            If LireMontant(correspondance.Groups(1).Value, montant) Then Return True
        Next
        montant = 0D
        Return False
    End Function

    Private Shared Function LireMontant(texte As String, ByRef montant As Decimal) As Boolean
        Dim valeur As String = texte.Replace(" ", String.Empty).Replace(ChrW(&HA0).ToString(), String.Empty).Replace(ChrW(&H202F).ToString(), String.Empty)
        Dim positionVirgule As Integer = valeur.LastIndexOf(","c)
        Dim positionPoint As Integer = valeur.LastIndexOf("."c)
        Dim normalisee As String

        If positionVirgule >= 0 AndAlso positionPoint >= 0 Then
            If positionVirgule > positionPoint Then
                normalisee = valeur.Replace(".", String.Empty).Replace(",", ".")
            Else
                normalisee = valeur.Replace(",", String.Empty)
            End If
        ElseIf positionVirgule >= 0 OrElse positionPoint >= 0 Then
            Dim separateur As Char = If(positionVirgule >= 0, ","c, "."c)
            Dim segments As String() = valeur.Split(separateur)
            If segments.Length > 2 OrElse segments(segments.Length - 1).Length = 3 Then
                normalisee = valeur.Replace(separateur.ToString(), String.Empty)
            ElseIf segments(segments.Length - 1).Length <= 2 Then
                normalisee = valeur.Replace(separateur, "."c)
            Else
                normalisee = valeur.Replace(separateur.ToString(), String.Empty)
            End If
        Else
            normalisee = valeur
        End If

        Return Decimal.TryParse(normalisee, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, montant) AndAlso montant >= 0D
    End Function

    Private Shared Function Jetons(valeur As String) As String()
        Return valeur.Split(" "c, StringSplitOptions.RemoveEmptyEntries)
    End Function

    Private Shared Function Normaliser(valeur As String) As String
        Dim normalise As String = If(valeur, String.Empty).Normalize(Text.NormalizationForm.FormD).ToLowerInvariant()
        normalise = Regex.Replace(normalise, "\p{Mn}", String.Empty)
        Return Regex.Replace(normalise, "[^\p{L}\p{Nd}]+", " ").Trim()
    End Function

    Private NotInheritable Class CorrespondanceProduit
        Public Property Produit As Produit
        Public Property Score As Decimal
    End Class
End Class
