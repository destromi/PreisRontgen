Module Session
    Public Property user As Utilisateurs
    Public ReadOnly Property estConnecté As Boolean
        Get
            Return user IsNot Nothing
        End Get
    End Property
    Public Sub Deconnecter()
        user = Nothing
    End Sub
End Module