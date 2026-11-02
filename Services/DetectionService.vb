' ============================================================================
' FICHIER : Services/DetectionService.vb
' RÔLE    : EF-07 (hausses de prix) et EF-08 (shrinkflation).
' PRINCIPE: à chaque nouvelle observation, on la compare à la précédente (même
'           produit, même magasin) et on crée une alerte si une règle est remplie.
'           Après l'enregistrement de chaque alerte, l'événement AlerteDetectee
'           est déclenché pour prévenir l'interface (compteur d'alertes).
' NOTION  : un « événement » (Event) permet à une classe de PRÉVENIR les autres
'           sans les connaître : elle crie (RaiseEvent), ceux qui écoutent (Handles
'           ou AddHandler) réagissent. C'est le cœur de la programmation événementielle.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports PreisRontgen.Data      ' Pour utiliser IAlerteRepository et IObservationLecteur
Imports PreisRontgen.Models    ' Pour utiliser Observation, Alerte, TypeAlerte, ParametresDetection

Namespace Services             ' Espace de noms : PreisRontgen.Services

    ''' <summary>Données de l'événement AlerteDetectee.</summary>
    Public Class AlerteDetecteeEventArgs   ' Paquet d'informations transmis avec l'événement
        Inherits EventArgs                 ' Hérite de EventArgs : convention .NET pour tout paramètre d'événement

        Public ReadOnly Property Alerte As Alerte   ' L'alerte qui vient d'être détectée ; ReadOnly = lecture seule pour les abonnés

        Public Sub New(alerte As Alerte)    ' Constructeur : appelé par « New AlerteDetecteeEventArgs(uneAlerte) »
            Me.Alerte = alerte              ' Range l'alerte reçue dans la propriété (Me = l'objet en cours)
        End Sub                             ' Fin du constructeur

    End Class                               ' Fin de AlerteDetecteeEventArgs

    ''' <summary>
    ''' EF-07 et EF-08 : détection des hausses de prix et de la shrinkflation.
    ''' Chaque nouvelle observation est comparée à l'observation précédente du même produit
    ''' dans le même magasin. Un produit sans observation antérieure ne déclenche aucune alerte.
    ''' L'événement AlerteDetectee est déclenché pour chaque alerte enregistrée ;
    ''' le formulaire principal s'y abonne pour mettre à jour son compteur.
    ''' </summary>
    Public Class DetectionService          ' Début de la classe DetectionService

        ''' <summary>
        ''' Déclenché après l'enregistrement de chaque alerte.
        ''' Attention : il est déclenché sur le thread de l'appelant ; un abonné qui touche à
        ''' l'interface doit passer par Control.Invoke si l'analyse tourne sur un autre thread.
        ''' </summary>
        Public Event AlerteDetectee As EventHandler(Of AlerteDetecteeEventArgs)   ' Déclaration de l'événement personnalisé exigé par le cahier des charges

        Private ReadOnly _lecteur As IObservationLecteur   ' Outil pour retrouver l'observation précédente (fourni de l'extérieur)
        Private ReadOnly _alertes As IAlerteRepository     ' Outil pour enregistrer les alertes (fourni de l'extérieur)
        Private ReadOnly _parametres As ParametresDetection   ' Seuils de détection (5 % et 3 % par défaut)

        Public Sub New(lecteur As IObservationLecteur,
                       alertes As IAlerteRepository,
                       parametres As ParametresDetection)           ' Constructeur : reçoit les trois dépendances
            If lecteur Is Nothing Then Throw New ArgumentNullException(NameOf(lecteur))        ' Refuse un lecteur absent
            If alertes Is Nothing Then Throw New ArgumentNullException(NameOf(alertes))        ' Refuse un dépôt d'alertes absent
            If parametres Is Nothing Then Throw New ArgumentNullException(NameOf(parametres))  ' Refuse des paramètres absents
            _lecteur = lecteur          ' Mémorise le lecteur pour les appels futurs
            _alertes = alertes          ' Mémorise le dépôt d'alertes
            _parametres = parametres    ' Mémorise les seuils
        End Sub                         ' Fin du constructeur

        ''' <summary>
        ''' Analyse une nouvelle observation, enregistre les alertes détectées et les retourne.
        ''' Une même observation peut produire une alerte de hausse et une alerte de shrinkflation.
        ''' </summary>
        Public Function Analyser(nouvelle As Observation) As List(Of Alerte)   ' Point d'entrée appelé par la saisie de ticket (T2)
            If nouvelle Is Nothing Then Throw New ArgumentNullException(NameOf(nouvelle))   ' Refuse une observation absente

            Dim resultat As New List(Of Alerte)()    ' Liste (vide au départ) des alertes créées par cette analyse
            Dim precedente As Observation = _lecteur.ObtenirPrecedente(nouvelle)   ' Cherche la dernière observation du même produit/magasin
            If precedente Is Nothing Then            ' Aucune observation antérieure
                Return resultat                      ' Règle du cahier : aucune alerte ; on rend la liste vide
            End If                                   ' Fin du contrôle

            Dim hausse As Alerte = EvaluerHausse(precedente, nouvelle, _parametres.SeuilHausse)   ' Applique la règle EF-07 (Nothing si pas d'alerte)
            If hausse IsNot Nothing Then             ' Une alerte de hausse existe
                Enregistrer(hausse, resultat)        ' On l'enregistre, l'ajoute au résultat et déclenche l'événement
            End If                                   ' Fin du test hausse

            Dim shrinkflation As Alerte = EvaluerShrinkflation(precedente, nouvelle, _parametres.SeuilShrinkflation)   ' Applique la règle EF-08
            If shrinkflation IsNot Nothing Then      ' Une alerte de shrinkflation existe
                Enregistrer(shrinkflation, resultat) ' Même traitement
            End If                                   ' Fin du test shrinkflation

            Return resultat                          ' Rend toutes les alertes créées (0, 1 ou 2)
        End Function                                 ' Fin de Analyser

        Private Sub Enregistrer(alerte As Alerte, resultat As List(Of Alerte))   ' Méthode interne : enregistre puis prévient
            alerte.Id = _alertes.Ajouter(alerte)     ' Enregistre en base ; la base renvoie l'identifiant attribué
            resultat.Add(alerte)                     ' Ajoute l'alerte à la liste retournée à l'appelant
            RaiseEvent AlerteDetectee(Me, New AlerteDetecteeEventArgs(alerte))   ' Crie « une alerte est détectée » à tous les abonnés
        End Sub                                      ' Fin de Enregistrer

        ''' <summary>
        ''' Variation relative entre deux valeurs : (nouveau - ancien) / ancien.
        ''' Retourne Nothing si l'ancienne valeur est nulle ou négative (variation non définie).
        ''' </summary>
        Public Shared Function CalculerVariation(ancien As Decimal, nouveau As Decimal) As Decimal?   ' « Decimal? » : peut rendre « pas de valeur »
            If ancien <= 0D Then          ' Diviser par 0 est impossible ; une base négative n'a pas de sens
                Return Nothing            ' On rend « pas de valeur » au lieu de planter
            End If                        ' Fin du contrôle
            Return (nouveau - ancien) / ancien   ' Ex. (4,20 - 4,00) / 4,00 = 0,05 soit +5 %
        End Function                      ' Fin de CalculerVariation

        ''' <summary>
        ''' EF-07 : hausse si la variation relative du prix unitaire entre deux observations
        ''' consécutives du même produit dans le même magasin dépasse strictement le seuil.
        ''' Retourne Nothing s'il n'y a pas d'alerte. L'alerte retournée n'est pas encore enregistrée.
        ''' </summary>
        Public Shared Function EvaluerHausse(ancienne As Observation,
                                             nouvelle As Observation,
                                             seuil As Decimal) As Alerte   ' Fonction « pure » : calcule sans toucher à la base (donc facile à tester)
            If Not MemeProduitEtMagasin(ancienne, nouvelle) Then   ' On ne compare que le même produit dans le même magasin
                Return Nothing            ' Sinon : pas d'alerte
            End If                        ' Fin du contrôle

            Dim variation As Decimal? = CalculerVariation(ancienne.PrixUnitaire, nouvelle.PrixUnitaire)   ' Variation du prix PAR KILO/LITRE/PIÈCE
            If Not variation.HasValue OrElse variation.Value <= seuil Then   ' Pas calculable, ou pas assez haute (« dépasse » = strictement)
                Return Nothing            ' Pas d'alerte
            End If                        ' Fin du contrôle

            Return New Alerte With {      ' Crée l'alerte (pas encore enregistrée) et remplit ses champs
                .IdProduit = nouvelle.IdProduit,      ' Produit concerné
                .IdMagasin = nouvelle.IdMagasin,      ' Magasin concerné
                .IdObservation = nouvelle.Id,         ' Observation à l'origine de l'alerte (doit déjà être enregistrée)
                .Type = TypeAlerte.Hausse,            ' Type : hausse de prix
                .Valeur = variation.Value,            ' Valeur de la hausse en fraction (0,0525 = +5,25 %)
                .DateAlerte = nouvelle.DateObs,       ' Date du ticket qui a révélé la hausse
                .Lue = False,                         ' Une nouvelle alerte est « non lue »
                .Detail = $"Prix unitaire : {ancienne.PrixUnitaire:N2} vers {nouvelle.PrixUnitaire:N2} ({variation.Value:P1})."   ' Phrase explicative ; N2 = 2 décimales, P1 = pourcentage à 1 décimale
            }                                         ' Fin de l'initialisation
        End Function                                  ' Fin de EvaluerHausse

        ''' <summary>
        ''' EF-08 : shrinkflation si le conditionnement baisse d'au moins le seuil alors que le prix
        ''' de l'article reste identique ou augmente. La valeur de l'alerte est la hausse réelle
        ''' du prix unitaire. Retourne Nothing s'il n'y a pas d'alerte.
        ''' </summary>
        Public Shared Function EvaluerShrinkflation(ancienne As Observation,
                                                    nouvelle As Observation,
                                                    seuil As Decimal) As Alerte   ' Fonction « pure » comme la précédente
            If Not MemeProduitEtMagasin(ancienne, nouvelle) Then   ' Même produit, même magasin obligatoires
                Return Nothing            ' Sinon : pas d'alerte
            End If                        ' Fin du contrôle

            Dim baisseQuantite As Decimal? = CalculerVariation(ancienne.QuantiteBase, nouvelle.QuantiteBase)   ' Variation de la QUANTITÉ (négative si elle baisse)
            If Not baisseQuantite.HasValue Then   ' Ancienne quantité nulle : calcul impossible
                Return Nothing            ' Pas d'alerte
            End If                        ' Fin du contrôle

            ' La baisse est l'opposée de la variation : -0,10 de variation = 10 % de baisse.
            Dim baisse As Decimal = -baisseQuantite.Value   ' Rend la baisse positive (0,10 pour -10 %)
            If baisse < seuil OrElse nouvelle.PrixTotal < ancienne.PrixTotal Then   ' Baisse trop faible, OU prix de l'article en baisse
                Return Nothing            ' Ce n'est pas de la shrinkflation
            End If                        ' Fin du contrôle (baisse >= seuil, donc « au moins »)

            Dim hausseReelle As Decimal? = CalculerVariation(ancienne.PrixUnitaire, nouvelle.PrixUnitaire)   ' Hausse cachée : variation du prix par kilo/litre
            If Not hausseReelle.HasValue Then   ' Ancien prix unitaire nul : calcul impossible
                Return Nothing            ' Pas d'alerte
            End If                        ' Fin du contrôle

            Return New Alerte With {      ' Crée l'alerte de shrinkflation
                .IdProduit = nouvelle.IdProduit,      ' Produit concerné
                .IdMagasin = nouvelle.IdMagasin,      ' Magasin concerné
                .IdObservation = nouvelle.Id,         ' Observation à l'origine de l'alerte (doit déjà être enregistrée)
                .Type = TypeAlerte.Shrinkflation,     ' Type : shrinkflation
                .Valeur = hausseReelle.Value,         ' Valeur = hausse réelle du prix unitaire (exigence EF-08)
                .DateAlerte = nouvelle.DateObs,       ' Date du ticket
                .Lue = False,                         ' Non lue à la création
                .Detail = $"Quantité : {ancienne.QuantiteBase:0.###} vers {nouvelle.QuantiteBase:0.###} (-{baisse:P1}) pour un prix de {ancienne.PrixTotal:N2} vers {nouvelle.PrixTotal:N2} ; hausse réelle du prix unitaire : {hausseReelle.Value:P1}."   ' Phrase explicative complète
            }                                         ' Fin de l'initialisation
        End Function                                  ' Fin de EvaluerShrinkflation

        Private Shared Function MemeProduitEtMagasin(a As Observation, b As Observation) As Boolean   ' Outil interne de contrôle
            Return a IsNot Nothing AndAlso b IsNot Nothing AndAlso
                   a.IdProduit = b.IdProduit AndAlso a.IdMagasin = b.IdMagasin   ' Vrai si les deux existent ET ont même produit ET même magasin (AndAlso s'arrête dès qu'une condition est fausse)
        End Function                                  ' Fin de MemeProduitEtMagasin

    End Class                                         ' Fin de la classe DetectionService

End Namespace                                         ' Fin de l'espace de noms Services
