Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Text.Json
Imports System.Threading.Tasks

Public NotInheritable Class ProduitCatalogueExterne
    Public Property Nom As String = String.Empty
    Public Property Marque As String = String.Empty
    Public Property Categories As List(Of String) = New List(Of String)()
    Public Property QuantiteIndiquee As String = String.Empty
End Class

Public NotInheritable Class OpenFoodFactsService
    Private Const UrlProduit As String = "https://world.openfoodfacts.org/api/v3/product/"
    Private Const Champs As String = "product_name,brands,categories,quantity,product_quantity,product_quantity_unit"
    Private Shared ReadOnly ClientPartage As New HttpClient With {.Timeout = TimeSpan.FromSeconds(10)}
    Private ReadOnly _client As HttpClient

    Public Sub New()
        _client = ClientPartage
    End Sub

    Public Sub New(client As HttpClient)
        If client Is Nothing Then Throw New ArgumentNullException(NameOf(client))
        _client = client
    End Sub

    Public Async Function RechercherParCodeBarresAsync(codeBarres As String) As Task(Of ProduitCatalogueExterne)
        Dim code As String = If(codeBarres, String.Empty).Trim()
        If code.Length = 0 OrElse code.Length > 64 OrElse Not code.All(Function(caractere As Char) caractere >= "0"c AndAlso caractere <= "9"c) Then
            Throw New ArgumentException("Le code-barres doit contenir uniquement des chiffres.", NameOf(codeBarres))
        End If

        Dim url As String = UrlProduit & Uri.EscapeDataString(code) & "?fields=" & Uri.EscapeDataString(Champs) & "&lc=fr&tags_lc=fr&cc=cm"
        Using requete As New HttpRequestMessage(HttpMethod.Get, url)
            requete.Headers.UserAgent.ParseAdd("Pricieux/1.0 (Windows desktop; user initiated barcode lookup)")
            requete.Headers.Accept.ParseAdd("application/json")
            Using reponse As HttpResponseMessage = Await _client.SendAsync(requete, HttpCompletionOption.ResponseHeadersRead)
                If reponse.StatusCode = HttpStatusCode.NotFound Then Return Nothing
                reponse.EnsureSuccessStatusCode()
                Using flux = Await reponse.Content.ReadAsStreamAsync()
                    Using document As JsonDocument = Await JsonDocument.ParseAsync(flux)
                        Return LireProduit(document.RootElement)
                    End Using
                End Using
            End Using
        End Using
    End Function

    Private Shared Function LireProduit(racine As JsonElement) As ProduitCatalogueExterne
        Dim statut As JsonElement
        Dim produit As JsonElement
        If Not racine.TryGetProperty("status", statut) Then Return Nothing
        Select Case statut.ValueKind
            Case JsonValueKind.Number
                If statut.GetInt32() <> 1 Then Return Nothing
            Case JsonValueKind.String
                Dim statutTexte As String = If(statut.GetString(), String.Empty)
                If Not statutTexte.StartsWith("success", StringComparison.OrdinalIgnoreCase) Then Return Nothing
            Case Else
                Return Nothing
        End Select
        If Not racine.TryGetProperty("product", produit) OrElse produit.ValueKind <> JsonValueKind.Object Then Return Nothing

        Dim resultat As New ProduitCatalogueExterne With {
            .Nom = LireTexte(produit, "product_name"),
            .Marque = LireTexte(produit, "brands"),
            .Categories = LireTexte(produit, "categories").Split(","c, StringSplitOptions.TrimEntries Or StringSplitOptions.RemoveEmptyEntries).ToList(),
            .QuantiteIndiquee = LireTexte(produit, "quantity")
        }
        If String.IsNullOrWhiteSpace(resultat.QuantiteIndiquee) Then
            Dim quantite As String = LireTexte(produit, "product_quantity")
            Dim unite As String = LireTexte(produit, "product_quantity_unit")
            resultat.QuantiteIndiquee = String.Join(" ", {quantite, unite}.Where(Function(valeur As String) Not String.IsNullOrWhiteSpace(valeur)))
        End If
        Return resultat
    End Function

    Private Shared Function LireTexte(element As JsonElement, propriete As String) As String
        Dim valeur As JsonElement
        If Not element.TryGetProperty(propriete, valeur) OrElse valeur.ValueKind <> JsonValueKind.String Then Return String.Empty
        Return If(valeur.GetString(), String.Empty).Trim()
    End Function
End Class
