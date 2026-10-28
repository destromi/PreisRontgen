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
- fournit la chaîne de connexion (hors du code) à `AlerteRepositoryMySql` et `ObservationLecteurMySql`.

## Ajouts demandés au script SQL (T2) par rapport au modèle indicatif

```sql
-- observation_prix : le prix payé pour l'article est nécessaire à la shrinkflation
ALTER TABLE observation_prix ADD COLUMN prix_total DECIMAL(12,4) NOT NULL;

-- alerte : magasin (filtre EF-09) et texte explicatif
CREATE TABLE alerte (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    id_produit  INT NOT NULL,
    id_magasin  INT NULL,
    `type`      ENUM('HAUSSE','SHRINKFLATION') NOT NULL,
    valeur      DECIMAL(12,6) NOT NULL,      -- variation relative (0,05 = +5 %)
    date_alerte DATE NOT NULL,
    lue         TINYINT(1) NOT NULL DEFAULT 0,
    detail      VARCHAR(500) NULL,
    FOREIGN KEY (id_produit) REFERENCES produit(id),
    FOREIGN KEY (id_magasin) REFERENCES magasin(id),
    INDEX idx_alerte_filtre (date_alerte, id_magasin, `type`, lue)
);
```

## Règles appliquées (cahier des charges §4.2)

- Prix unitaire = prix de l'article / quantité en unité de base (2,40 pour 500 g = 4,80 par kg).
- Hausse : variation du prix unitaire **strictement supérieure** au seuil (5 %), même produit, même magasin, observations consécutives.
- Shrinkflation : baisse du conditionnement **au moins égale** au seuil (3 %) avec un prix de l'article supérieur ou égal ; la valeur de l'alerte est la hausse réelle du prix unitaire.
- Sans observation antérieure : aucune alerte. Les deux alertes peuvent coexister sur une même observation.
