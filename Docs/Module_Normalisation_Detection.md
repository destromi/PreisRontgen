# Module « Normalisation et détection » (T3) : EF-05, EF-07, EF-08, EF-09

## Fichiers

| Dossier | Fichier | Rôle |
|---|---|---|
| Models | `Unites.vb`, `Observation.vb`, `Alerte.vb`, `ParametresDetection.vb` | Entités et seuils |
| Services | `NormalisationService.vb` | EF-05 : conversion des unités, prix unitaire |
| Services | `DetectionService.vb` | EF-07 et EF-08 : détection, événement `AlerteDetectee` |
| Data | `IAlerteRepository.vb` | Interfaces `IAlerteRepository` et `IObservationLecteur` |
| Data | `AlerteRepositoryMySql.vb`, `ObservationLecteurMySql.vb` | Implémentations MySQL |
| Views | `frmAlertes.vb` (+ Designer) | EF-09 : liste des alertes et filtres |
| Tests | `NormalisationServiceTests.vb`, `DetectionServiceTests.vb` | 36 cas de test (CT-01 à CT-36) |

## Ce que les autres modules doivent faire

**T2 (saisie de ticket)**, pour chaque ligne enregistrée :
1. `Dim obs = NormalisationService.CreerObservation(idProduit, idMagasin, dateAchat, prixLigne, quantiteConditionnement, unite)`
2. Enregistrer `obs` dans `observation_prix`.
3. `detection.Analyser(obs)` : cela enregistre les alertes et déclenche `AlerteDetectee`.

`prixLigne` est le prix **d'un article** (un conditionnement). Le contrôle du total (EF-04) se fait donc avec `somme(prix x nb_achete)`.

**T1 (formulaire principal)** :
- s'abonne à `DetectionService.AlerteDetectee` (ou à `frmAlertes.CompteurChange`) pour mettre à jour le compteur ;
- fournit `ParametresDetection` (seuils de la table `parametre`, 5 % et 3 % par défaut) ;
- fournit la chaîne de connexion (hors du code) et l'identifiant de l'utilisateur connecté à `AlerteRepositoryMySql` et `ObservationLecteurMySql` ;
- appelle `DetectionService.Analyser` (de préférence après l'enregistrement de l'observation).

## Compatibilité avec le schéma final du groupe (database/schema.sql du chef)

La base finale du groupe est celle du fichier `database/schema.sql` (base `pricieux_app`). Le code d'accès aux données est adapté à ce schéma : **aucune colonne à ajouter**.

| Sujet | Choix retenu |
|---|---|
| Table `alerte` | Colonnes utilisées : `id_utilisateur`, `id_produit`, `id_magasin`, `type`, `valeur`, `prix_precedent`, `prix_actuel`, `date_alerte`, `lue`. `DetectionService` renseigne `PrixPrecedent` et `PrixActuel` (prix unitaires). |
| `detail` (texte explicatif) | Non stocké : reconstruit à la lecture à partir du type, de la valeur et des prix précédent / actuel. |
| `id_utilisateur` | Toutes les requêtes sont limitées à l'utilisateur connecté : le constructeur de `AlerteRepositoryMySql` et de `ObservationLecteurMySql` reçoit `(chaineConnexion, idUtilisateur)`. Pour `observation_prix`, l'utilisateur se déduit du ticket (`observation_prix.id_ticket` vers `ticket.id_utilisateur`). |
| Prix de l'article (shrinkflation) | Lu dans `observation_prix.prix_conditionnement`. |
| Anti-doublon | `ExisteDeja(alerte)` : même utilisateur, produit, magasin, type, jour et mêmes prix précédent / actuel (petit écart toléré pour les arrondis). |
| Observation avant l'analyse | Plus obligatoire pour les alertes (pas de colonne `id_observation`). Si l'observation est déjà enregistrée, elle est exclue de la comparaison ; sinon (Id = 0) les observations du même jour comptent. |
| `date_alerte` (TIMESTAMP) | Le filtre « Au » inclut toute la journée de fin. |
| `parametre` | Colonnes `cle` et `valeur` (texte) : `seuil_hausse_pct` (« 5 ») et `seuil_shrinkflation_pct` (« 3 »). `ParametresDetectionMySql` convertit les pourcentages en fractions (0,05 ; 0,03). |
| `alerte.valeur` | Stocke une fraction (0,082 = + 8,2 %), DECIMAL(12,4). |
| Observation précédente | Même utilisateur, même produit, même magasin, strictement antérieure (date puis identifiant). |

## Règles appliquées (cahier des charges §4.2)

- Prix unitaire = prix de l'article / quantité en unité de base (2,40 pour 500 g = 4,80 par kg).
- Hausse : variation du prix unitaire **strictement supérieure** au seuil (5 % par défaut, paramétrable), même produit, même magasin, **observations consécutives**.
- Shrinkflation : baisse du conditionnement **au moins égale** au seuil (3 %) avec un prix de l'article supérieur ou égal ; la valeur de l'alerte est la hausse réelle du prix unitaire.
- Sans observation antérieure : aucune alerte. Les deux alertes peuvent coexister sur une même observation.

## Améliorations ajoutées (validées par le chef de projet)

Aucune ne modifie la structure de la base de données.

| Amélioration | Où | Comment l'utiliser |
|---|---|---|
| Paquets multiples (« 6 x 33 cl ») | `NormalisationService.TryParseConditionnement` | Rend la quantité totale (198) et l'unité (cl) ; ensuite `CreerObservation` fait la conversion (1,98 L). |
| Prix pour 100 g / 100 ml | `NormalisationService.PrixPourCent`, `LibelleEchelleCent` | Pour l'affichage (ex. « 0,48 / 100 g ») ; Nothing pour une pièce. |
| Alerte non dupliquée | `DetectionService.Analyser` + `IAlerteRepository.ExisteDeja` | Un ticket analysé deux fois (double clic, réimport) ne recrée pas les mêmes alertes (même produit, magasin, type, jour et prix). |
| Hausse cumulée | `DetectionService.EvaluerHausseCumulee` | **Facultative et DÉSACTIVÉE par défaut** (absente du cahier des charges : la hausse du cahier compare deux observations consécutives). Pour l'activer : `ParametresDetection.HausseCumuleeActive = True`. Alors : hausse totale > 10 % sur 3 mois, enregistrée comme type HAUSSE avec un texte « Hausse cumulée ». Nécessite un lecteur qui implémente `IHistoriqueLecteur` (c'est le cas de `ObservationLecteurMySql`). |
| Analyse par lot | `DetectionService.AnalyserLot` | Pour l'import CSV : l'historique de chaque produit/magasin est lu une fois, puis tenu à jour en mémoire. Les observations du lot sont triées de la plus ancienne à la plus récente. |
| Prix inhabituel | `DetectionService.VerifierPrixInhabituel` | À appeler par l'écran de saisie avant d'enregistrer ; rend un message (ou Nothing) quand le prix est 3 fois trop haut ou trop bas par rapport à la médiane de l'historique (au moins 3 relevés). L'écran de saisie décide de demander confirmation. |
