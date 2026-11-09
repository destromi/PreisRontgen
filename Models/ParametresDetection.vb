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
        Public Const SeuilHausseCumuleeParDefaut As Decimal = 0.1D     ' Constante : 10 % sur la période (amélioration : plusieurs petites hausses de suite)
        Public Const FenetreCumuleeMoisParDefaut As Integer = 3        ' Constante : la période de comparaison dure 3 mois
        Public Const FacteurPrixInhabituelParDefaut As Decimal = 3D    ' Constante : un prix 3 fois trop haut ou trop bas est signalé
        Public Const MinObservationsPrixInhabituel As Integer = 3      ' Constante : il faut au moins 3 observations pour juger qu'un prix est inhabituel

        ''' <summary>Hausse du prix unitaire à dépasser (strictement) pour déclencher une alerte.</summary>
        Public Property SeuilHausse As Decimal = SeuilHausseParDefaut  ' Seuil actuel, initialisé à 5 % mais modifiable

        ''' <summary>Baisse minimale du conditionnement (au moins égale) pour déclencher une alerte de shrinkflation.</summary>
        Public Property SeuilShrinkflation As Decimal = SeuilShrinkflationParDefaut   ' Seuil actuel, initialisé à 3 % mais modifiable

        ''' <summary>
        ''' Active la hausse cumulée (amélioration facultative, absente du cahier des charges).
        ''' Désactivée par défaut : seule la règle du cahier (deux observations consécutives, seuil 5 %) s'applique.
        ''' </summary>
        Public Property HausseCumuleeActive As Boolean = False   ' False par défaut : on respecte strictement le cahier des charges

        ''' <summary>Hausse totale du prix unitaire, sur la période, à dépasser pour une alerte de hausse cumulée.</summary>
        Public Property SeuilHausseCumulee As Decimal = SeuilHausseCumuleeParDefaut   ' Seuil de la hausse cumulée (10 % par défaut)

        ''' <summary>Durée en mois de la période de la hausse cumulée.</summary>
        Public Property FenetreCumuleeMois As Integer = FenetreCumuleeMoisParDefaut   ' Période de comparaison (3 mois par défaut)

        ''' <summary>Un prix unitaire plus de N fois supérieur (ou N fois inférieur) au prix habituel est signalé comme inhabituel.</summary>
        Public Property FacteurPrixInhabituel As Decimal = FacteurPrixInhabituelParDefaut   ' Facteur du contrôle de prix inhabituel (3 par défaut)

    End Class                          ' Fin de la classe ParametresDetection

End Namespace                          ' Fin de l'espace de noms Models
