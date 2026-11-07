' ============================================================================================
' FICHIER GÉNÉRÉ PAR LE CONCEPTEUR DE FORMULAIRES DE VISUAL STUDIO (partie « dessin » de frmAnalyses).
' Il décrit les contrôles de la fenêtre, leur position et leurs propriétés ; le comportement est dans frmAnalyses.vb.
' Mise en page conforme à la maquette du groupe, en vert forêt, avec des zones « Dock » (pas d'Anchor à droite :
' elles se déplacent mal quand l'écran est réglé à 125 % ou 150 %).
' ============================================================================================
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  ' Fichier généré par le concepteur
Partial Class frmAnalyses  ' Partial : la classe est coupée en deux fichiers (apparence ici, comportement dans frmAnalyses.vb)
    Inherits System.Windows.Forms.Form  ' La classe hérite de Form : c'est une fenêtre Windows

    <System.Diagnostics.DebuggerNonUserCode()>  ' Code technique : le débogueur n'y entre pas
    Protected Overrides Sub Dispose(disposing As Boolean)  ' Libère les ressources du formulaire à sa fermeture
        Try  ' Début du bloc protégé
            If disposing AndAlso components IsNot Nothing Then  ' Si on libère les ressources
                components.Dispose()  ' Libère les composants
            End If  ' Fin du test
        Finally  ' S'exécute toujours
            MyBase.Dispose(disposing)  ' Libération de la classe Form
        End Try  ' Fin du bloc protégé
    End Sub  ' Fin de Dispose

    Private components As System.ComponentModel.IContainer  ' Conteneur de composants (exigé par le concepteur)

    <System.Diagnostics.DebuggerStepThrough()>  ' Le débogueur ne s'arrête pas ici
    Private Sub InitializeComponent()  ' Crée et règle tous les contrôles ; appelée par le constructeur
        pnlTitre = New System.Windows.Forms.Panel()  ' Crée pnlTitre
        lblTitre = New System.Windows.Forms.Label()  ' Crée lblTitre
        pnlIndice = New System.Windows.Forms.Panel()  ' Crée pnlIndice
        lblIndiceTitre = New System.Windows.Forms.Label()  ' Crée lblIndiceTitre
        lblDepart = New System.Windows.Forms.Label()  ' Crée lblDepart
        dtpDepart = New System.Windows.Forms.DateTimePicker()  ' Crée dtpDepart
        lblArrivee = New System.Windows.Forms.Label()  ' Crée lblArrivee
        dtpArrivee = New System.Windows.Forms.DateTimePicker()  ' Crée dtpArrivee
        btnCalculer = New System.Windows.Forms.Button()  ' Crée btnCalculer
        lblIndiceValeur = New System.Windows.Forms.Label()  ' Crée lblIndiceValeur
        lblIndiceTexte = New System.Windows.Forms.Label()  ' Crée lblIndiceTexte
        lblIndiceNote = New System.Windows.Forms.Label()  ' Crée lblIndiceNote
        pnlBas = New System.Windows.Forms.Panel()  ' Crée pnlBas
        lblAvertissement = New System.Windows.Forms.Label()  ' Crée lblAvertissement
        pnlComparaison = New System.Windows.Forms.Panel()  ' Crée pnlComparaison
        tlpGrilles = New System.Windows.Forms.TableLayoutPanel()  ' Crée tlpGrilles
        pnlCompFiltres = New System.Windows.Forms.Panel()  ' Crée pnlCompFiltres
        lblProduit = New System.Windows.Forms.Label()  ' Crée lblProduit
        cmbProduit = New System.Windows.Forms.ComboBox()  ' Crée cmbProduit
        lblCategorie = New System.Windows.Forms.Label()  ' Crée lblCategorie
        cmbCategorie = New System.Windows.Forms.ComboBox()  ' Crée cmbCategorie
        btnComparer = New System.Windows.Forms.Button()  ' Crée btnComparer
        btnEquivalents = New System.Windows.Forms.Button()  ' Crée btnEquivalents
        lblTitreMagasins = New System.Windows.Forms.Label()  ' Crée lblTitreMagasins
        lblTitreEquivalents = New System.Windows.Forms.Label()  ' Crée lblTitreEquivalents
        dgvMagasins = New System.Windows.Forms.DataGridView()  ' Crée dgvMagasins
        colMProduit = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colMProduit
        colMMagasin = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colMMagasin
        colMPrix = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colMPrix
        colMDate = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colMDate
        colMEcart = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colMEcart
        dgvEquivalents = New System.Windows.Forms.DataGridView()  ' Crée dgvEquivalents
        colEProduit = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colEProduit
        colEMagasin = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colEMagasin
        colEPrix = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colEPrix
        colEEconomie = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colEEconomie
        lblMessage = New System.Windows.Forms.Label()  ' Crée lblMessage
        pnlTitre.SuspendLayout()  ' Suspend la mise en page de pnlTitre pendant le réglage
        pnlIndice.SuspendLayout()  ' Suspend la mise en page de pnlIndice pendant le réglage
        pnlBas.SuspendLayout()  ' Suspend la mise en page de pnlBas pendant le réglage
        pnlComparaison.SuspendLayout()  ' Suspend la mise en page de pnlComparaison pendant le réglage
        tlpGrilles.SuspendLayout()  ' Suspend la mise en page de tlpGrilles pendant le réglage
        pnlCompFiltres.SuspendLayout()  ' Suspend la mise en page de pnlCompFiltres pendant le réglage
        CType(dgvMagasins, System.ComponentModel.ISupportInitialize).BeginInit()  ' Début de l'initialisation de la grille
        CType(dgvEquivalents, System.ComponentModel.ISupportInitialize).BeginInit()  ' Début de l'initialisation de la grille
        SuspendLayout()  ' Suspend la mise en page du formulaire
        '
        'pnlTitre
        '
        pnlTitre.Controls.Add(lblTitre)  ' Place lblTitre dans pnlTitre
        pnlTitre.BackColor = System.Drawing.Color.FromArgb(244, 247, 245)
        pnlTitre.Dock = System.Windows.Forms.DockStyle.Top
        pnlTitre.Size = New System.Drawing.Size(100, 44)
        pnlTitre.Name = "pnlTitre"  ' Nom interne
        pnlTitre.TabIndex = 0  ' Ordre de tabulation
        '
        'lblTitre
        '
        lblTitre.AutoSize = True
        lblTitre.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblTitre.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblTitre.Location = New System.Drawing.Point(14, 10)
        lblTitre.Text = "Analyses et comparaisons"
        lblTitre.Name = "lblTitre"  ' Nom interne
        lblTitre.TabIndex = 0  ' Ordre de tabulation
        '
        'pnlIndice
        '
        pnlIndice.Controls.Add(lblIndiceTitre)  ' Place lblIndiceTitre dans pnlIndice
        pnlIndice.Controls.Add(lblDepart)  ' Place lblDepart dans pnlIndice
        pnlIndice.Controls.Add(dtpDepart)  ' Place dtpDepart dans pnlIndice
        pnlIndice.Controls.Add(lblArrivee)  ' Place lblArrivee dans pnlIndice
        pnlIndice.Controls.Add(dtpArrivee)  ' Place dtpArrivee dans pnlIndice
        pnlIndice.Controls.Add(btnCalculer)  ' Place btnCalculer dans pnlIndice
        pnlIndice.Controls.Add(lblIndiceValeur)  ' Place lblIndiceValeur dans pnlIndice
        pnlIndice.Controls.Add(lblIndiceTexte)  ' Place lblIndiceTexte dans pnlIndice
        pnlIndice.Controls.Add(lblIndiceNote)  ' Place lblIndiceNote dans pnlIndice
        pnlIndice.BackColor = System.Drawing.Color.White
        pnlIndice.Dock = System.Windows.Forms.DockStyle.Top
        pnlIndice.Size = New System.Drawing.Size(100, 170)
        pnlIndice.Name = "pnlIndice"  ' Nom interne
        pnlIndice.TabIndex = 1  ' Ordre de tabulation
        '
        'lblIndiceTitre
        '
        lblIndiceTitre.AutoSize = True
        lblIndiceTitre.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblIndiceTitre.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblIndiceTitre.Location = New System.Drawing.Point(18, 10)
        lblIndiceTitre.Text = "Indice de prix personnel (base 100)"
        lblIndiceTitre.Name = "lblIndiceTitre"  ' Nom interne
        lblIndiceTitre.TabIndex = 0  ' Ordre de tabulation
        '
        'lblDepart
        '
        lblDepart.AutoSize = True
        lblDepart.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblDepart.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblDepart.Location = New System.Drawing.Point(18, 44)
        lblDepart.Text = "Date de départ"
        lblDepart.Name = "lblDepart"  ' Nom interne
        lblDepart.TabIndex = 1  ' Ordre de tabulation
        '
        'dtpDepart
        '
        dtpDepart.Format = System.Windows.Forms.DateTimePickerFormat.Short
        dtpDepart.Location = New System.Drawing.Point(18, 64)
        dtpDepart.Size = New System.Drawing.Size(120, 23)
        dtpDepart.Name = "dtpDepart"  ' Nom interne
        dtpDepart.TabIndex = 2  ' Ordre de tabulation
        '
        'lblArrivee
        '
        lblArrivee.AutoSize = True
        lblArrivee.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblArrivee.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblArrivee.Location = New System.Drawing.Point(158, 44)
        lblArrivee.Text = "Date d'arrivée"
        lblArrivee.Name = "lblArrivee"  ' Nom interne
        lblArrivee.TabIndex = 3  ' Ordre de tabulation
        '
        'dtpArrivee
        '
        dtpArrivee.Format = System.Windows.Forms.DateTimePickerFormat.Short
        dtpArrivee.Location = New System.Drawing.Point(158, 64)
        dtpArrivee.Size = New System.Drawing.Size(120, 23)
        dtpArrivee.Name = "dtpArrivee"  ' Nom interne
        dtpArrivee.TabIndex = 4  ' Ordre de tabulation
        '
        'btnCalculer
        '
        btnCalculer.BackColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnCalculer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        btnCalculer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnCalculer.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        btnCalculer.ForeColor = System.Drawing.Color.White
        btnCalculer.Location = New System.Drawing.Point(298, 62)
        btnCalculer.Size = New System.Drawing.Size(110, 30)
        btnCalculer.Text = "Calculer"
        btnCalculer.UseVisualStyleBackColor = False
        btnCalculer.Name = "btnCalculer"  ' Nom interne
        btnCalculer.TabIndex = 5  ' Ordre de tabulation
        '
        'lblIndiceValeur
        '
        lblIndiceValeur.AutoSize = True
        lblIndiceValeur.Font = New System.Drawing.Font("Segoe UI", 26.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblIndiceValeur.ForeColor = System.Drawing.Color.FromArgb(30, 77, 58)
        lblIndiceValeur.Location = New System.Drawing.Point(440, 34)
        lblIndiceValeur.Text = "—"
        lblIndiceValeur.Name = "lblIndiceValeur"  ' Nom interne
        lblIndiceValeur.TabIndex = 6  ' Ordre de tabulation
        '
        'lblIndiceTexte
        '
        lblIndiceTexte.AutoSize = False
        lblIndiceTexte.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        lblIndiceTexte.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblIndiceTexte.Location = New System.Drawing.Point(440, 92)
        lblIndiceTexte.Size = New System.Drawing.Size(600, 40)
        lblIndiceTexte.Text = ""
        lblIndiceTexte.Name = "lblIndiceTexte"  ' Nom interne
        lblIndiceTexte.TabIndex = 7  ' Ordre de tabulation
        '
        'lblIndiceNote
        '
        lblIndiceNote.AutoSize = False
        lblIndiceNote.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        lblIndiceNote.ForeColor = System.Drawing.Color.FromArgb(90, 106, 98)
        lblIndiceNote.Location = New System.Drawing.Point(18, 138)
        lblIndiceNote.Size = New System.Drawing.Size(1000, 20)
        lblIndiceNote.Text = ""
        lblIndiceNote.Name = "lblIndiceNote"  ' Nom interne
        lblIndiceNote.TabIndex = 8  ' Ordre de tabulation
        '
        'pnlBas
        '
        pnlBas.Controls.Add(lblAvertissement)  ' Place lblAvertissement dans pnlBas
        pnlBas.BackColor = System.Drawing.Color.FromArgb(244, 247, 245)
        pnlBas.Dock = System.Windows.Forms.DockStyle.Bottom
        pnlBas.Size = New System.Drawing.Size(100, 34)
        pnlBas.Name = "pnlBas"  ' Nom interne
        pnlBas.TabIndex = 2  ' Ordre de tabulation
        '
        'lblAvertissement
        '
        lblAvertissement.AutoSize = False
        lblAvertissement.Dock = System.Windows.Forms.DockStyle.Fill
        lblAvertissement.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        lblAvertissement.ForeColor = System.Drawing.Color.FromArgb(90, 106, 98)
        lblAvertissement.Padding = New System.Windows.Forms.Padding(14, 0, 0, 0)
        lblAvertissement.Text = ""
        lblAvertissement.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        lblAvertissement.Name = "lblAvertissement"  ' Nom interne
        lblAvertissement.TabIndex = 0  ' Ordre de tabulation
        '
        'pnlComparaison
        '
        pnlComparaison.Controls.Add(tlpGrilles)  ' Place tlpGrilles dans pnlComparaison
        pnlComparaison.Controls.Add(pnlCompFiltres)  ' Place pnlCompFiltres dans pnlComparaison
        pnlComparaison.BackColor = System.Drawing.Color.FromArgb(244, 247, 245)
        pnlComparaison.Dock = System.Windows.Forms.DockStyle.Fill
        pnlComparaison.Name = "pnlComparaison"  ' Nom interne
        pnlComparaison.TabIndex = 3  ' Ordre de tabulation
        '
        'tlpGrilles
        '
        tlpGrilles.Controls.Add(lblTitreMagasins, 0, 0)  ' Place lblTitreMagasins dans la cellule (0, 0)
        tlpGrilles.Controls.Add(lblTitreEquivalents, 1, 0)  ' Place lblTitreEquivalents dans la cellule (1, 0)
        tlpGrilles.Controls.Add(dgvMagasins, 0, 1)  ' Place dgvMagasins dans la cellule (0, 1)
        tlpGrilles.Controls.Add(dgvEquivalents, 1, 1)  ' Place dgvEquivalents dans la cellule (1, 1)
        tlpGrilles.ColumnCount = 2
        tlpGrilles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        tlpGrilles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        tlpGrilles.Dock = System.Windows.Forms.DockStyle.Fill
        tlpGrilles.RowCount = 2
        tlpGrilles.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        tlpGrilles.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        tlpGrilles.Name = "tlpGrilles"  ' Nom interne
        tlpGrilles.TabIndex = 0  ' Ordre de tabulation
        '
        'pnlCompFiltres
        '
        pnlCompFiltres.Controls.Add(lblProduit)  ' Place lblProduit dans pnlCompFiltres
        pnlCompFiltres.Controls.Add(cmbProduit)  ' Place cmbProduit dans pnlCompFiltres
        pnlCompFiltres.Controls.Add(lblCategorie)  ' Place lblCategorie dans pnlCompFiltres
        pnlCompFiltres.Controls.Add(cmbCategorie)  ' Place cmbCategorie dans pnlCompFiltres
        pnlCompFiltres.Controls.Add(btnComparer)  ' Place btnComparer dans pnlCompFiltres
        pnlCompFiltres.Controls.Add(btnEquivalents)  ' Place btnEquivalents dans pnlCompFiltres
        pnlCompFiltres.Controls.Add(lblMessage)  ' Place lblMessage dans pnlCompFiltres
        pnlCompFiltres.BackColor = System.Drawing.Color.White
        pnlCompFiltres.Dock = System.Windows.Forms.DockStyle.Top
        pnlCompFiltres.Size = New System.Drawing.Size(100, 74)
        pnlCompFiltres.Name = "pnlCompFiltres"  ' Nom interne
        pnlCompFiltres.TabIndex = 1  ' Ordre de tabulation
        '
        'lblProduit
        '
        lblProduit.AutoSize = True
        lblProduit.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblProduit.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblProduit.Location = New System.Drawing.Point(18, 10)
        lblProduit.Text = "Produit"
        lblProduit.Name = "lblProduit"  ' Nom interne
        lblProduit.TabIndex = 0  ' Ordre de tabulation
        '
        'cmbProduit
        '
        cmbProduit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbProduit.FormattingEnabled = True
        cmbProduit.Location = New System.Drawing.Point(18, 30)
        cmbProduit.Size = New System.Drawing.Size(250, 23)
        cmbProduit.Name = "cmbProduit"  ' Nom interne
        cmbProduit.TabIndex = 1  ' Ordre de tabulation
        '
        'lblCategorie
        '
        lblCategorie.AutoSize = True
        lblCategorie.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblCategorie.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblCategorie.Location = New System.Drawing.Point(288, 10)
        lblCategorie.Text = "ou catégorie"
        lblCategorie.Name = "lblCategorie"  ' Nom interne
        lblCategorie.TabIndex = 2  ' Ordre de tabulation
        '
        'cmbCategorie
        '
        cmbCategorie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbCategorie.FormattingEnabled = True
        cmbCategorie.Location = New System.Drawing.Point(288, 30)
        cmbCategorie.Size = New System.Drawing.Size(200, 23)
        cmbCategorie.Name = "cmbCategorie"  ' Nom interne
        cmbCategorie.TabIndex = 3  ' Ordre de tabulation
        '
        'btnComparer
        '
        btnComparer.BackColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnComparer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        btnComparer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnComparer.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        btnComparer.ForeColor = System.Drawing.Color.White
        btnComparer.Location = New System.Drawing.Point(508, 28)
        btnComparer.Size = New System.Drawing.Size(150, 30)
        btnComparer.Text = "Comparer magasins"
        btnComparer.UseVisualStyleBackColor = False
        btnComparer.Name = "btnComparer"  ' Nom interne
        btnComparer.TabIndex = 4  ' Ordre de tabulation
        '
        'btnEquivalents
        '
        btnEquivalents.BackColor = System.Drawing.Color.White
        btnEquivalents.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        btnEquivalents.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnEquivalents.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        btnEquivalents.ForeColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnEquivalents.Location = New System.Drawing.Point(668, 28)
        btnEquivalents.Size = New System.Drawing.Size(200, 30)
        btnEquivalents.Text = "Voir équivalents moins chers"
        btnEquivalents.UseVisualStyleBackColor = False
        btnEquivalents.Name = "btnEquivalents"  ' Nom interne
        btnEquivalents.TabIndex = 5  ' Ordre de tabulation
        '
        'lblTitreMagasins
        '
        lblTitreMagasins.AutoSize = True
        lblTitreMagasins.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblTitreMagasins.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblTitreMagasins.Text = "Classement des magasins"
        lblTitreMagasins.Margin = New System.Windows.Forms.Padding(14, 6, 3, 0)
        lblTitreMagasins.Name = "lblTitreMagasins"  ' Nom interne
        lblTitreMagasins.TabIndex = 0  ' Ordre de tabulation
        '
        'lblTitreEquivalents
        '
        lblTitreEquivalents.AutoSize = True
        lblTitreEquivalents.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblTitreEquivalents.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblTitreEquivalents.Text = "Produits équivalents moins chers"
        lblTitreEquivalents.Margin = New System.Windows.Forms.Padding(14, 6, 3, 0)
        lblTitreEquivalents.Name = "lblTitreEquivalents"  ' Nom interne
        lblTitreEquivalents.TabIndex = 1  ' Ordre de tabulation
        '
        'dgvMagasins
        '
        dgvMagasins.AllowUserToAddRows = False  ' Lecture seule : pas d'ajout par l'utilisateur
        dgvMagasins.AllowUserToDeleteRows = False  ' Pas de suppression
        dgvMagasins.AllowUserToResizeRows = False  ' Hauteur des lignes fixe
        dgvMagasins.AutoGenerateColumns = False  ' Colonnes définies à la main ci-dessous
        dgvMagasins.BackgroundColor = System.Drawing.Color.White  ' Fond blanc
        dgvMagasins.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle  ' Fine bordure
        dgvMagasins.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single  ' Quadrillage complet
        dgvMagasins.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single  ' En-têtes avec bordure simple
        dgvMagasins.ColumnHeadersDefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.BackColor = System.Drawing.Color.FromArgb(227, 237, 230), .ForeColor = System.Drawing.Color.FromArgb(23, 38, 31), .Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point), .SelectionBackColor = System.Drawing.Color.FromArgb(227, 237, 230), .SelectionForeColor = System.Drawing.Color.FromArgb(23, 38, 31)}  ' En-tête vert très clair
        dgvMagasins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize  ' Hauteur d'en-tête automatique
        dgvMagasins.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {colMProduit, colMMagasin, colMPrix, colMDate, colMEcart})  ' Ajoute les colonnes dans l'ordre de la maquette
        dgvMagasins.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.SelectionBackColor = System.Drawing.Color.FromArgb(221, 235, 226), .SelectionForeColor = System.Drawing.Color.FromArgb(23, 38, 31)}  ' Ligne sélectionnée : vert clair
        dgvMagasins.Dock = System.Windows.Forms.DockStyle.Fill  ' Occupe tout l'espace restant
        dgvMagasins.EnableHeadersVisualStyles = False  ' Obligatoire pour appliquer la couleur d'en-tête
        dgvMagasins.GridColor = System.Drawing.Color.FromArgb(195, 208, 200)  ' Couleur du quadrillage
        dgvMagasins.Location = New System.Drawing.Point(0, 0)  ' Position (recalculée par Dock)
        dgvMagasins.MultiSelect = False  ' Une seule ligne sélectionnée
        dgvMagasins.ReadOnly = True  ' Lecture seule
        dgvMagasins.RowHeadersVisible = False  ' Cache la colonne grise de gauche
        dgvMagasins.RowTemplate.Height = 28  ' Hauteur des lignes : 28 px
        dgvMagasins.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect  ' Un clic sélectionne la ligne
        dgvMagasins.Size = New System.Drawing.Size(300, 200)  ' Taille (recalculée par Dock)
        dgvMagasins.TabIndex = 2  ' Ordre de tabulation
        '
        'colMProduit
        '
        colMProduit.DataPropertyName = "Produit"  ' Propriété de la ligne affichée
        colMProduit.FillWeight = 30.0!  ' Largeur relative
        colMProduit.HeaderText = "Produit"  ' Titre de la colonne
        colMProduit.Name = "colMProduit"  ' Nom interne
        colMProduit.ReadOnly = True  ' Lecture seule
        colMProduit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colMMagasin
        '
        colMMagasin.DataPropertyName = "Magasin"  ' Propriété de la ligne affichée
        colMMagasin.FillWeight = 22.0!  ' Largeur relative
        colMMagasin.HeaderText = "Magasin"  ' Titre de la colonne
        colMMagasin.Name = "colMMagasin"  ' Nom interne
        colMMagasin.ReadOnly = True  ' Lecture seule
        colMMagasin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colMPrix
        '
        colMPrix.DataPropertyName = "PrixTexte"  ' Propriété de la ligne affichée
        colMPrix.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight}  ' Nombres alignés à droite
        colMPrix.FillWeight = 20.0!  ' Largeur relative
        colMPrix.HeaderText = "Prix unitaire"  ' Titre de la colonne
        colMPrix.Name = "colMPrix"  ' Nom interne
        colMPrix.ReadOnly = True  ' Lecture seule
        colMPrix.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colMDate
        '
        colMDate.DataPropertyName = "DateTexte"  ' Propriété de la ligne affichée
        colMDate.FillWeight = 16.0!  ' Largeur relative
        colMDate.HeaderText = "Dernière obs."  ' Titre de la colonne
        colMDate.Name = "colMDate"  ' Nom interne
        colMDate.ReadOnly = True  ' Lecture seule
        colMDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colMEcart
        '
        colMEcart.DataPropertyName = "EcartTexte"  ' Propriété de la ligne affichée
        colMEcart.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight}  ' Nombres alignés à droite
        colMEcart.FillWeight = 14.0!  ' Largeur relative
        colMEcart.HeaderText = "Écart"  ' Titre de la colonne
        colMEcart.Name = "colMEcart"  ' Nom interne
        colMEcart.ReadOnly = True  ' Lecture seule
        colMEcart.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'dgvEquivalents
        '
        dgvEquivalents.AllowUserToAddRows = False  ' Lecture seule : pas d'ajout par l'utilisateur
        dgvEquivalents.AllowUserToDeleteRows = False  ' Pas de suppression
        dgvEquivalents.AllowUserToResizeRows = False  ' Hauteur des lignes fixe
        dgvEquivalents.AutoGenerateColumns = False  ' Colonnes définies à la main ci-dessous
        dgvEquivalents.BackgroundColor = System.Drawing.Color.White  ' Fond blanc
        dgvEquivalents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle  ' Fine bordure
        dgvEquivalents.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single  ' Quadrillage complet
        dgvEquivalents.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single  ' En-têtes avec bordure simple
        dgvEquivalents.ColumnHeadersDefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.BackColor = System.Drawing.Color.FromArgb(227, 237, 230), .ForeColor = System.Drawing.Color.FromArgb(23, 38, 31), .Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point), .SelectionBackColor = System.Drawing.Color.FromArgb(227, 237, 230), .SelectionForeColor = System.Drawing.Color.FromArgb(23, 38, 31)}  ' En-tête vert très clair
        dgvEquivalents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize  ' Hauteur d'en-tête automatique
        dgvEquivalents.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {colEProduit, colEMagasin, colEPrix, colEEconomie})  ' Ajoute les colonnes dans l'ordre de la maquette
        dgvEquivalents.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.SelectionBackColor = System.Drawing.Color.FromArgb(221, 235, 226), .SelectionForeColor = System.Drawing.Color.FromArgb(23, 38, 31)}  ' Ligne sélectionnée : vert clair
        dgvEquivalents.Dock = System.Windows.Forms.DockStyle.Fill  ' Occupe tout l'espace restant
        dgvEquivalents.EnableHeadersVisualStyles = False  ' Obligatoire pour appliquer la couleur d'en-tête
        dgvEquivalents.GridColor = System.Drawing.Color.FromArgb(195, 208, 200)  ' Couleur du quadrillage
        dgvEquivalents.Location = New System.Drawing.Point(0, 0)  ' Position (recalculée par Dock)
        dgvEquivalents.MultiSelect = False  ' Une seule ligne sélectionnée
        dgvEquivalents.ReadOnly = True  ' Lecture seule
        dgvEquivalents.RowHeadersVisible = False  ' Cache la colonne grise de gauche
        dgvEquivalents.RowTemplate.Height = 28  ' Hauteur des lignes : 28 px
        dgvEquivalents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect  ' Un clic sélectionne la ligne
        dgvEquivalents.Size = New System.Drawing.Size(300, 200)  ' Taille (recalculée par Dock)
        dgvEquivalents.TabIndex = 3  ' Ordre de tabulation
        '
        'colEProduit
        '
        colEProduit.DataPropertyName = "Produit"  ' Propriété de la ligne affichée
        colEProduit.FillWeight = 38.0!  ' Largeur relative
        colEProduit.HeaderText = "Produit"  ' Titre de la colonne
        colEProduit.Name = "colEProduit"  ' Nom interne
        colEProduit.ReadOnly = True  ' Lecture seule
        colEProduit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colEMagasin
        '
        colEMagasin.DataPropertyName = "Magasin"  ' Propriété de la ligne affichée
        colEMagasin.FillWeight = 24.0!  ' Largeur relative
        colEMagasin.HeaderText = "Magasin"  ' Titre de la colonne
        colEMagasin.Name = "colEMagasin"  ' Nom interne
        colEMagasin.ReadOnly = True  ' Lecture seule
        colEMagasin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colEPrix
        '
        colEPrix.DataPropertyName = "PrixTexte"  ' Propriété de la ligne affichée
        colEPrix.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight}  ' Nombres alignés à droite
        colEPrix.FillWeight = 22.0!  ' Largeur relative
        colEPrix.HeaderText = "Prix unitaire"  ' Titre de la colonne
        colEPrix.Name = "colEPrix"  ' Nom interne
        colEPrix.ReadOnly = True  ' Lecture seule
        colEPrix.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colEEconomie
        '
        colEEconomie.DataPropertyName = "EconomieTexte"  ' Propriété de la ligne affichée
        colEEconomie.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight}  ' Nombres alignés à droite
        colEEconomie.FillWeight = 16.0!  ' Largeur relative
        colEEconomie.HeaderText = "Économie"  ' Titre de la colonne
        colEEconomie.Name = "colEEconomie"  ' Nom interne
        colEEconomie.ReadOnly = True  ' Lecture seule
        colEEconomie.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'lblMessage
        '
        lblMessage.AutoSize = True
        lblMessage.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        lblMessage.ForeColor = System.Drawing.Color.FromArgb(90, 106, 98)
        lblMessage.Location = New System.Drawing.Point(18, 56)
        lblMessage.Text = ""
        lblMessage.Name = "lblMessage"  ' Nom interne
        lblMessage.TabIndex = 6  ' Ordre de tabulation
        '
        'frmAnalyses
        '
        AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)  ' Dimensions de référence (DPI)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font  ' S'adapte à la police de l'écran
        BackColor = System.Drawing.Color.FromArgb(244, 247, 245)  ' Fond gris très clair
        ClientSize = New System.Drawing.Size(1100, 640)  ' Taille intérieure
        Controls.Add(pnlComparaison)  ' Ajoute pnlComparaison (le contrôle Fill d'abord, puis les bords)
        Controls.Add(pnlBas)  ' Ajoute pnlBas (le contrôle Fill d'abord, puis les bords)
        Controls.Add(pnlIndice)  ' Ajoute pnlIndice (le contrôle Fill d'abord, puis les bords)
        Controls.Add(pnlTitre)  ' Ajoute pnlTitre (le contrôle Fill d'abord, puis les bords)
        MinimumSize = New System.Drawing.Size(1116, 520)  ' Taille minimale
        Name = "frmAnalyses"  ' Nom interne
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent  ' Centrée sur la fenêtre parente
        Text = "Analyses et comparaisons"  ' Titre de la fenêtre
        pnlTitre.ResumeLayout(False)  ' Reprend la mise en page de pnlTitre
        pnlTitre.PerformLayout()  ' Applique la mise en page
        pnlIndice.ResumeLayout(False)  ' Reprend la mise en page de pnlIndice
        pnlIndice.PerformLayout()  ' Applique la mise en page
        pnlBas.ResumeLayout(False)  ' Reprend la mise en page de pnlBas
        pnlBas.PerformLayout()  ' Applique la mise en page
        pnlComparaison.ResumeLayout(False)  ' Reprend la mise en page de pnlComparaison
        pnlComparaison.PerformLayout()  ' Applique la mise en page
        tlpGrilles.ResumeLayout(False)  ' Reprend la mise en page de tlpGrilles
        tlpGrilles.PerformLayout()  ' Applique la mise en page
        pnlCompFiltres.ResumeLayout(False)  ' Reprend la mise en page de pnlCompFiltres
        pnlCompFiltres.PerformLayout()  ' Applique la mise en page
        CType(dgvMagasins, System.ComponentModel.ISupportInitialize).EndInit()  ' Fin de l'initialisation de la grille
        CType(dgvEquivalents, System.ComponentModel.ISupportInitialize).EndInit()  ' Fin de l'initialisation de la grille
        ResumeLayout(False)  ' Reprend la mise en page du formulaire
    End Sub  ' Fin de InitializeComponent

    Friend WithEvents pnlTitre As System.Windows.Forms.Panel  ' Bandeau du titre
    Friend WithEvents lblTitre As System.Windows.Forms.Label  ' Texte « Analyses et comparaisons »
    Friend WithEvents pnlIndice As System.Windows.Forms.Panel  ' Zone de l'indice de prix personnel
    Friend WithEvents lblIndiceTitre As System.Windows.Forms.Label  ' Texte « Indice de prix personnel (base 100) »
    Friend WithEvents lblDepart As System.Windows.Forms.Label  ' Texte « Date de départ »
    Friend WithEvents dtpDepart As System.Windows.Forms.DateTimePicker  ' Date de départ
    Friend WithEvents lblArrivee As System.Windows.Forms.Label  ' Texte « Date d'arrivée »
    Friend WithEvents dtpArrivee As System.Windows.Forms.DateTimePicker  ' Date d'arrivée
    Friend WithEvents btnCalculer As System.Windows.Forms.Button  ' Calcule l'indice
    Friend WithEvents lblIndiceValeur As System.Windows.Forms.Label  ' Valeur de l'indice
    Friend WithEvents lblIndiceTexte As System.Windows.Forms.Label  ' Détail : coûts du panier et nombre de produits
    Friend WithEvents lblIndiceNote As System.Windows.Forms.Label  ' Rappel : l'indice n'est pas un conseil d'achat
    Friend WithEvents pnlBas As System.Windows.Forms.Panel  ' Barre du bas : avertissement
    Friend WithEvents lblAvertissement As System.Windows.Forms.Label  ' Phrase : information et non conseil
    Friend WithEvents pnlComparaison As System.Windows.Forms.Panel  ' Zone des comparaisons
    Friend WithEvents tlpGrilles As System.Windows.Forms.TableLayoutPanel  ' Deux grilles côte à côte
    Friend WithEvents pnlCompFiltres As System.Windows.Forms.Panel  ' Filtres de la comparaison
    Friend WithEvents lblProduit As System.Windows.Forms.Label  ' Texte « Produit »
    Friend WithEvents cmbProduit As System.Windows.Forms.ComboBox  ' Produit à comparer
    Friend WithEvents lblCategorie As System.Windows.Forms.Label  ' Texte « ou catégorie »
    Friend WithEvents cmbCategorie As System.Windows.Forms.ComboBox  ' Catégorie à comparer
    Friend WithEvents btnComparer As System.Windows.Forms.Button  ' Classe les magasins par prix unitaire
    Friend WithEvents btnEquivalents As System.Windows.Forms.Button  ' Cherche les produits équivalents moins chers
    Friend WithEvents lblTitreMagasins As System.Windows.Forms.Label  ' Texte « Classement des magasins »
    Friend WithEvents lblTitreEquivalents As System.Windows.Forms.Label  ' Texte « Produits équivalents moins chers »
    Friend WithEvents dgvMagasins As System.Windows.Forms.DataGridView  ' Classement des magasins
    Friend WithEvents colMProduit As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Produit »
    Friend WithEvents colMMagasin As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Magasin »
    Friend WithEvents colMPrix As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Prix unitaire »
    Friend WithEvents colMDate As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Dernière obs. »
    Friend WithEvents colMEcart As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Écart »
    Friend WithEvents dgvEquivalents As System.Windows.Forms.DataGridView  ' Produits équivalents
    Friend WithEvents colEProduit As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Produit »
    Friend WithEvents colEMagasin As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Magasin »
    Friend WithEvents colEPrix As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Prix unitaire »
    Friend WithEvents colEEconomie As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Économie »
    Friend WithEvents lblMessage As System.Windows.Forms.Label  ' Message d'information (aucun résultat...)
End Class
