# Pricieux

**Suivi des prix et hausse des prix**

Pricieux est une application de bureau Windows qui aide un foyer à enregistrer ses achats, suivre l’évolution des prix et repérer les hausses ou réductions de quantité. Les observations viennent des tickets saisis ou importés par les utilisateurs ; l’application ne télécharge pas les prix des magasins.

## Fonctionnalités

- Comptes utilisateur avec mots de passe hachés et salés ; le premier compte créé reçoit le rôle administrateur.
- Gestion des magasins, catégories et produits, avec unités de référence kilogramme, litre et pièce.
- Saisie de tickets et calcul du prix par unité de référence, avec conversion g/kg, mg/kg, ml/L et cl/L.
- Historique des observations de prix par produit et magasin, graphiques et alertes filtrables.
- Détection des hausses de prix et de la réduflation à partir des quantités et prix saisis sur les tickets.
- Indice personnel de type Laspeyres, comparaison des prix entre magasins et recherche de produits moins chers de même catégorie et unité.
- Tableau de bord des dépenses du mois, des alertes non lues, de l’indice personnel et des échéances proches.
- Gestion de dépenses récurrentes hebdomadaires, mensuelles, trimestrielles ou annuelles, avec rappels.
- Import et export d’historique au format CSV.
- Lecture facultative de photos de tickets avec Tesseract OCR ; les champs reconnus doivent être vérifiés avant l’enregistrement.
- Recherche facultative d’un produit par code-barres dans Open Food Facts.

## Technologies

- VB.NET et Windows Forms
- .NET 8 pour Windows
- MySQL ou MariaDB
- [MySqlConnector](https://www.nuget.org/packages/MySqlConnector) 2.6.2
- Tesseract OCR, en option
- API Open Food Facts, en option

## Prérequis

- Windows 10 ou version ultérieure
- .NET 8 SDK, ou Visual Studio 2022 avec la charge de travail **Développement .NET Desktop**
- MySQL 8 ou MariaDB accessible depuis l’ordinateur
- Tesseract avec les données linguistiques française et anglaise, uniquement pour la lecture OCR

## Installation et premier lancement

### 1. Récupérer le projet

```powershell
git clone <URL_DU_DEPOT>
cd <DOSSIER_DU_PROJET>
```

Ouvre `Pricieux.sln` dans Visual Studio, ou utilise les commandes .NET indiquées plus bas.

### 2. Créer la base de données

Dans MySQL Workbench, ouvre et exécute [`database/schema.sql`](database/schema.sql). Le script crée la base `pricieux_app` et ses tables. Il ne modifie pas une base existante appelée `pricieux`.

Crée ensuite un compte MySQL dédié à l’application. Par exemple, après avoir remplacé le mot de passe fictif :

```sql
CREATE USER 'pricieux_app_user'@'localhost'
  IDENTIFIED BY 'REMPLACER_PAR_UN_MOT_DE_PASSE_FORT';

GRANT SELECT, INSERT, UPDATE, DELETE
  ON pricieux_app.* TO 'pricieux_app_user'@'localhost';
```

Le compte d’exécution n’a pas besoin d’être `root`. L’import du schéma doit être fait par un compte MySQL autorisé à créer la base et les tables.

### 3. Configurer la connexion

Au premier démarrage, Pricieux ouvre la fenêtre de configuration de la base. Saisis le serveur, le port, `pricieux_app`, ainsi que le compte MySQL dédié, puis clique sur **Tester et enregistrer**.

La chaîne de connexion est enregistrée pour le compte Windows courant, chiffrée avec Windows DPAPI, dans `%LOCALAPPDATA%\Pricieux\connexion.bin`. Si la connexion échoue lors d’une tentative de connexion à Pricieux, l’application propose d’ouvrir à nouveau la configuration.

Pour les installations avancées, une variable d’environnement temporaire peut remplacer cette configuration enregistrée lorsqu’aucune configuration locale n’existe :

```powershell
$env:PRICIEUX_CONNECTION_STRING = "Server=localhost;Port=3306;Database=pricieux_app;User ID=pricieux_app_user;Password=REMPLACER;"
```

Remplace `REMPLACER` par le vrai mot de passe. Ne publie jamais une chaîne de connexion réelle dans le dépôt, un ticket ou une capture d’écran. Une connexion enregistrée localement est prioritaire sur la variable d’environnement.

### 4. Restaurer les dépendances et démarrer

Depuis le dossier du projet :

```powershell
dotnet restore .\Pricieux.sln
dotnet run --project .\Pricieux.vbproj
```

Ou démarre le projet **Pricieux** dans Visual Studio. Crée un compte depuis l’écran d’inscription ; le premier compte de la base devient administrateur. Les comptes suivants sont des utilisateurs.

## Configuration facultative de l’OCR

Installe Tesseract et ses données linguistiques `fra` et `eng`, puis définis le chemin vers `tesseract.exe` dans la session PowerShell qui démarre l’application :

```powershell
$env:PRICIEUX_TESSERACT_PATH = "C:\Program Files\Tesseract-OCR\tesseract.exe"
dotnet run --project .\Pricieux.vbproj
```

La photo est lue localement par Tesseract. Les valeurs détectées restent à contrôler dans le formulaire de ticket avant l’enregistrement.

## Utilisation d’Open Food Facts

La recherche est lancée manuellement depuis **Référentiels → Produits**, à partir d’un code-barres numérique. L’application demande confirmation avant la requête. Seul le code-barres et les métadonnées réseau habituelles sont transmis à Open Food Facts ; les tickets, prix payés et comptes ne le sont pas.

Le catalogue est collaboratif : vérifie les informations proposées. La quantité du catalogue est indicative et ne sert pas à détecter la réduflation ; la quantité utilisée pour cette détection est saisie sur le ticket. La saisie manuelle reste disponible sans connexion Internet.

Consulte la [documentation de l’API Open Food Facts](https://openfoodfacts.github.io/documentation/docs/Product-Opener/api/) ainsi que ses conditions et licences avant toute redistribution de données provenant du catalogue.

## Règles de calcul et limites

- Les hausses et réductions de quantité sont comparées pour le même produit et le même magasin, à partir des observations enregistrées dans Pricieux.
- Le seuil initial de hausse de prix est de 5 % ; celui de réduction de quantité est de 3 %. Un administrateur peut les modifier dans l’application.
- Une alerte de réduflation est déclenchée lorsque la quantité par paquet baisse au moins du seuil configuré et que le prix du paquet ne baisse pas.
- Les rappels de dépenses sont vérifiés au démarrage puis pendant que l’application reste ouverte. Une échéance proche apparaît dans le tableau de bord ; aucun paiement automatique n’est déclenché.
- Les magasins, catégories et produits peuvent être désactivés afin de préserver l’historique. Cette opération n’efface pas les données historiques ; l’interface actuelle ne propose pas de réactivation.
- Les prix ne sont pas collectés automatiquement depuis les magasins ou les comptes bancaires. Ils proviennent de tickets saisis, importés en CSV ou préremplis à partir d’une photo soumise manuellement à l’OCR.

## Format CSV

L’import accepte les fichiers séparés par point-virgule ou virgule. La première ligne doit contenir ces colonnes :

```csv
date;magasin;produit;marque;categorie;unite_base;conditionnement;unite;prix;nombre
2026-10-31;Marché Central;Riz;Marque A;Épicerie;kg;500;g;750;2
```

Les magasins, catégories et produits doivent déjà exister dans les référentiels. L’import ne les crée pas automatiquement. Les lignes rejetées sont signalées après traitement.

## Structure du dépôt

```text
Data/       Accès MySQL et dépôts de données
Models/     Entités et objets métier
Services/   Calculs, normalisation, import/export, OCR et API externe
Utils/      Configuration de connexion et hachage des mots de passe
Views/      Fenêtres et écrans Windows Forms
database/   Schéma initial de la base MySQL/MariaDB
```

## Compilation

```powershell
dotnet build .\Pricieux.sln -c Release
```

Le projet cible `net8.0-windows` et nécessite Windows pour exécuter l’interface.

## Sécurité et dépôt Git

- N’ajoute pas de mot de passe, chaîne de connexion, export CSV personnel, photo de ticket ou fichier de configuration locale au dépôt.
- Les mots de passe des comptes Pricieux sont stockés sous forme de hachage salé ; la chaîne de connexion locale est chiffrée pour l’utilisateur Windows courant.
- Les requêtes MySQL utilisent des paramètres pour les valeurs fournies par l’utilisateur.
- Vérifie les fichiers ignorés par [`.gitignore`](.gitignore) avant le premier `git add`.

## Contribution et attribution

Le code a été réalisé avec l’aide d’un assistant d’intelligence artificielle. Les contributeurs doivent relire les modifications, pouvoir expliquer le code remis et déclarer cette aide conformément aux règles académiques ou de contribution applicables au projet.
