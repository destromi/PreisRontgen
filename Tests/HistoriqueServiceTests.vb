' ============================================================================
' FICHIER : Tests/HistoriqueServiceTests.vb
' RÔLE    : tests automatiques de EF-06 (historique et données du graphique).
'           Cas CT-56 à CT-64, à reprendre dans le plan de tests (L5).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Diagnostics     ' Pour Stopwatch (chronomètre)
Imports System.Linq            ' Pour Select et ToList
Imports PreisRontgen.Models    ' Pour PointPrix
Imports PreisRontgen.Services  ' Pour HistoriqueService

''' <summary>Tests de HistoriqueService (EF-06).</summary>
Public Class HistoriqueServiceTests                ' Début de la classe

    Private Shared Function P(jour As Integer, prix As Decimal, Optional magasin As String = "Casino") As PointPrix   ' Fabrique un point de test
        Return New PointPrix With {.DateObs = New Date(2026, 1, 1).AddDays(jour), .PrixUnitaire = prix, .NomMagasin = magasin, .QuantiteBase = 1D}   ' Date = 1er janvier + jour
    End Function                                   ' Fin de P

    <Fact>
    Public Sub Trier_PlusRecentEnPremier()         ' CT-56
        Dim t As List(Of PointPrix) = HistoriqueService.Trier({P(1, 10D), P(5, 12D), P(3, 11D)})   ' Désordre volontaire
        Assert.Equal(New Date(2026, 1, 6), t(0).DateObs)   ' Le plus récent d'abord
        Assert.Equal(New Date(2026, 1, 2), t(2).DateObs)   ' Le plus ancien en dernier
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub PreparerCourbe_OrdreChronologique() ' CT-57
        Dim c As List(Of PointPrix) = HistoriqueService.PreparerCourbe({P(5, 12D), P(1, 10D), P(3, 11D)})   ' Désordre volontaire
        Assert.Equal(10D, c(0).PrixUnitaire)       ' Le plus ancien à gauche
        Assert.Equal(12D, c(2).PrixUnitaire)       ' Le plus récent à droite
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub PreparerCourbe_ReduitEtGardePremierEtDernier()   ' CT-58
        Dim beaucoup As New List(Of PointPrix)()   ' 1000 observations
        For i As Integer = 0 To 999                ' Une par jour
            beaucoup.Add(P(i, 100D + i))           ' Prix croissant
        Next                                       ' Fin de la boucle
        Dim c As List(Of PointPrix) = HistoriqueService.PreparerCourbe(beaucoup)   ' Réduction à 400
        Assert.Equal(HistoriqueService.NombreMaxPointsCourbe, c.Count)   ' 400 points
        Assert.Equal(100D, c(0).PrixUnitaire)      ' Premier conservé
        Assert.Equal(1099D, c(c.Count - 1).PrixUnitaire)   ' Dernier conservé
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub PreparerCourbe_PeuDePoints_NeReduitPas()   ' CT-59
        Assert.Equal(3, HistoriqueService.PreparerCourbe({P(1, 1D), P(2, 2D), P(3, 3D)}).Count)   ' Trois points restent trois
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Resumer_CalculeMinMaxMoyenneEtVariation()   ' CT-60
        Dim r As ResumeHistorique = HistoriqueService.Resumer({P(1, 100D), P(2, 150D), P(3, 125D)})   ' Trois prix
        Assert.Equal(3, r.Nombre)                  ' Nombre
        Assert.Equal(100D, r.Minimum)              ' Plus bas
        Assert.Equal(150D, r.Maximum)              ' Plus haut
        Assert.Equal(125D, r.Moyenne)              ' Moyenne
        Assert.Equal(100D, r.Premier)              ' Plus ancien
        Assert.Equal(125D, r.Dernier)              ' Plus récent
        Assert.Equal(0.25D, r.VariationTotale.Value)   ' (125 - 100) / 100
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Resumer_Vide_RendUnResumeVide()     ' CT-61
        Dim r As ResumeHistorique = HistoriqueService.Resumer(New List(Of PointPrix)())   ' Aucune observation
        Assert.Equal(0, r.Nombre)                  ' Rien
        Assert.False(r.VariationTotale.HasValue)   ' Pas de variation
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub PositionsY_PrixIdentiques_AuMilieu()   ' CT-62
        Dim y As List(Of Double) = HistoriqueService.PositionsY(New List(Of Decimal) From {5D, 5D, 5D})   ' Courbe plate
        Assert.All(y, Sub(v) Assert.Equal(0.5R, v))   ' Tout au milieu
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub PositionsXetY_ExtremesEntreZeroEtUn()   ' CT-63
        Dim y As List(Of Double) = HistoriqueService.PositionsY(New List(Of Decimal) From {10D, 20D, 15D})   ' Prix variés
        Assert.Equal(0R, y(0))                     ' Plus bas = 0
        Assert.Equal(1R, y(1))                     ' Plus haut = 1
        Assert.Equal(0.5R, y(2))                   ' Milieu = 0,5
        Dim x As List(Of Double) = HistoriqueService.PositionsX(New List(Of Date) From {New Date(2026, 1, 1), New Date(2026, 1, 11), New Date(2026, 1, 21)})   ' Dates régulières
        Assert.Equal(0R, x(0))                     ' Plus ancienne = 0
        Assert.Equal(0.5R, x(1))                   ' Milieu du temps
        Assert.Equal(1R, x(2))                     ' Plus récente = 1
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Performance_5000Observations_MoinsDUneSeconde()   ' CT-64 : exigence du cahier des charges
        Dim beaucoup As New List(Of PointPrix)()   ' 5000 observations
        For i As Integer = 0 To 4999               ' Une par jour environ
            beaucoup.Add(P(i Mod 1500, 100D + (i Mod 37)))   ' Prix qui varient
        Next                                       ' Fin de la boucle
        Dim chrono As Stopwatch = Stopwatch.StartNew()   ' Démarre le chronomètre
        HistoriqueService.Trier(beaucoup)          ' Tableau
        HistoriqueService.Resumer(beaucoup)        ' Résumé
        Dim courbe As List(Of PointPrix) = HistoriqueService.PreparerCourbe(beaucoup)   ' Graphique
        HistoriqueService.PositionsX(courbe.Select(Function(c) c.DateObs).ToList())     ' Géométrie X
        HistoriqueService.PositionsY(courbe.Select(Function(c) c.PrixUnitaire).ToList())   ' Géométrie Y
        chrono.Stop()                              ' Arrête
        Assert.True(chrono.ElapsedMilliseconds < 1000, "Trop lent : " & chrono.ElapsedMilliseconds.ToString() & " ms")   ' Moins d'une seconde
    End Sub                                        ' Fin du test

End Class                                          ' Fin de HistoriqueServiceTests
