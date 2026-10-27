' ============================================================================
' FICHIER : Data/IAlerteRepository.vb
' RÔLE    : définit deux « contrats » (interfaces) entre la logique métier et la base.
' NOTION  : une Interface liste des méthodes SANS code. Toute classe qui
'           « implémente » l'interface s'engage à fournir ces méthodes.
'           Avantage : la détection ne dépend pas de MySQL. Dans les tests, on
'           lui donne de faux dépôts en mémoire ; en production, les vrais (MySQL).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports PreisRontgen.Models    ' Pour utiliser Alerte, FiltreAlerte, Observation

Namespace Data                 ' Espace de noms de l'accès aux données : PreisRontgen.Data

    ''' <summary>
    ''' Accès aux alertes (EF-07, EF-08, EF-09).
    ''' Passer par cette interface permet de tester la détection sans base de données.
    ''' </summary>
    Public Interface IAlerteRepository     ' Contrat d'accès aux alertes (le « I » devant le nom est la convention des interfaces)

        ''' <summary>Enregistre une alerte et retourne son identifiant.</summary>
        Function Ajouter(alerte As Alerte) As Integer    ' Insère l'alerte et rend l'identifiant attribué

        ''' <summary>Recherche les alertes correspondant au filtre, de la plus récente à la plus ancienne.</summary>
        Function Rechercher(filtre As FiltreAlerte) As List(Of Alerte)   ' Rend les alertes qui respectent le filtre (EF-09)

        Sub MarquerCommeLue(idAlerte As Integer)         ' Passe UNE alerte à l'état « lue »

        Sub MarquerToutesCommeLues()                     ' Passe TOUTES les alertes à l'état « lue »

        ''' <summary>Nombre d'alertes non lues (compteur du formulaire principal et du tableau de bord).</summary>
        Function CompterNonLues() As Integer             ' Rend le nombre d'alertes non lues

        ''' <summary>Liste des magasins (identifiant, nom) pour le filtre de la liste des alertes.</summary>
        Function ListerMagasins() As List(Of KeyValuePair(Of Integer, String))   ' Rend des couples (identifiant, nom) pour remplir la liste déroulante

    End Interface                                        ' Fin de IAlerteRepository

    ''' <summary>
    ''' Lecture de l'observation précédente d'un produit dans un magasin.
    ''' </summary>
    Public Interface IObservationLecteur     ' Contrat de lecture de l'observation précédente

        ''' <summary>
        ''' Retourne la dernière observation du même produit dans le même magasin,
        ''' antérieure ou à la même date que l'observation donnée (et différente d'elle),
        ''' ou Nothing s'il n'y en a pas.
        ''' </summary>
        Function ObtenirPrecedente(observation As Observation) As Observation   ' Rend l'observation précédente ou Nothing

    End Interface                            ' Fin de IObservationLecteur

End Namespace                                ' Fin de l'espace de noms Data
