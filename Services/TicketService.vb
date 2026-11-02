Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Threading.Tasks

Public NotInheritable Class TicketService
    Private ReadOnly _depot As TicketDepot
    Private ReadOnly _detecteur As DetectionPrixService

    Public Sub New(depot As TicketDepot, detecteur As DetectionPrixService)
        _depot = depot
        _detecteur = detecteur
    End Sub

    Public Function PreparerLigne(produit As Produit, conditionnement As Decimal, unite As String, prix As Decimal, nombre As Decimal) As LigneTicket
        If produit Is Nothing Then Throw New ArgumentNullException(NameOf(produit))
        If conditionnement <= 0D Then Throw New ArgumentOutOfRangeException(NameOf(conditionnement), "Le conditionnement doit être positif.")
        If prix < 0D Then Throw New ArgumentOutOfRangeException(NameOf(prix), "Le prix ne peut pas être négatif.")
        If nombre <= 0D Then Throw New ArgumentOutOfRangeException(NameOf(nombre), "Le nombre acheté doit être positif.")
        Dim conversion = NormalisationUnites.CalculerPrixUnitaire(prix, nombre, conditionnement, unite, produit.UniteBase)
        Return New LigneTicket With {
            .ProduitId = produit.Id,
            .ProduitNom = produit.Nom,
            .Marque = produit.Marque,
            .Conditionnement = conditionnement,
            .Unite = unite.Trim(),
            .Prix = prix,
            .NombreAchete = nombre,
            .QuantiteBaseTotale = conversion.QuantiteBase,
            .PrixUnitaire = conversion.PrixUnitaire
        }
    End Function

    Public Async Function EnregistrerAsync(utilisateur As UtilisateurSession, magasinId As Long, dateAchat As Date, totalSaisi As Decimal, lignes As IList(Of LigneTicket)) As Task(Of List(Of AlertePrix))
        If utilisateur Is Nothing Then Throw New ArgumentNullException(NameOf(utilisateur))
        Dim alertes As List(Of AlertePrix) = Await _depot.EnregistrerAsync(utilisateur.Id, magasinId, dateAchat, totalSaisi, lignes, _detecteur)
        _detecteur.Publier(alertes)
        Return alertes
    End Function
End Class
