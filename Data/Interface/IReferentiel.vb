Public Interface IReferentiel
    Function saveMagasins(name As String) As Integer
    Function GetMagasin() As DataTable
    Function saveCategorie(name As String) As Integer
    Function GetCategorie() As DataTable
    Sub desactiveMagasins(id As Integer)
    Sub desactiverCategorie(id As Integer)
    Sub activeMagasins(nom As String)
    Sub activeCategorie(nom As String)

End Interface
