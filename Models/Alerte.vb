' ============================================================================
' FICHIER : Models/Alerte.vb
' RÔLE    : contient trois types :
'           - TypeAlerte  : les deux sortes d'alerte (Hausse, Shrinkflation)
'           - Alerte      : une alerte produite par la détection (EF-07, EF-08)
'           - FiltreAlerte: les critères de recherche de la liste d'alertes (EF-09)
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Namespace Models      ' Espace de noms : PreisRontgen.Models

    ''' <summary>Type d'alerte (colonne type de la table alerte).</summary>
    Public Enum TypeAlerte        ' Énumération des types d'alerte
        Hausse = 1                ' Le prix unitaire a trop augmenté (EF-07)
        Shrinkflation = 2         ' La quantité a baissé sans baisse de prix (EF-08)
    End Enum                      ' Fin de TypeAlerte

    ''' <summary>
    ''' Alerte générée par la détection (EF-07 et EF-08).
    ''' </summary>
    Public Class Alerte           ' Début de la classe Alerte

        Public Property Id As Integer          ' Identifiant en base, attribué à l'enregistrement

        Public Property IdProduit As Integer   ' Produit concerné par l'alerte

        ''' <summary>
        ''' Observation de prix à l'origine de l'alerte (colonne id_observation de la table alerte).
        ''' L'observation doit déjà être enregistrée (Id &gt; 0) avant l'analyse.
        ''' </summary>
        Public Property IdObservation As Integer   ' Clé étrangère vers observation_prix ; le magasin de l'alerte se déduit de cette observation

        Public Property IdMagasin As Integer   ' Magasin concerné par l'alerte

        Public Property Type As TypeAlerte     ' Hausse ou Shrinkflation

        ''' <summary>
        ''' Variation relative du prix unitaire, en fraction (0,05 = +5 %).
        ''' Pour une shrinkflation, c'est la hausse réelle du prix unitaire.
        ''' </summary>
        Public Property Valeur As Decimal      ' Stockée en fraction ; l'écran la formate en pourcentage

        ''' <summary>Date de l'observation qui a déclenché l'alerte.</summary>
        Public Property DateAlerte As Date     ' Reprend la date du ticket (utile pour filtrer par période)

        Public Property Lue As Boolean         ' False = non lue (nouvelle), True = déjà consultée

        ''' <summary>Texte explicatif affiché dans la liste des alertes.</summary>
        Public Property Detail As String = String.Empty   ' Valeur initiale : texte vide (jamais Nothing)

        ' --- Champs de lecture seule, remplis par la jointure lors d'une recherche (non enregistrés) ---

        Public Property NomProduit As String = String.Empty   ' Nom du produit, lu dans la table produit lors de l'affichage

        Public Property NomMagasin As String = String.Empty   ' Nom du magasin, lu dans la table magasin lors de l'affichage

    End Class                  ' Fin de la classe Alerte

    ''' <summary>
    ''' Critères de filtrage de la liste des alertes (EF-09).
    ''' Une valeur Nothing signifie « pas de filtre sur ce critère ».
    ''' </summary>
    Public Class FiltreAlerte  ' Début de la classe FiltreAlerte

        Public Property DateDebut As Date?     ' « Date? » = date facultative (peut valoir Nothing = aucun filtre)

        Public Property DateFin As Date?       ' Fin de la période, facultative aussi

        Public Property IdMagasin As Integer?  ' Magasin choisi ; Nothing = tous les magasins

        Public Property Type As TypeAlerte?    ' Type choisi ; Nothing = tous les types

        ''' <summary>True = alertes lues, False = non lues, Nothing = toutes.</summary>
        Public Property Lue As Boolean?        ' Trois états possibles grâce au « ? » : True, False, Nothing

    End Class                  ' Fin de la classe FiltreAlerte

End Namespace                  ' Fin de l'espace de noms Models
