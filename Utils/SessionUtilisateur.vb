
Option Strict On
Option Explicit On

Public Module SessionUtilisateur

    Private _idUtilisateur As Integer
    Private _login As String = String.Empty
    Private _role As String = String.Empty

    Public ReadOnly Property IdUtilisateur As Integer
        Get
            Return _idUtilisateur
        End Get
    End Property

    Public ReadOnly Property Login As String
        Get
            Return _login
        End Get
    End Property

    Public ReadOnly Property Role As String
        Get
            Return _role
        End Get
    End Property

    Public ReadOnly Property EstConnecte As Boolean
        Get
            Return _idUtilisateur > 0
        End Get
    End Property

    Public ReadOnly Property EstAdmin As Boolean
        Get
            Return String.Equals(
                _role,
                "ADMIN",
                StringComparison.OrdinalIgnoreCase)
        End Get
    End Property

    Public Sub OuvrirSession(
        id As Integer,
        loginUtilisateur As String,
        roleUtilisateur As String)

        If id <= 0 Then
            Throw New ArgumentException(
                "Identifiant utilisateur invalide.")
        End If

        If String.IsNullOrWhiteSpace(loginUtilisateur) Then
            Throw New ArgumentException(
                "Login invalide.")
        End If

        If roleUtilisateur <> "ADMIN" AndAlso
           roleUtilisateur <> "UTILISATEUR" Then

            Throw New ArgumentException(
                "Rôle invalide.")

        End If

        _idUtilisateur = id
        _login = loginUtilisateur
        _role = roleUtilisateur

    End Sub

    Public Sub FermerSession()

        _idUtilisateur = 0
        _login = String.Empty
        _role = String.Empty

    End Sub

End Module
