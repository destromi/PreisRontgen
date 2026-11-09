Option Explicit On
Option Strict On
Imports MySqlConnector

Public Class ReferentielMySql
    Implements IReferentiel

    Private sql As String
    Public Function saveMagasins(name As String) As Integer Implements IReferentiel.saveMagasins
        Dim id As Integer
        Try
            Using connection As MySqlConnection = GetConnection
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                    sql = "insert into magasin (nom) values (@nom);"
                    Using insertion As New MySqlCommand(sql, connection)
                        insertion.Parameters.AddWithValue("@nom", name)
                        insertion.ExecuteNonQuery()
                        id = CInt(insertion.LastInsertedId)
                    End Using
                End If
            End Using
            Return id
        Catch ex As MySqlException
            Dim reponse As DialogResult = MessageBox.Show("Ce magasin est désactivé. Voulez-vous le réactiver ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reponse = DialogResult.Yes Then
                ' Réactiver le magasin
                activeMagasins(name)
            End If
        End Try
    End Function

    Public Function saveCategorie(name As String) As Integer Implements IReferentiel.saveCategorie
        Dim id As Integer
        Try
            Using connection As MySqlConnection = GetConnection
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                    sql = "insert into categorie (nom) values (@nom);"
                    Using insertion As New MySqlCommand(sql, connection)
                        insertion.Parameters.AddWithValue("@nom", name)
                        insertion.ExecuteNonQuery()
                        id = CInt(insertion.LastInsertedId)
                    End Using
                End If
            End Using
        Catch ex As MySqlException
            Dim reponse As DialogResult = MessageBox.Show("Cette catégorie est désactivé. Voulez-vous le réactiver  ? " & vbCrLf, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reponse = DialogResult.Yes Then
                'Reactive la catégorie
                activeCategorie(name)
            End If
        End Try
        Return id
    End Function

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
    Public Sub desactiveMagasins(id As Integer) Implements IReferentiel.desactiveMagasins
        Try
            Using connection As MySqlConnection = GetConnection
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                    sql = "Update magasin set actif = 0 where id = @id;"
                    Using cmd As New MySqlCommand(sql, connection)
                        cmd.Parameters.AddWithValue("@id", id)
                        cmd.ExecuteNonQuery()
                    End Using
                End If
            End Using
        Catch ex As MySqlException
            Throw New InvalidOperationException("Erreur lors de la désactivation du magasin : " & ex.Message, ex)
        End Try
    End Sub

    Public Sub activeMagasins(nom As String) Implements IReferentiel.activeMagasins
        Try
            Using connection As MySqlConnection = GetConnection
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                    sql = "Update magasin set actif = 1 where nom = @nom;"
                    Using cmd As New MySqlCommand(sql, connection)
                        cmd.Parameters.AddWithValue("@nom", nom)
                        cmd.ExecuteNonQuery()
                    End Using
                End If
            End Using
        Catch ex As MySqlException
            Throw New InvalidOperationException("Erreur lors de l'activation du magasin : " & ex.Message, ex)
        End Try
    End Sub

    Public Sub desactiveCategorie(id As Integer) Implements IReferentiel.desactiverCategorie
        Try
            Using connection As MySqlConnection = GetConnection
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                    sql = "Update categorie set actif = 0 where id = @id;"
                    Using cmd As New MySqlCommand(sql, connection)
                        cmd.Parameters.AddWithValue("@id", id)
                        cmd.ExecuteNonQuery()
                    End Using
                End If
            End Using
        Catch ex As MySqlException
            Throw New InvalidOperationException("Erreur lors de la désactivation de la catégorie : " & ex.Message, ex)
        End Try
    End Sub

    Public Sub activeCategorie(nom As String) Implements IReferentiel.activeCategorie
        Try
            Using connection As MySqlConnection = GetConnection
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                    sql = "Update categorie set actif = 1 where nom = @nom;"
                    Using cmd As New MySqlCommand(sql, connection)
                        cmd.Parameters.AddWithValue("@nom", nom)
                        cmd.ExecuteNonQuery()
                    End Using
                End If
            End Using
        Catch ex As MySqlException
            Throw New InvalidOperationException("Erreur lors de l'activation du magasin : " & ex.Message, ex)
        End Try
    End Sub

End Class
