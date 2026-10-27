' ============================================================================
' FICHIER : Tests/DetectionServiceTests.vb
' RÔLE    : tests automatiques de EF-07 (hausses) et EF-08 (shrinkflation), SANS
'           base de données : on utilise de faux objets en mémoire (voir la fin du fichier).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports PreisRontgen.Data        ' Pour IAlerteRepository et IObservationLecteur
Imports PreisRontgen.Models      ' Pour Observation, Alerte, TypeAlerte, Unite...
Imports PreisRontgen.Services    ' Pour DetectionService et NormalisationService

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

    Private NotInheritable Class FauxDepotAlertes               ' Faux dépôt : garde les alertes en mémoire au lieu de la base
        Implements IAlerteRepository                            ' Respecte le contrat du vrai dépôt

        Public ReadOnly Enregistrees As New List(Of Alerte)()   ' Alertes « enregistrées » (visibles par les tests)

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
