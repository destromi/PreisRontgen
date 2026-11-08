' ============================================================================
' FICHIER : Demo/DepotDepenseDemo.vb
' RÔLE    : faux dépôt de dépenses récurrentes EN MÉMOIRE, pour essayer l'écran frmDepenses
'           sans base MySQL. Quelques dépenses d'exemple, dont certaines proches de l'échéance.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Linq          ' Pour Where, OrderBy...
Imports PreisRontgen.Data    ' Pour IDepenseRepository
Imports PreisRontgen.Models  ' Pour DepenseRecurrente, Periodicite

''' <summary>Dépôt de dépenses récurrentes en mémoire (démonstration uniquement).</summary>
Public Class DepotDepenseDemo                       ' Début de la classe
    Implements IDepenseRepository                   ' S'engage à fournir Lister, Ajouter, Modifier, Desactiver

    Private ReadOnly _donnees As New List(Of DepenseRecurrente)()   ' Dépenses « enregistrées » (perdues à la fermeture)
    Private _prochainId As Integer = 1              ' Prochain identifiant à attribuer

    Public Sub New()                                ' Constructeur : données d'exemple
        Dim aujourdhui As Date = Date.Today         ' Date du jour
        Ajouter(Creer("Loyer", 150000D, Periodicite.Mensuelle, aujourdhui.AddDays(3)))              ' Dans 3 jours : rappel
        Ajouter(Creer("Internet", 20000D, Periodicite.Mensuelle, aujourdhui.AddDays(6)))            ' Dans 6 jours : rappel
        Ajouter(Creer("Électricité", 25000D, Periodicite.Mensuelle, aujourdhui.AddDays(12)))        ' Pas encore de rappel
        Ajouter(Creer("Eau", 12000D, Periodicite.Trimestrielle, aujourdhui.AddDays(25)))            ' Trimestrielle
        Ajouter(Creer("Assurance habitation", 180000D, Periodicite.Annuelle, aujourdhui.AddDays(120)))   ' Annuelle
        Ajouter(Creer("Abonnement TV", 8000D, Periodicite.Mensuelle, aujourdhui.AddDays(-1)))       ' Échéance dépassée : « en retard »
    End Sub                                         ' Fin du constructeur

    Private Shared Function Creer(libelle As String, montant As Decimal, periodicite As Periodicite, echeance As Date) As DepenseRecurrente   ' Fabrique une dépense
        Return New DepenseRecurrente With {.Libelle = libelle, .Montant = montant, .Periodicite = periodicite, .ProchaineEcheance = echeance, .Actif = True}   ' Dépense d'exemple
    End Function                                    ' Fin de Creer

    Public Function Lister() As List(Of DepenseRecurrente) Implements IDepenseRepository.Lister   ' Dépenses actives
        Return _donnees.Where(Function(d) d.Actif).OrderBy(Function(d) d.ProchaineEcheance).ThenBy(Function(d) d.Libelle).ToList()   ' Échéance la plus proche d'abord
    End Function                                    ' Fin de Lister

    Public Function Ajouter(depense As DepenseRecurrente) As Integer Implements IDepenseRepository.Ajouter   ' Nouvelle dépense
        depense.Id = _prochainId                    ' Attribue un identifiant
        _prochainId += 1                            ' Prépare le suivant
        _donnees.Add(depense)                       ' Stocke
        Return depense.Id                           ' Rend l'identifiant
    End Function                                    ' Fin de Ajouter

    Public Sub Modifier(depense As DepenseRecurrente) Implements IDepenseRepository.Modifier   ' Met à jour
        Dim index As Integer = _donnees.FindIndex(Function(d) d.Id = depense.Id)   ' Position de la dépense
        If index >= 0 Then _donnees(index) = depense   ' Remplace
    End Sub                                         ' Fin de Modifier

    Public Sub Desactiver(idDepense As Integer) Implements IDepenseRepository.Desactiver   ' Désactive
        Dim d As DepenseRecurrente = _donnees.FirstOrDefault(Function(x) x.Id = idDepense)   ' Dépense visée
        If d IsNot Nothing Then d.Actif = False     ' Passe à inactive
    End Sub                                         ' Fin de Desactiver

End Class                                           ' Fin de DepotDepenseDemo
