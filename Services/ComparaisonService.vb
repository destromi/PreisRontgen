' ============================================================================
' FICHIER : Services/ComparaisonService.vb
' RÔLE    : EF-11, classement des magasins par prix unitaire (pour un produit ou une catégorie)
'           et EF-12, produits équivalents moins chers.
' RÈGLES  : - le prix d'un magasin est le DERNIER prix unitaire observé dans ce magasin ;
'           - produit équivalent = même catégorie ET même unité de base ; seul le prix
'             unitaire est comparé, sans jugement de qualité (cahier des charges §4.2) ;
'           - ces résultats sont une information tirée des relevés du foyer, pas un conseil.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Linq            ' Pour OrderBy, Where, Select...
Imports PreisRontgen.Data      ' Pour IAnalyseRepository
Imports PreisRontgen.Models    ' Pour LignePrixMagasin, Equivalent, ProduitRef

Namespace Services             ' Espace de noms : PreisRontgen.Services

    ''' <summary>Comparaison de magasins (EF-11) et produits équivalents (EF-12).</summary>
    Public Class ComparaisonService                  ' Début de la classe

        ''' <summary>Phrase affichée avec les suggestions : information et non conseil.</summary>
        Public Const Avertissement As String = "Information fondée sur vos propres relevés ; elle ne constitue pas un conseil d'achat."

        Private ReadOnly _depot As IAnalyseRepository   ' Accès aux données (reçu à la création)

        Public Sub New(depot As IAnalyseRepository)   ' Constructeur
            If depot Is Nothing Then Throw New ArgumentNullException(NameOf(depot))   ' Refuse un dépôt absent
            _depot = depot                            ' Mémorise le dépôt
        End Sub                                       ' Fin du constructeur

        ''' <summary>Classement des magasins pour un produit : du moins cher au plus cher.</summary>
        Public Function ComparerMagasins(idProduit As Integer) As List(Of LignePrixMagasin)   ' Appelé par le bouton « Comparer magasins »
            Return ClasserParPrix(_depot.ListerDernieresObservations(idProduit, Nothing))   ' Lit puis trie
        End Function                                  ' Fin de ComparerMagasins

        ''' <summary>Classement pour une catégorie : produit par produit, du moins cher au plus cher.</summary>
        Public Function ComparerCategorie(idCategorie As Integer) As List(Of LignePrixMagasin)   ' Variante par catégorie
            Return ClasserParProduit(_depot.ListerDernieresObservations(Nothing, idCategorie))   ' Lit puis trie
        End Function                                  ' Fin de ComparerCategorie

        ''' <summary>
        ''' Produits de la même catégorie et de la même unité de base dont le prix unitaire est inférieur
        ''' au dernier prix observé du produit choisi. Liste vide si le produit est inconnu ou sans observation.
        ''' </summary>
        Public Function TrouverEquivalents(idProduit As Integer) As List(Of Equivalent)   ' Appelé par le bouton « Voir équivalents moins chers »
            Dim produit As ProduitRef = _depot.ListerProduits().FirstOrDefault(Function(p) p.Id = idProduit)   ' Produit de départ
            If produit Is Nothing Then Return New List(Of Equivalent)()   ' Produit inconnu

            Dim siennes As List(Of LignePrixMagasin) = _depot.ListerDernieresObservations(idProduit, Nothing)   ' Ses propres observations
            If siennes.Count = 0 Then Return New List(Of Equivalent)()    ' Sans observation, pas de référence

            Dim reference As LignePrixMagasin = siennes.OrderByDescending(Function(l) l.DateObs).First()   ' Dernier prix observé = référence
            Dim candidats As List(Of LignePrixMagasin) = _depot.ListerDernieresObservations(Nothing, produit.IdCategorie)   ' Produits de la même catégorie
            Return FiltrerEquivalents(reference, candidats)   ' Garde ceux qui sont moins chers
        End Function                                 ' Fin de TrouverEquivalents

        ''' <summary>Du moins cher au plus cher ; à prix égal, par magasin.</summary>
        Public Shared Function ClasserParPrix(lignes As IEnumerable(Of LignePrixMagasin)) As List(Of LignePrixMagasin)   ' Classement des magasins
            If lignes Is Nothing Then Throw New ArgumentNullException(NameOf(lignes))   ' Refuse une liste absente
            Return lignes.OrderBy(Function(l) l.PrixUnitaire).ThenBy(Function(l) l.NomMagasin).ToList()   ' Prix croissants
        End Function                                 ' Fin de ClasserParPrix

        ''' <summary>Par produit (ordre alphabétique), puis du moins cher au plus cher.</summary>
        Public Shared Function ClasserParProduit(lignes As IEnumerable(Of LignePrixMagasin)) As List(Of LignePrixMagasin)   ' Classement d'une catégorie
            If lignes Is Nothing Then Throw New ArgumentNullException(NameOf(lignes))   ' Refuse une liste absente
            Return lignes.OrderBy(Function(l) l.NomProduit).ThenBy(Function(l) l.PrixUnitaire).ThenBy(Function(l) l.NomMagasin).ToList()   ' Regroupe par produit
        End Function                                 ' Fin de ClasserParProduit

        ''' <summary>
        ''' Garde, parmi les candidats (supposés de la même catégorie), les produits de même unité de base
        ''' dont le prix unitaire est strictement inférieur à celui de la référence. Le produit de référence
        ''' lui-même est exclu. Résultat trié du moins cher au plus cher.
        ''' </summary>
        Public Shared Function FiltrerEquivalents(reference As LignePrixMagasin,
                                                  candidats As IEnumerable(Of LignePrixMagasin)) As List(Of Equivalent)   ' Règle EF-12, sans base de données
            Dim resultat As New List(Of Equivalent)()    ' Résultat
            If reference Is Nothing OrElse candidats Is Nothing OrElse reference.PrixUnitaire <= 0D Then Return resultat   ' Rien à comparer

            Dim moinsChers As IEnumerable(Of LignePrixMagasin) = candidats.Where(Function(c) c.IdProduit <> reference.IdProduit AndAlso c.UniteBase = reference.UniteBase AndAlso c.PrixUnitaire < reference.PrixUnitaire)   ' Même unité, autre produit, prix inférieur
            For Each c As LignePrixMagasin In moinsChers.OrderBy(Function(x) x.PrixUnitaire).ThenBy(Function(x) x.NomProduit).ThenBy(Function(x) x.NomMagasin)   ' Du moins cher au plus cher
                resultat.Add(New Equivalent With {
                    .IdProduit = c.IdProduit,            ' Produit équivalent
                    .NomProduit = c.NomProduit,          ' Nom
                    .NomMagasin = c.NomMagasin,          ' Magasin observé
                    .PrixUnitaire = c.PrixUnitaire,      ' Prix unitaire
                    .UniteBase = c.UniteBase,            ' Unité de base
                    .EconomiePct = (reference.PrixUnitaire - c.PrixUnitaire) / reference.PrixUnitaire   ' Écart relatif avec la référence
                })                                       ' Ajoute la suggestion
            Next                                         ' Fin de la boucle
            Return resultat                              ' Rend les suggestions
        End Function                                     ' Fin de FiltrerEquivalents

    End Class                                            ' Fin de ComparaisonService

End Namespace                                            ' Fin de l'espace de noms Services
