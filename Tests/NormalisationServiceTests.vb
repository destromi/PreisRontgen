' ============================================================================
' FICHIER : Tests/NormalisationServiceTests.vb
' RÔLE    : tests automatiques de EF-05 (conversion d'unités et prix unitaire).
'           Un test = un petit programme qui appelle une méthode avec des valeurs
'           connues et vérifie le résultat (Assert). Vert = correct, rouge = erreur.
'           Chaque cas CT-xx est à reprendre dans le plan de tests (livrable L5).
' NOTION  : <Fact> = un test simple. <Theory> + <InlineData>/<MemberData> = le même
'           test répété avec plusieurs jeux de valeurs.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports PreisRontgen.Models      ' Pour Unite, UniteBase, Observation
Imports PreisRontgen.Services
Imports Xunit    ' Pour NormalisationService

''' <summary>
''' Tests de EF-05 (conversion des unités et prix unitaire normalisé).
''' Chaque cas est repris dans le plan de tests (L5).
''' </summary>
Public Class NormalisationServiceTests   ' Classe qui regroupe les tests de la normalisation

    ' Quantité saisie, unité, quantité attendue en unité de base
    Public Shared ReadOnly Property CasConversion As IEnumerable(Of Object())   ' Fournit les jeux de valeurs au test théorique ci-dessous
        Get                                                                      ' Début de la lecture de la propriété
            Return New List(Of Object()) From {                                  ' Liste de jeux de valeurs (chaque jeu = un tableau d'objets)
                New Object() {500D, Unite.Gramme, 0.5D},        ' CT-01 : g vers kg
                New Object() {1500D, Unite.Gramme, 1.5D},       ' CT-02 : g vers kg (plus d'un kilo)
                New Object() {2D, Unite.Kilogramme, 2D},        ' CT-03 : kg inchangé
                New Object() {250D, Unite.Millilitre, 0.25D},   ' CT-04 : ml vers L
                New Object() {33D, Unite.Centilitre, 0.33D},    ' CT-05 : cl vers L
                New Object() {1.5D, Unite.Litre, 1.5D},         ' CT-06 : L inchangé
                New Object() {6D, Unite.Piece, 6D}              ' CT-07 : pièce inchangée
            }                                                                    ' Fin de la liste
        End Get                                                                  ' Fin de la lecture
    End Property                                                                 ' Fin de la propriété

    <Theory>                                         ' Test répété pour chaque jeu de valeurs
    <MemberData(NameOf(CasConversion))>              ' Les jeux de valeurs viennent de la propriété CasConversion
    Public Sub ConvertirEnUniteBase_ConvertitCorrectement(quantite As Decimal, uniteSaisie As Unite, attendu As Decimal)   ' Reçoit les trois valeurs d'un jeu
        Assert.Equal(attendu, NormalisationService.ConvertirEnUniteBase(quantite, uniteSaisie))   ' Vérifie que le résultat calculé égale le résultat attendu
    End Sub                                          ' Fin du test

    ' CT-08 : exemple du cahier des charges, 2,40 pour 500 g donne 4,80 par kg
    <Fact>                                           ' Test simple
    Public Sub CalculerPrixUnitaire_ExempleDuCahierDesCharges()   ' Nom explicite : méthode_situation
        Assert.Equal(4.8D, NormalisationService.CalculerPrixUnitaire(2.4D, 500D, Unite.Gramme))   ' 2,40 / 0,5 kg doit donner 4,80
    End Sub                                          ' Fin du test

    ' CT-09 : 1,20 pour 33 cl donne environ 3,6364 par litre
    <Fact>                                           ' Test simple
    Public Sub CalculerPrixUnitaire_ConditionnementEnCentilitres()   ' Cas des centilitres
        Assert.Equal(3.6364D, NormalisationService.CalculerPrixUnitaire(1.2D, 33D, Unite.Centilitre), 4)   ' Compare à 4 décimales (le résultat exact est 3,636363...)
    End Sub                                          ' Fin du test

    ' CT-10 : prix par pièce pour un lot (3 pour 6 pièces donne 0,5)
    <Fact>                                           ' Test simple
    Public Sub CalculerPrixUnitaire_PrixParPiece()   ' Cas des pièces
        Assert.Equal(0.5D, NormalisationService.CalculerPrixUnitaire(3D, 6D, Unite.Piece))   ' 3 / 6 = 0,5 par pièce
    End Sub                                          ' Fin du test

    ' CT-11 : deux conditionnements différents du même produit sont comparables
    <Fact>                                           ' Test simple
    Public Sub CalculerPrixUnitaire_ConditionnementsDifferentsSontComparables()   ' Le but même de la normalisation
        Dim petit As Decimal = NormalisationService.CalculerPrixUnitaire(0.9D, 250D, Unite.Gramme)   ' 3,60 par kg
        Dim grand As Decimal = NormalisationService.CalculerPrixUnitaire(3D, 1D, Unite.Kilogramme)   ' 3,00 par kg
        Assert.Equal(3.6D, petit)                    ' Vérifie le prix du petit conditionnement
        Assert.Equal(3D, grand)                      ' Vérifie le prix du grand conditionnement
        Assert.True(grand < petit)                   ' Le grand conditionnement est moins cher au kilo
    End Sub                                          ' Fin du test

    ' CT-12 : un prix nul est accepté (produit offert), le prix unitaire vaut 0
    <Fact>                                           ' Test simple
    Public Sub CalculerPrixUnitaire_PrixNulAccepte() ' Cas limite : prix nul
        Assert.Equal(0D, NormalisationService.CalculerPrixUnitaire(0D, 500D, Unite.Gramme))   ' 0 / 0,5 = 0
    End Sub                                          ' Fin du test

    ' CT-13 : unité incompatible avec l'unité de base du produit
    <Fact>                                           ' Test simple
    Public Sub CalculerPrixUnitaire_UniteIncompatible_Refuse()   ' Des grammes pour un produit vendu au litre
        Assert.Throws(Of ArgumentException)(
            Function() NormalisationService.CalculerPrixUnitaire(2D, 500D, Unite.Gramme, UniteBase.Litre))   ' Vérifie qu'une ArgumentException est bien levée
    End Sub                                          ' Fin du test

    ' CT-14 : unité compatible avec l'unité de base du produit
    <Fact>                                           ' Test simple
    Public Sub CalculerPrixUnitaire_UniteCompatible_Accepte()   ' Des millilitres pour un produit vendu au litre
        Assert.Equal(4D, NormalisationService.CalculerPrixUnitaire(2D, 500D, Unite.Millilitre, UniteBase.Litre))   ' 2 / 0,5 L = 4 par litre
    End Sub                                          ' Fin du test

    ' CT-15 : quantité nulle ou négative refusée (pas de division par zéro)
    <Theory>                                         ' Test répété pour chaque valeur ci-dessous
    <InlineData(0)>                                  ' Premier jeu : quantité nulle
    <InlineData(-5)>                                 ' Second jeu : quantité négative
    Public Sub ConvertirEnUniteBase_QuantiteNonPositive_Refuse(quantite As Integer)   ' Reçoit la quantité du jeu
        Assert.Throws(Of ArgumentOutOfRangeException)(
            Function() NormalisationService.ConvertirEnUniteBase(quantite, Unite.Gramme))   ' Vérifie qu'une ArgumentOutOfRangeException est levée
    End Sub                                          ' Fin du test

    ' CT-16 : prix négatif refusé
    <Fact>                                           ' Test simple
    Public Sub CalculerPrixUnitaire_PrixNegatif_Refuse()   ' Cas d'erreur : prix négatif
        Assert.Throws(Of ArgumentOutOfRangeException)(
            Function() NormalisationService.CalculerPrixUnitaire(-1D, 500D, Unite.Gramme))   ' Vérifie qu'une exception est levée
    End Sub                                          ' Fin du test

    ' CT-17 : lecture des unités écrites sur un ticket ou dans un CSV
    <Theory>                                         ' Test répété pour chaque texte ci-dessous
    <InlineData("g", Unite.Gramme)>                  ' « g » doit donner Gramme
    <InlineData(" KG ", Unite.Kilogramme)>           ' Espaces et majuscules ignorés
    <InlineData("ml", Unite.Millilitre)>             ' « ml » doit donner Millilitre
    <InlineData("cl", Unite.Centilitre)>             ' « cl » doit donner Centilitre
    <InlineData("L", Unite.Litre)>                   ' « L » doit donner Litre
    <InlineData("pièce", Unite.Piece)>               ' « pièce » doit donner Piece
    Public Sub TryParseUnite_ReconnaitLesUnitesUsuelles(texte As String, attendue As Unite)   ' Reçoit le texte et l'unité attendue
        Dim lue As Unite                             ' Variable qui recevra l'unité lue
        Assert.True(NormalisationService.TryParseUnite(texte, lue))   ' Le texte doit être reconnu
        Assert.Equal(attendue, lue)                  ' Et l'unité lue doit être la bonne
    End Sub                                          ' Fin du test

    ' CT-18 : unité inconnue ou vide
    <Theory>                                         ' Test répété pour chaque texte ci-dessous
    <InlineData("tonne")>                            ' Unité non prévue
    <InlineData("")>                                 ' Texte vide
    <InlineData("   ")>                              ' Texte fait d'espaces
    Public Sub TryParseUnite_TexteInconnu_RetourneFaux(texte As String)   ' Reçoit le texte du jeu
        Dim lue As Unite                             ' Variable qui recevrait l'unité
        Assert.False(NormalisationService.TryParseUnite(texte, lue))   ' Le texte ne doit PAS être reconnu
    End Sub                                          ' Fin du test

    ' CT-19 : construction d'une observation à partir d'une ligne de ticket
    <Fact>                                           ' Test simple
    Public Sub CreerObservation_RemplitTousLesChamps()   ' Vérifie chaque champ de l'observation créée
        Dim obs As Observation = NormalisationService.CreerObservation(7, 3, New Date(2026, 10, 5), 2.4D, 500D, Unite.Gramme)   ' Produit 7, magasin 3, 5 oct. 2026, 2,40 pour 500 g
        Assert.Equal(7, obs.IdProduit)               ' Le produit est bien reporté
        Assert.Equal(3, obs.IdMagasin)               ' Le magasin est bien reporté
        Assert.Equal(New Date(2026, 10, 5), obs.DateObs)   ' La date est bien reportée
        Assert.Equal(2.4D, obs.PrixTotal)            ' Le prix de l'article est bien reporté
        Assert.Equal(0.5D, obs.QuantiteBase)         ' 500 g converti en 0,5 kg
        Assert.Equal(4.8D, obs.PrixUnitaire)         ' Prix unitaire : 4,80 par kg
    End Sub                                          ' Fin du test

    ' CT-20 : arrondi à 4 décimales
    <Fact>                                           ' Test simple
    Public Sub Arrondir_QuatreDecimales()            ' Vérifie la méthode Arrondir
        Assert.Equal(0.1111D, NormalisationService.Arrondir(1D / 9D))   ' 0,11111... devient 0,1111
        Assert.Equal(0.1235D, NormalisationService.Arrondir(0.12345D))  ' 0,12345 devient 0,1235 (le 5 s'arrondit vers le haut)
    End Sub                                          ' Fin du test

End Class                                            ' Fin de la classe de tests
