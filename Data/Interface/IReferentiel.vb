Public Interface IReferentiel
    Sub saveMagasins(name As String)
    Function GetMagasin() As DataTable
    Sub saveCategorie(name As String)
    Function GetCategorie() As DataTable
End Interface
