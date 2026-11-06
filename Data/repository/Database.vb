Imports MySqlConnector

Module Database

    Public ReadOnly Property GetConnection As MySqlConnection
        Get
            Dim Builder As New MySqlConnectionStringBuilder With {
                .Server = "localhost",
                .UserID = "Destroyer",
                .Password = "Destroyer@28",
                .Database = "Pricieux"
                }
            Return New MySqlConnection(Builder.ToString)
        End Get
    End Property
End Module
