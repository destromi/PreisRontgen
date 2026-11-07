' ============================================================================
' FICHIER : Tests/ComparaisonServiceTests.vb
' RÔLE    : tests automatiques de EF-11 (comparaison de magasins) et EF-12 (équivalents).
'           Cas CT-78 à CT-84, à reprendre dans le plan de tests (L5).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports PreisRontgen.Models    ' Pour LignePrixMagasin, Equivalent, ProduitRef
Imports PreisRontgen.Services  ' Pour ComparaisonService

''' <summary>Tests de ComparaisonService (EF-11 et EF-12).</summary>
Public Class ComparaisonServiceTests               ' Début de la classe

    Private Shared Function Lg(idProduit As Integer, nom As String, magasin As String, prix As Decimal, Optional unite As UniteBase = UniteBase.Litre) As LignePrixMagasin   ' Fabrique une ligne de test
        Return New LignePrixMagasin With {.IdProduit = idProduit, .NomProduit = nom, .IdMagasin = magasin.Length, .NomMagasin = magasin, .PrixUnitaire = prix, .DateObs = New Date(2026, 5, 1), .UniteBase = unite}   ' Ligne de test
    End Function                                   ' Fin de Lg

    <Fact>
    Public Sub ClasserParPrix_MoinsCherEnPremier() ' CT-78
        Dim l As List(Of LignePrixMagasin) = ComparaisonService.ClasserParPrix({Lg(1, "Lait", "Casino", 1200D), Lg(1, "Lait", "Dovv", 1000D), Lg(1, "Lait", "Mahima", 1100D)})   ' Trois magasins
        Assert.Equal("Dovv", l(0).NomMagasin)      ' Le moins cher
        Assert.Equal("Casino", l(2).NomMagasin)    ' Le plus cher
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub ClasserParPrix_PrixEgal_OrdreParMagasin()   ' CT-79
        Dim l As List(Of LignePrixMagasin) = ComparaisonService.ClasserParPrix({Lg(1, "Lait", "Zoe", 1000D), Lg(1, "Lait", "Alpha", 1000D)})   ' Même prix
        Assert.Equal("Alpha", l(0).NomMagasin)     ' Ordre alphabétique
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub FiltrerEquivalents_GardeSeulementMoinsChersMemeUnite()   ' CT-80
        Dim reference As LignePrixMagasin = Lg(1, "Lait A", "Casino", 1000D)   ' Référence : 1000 / L
        Dim eq As List(Of Equivalent) = ComparaisonService.FiltrerEquivalents(reference, {
            Lg(2, "Lait B", "Dovv", 800D),                    ' Moins cher : gardé
            Lg(3, "Lait C", "Dovv", 1200D),                   ' Plus cher : écarté
            Lg(4, "Lait D", "Dovv", 1000D),                   ' Même prix : écarté
            Lg(5, "Farine", "Dovv", 500D, UniteBase.Kg),      ' Autre unité : écarté
            Lg(1, "Lait A", "Dovv", 700D)                     ' Même produit : écarté
        })                                         ' Candidats
        Assert.Single(eq)                          ' Un seul équivalent
        Assert.Equal("Lait B", eq(0).NomProduit)   ' Le bon
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub FiltrerEquivalents_CalculeLEconomie()   ' CT-81
        Dim eq As List(Of Equivalent) = ComparaisonService.FiltrerEquivalents(Lg(1, "A", "X", 1000D), {Lg(2, "B", "Y", 800D)})   ' 800 au lieu de 1000
        Assert.Equal(0.2D, eq(0).EconomiePct)      ' 20 % moins cher
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub FiltrerEquivalents_TriesDuMoinsCherAuPlusCher()   ' CT-82
        Dim eq As List(Of Equivalent) = ComparaisonService.FiltrerEquivalents(Lg(1, "A", "X", 1000D), {Lg(2, "B", "Y", 900D), Lg(3, "C", "Y", 700D)})   ' Deux candidats
        Assert.Equal("C", eq(0).NomProduit)        ' Le moins cher d'abord
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub TrouverEquivalents_AvecFauxDepot_MemeCategorieSeulement()   ' CT-83
        Dim depot As New FauxAnalyse()             ' Faux dépôt
        depot.Produits.Add(New ProduitRef With {.Id = 1, .Nom = "Lait A", .IdCategorie = 10, .UniteBase = UniteBase.Litre})   ' Produit de départ
        depot.Produits.Add(New ProduitRef With {.Id = 2, .Nom = "Lait B", .IdCategorie = 10, .UniteBase = UniteBase.Litre})   ' Même catégorie
        depot.Produits.Add(New ProduitRef With {.Id = 3, .Nom = "Jus", .IdCategorie = 20, .UniteBase = UniteBase.Litre})      ' Autre catégorie
        depot.Dernieres.Add(Lg(1, "Lait A", "Casino", 1000D))   ' Prix du produit de départ
        depot.Dernieres.Add(Lg(2, "Lait B", "Dovv", 800D))      ' Moins cher, même catégorie
        depot.Dernieres.Add(Lg(3, "Jus", "Dovv", 500D))         ' Moins cher mais autre catégorie
        Dim eq As List(Of Equivalent) = New ComparaisonService(depot).TrouverEquivalents(1)   ' Cherche
        Assert.Single(eq)                          ' Un seul
        Assert.Equal(2, eq(0).IdProduit)           ' Lait B
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub TrouverEquivalents_ProduitInconnuOuSansPrix_ListeVide()   ' CT-84
        Dim depot As New FauxAnalyse()             ' Faux dépôt vide
        Dim service As New ComparaisonService(depot)   ' Service
        Assert.Empty(service.TrouverEquivalents(99))   ' Produit inconnu
        depot.Produits.Add(New ProduitRef With {.Id = 1, .Nom = "Lait", .IdCategorie = 10})   ' Produit connu mais sans observation
        Assert.Empty(service.TrouverEquivalents(1))    ' Pas de référence
    End Sub                                        ' Fin du test

End Class                                          ' Fin de ComparaisonServiceTests
