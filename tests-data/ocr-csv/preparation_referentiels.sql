-- Jeu de référence facultatif; exécuter uniquement dans pricieux_app.
USE pricieux_app;
INSERT INTO magasin(nom,actif) VALUES('Magasin test OCR',TRUE)
ON DUPLICATE KEY UPDATE actif=TRUE;
INSERT INTO categorie(nom,actif) VALUES
 ('Test OCR - Produits laitiers',TRUE),('Test OCR - Épicerie',TRUE)
ON DUPLICATE KEY UPDATE actif=TRUE;
INSERT INTO produit(nom,marque,id_categorie,unite_base,actif)
 SELECT 'Lait entier test OCR','Camlait OCR',id,'L',TRUE FROM categorie WHERE nom='Test OCR - Produits laitiers' LIMIT 1
ON DUPLICATE KEY UPDATE id_categorie=VALUES(id_categorie),unite_base=VALUES(unite_base),actif=TRUE;
INSERT INTO produit(nom,marque,id_categorie,unite_base,actif)
 SELECT 'Riz parfumé test OCR','Riz Nord OCR',id,'kg',TRUE FROM categorie WHERE nom='Test OCR - Épicerie' LIMIT 1
ON DUPLICATE KEY UPDATE id_categorie=VALUES(id_categorie),unite_base=VALUES(unite_base),actif=TRUE;
INSERT INTO produit(nom,marque,id_categorie,unite_base,actif)
 SELECT 'Café moulu test OCR','Aroma OCR',id,'kg',TRUE FROM categorie WHERE nom='Test OCR - Épicerie' LIMIT 1
ON DUPLICATE KEY UPDATE id_categorie=VALUES(id_categorie),unite_base=VALUES(unite_base),actif=TRUE;
