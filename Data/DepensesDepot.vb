Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports MySqlConnector

Public NotInheritable Class DepensesDepot
    Public Async Function ListerAsync(utilisateurId As Long) As Task(Of List(Of DepenseRecurrente))
        Dim resultats As New List(Of DepenseRecurrente)()
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand("SELECT id, libelle, montant, periodicite, prochaine_echeance FROM depense_recurrente WHERE id_utilisateur=@utilisateur AND actif=TRUE ORDER BY prochaine_echeance;", connexion)
                commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    While Await lecteur.ReadAsync()
                        resultats.Add(New DepenseRecurrente With {
                            .Id = lecteur.GetInt64("id"), .Libelle = lecteur.GetString("libelle"),
                            .Montant = lecteur.GetDecimal("montant"), .Periodicite = lecteur.GetString("periodicite"),
                            .ProchaineEcheance = lecteur.GetDateTime("prochaine_echeance")
                        })
                    End While
                End Using
            End Using
        End Using
        Return resultats
    End Function

    Public Async Function AvancerEcheancesDepasseesAsync(utilisateurId As Long, dateReference As Date) As Task(Of List(Of DepenseRecurrente))
        Dim depenses As List(Of DepenseRecurrente) = Await ListerAsync(utilisateurId)
        Dim modifications As New List(Of (Depense As DepenseRecurrente, NouvelleEcheance As Date))()
        For Each depense As DepenseRecurrente In depenses
            Dim prochaine As Date = depense.ProchaineEcheance.Date
            If prochaine > dateReference.Date Then Continue For
            While prochaine <= dateReference.Date
                Dim suivante As Date = AjouterPeriode(prochaine, depense.Periodicite)
                If suivante <= prochaine Then Throw New InvalidOperationException("La périodicité doit produire une échéance ultérieure.")
                prochaine = suivante
            End While
            modifications.Add((depense, prochaine))
        Next

        Dim avancees As New List(Of DepenseRecurrente)()
        If modifications.Count = 0 Then Return avancees
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using transaction As MySqlTransaction = DirectCast(Await connexion.BeginTransactionAsync(), MySqlTransaction)
                For Each modification As (Depense As DepenseRecurrente, NouvelleEcheance As Date) In modifications
                    Const sql As String = "UPDATE depense_recurrente SET prochaine_echeance=@nouvelle WHERE id=@id AND id_utilisateur=@utilisateur AND prochaine_echeance=@ancienne AND actif=TRUE;"
                    Using commande As New MySqlCommand(sql, connexion, transaction)
                        commande.Parameters.AddWithValue("@nouvelle", modification.NouvelleEcheance)
                        commande.Parameters.AddWithValue("@id", modification.Depense.Id)
                        commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                        commande.Parameters.AddWithValue("@ancienne", modification.Depense.ProchaineEcheance.Date)
                        If Await commande.ExecuteNonQueryAsync() = 1 Then
                            modification.Depense.ProchaineEcheance = modification.NouvelleEcheance
                            avancees.Add(modification.Depense)
                        End If
                    End Using
                Next
                Await transaction.CommitAsync()
            End Using
        End Using
        Return avancees
    End Function

    Public Async Function EnregistrerAsync(utilisateurId As Long, depense As DepenseRecurrente) As Task
        If String.IsNullOrWhiteSpace(depense.Libelle) Then Throw New ArgumentException("Le libellé est obligatoire.")
        If depense.Montant < 0D Then Throw New ArgumentException("Le montant ne peut pas être négatif.")
        Dim sql As String = If(depense.Id > 0,
            "UPDATE depense_recurrente SET libelle=@libelle, montant=@montant, periodicite=@periodicite, prochaine_echeance=@echeance WHERE id=@id AND id_utilisateur=@utilisateur;",
            "INSERT INTO depense_recurrente (id_utilisateur, libelle, montant, periodicite, prochaine_echeance) VALUES (@utilisateur, @libelle, @montant, @periodicite, @echeance);")
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand(sql, connexion)
                commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                commande.Parameters.AddWithValue("@libelle", depense.Libelle.Trim())
                commande.Parameters.AddWithValue("@montant", depense.Montant)
                commande.Parameters.AddWithValue("@periodicite", depense.Periodicite)
                commande.Parameters.AddWithValue("@echeance", depense.ProchaineEcheance.Date)
                If depense.Id > 0 Then commande.Parameters.AddWithValue("@id", depense.Id)
                Await commande.ExecuteNonQueryAsync()
            End Using
        End Using
    End Function

    Public Async Function SupprimerAsync(utilisateurId As Long, id As Long) As Task
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand("UPDATE depense_recurrente SET actif=FALSE WHERE id=@id AND id_utilisateur=@utilisateur;", connexion)
                commande.Parameters.AddWithValue("@id", id)
                commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                Await commande.ExecuteNonQueryAsync()
            End Using
        End Using
    End Function

    Private Shared Function AjouterPeriode(dateEcheance As Date, periodicite As String) As Date
        Select Case If(periodicite, String.Empty).Trim().ToUpperInvariant()
            Case "HEBDOMADAIRE"
                Return dateEcheance.AddDays(7)
            Case "MENSUELLE"
                Return dateEcheance.AddMonths(1)
            Case "TRIMESTRIELLE"
                Return dateEcheance.AddMonths(3)
            Case "ANNUELLE"
                Return dateEcheance.AddYears(1)
            Case Else
                Throw New InvalidOperationException("La périodicité de la dépense récurrente n'est pas reconnue.")
        End Select
    End Function

    Public Async Function LireSeuilsAsync() As Task(Of (Hausse As Decimal, Shrinkflation As Decimal))
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand("SELECT cle, valeur FROM parametre WHERE cle IN ('seuil_hausse_pct','seuil_shrinkflation_pct');", connexion)
                Dim hausse As Decimal = 5D
                Dim shrink As Decimal = 3D
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    While Await lecteur.ReadAsync()
                        Dim valeur As Decimal
                        If Decimal.TryParse(lecteur.GetString("valeur"), Globalization.NumberStyles.Number, Globalization.CultureInfo.InvariantCulture, valeur) Then
                            If lecteur.GetString("cle") = "seuil_hausse_pct" Then hausse = valeur
                            If lecteur.GetString("cle") = "seuil_shrinkflation_pct" Then shrink = valeur
                        End If
                    End While
                End Using
                Return (hausse, shrink)
            End Using
        End Using
    End Function

    Public Async Function EnregistrerSeuilsAsync(seuilHausse As Decimal, seuilShrink As Decimal) As Task
        If seuilHausse < 0D OrElse seuilHausse > 100D OrElse seuilShrink < 0D OrElse seuilShrink > 100D Then Throw New ArgumentOutOfRangeException("Les seuils doivent être compris entre 0 et 100 %.")
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using transaction As MySqlTransaction = DirectCast(Await connexion.BeginTransactionAsync(), MySqlTransaction)
                For Each item As (Cle As String, Valeur As Decimal) In {("seuil_hausse_pct", seuilHausse), ("seuil_shrinkflation_pct", seuilShrink)}
                    Using commande As New MySqlCommand("INSERT INTO parametre (cle, valeur) VALUES (@cle, @valeur) ON DUPLICATE KEY UPDATE valeur=VALUES(valeur);", connexion, transaction)
                        commande.Parameters.AddWithValue("@cle", item.Cle)
                        commande.Parameters.AddWithValue("@valeur", item.Valeur.ToString(Globalization.CultureInfo.InvariantCulture))
                        Await commande.ExecuteNonQueryAsync()
                    End Using
                Next
                Await transaction.CommitAsync()
            End Using
        End Using
    End Function
End Class
