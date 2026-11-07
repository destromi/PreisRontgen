' ============================================================================
' FICHIER : Services/DetectionService.vb
' RÔLE    : EF-07 (hausses de prix) et EF-08 (shrinkflation), plus les améliorations :
'           alerte non dupliquée, hausse cumulée, prix inhabituel, analyse par lot.
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

Imports System.Linq            ' Pour Where, OrderBy, Select... sur les listes
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
        ''' Si l'observation a déjà été analysée (Id &gt; 0 et alerte déjà enregistrée), aucun doublon n'est créé.
        ''' Si le lecteur sait aussi lire l'historique (IHistoriqueLecteur), la hausse cumulée est recherchée.
        ''' </summary>
        Public Function Analyser(nouvelle As Observation) As List(Of Alerte)   ' Point d'entrée appelé par la saisie de ticket (T2)
            If nouvelle Is Nothing Then Throw New ArgumentNullException(NameOf(nouvelle))   ' Refuse une observation absente

            Dim precedente As Observation = _lecteur.ObtenirPrecedente(nouvelle)   ' Cherche la dernière observation du même produit/magasin
            If precedente Is Nothing Then            ' Aucune observation antérieure
                Return New List(Of Alerte)()         ' Règle du cahier : aucune alerte ; on rend la liste vide
            End If                                   ' Fin du contrôle

            Dim historique As List(Of Observation) = Nothing   ' Historique de la période (Nothing si le lecteur ne sait pas le lire)
            Dim lecteurHistorique As IHistoriqueLecteur = TryCast(_lecteur, IHistoriqueLecteur)   ' TryCast : rend Nothing si le lecteur n'a pas cette capacité
            If lecteurHistorique IsNot Nothing Then  ' Le lecteur sait lire l'historique
                historique = lecteurHistorique.ListerDepuis(nouvelle, DebutFenetre(nouvelle))   ' Lit les observations des derniers mois
            End If                                   ' Fin du test
            Return AnalyserAvec(nouvelle, precedente, historique)   ' Applique les règles
        End Function                                 ' Fin de Analyser

        ''' <summary>
        ''' Analyse par lot (import CSV) : les observations sont traitées de la plus ancienne à la plus récente.
        ''' L'historique de chaque couple produit/magasin n'est lu en base qu'UNE seule fois ; ensuite
        ''' il est tenu à jour en mémoire. Les observations du lot doivent déjà être enregistrées (Id &gt; 0).
        ''' </summary>
        Public Function AnalyserLot(observations As IEnumerable(Of Observation)) As List(Of Alerte)   ' Version rapide pour beaucoup de lignes
            If observations Is Nothing Then Throw New ArgumentNullException(NameOf(observations))   ' Refuse un lot absent

            Dim resultat As New List(Of Alerte)()    ' Toutes les alertes créées par le lot
            Dim historiques As New Dictionary(Of Tuple(Of Integer, Integer), List(Of Observation))()   ' Mémoire : (produit, magasin) vers observations déjà connues
            Dim lecteurHistorique As IHistoriqueLecteur = TryCast(_lecteur, IHistoriqueLecteur)   ' Capacité facultative du lecteur

            For Each obs As Observation In observations.OrderBy(Function(o) o.DateObs).ThenBy(Function(o) o.Id)   ' Du plus ancien au plus récent
                Dim cle As Tuple(Of Integer, Integer) = Tuple.Create(obs.IdProduit, obs.IdMagasin)   ' Clé du couple produit/magasin
                Dim connues As List(Of Observation) = Nothing   ' Observations déjà connues pour ce couple
                If Not historiques.TryGetValue(cle, connues) Then   ' Premier passage pour ce couple : une seule lecture en base
                    connues = ChargerHistorique(obs, lecteurHistorique)   ' Lit l'historique de départ
                    historiques.Add(cle, connues)        ' Le garde en mémoire
                End If                                   ' Fin du test

                If connues.Count > 0 Then                ' Il existe au moins une observation antérieure
                    Dim debut As Date = DebutFenetre(obs)   ' Début de la période de comparaison
                    Dim recentes As List(Of Observation) = connues.Where(Function(o) o.DateObs >= debut).ToList()   ' Observations de la période
                    resultat.AddRange(AnalyserAvec(obs, connues(connues.Count - 1), recentes))   ' Compare à la dernière observation connue
                End If                                   ' Fin du test
                connues.Add(obs)                         ' L'observation devient la dernière connue
            Next                                         ' Fin de la boucle
            Return resultat                              ' Rend les alertes créées
        End Function                                     ' Fin de AnalyserLot

        ''' <summary>
        ''' Contrôle de saisie : retourne un message d'avertissement si le prix unitaire est très éloigné
        ''' du prix habituel (facteur du paramétrage, 3 par défaut), sinon Nothing.
        ''' À appeler par l'écran de saisie AVANT d'enregistrer ; l'écran affiche le message et demande confirmation.
        ''' </summary>
        Public Function VerifierPrixInhabituel(observation As Observation) As String   ' Rend le message ou Nothing
            If observation Is Nothing Then Throw New ArgumentNullException(NameOf(observation))   ' Refuse une observation absente
            Dim lecteurHistorique As IHistoriqueLecteur = TryCast(_lecteur, IHistoriqueLecteur)   ' Il faut pouvoir lire l'historique
            If lecteurHistorique Is Nothing Then Return Nothing   ' Sinon : pas de contrôle possible

            Dim historique As List(Of Observation) = lecteurHistorique.ListerDepuis(observation, observation.DateObs.AddYears(-1))   ' Un an d'historique
            Dim habituel As Decimal                      ' Prix habituel (médiane), rempli par la méthode ci-dessous
            If Not EstPrixInhabituel(historique, observation.PrixUnitaire, _parametres.FacteurPrixInhabituel, habituel) Then   ' Prix normal
                Return Nothing                           ' Aucun avertissement
            End If                                       ' Fin du test
            Return $"Prix inhabituel : {observation.PrixUnitaire:N2} alors que le prix habituel est d'environ {habituel:N2}. Vérifiez la saisie."   ' Message pour l'écran
        End Function                                     ' Fin de VerifierPrixInhabituel

        ' Applique toutes les règles à une observation, avec l'observation précédente et l'historique déjà lus
        Private Function AnalyserAvec(nouvelle As Observation,
                                      precedente As Observation,
                                      historique As IEnumerable(Of Observation)) As List(Of Alerte)   ' Cœur commun à Analyser et AnalyserLot
            Dim resultat As New List(Of Alerte)()    ' Liste (vide au départ) des alertes créées par cette analyse

            Dim hausse As Alerte = EvaluerHausse(precedente, nouvelle, _parametres.SeuilHausse)   ' Règle EF-07 : hausse entre deux observations consécutives
            If hausse Is Nothing AndAlso historique IsNot Nothing AndAlso _parametres.HausseCumuleeActive Then   ' Hausse cumulée : seulement si elle est activée (hors cahier des charges)
                hausse = EvaluerHausseCumulee(historique, nouvelle, _parametres.SeuilHausseCumulee, _parametres.FenetreCumuleeMois)   ' Plusieurs petites hausses de suite
            End If                                   ' Fin du test
            If hausse IsNot Nothing Then             ' Une alerte de hausse existe
                EnregistrerSiNouvelle(hausse, nouvelle, resultat)   ' On l'enregistre sauf si elle existe déjà
            End If                                   ' Fin du test hausse

            Dim shrinkflation As Alerte = EvaluerShrinkflation(precedente, nouvelle, _parametres.SeuilShrinkflation)   ' Règle EF-08
            If shrinkflation IsNot Nothing Then      ' Une alerte de shrinkflation existe
                EnregistrerSiNouvelle(shrinkflation, nouvelle, resultat)   ' Même traitement
            End If                                   ' Fin du test shrinkflation

            Return resultat                          ' Rend toutes les alertes créées (0, 1 ou 2)
        End Function                                 ' Fin de AnalyserAvec

        ' Lit l'historique de départ d'un couple produit/magasin (une seule fois par lot)
        Private Function ChargerHistorique(obs As Observation, lecteurHistorique As IHistoriqueLecteur) As List(Of Observation)   ' Rend une liste modifiable
            Dim connues As New List(Of Observation)()   ' Résultat (vide au départ)
            If lecteurHistorique IsNot Nothing Then     ' Lecture de toute la période en une requête
                connues.AddRange(lecteurHistorique.ListerDepuis(obs, DebutFenetre(obs)))   ' Observations des derniers mois
            End If                                      ' Fin du test
            If connues.Count = 0 Then                   ' Rien dans la période (ou lecteur simple)
                Dim precedente As Observation = _lecteur.ObtenirPrecedente(obs)   ' On cherche au moins la dernière observation
                If precedente IsNot Nothing Then connues.Add(precedente)          ' Elle sert de point de comparaison
            End If                                      ' Fin du test
            Return connues                              ' Rend l'historique de départ
        End Function                                    ' Fin de ChargerHistorique

        Private Function DebutFenetre(obs As Observation) As Date   ' Début de la période de la hausse cumulée
            Return obs.DateObs.AddMonths(-Math.Max(1, _parametres.FenetreCumuleeMois))   ' Au moins 1 mois en arrière
        End Function                                    ' Fin de DebutFenetre

        Private Sub EnregistrerSiNouvelle(alerte As Alerte, observation As Observation, resultat As List(Of Alerte))   ' Évite les doublons
            If _alertes.ExisteDeja(alerte) Then      ' Même produit, magasin, type, jour et prix : déjà enregistrée
                Return                               ' On ne fait rien (pas de doublon, pas d'événement)
            End If                                   ' Fin du test
            Enregistrer(alerte, resultat)            ' Sinon : enregistre, ajoute au résultat et prévient
        End Sub                                      ' Fin de EnregistrerSiNouvelle

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
                .IdObservation = nouvelle.Id,         ' Observation à l'origine de l'alerte (information de travail)
                .PrixPrecedent = ancienne.PrixUnitaire,   ' Prix unitaire avant la hausse (colonne prix_precedent)
                .PrixActuel = nouvelle.PrixUnitaire,      ' Prix unitaire après la hausse (colonne prix_actuel)
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
                .IdObservation = nouvelle.Id,         ' Observation à l'origine de l'alerte (information de travail)
                .PrixPrecedent = ancienne.PrixUnitaire,   ' Prix unitaire avant la réduction de quantité
                .PrixActuel = nouvelle.PrixUnitaire,      ' Prix unitaire après la réduction de quantité
                .Type = TypeAlerte.Shrinkflation,     ' Type : shrinkflation
                .Valeur = hausseReelle.Value,         ' Valeur = hausse réelle du prix unitaire (exigence EF-08)
                .DateAlerte = nouvelle.DateObs,       ' Date du ticket
                .Lue = False,                         ' Non lue à la création
                .Detail = $"Quantité : {ancienne.QuantiteBase:0.###} vers {nouvelle.QuantiteBase:0.###} (-{baisse:P1}) pour un prix de {ancienne.PrixTotal:N2} vers {nouvelle.PrixTotal:N2} ; hausse réelle du prix unitaire : {hausseReelle.Value:P1}."   ' Phrase explicative complète
            }                                         ' Fin de l'initialisation
        End Function                                  ' Fin de EvaluerShrinkflation

        ''' <summary>
        ''' Hausse cumulée : compare le prix unitaire de la nouvelle observation au plus ancien relevé de la période
        ''' (fenetreMois mois). Détecte les petites hausses répétées (5 fois +2 %) qui ne dépassent jamais le seuil simple.
        ''' Il faut au moins deux relevés dans la période. Retourne Nothing s'il n'y a pas d'alerte.
        ''' </summary>
        Public Shared Function EvaluerHausseCumulee(historique As IEnumerable(Of Observation),
                                                    nouvelle As Observation,
                                                    seuil As Decimal,
                                                    fenetreMois As Integer) As Alerte   ' Fonction « pure » : facile à tester
            If historique Is Nothing OrElse nouvelle Is Nothing Then Return Nothing   ' Rien à comparer
            Dim debut As Date = nouvelle.DateObs.AddMonths(-Math.Max(1, fenetreMois))   ' Début de la période
            Dim candidats As IEnumerable(Of Observation) = historique.Where(Function(o) MemeProduitEtMagasin(o, nouvelle) AndAlso o.DateObs >= debut AndAlso o.DateObs <= nouvelle.DateObs)   ' Même produit, même magasin, dans la période
            Dim periode As List(Of Observation) = candidats.OrderBy(Function(o) o.DateObs).ThenBy(Function(o) o.Id).ToList()   ' Du plus ancien au plus récent
            If periode.Count < 2 Then Return Nothing   ' Un seul relevé : c'est déjà la hausse simple

            Dim depart As Observation = periode(0)     ' Plus ancien relevé de la période
            Dim variation As Decimal? = CalculerVariation(depart.PrixUnitaire, nouvelle.PrixUnitaire)   ' Hausse totale sur la période
            If Not variation.HasValue OrElse variation.Value <= seuil Then Return Nothing   ' Pas calculable ou pas assez haute

            Return New Alerte With {
                .IdProduit = nouvelle.IdProduit,      ' Produit concerné
                .IdMagasin = nouvelle.IdMagasin,      ' Magasin concerné
                .IdObservation = nouvelle.Id,         ' Observation à l'origine de l'alerte
                .PrixPrecedent = depart.PrixUnitaire,     ' Prix unitaire au début de la période
                .PrixActuel = nouvelle.PrixUnitaire,      ' Prix unitaire actuel
                .Type = TypeAlerte.Hausse,            ' Même type que la hausse simple (pas de nouveau type en base)
                .Valeur = variation.Value,            ' Hausse totale sur la période
                .DateAlerte = nouvelle.DateObs,       ' Date du ticket
                .Lue = False,                         ' Non lue à la création
                .Detail = $"Hausse cumulée sur {Math.Max(1, fenetreMois)} mois : prix unitaire {depart.PrixUnitaire:N2} vers {nouvelle.PrixUnitaire:N2} ({variation.Value:P1}), en {periode.Count + 1} relevés."   ' Phrase explicative
            }                                         ' Fin de l'initialisation
        End Function                                  ' Fin de EvaluerHausseCumulee

        ''' <summary>
        ''' Un prix unitaire est « inhabituel » s'il dépasse N fois le prix habituel (médiane de l'historique)
        ''' ou s'il est N fois plus bas. Il faut au moins 3 observations pour juger. Le prix habituel est rendu
        ''' par le paramètre prixHabituel (0 si on ne peut pas juger).
        ''' </summary>
        Public Shared Function EstPrixInhabituel(historique As IEnumerable(Of Observation),
                                                 prixUnitaire As Decimal,
                                                 facteur As Decimal,
                                                 ByRef prixHabituel As Decimal) As Boolean   ' Vrai si le prix est suspect
            prixHabituel = 0D                         ' Valeur neutre
            If historique Is Nothing OrElse facteur <= 1D Then Return False   ' Pas d'historique, ou facteur sans sens
            Dim prix As List(Of Decimal) = historique.Select(Function(o) o.PrixUnitaire).OrderBy(Function(p) p).ToList()   ' Prix triés
            If prix.Count < ParametresDetection.MinObservationsPrixInhabituel Then Return False   ' Trop peu de relevés pour juger

            Dim milieu As Integer = prix.Count \ 2     ' Position centrale (\ = division entière)
            If prix.Count Mod 2 = 1 Then               ' Nombre impair de relevés
                prixHabituel = prix(milieu)            ' Médiane = valeur du milieu
            Else                                       ' Nombre pair
                prixHabituel = (prix(milieu - 1) + prix(milieu)) / 2D   ' Médiane = moyenne des deux valeurs du milieu
            End If                                     ' Fin du calcul
            If prixHabituel <= 0D Then Return False    ' Prix habituel nul : comparaison impossible
            Return prixUnitaire > prixHabituel * facteur OrElse prixUnitaire < prixHabituel / facteur   ' Trop haut ou trop bas
        End Function                                   ' Fin de EstPrixInhabituel

        Private Shared Function MemeProduitEtMagasin(a As Observation, b As Observation) As Boolean   ' Outil interne de contrôle
            Return a IsNot Nothing AndAlso b IsNot Nothing AndAlso
                   a.IdProduit = b.IdProduit AndAlso a.IdMagasin = b.IdMagasin   ' Vrai si les deux existent ET ont même produit ET même magasin (AndAlso s'arrête dès qu'une condition est fausse)
        End Function                                  ' Fin de MemeProduitEtMagasin

    End Class                                         ' Fin de la classe DetectionService

End Namespace                                         ' Fin de l'espace de noms Services
