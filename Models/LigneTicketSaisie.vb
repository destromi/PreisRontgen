Option Strict On
Option Explicit On

''' <summary>Ligne en cours de saisie. Les valeurs calculées se mettent à jour quand une valeur change.</summary>
Public Class LigneTicketSaisie
    Public Property IdProduit As Integer
    Public Property NomProduit As String = ""
    Public Property UniteBaseProduit As String = "piece"
    Public Property Conditionnement As Decimal
    Public Property Unite As String = "piece"
    Public Property Prix As Decimal
    Public Property NbAchete As Integer = 1

    Public ReadOnly Property TotalLigne As Decimal
        Get
            Return Prix * NbAchete
        End Get
    End Property

    Public ReadOnly Property QuantiteBase As Decimal
        Get
            Return NormalisationService.VersUniteBase(Conditionnement, Unite)
        End Get
    End Property

    Public ReadOnly Property PrixUnitaire As Decimal
        Get
            Return NormalisationService.PrixUnitaire(Prix, Conditionnement, Unite)
        End Get
    End Property
End Class