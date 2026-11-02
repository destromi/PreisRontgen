' ============================================================================
' FICHIER : Data/AlerteRepositoryMySql.vb
' RÔLE    : implémentation MySQL de IAlerteRepository, ADAPTÉE AU SCRIPT SQL DU
'           GROUPE (tables alerte, observation_prix, produit, magasin).
' POINTS DE COMPATIBILITÉ avec le script :
'           - alerte contient id_utilisateur, id_produit, id_observation, type,
'             valeur, date_alerte (DATETIME), lue : on n'ajoute AUCUNE colonne ;
'           - le magasin d'une alerte se déduit de l'observation (id_observation) ;
'           - toutes les requêtes sont limitées à l'utilisateur connecté
'             (isolation des foyers) ;
'           - le texte du détail n'est pas stocké : il est reconstruit à la lecture.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Text            ' Pour StringBuilder (construction efficace d'un texte long)
Imports MySqlConnector         ' Bibliothèque NuGet qui permet de parler à MySQL
Imports PreisRontgen.Models    ' Pour utiliser Alerte, FiltreAlerte, TypeAlerte

Namespace Data                 ' Espace de noms : PreisRontgen.Data

    Public Class AlerteRepositoryMySql           ' Début de la classe
        Implements IAlerteRepository             ' S'engage à fournir toutes les méthodes du contrat IAlerteRepository

        Private ReadOnly _chaineConnexion As String   ' Texte de connexion (serveur, base, utilisateur...) ; ReadOnly = fixé une fois pour toutes
        Private ReadOnly _idUtilisateur As Integer    ' Utilisateur connecté : seules SES alertes sont lues et modifiées

        Public Sub New(chaineConnexion As String, idUtilisateur As Integer)   ' Constructeur : chaîne de connexion + utilisateur connecté
            If String.IsNullOrWhiteSpace(chaineConnexion) Then   ' Texte absent ou vide
                Throw New ArgumentException("La chaîne de connexion est obligatoire.", NameOf(chaineConnexion))   ' On refuse avec un message clair
            End If                                    ' Fin du contrôle
            If idUtilisateur <= 0 Then                ' Identifiant d'utilisateur invalide
                Throw New ArgumentOutOfRangeException(NameOf(idUtilisateur), "L'identifiant de l'utilisateur doit être positif.")   ' On refuse
            End If                                    ' Fin du contrôle
            _chaineConnexion = chaineConnexion        ' Mémorise la chaîne pour les futures requêtes
            _idUtilisateur = idUtilisateur            ' Mémorise l'utilisateur connecté
        End Sub                                       ' Fin du constructeur

        Public Function Ajouter(alerte As Alerte) As Integer Implements IAlerteRepository.Ajouter   ' Enregistre une alerte
            If alerte.IdObservation <= 0 Then         ' L'observation d'origine n'est pas enregistrée
                Throw New InvalidOperationException("L'observation doit être enregistrée avant l'analyse (IdObservation manquant).")   ' Message clair pour T2
            End If                                    ' Fin du contrôle
            Const sql As String =
                "INSERT INTO alerte (id_utilisateur, id_produit, id_observation, `type`, valeur, date_alerte, lue) " &
                "VALUES (@utilisateur, @produit, @observation, @type, @valeur, @date, @lue); " &
                "SELECT LAST_INSERT_ID();"            ' Requête : insère puis demande l'identifiant créé ; les @... sont des paramètres
            Try                                       ' Début du bloc surveillé : toute erreur MySQL sera interceptée plus bas
                Using cn As New MySqlConnection(_chaineConnexion)   ' Crée la connexion ; Using la fermera automatiquement
                    cn.Open()                         ' Ouvre la connexion à MySQL
                    Using cmd As New MySqlCommand(sql, cn)          ' Crée la commande SQL liée à cette connexion
                        cmd.Parameters.AddWithValue("@utilisateur", _idUtilisateur)      ' Utilisateur connecté
                        cmd.Parameters.AddWithValue("@produit", alerte.IdProduit)        ' Produit concerné
                        cmd.Parameters.AddWithValue("@observation", alerte.IdObservation)   ' Observation à l'origine de l'alerte
                        cmd.Parameters.AddWithValue("@type", alerte.Type.ToString().ToUpperInvariant())   ' Type en texte majuscule : HAUSSE ou SHRINKFLATION
                        cmd.Parameters.AddWithValue("@valeur", alerte.Valeur)            ' Variation en fraction (0,082 = +8,2 %)
                        cmd.Parameters.AddWithValue("@date", alerte.DateAlerte)          ' Date de l'alerte (colonne DATETIME)
                        cmd.Parameters.AddWithValue("@lue", alerte.Lue)                  ' État lue / non lue
                        Return Convert.ToInt32(cmd.ExecuteScalar())                      ' Exécute et rend l'identifiant créé
                    End Using                         ' Libère la commande
                End Using                             ' Ferme la connexion
            Catch ex As MySqlException                ' Si MySQL signale une erreur (serveur arrêté, table absente...)
                Throw New InvalidOperationException("Impossible d'enregistrer l'alerte : " & ex.Message, ex)   ' On la transforme en message clair pour l'interface
            End Try                                   ' Fin du bloc surveillé
        End Function                                  ' Fin de Ajouter

        Public Function Rechercher(filtre As FiltreAlerte) As List(Of Alerte) Implements IAlerteRepository.Rechercher   ' Recherche avec filtres (EF-09)
            Dim sql As New StringBuilder(
                "SELECT a.id, a.id_produit, o.id_magasin, a.`type`, a.valeur, a.date_alerte, a.lue, " &
                "p.nom AS nom_produit, m.nom AS nom_magasin, o.prix_unitaire, o.quantite_base, a.id_observation " &
                "FROM alerte a " &
                "INNER JOIN produit p ON p.id = a.id_produit " &
                "INNER JOIN observation_prix o ON o.id = a.id_observation " &
                "INNER JOIN magasin m ON m.id = o.id_magasin " &
                "WHERE a.id_utilisateur = @utilisateur")   ' Le magasin vient de l'observation ; on ne lit que les alertes de l'utilisateur connecté
            Dim parametres As New List(Of MySqlParameter) From {
                New MySqlParameter("@utilisateur", _idUtilisateur)
            }                                         ' Liste des paramètres à donner à la requête

            If filtre IsNot Nothing Then              ' Si un filtre est fourni, on ajoute les conditions demandées
                If filtre.DateDebut.HasValue Then     ' Une date de début est choisie
                    sql.Append(" AND a.date_alerte >= @debut")   ' Ajoute la condition « à partir de » (minuit)
                    parametres.Add(New MySqlParameter("@debut", filtre.DateDebut.Value.Date))   ' Valeur du paramètre @debut
                End If                                ' Fin date de début
                If filtre.DateFin.HasValue Then       ' Une date de fin est choisie
                    sql.Append(" AND a.date_alerte < @finExclue")   ' Strictement avant le lendemain : inclut toute la journée de fin, heures comprises (date_alerte est un DATETIME)
                    parametres.Add(New MySqlParameter("@finExclue", filtre.DateFin.Value.Date.AddDays(1)))   ' Lendemain de la date de fin
                End If                                ' Fin date de fin
                If filtre.IdMagasin.HasValue Then     ' Un magasin est choisi
                    sql.Append(" AND o.id_magasin = @magasin")   ' Condition sur le magasin de l'observation
                    parametres.Add(New MySqlParameter("@magasin", filtre.IdMagasin.Value))      ' Valeur du paramètre @magasin
                End If                                ' Fin magasin
                If filtre.Type.HasValue Then          ' Un type d'alerte est choisi
                    sql.Append(" AND a.`type` = @type")          ' Ajoute la condition sur le type
                    parametres.Add(New MySqlParameter("@type", filtre.Type.Value.ToString().ToUpperInvariant()))   ' HAUSSE ou SHRINKFLATION
                End If                                ' Fin type
                If filtre.Lue.HasValue Then           ' Un statut est choisi
                    sql.Append(" AND a.lue = @lue")              ' Ajoute la condition sur lu / non lu
                    parametres.Add(New MySqlParameter("@lue", filtre.Lue.Value))                ' Valeur du paramètre @lue
                End If                                ' Fin statut
            End If                                    ' Fin des filtres

            sql.Append(" ORDER BY a.date_alerte DESC, a.id DESC")   ' Tri : les plus récentes d'abord

            Dim resultat As New List(Of Alerte)()     ' Liste (vide) des alertes trouvées
            Try                                       ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion (fermée automatiquement)
                    cn.Open()                         ' Ouvre la connexion
                    Using cmd As New MySqlCommand(sql.ToString(), cn)   ' Commande avec le texte SQL construit
                        cmd.Parameters.AddRange(parametres.ToArray())   ' Transmet tous les paramètres d'un coup
                        Using lecteur As MySqlDataReader = cmd.ExecuteReader()   ' Exécute et obtient un « lecteur » de lignes
                            While lecteur.Read()      ' Avance ligne par ligne tant qu'il en reste
                                resultat.Add(LireAlerte(lecteur))   ' Convertit la ligne en objet Alerte et l'ajoute
                            End While                 ' Fin de la boucle
                        End Using                     ' Libère le lecteur
                    End Using                         ' Libère la commande
                End Using                             ' Ferme la connexion
            Catch ex As MySqlException                ' Erreur MySQL
                Throw New InvalidOperationException("Impossible de lire les alertes : " & ex.Message, ex)   ' Message clair
            End Try                                   ' Fin du bloc surveillé
            Return resultat                           ' Rend la liste des alertes
        End Function                                  ' Fin de Rechercher

        Private Shared Function LireAlerte(lecteur As MySqlDataReader) As Alerte   ' Transforme la ligne courante en objet Alerte
            Dim typeLu As TypeAlerte              ' Variable qui recevra le type lu en base
            If Not [Enum].TryParse(lecteur.GetString(3), True, typeLu) Then   ' Convertit le texte en TypeAlerte (True = sans tenir compte des majuscules)
                typeLu = TypeAlerte.Hausse        ' Texte inconnu : valeur par défaut
            End If                                ' Fin du contrôle

            Dim valeur As Decimal = lecteur.GetDecimal(4)        ' Colonne 4 : variation (fraction)
            Dim prixUnitaire As Decimal = lecteur.GetDecimal(9)  ' Colonne 9 : prix unitaire de l'observation d'origine
            Dim quantiteBase As Decimal = lecteur.GetDecimal(10) ' Colonne 10 : quantité (unité de base) de l'observation d'origine

            Return New Alerte With {              ' Crée l'objet Alerte
                .Id = lecteur.GetInt32(0),        ' Colonne 0 : identifiant
                .IdProduit = lecteur.GetInt32(1), ' Colonne 1 : produit
                .IdMagasin = lecteur.GetInt32(2), ' Colonne 2 : magasin (déduit de l'observation)
                .Type = typeLu,                   ' Colonne 3 : type déjà converti
                .Valeur = valeur,                 ' Variation
                .DateAlerte = lecteur.GetDateTime(5),   ' Colonne 5 : date
                .Lue = lecteur.GetBoolean(6),     ' Colonne 6 : lue ou non
                .NomProduit = lecteur.GetString(7),     ' Colonne 7 : nom du produit (jointure)
                .NomMagasin = lecteur.GetString(8),     ' Colonne 8 : nom du magasin (jointure)
                .IdObservation = lecteur.GetInt32(11),  ' Colonne 11 : observation d'origine
                .Detail = ConstruireDetail(typeLu, valeur, prixUnitaire, quantiteBase)   ' Phrase explicative reconstruite
            }                                     ' Fin de l'initialisation
        End Function                              ' Fin de LireAlerte

        ''' <summary>Reconstruit le texte du détail (non stocké en base) à partir de l'observation d'origine.</summary>
        Private Shared Function ConstruireDetail(type As TypeAlerte, valeur As Decimal, prixUnitaire As Decimal, quantiteBase As Decimal) As String   ' Texte affiché dans le panneau de détail
            If type = TypeAlerte.Hausse Then      ' Cas d'une hausse
                Return $"Le prix unitaire a augmenté de {valeur:P1} (nouveau prix unitaire : {prixUnitaire:N2} par unité de base)."   ' Phrase pour une hausse
            End If                                ' Fin du cas hausse
            Return $"Quantité réduite sans baisse de prix ; hausse réelle du prix unitaire : {valeur:P1} (quantité observée : {quantiteBase:0.###} en unité de base, prix unitaire : {prixUnitaire:N2})."   ' Phrase pour une shrinkflation
        End Function                              ' Fin de ConstruireDetail

        Public Sub MarquerCommeLue(idAlerte As Integer) Implements IAlerteRepository.MarquerCommeLue   ' Marque une alerte comme lue
            Try                                   ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion
                    cn.Open()                     ' Ouvre
                    Using cmd As New MySqlCommand("UPDATE alerte SET lue = 1 WHERE id = @id AND id_utilisateur = @utilisateur", cn)   ' Met à jour une seule alerte, seulement si elle appartient à l'utilisateur
                        cmd.Parameters.AddWithValue("@id", idAlerte)   ' Identifiant de l'alerte
                        cmd.Parameters.AddWithValue("@utilisateur", _idUtilisateur)   ' Utilisateur connecté
                        cmd.ExecuteNonQuery()     ' Exécute une requête qui ne renvoie pas de lignes
                    End Using                     ' Libère la commande
                End Using                         ' Ferme la connexion
            Catch ex As MySqlException           ' Erreur MySQL
                Throw New InvalidOperationException("Impossible de marquer l'alerte comme lue : " & ex.Message, ex)   ' Message clair
            End Try                               ' Fin du bloc surveillé
        End Sub                                   ' Fin de MarquerCommeLue

        Public Sub MarquerToutesCommeLues() Implements IAlerteRepository.MarquerToutesCommeLues   ' Marque toutes les alertes de l'utilisateur comme lues
            Try                                   ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion
                    cn.Open()                     ' Ouvre
                    Using cmd As New MySqlCommand("UPDATE alerte SET lue = 1 WHERE lue = 0 AND id_utilisateur = @utilisateur", cn)   ' Met à « lue » celles de l'utilisateur qui ne le sont pas
                        cmd.Parameters.AddWithValue("@utilisateur", _idUtilisateur)   ' Utilisateur connecté
                        cmd.ExecuteNonQuery()     ' Exécute la mise à jour
                    End Using                     ' Libère la commande
                End Using                         ' Ferme la connexion
            Catch ex As MySqlException           ' Erreur MySQL
                Throw New InvalidOperationException("Impossible de marquer les alertes comme lues : " & ex.Message, ex)   ' Message clair
            End Try                               ' Fin du bloc surveillé
        End Sub                                   ' Fin de MarquerToutesCommeLues

        Public Function CompterNonLues() As Integer Implements IAlerteRepository.CompterNonLues   ' Compte les alertes non lues de l'utilisateur
            Try                                   ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion
                    cn.Open()                     ' Ouvre
                    Using cmd As New MySqlCommand("SELECT COUNT(*) FROM alerte WHERE lue = 0 AND id_utilisateur = @utilisateur", cn)   ' Requête de comptage
                        cmd.Parameters.AddWithValue("@utilisateur", _idUtilisateur)   ' Utilisateur connecté
                        Return Convert.ToInt32(cmd.ExecuteScalar())   ' Rend le nombre obtenu (première valeur du résultat)
                    End Using                     ' Libère la commande
                End Using                         ' Ferme la connexion
            Catch ex As MySqlException           ' Erreur MySQL
                Throw New InvalidOperationException("Impossible de compter les alertes non lues : " & ex.Message, ex)   ' Message clair
            End Try                               ' Fin du bloc surveillé
        End Function                              ' Fin de CompterNonLues

        Public Function ListerMagasins() As List(Of KeyValuePair(Of Integer, String)) Implements IAlerteRepository.ListerMagasins   ' Liste les magasins pour le filtre
            Dim resultat As New List(Of KeyValuePair(Of Integer, String))()   ' Liste (vide) de couples (identifiant, nom)
            Try                                   ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion
                    cn.Open()                     ' Ouvre
                    Using cmd As New MySqlCommand("SELECT id, nom FROM magasin ORDER BY nom", cn)   ' Requête : magasins triés par nom
                        Using lecteur As MySqlDataReader = cmd.ExecuteReader()   ' Exécute et lit les lignes
                            While lecteur.Read()  ' Pour chaque magasin
                                resultat.Add(New KeyValuePair(Of Integer, String)(lecteur.GetInt32(0), lecteur.GetString(1)))   ' Ajoute le couple (id, nom)
                            End While             ' Fin de la boucle
                        End Using                 ' Libère le lecteur
                    End Using                     ' Libère la commande
                End Using                         ' Ferme la connexion
            Catch ex As MySqlException           ' Erreur MySQL
                Throw New InvalidOperationException("Impossible de lire les magasins : " & ex.Message, ex)   ' Message clair
            End Try                               ' Fin du bloc surveillé
            Return resultat                       ' Rend la liste des magasins
        End Function                              ' Fin de ListerMagasins

    End Class                                     ' Fin de la classe AlerteRepositoryMySql

End Namespace                                     ' Fin de l'espace de noms Data
