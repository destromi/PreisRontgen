' ============================================================================
' FICHIER : Models/ParametresDetection.vb
' RÔLE    : regroupe les deux seuils de la détection. Ils sont modifiables par
'           l'administrateur (table parametre, module de T1).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Namespace Models      ' Espace de noms : PreisRontgen.Models

    ''' <summary>
    ''' Seuils de détection. Les valeurs par défaut sont celles du cahier des charges (§4.2).
    ''' L'administrateur peut les modifier (table parametre, module de T1).
    ''' Les seuils sont des fractions : 0,05 = 5 %.
    ''' </summary>
    Public Class ParametresDetection   ' Début de la classe

        Public Const SeuilHausseParDefaut As Decimal = 0.05D           ' Constante : 5 % (le suffixe D signifie « Decimal »)
        Public Const SeuilShrinkflationParDefaut As Decimal = 0.03D    ' Constante : 3 % (valeur du cahier des charges)

        ''' <summary>Hausse du prix unitaire à dépasser (strictement) pour déclencher une alerte.</summary>
        Public Property SeuilHausse As Decimal = SeuilHausseParDefaut  ' Seuil actuel, initialisé à 5 % mais modifiable

        ''' <summary>Baisse minimale du conditionnement (au moins égale) pour déclencher une alerte de shrinkflation.</summary>
        Public Property SeuilShrinkflation As Decimal = SeuilShrinkflationParDefaut   ' Seuil actuel, initialisé à 3 % mais modifiable

    End Class                          ' Fin de la classe ParametresDetection

End Namespace                          ' Fin de l'espace de noms Models
