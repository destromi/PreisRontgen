Option Strict On
Option Explicit On

Imports System

Public NotInheritable Class NormalisationUnites
    Private Sub New()
    End Sub

    Public Shared Function ConvertirEnUniteBase(quantite As Decimal, unite As String, uniteBase As UniteProduit) As Decimal
        If quantite <= 0D Then Throw New ArgumentOutOfRangeException(NameOf(quantite), "La quantité doit être supérieure à zéro.")
        Dim code As String = If(unite, String.Empty).Trim().ToLowerInvariant()
        Dim facteur As Decimal
        Select Case uniteBase
            Case UniteProduit.Kilogramme
                Select Case code
                    Case "kg", "kilogramme", "kilogrammes"
                        facteur = 1D
                    Case "g", "gramme", "grammes"
                        facteur = 0.001D
                    Case "mg", "milligramme", "milligrammes"
                        facteur = 0.000001D
                    Case Else
                        Throw New ArgumentException("Utilisez g, kg ou mg pour un produit mesuré en kilogrammes.", NameOf(unite))
                End Select
            Case UniteProduit.Litre
                Select Case code
                    Case "l", "lt", "litre", "litres"
                        facteur = 1D
                    Case "cl", "centilitre", "centilitres"
                        facteur = 0.01D
                    Case "ml", "millilitre", "millilitres"
                        facteur = 0.001D
                    Case Else
                        Throw New ArgumentException("Utilisez ml, cl ou L pour un produit mesuré en litres.", NameOf(unite))
                End Select
            Case UniteProduit.Piece
                Select Case code
                    Case "pièce", "piece", "pièces", "pieces", "unité", "unite", "unités", "unites", "u"
                        facteur = 1D
                    Case Else
                        Throw New ArgumentException("Utilisez pièce pour un produit vendu à l'unité.", NameOf(unite))
                End Select
            Case Else
                Throw New ArgumentOutOfRangeException(NameOf(uniteBase))
        End Select
        Return quantite * facteur
    End Function

    Public Shared Function CalculerPrixUnitaire(prixConditionnement As Decimal, nombreAchete As Decimal, quantiteConditionnement As Decimal, unite As String, uniteBase As UniteProduit) As (QuantiteBase As Decimal, PrixUnitaire As Decimal)
        If prixConditionnement < 0D Then Throw New ArgumentOutOfRangeException(NameOf(prixConditionnement))
        If nombreAchete <= 0D Then Throw New ArgumentOutOfRangeException(NameOf(nombreAchete))
        Dim quantiteBase As Decimal = ConvertirEnUniteBase(quantiteConditionnement, unite, uniteBase)
        Dim prixUnitaire As Decimal = prixConditionnement / quantiteBase
        Return (quantiteBase * nombreAchete, prixUnitaire)
    End Function
End Class
