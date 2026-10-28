' ============================================================================================
' FICHIER GÉNÉRÉ PAR LE CONCEPTEUR DE FORMULAIRES DE VISUAL STUDIO.
' Il décrit la fenêtre frmAlertes : quels contrôles existent, où ils sont et quelles propriétés ils ont.
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
        pnlFiltres = New System.Windows.Forms.Panel()   ' --- Création de tous les contrôles (New) : pour l'instant ils existent en mémoire mais ne sont pas réglés ---
        chkPeriode = New System.Windows.Forms.CheckBox()  ' Crée chkPeriode : une case à cocher
        lblDu = New System.Windows.Forms.Label()  ' Crée lblDu : une étiquette de texte
        dtpDebut = New System.Windows.Forms.DateTimePicker()  ' Crée dtpDebut : un sélecteur de date
        lblAu = New System.Windows.Forms.Label()  ' Crée lblAu : une étiquette de texte
        dtpFin = New System.Windows.Forms.DateTimePicker()  ' Crée dtpFin : un sélecteur de date
        lblMagasin = New System.Windows.Forms.Label()  ' Crée lblMagasin : une étiquette de texte
        cmbMagasin = New System.Windows.Forms.ComboBox()  ' Crée cmbMagasin : une liste déroulante
        lblType = New System.Windows.Forms.Label()  ' Crée lblType : une étiquette de texte
        cmbType = New System.Windows.Forms.ComboBox()  ' Crée cmbType : une liste déroulante
        lblStatut = New System.Windows.Forms.Label()  ' Crée lblStatut : une étiquette de texte
        cmbStatut = New System.Windows.Forms.ComboBox()  ' Crée cmbStatut : une liste déroulante
        btnFiltrer = New System.Windows.Forms.Button()  ' Crée btnFiltrer : un bouton
        btnReinitialiser = New System.Windows.Forms.Button()  ' Crée btnReinitialiser : un bouton
        pnlBas = New System.Windows.Forms.Panel()  ' Crée pnlBas : un panneau (conteneur)
        lblCompteur = New System.Windows.Forms.Label()  ' Crée lblCompteur : une étiquette de texte
        btnMarquerLue = New System.Windows.Forms.Button()  ' Crée btnMarquerLue : un bouton
        btnToutLire = New System.Windows.Forms.Button()  ' Crée btnToutLire : un bouton
        dgvAlertes = New System.Windows.Forms.DataGridView()  ' Crée dgvAlertes : une grille de données (tableau)
        colDate = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colDate : une colonne de texte de la grille
        colProduit = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colProduit : une colonne de texte de la grille
        colMagasin = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colMagasin : une colonne de texte de la grille
        colType = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colType : une colonne de texte de la grille
        colVariation = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colVariation : une colonne de texte de la grille
        colDetail = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colDetail : une colonne de texte de la grille
        colStatut = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colStatut : une colonne de texte de la grille
        pnlFiltres.SuspendLayout()   ' Suspend le calcul de mise en page pendant le réglage (plus rapide, évite le scintillement)
        pnlBas.SuspendLayout()  ' Suspend la mise en page de pnlBas pendant sa configuration
        CType(dgvAlertes, System.ComponentModel.ISupportInitialize).BeginInit()   ' Prévient la grille que son initialisation commence
        SuspendLayout()  ' Suspend le calcul de la mise en page du formulaire pendant la configuration (plus rapide)
        '
        'pnlFiltres
        '
        pnlFiltres.Controls.Add(chkPeriode)   ' Place chaque contrôle de filtre dans le panneau du haut
        pnlFiltres.Controls.Add(lblDu)  ' Place lblDu à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(dtpDebut)  ' Place dtpDebut à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(lblAu)  ' Place lblAu à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(dtpFin)  ' Place dtpFin à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(lblMagasin)  ' Place lblMagasin à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(cmbMagasin)  ' Place cmbMagasin à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(lblType)  ' Place lblType à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(cmbType)  ' Place cmbType à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(lblStatut)  ' Place lblStatut à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(cmbStatut)  ' Place cmbStatut à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(btnFiltrer)  ' Place btnFiltrer à l'intérieur de pnlFiltres
        pnlFiltres.Controls.Add(btnReinitialiser)  ' Place btnReinitialiser à l'intérieur de pnlFiltres
        pnlFiltres.Dock = System.Windows.Forms.DockStyle.Top   ' Dock = Top : le panneau colle en haut et prend toute la largeur
        pnlFiltres.Location = New System.Drawing.Point(0, 0)  ' Position de pnlFiltres : 0 px depuis la gauche, 0 px depuis le haut
        pnlFiltres.Name = "pnlFiltres"  ' Nom interne de pnlFiltres (utilisé par Visual Studio)
        pnlFiltres.Size = New System.Drawing.Size(984, 96)  ' Taille de pnlFiltres : largeur 984 px, hauteur 96 px
        pnlFiltres.TabIndex = 0  ' Ordre de pnlFiltres quand on navigue avec la touche Tab : n° 0
        '
        'chkPeriode
        '
        chkPeriode.AutoSize = True  ' chkPeriode ajuste sa taille à son contenu
        chkPeriode.Location = New System.Drawing.Point(12, 14)  ' Position de chkPeriode : 12 px depuis la gauche, 14 px depuis le haut
        chkPeriode.Name = "chkPeriode"  ' Nom interne de chkPeriode (utilisé par Visual Studio)
        chkPeriode.Size = New System.Drawing.Size(129, 19)  ' Taille de chkPeriode : largeur 129 px, hauteur 19 px
        chkPeriode.TabIndex = 0  ' Ordre de chkPeriode quand on navigue avec la touche Tab : n° 0
        chkPeriode.Text = "Filtrer par période"   ' Texte de la case à cocher qui active le filtre de période
        chkPeriode.UseVisualStyleBackColor = True  ' chkPeriode utilise l'apparence standard de Windows
        '
        'lblDu
        '
        lblDu.AutoSize = True  ' lblDu ajuste sa taille à son contenu
        lblDu.Location = New System.Drawing.Point(160, 15)  ' Position de lblDu : 160 px depuis la gauche, 15 px depuis le haut
        lblDu.Name = "lblDu"  ' Nom interne de lblDu (utilisé par Visual Studio)
        lblDu.Size = New System.Drawing.Size(21, 15)  ' Taille de lblDu : largeur 21 px, hauteur 15 px
        lblDu.TabIndex = 1  ' Ordre de lblDu quand on navigue avec la touche Tab : n° 1
        lblDu.Text = "Du"  ' Texte affiché par lblDu : « Du »
        '
        'dtpDebut
        '
        dtpDebut.Enabled = False   ' Désactivé tant que « Filtrer par période » n'est pas coché (voir chkPeriode_CheckedChanged)
        dtpDebut.Format = System.Windows.Forms.DateTimePickerFormat.Short  ' dtpDebut affiche la date au format court (ex. 15/10/2026)
        dtpDebut.Location = New System.Drawing.Point(190, 11)  ' Position de dtpDebut : 190 px depuis la gauche, 11 px depuis le haut
        dtpDebut.Name = "dtpDebut"  ' Nom interne de dtpDebut (utilisé par Visual Studio)
        dtpDebut.Size = New System.Drawing.Size(110, 23)  ' Taille de dtpDebut : largeur 110 px, hauteur 23 px
        dtpDebut.TabIndex = 2  ' Ordre de dtpDebut quand on navigue avec la touche Tab : n° 2
        '
        'lblAu
        '
        lblAu.AutoSize = True  ' lblAu ajuste sa taille à son contenu
        lblAu.Location = New System.Drawing.Point(312, 15)  ' Position de lblAu : 312 px depuis la gauche, 15 px depuis le haut
        lblAu.Name = "lblAu"  ' Nom interne de lblAu (utilisé par Visual Studio)
        lblAu.Size = New System.Drawing.Size(20, 15)  ' Taille de lblAu : largeur 20 px, hauteur 15 px
        lblAu.TabIndex = 3  ' Ordre de lblAu quand on navigue avec la touche Tab : n° 3
        lblAu.Text = "au"  ' Texte affiché par lblAu : « au »
        '
        'dtpFin
        '
        dtpFin.Enabled = False  ' dtpFin est désactivé au départ (activé quand on coche la période)
        dtpFin.Format = System.Windows.Forms.DateTimePickerFormat.Short  ' dtpFin affiche la date au format court (ex. 15/10/2026)
        dtpFin.Location = New System.Drawing.Point(340, 11)  ' Position de dtpFin : 340 px depuis la gauche, 11 px depuis le haut
        dtpFin.Name = "dtpFin"  ' Nom interne de dtpFin (utilisé par Visual Studio)
        dtpFin.Size = New System.Drawing.Size(110, 23)  ' Taille de dtpFin : largeur 110 px, hauteur 23 px
        dtpFin.TabIndex = 4  ' Ordre de dtpFin quand on navigue avec la touche Tab : n° 4
        '
        'lblMagasin
        '
        lblMagasin.AutoSize = True  ' lblMagasin ajuste sa taille à son contenu
        lblMagasin.Location = New System.Drawing.Point(12, 59)  ' Position de lblMagasin : 12 px depuis la gauche, 59 px depuis le haut
        lblMagasin.Name = "lblMagasin"  ' Nom interne de lblMagasin (utilisé par Visual Studio)
        lblMagasin.Size = New System.Drawing.Size(51, 15)  ' Taille de lblMagasin : largeur 51 px, hauteur 15 px
        lblMagasin.TabIndex = 5  ' Ordre de lblMagasin quand on navigue avec la touche Tab : n° 5
        lblMagasin.Text = "Magasin"  ' Texte affiché par lblMagasin : « Magasin »
        '
        'cmbMagasin
        '
        cmbMagasin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList   ' DropDownList : l'utilisateur choisit dans la liste, il ne peut pas taper n'importe quoi
        cmbMagasin.Location = New System.Drawing.Point(70, 55)  ' Position de cmbMagasin : 70 px depuis la gauche, 55 px depuis le haut
        cmbMagasin.Name = "cmbMagasin"  ' Nom interne de cmbMagasin (utilisé par Visual Studio)
        cmbMagasin.Size = New System.Drawing.Size(150, 23)  ' Taille de cmbMagasin : largeur 150 px, hauteur 23 px
        cmbMagasin.TabIndex = 6  ' Ordre de cmbMagasin quand on navigue avec la touche Tab : n° 6
        '
        'lblType
        '
        lblType.AutoSize = True  ' lblType ajuste sa taille à son contenu
        lblType.Location = New System.Drawing.Point(236, 59)  ' Position de lblType : 236 px depuis la gauche, 59 px depuis le haut
        lblType.Name = "lblType"  ' Nom interne de lblType (utilisé par Visual Studio)
        lblType.Size = New System.Drawing.Size(32, 15)  ' Taille de lblType : largeur 32 px, hauteur 15 px
        lblType.TabIndex = 7  ' Ordre de lblType quand on navigue avec la touche Tab : n° 7
        lblType.Text = "Type"  ' Texte affiché par lblType : « Type »
        '
        'cmbType
        '
        cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList  ' cmbType : liste déroulante où l'on ne peut que choisir (pas de saisie libre)
        cmbType.Location = New System.Drawing.Point(276, 55)  ' Position de cmbType : 276 px depuis la gauche, 55 px depuis le haut
        cmbType.Name = "cmbType"  ' Nom interne de cmbType (utilisé par Visual Studio)
        cmbType.Size = New System.Drawing.Size(140, 23)  ' Taille de cmbType : largeur 140 px, hauteur 23 px
        cmbType.TabIndex = 8  ' Ordre de cmbType quand on navigue avec la touche Tab : n° 8
        '
        'lblStatut
        '
        lblStatut.AutoSize = True  ' lblStatut ajuste sa taille à son contenu
        lblStatut.Location = New System.Drawing.Point(432, 59)  ' Position de lblStatut : 432 px depuis la gauche, 59 px depuis le haut
        lblStatut.Name = "lblStatut"  ' Nom interne de lblStatut (utilisé par Visual Studio)
        lblStatut.Size = New System.Drawing.Size(40, 15)  ' Taille de lblStatut : largeur 40 px, hauteur 15 px
        lblStatut.TabIndex = 9  ' Ordre de lblStatut quand on navigue avec la touche Tab : n° 9
        lblStatut.Text = "Statut"  ' Texte affiché par lblStatut : « Statut »
        '
        'cmbStatut
        '
        cmbStatut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList  ' cmbStatut : liste déroulante où l'on ne peut que choisir (pas de saisie libre)
        cmbStatut.Location = New System.Drawing.Point(480, 55)  ' Position de cmbStatut : 480 px depuis la gauche, 55 px depuis le haut
        cmbStatut.Name = "cmbStatut"  ' Nom interne de cmbStatut (utilisé par Visual Studio)
        cmbStatut.Size = New System.Drawing.Size(120, 23)  ' Taille de cmbStatut : largeur 120 px, hauteur 23 px
        cmbStatut.TabIndex = 10  ' Ordre de cmbStatut quand on navigue avec la touche Tab : n° 10
        '
        'btnFiltrer
        '
        btnFiltrer.Location = New System.Drawing.Point(620, 54)  ' Position de btnFiltrer : 620 px depuis la gauche, 54 px depuis le haut
        btnFiltrer.Name = "btnFiltrer"  ' Nom interne de btnFiltrer (utilisé par Visual Studio)
        btnFiltrer.Size = New System.Drawing.Size(90, 25)  ' Taille de btnFiltrer : largeur 90 px, hauteur 25 px
        btnFiltrer.TabIndex = 11  ' Ordre de btnFiltrer quand on navigue avec la touche Tab : n° 11
        btnFiltrer.Text = "Filtrer"   ' Bouton qui déclenche btnFiltrer_Click dans frmAlertes.vb
        btnFiltrer.UseVisualStyleBackColor = True  ' btnFiltrer utilise l'apparence standard de Windows
        '
        'btnReinitialiser
        '
        btnReinitialiser.Location = New System.Drawing.Point(716, 54)  ' Position de btnReinitialiser : 716 px depuis la gauche, 54 px depuis le haut
        btnReinitialiser.Name = "btnReinitialiser"  ' Nom interne de btnReinitialiser (utilisé par Visual Studio)
        btnReinitialiser.Size = New System.Drawing.Size(100, 25)  ' Taille de btnReinitialiser : largeur 100 px, hauteur 25 px
        btnReinitialiser.TabIndex = 12  ' Ordre de btnReinitialiser quand on navigue avec la touche Tab : n° 12
        btnReinitialiser.Text = "Réinitialiser"   ' Bouton qui déclenche btnReinitialiser_Click
        btnReinitialiser.UseVisualStyleBackColor = True  ' btnReinitialiser utilise l'apparence standard de Windows
        '
        'pnlBas
        '
        pnlBas.Controls.Add(lblCompteur)  ' Place lblCompteur à l'intérieur de pnlBas
        pnlBas.Controls.Add(btnMarquerLue)  ' Place btnMarquerLue à l'intérieur de pnlBas
        pnlBas.Controls.Add(btnToutLire)  ' Place btnToutLire à l'intérieur de pnlBas
        pnlBas.Dock = System.Windows.Forms.DockStyle.Bottom   ' Dock = Bottom : le panneau colle en bas (compteur et boutons de lecture)
        pnlBas.Location = New System.Drawing.Point(0, 521)  ' Position de pnlBas : 0 px depuis la gauche, 521 px depuis le haut
        pnlBas.Name = "pnlBas"  ' Nom interne de pnlBas (utilisé par Visual Studio)
        pnlBas.Size = New System.Drawing.Size(984, 48)  ' Taille de pnlBas : largeur 984 px, hauteur 48 px
        pnlBas.TabIndex = 2  ' Ordre de pnlBas quand on navigue avec la touche Tab : n° 2
        '
        'lblCompteur
        '
        lblCompteur.AutoSize = True  ' lblCompteur ajuste sa taille à son contenu
        lblCompteur.Location = New System.Drawing.Point(12, 17)  ' Position de lblCompteur : 12 px depuis la gauche, 17 px depuis le haut
        lblCompteur.Name = "lblCompteur"  ' Nom interne de lblCompteur (utilisé par Visual Studio)
        lblCompteur.Size = New System.Drawing.Size(0, 15)  ' Taille de lblCompteur : largeur 0 px, hauteur 15 px
        lblCompteur.TabIndex = 0  ' Ordre de lblCompteur quand on navigue avec la touche Tab : n° 0
        '
        'btnMarquerLue
        '
        btnMarquerLue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)   ' Anchor Top + Right : le bouton reste accroché au bord droit quand on agrandit la fenêtre
        btnMarquerLue.Location = New System.Drawing.Point(654, 11)  ' Position de btnMarquerLue : 654 px depuis la gauche, 11 px depuis le haut
        btnMarquerLue.Name = "btnMarquerLue"  ' Nom interne de btnMarquerLue (utilisé par Visual Studio)
        btnMarquerLue.Size = New System.Drawing.Size(170, 27)  ' Taille de btnMarquerLue : largeur 170 px, hauteur 27 px
        btnMarquerLue.TabIndex = 1  ' Ordre de btnMarquerLue quand on navigue avec la touche Tab : n° 1
        btnMarquerLue.Text = "Marquer comme lue"  ' Texte affiché par btnMarquerLue : « Marquer comme lue »
        btnMarquerLue.UseVisualStyleBackColor = True  ' btnMarquerLue utilise l'apparence standard de Windows
        '
        'btnToutLire
        '
        btnToutLire.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)  ' btnToutLire reste accroché au bord haut et au bord droit quand la fenêtre est redimensionnée
        btnToutLire.Location = New System.Drawing.Point(830, 11)  ' Position de btnToutLire : 830 px depuis la gauche, 11 px depuis le haut
        btnToutLire.Name = "btnToutLire"  ' Nom interne de btnToutLire (utilisé par Visual Studio)
        btnToutLire.Size = New System.Drawing.Size(142, 27)  ' Taille de btnToutLire : largeur 142 px, hauteur 27 px
        btnToutLire.TabIndex = 2  ' Ordre de btnToutLire quand on navigue avec la touche Tab : n° 2
        btnToutLire.Text = "Tout marquer lu"  ' Texte affiché par btnToutLire : « Tout marquer lu »
        btnToutLire.UseVisualStyleBackColor = True  ' btnToutLire utilise l'apparence standard de Windows
        '
        'dgvAlertes
        '
        dgvAlertes.AllowUserToAddRows = False   ' La grille est en lecture seule : pas d'ajout de ligne par l'utilisateur
        dgvAlertes.AllowUserToDeleteRows = False  ' L'utilisateur ne peut pas supprimer de ligne dans la grille
        dgvAlertes.AutoGenerateColumns = False   ' Les colonnes sont définies à la main ci-dessous (pas créées automatiquement)
        dgvAlertes.BackgroundColor = System.Drawing.SystemColors.Window  ' Couleur de fond de la grille : celle des zones de saisie de Windows
        dgvAlertes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize  ' La hauteur de l'en-tête s'ajuste à son contenu
        dgvAlertes.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {colDate, colProduit, colMagasin, colType, colVariation, colDetail, colStatut})   ' Ajoute les 7 colonnes à la grille, dans l'ordre d'affichage
        dgvAlertes.Dock = System.Windows.Forms.DockStyle.Fill   ' Fill : la grille occupe tout l'espace restant entre le haut et le bas
        dgvAlertes.Location = New System.Drawing.Point(0, 96)  ' Position de dgvAlertes : 0 px depuis la gauche, 96 px depuis le haut
        dgvAlertes.Name = "dgvAlertes"  ' Nom interne de dgvAlertes (utilisé par Visual Studio)
        dgvAlertes.ReadOnly = True  ' dgvAlertes est en lecture seule (l'utilisateur ne peut pas modifier)
        dgvAlertes.RowHeadersVisible = False  ' Cache la colonne grise de gauche (en-têtes de lignes)
        dgvAlertes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect   ' Un clic sélectionne toute la ligne (utile pour « Marquer comme lue »)
        dgvAlertes.Size = New System.Drawing.Size(984, 425)  ' Taille de dgvAlertes : largeur 984 px, hauteur 425 px
        dgvAlertes.TabIndex = 1  ' Ordre de dgvAlertes quand on navigue avec la touche Tab : n° 1
        '
        'colDate
        '
        colDate.DataPropertyName = "DateTexte"   ' DataPropertyName : propriété de LigneAlerte (dans frmAlertes.vb) affichée dans cette colonne
        colDate.HeaderText = "Date"  ' Titre de la colonne colDate : « Date »
        colDate.Name = "colDate"  ' Nom interne de colDate (utilisé par Visual Studio)
        colDate.ReadOnly = True  ' colDate est en lecture seule (l'utilisateur ne peut pas modifier)
        colDate.Width = 90  ' Largeur de la colonne colDate : 90 px
        '
        'colProduit
        '
        colProduit.DataPropertyName = "Produit"  ' La colonne colProduit affiche la propriété « Produit » de LigneAlerte
        colProduit.HeaderText = "Produit"  ' Titre de la colonne colProduit : « Produit »
        colProduit.Name = "colProduit"  ' Nom interne de colProduit (utilisé par Visual Studio)
        colProduit.ReadOnly = True  ' colProduit est en lecture seule (l'utilisateur ne peut pas modifier)
        colProduit.Width = 170  ' Largeur de la colonne colProduit : 170 px
        '
        'colMagasin
        '
        colMagasin.DataPropertyName = "Magasin"  ' La colonne colMagasin affiche la propriété « Magasin » de LigneAlerte
        colMagasin.HeaderText = "Magasin"  ' Titre de la colonne colMagasin : « Magasin »
        colMagasin.Name = "colMagasin"  ' Nom interne de colMagasin (utilisé par Visual Studio)
        colMagasin.ReadOnly = True  ' colMagasin est en lecture seule (l'utilisateur ne peut pas modifier)
        colMagasin.Width = 130  ' Largeur de la colonne colMagasin : 130 px
        '
        'colType
        '
        colType.DataPropertyName = "TypeTexte"  ' La colonne colType affiche la propriété « TypeTexte » de LigneAlerte
        colType.HeaderText = "Type"  ' Titre de la colonne colType : « Type »
        colType.Name = "colType"  ' Nom interne de colType (utilisé par Visual Studio)
        colType.ReadOnly = True  ' colType est en lecture seule (l'utilisateur ne peut pas modifier)
        colType.Width = 110  ' Largeur de la colonne colType : 110 px
        '
        'colVariation
        '
        colVariation.DataPropertyName = "VariationTexte"  ' La colonne colVariation affiche la propriété « VariationTexte » de LigneAlerte
        colVariation.HeaderText = "Variation du prix unitaire"  ' Titre de la colonne colVariation : « Variation du prix unitaire »
        colVariation.Name = "colVariation"  ' Nom interne de colVariation (utilisé par Visual Studio)
        colVariation.ReadOnly = True  ' colVariation est en lecture seule (l'utilisateur ne peut pas modifier)
        colVariation.Width = 150  ' Largeur de la colonne colVariation : 150 px
        '
        'colDetail
        '
        colDetail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill   ' Fill : cette colonne prend toute la largeur restante (le détail est long)
        colDetail.DataPropertyName = "Detail"  ' La colonne colDetail affiche la propriété « Detail » de LigneAlerte
        colDetail.HeaderText = "Détail"  ' Titre de la colonne colDetail : « Détail »
        colDetail.Name = "colDetail"  ' Nom interne de colDetail (utilisé par Visual Studio)
        colDetail.ReadOnly = True  ' colDetail est en lecture seule (l'utilisateur ne peut pas modifier)
        '
        'colStatut
        '
        colStatut.DataPropertyName = "StatutTexte"  ' La colonne colStatut affiche la propriété « StatutTexte » de LigneAlerte
        colStatut.HeaderText = "Statut"  ' Titre de la colonne colStatut : « Statut »
        colStatut.Name = "colStatut"  ' Nom interne de colStatut (utilisé par Visual Studio)
        colStatut.ReadOnly = True  ' colStatut est en lecture seule (l'utilisateur ne peut pas modifier)
        colStatut.Width = 80  ' Largeur de la colonne colStatut : 80 px
        '
        'frmAlertes
        '
        AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)  ' Dimensions de référence pour l'adaptation à l'écran (DPI)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font   ' La fenêtre s'adapte à la taille de police de l'écran (écrans haute résolution)
        ClientSize = New System.Drawing.Size(984, 569)  ' Taille de la zone intérieure du formulaire : 984 x 569 px
        Controls.Add(dgvAlertes)   ' Ajoute les trois zones à la fenêtre (grille, bas, filtres)
        Controls.Add(pnlBas)  ' Ajoute ce contrôle au formulaire (l'ordre détermine le recouvrement)
        Controls.Add(pnlFiltres)  ' Ajoute ce contrôle au formulaire (l'ordre détermine le recouvrement)
        MinimumSize = New System.Drawing.Size(900, 400)   ' Empêche de réduire la fenêtre au point de cacher les filtres
        Name = "frmAlertes"  ' Nom interne du formulaire
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent   ' S'ouvre au centre de la fenêtre qui l'a ouverte
        Text = "Alertes de prix"   ' Titre de la fenêtre
        pnlFiltres.ResumeLayout(False)   ' Fin du réglage : reprend le calcul de la mise en page
        pnlFiltres.PerformLayout()  ' Applique la mise en page en attente de pnlFiltres
        pnlBas.ResumeLayout(False)  ' Reprend la mise en page de pnlBas
        pnlBas.PerformLayout()  ' Applique la mise en page en attente de pnlBas
        CType(dgvAlertes, System.ComponentModel.ISupportInitialize).EndInit()   ' Prévient la grille que son initialisation est terminée
        ResumeLayout(False)  ' Reprend le calcul de la mise en page du formulaire
    End Sub  ' Fin de la méthode

    Friend WithEvents pnlFiltres As System.Windows.Forms.Panel   ' Déclarations des contrôles : WithEvents permet d'écrire « Handles btnFiltrer.Click » dans frmAlertes.vb
    Friend WithEvents chkPeriode As System.Windows.Forms.CheckBox  ' Déclare chkPeriode (une case à cocher) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents lblDu As System.Windows.Forms.Label  ' Déclare lblDu (une étiquette de texte) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents dtpDebut As System.Windows.Forms.DateTimePicker  ' Déclare dtpDebut (un sélecteur de date) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents lblAu As System.Windows.Forms.Label  ' Déclare lblAu (une étiquette de texte) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents dtpFin As System.Windows.Forms.DateTimePicker  ' Déclare dtpFin (un sélecteur de date) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents lblMagasin As System.Windows.Forms.Label  ' Déclare lblMagasin (une étiquette de texte) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents cmbMagasin As System.Windows.Forms.ComboBox  ' Déclare cmbMagasin (une liste déroulante) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents lblType As System.Windows.Forms.Label  ' Déclare lblType (une étiquette de texte) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox  ' Déclare cmbType (une liste déroulante) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents lblStatut As System.Windows.Forms.Label  ' Déclare lblStatut (une étiquette de texte) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents cmbStatut As System.Windows.Forms.ComboBox  ' Déclare cmbStatut (une liste déroulante) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents btnFiltrer As System.Windows.Forms.Button  ' Déclare btnFiltrer (un bouton) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents btnReinitialiser As System.Windows.Forms.Button  ' Déclare btnReinitialiser (un bouton) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents pnlBas As System.Windows.Forms.Panel  ' Déclare pnlBas (un panneau (conteneur)) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents lblCompteur As System.Windows.Forms.Label  ' Déclare lblCompteur (une étiquette de texte) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents btnMarquerLue As System.Windows.Forms.Button  ' Déclare btnMarquerLue (un bouton) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents btnToutLire As System.Windows.Forms.Button  ' Déclare btnToutLire (un bouton) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents dgvAlertes As System.Windows.Forms.DataGridView  ' Déclare dgvAlertes (une grille de données (tableau)) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents colDate As System.Windows.Forms.DataGridViewTextBoxColumn  ' Déclare colDate (une colonne de texte de la grille) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents colProduit As System.Windows.Forms.DataGridViewTextBoxColumn  ' Déclare colProduit (une colonne de texte de la grille) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents colMagasin As System.Windows.Forms.DataGridViewTextBoxColumn  ' Déclare colMagasin (une colonne de texte de la grille) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents colType As System.Windows.Forms.DataGridViewTextBoxColumn  ' Déclare colType (une colonne de texte de la grille) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents colVariation As System.Windows.Forms.DataGridViewTextBoxColumn  ' Déclare colVariation (une colonne de texte de la grille) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents colDetail As System.Windows.Forms.DataGridViewTextBoxColumn  ' Déclare colDetail (une colonne de texte de la grille) ; WithEvents permet de lui associer des événements avec Handles
    Friend WithEvents colStatut As System.Windows.Forms.DataGridViewTextBoxColumn  ' Déclare colStatut (une colonne de texte de la grille) ; WithEvents permet de lui associer des événements avec Handles

End Class  ' Fin de la classe
