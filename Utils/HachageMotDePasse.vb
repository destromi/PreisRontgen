Option Strict On
Option Explicit On

Imports System.Security.Cryptography

Public NotInheritable Class HachageMotDePasse
    Private Const NombreIterations As Integer = 310000
    Private Const TailleSel As Integer = 16
    Private Const TailleHachage As Integer = 32

    Private Sub New()
    End Sub

    Public Shared Function Creer(motDePasse As String) As (Sel As Byte(), Hachage As Byte())
        Dim sel(TailleSel - 1) As Byte
        RandomNumberGenerator.Fill(sel)
        Dim hachage As Byte() = Rfc2898DeriveBytes.Pbkdf2(motDePasse, sel, NombreIterations, HashAlgorithmName.SHA256, TailleHachage)
        Return (sel, hachage)
    End Function

    Public Shared Function Verifier(motDePasse As String, sel As Byte(), hachageAttendu As Byte()) As Boolean
        If sel Is Nothing OrElse hachageAttendu Is Nothing Then Return False
        Dim hachage As Byte() = Rfc2898DeriveBytes.Pbkdf2(motDePasse, sel, NombreIterations, HashAlgorithmName.SHA256, hachageAttendu.Length)
        Return CryptographicOperations.FixedTimeEquals(hachage, hachageAttendu)
    End Function
End Class
