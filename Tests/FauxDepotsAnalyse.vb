' ============================================================================
' FICHIER : Tests/FauxDepotsAnalyse.vb
' RÔLE    : faux dépôts EN MÉMOIRE pour tester les services de T4 sans base de données.
'           Un « faux » dépôt respecte le même contrat (interface) que le vrai, mais
'           lit et écrit dans des listes : les tests sont rapides et reproductibles.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Linq            ' Pour Where, OrderBy...
Imports PreisRontgen.Data      ' Pour IAnalyseRepository et IDepenseRepository
Imports PreisRontgen.Models    ' Pour les classes de données T4

''' <summary>Prix connu d'un produit à une date (sert à simuler ObtenirPrixALaDate).</summary>
Friend Class PrixDate                              ' Début de la classe
    Public Property IdProduit As Integer           ' Produit concerné
    Public Property DateObs As Date                ' Date de l'observation
    Public Property Prix As Decimal                ' Prix unitaire observé
End Class                                          ' Fin de PrixDate

''' <summary>Faux dépôt d'analyse : toutes les données sont fournies par le test.</summary>
Friend Class FauxAnalyse                           ' Début de la classe
    Implements IAnalyseRepository                  ' Même contrat que le vrai dépôt MySQL

    Public Property Produits As New List(Of ProduitRef)()                    ' Produits connus
    Public Property Magasins As New List(Of KeyValuePair(Of Integer, String))()    ' Magasins connus
    Public Property Categories As New List(Of KeyValuePair(Of Integer, String))()  ' Catégories connues
    Public Property Observations As New List(Of PointPrix)()                 ' Historique (toujours pour le même produit)
    Public Property Dernieres As New List(Of LignePrixMagasin)()             ' Dernier prix par produit et magasin
    Public Property Achats As New List(Of AchatProduit)()                    ' Achats de la période
    Public Property Prix As New List(Of PrixDate)()                          ' Prix connus à des dates

    Public Function ListerProduits() As List(Of ProduitRef) Implements IAnalyseRepository.ListerProduits   ' Produits
        Return Produits                            ' Liste du test
    End Function                                   ' Fin de ListerProduits

    Public Function ListerMagasins() As List(Of KeyValuePair(Of Integer, String)) Implements IAnalyseRepository.ListerMagasins   ' Magasins
        Return Magasins                            ' Liste du test
    End Function                                   ' Fin de ListerMagasins

    Public Function ListerCategories() As List(Of KeyValuePair(Of Integer, String)) Implements IAnalyseRepository.ListerCategories   ' Catégories
        Return Categories                          ' Liste du test
    End Function                                   ' Fin de ListerCategories

    Public Function ListerObservations(idProduit As Integer, idMagasin As Integer?) As List(Of PointPrix) Implements IAnalyseRepository.ListerObservations   ' Historique
        Return Observations.Where(Function(p) Not idMagasin.HasValue OrElse p.IdMagasin = idMagasin.Value).ToList()   ' Filtre magasin facultatif
    End Function                                   ' Fin de ListerObservations

    Public Function ListerDernieresObservations(idProduit As Integer?, idCategorie As Integer?) As List(Of LignePrixMagasin) Implements IAnalyseRepository.ListerDernieresObservations   ' Derniers prix
        Dim ids As HashSet(Of Integer) = Nothing   ' Produits de la catégorie demandée
        If idCategorie.HasValue Then               ' Restriction par catégorie
            ids = New HashSet(Of Integer)(Produits.Where(Function(p) p.IdCategorie = idCategorie.Value).Select(Function(p) p.Id))   ' Identifiants des produits de la catégorie
        End If                                     ' Fin du test
        Return Dernieres.Where(Function(l) (Not idProduit.HasValue OrElse l.IdProduit = idProduit.Value) AndAlso (ids Is Nothing OrElse ids.Contains(l.IdProduit))).ToList()   ' Applique les restrictions
    End Function                                   ' Fin de ListerDernieresObservations

    Public Function ListerAchats(debut As Date, fin As Date) As List(Of AchatProduit) Implements IAnalyseRepository.ListerAchats   ' Achats
        Return Achats                              ' Liste du test
    End Function                                   ' Fin de ListerAchats

    Public Function ObtenirPrixALaDate(idsProduits As IEnumerable(Of Integer), dateLimite As Date) As Dictionary(Of Integer, Decimal) Implements IAnalyseRepository.ObtenirPrixALaDate   ' Prix à une date
        Dim resultat As New Dictionary(Of Integer, Decimal)()   ' Résultat
        For Each id As Integer In idsProduits      ' Pour chaque produit demandé
            Dim dernier As PrixDate = Prix.Where(Function(p) p.IdProduit = id AndAlso p.DateObs <= dateLimite).OrderByDescending(Function(p) p.DateObs).FirstOrDefault()   ' Dernier prix à la date ou avant
            If dernier IsNot Nothing Then resultat(id) = dernier.Prix   ' Absent si aucune observation
        Next                                       ' Fin de la boucle
        Return resultat                            ' Rend les prix
    End Function                                   ' Fin de ObtenirPrixALaDate
End Class                                          ' Fin de FauxAnalyse

''' <summary>Faux dépôt de dépenses récurrentes.</summary>
Friend Class FauxDepenses                          ' Début de la classe
    Implements IDepenseRepository                  ' Même contrat que le vrai dépôt MySQL

    Public Property Donnees As New List(Of DepenseRecurrente)()   ' Dépenses stockées
    Private _prochainId As Integer = 1             ' Compteur d'identifiants

    Public Function Lister() As List(Of DepenseRecurrente) Implements IDepenseRepository.Lister   ' Dépenses actives
        Return Donnees.Where(Function(d) d.Actif).OrderBy(Function(d) d.ProchaineEcheance).ToList()   ' Actives, échéance la plus proche d'abord
    End Function                                   ' Fin de Lister

    Public Function Ajouter(depense As DepenseRecurrente) As Integer Implements IDepenseRepository.Ajouter   ' Nouvelle dépense
        depense.Id = _prochainId                   ' Attribue un identifiant
        _prochainId += 1                           ' Prépare le suivant
        Donnees.Add(depense)                       ' Stocke
        Return depense.Id                          ' Rend l'identifiant
    End Function                                   ' Fin de Ajouter

    Public Sub Modifier(depense As DepenseRecurrente) Implements IDepenseRepository.Modifier   ' Met à jour
        Dim index As Integer = Donnees.FindIndex(Function(d) d.Id = depense.Id)   ' Position de la dépense
        If index >= 0 Then Donnees(index) = depense   ' Remplace
    End Sub                                        ' Fin de Modifier

    Public Sub Desactiver(idDepense As Integer) Implements IDepenseRepository.Desactiver   ' Désactive
        Dim d As DepenseRecurrente = Donnees.FirstOrDefault(Function(x) x.Id = idDepense)   ' Dépense visée
        If d IsNot Nothing Then d.Actif = False    ' Passe à inactive
    End Sub                                        ' Fin de Desactiver
End Class                                          ' Fin de FauxDepenses
