' ============================================================================
' FICHIER : Data/DepenseRepositoryMySql.vb
' RÔLE    : implémentation MySQL de IDepenseRepository (table depense_recurrente
'           du schéma final). Chaque requête est limitée à l'utilisateur connecté.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports MySqlConnector         ' Bibliothèque pour parler à MySQL
Imports PreisRontgen.Models    ' Pour DepenseRecurrente et Periodicite

Namespace Data                 ' Espace de noms : PreisRontgen.Data

    ''' <summary>Lecture et écriture des dépenses récurrentes dans MySQL.</summary>
    Public Class DepenseRepositoryMySql          ' Début de la classe
        Implements IDepenseRepository            ' S'engage à fournir Lister, Ajouter, Modifier, Desactiver

        Private ReadOnly _chaineConnexion As String   ' Texte de connexion à MySQL
        Private ReadOnly _idUtilisateur As Integer    ' Utilisateur connecté

        Public Sub New(chaineConnexion As String, idUtilisateur As Integer)   ' Constructeur
            If String.IsNullOrWhiteSpace(chaineConnexion) Then   ' Texte absent
                Throw New ArgumentException("La chaîne de connexion est obligatoire.", NameOf(chaineConnexion))   ' Refus explicite
            End If                                    ' Fin du contrôle
            If idUtilisateur <= 0 Then                ' Identifiant invalide
                Throw New ArgumentOutOfRangeException(NameOf(idUtilisateur), "L'identifiant de l'utilisateur doit être positif.")   ' Refus explicite
            End If                                    ' Fin du contrôle
            _chaineConnexion = chaineConnexion        ' Mémorise la chaîne
            _idUtilisateur = idUtilisateur            ' Mémorise l'utilisateur
        End Sub                                       ' Fin du constructeur

        ''' <summary>Texte stocké dans la colonne ENUM periodicite.</summary>
        Public Shared Function VersTexte(p As Periodicite) As String   ' Énumération vers base
            Select Case p                             ' Aiguillage
                Case Periodicite.Hebdomadaire : Return "HEBDOMADAIRE"   ' Chaque semaine
                Case Periodicite.Trimestrielle : Return "TRIMESTRIELLE" ' Chaque trimestre
                Case Periodicite.Annuelle : Return "ANNUELLE"           ' Chaque année
                Case Else : Return "MENSUELLE"                          ' Chaque mois
            End Select                                ' Fin de l'aiguillage
        End Function                                  ' Fin de VersTexte

        ''' <summary>Lit la colonne ENUM periodicite.</summary>
        Public Shared Function DepuisTexte(texte As String) As Periodicite   ' Base vers énumération
            Select Case If(texte, String.Empty).Trim().ToUpperInvariant()   ' Texte normalisé
                Case "HEBDOMADAIRE" : Return Periodicite.Hebdomadaire       ' Chaque semaine
                Case "TRIMESTRIELLE" : Return Periodicite.Trimestrielle     ' Chaque trimestre
                Case "ANNUELLE" : Return Periodicite.Annuelle               ' Chaque année
                Case Else : Return Periodicite.Mensuelle                    ' Chaque mois (valeur par défaut)
            End Select                                ' Fin de l'aiguillage
        End Function                                  ' Fin de DepuisTexte

        Public Function Lister() As List(Of DepenseRecurrente) Implements IDepenseRepository.Lister   ' Dépenses actives
            Const sql As String =
                "SELECT id, libelle, montant, periodicite, prochaine_echeance, actif " &
                "FROM depense_recurrente WHERE id_utilisateur = @utilisateur AND actif = 1 " &
                "ORDER BY prochaine_echeance, libelle"   ' Échéance la plus proche d'abord
            Dim resultat As New List(Of DepenseRecurrente)()   ' Résultat
            Try                                       ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion
                    cn.Open()                         ' Ouvre
                    Using cmd As New MySqlCommand(sql, cn)          ' Commande
                        cmd.Parameters.AddWithValue("@utilisateur", _idUtilisateur)   ' Utilisateur
                        Using lecteur As MySqlDataReader = cmd.ExecuteReader()        ' Exécute
                            While lecteur.Read()                                      ' Pour chaque ligne
                                resultat.Add(New DepenseRecurrente With {
                                    .Id = lecteur.GetInt32(0),                       ' Identifiant
                                    .Libelle = lecteur.GetString(1),                 ' Nom
                                    .Montant = lecteur.GetDecimal(2),                ' Montant
                                    .Periodicite = DepuisTexte(lecteur.GetString(3)), ' Fréquence
                                    .ProchaineEcheance = lecteur.GetDateTime(4),     ' Échéance
                                    .Actif = lecteur.GetBoolean(5)                   ' Active
                                })                                                    ' Ajoute la dépense
                            End While                                                 ' Fin de la boucle
                        End Using                                                     ' Libère le lecteur
                    End Using                                                         ' Libère la commande
                End Using                                                             ' Ferme la connexion
            Catch ex As MySqlException                                                ' Erreur MySQL
                Throw New InvalidOperationException("Impossible de lire les dépenses récurrentes : " & ex.Message, ex)   ' Message clair
            End Try                                                                   ' Fin du bloc surveillé
            Return resultat                           ' Rend la liste
        End Function                                  ' Fin de Lister

        Public Function Ajouter(depense As DepenseRecurrente) As Integer Implements IDepenseRepository.Ajouter   ' Nouvelle dépense
            Const sql As String =
                "INSERT INTO depense_recurrente (id_utilisateur, libelle, montant, periodicite, prochaine_echeance, actif) " &
                "VALUES (@utilisateur, @libelle, @montant, @periodicite, @echeance, 1); SELECT LAST_INSERT_ID();"   ' Insère puis lit l'identifiant créé
            Try                                       ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion
                    cn.Open()                         ' Ouvre
                    Using cmd As New MySqlCommand(sql, cn)          ' Commande
                        cmd.Parameters.AddWithValue("@utilisateur", _idUtilisateur)                  ' Utilisateur
                        cmd.Parameters.AddWithValue("@libelle", depense.Libelle)                     ' Nom
                        cmd.Parameters.AddWithValue("@montant", depense.Montant)                     ' Montant
                        cmd.Parameters.AddWithValue("@periodicite", VersTexte(depense.Periodicite))  ' Fréquence
                        cmd.Parameters.AddWithValue("@echeance", depense.ProchaineEcheance.Date)     ' Échéance
                        Return Convert.ToInt32(cmd.ExecuteScalar())                                  ' Identifiant créé
                    End Using                                                                        ' Libère la commande
                End Using                                                                            ' Ferme la connexion
            Catch ex As MySqlException                                                               ' Erreur MySQL
                Throw New InvalidOperationException("Impossible d'enregistrer la dépense : " & ex.Message, ex)   ' Message clair
            End Try                                                                                  ' Fin du bloc surveillé
        End Function                                  ' Fin de Ajouter

        Public Sub Modifier(depense As DepenseRecurrente) Implements IDepenseRepository.Modifier   ' Mise à jour
            Const sql As String =
                "UPDATE depense_recurrente SET libelle = @libelle, montant = @montant, periodicite = @periodicite, " &
                "prochaine_echeance = @echeance WHERE id = @id AND id_utilisateur = @utilisateur"   ' Uniquement une dépense de l'utilisateur
            Try                                       ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion
                    cn.Open()                         ' Ouvre
                    Using cmd As New MySqlCommand(sql, cn)          ' Commande
                        cmd.Parameters.AddWithValue("@libelle", depense.Libelle)                     ' Nom
                        cmd.Parameters.AddWithValue("@montant", depense.Montant)                     ' Montant
                        cmd.Parameters.AddWithValue("@periodicite", VersTexte(depense.Periodicite))  ' Fréquence
                        cmd.Parameters.AddWithValue("@echeance", depense.ProchaineEcheance.Date)     ' Échéance
                        cmd.Parameters.AddWithValue("@id", depense.Id)                               ' Dépense visée
                        cmd.Parameters.AddWithValue("@utilisateur", _idUtilisateur)                  ' Utilisateur
                        cmd.ExecuteNonQuery()                                                        ' Exécute
                    End Using                                                                        ' Libère la commande
                End Using                                                                            ' Ferme la connexion
            Catch ex As MySqlException                                                               ' Erreur MySQL
                Throw New InvalidOperationException("Impossible de modifier la dépense : " & ex.Message, ex)   ' Message clair
            End Try                                                                                  ' Fin du bloc surveillé
        End Sub                                       ' Fin de Modifier

        Public Sub Desactiver(idDepense As Integer) Implements IDepenseRepository.Desactiver   ' Désactivation
            Const sql As String =
                "UPDATE depense_recurrente SET actif = 0 WHERE id = @id AND id_utilisateur = @utilisateur"   ' Pas de suppression
            Try                                       ' Début du bloc surveillé
                Using cn As New MySqlConnection(_chaineConnexion)   ' Connexion
                    cn.Open()                         ' Ouvre
                    Using cmd As New MySqlCommand(sql, cn)          ' Commande
                        cmd.Parameters.AddWithValue("@id", idDepense)                ' Dépense visée
                        cmd.Parameters.AddWithValue("@utilisateur", _idUtilisateur)  ' Utilisateur
                        cmd.ExecuteNonQuery()                                        ' Exécute
                    End Using                                                        ' Libère la commande
                End Using                                                            ' Ferme la connexion
            Catch ex As MySqlException                                               ' Erreur MySQL
                Throw New InvalidOperationException("Impossible de désactiver la dépense : " & ex.Message, ex)   ' Message clair
            End Try                                                                  ' Fin du bloc surveillé
        End Sub                                       ' Fin de Desactiver

    End Class                                         ' Fin de DepenseRepositoryMySql

End Namespace                                         ' Fin de l'espace de noms Data
