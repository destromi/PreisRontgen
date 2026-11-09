Public Interface IReferentiel
    Function saveMagasins(name As String) As Integer
    Function GetMagasin() As DataTable
    Function saveCategorie(name As String) As Integer
    Function GetCategorie() As DataTable
End Interface
