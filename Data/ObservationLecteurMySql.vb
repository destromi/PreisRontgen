' ============================================================================
' FICHIER : Data/ObservationLecteurMySql.vb
' RÔLE    : implémentation MySQL de IObservationLecteur. Retrouve, dans la table
'           observation_prix, la dernière observation du même produit dans le même
'           magasin. La détection la compare ensuite à la nouvelle observation.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports MySqlConnector         ' Bibliothèque pour parler à MySQL
Imports PreisRontgen.Models    ' Pour utiliser Observation

Namespace Data                 ' Espace de noms : PreisRontgen.Data

    ''' <summary>
    ''' Lit dans observation_prix l'observation précédente d'un produit dans un magasin.
    ''' L'écriture des observations reste à la charge de la saisie de ticket (T2).
    ''' </summary>
    Public Class ObservationLecteurMySql         ' Début de la classe
        Implements IObservationLecteur           ' S'engage à fournir ObtenirPrecedente

        Private ReadOnly _chaineConnexion As String   ' Texte de connexion à MySQL

        Public Sub New(chaineConnexion As String)     ' Constructeur
            If String.IsNullOrWhiteSpace(chaineConnexion) Then   ' Texte absent ou vide
                Throw New ArgumentException("La chaîne de connexion est obligatoire.", NameOf(chaineConnexion))   ' Refus explicite
            End If                                    ' Fin du contrôle
            _chaineConnexion = chaineConnexion        ' Mémorise la chaîne
        End Sub                                       ' Fin du constructeur

        Public Function ObtenirPrecedente(observation As Observation) As Observation Implements IObservationLecteur.ObtenirPrecedente   ' Cherche l'observation précédente
            Const sql As String =
                "SELECT id, id_produit, id_magasin, date_obs, prix_unitaire, quantite_base, prix_total " &
                "FROM observation_prix " &
                "WHERE id_produit = @produit AND id_magasin = @magasin " &
                "AND date_obs <= @date AND id <> @id " &
                "ORDER BY date_obs DESC, id DESC LIMIT 1"   ' Même produit, même magasin, date antérieure ou égale, différente de l'observation elle-même ; on garde la plus récente
            Try                                       ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion (fermée automatiquement)
                    cn.Open()                         ' Ouvre
                    Using cmd As New MySqlCommand(sql, cn)          ' Commande SQL
                        cmd.Parameters.AddWithValue("@produit", observation.IdProduit)       ' Produit cherché
                        cmd.Parameters.AddWithValue("@magasin", observation.IdMagasin)       ' Magasin cherché
                        cmd.Parameters.AddWithValue("@date", observation.DateObs.Date)       ' Date de la nouvelle observation
                        cmd.Parameters.AddWithValue("@id", observation.Id)                   ' Exclut l'observation elle-même
                        Using lecteur As MySqlDataReader = cmd.ExecuteReader()               ' Exécute et obtient le lecteur
                            If Not lecteur.Read() Then                                        ' Aucune ligne trouvée
                                Return Nothing                                                ' Pas d'observation précédente
                            End If                                                            ' Fin du contrôle
                            Return New Observation With {                                     ' Convertit la ligne trouvée en objet
                                .Id = lecteur.GetInt32(0),            ' Colonne 0 : identifiant
                                .IdProduit = lecteur.GetInt32(1),     ' Colonne 1 : produit
                                .IdMagasin = lecteur.GetInt32(2),     ' Colonne 2 : magasin
                                .DateObs = lecteur.GetDateTime(3),    ' Colonne 3 : date
                                .PrixUnitaire = lecteur.GetDecimal(4),   ' Colonne 4 : prix unitaire
                                .QuantiteBase = lecteur.GetDecimal(5),   ' Colonne 5 : quantité en unité de base
                                .PrixTotal = lecteur.GetDecimal(6)       ' Colonne 6 : prix de l'article
                            }                                     ' Fin de l'initialisation
                        End Using                                 ' Libère le lecteur
                    End Using                                     ' Libère la commande
                End Using                                         ' Ferme la connexion
            Catch ex As MySqlException                            ' Erreur MySQL
                Throw New InvalidOperationException("Impossible de lire l'observation précédente : " & ex.Message, ex)   ' Message clair
            End Try                                               ' Fin du bloc surveillé
        End Function                                              ' Fin de ObtenirPrecedente

    End Class                                                     ' Fin de la classe ObservationLecteurMySql

End Namespace                                                     ' Fin de l'espace de noms Data
