' ============================================================================
' FICHIER : Demo/DepotDemo.vb
' RÔLE    : faux dépôt d'alertes EN MÉMOIRE, pour essayer l'écran frmAlertes
'           sans base MySQL. Il contient quelques alertes d'exemple.
' NOTION  : DepotDemo « implémente » IAlerteRepository, exactement comme la vraie
'           classe MySQL. L'écran ne voit aucune différence : c'est l'intérêt d'une interface.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Linq          ' Pour Where, OrderBy, Select, Count...
Imports PreisRontgen.Data    ' Pour IAlerteRepository
Imports PreisRontgen.Models  ' Pour Alerte, FiltreAlerte, TypeAlerte

''' <summary>Dépôt d'alertes en mémoire, avec des données d'exemple (démonstration uniquement).</summary>
Public Class DepotDemo                          ' Début de la classe
    Implements IAlerteRepository                ' S'engage à fournir toutes les méthodes du contrat

    Private ReadOnly _alertes As New List(Of Alerte)()    ' Les alertes « enregistrées » (perdues à la fermeture du programme)
    Private _prochainId As Integer = 1                    ' Prochain identifiant à attribuer

    Public Sub New()                            ' Constructeur : prépare les données d'exemple
        Dim aujourdhui As Date = Date.Today     ' Date du jour, pour que les exemples soient toujours récents

        ' --- Hausses (EF-07) : le prix unitaire a augmenté de plus de 5 % ---
        Ajouter(Creer(1, "Huile végétale Mayor", 1, "Carrefour Market", TypeAlerte.Hausse, 0.082D, aujourdhui.AddDays(-1),
                      "Prix unitaire : 1 941 FCFA/L vers 2 100 FCFA/L (+ 8,2 %), au-dessus du seuil de 5 %."))   ' Hausse récente, non lue
        Ajouter(Creer(2, "Lait entier Camlait", 2, "Santa Lucia", TypeAlerte.Shrinkflation, 0.1111D, aujourdhui.AddDays(-2),
                      "Quantité : 1 L vers 900 ml (- 10,0 %) pour un prix identique de 1 250 FCFA ; prix unitaire réel : 1 250 vers 1 389 FCFA/L (+ 11,1 %)."))   ' Shrinkflation, non lue
        Ajouter(Creer(3, "Sucre cristallisé 1 kg", 2, "Santa Lucia", TypeAlerte.Hausse, 0.071D, aujourdhui.AddDays(-3),
                      "Prix unitaire : 800 FCFA/kg vers 857 FCFA/kg (+ 7,1 %), au-dessus du seuil de 5 %."))   ' Hausse, non lue

        Ajouter(Creer(4, "Riz parfumé", 3, "Marché central", TypeAlerte.Hausse, 0.054D, aujourdhui.AddDays(-5),
                      "Prix unitaire : 1 708 FCFA/kg vers 1 800 FCFA/kg (+ 5,4 %), au-dessus du seuil de 5 %."))   ' Hausse déjà lue
        Ajouter(Creer(5, "Café moulu 250 g", 1, "Carrefour Market", TypeAlerte.Shrinkflation, 0.0638D, aujourdhui.AddDays(-9),
                      "Quantité : 250 g vers 235 g (- 6,0 %) pour un prix identique de 1 600 FCFA ; prix unitaire réel : 6 400 vers 6 808 FCFA/kg (+ 6,4 %)."))   ' Shrinkflation déjà lue
        Ajouter(Creer(6, "Pain complet", 3, "Marché central", TypeAlerte.Hausse, 0.09D, aujourdhui.AddDays(-20),
                      "Prix unitaire : 1 000 FCFA/kg vers 1 090 FCFA/kg (+ 9,0 %), au-dessus du seuil de 5 %."))   ' Plus ancienne, déjà lue

        _alertes(3).Lue = True                  ' Les alertes 4, 5 et 6 sont déjà « lues » :
        _alertes(4).Lue = True                  ' elles servent à tester la case
        _alertes(5).Lue = True                  ' « Non lues uniquement »
    End Sub                                     ' Fin du constructeur

    ''' <summary>Fabrique une alerte d'exemple (évite de répéter dix lignes d'initialisation).</summary>
    Private Shared Function Creer(idProduit As Integer, nomProduit As String, idMagasin As Integer, nomMagasin As String,
                                  type As TypeAlerte, valeur As Decimal, dateAlerte As Date, detail As String) As Alerte
        Return New Alerte With {                ' Crée l'alerte et remplit ses propriétés
            .IdProduit = idProduit,             ' Produit concerné
            .NomProduit = nomProduit,           ' Nom affiché dans la grille
            .IdMagasin = idMagasin,             ' Magasin concerné
            .NomMagasin = nomMagasin,           ' Nom affiché dans la grille
            .Type = type,                       ' Hausse ou Shrinkflation
            .Valeur = valeur,                   ' Variation en fraction (0,0824 = 8,24 %)
            .DateAlerte = dateAlerte,           ' Date de l'alerte
            .Lue = False,                       ' Nouvelle alerte : non lue
            .Detail = detail                    ' Phrase explicative
        }
    End Function                                ' Fin de Creer

    Private Shared Function Copie(a As Alerte) As Alerte   ' Renvoie une copie : l'écran ne modifie pas directement le stockage
        Return New Alerte With {.Id = a.Id, .IdProduit = a.IdProduit, .IdMagasin = a.IdMagasin, .Type = a.Type,
                                .Valeur = a.Valeur, .DateAlerte = a.DateAlerte, .Lue = a.Lue, .Detail = a.Detail,
                                .NomProduit = a.NomProduit, .NomMagasin = a.NomMagasin}   ' Recopie chaque propriété
    End Function                                ' Fin de Copie

    Public Function Ajouter(alerte As Alerte) As Integer Implements IAlerteRepository.Ajouter   ' Enregistre une alerte
        alerte.Id = _prochainId                 ' Attribue l'identifiant
        _prochainId += 1                        ' Prépare le suivant
        _alertes.Add(alerte)                    ' Ajoute à la liste
        Return alerte.Id                        ' Rend l'identifiant
    End Function                                ' Fin de Ajouter

    Public Function Rechercher(filtre As FiltreAlerte) As List(Of Alerte) Implements IAlerteRepository.Rechercher   ' Applique les filtres
        Return _alertes.Where(Function(a) (Not filtre.DateDebut.HasValue OrElse a.DateAlerte.Date >= filtre.DateDebut.Value.Date) _
                                  AndAlso (Not filtre.DateFin.HasValue OrElse a.DateAlerte.Date <= filtre.DateFin.Value.Date) _
                                  AndAlso (Not filtre.IdMagasin.HasValue OrElse a.IdMagasin = filtre.IdMagasin.Value) _
                                  AndAlso (Not filtre.Type.HasValue OrElse a.Type = filtre.Type.Value) _
                                  AndAlso (Not filtre.Lue.HasValue OrElse a.Lue = filtre.Lue.Value)) _
                       .OrderByDescending(Function(a) a.DateAlerte) _
                       .ThenByDescending(Function(a) a.Id) _
                       .Select(Function(a) Copie(a)) _
                       .ToList()                ' Garde les alertes qui respectent tous les critères, de la plus récente à la plus ancienne
    End Function                                ' Fin de Rechercher

    Public Sub MarquerCommeLue(idAlerte As Integer) Implements IAlerteRepository.MarquerCommeLue   ' Passe une alerte à « lue »
        For Each a As Alerte In _alertes        ' Parcourt les alertes
            If a.Id = idAlerte Then a.Lue = True   ' Marque celle qui correspond
        Next                                    ' Fin de la boucle
    End Sub                                     ' Fin de MarquerCommeLue

    Public Sub MarquerToutesCommeLues() Implements IAlerteRepository.MarquerToutesCommeLues   ' Passe tout à « lu »
        For Each a As Alerte In _alertes        ' Parcourt les alertes
            a.Lue = True                        ' Marque chacune
        Next                                    ' Fin de la boucle
    End Sub                                     ' Fin de MarquerToutesCommeLues

    Public Function CompterNonLues() As Integer Implements IAlerteRepository.CompterNonLues   ' Compte les non lues
        Return _alertes.Where(Function(a) Not a.Lue).Count()   ' Nombre d'alertes dont Lue vaut False
    End Function                                ' Fin de CompterNonLues

    Public Function ListerMagasins() As List(Of KeyValuePair(Of Integer, String)) Implements IAlerteRepository.ListerMagasins   ' Magasins du filtre
        Return New List(Of KeyValuePair(Of Integer, String)) From {
            New KeyValuePair(Of Integer, String)(1, "Carrefour Market"),
            New KeyValuePair(Of Integer, String)(2, "Santa Lucia"),
            New KeyValuePair(Of Integer, String)(3, "Marché central")
        }                                       ' Trois magasins d'exemple
    End Function                                ' Fin de ListerMagasins

End Class                                       ' Fin de DepotDemo
