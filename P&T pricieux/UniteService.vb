
Option Strict On
Option Explicit On

Imports System.Text.RegularExpressions

Public Module UniteService

    Public ReadOnly UnitesBase As String() = {"kg", "L", "pièce"}

    ''' <summary>Retourne les unités compatibles avec l'unité de base.</summary>
    Public Function UnitesPour(uniteBase As String) As String()

        Select Case uniteBase
            Case "kg"
                Return New String() {"g", "kg"}

            Case "L"
                Return New String() {"ml", "cl", "L"}

            Case "pièce"
                Return New String() {"pièce"}

            Case Else
                Return New String() {}
        End Select

    End Function

    Public Function UniteBaseDe(unite As String) As String

        Select Case unite.Trim().ToLowerInvariant()
            Case "g", "kg"
                Return "kg"

            Case "ml", "cl", "l"
                Return "L"

            Case "pièce", "piece"
                Return "pièce"

            Case Else
                Throw New ArgumentException("Unité inconnue : " & unite)
        End Select

    End Function

    Public Function VersBase(quantite As Decimal, unite As String) As Decimal

        Dim q As Decimal

        Select Case unite.Trim().ToLowerInvariant()
            Case "g"
                q = quantite / 1000D

            Case "kg"
                q = quantite

            Case "ml"
                q = quantite / 1000D

            Case "cl"
                q = quantite / 100D

            Case "l"
                q = quantite

            Case "pièce", "piece"
                q = quantite

            Case Else
                Throw New ArgumentException("Unité inconnue : " & unite)
        End Select

        Return Math.Round(q, 6)

    End Function

    ''' <summary>Calcule le prix par unité de base.</summary>
    Public Function PrixUnitaire(prix As Decimal, quantiteBase As Decimal) As Decimal

        If quantiteBase <= 0D Then Return 0D

        Return Math.Round(prix / quantiteBase, 4)

    End Function

    ''' <summary>Déduit l'unité de base depuis la quantité Open Food Facts.</summary>
    Public Function DeduireUniteBase(quantite As String) As String

        If String.IsNullOrWhiteSpace(quantite) Then Return ""

        Dim m As Match = Regex.Match(
            quantite,
            "(\d+(?:[.,]\d+)?)\s*(kg|g|ml|cl|l)",
            RegexOptions.IgnoreCase
        )

        If Not m.Success Then Return ""

        Select Case m.Groups(2).Value.ToLowerInvariant()
            Case "kg", "g"
                Return "kg"

            Case Else
                Return "L"
        End Select

    End Function

End Module
