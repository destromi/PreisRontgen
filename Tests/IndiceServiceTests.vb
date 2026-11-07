' ============================================================================
' FICHIER : Tests/IndiceServiceTests.vb
' RÔLE    : tests automatiques de EF-10 (indice de prix personnel, Laspeyres base 100).
'           Cas CT-65 à CT-77, à reprendre dans le plan de tests (L5).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports PreisRontgen.Models    ' Pour LignePanier, AchatProduit, ResultatIndice
Imports PreisRontgen.Services  ' Pour IndiceService

''' <summary>Tests de IndiceService (EF-10).</summary>
Public Class IndiceServiceTests                    ' Début de la classe

    Private Shared Function L(id As Integer, quantite As Decimal, depart As Decimal, arrivee As Decimal) As LignePanier   ' Fabrique une ligne de panier
        Return New LignePanier With {.IdProduit = id, .NomProduit = "P" & id.ToString(), .QuantiteBase = quantite, .PrixDepart = depart, .PrixArrivee = arrivee}   ' Ligne de test
    End Function                                   ' Fin de L

    <Fact>
    Public Sub Indice_PrixStables_Vaut100()        ' CT-65
        Dim r As ResultatIndice = IndiceService.CalculerIndice({L(1, 2D, 100D, 100D), L(2, 1D, 500D, 500D)})   ' Rien ne change
        Assert.Equal(100D, r.Indice.Value)         ' Base 100
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Indice_HausseDe10Pourcent_Vaut110() ' CT-66
        Dim r As ResultatIndice = IndiceService.CalculerIndice({L(1, 1D, 100D, 110D)})   ' +10 %
        Assert.Equal(110D, r.Indice.Value)         ' 110
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Indice_Baisse_VautMoinsDe100()      ' CT-67
        Dim r As ResultatIndice = IndiceService.CalculerIndice({L(1, 1D, 100D, 90D)})   ' -10 %
        Assert.Equal(90D, r.Indice.Value)          ' 90
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Indice_QuantitesPonderent_ExempleCalcule()   ' CT-68
        ' A : 2 unités, 100 -> 110 ; B : 1 unité, 200 -> 200
        ' Départ = 2x100 + 1x200 = 400 ; arrivée = 2x110 + 1x200 = 420 ; indice = 100 x 420 / 400 = 105
        Dim r As ResultatIndice = IndiceService.CalculerIndice({L(1, 2D, 100D, 110D), L(2, 1D, 200D, 200D)})   ' Deux produits
        Assert.Equal(400D, r.CoutDepart)           ' Coût au départ
        Assert.Equal(420D, r.CoutArrivee)          ' Coût à l'arrivée
        Assert.Equal(105D, r.Indice.Value)         ' Indice pondéré
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Indice_ArrondiADeuxDecimales()      ' CT-69
        Dim r As ResultatIndice = IndiceService.CalculerIndice({L(1, 1D, 3D, 4D)})   ' 4 / 3 = 133,333...
        Assert.Equal(133.33D, r.Indice.Value)      ' Arrondi
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Indice_AucuneLigne_PasDIndiceEtMessage()   ' CT-70
        Dim r As ResultatIndice = IndiceService.CalculerIndice(New List(Of LignePanier)())   ' Panier vide
        Assert.False(r.Indice.HasValue)            ' Pas d'indice
        Assert.False(String.IsNullOrWhiteSpace(r.Message))   ' Explication fournie
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Indice_PrixDeDepartNul_LigneIgnoree()   ' CT-71
        Dim r As ResultatIndice = IndiceService.CalculerIndice({L(1, 1D, 0D, 50D), L(2, 1D, 100D, 120D)})   ' La première ligne est inutilisable
        Assert.Equal(1, r.NombreProduits)          ' Une seule ligne retenue
        Assert.Equal(120D, r.Indice.Value)         ' Calcul sur la seconde
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Assembler_ProduitSansPrixArrivee_EstEcarte()   ' CT-72
        Dim panier As New List(Of AchatProduit) From {
            New AchatProduit With {.IdProduit = 1, .NomProduit = "A", .QuantiteBase = 1D, .NombreAchats = 1},
            New AchatProduit With {.IdProduit = 2, .NomProduit = "B", .QuantiteBase = 1D, .NombreAchats = 1}
        }                                          ' Deux produits achetés
        Dim depart As New Dictionary(Of Integer, Decimal) From {{1, 100D}, {2, 200D}}   ' Prix au départ
        Dim arrivee As New Dictionary(Of Integer, Decimal) From {{1, 110D}}             ' Produit 2 sans prix à l'arrivée
        Dim lignes As List(Of LignePanier) = IndiceService.Assembler(panier, depart, arrivee)   ' Assemble
        Assert.Single(lignes)                      ' Une seule ligne
        Assert.Equal(1, lignes(0).IdProduit)       ' C'est le produit 1
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub ChoisirPanier_GardeLesPlusAchetes()   ' CT-73
        Dim achats As New List(Of AchatProduit)()  ' 30 produits
        For i As Integer = 1 To 30                 ' Du moins au plus acheté
            achats.Add(New AchatProduit With {.IdProduit = i, .NomProduit = "P" & i.ToString("00"), .QuantiteBase = 1D, .NombreAchats = i})   ' Produit i acheté i fois
        Next                                       ' Fin de la boucle
        Dim panier As List(Of AchatProduit) = IndiceService.ChoisirPanier(achats, 20)   ' Garde 20 produits
        Assert.Equal(20, panier.Count)             ' Limite respectée
        Assert.Equal(30, panier(0).IdProduit)      ' Le plus acheté en premier
        Assert.DoesNotContain(panier, Function(a) a.IdProduit <= 10)   ' Les 10 moins achetés sont écartés
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Calculer_DatesInversees_Message()   ' CT-74
        Dim service As New IndiceService(New FauxAnalyse())   ' Dépôt vide
        Dim r As ResultatIndice = service.Calculer(New Date(2026, 5, 1), New Date(2026, 4, 1))   ' Arrivée avant départ
        Assert.False(r.Indice.HasValue)            ' Pas d'indice
        Assert.Contains("postérieure", r.Message)  ' Message adapté
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Calculer_SansAchat_Message()        ' CT-75
        Dim service As New IndiceService(New FauxAnalyse())   ' Aucun achat
        Dim r As ResultatIndice = service.Calculer(New Date(2026, 4, 1), New Date(2026, 5, 1))   ' Dates correctes
        Assert.False(r.Indice.HasValue)            ' Pas d'indice
        Assert.Contains("Aucun achat", r.Message)  ' Message adapté
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Calculer_DeBoutEnBout_AvecFauxDepot()   ' CT-76
        Dim depot As New FauxAnalyse()             ' Faux dépôt
        depot.Achats.Add(New AchatProduit With {.IdProduit = 1, .NomProduit = "Lait", .QuantiteBase = 4D, .NombreAchats = 3})     ' 4 L de lait
        depot.Achats.Add(New AchatProduit With {.IdProduit = 2, .NomProduit = "Riz", .QuantiteBase = 2D, .NombreAchats = 2})      ' 2 kg de riz
        depot.Prix.Add(New PrixDate With {.IdProduit = 1, .DateObs = New Date(2026, 3, 20), .Prix = 1000D})   ' Lait au départ
        depot.Prix.Add(New PrixDate With {.IdProduit = 1, .DateObs = New Date(2026, 5, 2), .Prix = 1100D})    ' Lait à l'arrivée
        depot.Prix.Add(New PrixDate With {.IdProduit = 2, .DateObs = New Date(2026, 3, 25), .Prix = 600D})    ' Riz au départ
        depot.Prix.Add(New PrixDate With {.IdProduit = 2, .DateObs = New Date(2026, 4, 20), .Prix = 600D})    ' Riz à l'arrivée (inchangé)
        Dim r As ResultatIndice = New IndiceService(depot).Calculer(New Date(2026, 4, 1), New Date(2026, 5, 15))   ' Calcul
        ' Départ = 4x1000 + 2x600 = 5200 ; arrivée = 4x1100 + 2x600 = 5600 ; indice = 107,69
        Assert.Equal(5200D, r.CoutDepart)          ' Coût au départ
        Assert.Equal(5600D, r.CoutArrivee)         ' Coût à l'arrivée
        Assert.Equal(107.69D, r.Indice.Value)      ' Indice
        Assert.Equal(2, r.NombreProduits)          ' Deux produits
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Calculer_AucunPrixAuxDeuxDates_Message()   ' CT-77
        Dim depot As New FauxAnalyse()             ' Faux dépôt
        depot.Achats.Add(New AchatProduit With {.IdProduit = 1, .NomProduit = "Lait", .QuantiteBase = 1D, .NombreAchats = 1})   ' Un achat
        Dim r As ResultatIndice = New IndiceService(depot).Calculer(New Date(2026, 4, 1), New Date(2026, 5, 1))   ' Mais aucun prix connu
        Assert.False(r.Indice.HasValue)            ' Pas d'indice
        Assert.False(String.IsNullOrWhiteSpace(r.Message))   ' Explication fournie
    End Sub                                        ' Fin du test

End Class                                          ' Fin de IndiceServiceTests
