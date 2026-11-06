Public Interface ITableauBord
    Function DepenseTotal(id As Integer) As Decimal
    Function AlerteNonLue(id As Integer) As Integer
    Function DepenseCategorie(id As Integer, maDate As Date) As DataTable
    Function EcheanceTotal(id As Integer, MyDate As Date) As DataTable
End Interface
