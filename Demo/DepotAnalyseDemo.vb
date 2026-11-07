' ============================================================================
' FICHIER : Demo/DepotAnalyseDemo.vb
' RÔLE    : faux dépôt d'analyse EN MÉMOIRE, pour essayer les écrans frmHistorique et
'           frmAnalyses sans base MySQL. Il fabrique neuf mois de relevés de prix pour
'           sept produits dans trois magasins, avec des hausses plus ou moins fortes.
' NOTION  : comme DepotDemo, il « implémente » IAnalyseRepository : les écrans ne voient
'           aucune différence avec la vraie classe MySQL (AnalyseRepositoryMySql).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Linq          ' Pour Where, OrderBy, Select...
Imports PreisRontgen.Data    ' Pour IAnalyseRepository
Imports PreisRontgen.Models  ' Pour ProduitRef, PointPrix, LignePrixMagasin, AchatProduit

''' <summary>Dépôt d'analyse en mémoire avec des données d'exemple (démonstration uniquement).</summary>
Public Class DepotAnalyseDemo                       ' Début de la classe
    Implements IAnalyseRepository                   ' S'engage à fournir toutes les lectures du contrat

    ''' <summary>Une observation de prix fabriquée pour la démonstration.</summary>
    Private Class Obs                               ' Classe interne : un relevé
        Public Property IdProduit As Integer        ' Produit
        Public Property IdMagasin As Integer        ' Magasin
        Public Property DateObs As Date             ' Date
        Public Property Prix As Decimal             ' Prix unitaire
    End Class                                       ' Fin de Obs

    Private ReadOnly _produits As New List(Of ProduitRef)()                   ' Produits connus
    Private ReadOnly _magasins As New List(Of KeyValuePair(Of Integer, String))()    ' Magasins connus
    Private ReadOnly _categories As New List(Of KeyValuePair(Of Integer, String))()  ' Catégories connues
    Private ReadOnly _observations As New List(Of Obs)()                      ' Relevés de prix
    Private ReadOnly _achats As New List(Of AchatProduit)()                   ' Achats habituels du foyer

    Public Sub New()                                ' Constructeur : prépare les données d'exemple
        _magasins.Add(New KeyValuePair(Of Integer, String)(1, "Carrefour Market"))   ' Magasin 1
        _magasins.Add(New KeyValuePair(Of Integer, String)(2, "Santa Lucia"))        ' Magasin 2
        _magasins.Add(New KeyValuePair(Of Integer, String)(3, "Marché central"))     ' Magasin 3
        _categories.Add(New KeyValuePair(Of Integer, String)(1, "Épicerie"))         ' Catégorie 1
        _categories.Add(New KeyValuePair(Of Integer, String)(2, "Produits laitiers"))   ' Catégorie 2

        AjouterProduit(1, "Huile végétale Mayor", 1, UniteBase.Litre)    ' Huile 1
        AjouterProduit(2, "Huile Dinor", 1, UniteBase.Litre)             ' Huile moins chère (équivalent)
        AjouterProduit(3, "Lait entier Camlait", 2, UniteBase.Litre)     ' Lait 1
        AjouterProduit(4, "Lait Candia", 2, UniteBase.Litre)             ' Lait moins cher (équivalent)
        AjouterProduit(5, "Sucre cristallisé", 1, UniteBase.Kg)          ' Sucre
        AjouterProduit(6, "Riz parfumé", 1, UniteBase.Kg)                ' Riz
        AjouterProduit(7, "Café moulu", 1, UniteBase.Kg)                 ' Café

        ' produit, magasin, prix de départ (il y a 8 mois), hausse mensuelle
        Serie(1, 1, 1900D, 0.02D) : Serie(1, 2, 1950D, 0.01D) : Serie(1, 3, 1850D, 0.015D)
        Serie(2, 1, 1700D, 0.005D) : Serie(2, 3, 1650D, 0D)
        Serie(3, 1, 1250D, 0.01D) : Serie(3, 2, 1250D, 0.02D)
        Serie(4, 1, 1100D, 0D) : Serie(4, 2, 1150D, 0.005D)
        Serie(5, 1, 800D, 0.01D) : Serie(5, 2, 820D, 0.015D) : Serie(5, 3, 780D, 0.01D)
        Serie(6, 3, 1700D, 0.01D) : Serie(6, 1, 1750D, 0.005D)
        Serie(7, 1, 6400D, 0.01D) : Serie(7, 2, 6500D, 0D)

        _achats.Add(New AchatProduit With {.IdProduit = 1, .NomProduit = "Huile végétale Mayor", .QuantiteBase = 3D, .NombreAchats = 3})   ' 3 L d'huile
        _achats.Add(New AchatProduit With {.IdProduit = 3, .NomProduit = "Lait entier Camlait", .QuantiteBase = 6D, .NombreAchats = 4})    ' 6 L de lait
        _achats.Add(New AchatProduit With {.IdProduit = 5, .NomProduit = "Sucre cristallisé", .QuantiteBase = 4D, .NombreAchats = 3})     ' 4 kg de sucre
        _achats.Add(New AchatProduit With {.IdProduit = 6, .NomProduit = "Riz parfumé", .QuantiteBase = 5D, .NombreAchats = 2})           ' 5 kg de riz
        _achats.Add(New AchatProduit With {.IdProduit = 7, .NomProduit = "Café moulu", .QuantiteBase = 1D, .NombreAchats = 1})           ' 1 kg de café
    End Sub                                         ' Fin du constructeur

    Private Sub AjouterProduit(id As Integer, nom As String, idCategorie As Integer, unite As UniteBase)   ' Ajoute un produit
        _produits.Add(New ProduitRef With {.Id = id, .Nom = nom, .IdCategorie = idCategorie, .UniteBase = unite})   ' Crée la fiche
    End Sub                                         ' Fin de AjouterProduit

    ''' <summary>Fabrique neuf relevés mensuels (il y a 8 mois jusqu'à aujourd'hui) pour un produit dans un magasin.</summary>
    Private Sub Serie(idProduit As Integer, idMagasin As Integer, prixDepart As Decimal, hausseMensuelle As Decimal)   ' Série de relevés
        Dim aujourdhui As Date = Date.Today        ' Date du jour
        For k As Integer = 8 To 0 Step -1          ' De « il y a 8 mois » à « ce mois-ci »
            Dim facteur As Decimal = 1D            ' Facteur de hausse cumulée
            For m As Integer = 1 To 8 - k          ' Une hausse par mois écoulé
                facteur *= (1D + hausseMensuelle)  ' Hausse composée
            Next                                   ' Fin de la boucle
            _observations.Add(New Obs With {
                .IdProduit = idProduit,            ' Produit
                .IdMagasin = idMagasin,            ' Magasin
                .DateObs = aujourdhui.AddMonths(-k).AddDays(-idMagasin * 2),   ' Date (décalée selon le magasin)
                .Prix = Math.Round(prixDepart * facteur, 0)   ' Prix arrondi au franc
            })                                     ' Ajoute le relevé
        Next                                       ' Fin de la boucle
    End Sub                                        ' Fin de Serie

    Private Function NomMagasin(id As Integer) As String   ' Nom d'un magasin
        Return _magasins.First(Function(m) m.Key = id).Value   ' Cherche dans la liste
    End Function                                   ' Fin de NomMagasin

    Public Function ListerProduits() As List(Of ProduitRef) Implements IAnalyseRepository.ListerProduits   ' Produits
        Return _produits.OrderBy(Function(p) p.Nom).ToList()   ' Par ordre alphabétique
    End Function                                   ' Fin de ListerProduits

    Public Function ListerMagasins() As List(Of KeyValuePair(Of Integer, String)) Implements IAnalyseRepository.ListerMagasins   ' Magasins
        Return _magasins.ToList()                  ' Copie de la liste
    End Function                                   ' Fin de ListerMagasins

    Public Function ListerCategories() As List(Of KeyValuePair(Of Integer, String)) Implements IAnalyseRepository.ListerCategories   ' Catégories
        Return _categories.ToList()                ' Copie de la liste
    End Function                                   ' Fin de ListerCategories

    Public Function ListerObservations(idProduit As Integer, idMagasin As Integer?) As List(Of PointPrix) Implements IAnalyseRepository.ListerObservations   ' Historique d'un produit
        Dim produit As ProduitRef = _produits.First(Function(p) p.Id = idProduit)   ' Fiche du produit
        Dim choisies As IEnumerable(Of Obs) = _observations.Where(Function(o) o.IdProduit = idProduit AndAlso (Not idMagasin.HasValue OrElse o.IdMagasin = idMagasin.Value))   ' Relevés du produit (et du magasin)
        Dim recentes As IEnumerable(Of Obs) = choisies.OrderByDescending(Function(o) o.DateObs)   ' Du plus récent au plus ancien
        Return recentes.Select(Function(o) New PointPrix With {.DateObs = o.DateObs, .IdMagasin = o.IdMagasin, .NomMagasin = NomMagasin(o.IdMagasin), .QuantiteBase = 1D, .PrixUnitaire = o.Prix, .UniteBase = produit.UniteBase}).ToList()   ' Convertit en points
    End Function                                   ' Fin de ListerObservations

    Public Function ListerDernieresObservations(idProduit As Integer?, idCategorie As Integer?) As List(Of LignePrixMagasin) Implements IAnalyseRepository.ListerDernieresObservations   ' Dernier prix par produit et magasin
        Dim resultat As New List(Of LignePrixMagasin)()   ' Résultat
        For Each p As ProduitRef In _produits      ' Pour chaque produit
            If idProduit.HasValue AndAlso p.Id <> idProduit.Value Then Continue For   ' Pas le produit demandé
            If idCategorie.HasValue AndAlso p.IdCategorie <> idCategorie.Value Then Continue For   ' Pas la catégorie demandée
            Dim idProd As Integer = p.Id           ' Copie locale pour les expressions ci-dessous
            For Each idMag As Integer In _observations.Where(Function(o) o.IdProduit = idProd).Select(Function(o) o.IdMagasin).Distinct()   ' Pour chaque magasin qui vend ce produit
                Dim idM As Integer = idMag         ' Copie locale
                Dim derniere As Obs = _observations.Where(Function(o) o.IdProduit = idProd AndAlso o.IdMagasin = idM).OrderByDescending(Function(o) o.DateObs).First()   ' Dernier relevé
                resultat.Add(New LignePrixMagasin With {
                    .IdProduit = p.Id, .NomProduit = p.Nom, .IdMagasin = idM, .NomMagasin = NomMagasin(idM),
                    .PrixUnitaire = derniere.Prix, .DateObs = derniere.DateObs, .UniteBase = p.UniteBase})   ' Ajoute la ligne
            Next                                   ' Fin de la boucle des magasins
        Next                                       ' Fin de la boucle des produits
        Return resultat                            ' Rend les lignes
    End Function                                   ' Fin de ListerDernieresObservations

    Public Function ListerAchats(debut As Date, fin As Date) As List(Of AchatProduit) Implements IAnalyseRepository.ListerAchats   ' Achats habituels
        Return _achats.ToList()                    ' Achats habituels du foyer (identiques quelle que soit la période, pour la démonstration)
    End Function                                   ' Fin de ListerAchats

    Public Function ObtenirPrixALaDate(idsProduits As IEnumerable(Of Integer), dateLimite As Date) As Dictionary(Of Integer, Decimal) Implements IAnalyseRepository.ObtenirPrixALaDate   ' Prix à une date
        Dim resultat As New Dictionary(Of Integer, Decimal)()   ' Résultat
        For Each id As Integer In idsProduits      ' Pour chaque produit demandé
            Dim idCopie As Integer = id            ' Copie locale
            Dim dernier As Obs = _observations.Where(Function(o) o.IdProduit = idCopie AndAlso o.DateObs <= dateLimite).OrderByDescending(Function(o) o.DateObs).FirstOrDefault()   ' Dernier relevé à la date ou avant
            If dernier IsNot Nothing Then resultat(id) = dernier.Prix   ' Absent s'il n'existe pas
        Next                                       ' Fin de la boucle
        Return resultat                            ' Rend les prix
    End Function                                   ' Fin de ObtenirPrixALaDate

End Class                                          ' Fin de DepotAnalyseDemo
