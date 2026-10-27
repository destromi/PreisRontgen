Option Explicit On
Option Strict On
Imports System.Security.Cryptography

Public Class Hachage_Mot_de_passe
    Private Shared tailleSel As Integer = 16
    Private Shared tailleHash As Integer = 32
    Private Shared repetition As Integer = 1000000

    Public Shared Function PwdHash(pwd As String) As String
        Dim sel As Byte() = RandomNumberGenerator.GetBytes(tailleSel)
        Dim hash As Byte() = Rfc2898DeriveBytes.Pbkdf2(pwd, sel, repetition, HashAlgorithmName.SHA256, tailleHash)
        Return $"{repetition}.{Convert.ToBase64String(sel)}.{Convert.ToBase64String(hash)}"
    End Function

    Public Shared Function VerifPasd(pswd As String, hash As String) As Boolean
        Dim partie As String() = hash.Split(".")
        Dim interation As Integer = CType(partie(0), Integer)
        Dim sel As Byte() = Convert.FromBase64String(partie(1))
        Dim hashAttendu As Byte() = Convert.FromBase64String(partie(2))
        Dim hashCalculer As Byte() = Rfc2898DeriveBytes.Pbkdf2(pswd, sel, interation, HashAlgorithmName.SHA256, hashAttendu.Length)
        Return CryptographicOperations.FixedTimeEquals(hashAttendu, hashCalculer)
    End Function
End Class
