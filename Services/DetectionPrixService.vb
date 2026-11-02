Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic

Public NotInheritable Class AlerteDetecteeEventArgs
    Inherits EventArgs

    Public Sub New(alerte As AlertePrix)
        Me.Alerte = alerte
    End Sub

    Public ReadOnly Property Alerte As AlertePrix
End Class

Public NotInheritable Class DetectionPrixService
    Public Event AlerteDetectee As EventHandler(Of AlerteDetecteeEventArgs)

    Public Function Analyser(precedente As ObservationPrix, actuelle As ObservationPrix, seuilHausse As Decimal, seuilReductionQuantite As Decimal, nomMagasin As String, nomProduit As String, marque As String) As List(Of AlertePrix)
        Dim alertes As New List(Of AlertePrix)()
        If precedente Is Nothing OrElse actuelle Is Nothing Then Return alertes
        If precedente.PrixUnitaire <= 0D OrElse precedente.QuantiteBase <= 0D OrElse actuelle.QuantiteBase <= 0D Then Return alertes

        Dim variationPrix As Decimal = (actuelle.PrixUnitaire / precedente.PrixUnitaire - 1D) * 100D
        Dim baisseQuantite As Decimal = (1D - actuelle.QuantiteBase / precedente.QuantiteBase) * 100D
        Dim prixConditionnementNonInferieur As Boolean = actuelle.PrixConditionnement >= precedente.PrixConditionnement

        If baisseQuantite >= seuilReductionQuantite AndAlso prixConditionnementNonInferieur Then
            alertes.Add(CreerAlerte("SHRINKFLATION", variationPrix, precedente, actuelle, nomMagasin, nomProduit, marque))
        ElseIf variationPrix >= seuilHausse Then
            alertes.Add(CreerAlerte("HAUSSE", variationPrix, precedente, actuelle, nomMagasin, nomProduit, marque))
        End If
        Return alertes
    End Function

    Public Sub Publier(alertes As IEnumerable(Of AlertePrix))
        For Each alerte As AlertePrix In alertes
            RaiseEvent AlerteDetectee(Me, New AlerteDetecteeEventArgs(alerte))
        Next
    End Sub

    Private Shared Function CreerAlerte(type As String, valeur As Decimal, precedente As ObservationPrix, actuelle As ObservationPrix, magasin As String, produit As String, marque As String) As AlertePrix
        Return New AlertePrix With {
            .ProduitNom = produit,
            .Marque = marque,
            .MagasinNom = magasin,
            .Type = type,
            .Valeur = Decimal.Round(valeur, 2),
            .PrixPrecedent = precedente.PrixUnitaire,
            .PrixActuel = actuelle.PrixUnitaire,
            .DateAlerte = Date.Now,
            .Lue = False
        }
    End Function
End Class
