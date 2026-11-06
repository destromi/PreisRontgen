Option Explicit On
Option Strict On
Imports MySqlConnector

Public Class ReferentielMySql
    Implements IReferentiel

    Private sql As String
    Public Sub saveMagasins(name As String) Implements IReferentiel.saveMagasins
        Using connection As MySqlConnection = GetConnection
            If connection.State <> ConnectionState.Open Then
                connection.Open()
                sql = "insert into magasin (nom) values (@nom);"
                Using insertion As New MySqlCommand(sql, connection)
                    insertion.Parameters.AddWithValue("@nom", name)
                    insertion.ExecuteNonQuery()
                End Using
            End If

        End Using
    End Sub

    Public Sub saveCategorie(name As String) Implements IReferentiel.saveCategorie
        Using connection As MySqlConnection = GetConnection
            If connection.State <> ConnectionState.Open Then
                connection.Open()
                sql = "insert into categorie (nom) values (@nom);"
                Using insertion As New MySqlCommand(sql, connection)
                    insertion.Parameters.AddWithValue("@nom", name)
                    insertion.ExecuteNonQuery()
                End Using
            End If

        End Using
    End Sub

    Public Function GetMagasin() As DataTable Implements IReferentiel.GetMagasin
        Dim Magasin As New DataTable
        Using connection As MySqlConnection = GetConnection
            sql = "select nom from magasin;"
            Using recup As New MySqlCommand(sql, connection)
                Using DA As New MySqlDataAdapter(recup)
                    DA.Fill(Magasin)
                End Using
            End Using
        End Using
        Return Magasin
    End Function

    Public Function GetCategorie() As DataTable Implements IReferentiel.GetCategorie
        Dim Categorie As New DataTable
        Using connection As MySqlConnection = GetConnection
            sql = "select nom from categorie;"
            Using recup As New MySqlCommand(sql, connection)
                Using DA As New MySqlDataAdapter(recup)
                    DA.Fill(Categorie)
                End Using
            End Using
        End Using
        Return Categorie
    End Function
End Class
