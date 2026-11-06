' ============================================================================
' FICHIER : Models/Unites.vb
' RÔLE    : définit les deux listes d'unités utilisées par le module (EF-05).
'           - Unite     : l'unité écrite sur le ticket (g, kg, ml, cl, L, pièce)
'           - UniteBase : l'unité commune de comparaison (kg, L, pièce)
' NOTION  : une « Enum » est une liste de valeurs nommées. Au lieu d'écrire le
'           texte "kg" partout (risque de faute de frappe), on écrit Unite.Kilogramme.
' ============================================================================
Option Strict On      ' Interdit les conversions implicites dangereuses (ex. texte vers nombre) : exigé par le cahier des charges
Option Explicit On    ' Oblige à déclarer chaque variable avant de l'utiliser : exigé par le cahier des charges

Namespace Models      ' Regroupe les classes « données » du projet ; nom complet : PreisRontgen.Models

    ''' <summary>
    ''' Unité de base servant à comparer les prix (EF-05) :
    ''' prix au kilogramme, au litre ou à la pièce.
    ''' </summary>
    Public Enum UniteBase       ' Début de l'énumération ; Public = utilisable par tout le projet
        Kg = 1                  ' Produits pesés : le prix unitaire est exprimé par kilogramme
        Litre = 2               ' Liquides : le prix unitaire est exprimé par litre
        Piece = 3               ' Produits comptés : le prix unitaire est exprimé par pièce
    End Enum                    ' Fin de l'énumération UniteBase

    ''' <summary>
    ''' Unité dans laquelle le conditionnement est lu sur le ticket.
    ''' </summary>
    Public Enum Unite           ' Début de l'énumération des unités de saisie
        Gramme = 1              ' 1 g = 0,001 kg (famille « kilogramme »)
        Kilogramme = 2          ' Déjà une unité de base (famille « kilogramme »)
        Millilitre = 3          ' 1 ml = 0,001 L (famille « litre »)
        Centilitre = 4          ' 1 cl = 0,01 L (famille « litre »)
        Litre = 5               ' Déjà une unité de base (famille « litre »)
        Piece = 6               ' Produit compté à l'unité (famille « pièce »)
    End Enum                    ' Fin de l'énumération Unite

End Namespace                   ' Fin de l'espace de noms Models
