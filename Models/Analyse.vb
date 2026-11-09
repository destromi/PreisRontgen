Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Namespace Models      ' Espace de noms : PreisRontgen.Models

    ''' <summary>Produit tel que l'écran en a besoin : identifiant, nom affiché, catégorie et unité de base.</summary>
    Public Class ProduitRef                       ' Début de la classe
        Public Property Id As Integer             ' Identifiant du produit
        Public Property Nom As String = String.Empty   ' Nom affiché (nom + marque)
        Public Property IdCategorie As Integer    ' Catégorie du produit (sert aux produits équivalents)
        Public Property UniteBase As UniteBase    ' kg, L ou pièce
    End Class                                     ' Fin de ProduitRef

    ''' <summary>Une observation de prix affichée dans l'historique d'un produit (EF-06).</summary>
    Public Class PointPrix                        ' Début de la classe
        Public Property DateObs As Date           ' Date de l'observation (date du ticket)
        Public Property IdMagasin As Integer      ' Magasin concerné
        Public Property NomMagasin As String = String.Empty   ' Nom du magasin
        Public Property QuantiteBase As Decimal   ' Quantité du conditionnement, en unité de base (1 pour 1 L)
        Public Property PrixUnitaire As Decimal   ' Prix par kg, L ou pièce
        Public Property UniteBase As UniteBase    ' Unité de base du produit
    End Class                                     ' Fin de PointPrix

    ''' <summary>Résumé chiffré d'un historique (affiché sous le graphique).</summary>
    Public Class ResumeHistorique                 ' Début de la classe
        Public Property Nombre As Integer         ' Nombre d'observations
        Public Property Minimum As Decimal        ' Prix unitaire le plus bas
        Public Property Maximum As Decimal        ' Prix unitaire le plus haut
        Public Property Moyenne As Decimal        ' Prix unitaire moyen
        Public Property Premier As Decimal        ' Prix de l'observation la plus ancienne
        Public Property Dernier As Decimal        ' Prix de l'observation la plus récente
        Public Property VariationTotale As Decimal?   ' (Dernier - Premier) / Premier ; Nothing si Premier vaut 0
    End Class                                     ' Fin de ResumeHistorique

    ''' <summary>Dernier prix unitaire observé pour un produit dans un magasin (EF-11).</summary>
    Public Class LignePrixMagasin                 ' Début de la classe
        Public Property IdProduit As Integer      ' Produit concerné
        Public Property NomProduit As String = String.Empty   ' Nom du produit
        Public Property IdMagasin As Integer      ' Magasin concerné
        Public Property NomMagasin As String = String.Empty   ' Nom du magasin
        Public Property PrixUnitaire As Decimal   ' Dernier prix unitaire observé
        Public Property DateObs As Date           ' Date de cette observation
        Public Property UniteBase As UniteBase    ' Unité de base du produit
    End Class                                     ' Fin de LignePrixMagasin

    ''' <summary>Produit équivalent moins cher (EF-12) : même catégorie, même unité de base.</summary>
    Public Class Equivalent                       ' Début de la classe
        Public Property IdProduit As Integer      ' Produit équivalent
        Public Property NomProduit As String = String.Empty   ' Nom du produit équivalent
        Public Property NomMagasin As String = String.Empty   ' Magasin où ce prix a été observé
        Public Property PrixUnitaire As Decimal   ' Prix unitaire observé
        Public Property UniteBase As UniteBase    ' Unité de base (identique au produit de départ)
        Public Property EconomiePct As Decimal    ' Écart relatif avec le produit de départ (0,20 = 20 % moins cher)
    End Class                                     ' Fin de Equivalent

    ''' <summary>Quantité achetée d'un produit sur une période (sert à bâtir le panier de l'indice).</summary>
    Public Class AchatProduit                     ' Début de la classe
        Public Property IdProduit As Integer      ' Produit acheté
        Public Property NomProduit As String = String.Empty   ' Nom du produit
        Public Property QuantiteBase As Decimal   ' Quantité totale achetée, en unité de base
        Public Property NombreAchats As Integer   ' Nombre de tickets où le produit apparaît
    End Class                                     ' Fin de AchatProduit

    ''' <summary>Ligne du panier de référence : quantité de la période de départ et prix aux deux dates.</summary>
    Public Class LignePanier                      ' Début de la classe
        Public Property IdProduit As Integer      ' Produit du panier
        Public Property NomProduit As String = String.Empty   ' Nom du produit
        Public Property QuantiteBase As Decimal   ' Quantité de la période de départ (indice de Laspeyres)
        Public Property PrixDepart As Decimal     ' Prix unitaire à la date de départ
        Public Property PrixArrivee As Decimal    ' Prix unitaire à la date d'arrivée

        Public ReadOnly Property CoutDepart As Decimal    ' Coût de cette ligne aux prix de départ
            Get
                Return QuantiteBase * PrixDepart          ' Quantité x prix de départ
            End Get
        End Property

        Public ReadOnly Property CoutArrivee As Decimal   ' Coût de cette ligne aux prix d'arrivée
            Get
                Return QuantiteBase * PrixArrivee         ' Quantité x prix d'arrivée
            End Get
        End Property
    End Class                                     ' Fin de LignePanier

    ''' <summary>Résultat du calcul de l'indice personnel (EF-10).</summary>
    Public Class ResultatIndice                   ' Début de la classe
        Public Property Indice As Decimal?        ' Indice de base 100 ; Nothing si le calcul est impossible
        Public Property CoutDepart As Decimal     ' Coût du panier aux prix de départ
        Public Property CoutArrivee As Decimal    ' Coût du panier aux prix d'arrivée
        Public Property NombreProduits As Integer ' Nombre de produits retenus dans le panier
        Public Property Lignes As New List(Of LignePanier)()   ' Détail du panier
        Public Property Message As String = String.Empty       ' Explication quand le calcul est impossible

        ''' <summary>Fabrique un résultat « calcul impossible » accompagné d'une explication.</summary>
        Public Shared Function Echec(message As String) As ResultatIndice   ' Raccourci pratique
            Return New ResultatIndice With {.Indice = Nothing, .Message = message}   ' Pas d'indice, juste un message
        End Function                              ' Fin de Echec
    End Class                                     ' Fin de ResultatIndice

    ''' <summary>Fréquence d'une dépense récurrente (colonne periodicite de la table depense_recurrente).</summary>
    Public Enum Periodicite                       ' Début de l'énumération
        Hebdomadaire = 1                          ' Toutes les semaines
        Mensuelle = 2                             ' Tous les mois
        Trimestrielle = 3                         ' Tous les trois mois
        Annuelle = 4                              ' Tous les ans
    End Enum                                      ' Fin de Periodicite

    ''' <summary>Dépense récurrente : abonnement, loyer, énergie... (EF-14).</summary>
    Public Class DepenseRecurrente                ' Début de la classe
        Public Property Id As Integer             ' Identifiant en base (0 = pas encore enregistrée)
        Public Property Libelle As String = String.Empty   ' Nom de la dépense (Loyer, Électricité...)
        Public Property Montant As Decimal        ' Montant en FCFA
        Public Property Periodicite As Periodicite = Periodicite.Mensuelle   ' Fréquence
        Public Property ProchaineEcheance As Date = Date.Today   ' Prochaine date d'échéance
        Public Property Actif As Boolean = True   ' False = dépense désactivée (n'apparaît plus)
    End Class                                     ' Fin de DepenseRecurrente

    ''' <summary>Rappel d'échéance : une dépense bientôt due (ou déjà échue).</summary>
    Public Class RappelEcheance                   ' Début de la classe
        Public Property Depense As DepenseRecurrente = New DepenseRecurrente()   ' Dépense concernée
        Public Property JoursRestants As Integer  ' Jours avant l'échéance (0 = aujourd'hui, négatif = en retard)
    End Class                                     ' Fin de RappelEcheance

End Namespace                                     ' Fin de l'espace de noms Models
