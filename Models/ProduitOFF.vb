
Option Strict On
Option Explicit On

''' <summary>
''' Représente un produit récupéré depuis Open Food Facts.
''' </summary>
Public Class ProduitOFF

    Public Property CodeBarres As String = String.Empty

    Public Property Nom As String = String.Empty

    Public Property Marque As String = String.Empty

    Public Property Categories As String = String.Empty

    Public Property Quantite As String = String.Empty

    Public Property ImageUrl As String = String.Empty

    Public Property EstTrouve As Boolean = False

End Class
