' ============================================================================
' FICHIER : Demo/EcranAmeliorations.vb
' RÔLE    : écran de DÉMONSTRATION des améliorations du module de Stéphanie : on peut
'           essayer chaque nouveauté à la main, sans base de données :
'           1. lecture des paquets multiples et prix pour 100 g / 100 ml ;
'           2. hausse cumulée sur 3 mois ;
'           3. alerte non dupliquée ;
'           4. avertissement de prix inhabituel.
' NOTION  : cet écran n'utilise que les vrais services (NormalisationService et
'           DetectionService) ; seules les données sont fictives (LecteurDemo, DepotDemo).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Globalization   ' Pour lire un prix tapé avec une virgule ou un point
Imports System.Linq            ' Pour Where, OrderBy...
Imports PreisRontgen.Data      ' Pour IObservationLecteur et IHistoriqueLecteur
Imports PreisRontgen.Models    ' Pour Observation, Unite, UniteBase, Alerte, ParametresDetection
Imports PreisRontgen.Services  ' Pour NormalisationService et DetectionService

''' <summary>Panneau de démonstration des améliorations (normalisation et détection).</summary>
Public Class EcranAmeliorations                ' Début de la classe
    Inherits Panel                             ' C'est un panneau, placé dans un onglet

    Private Shared ReadOnly VertForet As Color = Color.FromArgb(30, 77, 58)        ' Vert forêt du groupe
    Private Shared ReadOnly BrunAlerte As Color = Color.FromArgb(122, 51, 0)       ' Brun des messages d'alerte (comme la grille)
    Private Shared ReadOnly FondClair As Color = Color.FromArgb(244, 247, 245)     ' Gris très clair du fond

    Private ReadOnly _txtConditionnement As New TextBox()   ' Zone « conditionnement » (ex. 6 x 33 cl)
    Private ReadOnly _txtPrix As New TextBox()              ' Zone « prix de l'article »
    Private ReadOnly _lblConditionnement As New Label()     ' Résultat du calcul du prix unitaire
    Private ReadOnly _lblCumulee As New Label()             ' Résultat du scénario de hausse cumulée
    Private ReadOnly _lblDoublon As New Label()             ' Résultat du scénario anti-doublon
    Private ReadOnly _txtPrixSaisi As New TextBox()         ' Zone « prix au kilo » du contrôle de prix inhabituel
    Private ReadOnly _lblInhabituel As New Label()          ' Résultat du contrôle de prix inhabituel

    Public Sub New()                           ' Constructeur : construit tout l'écran
        Dock = DockStyle.Fill                  ' Occupe tout l'onglet
        AutoScroll = True                      ' Barre de défilement si la fenêtre est trop petite
        BackColor = FondClair                  ' Fond gris très clair

        ' --- 1. Paquets multiples et prix pour 100 ---
        Dim g1 As GroupBox = CreerGroupe("1. Paquets multiples et prix pour 100 g / 100 ml", 10, 150)   ' Cadre numéro 1
        AjouterTexte(g1, "Conditionnement (ex. 500 g, 1,5 L, 6 x 33 cl) :", 16, 26)   ' Légende
        PlacerZone(g1, _txtConditionnement, "6 x 33 cl", 16, 46, 230)                 ' Zone de saisie
        AjouterTexte(g1, "Prix de l'article (FCFA) :", 270, 26)                       ' Légende
        PlacerZone(g1, _txtPrix, "850", 270, 46, 120)                                 ' Zone de saisie
        g1.Controls.Add(CreerBouton("Calculer", 410, 44, 120, AddressOf Calculer_Click))   ' Bouton
        PreparerResultat(g1, _lblConditionnement, 16, 86)                             ' Zone du résultat

        ' --- 2. Hausse cumulée ---
        Dim g2 As GroupBox = CreerGroupe("2. Hausse cumulée sur 3 mois", 170, 130)    ' Cadre numéro 2
        AjouterTexte(g2, "Prix au kg : 1 000, puis 1 040, puis 1 075, puis 1 115 FCFA. Aucune hausse isolée ne dépasse 5 %.", 16, 26)   ' Description du scénario
        g2.Controls.Add(CreerBouton("Lancer le scénario", 16, 52, 170, AddressOf Cumulee_Click))   ' Bouton
        PreparerResultat(g2, _lblCumulee, 16, 90)                                     ' Zone du résultat

        ' --- 3. Alerte non dupliquée ---
        Dim g3 As GroupBox = CreerGroupe("3. Alerte non dupliquée", 310, 130)         ' Cadre numéro 3
        AjouterTexte(g3, "Un ticket passe de 500 g à 450 g au même prix, puis il est analysé deux fois (double clic, réimport...).", 16, 26)   ' Description
        g3.Controls.Add(CreerBouton("Analyser deux fois", 16, 52, 170, AddressOf Doublon_Click))   ' Bouton
        PreparerResultat(g3, _lblDoublon, 16, 90)                                     ' Zone du résultat

        ' --- 4. Prix inhabituel ---
        Dim g4 As GroupBox = CreerGroupe("4. Avertissement de prix inhabituel", 450, 130)   ' Cadre numéro 4
        AjouterTexte(g4, "Historique : 1 000, 1 050 et 1 025 FCFA/kg. Tapez le prix au kilo d'un nouvel achat (essayez 10250) :", 16, 26)   ' Description
        PlacerZone(g4, _txtPrixSaisi, "10250", 16, 52, 120)                           ' Zone de saisie
        g4.Controls.Add(CreerBouton("Vérifier", 150, 50, 120, AddressOf Inhabituel_Click))   ' Bouton
        PreparerResultat(g4, _lblInhabituel, 16, 90)                                  ' Zone du résultat
    End Sub                                    ' Fin du constructeur

    ' ------------------------------------------------------------ outils de construction

    Private Function CreerGroupe(titre As String, y As Integer, hauteur As Integer) As GroupBox   ' Crée un cadre titré
        Dim g As New GroupBox()                ' Le cadre
        g.Text = titre                         ' Titre du cadre
        g.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)   ' Titre en gras
        g.ForeColor = VertForet                ' Titre en vert forêt
        g.BackColor = Color.White              ' Fond blanc
        g.Location = New Point(10, y)          ' Position dans l'écran
        g.Size = New Size(1000, hauteur)       ' Taille
        Controls.Add(g)                        ' Ajoute le cadre à l'écran
        Return g                               ' Rend le cadre
    End Function                               ' Fin de CreerGroupe

    Private Sub AjouterTexte(parent As Control, texte As String, x As Integer, y As Integer)   ' Ajoute une ligne de texte
        Dim l As New Label()                   ' Le texte
        l.Text = texte                         ' Contenu
        l.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)   ' Police normale (le cadre est en gras)
        l.ForeColor = Color.FromArgb(23, 38, 31)   ' Texte foncé
        l.AutoSize = True                      ' Taille adaptée au texte
        l.MaximumSize = New Size(960, 0)       ' Passe à la ligne si trop long
        l.Location = New Point(x, y)           ' Position
        parent.Controls.Add(l)                 ' Ajoute au cadre
    End Sub                                    ' Fin de AjouterTexte

    Private Sub PlacerZone(parent As Control, zone As TextBox, valeur As String, x As Integer, y As Integer, largeur As Integer)   ' Place une zone de saisie
        zone.Text = valeur                     ' Valeur proposée
        zone.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)   ' Police normale
        zone.Location = New Point(x, y)        ' Position
        zone.Width = largeur                   ' Largeur
        parent.Controls.Add(zone)              ' Ajoute au cadre
    End Sub                                    ' Fin de PlacerZone

    Private Sub PreparerResultat(parent As Control, zone As Label, x As Integer, y As Integer)   ' Prépare la zone où s'écrit le résultat
        zone.Text = "Cliquez sur le bouton pour voir le résultat."   ' Texte de départ
        zone.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)    ' Police normale
        zone.ForeColor = Color.FromArgb(74, 92, 82)   ' Gris-vert discret
        zone.AutoSize = True                   ' Taille adaptée au texte
        zone.MaximumSize = New Size(960, 0)    ' Passe à la ligne si trop long
        zone.Location = New Point(x, y)        ' Position
        parent.Controls.Add(zone)              ' Ajoute au cadre
    End Sub                                    ' Fin de PreparerResultat

    Private Shared Function CreerBouton(texte As String, x As Integer, y As Integer, largeur As Integer, gestionnaire As EventHandler) As Button   ' Crée un bouton vert forêt
        Dim b As New Button()                  ' Le bouton
        b.Text = texte                         ' Texte
        b.FlatStyle = FlatStyle.Flat           ' Bouton plat
        b.BackColor = VertForet                ' Fond vert forêt
        b.ForeColor = Color.White              ' Texte blanc
        b.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)   ' Texte en gras
        b.Location = New Point(x, y)           ' Position
        b.Size = New Size(largeur, 30)         ' Taille
        AddHandler b.Click, gestionnaire       ' Relie le clic à la méthode demandée
        Return b                               ' Rend le bouton
    End Function                               ' Fin de CreerBouton

    Private Shared Sub Afficher(zone As Label, texte As String, avertissement As Boolean)   ' Écrit un résultat en vert (normal) ou brun (alerte)
        zone.Text = texte                      ' Texte
        zone.ForeColor = If(avertissement, BrunAlerte, VertForet)   ' Brun pour une alerte, vert sinon
        zone.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)      ' Résultat en gras
    End Sub                                    ' Fin de Afficher

    ' ------------------------------------------------------------ 1. paquets multiples

    Private Sub Calculer_Click(sender As Object, e As EventArgs)   ' Clic sur « Calculer »
        Dim quantite As Decimal                ' Quantité totale lue
        Dim uniteLue As Unite                  ' Unité lue
        If Not NormalisationService.TryParseConditionnement(_txtConditionnement.Text, quantite, uniteLue) Then   ' Texte non reconnu
            Afficher(_lblConditionnement, "Conditionnement non reconnu. Exemples : 500 g, 1,5 L, 6 x 33 cl.", True)   ' Message d'erreur
            Return                             ' On s'arrête
        End If                                 ' Fin du test

        Dim prix As Decimal                    ' Prix tapé
        If Not LirePrix(_txtPrix.Text, prix) Then   ' Prix illisible
            Afficher(_lblConditionnement, "Prix illisible. Tapez un nombre positif, par exemple 850.", True)   ' Message d'erreur
            Return                             ' On s'arrête
        End If                                 ' Fin du test

        Dim famille As UniteBase = NormalisationService.UniteBaseDe(uniteLue)   ' Famille : kg, L ou pièce
        Dim quantiteBase As Decimal = NormalisationService.ConvertirEnUniteBase(quantite, uniteLue)   ' Quantité en unité de base
        Dim prixUnitaire As Decimal = NormalisationService.CalculerPrixUnitaire(prix, quantite, uniteLue)   ' Prix par kg, L ou pièce
        Dim symbole As String = SymboleDe(famille)   ' « kg », « L » ou « pièce »

        Dim texte As String = $"Quantité totale : {quantite:0.##} {uniteLue} soit {quantiteBase:0.###} {symbole}" & vbCrLf &
                              $"Prix unitaire : {prixUnitaire:N2} FCFA / {symbole}"   ' Premières lignes du résultat
        Dim pourCent As Decimal? = NormalisationService.PrixPourCent(prixUnitaire, famille)   ' Prix pour 100 g ou 100 ml (Nothing pour une pièce)
        If pourCent.HasValue Then              ' Cette échelle existe
            texte &= vbCrLf & $"Pour {NormalisationService.LibelleEchelleCent(famille)} : {pourCent.Value:N2} FCFA"   ' Ligne ajoutée
        End If                                 ' Fin du test
        Afficher(_lblConditionnement, texte, False)   ' Affiche le résultat
    End Sub                                    ' Fin de Calculer_Click

    Private Shared Function SymboleDe(uniteBase As UniteBase) As String   ' Texte de l'unité de base
        Select Case uniteBase                  ' Aiguillage selon la famille
            Case UniteBase.Kg                  ' Poids
                Return "kg"                    ' Kilogramme
            Case UniteBase.Litre               ' Volume
                Return "L"                     ' Litre
            Case Else                          ' Pièce
                Return "pièce"                 ' À l'unité
        End Select                             ' Fin de l'aiguillage
    End Function                               ' Fin de SymboleDe

    Private Shared Function LirePrix(texte As String, ByRef prix As Decimal) As Boolean   ' Lit un prix avec virgule ou point
        Dim propre As String = texte.Trim().Replace(" ", "").Replace(","c, "."c)   ' Enlève les espaces, uniformise la virgule
        Return Decimal.TryParse(propre, NumberStyles.Number, CultureInfo.InvariantCulture, prix) AndAlso prix >= 0D   ' Nombre valide et positif
    End Function                               ' Fin de LirePrix

    ' ------------------------------------------------------------ 2. hausse cumulée

    Private Sub Cumulee_Click(sender As Object, e As EventArgs)   ' Clic sur « Lancer le scénario »
        Dim aujourdhui As Date = Date.Today    ' Date du jour : les relevés sont toujours récents
        Dim lecteur As New LecteurDemo(New List(Of Observation) From {
            CreerObs(1, 1000D, aujourdhui.AddDays(-80)),    ' Il y a 80 jours : 1 000 FCFA/kg
            CreerObs(2, 1040D, aujourdhui.AddDays(-50)),    ' Il y a 50 jours : 1 040 (+4,0 %)
            CreerObs(3, 1075D, aujourdhui.AddDays(-20))     ' Il y a 20 jours : 1 075 (+3,4 %)
        })                                     ' Trois petites hausses de suite
        Dim service As New DetectionService(lecteur, New DepotDemo(), New ParametresDetection())   ' Vrai service, faux stockage
        Dim nouvelle As Observation = CreerObs(99, 1115D, aujourdhui)   ' Aujourd'hui : 1 115 (+3,7 % seulement par rapport au dernier)

        Dim alertes As List(Of Alerte) = service.Analyser(nouvelle)   ' Lance l'analyse
        If alertes.Count = 0 Then              ' Rien détecté
            Afficher(_lblCumulee, "Aucune alerte.", False)   ' Résultat
        Else                                   ' Une alerte a été créée
            Afficher(_lblCumulee, $"{alertes.Count} alerte détectée : {alertes(0).Detail}", True)   ' Texte explicatif de l'alerte
        End If                                 ' Fin du test
    End Sub                                    ' Fin de Cumulee_Click

    ' ------------------------------------------------------------ 3. anti-doublon

    Private Sub Doublon_Click(sender As Object, e As EventArgs)   ' Clic sur « Analyser deux fois »
        Dim aujourdhui As Date = Date.Today    ' Date du jour
        Dim ancienne As Observation = NormalisationService.CreerObservation(1, 1, aujourdhui.AddDays(-30), 1200D, 500D, Unite.Gramme)   ' 500 g à 1 200 FCFA
        ancienne.Id = 1                        ' Observation déjà enregistrée
        Dim nouvelle As Observation = NormalisationService.CreerObservation(1, 1, aujourdhui, 1200D, 450D, Unite.Gramme)   ' 450 g au même prix : shrinkflation
        nouvelle.Id = 2                        ' Observation enregistrée aussi

        Dim service As New DetectionService(New LecteurDemo(New List(Of Observation) From {ancienne}), New DepotDemo(), New ParametresDetection())   ' Vrai service, faux stockage
        Dim premiere As Integer = service.Analyser(nouvelle).Count   ' Première analyse
        Dim seconde As Integer = service.Analyser(nouvelle).Count    ' Seconde analyse du même ticket
        Afficher(_lblDoublon, $"Première analyse : {premiere} alerte(s) créée(s)." & vbCrLf &
                              $"Seconde analyse du même ticket : {seconde} alerte(s) créée(s)  (aucun doublon).", seconde <> 0)   ' Résultat
    End Sub                                    ' Fin de Doublon_Click

    ' ------------------------------------------------------------ 4. prix inhabituel

    Private Sub Inhabituel_Click(sender As Object, e As EventArgs)   ' Clic sur « Vérifier »
        Dim prix As Decimal                    ' Prix au kilo tapé
        If Not LirePrix(_txtPrixSaisi.Text, prix) Then   ' Prix illisible
            Afficher(_lblInhabituel, "Prix illisible. Tapez un nombre positif, par exemple 10250.", True)   ' Message d'erreur
            Return                             ' On s'arrête
        End If                                 ' Fin du test

        Dim aujourdhui As Date = Date.Today    ' Date du jour
        Dim lecteur As New LecteurDemo(New List(Of Observation) From {
            CreerObs(1, 1000D, aujourdhui.AddDays(-90)),    ' Relevé de 1 000 FCFA/kg
            CreerObs(2, 1050D, aujourdhui.AddDays(-60)),    ' Relevé de 1 050
            CreerObs(3, 1025D, aujourdhui.AddDays(-30))     ' Relevé de 1 025
        })                                     ' Trois relevés : le prix habituel est la médiane, 1 025
        Dim service As New DetectionService(lecteur, New DepotDemo(), New ParametresDetection())   ' Vrai service
        Dim message As String = service.VerifierPrixInhabituel(CreerObs(0, prix, aujourdhui))   ' Contrôle du prix saisi

        If message Is Nothing Then             ' Prix cohérent
            Afficher(_lblInhabituel, "Prix normal : aucun avertissement.", False)   ' Résultat
        Else                                   ' Prix suspect
            Afficher(_lblInhabituel, message, True)   ' Message du service
        End If                                 ' Fin du test
    End Sub                                    ' Fin de Inhabituel_Click

    ' Fabrique une observation de 1 kg (donc prix unitaire = prix) avec un identifiant donné
    Private Shared Function CreerObs(id As Integer, prixAuKilo As Decimal, jour As Date) As Observation   ' Observation de démonstration
        Dim obs As Observation = NormalisationService.CreerObservation(1, 1, jour, prixAuKilo, 1000D, Unite.Gramme)   ' 1 000 g au prix donné
        obs.Id = id                            ' Identifiant simulé
        Return obs                             ' Rend l'observation
    End Function                               ' Fin de CreerObs

    ' ------------------------------------------------------------ faux lecteur

    ''' <summary>Faux lecteur en mémoire : connaît quelques observations et sait lire l'historique.</summary>
    Private NotInheritable Class LecteurDemo   ' Classe interne
        Implements IObservationLecteur, IHistoriqueLecteur   ' Respecte les deux contrats

        Private ReadOnly _donnees As List(Of Observation)   ' Observations « en base »

        Public Sub New(donnees As IEnumerable(Of Observation))   ' Constructeur
            _donnees = New List(Of Observation)(donnees)         ' Copie les observations
        End Sub                                ' Fin du constructeur

        Public Function ObtenirPrecedente(observation As Observation) As Observation Implements IObservationLecteur.ObtenirPrecedente   ' Dernière observation avant celle-ci
            Dim candidats As IEnumerable(Of Observation) = _donnees.Where(Function(o) o.IdProduit = observation.IdProduit AndAlso o.IdMagasin = observation.IdMagasin AndAlso o.DateObs <= observation.DateObs AndAlso o.Id <> observation.Id)   ' Même produit, même magasin, antérieures
            Return candidats.OrderBy(Function(o) o.DateObs).ThenBy(Function(o) o.Id).LastOrDefault()   ' La plus récente (Nothing s'il n'y en a pas)
        End Function                           ' Fin de ObtenirPrecedente

        Public Function ListerDepuis(observation As Observation, dateLimite As Date) As List(Of Observation) Implements IHistoriqueLecteur.ListerDepuis   ' Historique de la période
            Dim candidats As IEnumerable(Of Observation) = _donnees.Where(Function(o) o.IdProduit = observation.IdProduit AndAlso o.IdMagasin = observation.IdMagasin AndAlso o.DateObs >= dateLimite AndAlso (o.DateObs < observation.DateObs OrElse (o.DateObs = observation.DateObs AndAlso o.Id < observation.Id)))   ' De la date limite jusqu'à l'observation (exclue)
            Return candidats.OrderBy(Function(o) o.DateObs).ThenBy(Function(o) o.Id).ToList()   ' Du plus ancien au plus récent
        End Function                           ' Fin de ListerDepuis
    End Class                                  ' Fin de LecteurDemo

End Class                                      ' Fin de EcranAmeliorations
