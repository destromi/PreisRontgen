
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class TicketRepository

    Public Function ChargerMagasins() As DataTable

        Dim table As New DataTable()

        Using connexion As MySqlConnection = ConnexionBD.ObtenirConnexion()

            Dim requete As String =
                "SELECT id, nom FROM magasin ORDER BY nom"

            Using commande As New MySqlCommand(requete, connexion)
                Using adaptateur As New MySqlDataAdapter(commande)
                    adaptateur.Fill(table)
                End Using
            End Using

        End Using

        Return table

    End Function

End Class
