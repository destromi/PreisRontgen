' ============================================================================
' FICHIER : Tests/DepenseServiceTests.vb
' RÔLE    : tests automatiques de EF-14 (dépenses récurrentes et rappels).
'           Cas CT-85 à CT-98, à reprendre dans le plan de tests (L5).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports PreisRontgen.Models    ' Pour DepenseRecurrente, Periodicite
Imports PreisRontgen.Services  ' Pour DepenseService

''' <summary>Tests de DepenseService (EF-14).</summary>
Public Class DepenseServiceTests                   ' Début de la classe

    Private Shared ReadOnly Aujourdhui As New Date(2026, 11, 7)   ' Date de référence des tests

    Private Shared Function D(libelle As String, jours As Integer, Optional actif As Boolean = True) As DepenseRecurrente   ' Dépense dont l'échéance est dans « jours » jours
        Return New DepenseRecurrente With {.Libelle = libelle, .Montant = 1000D, .Periodicite = Periodicite.Mensuelle, .ProchaineEcheance = Aujourdhui.AddDays(jours), .Actif = actif}   ' Dépense de test
    End Function                                   ' Fin de D

    <Fact>
    Public Sub Echeance_Hebdomadaire_Plus7Jours()  ' CT-85
        Assert.Equal(New Date(2026, 11, 14), DepenseService.CalculerEcheanceSuivante(New Date(2026, 11, 7), Periodicite.Hebdomadaire))   ' +7 jours
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Echeance_Mensuelle_PlusUnMois()     ' CT-86
        Assert.Equal(New Date(2026, 12, 7), DepenseService.CalculerEcheanceSuivante(New Date(2026, 11, 7), Periodicite.Mensuelle))   ' +1 mois
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Echeance_Mensuelle_FinDeMois_SeRamene()   ' CT-87
        Assert.Equal(New Date(2027, 2, 28), DepenseService.CalculerEcheanceSuivante(New Date(2027, 1, 31), Periodicite.Mensuelle))   ' 31 janvier -> 28 février
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Echeance_Trimestrielle_PlusTroisMois()   ' CT-88
        Assert.Equal(New Date(2027, 2, 7), DepenseService.CalculerEcheanceSuivante(New Date(2026, 11, 7), Periodicite.Trimestrielle))   ' +3 mois
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Echeance_Annuelle_PlusUnAn_Bissextile()   ' CT-89
        Assert.Equal(New Date(2027, 11, 7), DepenseService.CalculerEcheanceSuivante(New Date(2026, 11, 7), Periodicite.Annuelle))   ' +1 an
        Assert.Equal(New Date(2029, 2, 28), DepenseService.CalculerEcheanceSuivante(New Date(2028, 2, 29), Periodicite.Annuelle))   ' 29 février -> 28 février
    End Sub                                        ' Fin du test

    <Theory>
    <InlineData("", 1000, False)>
    <InlineData("   ", 1000, False)>
    <InlineData("Loyer", 0, False)>
    <InlineData("Loyer", -5, False)>
    <InlineData("Loyer", 1000, True)>
    Public Sub Valider_DonneesSaisies(libelle As String, montant As Integer, valide As Boolean)   ' CT-90
        Dim erreur As String = DepenseService.Valider(libelle, montant, Periodicite.Mensuelle)   ' Contrôle
        Assert.Equal(valide, erreur Is Nothing)    ' Message seulement si invalide
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Valider_NomTropLong_Refuse()        ' CT-91
        Assert.NotNull(DepenseService.Valider(New String("x"c, 181), 100D, Periodicite.Mensuelle))   ' 181 caractères
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub ListerRappels_InclutLes7Jours_ExclutLe8eme()   ' CT-92
        Dim r As List(Of RappelEcheance) = DepenseService.ListerRappels({D("Dans 7 jours", 7), D("Dans 8 jours", 8)}, Aujourdhui, 7)   ' Frontière
        Assert.Single(r)                           ' Une seule
        Assert.Equal("Dans 7 jours", r(0).Depense.Libelle)   ' La bonne
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub ListerRappels_EcheanceDepassee_JoursNegatifsEtEnPremier()   ' CT-93
        Dim r As List(Of RappelEcheance) = DepenseService.ListerRappels({D("Demain", 1), D("En retard", -3)}, Aujourdhui, 7)   ' Une en retard
        Assert.Equal("En retard", r(0).Depense.Libelle)   ' La plus urgente d'abord
        Assert.Equal(-3, r(0).JoursRestants)       ' Négatif = en retard
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub ListerRappels_DepenseInactive_Ignoree()   ' CT-94
        Assert.Empty(DepenseService.ListerRappels({D("Désactivée", 1, False)}, Aujourdhui, 7))   ' Pas de rappel
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub VerifierRappels_DeclencheUnEvenementParRappel()   ' CT-95
        Dim depot As New FauxDepenses()            ' Faux dépôt
        depot.Donnees.AddRange({D("A", 2), D("B", 5), D("C", 30)})   ' Deux proches, une lointaine
        Dim service As New DepenseService(depot)   ' Service
        Dim recus As New List(Of String)()         ' Libellés reçus par l'événement
        AddHandler service.EcheanceProche, Sub(sender, e) recus.Add(e.Rappel.Depense.Libelle)   ' S'abonne
        Dim rappels As List(Of RappelEcheance) = service.VerifierRappels(Aujourdhui)   ' Vérifie
        Assert.Equal(2, rappels.Count)             ' Deux rappels
        Assert.Equal(New List(Of String) From {"A", "B"}, recus)   ' Deux événements, dans l'ordre
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Enregistrer_Nouvelle_AttribueUnIdentifiant()   ' CT-96
        Dim depot As New FauxDepenses()            ' Faux dépôt
        Dim service As New DepenseService(depot)   ' Service
        Dim id As Integer = service.Enregistrer(New DepenseRecurrente With {.Libelle = "  Loyer  ", .Montant = 150000D})   ' Crée
        Assert.True(id > 0)                        ' Identifiant attribué
        Assert.Equal("Loyer", depot.Donnees(0).Libelle)   ' Espaces enlevés
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub Enregistrer_Invalide_LeveUneExceptionClaire()   ' CT-97
        Dim service As New DepenseService(New FauxDepenses())   ' Service
        Dim ex As ArgumentException = Assert.Throws(Of ArgumentException)(Sub() service.Enregistrer(New DepenseRecurrente With {.Libelle = "", .Montant = 10D}))   ' Nom vide
        Assert.Contains("obligatoire", ex.Message)   ' Message lisible
    End Sub                                        ' Fin du test

    <Fact>
    Public Sub DesactiverEtEcheanceSuivante_MettentAJourLeDepot()   ' CT-98
        Dim depot As New FauxDepenses()            ' Faux dépôt
        Dim service As New DepenseService(depot)   ' Service
        Dim a As New DepenseRecurrente With {.Libelle = "Internet", .Montant = 25000D, .Periodicite = Periodicite.Mensuelle, .ProchaineEcheance = New Date(2026, 11, 10)}   ' Dépense
        service.Enregistrer(a)                     ' Crée
        service.PasserALEcheanceSuivante(a)        ' Avance d'un mois
        Assert.Equal(New Date(2026, 12, 10), depot.Donnees(0).ProchaineEcheance)   ' Nouvelle échéance
        service.Desactiver(a.Id)                   ' Désactive
        Assert.Empty(service.Lister())             ' Plus dans la liste
    End Sub                                        ' Fin du test

End Class                                          ' Fin de DepenseServiceTests
