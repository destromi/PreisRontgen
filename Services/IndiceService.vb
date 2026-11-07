' ============================================================================
' FICHIER : Services/IndiceService.vb
' RÔLE    : EF-10, indice de prix personnel de base 100 (indice de Laspeyres).
' RÈGLE   : (cahier des charges §4.2) rapport entre le coût du panier de référence aux
'           prix de la date d'arrivée et son coût aux prix de la date de départ, avec
'           les quantités de la période de départ, multiplié par 100.
'           Panier de référence = produits les plus achetés pendant les 30 jours qui
'           précèdent la date de départ (maquette : « panier des 30 jours avant la
'           date de départ »). Prix à une date = dernier prix unitaire observé ce jour-là ou avant.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Linq            ' Pour OrderBy, Where, Sum...
Imports PreisRontgen.Data      ' Pour IAnalyseRepository
Imports PreisRontgen.Models    ' Pour AchatProduit, LignePanier, ResultatIndice

Namespace Services             ' Espace de noms : PreisRontgen.Services

    ''' <summary>Calcul de l'indice de prix personnel (EF-10).</summary>
    Public Class IndiceService                       ' Début de la classe

        ''' <summary>Durée de la période qui définit le panier, avant la date de départ.</summary>
        Public Const JoursPanier As Integer = 30      ' 30 jours, comme la maquette

        ''' <summary>Nombre maximal de produits dans le panier (les plus achetés).</summary>
        Public Const NombreMaxProduits As Integer = 20   ' « Produits les plus achetés » (EF-10)

        ''' <summary>Rappel affiché avec l'indice (maquette) : l'indice n'est pas un conseil d'achat.</summary>
        Public Const Avertissement As String = "L'indice décrit les relevés de l'utilisateur et ne constitue pas un conseil d'achat."

        Private ReadOnly _depot As IAnalyseRepository   ' Accès aux données (reçu à la création)

        Public Sub New(depot As IAnalyseRepository)   ' Constructeur
            If depot Is Nothing Then Throw New ArgumentNullException(NameOf(depot))   ' Refuse un dépôt absent
            _depot = depot                            ' Mémorise le dépôt
        End Sub                                       ' Fin du constructeur

        ''' <summary>
        ''' Calcule l'indice entre deux dates. Si le calcul est impossible (dates inversées, aucun achat,
        ''' aucun prix aux deux dates), le résultat contient Indice = Nothing et un message explicatif.
        ''' </summary>
        Public Function Calculer(dateDepart As Date, dateArrivee As Date) As ResultatIndice   ' Point d'entrée appelé par l'écran
            If dateArrivee.Date <= dateDepart.Date Then   ' Les dates sont dans le mauvais ordre
                Return ResultatIndice.Echec("La date d'arrivée doit être postérieure à la date de départ.")   ' Message pour l'écran
            End If                                    ' Fin du contrôle

            Dim achats As List(Of AchatProduit) = _depot.ListerAchats(dateDepart.Date.AddDays(-JoursPanier), dateDepart.Date)   ' Achats des 30 jours avant le départ
            If achats.Count = 0 Then                  ' Aucun achat : pas de panier
                Return ResultatIndice.Echec($"Aucun achat enregistré durant les {JoursPanier} jours qui précèdent la date de départ.")   ' Message pour l'écran
            End If                                    ' Fin du contrôle

            Dim panier As List(Of AchatProduit) = ChoisirPanier(achats, NombreMaxProduits)   ' Garde les produits les plus achetés
            Dim ids As List(Of Integer) = panier.Select(Function(a) a.IdProduit).ToList()    ' Identifiants des produits du panier
            Dim prixDepart As Dictionary(Of Integer, Decimal) = _depot.ObtenirPrixALaDate(ids, dateDepart.Date)       ' Prix au départ
            Dim prixArrivee As Dictionary(Of Integer, Decimal) = _depot.ObtenirPrixALaDate(ids, dateArrivee.Date)     ' Prix à l'arrivée
            Return CalculerIndice(Assembler(panier, prixDepart, prixArrivee))   ' Calcule l'indice
        End Function                                 ' Fin de Calculer

        ''' <summary>Garde les produits les plus achetés (nombre de tickets, puis quantité), au plus « maximum ».</summary>
        Public Shared Function ChoisirPanier(achats As IEnumerable(Of AchatProduit), maximum As Integer) As List(Of AchatProduit)   ' Constitution du panier
            If achats Is Nothing Then Throw New ArgumentNullException(NameOf(achats))   ' Refuse une liste absente
            Dim achetes As IEnumerable(Of AchatProduit) = achats.Where(Function(a) a.QuantiteBase > 0D)   ' Quantités positives seulement
            Dim classes As IEnumerable(Of AchatProduit) = achetes.OrderByDescending(Function(a) a.NombreAchats).ThenByDescending(Function(a) a.QuantiteBase).ThenBy(Function(a) a.NomProduit)   ' Les plus achetés d'abord
            Return classes.Take(Math.Max(0, maximum)).ToList()   ' Puis on coupe
        End Function                                 ' Fin de ChoisirPanier

        ''' <summary>
        ''' Associe à chaque produit du panier ses prix de départ et d'arrivée.
        ''' Un produit sans prix à l'une des deux dates est écarté du panier.
        ''' </summary>
        Public Shared Function Assembler(panier As IEnumerable(Of AchatProduit),
                                         prixDepart As IDictionary(Of Integer, Decimal),
                                         prixArrivee As IDictionary(Of Integer, Decimal)) As List(Of LignePanier)   ' Construit les lignes du panier
            Dim lignes As New List(Of LignePanier)()      ' Résultat
            For Each achat As AchatProduit In panier      ' Pour chaque produit du panier
                Dim depart As Decimal                     ' Prix de départ
                Dim arrivee As Decimal                    ' Prix d'arrivée
                If prixDepart.TryGetValue(achat.IdProduit, depart) AndAlso prixArrivee.TryGetValue(achat.IdProduit, arrivee) Then   ' Les deux prix existent
                    lignes.Add(New LignePanier With {
                        .IdProduit = achat.IdProduit,     ' Produit
                        .NomProduit = achat.NomProduit,   ' Nom
                        .QuantiteBase = achat.QuantiteBase,   ' Quantité de la période de départ
                        .PrixDepart = depart,             ' Prix au départ
                        .PrixArrivee = arrivee            ' Prix à l'arrivée
                    })                                    ' Ajoute la ligne
                End If                                    ' Sinon : produit écarté
            Next                                          ' Fin de la boucle
            Return lignes                                 ' Rend les lignes
        End Function                                      ' Fin de Assembler

        ''' <summary>
        ''' Indice de Laspeyres = 100 x (somme des quantités x prix d'arrivée) / (somme des quantités x prix de départ),
        ''' arrondi à deux décimales. Les lignes à quantité ou prix nul sont ignorées.
        ''' </summary>
        Public Shared Function CalculerIndice(lignes As IEnumerable(Of LignePanier)) As ResultatIndice   ' Cœur du calcul, sans base de données
            If lignes Is Nothing Then Throw New ArgumentNullException(NameOf(lignes))   ' Refuse une liste absente
            Dim valides As List(Of LignePanier) = lignes.Where(Function(l) l.QuantiteBase > 0D AndAlso l.PrixDepart > 0D AndAlso l.PrixArrivee > 0D).ToList()   ' Lignes exploitables
            If valides.Count = 0 Then                     ' Rien à calculer
                Return ResultatIndice.Echec("Aucun produit du panier n'a de prix aux deux dates.")   ' Message pour l'écran
            End If                                        ' Fin du contrôle

            Dim coutDepart As Decimal = valides.Sum(Function(l) l.CoutDepart)      ' Coût du panier aux prix de départ
            Dim coutArrivee As Decimal = valides.Sum(Function(l) l.CoutArrivee)    ' Coût du panier aux prix d'arrivée
            Return New ResultatIndice With {
                .Indice = Math.Round(100D * coutArrivee / coutDepart, 2, MidpointRounding.AwayFromZero),   ' Indice de base 100, deux décimales
                .CoutDepart = coutDepart,                 ' Coût au départ
                .CoutArrivee = coutArrivee,               ' Coût à l'arrivée
                .NombreProduits = valides.Count,          ' Nombre de produits retenus
                .Lignes = valides                         ' Détail du panier
            }                                             ' Fin de l'initialisation
        End Function                                      ' Fin de CalculerIndice

    End Class                                             ' Fin de IndiceService

End Namespace                                             ' Fin de l'espace de noms Services
