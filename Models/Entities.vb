Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic

Public Enum UniteProduit
    Kilogramme
    Litre
    Piece
End Enum

Public NotInheritable Class UtilisateurSession
    Public Property Id As Long
    Public Property Login As String = String.Empty
    Public Property Role As String = "UTILISATEUR"
    Public Property Actif As Boolean = True
    Public ReadOnly Property EstAdministrateur As Boolean
        Get
            Return String.Equals(Role, "ADMIN", StringComparison.OrdinalIgnoreCase)
        End Get
    End Property
End Class

Public NotInheritable Class Magasin
    Public Property Id As Long
    Public Property Nom As String = String.Empty
    Public Overrides Function ToString() As String
        Return Nom
    End Function
End Class

Public NotInheritable Class Categorie
    Public Property Id As Long
    Public Property Nom As String = String.Empty
    Public Overrides Function ToString() As String
        Return Nom
    End Function
End Class

Public NotInheritable Class Produit
    Public Property Id As Long
    Public Property Nom As String = String.Empty
    Public Property Marque As String = String.Empty
    Public Property CategorieId As Long
    Public Property CategorieNom As String = String.Empty
    Public Property UniteBase As UniteProduit
    Public Property CodeBarres As String = String.Empty
    Public ReadOnly Property Libelle As String
        Get
            Return ToString()
        End Get
    End Property
    Public Overrides Function ToString() As String
        If String.IsNullOrWhiteSpace(Marque) Then Return Nom
        Return Nom & " — " & Marque
    End Function
End Class

Public NotInheritable Class LigneTicket
    Public Property ProduitId As Long
    Public Property ProduitNom As String = String.Empty
    Public Property Marque As String = String.Empty
    Public Property Conditionnement As Decimal
    Public Property Unite As String = String.Empty
    Public Property Prix As Decimal
    Public Property NombreAchete As Decimal
    Public Property QuantiteBaseTotale As Decimal
    Public Property PrixUnitaire As Decimal
    Public ReadOnly Property Total As Decimal
        Get
            Return Decimal.Round(Prix * NombreAchete, 2, MidpointRounding.AwayFromZero)
        End Get
    End Property
End Class

Public NotInheritable Class ObservationPrix
    Public Property Id As Long
    Public Property ProduitId As Long
    Public Property ProduitNom As String = String.Empty
    Public Property Marque As String = String.Empty
    Public Property CategorieNom As String = String.Empty
    Public Property UniteBase As String = String.Empty
    Public Property MagasinId As Long
    Public Property MagasinNom As String = String.Empty
    Public Property TicketId As Long
    Public Property DateObservation As Date
    Public Property PrixUnitaire As Decimal
    Public Property QuantiteBase As Decimal
    Public Property PrixConditionnement As Decimal
End Class

Public NotInheritable Class AlertePrix
    Public Property Id As Long
    Public Property ProduitNom As String = String.Empty
    Public Property Marque As String = String.Empty
    Public Property MagasinNom As String = String.Empty
    Public Property Type As String = String.Empty
    Public Property Valeur As Decimal
    Public Property PrixPrecedent As Decimal
    Public Property PrixActuel As Decimal
    Public Property DateAlerte As Date
    Public Property Lue As Boolean
End Class

Public NotInheritable Class DepenseRecurrente
    Public Property Id As Long
    Public Property Libelle As String = String.Empty
    Public Property Montant As Decimal
    Public Property Periodicite As String = "MENSUELLE"
    Public Property ProchaineEcheance As Date
End Class

Public NotInheritable Class PrixMagasin
    Public Property MagasinNom As String = String.Empty
    Public Property ProduitNom As String = String.Empty
    Public Property UniteBase As String = String.Empty
    Public Property PrixUnitaire As Decimal
    Public Property DateObservation As Date
End Class

Public NotInheritable Class ProduitEquivalent
    Public Property ProduitNom As String = String.Empty
    Public Property Marque As String = String.Empty
    Public Property MagasinNom As String = String.Empty
    Public Property UniteBase As String = String.Empty
    Public Property PrixUnitaire As Decimal
End Class

Public NotInheritable Class DepenseCategorie
    Public Property CategorieNom As String = String.Empty
    Public Property Montant As Decimal
End Class

Public NotInheritable Class LigneImportee
    Public Property DateAchat As Date
    Public Property Magasin As String = String.Empty
    Public Property Produit As String = String.Empty
    Public Property Marque As String = String.Empty
    Public Property Categorie As String = String.Empty
    Public Property UniteBase As String = String.Empty
    Public Property Conditionnement As Decimal
    Public Property Unite As String = String.Empty
    Public Property Prix As Decimal
    Public Property Nombre As Decimal
End Class

Public NotInheritable Class ResultatImport
    Public Property LignesImportees As Integer
    Public Property Erreurs As New List(Of String)()
End Class

Public NotInheritable Class LigneOcr
    Public Property Produit As Produit
    Public Property PrixLu As Decimal
    Public Property TexteOriginal As String = String.Empty
    Public Property Confiance As Decimal
    Public Property QuantiteConditionnement As Decimal?
    Public Property UniteConditionnement As String = String.Empty
    Public Property NombreAchete As Decimal?
End Class
