Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports PreisRontgen.Models    ' Pour ProduitRef, PointPrix, LignePrixMagasin, AchatProduit, DepenseRecurrente

Namespace Data                 ' Espace de noms de l'accès aux données : PreisRontgen.Data

    ''' <summary>
    ''' Lectures nécessaires à l'historique (EF-06), à l'indice personnel (EF-10),
    ''' à la comparaison de magasins (EF-11) et aux produits équivalents (EF-12).
    ''' Toutes les lectures sont limitées aux tickets de l'utilisateur connecté.
    ''' </summary>
    Public Interface IAnalyseRepository       ' Contrat de lecture pour les analyses

        ''' <summary>Produits actifs, avec leur catégorie et leur unité de base.</summary>
        Function ListerProduits() As List(Of ProduitRef)   ' Remplit les listes de produits

        ''' <summary>Magasins actifs (identifiant, nom).</summary>
        Function ListerMagasins() As List(Of KeyValuePair(Of Integer, String))   ' Remplit les listes de magasins

        ''' <summary>Catégories actives (identifiant, nom).</summary>
        Function ListerCategories() As List(Of KeyValuePair(Of Integer, String))   ' Remplit la liste des catégories

        ''' <summary>
        ''' Observations d'un produit, de la plus récente à la plus ancienne ;
        ''' idMagasin = Nothing signifie « tous les magasins ».
        ''' </summary>
        Function ListerObservations(idProduit As Integer, idMagasin As Integer?) As List(Of PointPrix)   ' Historique d'un produit

        ''' <summary>
        ''' Dernière observation de chaque couple produit/magasin. On peut restreindre à un produit
        ''' (idProduit) ou à une catégorie (idCategorie) ; Nothing signifie « pas de restriction ».
        ''' </summary>
        Function ListerDernieresObservations(idProduit As Integer?, idCategorie As Integer?) As List(Of LignePrixMagasin)   ' Base des comparaisons

        ''' <summary>
        ''' Quantités achetées par produit entre deux dates incluses (tickets du foyer),
        ''' avec le nombre de tickets où chaque produit apparaît.
        ''' </summary>
        Function ListerAchats(debut As Date, fin As Date) As List(Of AchatProduit)   ' Base du panier de référence

        ''' <summary>
        ''' Dernier prix unitaire observé pour chaque produit demandé, à la date indiquée ou avant.
        ''' Un produit sans observation n'apparaît pas dans le dictionnaire.
        ''' </summary>
        Function ObtenirPrixALaDate(idsProduits As IEnumerable(Of Integer), dateLimite As Date) As Dictionary(Of Integer, Decimal)   ' Prix de départ et d'arrivée

    End Interface                             ' Fin de IAnalyseRepository

    ''' <summary>Accès aux dépenses récurrentes (EF-14) de l'utilisateur connecté.</summary>
    Public Interface IDepenseRepository       ' Contrat d'accès aux dépenses récurrentes

        ''' <summary>Dépenses actives, de l'échéance la plus proche à la plus lointaine.</summary>
        Function Lister() As List(Of DepenseRecurrente)   ' Remplit la grille

        ''' <summary>Enregistre une nouvelle dépense et retourne son identifiant.</summary>
        Function Ajouter(depense As DepenseRecurrente) As Integer   ' Insère la dépense

        ''' <summary>Modifie une dépense existante.</summary>
        Sub Modifier(depense As DepenseRecurrente)        ' Met à jour la dépense

        ''' <summary>Désactive une dépense (elle n'est pas supprimée).</summary>
        Sub Desactiver(idDepense As Integer)              ' Passe actif à faux

    End Interface                             ' Fin de IDepenseRepository

End Namespace                                 ' Fin de l'espace de noms Data
