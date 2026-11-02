' ============================================================================
' FICHIER : Demo/FenetreDemo.vb
' RÔLE    : fenêtre de DÉMONSTRATION qui imite la fenêtre principale de la maquette
'           du groupe : bandeau vert forêt (nom, utilisateur, compteur d'alertes,
'           déconnexion) et onglets horizontaux. L'onglet « Alertes » contient le
'           vrai écran frmAlertes (partie de Stéphanie) ; les autres onglets sont
'           réservés aux écrans des autres membres du groupe.
' NOTION  : un formulaire peut être placé DANS un onglet (TopLevel = False).
'           Le compteur du bandeau se met à jour grâce à l'événement
'           CompteurChange de frmAlertes (programmation événementielle).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports PreisRontgen.Data    ' Pour IAlerteRepository

Public Class FenetreDemo                       ' Début de la classe de la fenêtre de démonstration
    Inherits Form                              ' C'est une fenêtre Windows

    Private WithEvents _ecranAlertes As PreisRontgen.frmAlertes   ' L'écran des alertes (WithEvents : on peut écouter CompteurChange)
    Private ReadOnly _lblAlertes As New Label()    ' Texte « Alertes non lues : N » du bandeau

    Public Sub New(depot As IAlerteRepository)     ' Constructeur : construit toute la fenêtre
        Dim vertForet As Color = Color.FromArgb(30, 77, 58)       ' Vert forêt du bandeau
        Dim fondClair As Color = Color.FromArgb(244, 247, 245)    ' Gris très clair du fond

        Text = "Pricieux - Suivi des prix du foyer"   ' Titre de la fenêtre (comme la maquette)
        ClientSize = New Size(1120, 680)              ' Taille intérieure : 1120 x 680 px
        MinimumSize = New Size(1000, 500)             ' Taille minimale
        BackColor = fondClair                         ' Fond gris très clair
        StartPosition = FormStartPosition.CenterScreen   ' S'ouvre au centre de l'écran

        ' --- Onglets (créés d'abord : le contrôle « Fill » est ajouté avant le bandeau) ---
        Dim onglets As New TabControl()               ' Le contrôle à onglets
        onglets.Dock = DockStyle.Fill                 ' Occupe tout l'espace sous le bandeau
        Dim noms As String() = {"Accueil", "Saisie ticket", "Historique", "Alertes", "Analyses", "Référentiels", "CSV et dépenses"}   ' Les 7 onglets de la maquette
        For Each nom As String In noms                ' Pour chaque nom d'onglet
            Dim page As New TabPage(nom)              ' Crée la page de l'onglet
            page.BackColor = fondClair                ' Fond gris très clair
            If nom = "Alertes" Then                   ' L'onglet des alertes contient le vrai écran
                _ecranAlertes = New PreisRontgen.frmAlertes(depot)   ' Crée l'écran des alertes avec le faux dépôt
                _ecranAlertes.TopLevel = False        ' Autorise à placer le formulaire dans un conteneur
                _ecranAlertes.FormBorderStyle = FormBorderStyle.None   ' Sans bordure ni barre de titre
                _ecranAlertes.Dock = DockStyle.Fill   ' Remplit la page de l'onglet
                page.Controls.Add(_ecranAlertes)      ' Place l'écran dans la page
                _ecranAlertes.Show()                  ' Affiche l'écran (déclenche son chargement)
            Else                                      ' Les autres onglets sont des emplacements vides
                Dim texte As New Label()              ' Message d'information
                texte.Text = "Écran réalisé par un autre membre du groupe."   ' Explication
                texte.AutoSize = True                 ' Taille adaptée au texte
                texte.Location = New Point(20, 20)    ' Position dans la page
                page.Controls.Add(texte)              ' Place le message dans la page
            End If                                    ' Fin du choix
            onglets.TabPages.Add(page)                ' Ajoute la page au contrôle à onglets
        Next                                          ' Fin de la boucle
        onglets.SelectedIndex = 3                     ' Ouvre directement l'onglet « Alertes »
        Controls.Add(onglets)                         ' Ajoute les onglets à la fenêtre

        ' --- Bandeau vert forêt ---
        Dim bandeau As New Panel()                    ' Panneau du haut
        bandeau.Dock = DockStyle.Top                  ' Colle en haut
        bandeau.Height = 70                           ' Hauteur : 70 px
        bandeau.BackColor = vertForet                 ' Fond vert forêt

        Dim titre As New Label()                      ' Nom de l'application
        titre.Text = "Pricieux"                       ' Texte
        titre.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)   ' Grande police en gras
        titre.ForeColor = Color.White                 ' Texte blanc
        titre.AutoSize = True                         ' Taille adaptée au texte
        titre.Location = New Point(16, 6)             ' Position
        bandeau.Controls.Add(titre)                   ' Ajoute au bandeau

        Dim slogan As New Label()                     ' Phrase sous le nom
        slogan.Text = "Les prix de vos achats, clairement suivis."   ' Texte de la maquette
        slogan.Font = New Font("Segoe UI", 8.0F)      ' Petite police
        slogan.ForeColor = Color.FromArgb(213, 227, 218)   ' Vert très clair
        slogan.AutoSize = True                        ' Taille adaptée au texte
        slogan.Location = New Point(19, 42)           ' Position
        bandeau.Controls.Add(slogan)                  ' Ajoute au bandeau

        Dim utilisateur As New Label()                ' Nom de l'utilisateur connecté
        utilisateur.Text = "Aïcha · Administratrice" ' Texte de la maquette
        utilisateur.ForeColor = Color.White           ' Texte blanc
        utilisateur.AutoSize = True                   ' Taille adaptée au texte
        utilisateur.Anchor = AnchorStyles.Top Or AnchorStyles.Right   ' Reste accroché à droite
        utilisateur.Location = New Point(660, 26)     ' Position
        bandeau.Controls.Add(utilisateur)             ' Ajoute au bandeau

        _lblAlertes.Text = "Alertes non lues : 0"    ' Valeur initiale (mise à jour par CompteurChange)
        _lblAlertes.ForeColor = Color.FromArgb(241, 217, 138)   ' Jaune doux, comme la maquette
        _lblAlertes.AutoSize = True                   ' Taille adaptée au texte
        _lblAlertes.Anchor = AnchorStyles.Top Or AnchorStyles.Right   ' Reste accroché à droite
        _lblAlertes.Location = New Point(840, 26)     ' Position
        bandeau.Controls.Add(_lblAlertes)             ' Ajoute au bandeau

        Dim deconnexion As New Button()               ' Bouton « Se déconnecter »
        deconnexion.Text = "Se déconnecter"           ' Texte
        deconnexion.FlatStyle = FlatStyle.Flat        ' Bouton plat
        deconnexion.BackColor = Color.White           ' Fond blanc
        deconnexion.ForeColor = vertForet             ' Texte vert forêt
        deconnexion.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)   ' Texte en gras
        deconnexion.Size = New Size(110, 30)          ' Taille
        deconnexion.Anchor = AnchorStyles.Top Or AnchorStyles.Right   ' Reste accroché à droite
        deconnexion.Location = New Point(990, 20)     ' Position
        AddHandler deconnexion.Click, AddressOf Deconnexion_Click   ' Relie le clic à la méthode ci-dessous
        bandeau.Controls.Add(deconnexion)             ' Ajoute au bandeau

        Controls.Add(bandeau)                         ' Ajoute le bandeau à la fenêtre (tout en haut)
    End Sub                                           ' Fin du constructeur

    Private Sub Deconnexion_Click(sender As Object, e As EventArgs)   ' Clic sur « Se déconnecter »
        Close()                                       ' Dans la démonstration, ferme simplement la fenêtre
    End Sub                                           ' Fin du gestionnaire

    Private Sub EcranAlertes_CompteurChange(nombreNonLues As Integer) Handles _ecranAlertes.CompteurChange   ' Déclenché par frmAlertes après chaque rechargement
        _lblAlertes.Text = $"Alertes non lues : {nombreNonLues}"   ' Met à jour le texte du bandeau
    End Sub                                           ' Fin du gestionnaire

End Class                                             ' Fin de la classe FenetreDemo
