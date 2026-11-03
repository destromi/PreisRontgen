# Jeu d’essai OCR et import CSV

Ces fichiers sont fictifs et cohérents. L’image et le CSV décrivent le même achat daté du 3 novembre 2026 au magasin « Magasin test OCR ».

## Référentiels requis

L’import n’invente pas les magasins, catégories ni produits. Si les références du jeu d’essai ne sont pas déjà présentes, exécute `preparation_referentiels.sql` dans la base applicative `pricieux_app`. Le script ne vise pas la base `pricieux`. Tu peux aussi créer les mêmes entrées depuis l’onglet Référentiels.

## Lecture OCR

Configure Tesseract et ses données de langue française. Si nécessaire, définis `PRICIEUX_TESSERACT_PATH` vers `tesseract.exe` dans la session PowerShell qui démarre l’application. Dans **Saisie ticket**, choisis `Magasin test OCR`, clique **Lire une photo (OCR)** et ouvre `ticket_ocr_pricieux.png`.

Vérifie trois lignes avant de sauvegarder: lait entier, 2 × 1 L à 1 250 FCFA; riz parfumé, 2 × 500 g à 850 FCFA; café moulu, 1 × 100 g à 2 500 FCFA. Le total attendu est 6 700 FCFA. Les accents de « parfumé » et « Café » testent aussi la reconnaissance des caractères.

## Import CSV

Dans **CSV et dépenses > Import et export CSV**, clique **Importer un CSV** et sélectionne `historique_ticket_ocr.csv`. Les lignes ayant même date et même magasin doivent former un ticket de trois lignes pour un total de 6 700 FCFA.

Le CSV recrée le même achat que l’image. Pour éviter un doublon, teste les deux parcours dans une base de test réinitialisable ou n’enregistre qu’un parcours dans ta base courante: l’import ne détecte pas un ticket déjà présent.
