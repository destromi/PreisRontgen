
Option Strict On
Option Explicit On

Imports System.Net.Http
Imports System.Text.Json
Imports System.Threading.Tasks

Public Class OpenFoodFactsService

    Private Shared ReadOnly Client As New HttpClient()

    Shared Sub New()
        Client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Pricieux/1.0 (Windows Forms)")
    End Sub

    ''' <summary>
    ''' Recherche un produit par son code-barres.
    ''' </summary>
    Public Async Function RechercherParCodeBarresAsync(
        codeBarres As String
    ) As Task(Of ProduitOFF)

        If String.IsNullOrWhiteSpace(codeBarres) Then
            Throw New ArgumentException(
                "Le code-barres est obligatoire.")
        End If

        Dim url As String =
            "https://world.openfoodfacts.org/api/v2/product/" &
            Uri.EscapeDataString(codeBarres.Trim()) &
            ".json"

        Using reponse As HttpResponseMessage =
            Await Client.GetAsync(url)

            reponse.EnsureSuccessStatusCode()

            Dim contenu As String =
                Await reponse.Content.ReadAsStringAsync()

            Using document As JsonDocument =
                JsonDocument.Parse(contenu)

                Dim racine As JsonElement = document.RootElement

                Dim statut As Integer = 0

                Dim statutJson As JsonElement

                If racine.TryGetProperty("status", statutJson) AndAlso
   statutJson.ValueKind = JsonValueKind.Number Then

                    statut = statutJson.GetInt32()

                End If

                If statut <> 1 Then
                    Return New ProduitOFF With {
                        .CodeBarres = codeBarres.Trim(),
                        .EstTrouve = False
                    }
                End If

                Dim produitJson As JsonElement =
                    racine.GetProperty("product")

                Return New ProduitOFF With {
                    .CodeBarres = LireTexte(
                        produitJson, "code"),
                    .Nom = LireTexte(
                        produitJson, "product_name"),
                    .Marque = LireTexte(
                        produitJson, "brands"),
                    .Categories = LireTexte(
                        produitJson, "categories"),
                    .Quantite = LireTexte(
                        produitJson, "quantity"),
                    .ImageUrl = LireTexte(
                        produitJson, "image_url"),
                    .EstTrouve = True
                }

            End Using

        End Using

    End Function

    Private Shared Function LireTexte(
        element As JsonElement,
        propriete As String
    ) As String

        Dim valeur As JsonElement

        If element.TryGetProperty(propriete, valeur) AndAlso
           valeur.ValueKind = JsonValueKind.String Then

            Return valeur.GetString()

        End If

        Return String.Empty

    End Function

End Class
