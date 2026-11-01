
Option Strict On
Option Explicit On

Imports System.Globalization

Public Module Saisie

    Private ReadOnly Fr As CultureInfo =
        CultureInfo.GetCultureInfo("fr-FR")

    ''' <summary>
    ''' Convertit un texte en décimal.
    ''' Accepte la virgule ou le point.
    ''' </summary>
    Public Function TryDecimal(
        texte As String,
        ByRef valeur As Decimal
    ) As Boolean

        valeur = 0D

        If String.IsNullOrWhiteSpace(texte) Then
            Return False
        End If

        Dim s As String = texte.Trim().
            Replace(" ", "").
            Replace(ChrW(160), "").
            Replace(ChrW(&H202F), "").
            Replace("."c, ","c)

        Return Decimal.TryParse(s, NumberStyles.Number, Fr, valeur)

    End Function

    ''' <summary>
    ''' Vérifie un code-barres de 8, 12, 13 ou 14 chiffres.
    ''' </summary>
    Public Function CodeBarresValide(code As String) As Boolean

        If code Is Nothing Then Return False

        code = code.Trim()

        If code.Length <> 8 AndAlso
           code.Length <> 12 AndAlso
           code.Length <> 13 AndAlso
           code.Length <> 14 Then

            Return False
        End If

        For Each c As Char In code
            If c < "0"c OrElse c > "9"c Then
                Return False
            End If
        Next

        Dim somme As Integer = 0

        For i As Integer = 0 To code.Length - 2

            Dim chiffre As Integer =
                AscW(code(code.Length - 2 - i)) - 48

            somme += chiffre * If(i Mod 2 = 0, 3, 1)

        Next

        Dim cle As Integer = (10 - somme Mod 10) Mod 10

        Return cle = AscW(code(code.Length - 1)) - 48

    End Function

End Module
