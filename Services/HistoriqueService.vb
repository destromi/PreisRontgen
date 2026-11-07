' ============================================================================
' FICHIER : Services/HistoriqueService.vb
' RÔLE    : EF-06, préparation des données de l'historique d'un produit :
'           tri du tableau, réduction du nombre de points du graphique, résumé chiffré
'           et positions normalisées des points (le dessin lui-même se fait dans
'           l'événement Paint du formulaire frmHistorique).
' PERFORMANCE : le cahier exige moins d'une seconde pour 5 000 observations ; ici le
'           graphique ne garde que 400 points au maximum, et tout est calculé en mémoire.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Linq            ' Pour OrderBy, Where, Min, Max...
Imports PreisRontgen.Models    ' Pour PointPrix et ResumeHistorique

Namespace Services             ' Espace de noms : PreisRontgen.Services

    ''' <summary>Calculs « purs » (sans base de données) de l'historique des prix.</summary>
    Public NotInheritable Class HistoriqueService   ' Début de la classe

        ''' <summary>Nombre maximal de points dessinés sur le graphique.</summary>
        Public Const NombreMaxPointsCourbe As Integer = 400   ' Au-delà, on garde des points régulièrement espacés

        Private Sub New()                           ' Constructeur privé : classe-outil, jamais instanciée
        End Sub                                     ' Fin du constructeur

        ''' <summary>Tri du tableau : de la plus récente à la plus ancienne observation.</summary>
        Public Shared Function Trier(points As IEnumerable(Of PointPrix)) As List(Of PointPrix)   ' Ordre d'affichage de la grille
            If points Is Nothing Then Throw New ArgumentNullException(NameOf(points))   ' Refuse une liste absente
            Return points.OrderByDescending(Function(p) p.DateObs).ThenBy(Function(p) p.NomMagasin).ToList()   ' Dates décroissantes ; à date égale, par magasin
        End Function                                ' Fin de Trier

        ''' <summary>
        ''' Points du graphique : du plus ancien au plus récent. S'il y en a plus que nombreMax,
        ''' on garde nombreMax points régulièrement espacés, en conservant toujours le premier et le dernier.
        ''' </summary>
        Public Shared Function PreparerCourbe(points As IEnumerable(Of PointPrix),
                                              Optional nombreMax As Integer = NombreMaxPointsCourbe) As List(Of PointPrix)   ' Données du graphique
            If points Is Nothing Then Throw New ArgumentNullException(NameOf(points))   ' Refuse une liste absente
            Dim croissants As List(Of PointPrix) = points.OrderBy(Function(p) p.DateObs).ThenBy(Function(p) p.NomMagasin).ToList()   ' Ordre chronologique
            Dim limite As Integer = Math.Max(2, nombreMax)    ' Au moins deux points (premier et dernier)
            If croissants.Count <= limite Then Return croissants   ' Pas besoin de réduire

            Dim reduits As New List(Of PointPrix)(limite)     ' Liste réduite
            For i As Integer = 0 To limite - 1                ' Pour chaque point à garder
                Dim indice As Integer = CInt(Math.Round(i * (croissants.Count - 1) / CDbl(limite - 1)))   ' Position régulièrement répartie sur toute la liste
                reduits.Add(croissants(indice))               ' Garde ce point
            Next                                              ' Fin de la boucle
            Return reduits                                    ' Rend la liste réduite
        End Function                                          ' Fin de PreparerCourbe

        ''' <summary>Résumé chiffré : minimum, maximum, moyenne, premier, dernier et variation totale.</summary>
        Public Shared Function Resumer(points As IEnumerable(Of PointPrix)) As ResumeHistorique   ' Texte sous le graphique
            If points Is Nothing Then Throw New ArgumentNullException(NameOf(points))   ' Refuse une liste absente
            Dim croissants As List(Of PointPrix) = points.OrderBy(Function(p) p.DateObs).ToList()   ' Ordre chronologique
            Dim synthese As New ResumeHistorique()              ' Résultat (tout à zéro au départ)
            If croissants.Count = 0 Then Return synthese        ' Aucune observation : résumé vide

            synthese.Nombre = croissants.Count                  ' Nombre d'observations
            synthese.Minimum = croissants.Min(Function(p) p.PrixUnitaire)       ' Prix le plus bas
            synthese.Maximum = croissants.Max(Function(p) p.PrixUnitaire)       ' Prix le plus haut
            synthese.Moyenne = croissants.Average(Function(p) p.PrixUnitaire)   ' Prix moyen
            synthese.Premier = croissants(0).PrixUnitaire                       ' Plus ancienne observation
            synthese.Dernier = croissants(croissants.Count - 1).PrixUnitaire    ' Plus récente observation
            If synthese.Premier <> 0D Then                      ' Variation calculable
                synthese.VariationTotale = (synthese.Dernier - synthese.Premier) / synthese.Premier   ' (Dernier - Premier) / Premier
            End If                                            ' Fin du test
            Return synthese                                     ' Rend le résumé
        End Function                                          ' Fin de Resumer

        ''' <summary>
        ''' Positions verticales entre 0 (prix le plus bas) et 1 (prix le plus haut).
        ''' Si tous les prix sont identiques, la courbe est tracée au milieu (0,5).
        ''' </summary>
        Public Shared Function PositionsY(valeurs As IReadOnlyList(Of Decimal)) As List(Of Double)   ' Hauteur relative de chaque point
            Dim resultat As New List(Of Double)()             ' Résultat
            If valeurs Is Nothing OrElse valeurs.Count = 0 Then Return resultat   ' Rien à calculer
            Dim minimum As Decimal = valeurs.Min()            ' Plus bas
            Dim maximum As Decimal = valeurs.Max()            ' Plus haut
            For Each v As Decimal In valeurs                  ' Pour chaque valeur
                If maximum = minimum Then                     ' Tous égaux
                    resultat.Add(0.5R)                        ' Au milieu
                Else                                          ' Cas général
                    resultat.Add(CDbl((v - minimum) / (maximum - minimum)))   ' Proportion entre le plus bas et le plus haut
                End If                                        ' Fin du test
            Next                                              ' Fin de la boucle
            Return resultat                                   ' Rend les positions
        End Function                                          ' Fin de PositionsY

        ''' <summary>
        ''' Positions horizontales entre 0 (date la plus ancienne) et 1 (date la plus récente),
        ''' proportionnelles au temps écoulé. Une seule date (ou toutes égales) : au milieu (0,5).
        ''' </summary>
        Public Shared Function PositionsX(dates As IReadOnlyList(Of Date)) As List(Of Double)   ' Position horizontale relative de chaque point
            Dim resultat As New List(Of Double)()             ' Résultat
            If dates Is Nothing OrElse dates.Count = 0 Then Return resultat   ' Rien à calculer
            Dim premiere As Date = dates.Min()                ' Date la plus ancienne
            Dim etendue As Double = (dates.Max() - premiere).TotalDays   ' Nombre de jours couverts
            For Each d As Date In dates                       ' Pour chaque date
                If etendue <= 0R Then                         ' Une seule date
                    resultat.Add(0.5R)                        ' Au milieu
                Else                                          ' Cas général
                    resultat.Add((d - premiere).TotalDays / etendue)   ' Proportion du temps écoulé
                End If                                        ' Fin du test
            Next                                              ' Fin de la boucle
            Return resultat                                   ' Rend les positions
        End Function                                          ' Fin de PositionsX

    End Class                                                 ' Fin de HistoriqueService

End Namespace                                                 ' Fin de l'espace de noms Services
