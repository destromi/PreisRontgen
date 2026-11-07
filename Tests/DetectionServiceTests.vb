' ============================================================================
' FICHIER : Tests/DetectionServiceTests.vb
' RÔLE    : tests automatiques de EF-07 (hausses) et EF-08 (shrinkflation), SANS
'           base de données : on utilise de faux objets en mémoire (voir la fin du fichier).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

'Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports PreisRontgen.Data        ' Pour IAlerteRepository et IObservationLecteur
Imports PreisRontgen.Models      ' Pour Observation, Alerte, TypeAlerte, Unite...
Imports PreisRontgen.Services
Imports Xunit    ' Pour DetectionService et NormalisationService

''' <summary>
''' Tests de EF-07 (hausses) et EF-08 (shrinkflation), sans base de données.
''' </summary>
Public Class DetectionServiceTests   ' Classe qui regroupe les tests de la détection

    Private Const IdProduit As Integer = 1                      ' Produit utilisé dans tous les tests
    Private Const IdMagasin As Integer = 1                      ' Magasin utilisé par défaut
    Private Shared ReadOnly Jour1 As New Date(2026, 9, 1)       ' Date de la première observation (1er septembre 2026)
    Private Shared ReadOnly Jour2 As New Date(2026, 10, 1)      ' Date de la seconde observation (1er octobre 2026)

    ' Observation de départ : 500 g à 2,40, soit 4,80 par kg
    Private Shared Function Reference() As Observation          ' Rend l'observation de départ commune à plusieurs tests
        Return Creer(2.4D, 500D, Jour1)                         ' 2,40 pour 500 g le 1er septembre
    End Function                                                ' Fin de Reference

    Private Shared Function Creer(prix As Decimal, grammes As Decimal, jour As Date,
                                  Optional idMagasinObs As Integer = IdMagasin) As Observation   ' Fabrique une observation en grammes ; le magasin est facultatif
        Return NormalisationService.CreerObservation(IdProduit, idMagasinObs, jour, prix, grammes, Unite.Gramme)   ' Utilise le vrai service pour calculer prix unitaire et quantité
    End Function                                                ' Fin de Creer

    ' ------------------------------------------------------------ EF-07 : hausses

    ' CT-21 : hausse juste au seuil (5 %) : pas d'alerte, il faut dépasser le seuil
    <Fact>                                                      ' Test simple
    Public Sub Hausse_ExactementAuSeuil_PasDAlerte()            ' Cas limite du seuil
        Dim ancienne As Observation = Creer(4D, 1000D, Jour1)    ' 4,00 par kg
        Dim nouvelle As Observation = Creer(4.2D, 1000D, Jour2)  ' 4,20 par kg : +5 %
        Assert.Null(DetectionService.EvaluerHausse(ancienne, nouvelle, 0.05D))   ' Attendu : Nothing (pas d'alerte)
    End Sub                                                     ' Fin du test

    ' CT-22 : hausse au-dessus du seuil
    <Fact>                                                      ' Test simple
    Public Sub Hausse_AuDessusDuSeuil_Alerte()                  ' Cas normal d'une hausse
        Dim ancienne As Observation = Creer(4D, 1000D, Jour1)    ' 4,00 par kg
        Dim nouvelle As Observation = Creer(4.21D, 1000D, Jour2) ' +5,25 %
        Dim alerte As Alerte = DetectionService.EvaluerHausse(ancienne, nouvelle, 0.05D)   ' Évalue la règle
        Assert.NotNull(alerte)                                  ' Une alerte doit exister
        Assert.Equal(TypeAlerte.Hausse, alerte.Type)            ' De type Hausse
        Assert.Equal(0.0525D, alerte.Valeur)                    ' Valeur = +5,25 %
        Assert.Equal(Jour2, alerte.DateAlerte)                  ' Date = celle de la nouvelle observation
        Assert.False(alerte.Lue)                                ' Non lue à la création
    End Sub                                                     ' Fin du test

    ' CT-23 : baisse de prix : pas d'alerte
    <Fact>                                                      ' Test simple
    Public Sub Hausse_Baisse_PasDAlerte()                       ' Cas d'une baisse
        Assert.Null(DetectionService.EvaluerHausse(Creer(4D, 1000D, Jour1), Creer(3.5D, 1000D, Jour2), 0.05D))   ' 4,00 vers 3,50 : aucune alerte
    End Sub                                                     ' Fin du test

    ' CT-24 : le seuil est paramétrable (10 % : une hausse de 8 % ne déclenche rien)
    <Fact>                                                      ' Test simple
    Public Sub Hausse_SeuilParametrable()                       ' Vérifie l'influence du seuil
        Dim ancienne As Observation = Creer(5D, 1000D, Jour1)    ' 5,00 par kg
        Dim nouvelle As Observation = Creer(5.4D, 1000D, Jour2)  ' +8 %
        Assert.Null(DetectionService.EvaluerHausse(ancienne, nouvelle, 0.1D))      ' Seuil 10 % : pas d'alerte
        Assert.NotNull(DetectionService.EvaluerHausse(ancienne, nouvelle, 0.05D))  ' Seuil 5 % : alerte
    End Sub                                                     ' Fin du test

    ' CT-25 : la hausse se mesure sur le prix unitaire, pas sur le prix affiché
    '         (même prix affiché, conditionnement différent : voir la shrinkflation)
    <Fact>                                                      ' Test simple
    Public Sub Hausse_ComparaisonSurPrixUnitaire()              ' Vérifie qu'on compare bien le prix par kilo
        Dim ancienne As Observation = Creer(2D, 500D, Jour1)   ' 4,00 par kg
        Dim nouvelle As Observation = Creer(4D, 1000D, Jour2)  ' 4,00 par kg : prix affiché doublé, prix unitaire stable
        Assert.Null(DetectionService.EvaluerHausse(ancienne, nouvelle, 0.05D))   ' Aucune alerte
    End Sub                                                     ' Fin du test

    ' CT-26 : magasins différents : pas de comparaison
    <Fact>                                                      ' Test simple
    Public Sub Hausse_MagasinsDifferents_PasDAlerte()           ' Cas de deux magasins distincts
        Dim ancienne As Observation = Creer(4D, 1000D, Jour1)                      ' Magasin 1
        Dim nouvelle As Observation = Creer(6D, 1000D, Jour2, idMagasinObs:=2)     ' Magasin 2 (argument nommé)
        Assert.Null(DetectionService.EvaluerHausse(ancienne, nouvelle, 0.05D))     ' Pas de comparaison, donc pas d'alerte
    End Sub                                                     ' Fin du test

    ' CT-27 : ancien prix unitaire nul : variation non définie, pas d'alerte ni d'erreur
    <Fact>                                                      ' Test simple
    Public Sub Hausse_AncienPrixNul_PasDAlerte()                ' Cas d'un ancien prix à zéro
        Assert.Null(DetectionService.EvaluerHausse(Creer(0D, 1000D, Jour1), Creer(2D, 1000D, Jour2), 0.05D))   ' Division par zéro évitée
    End Sub                                                     ' Fin du test

    ' ------------------------------------------------- EF-08 : shrinkflation

    ' CT-28 : 500 g vers 450 g au même prix : alerte, hausse réelle de 11,11 %
    <Fact>                                                      ' Test simple
    Public Sub Shrinkflation_QuantiteEnBaisseAPrixEgal_Alerte() ' Cas typique de shrinkflation
        Dim nouvelle As Observation = Creer(2.4D, 450D, Jour2)  ' Même prix, 450 g au lieu de 500 g
        Dim alerte As Alerte = DetectionService.EvaluerShrinkflation(Reference(), nouvelle, 0.03D)   ' Évalue la règle
        Assert.NotNull(alerte)                                  ' Une alerte doit exister
        Assert.Equal(TypeAlerte.Shrinkflation, alerte.Type)     ' De type Shrinkflation
        Assert.Equal(0.1111D, alerte.Valeur, 4)                 ' Hausse réelle : +11,11 % (comparé à 4 décimales)
    End Sub                                                     ' Fin du test

    ' CT-29 : baisse exactement égale au seuil (3 %) : alerte (seuil « au moins »)
    <Fact>                                                      ' Test simple
    Public Sub Shrinkflation_BaisseExactementAuSeuil_Alerte()   ' Cas limite du seuil
        Dim nouvelle As Observation = Creer(2.4D, 485D, Jour2)   ' 500 g vers 485 g = -3 %
        Dim alerte As Alerte = DetectionService.EvaluerShrinkflation(Reference(), nouvelle, 0.03D)   ' Évalue la règle
        Assert.NotNull(alerte)                                  ' Une alerte doit exister
        Assert.Equal(0.0309D, alerte.Valeur, 4)                 ' Hausse réelle : environ +3,09 %
    End Sub                                                     ' Fin du test

    ' CT-30 : baisse inférieure au seuil (2 %) : pas d'alerte
    <Fact>                                                      ' Test simple
    Public Sub Shrinkflation_BaisseSousLeSeuil_PasDAlerte()     ' Baisse trop faible
        Assert.Null(DetectionService.EvaluerShrinkflation(Reference(), Creer(2.4D, 490D, Jour2), 0.03D))   ' 500 g vers 490 g = -2 % : rien
    End Sub                                                     ' Fin du test

    ' CT-31 : quantité en baisse et prix en hausse : alerte
    <Fact>                                                      ' Test simple
    Public Sub Shrinkflation_QuantiteEnBaissePrixEnHausse_Alerte()   ' Double mauvaise nouvelle pour le consommateur
        Dim alerte As Alerte = DetectionService.EvaluerShrinkflation(Reference(), Creer(2.5D, 450D, Jour2), 0.03D)   ' 450 g à 2,50
        Assert.NotNull(alerte)                                  ' Une alerte doit exister
        Assert.True(alerte.Valeur > 0.1111D)                    ' La hausse réelle dépasse celle du cas CT-28
    End Sub                                                     ' Fin du test

    ' CT-32 : quantité en baisse mais prix en baisse : ce n'est pas de la shrinkflation
    <Fact>                                                      ' Test simple
    Public Sub Shrinkflation_PrixEnBaisse_PasDAlerte()          ' Le prix a baissé aussi
        Assert.Null(DetectionService.EvaluerShrinkflation(Reference(), Creer(2D, 450D, Jour2), 0.03D))   ' 450 g à 2,00 : pas d'alerte
    End Sub                                                     ' Fin du test

    ' CT-33 : quantité en hausse : pas d'alerte
    <Fact>                                                      ' Test simple
    Public Sub Shrinkflation_QuantiteEnHausse_PasDAlerte()      ' La quantité a augmenté
        Assert.Null(DetectionService.EvaluerShrinkflation(Reference(), Creer(2.4D, 550D, Jour2), 0.03D))   ' 550 g : pas d'alerte
    End Sub                                                     ' Fin du test

    ' ---------------------------------------------------------- service complet

    ' CT-34 : un produit sans observation antérieure ne déclenche aucune alerte
    <Fact>                                                      ' Test simple
    Public Sub Analyser_SansObservationAnterieure_AucuneAlerte() ' Première observation d'un produit
        Dim depot As New FauxDepotAlertes()                     ' Faux dépôt en mémoire
        Dim service As New DetectionService(New FauxLecteur(Nothing), depot, New ParametresDetection())   ' Service avec un lecteur qui ne trouve rien
        Dim resultat As List(Of Alerte) = service.Analyser(Creer(9D, 100D, Jour2))   ' Lance l'analyse
        Assert.Empty(resultat)                                  ' Aucune alerte retournée
        Assert.Empty(depot.Enregistrees)                        ' Et rien d'enregistré
    End Sub                                                     ' Fin du test

    ' CT-35 : 500 g vers 450 g au même prix donne une hausse ET une shrinkflation,
    '         toutes deux enregistrées, avec un événement AlerteDetectee pour chacune
    <Fact>                                                      ' Test simple
    Public Sub Analyser_DeclencheLEvenementPourChaqueAlerte()   ' Vérifie l'événement personnalisé
        Dim depot As New FauxDepotAlertes()                     ' Faux dépôt en mémoire
        Dim service As New DetectionService(New FauxLecteur(Reference()), depot, New ParametresDetection())   ' Le lecteur renvoie l'observation de départ
        Dim recues As New List(Of Alerte)()                     ' Liste des alertes reçues par l'événement
        AddHandler service.AlerteDetectee, Sub(sender As Object, e As AlerteDetecteeEventArgs) recues.Add(e.Alerte)   ' S'abonne à l'événement : chaque alerte annoncée est notée dans la liste

        Dim resultat As List(Of Alerte) = service.Analyser(Creer(2.4D, 450D, Jour2))   ' Lance l'analyse (500 g vers 450 g au même prix)

        Assert.Equal(2, resultat.Count)                         ' Deux alertes retournées
        Assert.Equal(2, depot.Enregistrees.Count)               ' Deux alertes enregistrées
        Assert.Equal(2, recues.Count)                           ' Deux événements reçus
        Assert.Contains(resultat, Function(a) a.Type = TypeAlerte.Hausse)          ' L'une est une hausse
        Assert.Contains(resultat, Function(a) a.Type = TypeAlerte.Shrinkflation)   ' L'autre une shrinkflation
        Assert.All(resultat, Sub(a) Assert.True(a.Id > 0))   ' identifiant attribué à l'enregistrement
    End Sub                                                     ' Fin du test

    ' CT-36 : prix stable : rien n'est enregistré, aucun événement
    <Fact>                                                      ' Test simple
    Public Sub Analyser_PrixStable_AucuneAlerteNiEvenement()    ' Cas d'un prix inchangé
        Dim depot As New FauxDepotAlertes()                     ' Faux dépôt en mémoire
        Dim service As New DetectionService(New FauxLecteur(Reference()), depot, New ParametresDetection())   ' Le lecteur renvoie l'observation de départ
        Dim nbEvenements As Integer = 0                         ' Compteur d'événements reçus
        AddHandler service.AlerteDetectee, Sub(sender As Object, e As AlerteDetecteeEventArgs) nbEvenements += 1   ' S'abonne : +1 à chaque événement

        service.Analyser(Creer(2.4D, 500D, Jour2))              ' Même prix, même quantité

        Assert.Equal(0, nbEvenements)                           ' Aucun événement
        Assert.Empty(depot.Enregistrees)                        ' Aucune alerte enregistrée
    End Sub                                                     ' Fin du test

    ' ------------------------------------------------ améliorations : outils de test

    ' Fabrique une observation de 1 kg (donc prix unitaire = prix) avec un identifiant donné
    Private Shared Function CreerId(id As Integer, prix As Decimal, jour As Date,
                                    Optional idProd As Integer = IdProduit) As Observation   ' Observation « enregistrée » (Id > 0)
        Dim obs As Observation = NormalisationService.CreerObservation(idProd, IdMagasin, jour, prix, 1000D, Unite.Gramme)   ' 1000 g au prix donné
        obs.Id = id                                             ' Simule l'identifiant attribué par la base
        Return obs                                              ' Rend l'observation
    End Function                                                ' Fin de CreerId

    ' Historique de trois relevés : 4,00 (juillet), 4,15 (août), 4,30 (septembre)
    Private Shared Function HistoriqueEnHausseLente() As List(Of Observation)   ' Trois petites hausses de suite
        Return New List(Of Observation) From {
            CreerId(1, 4D, New Date(2026, 7, 1)),               ' 4,00 par kg en juillet
            CreerId(2, 4.15D, New Date(2026, 8, 1)),            ' 4,15 en août (+3,75 %)
            CreerId(3, 4.3D, New Date(2026, 9, 1))              ' 4,30 en septembre (+3,61 %)
        }                                                       ' Aucune hausse isolée ne dépasse 5 %
    End Function                                                ' Fin de HistoriqueEnHausseLente

    ' ------------------------------------------------ hausse cumulée

    ' CT-37 : trois petites hausses de suite (+11,25 % au total) : alerte de hausse cumulée
    <Fact>                                                      ' Test simple
    Public Sub HausseCumulee_PetitesHaussesRepetees_Alerte()    ' Cas typique
        Dim nouvelle As Observation = CreerId(99, 4.45D, New Date(2026, 10, 1))   ' 4,45 : +3,5 % seulement sur la dernière
        Assert.Null(DetectionService.EvaluerHausse(HistoriqueEnHausseLente()(2), nouvelle, 0.05D))   ' La hausse simple ne voit rien
        Dim alerte As Alerte = DetectionService.EvaluerHausseCumulee(HistoriqueEnHausseLente(), nouvelle, 0.1D, 3)   ' La hausse cumulée voit +11,25 %
        Assert.NotNull(alerte)                                  ' Une alerte doit exister
        Assert.Equal(TypeAlerte.Hausse, alerte.Type)            ' Même type que la hausse simple
        Assert.Equal(0.1125D, alerte.Valeur, 4)                 ' (4,45 - 4,00) / 4,00
        Assert.Contains("cumulée", alerte.Detail)               ' Le texte explique qu'elle est cumulée
    End Sub                                                     ' Fin du test

    ' CT-38 : hausse totale sous le seuil : pas d'alerte
    <Fact>                                                      ' Test simple
    Public Sub HausseCumulee_SousLeSeuil_PasDAlerte()           ' Hausse totale de 7,5 %
        Assert.Null(DetectionService.EvaluerHausseCumulee(HistoriqueEnHausseLente(), CreerId(99, 4.3D, New Date(2026, 10, 1)), 0.1D, 3))   ' 4,00 vers 4,30
    End Sub                                                     ' Fin du test

    ' CT-39 : un seul relevé dans la période : c'est la hausse simple, pas de hausse cumulée
    <Fact>                                                      ' Test simple
    Public Sub HausseCumulee_UnSeulReleve_PasDAlerte()          ' Pas assez de relevés
        Dim historique As New List(Of Observation) From {CreerId(1, 4D, New Date(2026, 9, 1))}   ' Un seul relevé
        Assert.Null(DetectionService.EvaluerHausseCumulee(historique, CreerId(99, 6D, New Date(2026, 10, 1)), 0.1D, 3))   ' +50 % mais un seul relevé
    End Sub                                                     ' Fin du test

    ' CT-40 : un relevé plus ancien que la période est ignoré
    <Fact>                                                      ' Test simple
    Public Sub HausseCumulee_ReleveHorsPeriode_Ignore()         ' La période est de 3 mois
        Dim historique As New List(Of Observation) From {
            CreerId(1, 2D, New Date(2026, 1, 1)),               ' Janvier : hors période (trop ancien)
            CreerId(2, 4.3D, New Date(2026, 9, 1))              ' Septembre : seul relevé de la période
        }                                                       ' Fin de la liste
        Assert.Null(DetectionService.EvaluerHausseCumulee(historique, CreerId(99, 4.5D, New Date(2026, 10, 1)), 0.1D, 3))   ' Pas de comparaison avec janvier
    End Sub                                                     ' Fin du test

    ' CT-41 : le service complet enregistre l'alerte de hausse cumulée (lecteur avec historique)
    <Fact>                                                      ' Test simple
    Public Sub Analyser_HausseCumulee_EnregistreLAlerte()       ' Vérifie le branchement dans le service
        Dim depot As New FauxDepotAlertes()                     ' Faux dépôt
        Dim parametres As New ParametresDetection With {.HausseCumuleeActive = True}   ' La hausse cumulée est activée explicitement
        Dim service As New DetectionService(New FauxHistorique(HistoriqueEnHausseLente()), depot, parametres)   ' Lecteur qui connaît l'historique
        Dim resultat As List(Of Alerte) = service.Analyser(CreerId(99, 4.45D, New Date(2026, 10, 1)))   ' Analyse la nouvelle observation
        Assert.Single(resultat)                                 ' Une seule alerte
        Assert.Contains("cumulée", resultat(0).Detail)          ' C'est la hausse cumulée
        Assert.Equal(4D, resultat(0).PrixPrecedent)             ' Prix au début de la période
        Assert.Equal(4.45D, resultat(0).PrixActuel)             ' Prix actuel
        Assert.Single(depot.Enregistrees)                       ' Elle est enregistrée
    End Sub                                                     ' Fin du test

    ' CT-54 : par défaut, la hausse cumulée est désactivée : seule la règle du cahier des charges s'applique
    <Fact>                                                      ' Test simple
    Public Sub Analyser_HausseCumulee_DesactiveeParDefaut()     ' Respect strict du cahier des charges
        Dim depot As New FauxDepotAlertes()                     ' Faux dépôt
        Dim service As New DetectionService(New FauxHistorique(HistoriqueEnHausseLente()), depot, New ParametresDetection())   ' Paramètres par défaut
        Assert.False(New ParametresDetection().HausseCumuleeActive)   ' Désactivée au départ
        Assert.Empty(service.Analyser(CreerId(99, 4.45D, New Date(2026, 10, 1))))   ' +3,5 % seulement par rapport au dernier relevé : pas d'alerte
        Assert.Empty(depot.Enregistrees)                        ' Rien n'est enregistré
    End Sub                                                     ' Fin du test

    ' CT-55 : les prix précédent et actuel sont renseignés (colonnes prix_precedent et prix_actuel du schéma final)
    <Fact>                                                      ' Test simple
    Public Sub Analyser_Hausse_RenseigneLesPrixPrecedentEtActuel()   ' Vérifie les colonnes ajoutées par le schéma final
        Dim depot As New FauxDepotAlertes()                     ' Faux dépôt
        Dim service As New DetectionService(New FauxLecteur(Reference()), depot, New ParametresDetection())   ' Référence : 500 g à 2,40
        Dim resultat As List(Of Alerte) = service.Analyser(Creer(2.4D, 450D, Jour2))   ' 450 g au même prix : hausse + shrinkflation
        Assert.Equal(2, resultat.Count)                         ' Deux alertes
        Assert.All(resultat, Sub(a) Assert.Equal(4.8D, a.PrixPrecedent))   ' Prix précédent : 4,80 par kg
        Assert.All(resultat, Sub(a) Assert.True(a.PrixActuel > a.PrixPrecedent))   ' Le prix unitaire a augmenté
        Assert.All(resultat, Sub(a) Assert.Equal(IdMagasin, a.IdMagasin))          ' Magasin renseigné
    End Sub                                                     ' Fin du test

    ' ------------------------------------------------ alerte non dupliquée

    ' CT-42 : la même observation analysée deux fois ne crée pas deux fois les mêmes alertes
    <Fact>                                                      ' Test simple
    Public Sub Analyser_DeuxFois_PasDeDoublon()                 ' Double clic sur Enregistrer, import refait...
        Dim depot As New FauxDepotAlertes()                     ' Faux dépôt
        Dim service As New DetectionService(New FauxLecteur(Reference()), depot, New ParametresDetection())   ' Lecteur simple
        Dim nouvelle As Observation = Creer(2.4D, 450D, Jour2)  ' 500 g vers 450 g au même prix : hausse + shrinkflation
        nouvelle.Id = 5                                         ' Observation déjà enregistrée
        Dim nbEvenements As Integer = 0                         ' Compteur d'événements
        AddHandler service.AlerteDetectee, Sub(sender As Object, e As AlerteDetecteeEventArgs) nbEvenements += 1   ' +1 par alerte annoncée

        Assert.Equal(2, service.Analyser(nouvelle).Count)       ' Première analyse : 2 alertes
        Assert.Empty(service.Analyser(nouvelle))                ' Seconde analyse : aucune nouvelle alerte
        Assert.Equal(2, depot.Enregistrees.Count)               ' Toujours 2 alertes en tout
        Assert.Equal(2, nbEvenements)                           ' Et 2 événements seulement
    End Sub                                                     ' Fin du test

    ' ------------------------------------------------ prix inhabituel

    ' CT-43 : 41,00 au lieu de 4,10 : prix inhabituel ; 4,50 : normal ; 1,00 : trop bas
    <Fact>                                                      ' Test simple
    Public Sub PrixInhabituel_TropHautOuTropBas_Signale()       ' Cas de la faute de frappe
        Dim historique As New List(Of Observation) From {CreerId(1, 4D, Jour1), CreerId(2, 4.2D, Jour1), CreerId(3, 4.1D, Jour1)}   ' Médiane : 4,10
        Dim habituel As Decimal                                 ' Reçoit le prix habituel
        Assert.True(DetectionService.EstPrixInhabituel(historique, 41D, 3D, habituel))   ' 10 fois trop haut
        Assert.Equal(4.1D, habituel)                            ' Médiane des trois relevés
        Assert.False(DetectionService.EstPrixInhabituel(historique, 4.5D, 3D, habituel))   ' Normal
        Assert.True(DetectionService.EstPrixInhabituel(historique, 1D, 3D, habituel))      ' Plus de 3 fois trop bas
    End Sub                                                     ' Fin du test

    ' CT-44 : moins de trois relevés : on ne juge pas
    <Fact>                                                      ' Test simple
    Public Sub PrixInhabituel_PeuDeReleves_PasDeJugement()      ' Pas assez d'historique
        Dim historique As New List(Of Observation) From {CreerId(1, 4D, Jour1), CreerId(2, 4.2D, Jour1)}   ' Deux relevés seulement
        Dim habituel As Decimal                                 ' Reçoit le prix habituel
        Assert.False(DetectionService.EstPrixInhabituel(historique, 41D, 3D, habituel))   ' Aucun avertissement
    End Sub                                                     ' Fin du test

    ' CT-45 : le service renvoie un message pour l'écran de saisie, ou Nothing
    <Fact>                                                      ' Test simple
    Public Sub VerifierPrixInhabituel_RendUnMessageOuNothing()  ' Utilisé par l'écran de saisie
        Dim base As New List(Of Observation) From {CreerId(1, 4D, Jour1), CreerId(2, 4.2D, Jour1), CreerId(3, 4.1D, Jour1)}   ' Trois relevés
        Dim service As New DetectionService(New FauxHistorique(base), New FauxDepotAlertes(), New ParametresDetection())   ' Lecteur avec historique
        Dim message As String = service.VerifierPrixInhabituel(CreerId(0, 41D, Jour2))   ' Prix suspect
        Assert.NotNull(message)                                 ' Un avertissement est rendu
        Assert.Contains("Prix inhabituel", message)             ' Le message est clair
        Assert.Null(service.VerifierPrixInhabituel(CreerId(0, 4.3D, Jour2)))   ' Prix normal : rien

        Dim simple As New DetectionService(New FauxLecteur(Nothing), New FauxDepotAlertes(), New ParametresDetection())   ' Lecteur sans historique
        Assert.Null(simple.VerifierPrixInhabituel(CreerId(0, 41D, Jour2)))   ' Pas de contrôle possible : Nothing
    End Sub                                                     ' Fin du test

    ' ------------------------------------------------ analyse par lot

    ' CT-46 : un lot de deux observations ne lit l'historique qu'une fois, et détecte la hausse
    <Fact>                                                      ' Test simple
    Public Sub AnalyserLot_LitLHistoriqueUneSeuleFois()         ' Performance de l'import CSV
        Dim lecteur As New FauxHistorique(New List(Of Observation) From {CreerId(1, 4D, New Date(2026, 8, 1))})   ' Un relevé déjà en base
        Dim depot As New FauxDepotAlertes()                     ' Faux dépôt
        Dim service As New DetectionService(lecteur, depot, New ParametresDetection())   ' Service
        Dim lot As New List(Of Observation) From {
            CreerId(12, 4.5D, New Date(2026, 10, 1)),           ' +12,5 % par rapport à septembre (donnée volontairement dans le désordre)
            CreerId(11, 4D, New Date(2026, 9, 1))               ' Stable par rapport à août
        }                                                       ' Fin du lot

        Dim resultat As List(Of Alerte) = service.AnalyserLot(lot)   ' Analyse tout le lot

        Assert.Single(resultat)                                 ' Une seule hausse détectée
        Assert.Equal(TypeAlerte.Hausse, resultat(0).Type)       ' C'est une hausse
        Assert.Equal(12, resultat(0).IdObservation)             ' Elle vient de l'observation du 1er octobre
        Assert.Equal(1, lecteur.NombreLectures)                 ' Une seule lecture en base pour deux observations
    End Sub                                                     ' Fin du test

    ' CT-47 : sans historique en base, le lot compare quand même ses propres observations entre elles
    <Fact>                                                      ' Test simple
    Public Sub AnalyserLot_SansHistorique_ComparerLesLignesEntreElles()   ' Premier import d'un produit
        Dim depot As New FauxDepotAlertes()                     ' Faux dépôt
        Dim service As New DetectionService(New FauxHistorique(New List(Of Observation)()), depot, New ParametresDetection())   ' Base vide
        Dim lot As New List(Of Observation) From {
            CreerId(1, 4D, New Date(2026, 9, 1)),               ' Première observation : aucune alerte
            CreerId(2, 4.5D, New Date(2026, 10, 1))             ' +12,5 % : hausse
        }                                                       ' Fin du lot
        Assert.Single(service.AnalyserLot(lot))                 ' Une alerte, sur la seconde ligne
    End Sub                                                     ' Fin du test

    ' CT-48 : produits différents dans un lot : chaque produit a son propre historique
    <Fact>                                                      ' Test simple
    Public Sub AnalyserLot_ProduitsDifferents_PasDeMelange()    ' Pas de comparaison entre produits
        Dim service As New DetectionService(New FauxHistorique(New List(Of Observation)()), New FauxDepotAlertes(), New ParametresDetection())   ' Base vide
        Dim lot As New List(Of Observation) From {
            CreerId(1, 4D, New Date(2026, 9, 1), 1),            ' Produit 1 à 4,00
            CreerId(2, 9D, New Date(2026, 10, 1), 2)            ' Produit 2 à 9,00 : ne doit PAS être comparé au produit 1
        }                                                       ' Fin du lot
        Assert.Empty(service.AnalyserLot(lot))                  ' Aucune alerte
    End Sub                                                     ' Fin du test

    ' ------------------------------------------------------------ faux objets

    Private NotInheritable Class FauxLecteur                    ' Faux lecteur : renvoie toujours la même « observation précédente »
        Implements IObservationLecteur                          ' Respecte le contrat du vrai lecteur

        Private ReadOnly _precedente As Observation             ' Observation à renvoyer (ou Nothing)

        Public Sub New(precedente As Observation)               ' Constructeur
            _precedente = precedente                            ' Mémorise l'observation à renvoyer
        End Sub                                                 ' Fin du constructeur

        Public Function ObtenirPrecedente(observation As Observation) As Observation Implements IObservationLecteur.ObtenirPrecedente   ' Méthode du contrat
            Return _precedente                                  ' Renvoie toujours l'observation mémorisée
        End Function                                            ' Fin de ObtenirPrecedente
    End Class                                                   ' Fin de FauxLecteur

    Private NotInheritable Class FauxHistorique                 ' Faux lecteur qui connaît un historique et compte ses lectures
        Implements IObservationLecteur, IHistoriqueLecteur      ' Respecte les deux contrats

        Private ReadOnly _donnees As List(Of Observation)       ' Observations « en base »
        Public NombreLectures As Integer                        ' Nombre de lectures demandées (pour tester la performance)

        Public Sub New(donnees As IEnumerable(Of Observation))  ' Constructeur
            _donnees = New List(Of Observation)(donnees)        ' Copie les observations
        End Sub                                                 ' Fin du constructeur

        Public Function ObtenirPrecedente(observation As Observation) As Observation Implements IObservationLecteur.ObtenirPrecedente   ' Dernière observation avant celle-ci
            NombreLectures += 1                                 ' Compte la lecture
            Dim candidats As IEnumerable(Of Observation) = _donnees.Where(Function(o) o.IdProduit = observation.IdProduit AndAlso o.IdMagasin = observation.IdMagasin AndAlso o.DateObs <= observation.DateObs AndAlso o.Id <> observation.Id)   ' Même produit, même magasin, antérieures
            Return candidats.OrderBy(Function(o) o.DateObs).ThenBy(Function(o) o.Id).LastOrDefault()   ' La plus récente (Nothing s'il n'y en a pas)
        End Function                                            ' Fin de ObtenirPrecedente

        Public Function ListerDepuis(observation As Observation, dateLimite As Date) As List(Of Observation) Implements IHistoriqueLecteur.ListerDepuis   ' Historique de la période
            NombreLectures += 1                                 ' Compte la lecture
            Dim candidats As IEnumerable(Of Observation) = _donnees.Where(Function(o) o.IdProduit = observation.IdProduit AndAlso o.IdMagasin = observation.IdMagasin AndAlso o.DateObs >= dateLimite AndAlso (o.DateObs < observation.DateObs OrElse (o.DateObs = observation.DateObs AndAlso o.Id < observation.Id)))   ' De la date limite jusqu'à l'observation (exclue)
            Return candidats.OrderBy(Function(o) o.DateObs).ThenBy(Function(o) o.Id).ToList()   ' Du plus ancien au plus récent
        End Function                                            ' Fin de ListerDepuis
    End Class                                                   ' Fin de FauxHistorique

    Private NotInheritable Class FauxDepotAlertes               ' Faux dépôt : garde les alertes en mémoire au lieu de la base
        Implements IAlerteRepository                            ' Respecte le contrat du vrai dépôt

        Public ReadOnly Enregistrees As New List(Of Alerte)()   ' Alertes « enregistrées » (visibles par les tests)

        Public Function ExisteDeja(alerte As Alerte) As Boolean Implements IAlerteRepository.ExisteDeja   ' Anti-doublon
            Return Enregistrees.Any(Function(a) a.IdProduit = alerte.IdProduit AndAlso a.IdMagasin = alerte.IdMagasin AndAlso a.Type = alerte.Type AndAlso
                                                a.DateAlerte.Date = alerte.DateAlerte.Date AndAlso
                                                Math.Abs(a.PrixPrecedent - alerte.PrixPrecedent) < 0.00001D AndAlso
                                                Math.Abs(a.PrixActuel - alerte.PrixActuel) < 0.00001D)   ' Vrai si la même alerte est déjà enregistrée
        End Function                                            ' Fin de ExisteDeja

        Public Function Ajouter(alerte As Alerte) As Integer Implements IAlerteRepository.Ajouter   ' Simule l'enregistrement
            Enregistrees.Add(alerte)                            ' Garde l'alerte en mémoire
            Return Enregistrees.Count                           ' Renvoie un identifiant fictif (1, 2, 3...)
        End Function                                            ' Fin de Ajouter

        Public Function Rechercher(filtre As FiltreAlerte) As List(Of Alerte) Implements IAlerteRepository.Rechercher   ' Simule la recherche
            Return New List(Of Alerte)(Enregistrees)            ' Renvoie une copie de toutes les alertes
        End Function                                            ' Fin de Rechercher

        Public Sub MarquerCommeLue(idAlerte As Integer) Implements IAlerteRepository.MarquerCommeLue   ' Non utilisé par les tests
        End Sub                                                 ' Ne fait rien

        Public Sub MarquerToutesCommeLues() Implements IAlerteRepository.MarquerToutesCommeLues   ' Non utilisé par les tests
        End Sub                                                 ' Ne fait rien

        Public Function CompterNonLues() As Integer Implements IAlerteRepository.CompterNonLues   ' Simule le comptage
            Return Enregistrees.Count                           ' Toutes les alertes sont considérées non lues
        End Function                                            ' Fin de CompterNonLues

        Public Function ListerMagasins() As List(Of KeyValuePair(Of Integer, String)) Implements IAlerteRepository.ListerMagasins   ' Non utilisé par les tests
            Return New List(Of KeyValuePair(Of Integer, String))()   ' Renvoie une liste vide
        End Function                                            ' Fin de ListerMagasins
    End Class                                                   ' Fin de FauxDepotAlertes

End Class                                                       ' Fin de la classe de tests
