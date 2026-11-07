Option Explicit On
Option Strict On

Imports MySqlConnector

Public Class TableauBordMySql
    Implements ITableauBord
    Dim sql As String
    Public Function DepenseTotal(id As Integer) As Decimal Implements ITableauBord.DepenseTotal
        Dim depense As Decimal
        Using connection As MySqlConnection = GetConnection
            If connection.State <> ConnectionState.Open Then
                connection.Open()
                sql = "select sum(montant) from depense_recurrente where id_utilisateur = @id;"
                Using SqlDepense As New MySqlCommand(sql, connection)
                    SqlDepense.Parameters.AddWithValue("@id", Session.user.Id)
                    Dim result As Object = SqlDepense.ExecuteScalar()
                    If result Is Nothing OrElse result Is DBNull.Value Then
                        depense = 0
                    Else
                        depense = Convert.ToInt32(result)
                    End If
                End Using
            End If
        End Using
        Return depense
    End Function

    Public Function AlerteNonLue(id As Integer) As Integer Implements ITableauBord.AlerteNonLue
        Dim nombrealerte As Integer
        Using connection As MySqlConnection = GetConnection
            If connection.State <> ConnectionState.Open Then
                connection.Open()
                sql = "Select count(lue) from alerte where id_utilisateur = @id and lue = false; "
                Using alerte As New MySqlCommand(sql, connection)
                    alerte.Parameters.AddWithValue("@id", id)
                    nombrealerte = Convert.ToInt32(alerte.ExecuteScalar)
                End Using
            End If
        End Using
        Return nombrealerte
    End Function

    'methode pour la recupèration des dépenses du mois en cours
    Public Function DepenseCategorie(id As Integer, maDate As Date) As DataTable Implements ITableauBord.DepenseCategorie
        Dim debut As New Date(maDate.Year, maDate.Month, 1)
        Dim fin As Date = debut.AddMonths(1)
        Dim depenseTotal As New DataTable
        Using connection As MySqlConnection = GetConnection
            sql = "select c.nom as categorie, sum(l.prix * l.nb_achete) as montant " &
            "from ligne_ticket l " &
            "inner join ticket t on l.id_ticket = t.id " &
            "inner join produit p on l.id_produit = p.id " &
            "inner join categorie c on c.id = p.id_categorie " &
            "where t.id_utilisateur = @id and t.date_achat >= @debut and t.date_achat < @fin " &
            "group by c.id, c.nom " &
            "order by montant desc;"

            Using depenseCmd As New MySqlCommand(sql, connection)
                depenseCmd.Parameters.AddWithValue("@id", id)
                depenseCmd.Parameters.AddWithValue("@debut", debut)
                depenseCmd.Parameters.AddWithValue("@fin", fin)

                Using adapter As New MySqlDataAdapter(depenseCmd)
                    adapter.Fill(depenseTotal)
                End Using
            End Using
        End Using
        Return depenseTotal
    End Function

    Public Function EcheanceTotal(id As Integer, MyDate As Date) As DataTable Implements ITableauBord.EcheanceTotal
        Dim echeance As New DataTable
        Dim fin As New Date(MyDate.Year, MyDate.AddMonths(1).Month, 1)
        'Dim fin As Date = debut.AddMonths(1)
        Using connection As MySqlConnection = GetConnection
            sql = "select libelle as Depense, montant, echeance from depense_recurrente where id_utilisateur = @id and echeance >= @MyDate and echeance < @fin;"
            Using echeanceCmd As New MySqlCommand(sql, connection)
                echeanceCmd.Parameters.AddWithValue("@id", id)
                echeanceCmd.Parameters.AddWithValue("@MyDate", MyDate)
                echeanceCmd.Parameters.AddWithValue("@fin", fin)
                Using echeanceAdapter As New MySqlDataAdapter(echeanceCmd)
                    echeanceAdapter.Fill(echeance)
                End Using
            End Using
        End Using
        Return echeance
    End Function
End Class
