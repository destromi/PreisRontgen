' ============================================================================
' FICHIER : Data/ParametresDetectionMySql.vb
' RÔLE    : lit et enregistre les seuils de détection dans la table « parametre »
'           du schéma final du groupe (colonnes cle et valeur, la valeur étant un texte).
' ATTENTION AUX UNITÉS : la table stocke des POURCENTAGES (« 5 » et « 3 »),
'           alors que ParametresDetection utilise des FRACTIONS (0,05 et 0,03).
'           Cette classe divise par 100 à la lecture et multiplie par 100 à l'écriture.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Globalization   ' Pour lire et écrire les nombres sans dépendre de la langue de Windows
Imports MySqlConnector         ' Bibliothèque NuGet qui permet de parler à MySQL
Imports PreisRontgen.Models    ' Pour utiliser ParametresDetection

Namespace Data                 ' Espace de noms : PreisRontgen.Data

    Public Class ParametresDetectionMySql        ' Début de la classe

        Private Const CleSeuilHausse As String = "seuil_hausse_pct"                 ' Clé du seuil de hausse (schema.sql du chef)
        Private Const CleSeuilShrinkflation As String = "seuil_shrinkflation_pct"   ' Clé du seuil de shrinkflation (schema.sql du chef)

        Private ReadOnly _chaineConnexion As String   ' Texte de connexion à MySQL

        Public Sub New(chaineConnexion As String)     ' Constructeur
            If String.IsNullOrWhiteSpace(chaineConnexion) Then   ' Texte absent ou vide
                Throw New ArgumentException("La chaîne de connexion est obligatoire.", NameOf(chaineConnexion))   ' Refus explicite
            End If                                    ' Fin du contrôle
            _chaineConnexion = chaineConnexion        ' Mémorise la chaîne
        End Sub                                       ' Fin du constructeur

        ''' <summary>Lit les seuils en base ; une ligne absente ou illisible garde la valeur par défaut (5 % et 3 %).</summary>
        Public Function Charger() As ParametresDetection   ' Rend les seuils sous forme de fractions
            Dim resultat As New ParametresDetection()      ' Valeurs par défaut au départ
            Try                                            ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion (fermée automatiquement)
                    cn.Open()                              ' Ouvre
                    Using cmd As New MySqlCommand("SELECT cle, valeur FROM parametre WHERE cle IN (@h, @s)", cn)   ' Lit les deux seuils
                        cmd.Parameters.AddWithValue("@h", CleSeuilHausse)           ' Clé du seuil de hausse
                        cmd.Parameters.AddWithValue("@s", CleSeuilShrinkflation)    ' Clé du seuil de shrinkflation
                        Using lecteur As MySqlDataReader = cmd.ExecuteReader()      ' Exécute et lit les lignes
                            While lecteur.Read()           ' Pour chaque ligne trouvée
                                Dim pourcentage As Decimal ' Valeur en pourcentage (« 5 »)
                                Dim texte As String = lecteur.GetString(1).Trim().Replace(","c, "."c)   ' Texte lu, virgule remplacée par un point
                                If Decimal.TryParse(texte, NumberStyles.Number, CultureInfo.InvariantCulture, pourcentage) AndAlso pourcentage >= 0D Then   ' Nombre lisible et positif
                                    If lecteur.GetString(0) = CleSeuilHausse Then           ' C'est le seuil de hausse
                                        resultat.SeuilHausse = pourcentage / 100D           ' 5 devient 0,05
                                    Else                   ' C'est le seuil de shrinkflation
                                        resultat.SeuilShrinkflation = pourcentage / 100D    ' 3 devient 0,03
                                    End If                 ' Fin du choix
                                End If                     ' Valeur illisible : on garde la valeur par défaut
                            End While                      ' Fin de la boucle
                        End Using                          ' Libère le lecteur
                    End Using                              ' Libère la commande
                End Using                                  ' Ferme la connexion
            Catch ex As MySqlException                     ' Erreur MySQL
                Throw New InvalidOperationException("Impossible de lire les seuils : " & ex.Message, ex)   ' Message clair
            End Try                                        ' Fin du bloc surveillé
            Return resultat                                ' Rend les seuils
        End Function                                       ' Fin de Charger

        ''' <summary>Enregistre les seuils (fractions) en base sous forme de pourcentages écrits en texte.</summary>
        Public Sub Enregistrer(parametres As ParametresDetection)   ' Écrit les deux seuils
            If parametres Is Nothing Then Throw New ArgumentNullException(NameOf(parametres))   ' Refuse un objet absent
            Try                                            ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion
                    cn.Open()                              ' Ouvre
                    EcrireSeuil(cn, CleSeuilHausse, parametres.SeuilHausse * 100D)                   ' 0,05 devient « 5 »
                    EcrireSeuil(cn, CleSeuilShrinkflation, parametres.SeuilShrinkflation * 100D)    ' 0,03 devient « 3 »
                End Using                                  ' Ferme la connexion
            Catch ex As MySqlException                     ' Erreur MySQL
                Throw New InvalidOperationException("Impossible d'enregistrer les seuils : " & ex.Message, ex)   ' Message clair
            End Try                                        ' Fin du bloc surveillé
        End Sub                                            ' Fin de Enregistrer

        Private Shared Sub EcrireSeuil(cn As MySqlConnection, cle As String, pourcentage As Decimal)   ' Écrit une ligne de la table parametre
            Using cmd As New MySqlCommand(
                "INSERT INTO parametre (cle, valeur) VALUES (@cle, @valeur) ON DUPLICATE KEY UPDATE valeur = VALUES(valeur)", cn)   ' Crée la ligne ou la met à jour (cle est la clé primaire)
                cmd.Parameters.AddWithValue("@cle", cle)               ' Clé du seuil
                cmd.Parameters.AddWithValue("@valeur", Math.Round(pourcentage, 4).ToString("0.####", CultureInfo.InvariantCulture))   ' Pourcentage écrit en texte, par exemple « 5 »
                cmd.ExecuteNonQuery()                                  ' Exécute
            End Using                                                  ' Libère la commande
        End Sub                                                        ' Fin de EcrireSeuil

    End Class                                                          ' Fin de la classe ParametresDetectionMySql

End Namespace                                                          ' Fin de l'espace de noms Data
