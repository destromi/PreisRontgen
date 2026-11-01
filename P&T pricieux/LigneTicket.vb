
Option Strict On
Option Explicit On

Public Class LigneTicket

    Public Property IdProduit As Integer

    Public Property LibelleProduit As String = ""

    Public Property Nombre As Integer = 1

    Public Property QuantiteConditionnement As Decimal

    Public Property Unite As String = ""

    ''' <summary>Prix d'un conditionnement.</summary>
    Public Property Prix As Decimal

    Public ReadOnly Property TotalLigne As Decimal
        Get
            Return Math.Round(Prix * Nombre, 2)
        End Get
    End Property

    Public ReadOnly Property QuantiteBase As Decimal
        Get
            Return UniteService.VersBase(QuantiteConditionnement, Unite)
        End Get
    End Property

    Public ReadOnly Property PrixUnitaire As Decimal
        Get
            Return UniteService.PrixUnitaire(Prix, QuantiteBase)
        End Get
    End Property

End Class
