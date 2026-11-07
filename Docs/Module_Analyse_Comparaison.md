# Module « Analyse et comparaison » (T4) : EF-06, EF-10, EF-11, EF-12, EF-14

Ce module suit l'onglet **Historique**, l'onglet **Analyses** et le sous-onglet **Dépenses récurrentes** (onglet « CSV et dépenses ») de la maquette du groupe, en vert forêt. Il fonctionne avec le schéma final `database/schema.sql` (base `pricieux_app`) et avec le module T3.

## Fichiers

| Dossier | Fichier | Rôle |
|---|---|---|
| Models | `Analyse.vb` | Classes de données : `ProduitRef`, `PointPrix`, `ResumeHistorique`, `LignePrixMagasin`, `Equivalent`, `AchatProduit`, `LignePanier`, `ResultatIndice`, `Periodicite`, `DepenseRecurrente`, `RappelEcheance` |
| Data | `IAnalyseRepository.vb` | Contrats `IAnalyseRepository` (lectures) et `IDepenseRepository` (dépenses) |
| Data | `AnalyseRepositoryMySql.vb`, `DepenseRepositoryMySql.vb` | Implémentations MySQL (requêtes paramétrées, limitées au foyer de l'utilisateur) |
| Services | `HistoriqueService.vb` | EF-06 : tri, réduction à 400 points, résumé, positions du graphique |
| Services | `IndiceService.vb` | EF-10 : indice de Laspeyres base 100 |
| Services | `ComparaisonService.vb` | EF-11 et EF-12 : classement des magasins, équivalents moins chers |
| Services | `DepenseService.vb` | EF-14 : validation, échéance suivante, rappels, événement `EcheanceProche` |
| Services | `FormatageAnalyse.vb` | Affichage des prix (« 1 250 / L »), des quantités et des unités |
| Views | `frmHistorique`, `frmAnalyses`, `frmDepenses` (+ Designer), `PanneauGraphique.vb` | Les trois écrans |
| Demo | `DepotAnalyseDemo.vb`, `DepotDepenseDemo.vb`, `FenetreDemo.vb` | Données d'exemple et onglets de démonstration |
| Tests | `HistoriqueServiceTests`, `IndiceServiceTests`, `ComparaisonServiceTests`, `DepenseServiceTests`, `FauxDepotsAnalyse` | 43 cas de test (CT-56 à CT-98) |

## Règles appliquées

**EF-06 Historique.** Tableau (Date, Magasin, Conditionnement, Prix unitaire) trié du plus récent au plus ancien. Graphique dessiné dans l'événement `Paint` d'un panneau sans scintillement : une courbe par magasin, légende, quadrillage, dates aux extrémités. Au-delà de 400 points, on garde 400 points régulièrement espacés (premier et dernier conservés). Mesuré par le test CT-64 : tri, résumé et courbe de 5 000 observations en moins d'une seconde.

**EF-10 Indice personnel.** Panier de référence = produits les plus achetés (nombre de tickets, puis quantité), au plus 20, pendant les 30 jours qui précèdent la date de départ. Prix à une date = dernier prix unitaire observé à cette date ou avant, tous magasins confondus. Indice = 100 x coût du panier aux prix d'arrivée / coût aux prix de départ (quantités de la période de départ), arrondi à 2 décimales. Un produit sans prix aux deux dates est écarté. Si le calcul est impossible (dates inversées, aucun achat, aucun prix), l'écran affiche un message clair au lieu d'une valeur.

**EF-11 Comparaison.** Le prix d'un magasin est le dernier prix unitaire observé dans ce magasin. Classement du moins cher au plus cher, avec l'écart par rapport au moins cher ; possible par produit ou par catégorie.

**EF-12 Équivalents.** Même catégorie et même unité de base, prix unitaire strictement inférieur au dernier prix du produit choisi, économie en pourcentage. Seul le prix est comparé, sans jugement de qualité. Un rappel « information et non conseil d'achat » est affiché.

**EF-14 Dépenses récurrentes.** Périodicités hebdomadaire (+7 jours), mensuelle (+1 mois), trimestrielle (+3 mois), annuelle (+1 an). Rappel pour toute dépense active dont l'échéance tombe dans les 7 jours ou est dépassée (« en retard de 2 jours »). Aucun paiement automatique : le bouton « Échéance suivante » avance seulement la date, après règlement par l'utilisateur. Une dépense désactivée n'est pas supprimée.

## Brancher les écrans dans la fenêtre principale

```vb
Dim cs As String = "<chaîne de connexion>"      ' celle déjà utilisée par frmAlertes
Dim idUtilisateur As Integer = <utilisateur connecté>

Dim historique As New frmHistorique(New AnalyseRepositoryMySql(cs, idUtilisateur))
Dim analyses   As New frmAnalyses(New AnalyseRepositoryMySql(cs, idUtilisateur))
Dim depenses   As New frmDepenses(New DepenseRepositoryMySql(cs, idUtilisateur))
```

Chaque écran se place dans son onglet comme `frmAlertes` (`TopLevel = False`, `Dock = Fill`, `Show()`). Après une saisie de ticket, `historique.Rafraichir()` met l'historique à jour ; `analyses.RechargerListes()` recharge les listes de produits.

## Essayer sans base de données

Le projet `Demo` ouvre la fenêtre de démonstration avec les onglets **Historique**, **Analyses** et **CSV et dépenses** (sous-onglet **Dépenses récurrentes**), alimentés par des dépôts en mémoire (neuf mois de prix pour sept produits dans trois magasins).

## Plan de tests (extrait pour L5)

| Cas | Vérifie |
|---|---|
| CT-56 à CT-59 | Tri du tableau, ordre chronologique, réduction à 400 points avec premier et dernier conservés |
| CT-60, CT-61 | Résumé chiffré, historique vide |
| CT-62, CT-63 | Positions du graphique (courbe plate, extrêmes) |
| CT-64 | 5 000 observations en moins d'une seconde |
| CT-65 à CT-71 | Indice : stable = 100, +10 % = 110, baisse, pondération par les quantités, arrondi, panier vide, prix nul |
| CT-72, CT-73 | Produit sans prix écarté, panier limité aux plus achetés |
| CT-74 à CT-77 | Dates inversées, aucun achat, calcul de bout en bout, aucun prix |
| CT-78 à CT-84 | Classement des magasins, équivalents (même unité, moins cher, économie, tri, même catégorie, produit inconnu) |
| CT-85 à CT-89 | Échéances hebdomadaire, mensuelle (fin de mois), trimestrielle, annuelle (29 février) |
| CT-90, CT-91 | Validation des saisies |
| CT-92 à CT-94 | Rappels : frontière des 7 jours, échéance dépassée, dépense inactive |
| CT-95 à CT-98 | Événement `EcheanceProche`, enregistrement, message d'erreur clair, désactivation et échéance suivante |
