Option Strict On
Option Explicit On
Imports PreisRontgen.Models
Imports PreisRontgen.Services

''' <summary>Ligne en cours de saisie. Les valeurs calculées se mettent à jour quand une valeur change.</summary>
Public Class LigneTicketSaisie
    Public Property IdProduit As Integer
    Public Property NomProduit As String = ""
    Public Property UniteBaseProduit As String = "piece"
    Public Property Conditionnement As Decimal
    Public Property Unite As Unite = Unite.Piece
    Public Property Prix As Decimal
    Public Property NbAchete As Integer = 1
    Private normalisation As NormalisationService

    Public ReadOnly Property TotalLigne As Decimal
        Get
            Return Prix * NbAchete
        End Get
    End Property

    Public ReadOnly Property QuantiteBase As Decimal
        Get
            Return NormalisationService.ConvertirEnUniteBase(Conditionnement, Unite)
        End Get
    End Property

    Public ReadOnly Property PrixUnitaire As Decimal
        Get
            Return NormalisationService.CalculerPrixUnitaire(Prix, Conditionnement, Unite)
        End Get
    End Property

    Public NotInheritable Class Produit
        Public Property Id As Integer
        Public Property Nom As String = String.Empty
        Public Property Marque As String = String.Empty
        Public Property CategorieId As Long
        Public Property CategorieNom As String = String.Empty
        Public Property UniteBase As UniteBase
        Public Property CodeBarres As String = String.Empty
        Public ReadOnly Property Libelle As String
            Get
                Return ToString()
            End Get
        End Property
        Public Overrides Function ToString() As String
            If String.IsNullOrWhiteSpace(Marque) Then Return Nom
            Return Nom & " — " & Marque
        End Function
    End Class

    Public NotInheritable Class LigneOcr
        Public Property Produit As Produit
        Public Property PrixLu As Decimal
        Public Property TexteOriginal As String = String.Empty
        Public Property Confiance As Decimal
        Public Property QuantiteConditionnement As Decimal?
        Public Property UniteConditionnement As String = String.Empty
        Public Property NombreAchete As Decimal?
    End Class

End Class