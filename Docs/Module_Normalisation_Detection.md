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
- enregistre l'observation AVANT d'appeler `DetectionService.Analyser`.

## Compatibilité avec le script SQL du groupe (T2)

Le code a été adapté au script SQL fourni par T2 : **aucune colonne à ajouter**.

| Sujet | Choix retenu |
|---|---|
| `prix_total` (prix de l'article, utile à la shrinkflation) | Non stocké : lu dans `ligne_ticket.prix` via `observation_prix.id_ligne_ticket`, sinon `prix_unitaire x quantite_base` (observation importée). |
| `id_magasin` de l'alerte | Déduit de l'observation (`alerte.id_observation` vers `observation_prix.id_magasin`). |
| `detail` (texte explicatif) | Non stocké : reconstruit à la lecture à partir du type, de la valeur et de l'observation. |
| `id_utilisateur` | Toutes les requêtes sont limitées à l'utilisateur connecté : le constructeur de `AlerteRepositoryMySql` et de `ObservationLecteurMySql` reçoit `(chaineConnexion, idUtilisateur)`. |
| `id_observation` (obligatoire) | `Alerte.IdObservation` est renseigné par `DetectionService` : **T2 doit enregistrer l'observation (Id > 0) AVANT d'appeler `Analyser`**. |
| `date_alerte` (DATETIME) | Le filtre « Au » inclut toute la journée de fin. |
| `parametre.valeur` | Stockée en pourcentage (5.000, 3.000) ; `ParametresDetectionMySql` convertit en fractions (0,05 ; 0,03). |
| `alerte.valeur` | Stocke une fraction (0,082 = + 8,2 %), DECIMAL(12,4). |
| Observation précédente | Même utilisateur, même produit, même magasin, strictement antérieure (date puis identifiant). |

## Règles appliquées (cahier des charges §4.2)

- Prix unitaire = prix de l'article / quantité en unité de base (2,40 pour 500 g = 4,80 par kg).
- Hausse : variation du prix unitaire **strictement supérieure** au seuil (5 %), même produit, même magasin, observations consécutives.
- Shrinkflation : baisse du conditionnement **au moins égale** au seuil (3 %) avec un prix de l'article supérieur ou égal ; la valeur de l'alerte est la hausse réelle du prix unitaire.
- Sans observation antérieure : aucune alerte. Les deux alertes peuvent coexister sur une même observation.
