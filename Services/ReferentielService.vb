Public Class ReferentielService
    Private referentielMysql As New ReferentielMySql()

    Public Function saveMagasins(name As String) As Integer
        Dim id As Integer = referentielMysql.saveMagasins(name)
        Return id
    End Function
    Public Function saveCategorie(name As String) As Integer
        Dim id As Integer = referentielMysql.saveCategorie(name)
        Return id
    End Function
    Public Function GetMagasin() As DataTable
        Return referentielMysql.GetMagasin()
    End Function
    Public Function GetCategorie() As DataTable
        Return referentielMysql.GetCategorie()
    End Function
    Public Sub DesactiverMagasin(id As Integer)
        referentielMysql.desactiveMagasins(id)
    End Sub
    Public Sub DesactiveCategorie(id As Integer)
        referentielMysql.desactiveCategorie(id)
    End Sub
End Class
