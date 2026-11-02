' ============================================================================================
' FICHIER GÉNÉRÉ PAR LE CONCEPTEUR DE FORMULAIRES DE VISUAL STUDIO.
' Il décrit la fenêtre frmAlertes : quels contrôles existent, où ils sont et quelles propriétés ils ont.
' Mise en page conforme à la maquette du groupe (onglet « Alertes ») avec le vert forêt comme couleur principale.
' Les commentaires ci-dessous sont des explications pour l'étude ; Visual Studio peut les effacer
' si vous modifiez la fenêtre avec le concepteur (c'est sans conséquence sur le fonctionnement).
' Chaque bloc « 'nomDuControle » regroupe les réglages d'un contrôle.
' ============================================================================================
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  ' Indique à Visual Studio que ce fichier est généré par le concepteur de formulaires
Partial Class frmAlertes   ' Partial : la classe est coupée en deux fichiers (celui-ci = apparence, frmAlertes.vb = comportement)
    Inherits System.Windows.Forms.Form  ' La classe hérite de Form : c'est une fenêtre Windows

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>  ' Le débogueur n'entre pas dans cette méthode (code technique)
    Protected Overrides Sub Dispose(disposing As Boolean)  ' Libère les ressources du formulaire à sa fermeture
        Try  ' Début du bloc protégé
            If disposing AndAlso components IsNot Nothing Then  ' Si on libère les ressources et que le conteneur de composants existe
                components.Dispose()  ' Libère les composants du formulaire
            End If  ' Fin du test
        Finally  ' Ce bloc s'exécute toujours, même en cas d'erreur
            MyBase.Dispose(disposing)  ' Appelle la libération de la classe Form (parent)
        End Try  ' Fin du bloc protégé
    End Sub  ' Fin de la méthode

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer  ' Conteneur des composants non visuels (aucun ici, exigé par le concepteur)

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>  ' Le débogueur ne s'arrête pas dans cette méthode générée
    Private Sub InitializeComponent()  ' Méthode qui crée et configure tous les contrôles ; appelée par le constructeur du formulaire
        pnlTitre = New System.Windows.Forms.Panel()   ' --- Création de tous les contrôles (New) : pour l'instant ils existent en mémoire mais ne sont pas réglés ---
        lblTitre = New System.Windows.Forms.Label()  ' Crée lblTitre : le titre « Alertes de prix »
        pnlFiltres = New System.Windows.Forms.Panel()  ' Crée pnlFiltres : le panneau blanc des filtres
        lblDu = New System.Windows.Forms.Label()  ' Crée lblDu : une étiquette de texte
        dtpDebut = New System.Windows.Forms.DateTimePicker()  ' Crée dtpDebut : un sélecteur de date
        lblAu = New System.Windows.Forms.Label()  ' Crée lblAu : une étiquette de texte
        dtpFin = New System.Windows.Forms.DateTimePicker()  ' Crée dtpFin : un sélecteur de date
        lblMagasin = New System.Windows.Forms.Label()  ' Crée lblMagasin : une étiquette de texte
        cmbMagasin = New System.Windows.Forms.ComboBox()  ' Crée cmbMagasin : une liste déroulante
        lblType = New System.Windows.Forms.Label()  ' Crée lblType : une étiquette de texte
        cmbType = New System.Windows.Forms.ComboBox()  ' Crée cmbType : une liste déroulante
        chkNonLues = New System.Windows.Forms.CheckBox()  ' Crée chkNonLues : la case « Non lues uniquement »
        btnFiltrer = New System.Windows.Forms.Button()  ' Crée btnFiltrer : un bouton
        btnMarquerLue = New System.Windows.Forms.Button()  ' Crée btnMarquerLue : un bouton
        pnlDetail = New System.Windows.Forms.Panel()  ' Crée pnlDetail : le panneau blanc du détail de l'alerte sélectionnée
        lblDetailTitre = New System.Windows.Forms.Label()  ' Crée lblDetailTitre : le titre du panneau de détail
        lblDetail = New System.Windows.Forms.Label()  ' Crée lblDetail : le texte explicatif de l'alerte sélectionnée
        pnlBas = New System.Windows.Forms.Panel()  ' Crée pnlBas : la barre du bas (compteur)
        lblCompteur = New System.Windows.Forms.Label()  ' Crée lblCompteur : une étiquette de texte
        dgvAlertes = New System.Windows.Forms.DataGridView()  ' Crée dgvAlertes : une grille de données (tableau)
        colDate = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colDate : une colonne de texte de la grille
        colType = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colType : une colonne de texte de la grille
        colProduit = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colProduit : une colonne de texte de la grille
        colMagasin = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colMagasin : une colonne de texte de la grille
        colVariation = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colVariation : une colonne de texte de la grille
        colStatut = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colStatut : une colonne de texte de la grille
        pnlTitre.SuspendLayout()   ' Suspend le calcul de mise en page pendant le réglage (plus rapide, évite le scintillement)
        pnlFiltres.SuspendLayout()  ' Suspend la mise en page de pnlFiltres pendant sa configuration
        pnlDetail.SuspendLayout()  ' Suspend la mise en page de pnlDetail pendant sa configuration
        pnlBas.SuspendLayout()  ' Suspend la mise en page de pnlBas pendant sa configuration
        CType(dgvAlertes, System.ComponentModel.ISupportInitialize).BeginInit()   ' Prévient la grille que son initialisation commence
        SuspendLayout()  ' Suspend le calcul de la mise en page du formulaire pendant la configuration (plus rapide)
        '
        'pnlTitre
        '
        pnlTitre.BackColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(245, Byte), Integer))   ' Fond gris très clair (comme la maquette)
        pnlTitre.Controls.Add(lblTitre)  ' Place le titre dans le panneau
        pnlTitre.Dock = System.Windows.Forms.DockStyle.Top   ' Le panneau colle en haut et prend toute la largeur
        pnlTitre.Location = New System.Drawing.Point(0, 0)  ' Position : tout en haut à gauche
        pnlTitre.Name = "pnlTitre"  ' Nom interne de pnlTitre
        pnlTitre.Size = New System.Drawing.Size(1060, 44)  ' Taille : largeur 1060 px, hauteur 44 px
        pnlTitre.TabIndex = 0  ' Ordre de tabulation : n° 0
        '
        'lblTitre
        '
        lblTitre.AutoSize = True  ' lblTitre ajuste sa taille à son contenu
        lblTitre.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)   ' Police Segoe UI 12 points en gras
        lblTitre.ForeColor = System.Drawing.Color.FromArgb(CType(CType(23, Byte), Integer), CType(CType(38, Byte), Integer), CType(CType(31, Byte), Integer))   ' Texte vert très sombre (presque noir)
        lblTitre.Location = New System.Drawing.Point(14, 10)  ' Position : 14 px depuis la gauche, 10 px depuis le haut
        lblTitre.Name = "lblTitre"  ' Nom interne de lblTitre
        lblTitre.Size = New System.Drawing.Size(114, 21)  ' Taille : largeur 114 px, hauteur 21 px
        lblTitre.TabIndex = 0  ' Ordre de tabulation : n° 0
        lblTitre.Text = "Alertes de prix"  ' Texte affiché : « Alertes de prix »
        '
        'pnlFiltres
        '
        pnlFiltres.BackColor = System.Drawing.Color.White   ' Panneau de filtres blanc, comme dans la maquette
        pnlFiltres.Controls.Add(lblDu)  ' Place lblDu à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(dtpDebut)  ' Place dtpDebut à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(lblAu)  ' Place lblAu à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(dtpFin)  ' Place dtpFin à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(lblMagasin)  ' Place lblMagasin à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(cmbMagasin)  ' Place cmbMagasin à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(lblType)  ' Place lblType à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(cmbType)  ' Place cmbType à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(chkNonLues)  ' Place chkNonLues à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(btnFiltrer)  ' Place btnFiltrer à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(btnMarquerLue)  ' Place btnMarquerLue à l'intérieur de pnlFiltres
        pnlFiltres.Dock = System.Windows.Forms.DockStyle.Top   ' Dock = Top : le panneau colle en haut sous le titre
        pnlFiltres.Location = New System.Drawing.Point(0, 44)  ' Position : sous le titre
        pnlFiltres.Name = "pnlFiltres"  ' Nom interne de pnlFiltres
        pnlFiltres.Size = New System.Drawing.Size(1060, 74)  ' Taille : largeur 1060 px, hauteur 74 px
        pnlFiltres.TabIndex = 1  ' Ordre de tabulation : n° 1
        '
        'lblDu
        '
        lblDu.AutoSize = True  ' lblDu ajuste sa taille à son contenu
        lblDu.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)   ' Petit libellé en gras (style de la maquette)
        lblDu.Location = New System.Drawing.Point(18, 10)  ' Position : 18 px depuis la gauche, 10 px depuis le haut
        lblDu.Name = "lblDu"  ' Nom interne de lblDu
        lblDu.Size = New System.Drawing.Size(20, 13)  ' Taille : largeur 20 px, hauteur 13 px
        lblDu.TabIndex = 0  ' Ordre de tabulation : n° 0
        lblDu.Text = "Du"  ' Texte affiché : « Du »
        '
        'dtpDebut
        '
        dtpDebut.Format = System.Windows.Forms.DateTimePickerFormat.Short  ' Affiche la date au format court (ex. 01/08/2026)
        dtpDebut.Location = New System.Drawing.Point(18, 30)  ' Position : 18 px depuis la gauche, 30 px depuis le haut
        dtpDebut.Name = "dtpDebut"  ' Nom interne de dtpDebut
        dtpDebut.Size = New System.Drawing.Size(110, 23)  ' Taille : largeur 110 px, hauteur 23 px
        dtpDebut.TabIndex = 1  ' Ordre de tabulation : n° 1
        '
        'lblAu
        '
        lblAu.AutoSize = True  ' lblAu ajuste sa taille à son contenu
        lblAu.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)   ' Petit libellé en gras
        lblAu.Location = New System.Drawing.Point(148, 10)  ' Position : 148 px depuis la gauche, 10 px depuis le haut
        lblAu.Name = "lblAu"  ' Nom interne de lblAu
        lblAu.Size = New System.Drawing.Size(18, 13)  ' Taille : largeur 18 px, hauteur 13 px
        lblAu.TabIndex = 2  ' Ordre de tabulation : n° 2
        lblAu.Text = "Au"  ' Texte affiché : « Au »
        '
        'dtpFin
        '
        dtpFin.Format = System.Windows.Forms.DateTimePickerFormat.Short  ' Affiche la date au format court
        dtpFin.Location = New System.Drawing.Point(148, 30)  ' Position : 148 px depuis la gauche, 30 px depuis le haut
        dtpFin.Name = "dtpFin"  ' Nom interne de dtpFin
        dtpFin.Size = New System.Drawing.Size(110, 23)  ' Taille : largeur 110 px, hauteur 23 px
        dtpFin.TabIndex = 3  ' Ordre de tabulation : n° 3
        '
        'lblMagasin
        '
        lblMagasin.AutoSize = True  ' lblMagasin ajuste sa taille à son contenu
        lblMagasin.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)   ' Petit libellé en gras
        lblMagasin.Location = New System.Drawing.Point(278, 10)  ' Position : 278 px depuis la gauche, 10 px depuis le haut
        lblMagasin.Name = "lblMagasin"  ' Nom interne de lblMagasin
        lblMagasin.Size = New System.Drawing.Size(48, 13)  ' Taille : largeur 48 px, hauteur 13 px
        lblMagasin.TabIndex = 4  ' Ordre de tabulation : n° 4
        lblMagasin.Text = "Magasin"  ' Texte affiché : « Magasin »
        '
        'cmbMagasin
        '
        cmbMagasin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList   ' DropDownList : l'utilisateur choisit dans la liste, il ne peut pas taper n'importe quoi
        cmbMagasin.Location = New System.Drawing.Point(278, 30)  ' Position : 278 px depuis la gauche, 30 px depuis le haut
        cmbMagasin.Name = "cmbMagasin"  ' Nom interne de cmbMagasin
        cmbMagasin.Size = New System.Drawing.Size(160, 23)  ' Taille : largeur 160 px, hauteur 23 px
        cmbMagasin.TabIndex = 5  ' Ordre de tabulation : n° 5
        '
        'lblType
        '
        lblType.AutoSize = True  ' lblType ajuste sa taille à son contenu
        lblType.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)   ' Petit libellé en gras
        lblType.Location = New System.Drawing.Point(458, 10)  ' Position : 458 px depuis la gauche, 10 px depuis le haut
        lblType.Name = "lblType"  ' Nom interne de lblType
        lblType.Size = New System.Drawing.Size(31, 13)  ' Taille : largeur 31 px, hauteur 13 px
        lblType.TabIndex = 6  ' Ordre de tabulation : n° 6
        lblType.Text = "Type"  ' Texte affiché : « Type »
        '
        'cmbType
        '
        cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList  ' Liste déroulante où l'on ne peut que choisir (pas de saisie libre)
        cmbType.Location = New System.Drawing.Point(458, 30)  ' Position : 458 px depuis la gauche, 30 px depuis le haut
        cmbType.Name = "cmbType"  ' Nom interne de cmbType
        cmbType.Size = New System.Drawing.Size(130, 23)  ' Taille : largeur 130 px, hauteur 23 px
        cmbType.TabIndex = 7  ' Ordre de tabulation : n° 7
        '
        'chkNonLues
        '
        chkNonLues.AutoSize = True  ' chkNonLues ajuste sa taille à son contenu
        chkNonLues.Location = New System.Drawing.Point(608, 32)  ' Position : 608 px depuis la gauche, 32 px depuis le haut
        chkNonLues.Name = "chkNonLues"  ' Nom interne de chkNonLues
        chkNonLues.Size = New System.Drawing.Size(138, 19)  ' Taille : largeur 138 px, hauteur 19 px
        chkNonLues.TabIndex = 8  ' Ordre de tabulation : n° 8
        chkNonLues.Text = "Non lues uniquement"  ' Texte de la case à cocher (comme la maquette)
        chkNonLues.UseVisualStyleBackColor = True  ' Utilise l'apparence standard de Windows
        '
        'btnFiltrer
        '
        btnFiltrer.BackColor = System.Drawing.Color.FromArgb(CType(CType(46, Byte), Integer), CType(CType(107, Byte), Integer), CType(CType(74, Byte), Integer))   ' Fond vert forêt
        btnFiltrer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(46, Byte), Integer), CType(CType(107, Byte), Integer), CType(CType(74, Byte), Integer))   ' Bordure de même vert
        btnFiltrer.FlatStyle = System.Windows.Forms.FlatStyle.Flat  ' Bouton plat (permet de choisir les couleurs)
        btnFiltrer.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)   ' Texte en gras
        btnFiltrer.ForeColor = System.Drawing.Color.White  ' Texte blanc sur fond vert
        btnFiltrer.Location = New System.Drawing.Point(790, 28)  ' Position : 790 px (à droite de la case, sans la recouvrir) depuis la gauche, 28 px depuis le haut
        btnFiltrer.Name = "btnFiltrer"  ' Nom interne de btnFiltrer
        btnFiltrer.Size = New System.Drawing.Size(90, 28)  ' Taille : largeur 90 px, hauteur 28 px
        btnFiltrer.TabIndex = 9  ' Ordre de tabulation : n° 9
        btnFiltrer.Text = "Filtrer"   ' Bouton qui déclenche btnFiltrer_Click dans frmAlertes.vb
        btnFiltrer.UseVisualStyleBackColor = False  ' On garde la couleur choisie ci-dessus
        '
        'btnMarquerLue
        '
        btnMarquerLue.BackColor = System.Drawing.Color.White   ' Fond blanc (bouton « secondaire » de la maquette)
        btnMarquerLue.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(46, Byte), Integer), CType(CType(107, Byte), Integer), CType(CType(74, Byte), Integer))   ' Bordure vert forêt
        btnMarquerLue.FlatStyle = System.Windows.Forms.FlatStyle.Flat  ' Bouton plat
        btnMarquerLue.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)   ' Texte en gras
        btnMarquerLue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(58, Byte), Integer))   ' Texte vert forêt sombre
        btnMarquerLue.Location = New System.Drawing.Point(890, 28)  ' Position : 890 px depuis la gauche, 28 px depuis le haut
        btnMarquerLue.Name = "btnMarquerLue"  ' Nom interne de btnMarquerLue
        btnMarquerLue.Size = New System.Drawing.Size(180, 28)  ' Taille : largeur 180 px (assez large pour tout le texte), hauteur 28 px
        btnMarquerLue.TabIndex = 10  ' Ordre de tabulation : n° 10
        btnMarquerLue.Text = "Marquer comme lue"  ' Texte affiché par le bouton
        btnMarquerLue.UseVisualStyleBackColor = False  ' On garde la couleur choisie ci-dessus
        '
        'pnlDetail
        '
        pnlDetail.BackColor = System.Drawing.Color.White  ' Panneau blanc
        pnlDetail.Controls.Add(lblDetailTitre)  ' Place le titre du détail dans le panneau
        pnlDetail.Controls.Add(lblDetail)  ' Place le texte du détail dans le panneau
        pnlDetail.Dock = System.Windows.Forms.DockStyle.Bottom   ' Dock = Bottom : colle en bas, au-dessus de la barre du compteur
        pnlDetail.Location = New System.Drawing.Point(0, 474)  ' Position (recalculée par Dock)
        pnlDetail.Name = "pnlDetail"  ' Nom interne de pnlDetail
        pnlDetail.Size = New System.Drawing.Size(1060, 86)  ' Taille : largeur 1060 px, hauteur 86 px
        pnlDetail.TabIndex = 3  ' Ordre de tabulation : n° 3
        '
        'lblDetailTitre
        '
        lblDetailTitre.AutoSize = True  ' Ajuste sa taille à son contenu
        lblDetailTitre.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)   ' Titre en gras
        lblDetailTitre.Location = New System.Drawing.Point(18, 10)  ' Position : 18 px depuis la gauche, 10 px depuis le haut
        lblDetailTitre.Name = "lblDetailTitre"  ' Nom interne de lblDetailTitre
        lblDetailTitre.Size = New System.Drawing.Size(172, 15)  ' Taille : largeur 172 px, hauteur 15 px
        lblDetailTitre.TabIndex = 0  ' Ordre de tabulation : n° 0
        lblDetailTitre.Text = "Détail de l'alerte sélectionnée"  ' Texte par défaut (remplacé quand une ligne est sélectionnée)
        '
        'lblDetail
        '
        lblDetail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)   ' S'étire avec la largeur de la fenêtre
        lblDetail.ForeColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(61, Byte), Integer))   ' Texte vert-gris sombre
        lblDetail.Location = New System.Drawing.Point(18, 34)  ' Position : 18 px depuis la gauche, 34 px depuis le haut
        lblDetail.Name = "lblDetail"  ' Nom interne de lblDetail
        lblDetail.Size = New System.Drawing.Size(1024, 42)  ' Taille : largeur 1024 px, hauteur 42 px (le texte passe à la ligne)
        lblDetail.TabIndex = 1  ' Ordre de tabulation : n° 1
        '
        'pnlBas
        '
        pnlBas.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(230, Byte), Integer))   ' Fond vert très clair
        pnlBas.Controls.Add(lblCompteur)  ' Place lblCompteur à l'intérieur de pnlBas
        pnlBas.Dock = System.Windows.Forms.DockStyle.Bottom   ' Dock = Bottom : colle tout en bas
        pnlBas.Location = New System.Drawing.Point(0, 560)  ' Position (recalculée par Dock)
        pnlBas.Name = "pnlBas"  ' Nom interne de pnlBas
        pnlBas.Size = New System.Drawing.Size(1060, 30)  ' Taille : largeur 1060 px, hauteur 30 px
        pnlBas.TabIndex = 4  ' Ordre de tabulation : n° 4
        '
        'lblCompteur
        '
        lblCompteur.AutoSize = True  ' lblCompteur ajuste sa taille à son contenu
        lblCompteur.Location = New System.Drawing.Point(18, 7)  ' Position : 18 px depuis la gauche, 7 px depuis le haut
        lblCompteur.Name = "lblCompteur"  ' Nom interne de lblCompteur
        lblCompteur.Size = New System.Drawing.Size(0, 15)  ' Taille : vide au départ (texte rempli par Rafraichir)
        lblCompteur.TabIndex = 0  ' Ordre de tabulation : n° 0
        '
        'dgvAlertes
        '
        dgvAlertes.AllowUserToAddRows = False   ' La grille est en lecture seule : pas d'ajout de ligne par l'utilisateur
        dgvAlertes.AllowUserToDeleteRows = False  ' L'utilisateur ne peut pas supprimer de ligne dans la grille
        dgvAlertes.AllowUserToResizeRows = False  ' La hauteur des lignes est fixe
        dgvAlertes.AutoGenerateColumns = False   ' Les colonnes sont définies à la main ci-dessous (pas créées automatiquement)
        dgvAlertes.BackgroundColor = System.Drawing.Color.White  ' Fond blanc de la grille
        dgvAlertes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle  ' Fine bordure autour de la grille
        dgvAlertes.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single  ' Quadrillage complet comme la maquette
        dgvAlertes.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single  ' En-têtes avec bordure simple
        dgvAlertes.ColumnHeadersDefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.BackColor = System.Drawing.Color.FromArgb(227, 237, 230), .ForeColor = System.Drawing.Color.FromArgb(23, 38, 31), .Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point), .SelectionBackColor = System.Drawing.Color.FromArgb(227, 237, 230), .SelectionForeColor = System.Drawing.Color.FromArgb(23, 38, 31)}   ' En-tête vert très clair, texte sombre en gras
        dgvAlertes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize  ' La hauteur de l'en-tête s'ajuste à son contenu
        dgvAlertes.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {colDate, colType, colProduit, colMagasin, colVariation, colStatut})   ' Ajoute les 6 colonnes à la grille, dans l'ordre de la maquette
        dgvAlertes.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.SelectionBackColor = System.Drawing.Color.FromArgb(221, 235, 226), .SelectionForeColor = System.Drawing.Color.FromArgb(23, 38, 31)}   ' Ligne sélectionnée : vert clair, texte sombre
        dgvAlertes.Dock = System.Windows.Forms.DockStyle.Fill   ' Fill : la grille occupe tout l'espace restant
        dgvAlertes.EnableHeadersVisualStyles = False  ' Obligatoire pour que la couleur d'en-tête choisie soit appliquée
        dgvAlertes.GridColor = System.Drawing.Color.FromArgb(195, 208, 200)  ' Couleur des traits du quadrillage : gris-vert
        dgvAlertes.Location = New System.Drawing.Point(0, 118)  ' Position (recalculée par Dock)
        dgvAlertes.MultiSelect = False  ' Une seule alerte sélectionnée à la fois (le détail correspond à cette alerte)
        dgvAlertes.Name = "dgvAlertes"  ' Nom interne de dgvAlertes
        dgvAlertes.ReadOnly = True  ' Lecture seule (l'utilisateur ne peut pas modifier)
        dgvAlertes.RowHeadersVisible = False  ' Cache la colonne grise de gauche (en-têtes de lignes)
        dgvAlertes.RowTemplate.Height = 28  ' Hauteur des lignes : 28 px
        dgvAlertes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect   ' Un clic sélectionne toute la ligne
        dgvAlertes.Size = New System.Drawing.Size(1060, 356)  ' Taille (recalculée par Dock)
        dgvAlertes.TabIndex = 2  ' Ordre de tabulation : n° 2
        '
        'colDate
        '
        colDate.DataPropertyName = "DateTexte"   ' DataPropertyName : propriété de LigneAlerte (dans frmAlertes.vb) affichée dans cette colonne
        colDate.HeaderText = "Date"  ' Titre de la colonne
        colDate.Name = "colDate"  ' Nom interne
        colDate.ReadOnly = True  ' Lecture seule
        colDate.Width = 100  ' Largeur : 100 px
        '
        'colType
        '
        colType.DataPropertyName = "TypeTexte"  ' La colonne affiche la propriété « TypeTexte » de LigneAlerte
        colType.HeaderText = "Type"  ' Titre de la colonne
        colType.Name = "colType"  ' Nom interne
        colType.ReadOnly = True  ' Lecture seule
        colType.Width = 130  ' Largeur : 130 px
        '
        'colProduit
        '
        colProduit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill   ' Fill : cette colonne prend toute la largeur restante
        colProduit.DataPropertyName = "Produit"  ' La colonne affiche la propriété « Produit » de LigneAlerte
        colProduit.HeaderText = "Produit"  ' Titre de la colonne
        colProduit.Name = "colProduit"  ' Nom interne
        colProduit.ReadOnly = True  ' Lecture seule
        '
        'colMagasin
        '
        colMagasin.DataPropertyName = "Magasin"  ' La colonne affiche la propriété « Magasin » de LigneAlerte
        colMagasin.HeaderText = "Magasin"  ' Titre de la colonne
        colMagasin.Name = "colMagasin"  ' Nom interne
        colMagasin.ReadOnly = True  ' Lecture seule
        colMagasin.Width = 170  ' Largeur : 170 px
        '
        'colVariation
        '
        colVariation.DataPropertyName = "VariationTexte"  ' La colonne affiche la propriété « VariationTexte » de LigneAlerte
        colVariation.HeaderText = "Variation prix unitaire"  ' Titre de la colonne (comme la maquette)
        colVariation.Name = "colVariation"  ' Nom interne
        colVariation.ReadOnly = True  ' Lecture seule
        colVariation.Width = 170  ' Largeur : 170 px
        '
        'colStatut
        '
        colStatut.DataPropertyName = "StatutTexte"  ' La colonne affiche la propriété « StatutTexte » de LigneAlerte
        colStatut.HeaderText = "Statut"  ' Titre de la colonne
        colStatut.Name = "colStatut"  ' Nom interne
        colStatut.ReadOnly = True  ' Lecture seule
        colStatut.Width = 90  ' Largeur : 90 px
        '
        'frmAlertes
        '
        AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)  ' Dimensions de référence pour l'adaptation à l'écran (DPI)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font   ' La fenêtre s'adapte à la taille de police de l'écran
        BackColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(245, Byte), Integer))   ' Fond gris très clair de la fenêtre
        ClientSize = New System.Drawing.Size(1100, 590)  ' Taille de la zone intérieure : 1100 x 590 px
        Controls.Add(dgvAlertes)   ' Ajoute les zones à la fenêtre : la grille (Fill) en premier, puis bas, puis haut
        Controls.Add(pnlDetail)  ' Ajoute le panneau de détail
        Controls.Add(pnlBas)  ' Ajoute la barre du bas
        Controls.Add(pnlFiltres)  ' Ajoute le panneau des filtres
        Controls.Add(pnlTitre)  ' Ajoute le titre (tout en haut)
        MinimumSize = New System.Drawing.Size(1116, 420)   ' Empêche de réduire la fenêtre au point de cacher les filtres
        Name = "frmAlertes"  ' Nom interne du formulaire
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent   ' S'ouvre au centre de la fenêtre qui l'a ouverte
        Text = "Alertes de prix"   ' Titre de la fenêtre
        pnlTitre.ResumeLayout(False)  ' Fin du réglage : reprend le calcul de la mise en page
        pnlTitre.PerformLayout()  ' Applique la mise en page en attente
        pnlFiltres.ResumeLayout(False)  ' Reprend la mise en page de pnlFiltres
        pnlFiltres.PerformLayout()  ' Applique la mise en page en attente de pnlFiltres
        pnlDetail.ResumeLayout(False)  ' Reprend la mise en page de pnlDetail
        pnlDetail.PerformLayout()  ' Applique la mise en page en attente de pnlDetail
        pnlBas.ResumeLayout(False)  ' Reprend la mise en page de pnlBas
        pnlBas.PerformLayout()  ' Applique la mise en page en attente de pnlBas
        CType(dgvAlertes, System.ComponentModel.ISupportInitialize).EndInit()   ' Prévient la grille que son initialisation est terminée
        ResumeLayout(False)  ' Reprend le calcul de la mise en page du formulaire
    End Sub  ' Fin de la méthode

    Friend WithEvents pnlTitre As System.Windows.Forms.Panel   ' Déclarations des contrôles : WithEvents permet d'écrire « Handles ... » dans frmAlertes.vb
    Friend WithEvents lblTitre As System.Windows.Forms.Label  ' Titre de l'écran
    Friend WithEvents pnlFiltres As System.Windows.Forms.Panel  ' Panneau des filtres
    Friend WithEvents lblDu As System.Windows.Forms.Label  ' Libellé « Du »
    Friend WithEvents dtpDebut As System.Windows.Forms.DateTimePicker  ' Date de début
    Friend WithEvents lblAu As System.Windows.Forms.Label  ' Libellé « Au »
    Friend WithEvents dtpFin As System.Windows.Forms.DateTimePicker  ' Date de fin
    Friend WithEvents lblMagasin As System.Windows.Forms.Label  ' Libellé « Magasin »
    Friend WithEvents cmbMagasin As System.Windows.Forms.ComboBox  ' Liste des magasins
    Friend WithEvents lblType As System.Windows.Forms.Label  ' Libellé « Type »
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox  ' Liste des types d'alerte
    Friend WithEvents chkNonLues As System.Windows.Forms.CheckBox  ' Case « Non lues uniquement »
    Friend WithEvents btnFiltrer As System.Windows.Forms.Button  ' Bouton « Filtrer »
    Friend WithEvents btnMarquerLue As System.Windows.Forms.Button  ' Bouton « Marquer comme lue »
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel  ' Panneau du détail
    Friend WithEvents lblDetailTitre As System.Windows.Forms.Label  ' Titre du détail
    Friend WithEvents lblDetail As System.Windows.Forms.Label  ' Texte du détail
    Friend WithEvents pnlBas As System.Windows.Forms.Panel  ' Barre du bas
    Friend WithEvents lblCompteur As System.Windows.Forms.Label  ' Compteur d'alertes
    Friend WithEvents dgvAlertes As System.Windows.Forms.DataGridView  ' La grille des alertes
    Friend WithEvents colDate As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne Date
    Friend WithEvents colType As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne Type
    Friend WithEvents colProduit As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne Produit
    Friend WithEvents colMagasin As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne Magasin
    Friend WithEvents colVariation As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne Variation prix unitaire
    Friend WithEvents colStatut As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne Statut

End Class  ' Fin de la classe
