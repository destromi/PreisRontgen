Option Strict On
Option Explicit On
Public NotInheritable Class Utilisateurs

    Public Property Id As Integer
    Public Property Login As String = String.Empty
    Public Property Role As String = "Utilisateur"
    Public Property Actif As Boolean = True
    Public ReadOnly Property EstAdministrateur As Boolean
        Get
            Return String.Equals(Role, "Admin", StringComparison.OrdinalIgnoreCase)
        End Get
    End Property

End Class