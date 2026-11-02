-- Base réservée à l'application Pricieux. La base existante `pricieux` n'est pas modifiée.
CREATE DATABASE IF NOT EXISTS pricieux_app
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE pricieux_app;

CREATE TABLE IF NOT EXISTS utilisateur (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  login VARCHAR(80) NOT NULL UNIQUE,
  mot_de_passe_hash VARBINARY(32) NOT NULL,
  sel VARBINARY(16) NOT NULL,
  role ENUM('ADMIN', 'UTILISATEUR') NOT NULL DEFAULT 'UTILISATEUR',
  actif BOOLEAN NOT NULL DEFAULT TRUE,
  cree_le TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS magasin (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  nom VARCHAR(120) NOT NULL UNIQUE,
  actif BOOLEAN NOT NULL DEFAULT TRUE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS categorie (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  nom VARCHAR(120) NOT NULL UNIQUE,
  actif BOOLEAN NOT NULL DEFAULT TRUE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS produit (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  nom VARCHAR(180) NOT NULL,
  marque VARCHAR(120) NOT NULL DEFAULT '',
  id_categorie BIGINT NOT NULL,
  unite_base ENUM('kg', 'L', 'pièce') NOT NULL,
  code_barres VARCHAR(64) NULL,
  actif BOOLEAN NOT NULL DEFAULT TRUE,
  CONSTRAINT uq_produit_nom_marque UNIQUE (nom, marque),
  CONSTRAINT uq_produit_code_barres UNIQUE (code_barres),
  CONSTRAINT fk_produit_categorie FOREIGN KEY (id_categorie) REFERENCES categorie(id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS ticket (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  id_utilisateur BIGINT NOT NULL,
  id_magasin BIGINT NOT NULL,
  date_achat DATE NOT NULL,
  total_saisi DECIMAL(12,2) NOT NULL,
  cree_le TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_ticket_utilisateur FOREIGN KEY (id_utilisateur) REFERENCES utilisateur(id),
  CONSTRAINT fk_ticket_magasin FOREIGN KEY (id_magasin) REFERENCES magasin(id),
  INDEX ix_ticket_date_magasin (date_achat, id_magasin)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS ligne_ticket (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  id_ticket BIGINT NOT NULL,
  id_produit BIGINT NOT NULL,
  quantite_conditionnement DECIMAL(12,4) NOT NULL,
  unite VARCHAR(12) NOT NULL,
  prix DECIMAL(12,2) NOT NULL,
  nb_achete DECIMAL(12,3) NOT NULL,
  quantite_base_totale DECIMAL(14,6) NOT NULL,
  prix_unitaire DECIMAL(14,6) NOT NULL,
  CONSTRAINT fk_ligne_ticket_ticket FOREIGN KEY (id_ticket) REFERENCES ticket(id) ON DELETE CASCADE,
  CONSTRAINT fk_ligne_ticket_produit FOREIGN KEY (id_produit) REFERENCES produit(id),
  CONSTRAINT ck_ligne_conditionnement CHECK (quantite_conditionnement > 0),
  CONSTRAINT ck_ligne_prix CHECK (prix >= 0),
  CONSTRAINT ck_ligne_nombre CHECK (nb_achete > 0),
  INDEX ix_ligne_produit (id_produit)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS observation_prix (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  id_produit BIGINT NOT NULL,
  id_magasin BIGINT NOT NULL,
  id_ticket BIGINT NOT NULL,
  date_obs DATE NOT NULL,
  prix_unitaire DECIMAL(14,6) NOT NULL,
  quantite_base DECIMAL(14,6) NOT NULL,
  prix_conditionnement DECIMAL(12,2) NOT NULL,
  cree_le TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_obs_produit FOREIGN KEY (id_produit) REFERENCES produit(id),
  CONSTRAINT fk_obs_magasin FOREIGN KEY (id_magasin) REFERENCES magasin(id),
  CONSTRAINT fk_obs_ticket FOREIGN KEY (id_ticket) REFERENCES ticket(id) ON DELETE CASCADE,
  INDEX ix_observation_produit_magasin_date (id_produit, id_magasin, date_obs, id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS alerte (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  id_utilisateur BIGINT NOT NULL,
  id_produit BIGINT NOT NULL,
  id_magasin BIGINT NOT NULL,
  type ENUM('HAUSSE', 'SHRINKFLATION') NOT NULL,
  valeur DECIMAL(12,4) NOT NULL,
  prix_precedent DECIMAL(14,6) NOT NULL,
  prix_actuel DECIMAL(14,6) NOT NULL,
  date_alerte TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  lue BOOLEAN NOT NULL DEFAULT FALSE,
  CONSTRAINT fk_alerte_utilisateur FOREIGN KEY (id_utilisateur) REFERENCES utilisateur(id),
  CONSTRAINT fk_alerte_produit FOREIGN KEY (id_produit) REFERENCES produit(id),
  CONSTRAINT fk_alerte_magasin FOREIGN KEY (id_magasin) REFERENCES magasin(id),
  INDEX ix_alerte_utilisateur_date (id_utilisateur, date_alerte, lue)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS parametre (
  cle VARCHAR(80) PRIMARY KEY,
  valeur VARCHAR(255) NOT NULL
) ENGINE=InnoDB;

INSERT IGNORE INTO parametre (cle, valeur) VALUES
  ('seuil_hausse_pct', '5'),
  ('seuil_shrinkflation_pct', '3');

CREATE TABLE IF NOT EXISTS depense_recurrente (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  id_utilisateur BIGINT NOT NULL,
  libelle VARCHAR(180) NOT NULL,
  montant DECIMAL(12,2) NOT NULL,
  periodicite ENUM('HEBDOMADAIRE', 'MENSUELLE', 'TRIMESTRIELLE', 'ANNUELLE') NOT NULL,
  prochaine_echeance DATE NOT NULL,
  actif BOOLEAN NOT NULL DEFAULT TRUE,
  CONSTRAINT fk_depense_utilisateur FOREIGN KEY (id_utilisateur) REFERENCES utilisateur(id) ON DELETE CASCADE
) ENGINE=InnoDB;
