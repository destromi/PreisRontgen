
Imports System.Net
Imports System.Net.Http
Imports System.Text.Json
Imports System.Threading.Tasks

Public Class OpenFoodFactsService

    Private Shared ReadOnly client As New HttpClient()

    Public Async Function RechercherProduitAsync(
        codeBarres As String
    ) As Task(Of ProduitOFF)

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim url As String =
            "https://world.openfoodfacts.org/api/v3.6/product/" &
            Uri.EscapeDataString(codeBarres) &
            ".json?fields=code,product_name,brands,quantity"

        If Not client.DefaultRequestHeaders.Contains("User-Agent") Then
            client.DefaultRequestHeaders.Add(
                "User-Agent",
                "Pricieux2.0/1.0 (application de suivi des prix)"
            )
        End If

        Dim reponse As HttpResponseMessage =
            Await client.GetAsync(url)

        reponse.EnsureSuccessStatusCode()

        Dim contenu As String =
            Await reponse.Content.ReadAsStringAsync()

        Using document As JsonDocument = JsonDocument.Parse(contenu)

            Dim racine As JsonElement = document.RootElement
            Dim elementStatut As JsonElement

            If Not racine.TryGetProperty("status", elementStatut) Then
                Return Nothing
            End If

            If elementStatut.GetInt32() <> 1 Then
                Return Nothing
            End If

            Dim elementProduit As JsonElement

            If Not racine.TryGetProperty("product", elementProduit) Then
                Return Nothing
            End If

            Return New ProduitOFF With {
                .CodeBarres = LireChamp(elementProduit, "code"),
                .Nom = LireChamp(elementProduit, "product_name"),
                .Marque = LireChamp(elementProduit, "brands"),
                .Quantite = LireChamp(elementProduit, "quantity")
            }

        End Using

    End Function

    Private Function LireChamp(
        produit As JsonElement,
        nomChamp As String
    ) As String

        Dim valeur As JsonElement

        If produit.TryGetProperty(nomChamp, valeur) Then
            If valeur.ValueKind <> JsonValueKind.Null Then
                Return valeur.ToString()
            End If
        End If

        Return ""

    End Function

End Class

Public Class ProduitOFF

    Public Property CodeBarres As String
    Public Property Nom As String
    Public Property Marque As String
    Public Property Quantite As String

End Class
