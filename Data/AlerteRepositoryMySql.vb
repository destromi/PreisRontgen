' ============================================================================
' FICHIER : Data/AlerteRepositoryMySql.vb
' RÔLE    : implémentation MySQL de IAlerteRepository : enregistre, recherche et
'           met à jour les alertes dans la table alerte.
' RÈGLES  : requêtes PARAMÉTRÉES (@nom) = protection contre l'injection SQL ;
'           « Using » = la connexion est fermée automatiquement, même en cas d'erreur ;
'           chaîne de connexion fournie de l'extérieur (jamais écrite en dur).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Text            ' Pour StringBuilder (construction efficace d'un texte long)
Imports MySqlConnector         ' Bibliothèque NuGet qui permet de parler à MySQL
Imports PreisRontgen.Models    ' Pour utiliser Alerte, FiltreAlerte, TypeAlerte

Namespace Data                 ' Espace de noms : PreisRontgen.Data

    ''' <summary>
    ''' Accès MySQL aux alertes (EF-07, EF-08, EF-09).
    ''' Toutes les requêtes sont paramétrées. La chaîne de connexion est fournie par l'appelant
    ''' (lue dans la configuration, jamais écrite en dur dans le code).
    ''' </summary>
    Public Class AlerteRepositoryMySql           ' Début de la classe
        Implements IAlerteRepository             ' S'engage à fournir toutes les méthodes du contrat IAlerteRepository

        Private ReadOnly _chaineConnexion As String   ' Texte de connexion (serveur, base, utilisateur...) ; ReadOnly = fixé une fois pour toutes

        Public Sub New(chaineConnexion As String)     ' Constructeur : reçoit la chaîne de connexion
            If String.IsNullOrWhiteSpace(chaineConnexion) Then   ' Texte absent ou vide
                Throw New ArgumentException("La chaîne de connexion est obligatoire.", NameOf(chaineConnexion))   ' On refuse avec un message clair
            End If                                    ' Fin du contrôle
            _chaineConnexion = chaineConnexion        ' Mémorise la chaîne pour les futures requêtes
        End Sub                                       ' Fin du constructeur

        Public Function Ajouter(alerte As Alerte) As Integer Implements IAlerteRepository.Ajouter   ' Enregistre une alerte
            Const sql As String =
                "INSERT INTO alerte (id_produit, id_magasin, `type`, valeur, date_alerte, lue, detail) " &
                "VALUES (@produit, @magasin, @type, @valeur, @date, @lue, @detail); " &
                "SELECT LAST_INSERT_ID();"            ' Requête : insère puis demande l'identifiant créé ; les @... sont des paramètres
            Try                                       ' Début du bloc surveillé : toute erreur MySQL sera interceptée plus bas
                Using cn As New MySqlConnection(_chaineConnexion)   ' Crée la connexion ; Using la fermera automatiquement
                    cn.Open()                         ' Ouvre la connexion à MySQL
                    Using cmd As New MySqlCommand(sql, cn)          ' Crée la commande SQL liée à cette connexion
                        cmd.Parameters.AddWithValue("@produit", alerte.IdProduit)    ' Donne l'identifiant du produit au paramètre @produit
                        cmd.Parameters.AddWithValue("@magasin", alerte.IdMagasin)    ' Donne l'identifiant du magasin
                        cmd.Parameters.AddWithValue("@type", alerte.Type.ToString().ToUpperInvariant())   ' Type en texte majuscule : HAUSSE ou SHRINKFLATION
                        cmd.Parameters.AddWithValue("@valeur", alerte.Valeur)        ' Variation en fraction
                        cmd.Parameters.AddWithValue("@date", alerte.DateAlerte.Date) ' Date sans l'heure
                        cmd.Parameters.AddWithValue("@lue", alerte.Lue)              ' État lue / non lue
                        cmd.Parameters.AddWithValue("@detail", alerte.Detail)        ' Texte explicatif
                        Return Convert.ToInt32(cmd.ExecuteScalar())                  ' Exécute et rend la première valeur du résultat : l'identifiant créé
                    End Using                         ' Libère la commande
                End Using                             ' Ferme la connexion
            Catch ex As MySqlException                ' Si MySQL signale une erreur (serveur arrêté, table absente...)
                Throw New InvalidOperationException("Impossible d'enregistrer l'alerte : " & ex.Message, ex)   ' On la transforme en message clair pour l'interface
            End Try                                   ' Fin du bloc surveillé
        End Function                                  ' Fin de Ajouter

        Public Function Rechercher(filtre As FiltreAlerte) As List(Of Alerte) Implements IAlerteRepository.Rechercher   ' Recherche avec filtres (EF-09)
            Dim sql As New StringBuilder(
                "SELECT a.id, a.id_produit, a.id_magasin, a.`type`, a.valeur, a.date_alerte, a.lue, a.detail, " &
                "p.nom AS nom_produit, m.nom AS nom_magasin " &
                "FROM alerte a " &
                "INNER JOIN produit p ON p.id = a.id_produit " &
                "LEFT JOIN magasin m ON m.id = a.id_magasin " &
                "WHERE 1 = 1")                        ' Début de la requête ; « WHERE 1 = 1 » est toujours vrai et simplifie l'ajout de « AND ... »
            Dim parametres As New List(Of MySqlParameter)()   ' Liste des paramètres à donner à la requête

            If filtre IsNot Nothing Then              ' Si un filtre est fourni, on ajoute les conditions demandées
                If filtre.DateDebut.HasValue Then     ' Une date de début est choisie
                    sql.Append(" AND a.date_alerte >= @debut")   ' Ajoute la condition « à partir de »
                    parametres.Add(New MySqlParameter("@debut", filtre.DateDebut.Value.Date))   ' Valeur du paramètre @debut
                End If                                ' Fin date de début
                If filtre.DateFin.HasValue Then       ' Une date de fin est choisie
                    sql.Append(" AND a.date_alerte <= @fin")     ' Ajoute la condition « jusqu'à »
                    parametres.Add(New MySqlParameter("@fin", filtre.DateFin.Value.Date))       ' Valeur du paramètre @fin
                End If                                ' Fin date de fin
                If filtre.IdMagasin.HasValue Then     ' Un magasin est choisi
                    sql.Append(" AND a.id_magasin = @magasin")   ' Ajoute la condition sur le magasin
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

        ' Ordre des colonnes : voir la requête de Rechercher.
        Private Shared Function LireAlerte(lecteur As MySqlDataReader) As Alerte   ' Transforme la ligne courante en objet Alerte
            Dim typeLu As TypeAlerte              ' Variable qui recevra le type lu en base
            If Not [Enum].TryParse(lecteur.GetString(3), True, typeLu) Then   ' Convertit le texte en TypeAlerte (True = sans tenir compte des majuscules)
                typeLu = TypeAlerte.Hausse        ' Texte inconnu : valeur par défaut
            End If                                ' Fin du contrôle

            Return New Alerte With {              ' Crée l'objet Alerte
                .Id = lecteur.GetInt32(0),        ' Colonne 0 : identifiant
                .IdProduit = lecteur.GetInt32(1), ' Colonne 1 : produit
                .IdMagasin = If(lecteur.IsDBNull(2), 0, lecteur.GetInt32(2)),   ' Colonne 2 : magasin (0 si la base contient NULL)
                .Type = typeLu,                   ' Colonne 3 : type déjà converti
                .Valeur = lecteur.GetDecimal(4),  ' Colonne 4 : variation
                .DateAlerte = lecteur.GetDateTime(5),   ' Colonne 5 : date
                .Lue = lecteur.GetBoolean(6),     ' Colonne 6 : lue ou non
                .Detail = If(lecteur.IsDBNull(7), String.Empty, lecteur.GetString(7)),   ' Colonne 7 : texte explicatif (vide si NULL)
                .NomProduit = lecteur.GetString(8),     ' Colonne 8 : nom du produit (jointure)
                .NomMagasin = If(lecteur.IsDBNull(9), String.Empty, lecteur.GetString(9))   ' Colonne 9 : nom du magasin (vide si NULL)
            }                                     ' Fin de l'initialisation
        End Function                              ' Fin de LireAlerte

        Public Sub MarquerCommeLue(idAlerte As Integer) Implements IAlerteRepository.MarquerCommeLue   ' Marque une alerte comme lue
            Try                                   ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion
                    cn.Open()                     ' Ouvre
                    Using cmd As New MySqlCommand("UPDATE alerte SET lue = 1 WHERE id = @id", cn)   ' Requête de mise à jour d'une seule alerte
                        cmd.Parameters.AddWithValue("@id", idAlerte)   ' Identifiant de l'alerte
                        cmd.ExecuteNonQuery()     ' Exécute une requête qui ne renvoie pas de lignes
                    End Using                     ' Libère la commande
                End Using                         ' Ferme la connexion
            Catch ex As MySqlException           ' Erreur MySQL
                Throw New InvalidOperationException("Impossible de marquer l'alerte comme lue : " & ex.Message, ex)   ' Message clair
            End Try                               ' Fin du bloc surveillé
        End Sub                                   ' Fin de MarquerCommeLue

        Public Sub MarquerToutesCommeLues() Implements IAlerteRepository.MarquerToutesCommeLues   ' Marque toutes les alertes comme lues
            Try                                   ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion
                    cn.Open()                     ' Ouvre
                    Using cmd As New MySqlCommand("UPDATE alerte SET lue = 1 WHERE lue = 0", cn)   ' Met à « lue » toutes celles qui ne le sont pas
                        cmd.ExecuteNonQuery()     ' Exécute la mise à jour
                    End Using                     ' Libère la commande
                End Using                         ' Ferme la connexion
            Catch ex As MySqlException           ' Erreur MySQL
                Throw New InvalidOperationException("Impossible de marquer les alertes comme lues : " & ex.Message, ex)   ' Message clair
            End Try                               ' Fin du bloc surveillé
        End Sub                                   ' Fin de MarquerToutesCommeLues

        Public Function CompterNonLues() As Integer Implements IAlerteRepository.CompterNonLues   ' Compte les alertes non lues
            Try                                   ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion
                    cn.Open()                     ' Ouvre
                    Using cmd As New MySqlCommand("SELECT COUNT(*) FROM alerte WHERE lue = 0", cn)   ' Requête de comptage
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
