
Option Strict On
Option Explicit On

Public Module NormalisationService

    Public ReadOnly UnitesSaisie As String() =
        {"g", "kg", "ml", "cl", "L", "piece"}

    ''' <summary>
    ''' Retourne l'unité de base d'un produit.
    ''' </summary>
    Public Function UniteBaseDe(unite As String) As String

        Select Case If(unite, "").Trim().ToLowerInvariant()

            Case "g", "kg"
                Return "kg"

            Case "ml", "cl", "l"
                Return "L"

            Case "piece"
                Return "piece"

            Case Else
                Throw New ArgumentException(
                    "Unité inconnue : " & unite)

        End Select

    End Function

    ''' <summary>
    ''' Convertit une quantité vers son unité de base.
    ''' </summary>
    Public Function VersUniteBase(
        quantite As Decimal,
        unite As String
    ) As Decimal

        If quantite <= 0D Then
            Throw New ArgumentException(
                "La quantité doit être positive.")
        End If

        Select Case If(unite, "").Trim().ToLowerInvariant()

            Case "g"
                Return quantite / 1000D

            Case "kg"
                Return quantite

            Case "ml"
                Return quantite / 1000D

            Case "cl"
                Return quantite / 100D

            Case "l"
                Return quantite

            Case "piece"
                Return quantite

            Case Else
                Throw New ArgumentException(
                    "Unité inconnue : " & unite)

        End Select

    End Function

    ''' <summary>
    ''' Calcule le prix par unité de base.
    ''' </summary>
    Public Function PrixUnitaire(
        prix As Decimal,
        conditionnement As Decimal,
        unite As String
    ) As Decimal

        If prix < 0D Then
            Throw New ArgumentException(
                "Le prix ne peut pas être négatif.")
        End If

        Dim quantiteBase As Decimal =
            VersUniteBase(conditionnement, unite)

        If quantiteBase <= 0D Then
            Throw New ArgumentException(
                "Quantité de base invalide.")
        End If

        Return Math.Round(
            prix / quantiteBase,
            4,
            MidpointRounding.AwayFromZero)

    End Function

End Module
