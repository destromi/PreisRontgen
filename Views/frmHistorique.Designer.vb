' ============================================================================================
' FICHIER GÉNÉRÉ PAR LE CONCEPTEUR DE FORMULAIRES DE VISUAL STUDIO (partie « dessin » de frmHistorique).
' Il décrit les contrôles de la fenêtre, leur position et leurs propriétés ; le comportement est dans frmHistorique.vb.
' Mise en page conforme à la maquette du groupe, en vert forêt, avec des zones « Dock » (pas d'Anchor à droite :
' elles se déplacent mal quand l'écran est réglé à 125 % ou 150 %).
' ============================================================================================
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  ' Fichier généré par le concepteur
Partial Class frmHistorique  ' Partial : la classe est coupée en deux fichiers (apparence ici, comportement dans frmHistorique.vb)
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
        pnlFiltres = New System.Windows.Forms.Panel()  ' Crée pnlFiltres
        lblProduit = New System.Windows.Forms.Label()  ' Crée lblProduit
        cmbProduit = New System.Windows.Forms.ComboBox()  ' Crée cmbProduit
        lblMagasin = New System.Windows.Forms.Label()  ' Crée lblMagasin
        cmbMagasin = New System.Windows.Forms.ComboBox()  ' Crée cmbMagasin
        lblAfficher = New System.Windows.Forms.Label()  ' Crée lblAfficher
        cmbAfficher = New System.Windows.Forms.ComboBox()  ' Crée cmbAfficher
        btnActualiser = New System.Windows.Forms.Button()  ' Crée btnActualiser
        pnlBas = New System.Windows.Forms.Panel()  ' Crée pnlBas
        lblResume = New System.Windows.Forms.Label()  ' Crée lblResume
        pnlGraphique = New PanneauGraphique()  ' Crée pnlGraphique
        dgvHistorique = New System.Windows.Forms.DataGridView()  ' Crée dgvHistorique
        colDate = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colDate
        colMagasin = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colMagasin
        colConditionnement = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colConditionnement
        colPrix = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colPrix
        pnlTitre.SuspendLayout()  ' Suspend la mise en page de pnlTitre pendant le réglage
        pnlFiltres.SuspendLayout()  ' Suspend la mise en page de pnlFiltres pendant le réglage
        pnlBas.SuspendLayout()  ' Suspend la mise en page de pnlBas pendant le réglage
        CType(dgvHistorique, System.ComponentModel.ISupportInitialize).BeginInit()  ' Début de l'initialisation de la grille
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
        lblTitre.Text = "Historique des prix"
        lblTitre.Name = "lblTitre"  ' Nom interne
        lblTitre.TabIndex = 0  ' Ordre de tabulation
        '
        'pnlFiltres
        '
        pnlFiltres.Controls.Add(lblProduit)  ' Place lblProduit dans pnlFiltres
        pnlFiltres.Controls.Add(cmbProduit)  ' Place cmbProduit dans pnlFiltres
        pnlFiltres.Controls.Add(lblMagasin)  ' Place lblMagasin dans pnlFiltres
        pnlFiltres.Controls.Add(cmbMagasin)  ' Place cmbMagasin dans pnlFiltres
        pnlFiltres.Controls.Add(lblAfficher)  ' Place lblAfficher dans pnlFiltres
        pnlFiltres.Controls.Add(cmbAfficher)  ' Place cmbAfficher dans pnlFiltres
        pnlFiltres.Controls.Add(btnActualiser)  ' Place btnActualiser dans pnlFiltres
        pnlFiltres.BackColor = System.Drawing.Color.White
        pnlFiltres.Dock = System.Windows.Forms.DockStyle.Top
        pnlFiltres.Size = New System.Drawing.Size(100, 74)
        pnlFiltres.Name = "pnlFiltres"  ' Nom interne
        pnlFiltres.TabIndex = 1  ' Ordre de tabulation
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
        'lblMagasin
        '
        lblMagasin.AutoSize = True
        lblMagasin.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblMagasin.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblMagasin.Location = New System.Drawing.Point(288, 10)
        lblMagasin.Text = "Magasin"
        lblMagasin.Name = "lblMagasin"  ' Nom interne
        lblMagasin.TabIndex = 2  ' Ordre de tabulation
        '
        'cmbMagasin
        '
        cmbMagasin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbMagasin.FormattingEnabled = True
        cmbMagasin.Location = New System.Drawing.Point(288, 30)
        cmbMagasin.Size = New System.Drawing.Size(170, 23)
        cmbMagasin.Name = "cmbMagasin"  ' Nom interne
        cmbMagasin.TabIndex = 3  ' Ordre de tabulation
        '
        'lblAfficher
        '
        lblAfficher.AutoSize = True
        lblAfficher.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblAfficher.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblAfficher.Location = New System.Drawing.Point(478, 10)
        lblAfficher.Text = "Afficher"
        lblAfficher.Name = "lblAfficher"  ' Nom interne
        lblAfficher.TabIndex = 4  ' Ordre de tabulation
        '
        'cmbAfficher
        '
        cmbAfficher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbAfficher.FormattingEnabled = True
        cmbAfficher.Location = New System.Drawing.Point(478, 30)
        cmbAfficher.Size = New System.Drawing.Size(150, 23)
        cmbAfficher.Name = "cmbAfficher"  ' Nom interne
        cmbAfficher.TabIndex = 5  ' Ordre de tabulation
        '
        'btnActualiser
        '
        btnActualiser.BackColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnActualiser.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        btnActualiser.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnActualiser.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        btnActualiser.ForeColor = System.Drawing.Color.White
        btnActualiser.Location = New System.Drawing.Point(648, 28)
        btnActualiser.Size = New System.Drawing.Size(110, 30)
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = False
        btnActualiser.Name = "btnActualiser"  ' Nom interne
        btnActualiser.TabIndex = 6  ' Ordre de tabulation
        '
        'pnlBas
        '
        pnlBas.Controls.Add(lblResume)  ' Place lblResume dans pnlBas
        pnlBas.BackColor = System.Drawing.Color.FromArgb(244, 247, 245)
        pnlBas.Dock = System.Windows.Forms.DockStyle.Bottom
        pnlBas.Size = New System.Drawing.Size(100, 40)
        pnlBas.Name = "pnlBas"  ' Nom interne
        pnlBas.TabIndex = 2  ' Ordre de tabulation
        '
        'lblResume
        '
        lblResume.AutoSize = False
        lblResume.Dock = System.Windows.Forms.DockStyle.Fill
        lblResume.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        lblResume.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblResume.Padding = New System.Windows.Forms.Padding(14, 0, 0, 0)
        lblResume.Text = ""
        lblResume.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        lblResume.Name = "lblResume"  ' Nom interne
        lblResume.TabIndex = 0  ' Ordre de tabulation
        '
        'pnlGraphique
        '
        pnlGraphique.BackColor = System.Drawing.Color.White
        pnlGraphique.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        pnlGraphique.Dock = System.Windows.Forms.DockStyle.Right
        pnlGraphique.Size = New System.Drawing.Size(520, 100)
        pnlGraphique.Name = "pnlGraphique"  ' Nom interne
        pnlGraphique.TabIndex = 3  ' Ordre de tabulation
        '
        'dgvHistorique
        '
        dgvHistorique.AllowUserToAddRows = False  ' Lecture seule : pas d'ajout par l'utilisateur
        dgvHistorique.AllowUserToDeleteRows = False  ' Pas de suppression
        dgvHistorique.AllowUserToResizeRows = False  ' Hauteur des lignes fixe
        dgvHistorique.AutoGenerateColumns = False  ' Colonnes définies à la main ci-dessous
        dgvHistorique.BackgroundColor = System.Drawing.Color.White  ' Fond blanc
        dgvHistorique.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle  ' Fine bordure
        dgvHistorique.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single  ' Quadrillage complet
        dgvHistorique.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single  ' En-têtes avec bordure simple
        dgvHistorique.ColumnHeadersDefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.BackColor = System.Drawing.Color.FromArgb(227, 237, 230), .ForeColor = System.Drawing.Color.FromArgb(23, 38, 31), .Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point), .SelectionBackColor = System.Drawing.Color.FromArgb(227, 237, 230), .SelectionForeColor = System.Drawing.Color.FromArgb(23, 38, 31)}  ' En-tête vert très clair
        dgvHistorique.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize  ' Hauteur d'en-tête automatique
        dgvHistorique.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {colDate, colMagasin, colConditionnement, colPrix})  ' Ajoute les colonnes dans l'ordre de la maquette
        dgvHistorique.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.SelectionBackColor = System.Drawing.Color.FromArgb(221, 235, 226), .SelectionForeColor = System.Drawing.Color.FromArgb(23, 38, 31)}  ' Ligne sélectionnée : vert clair
        dgvHistorique.Dock = System.Windows.Forms.DockStyle.Fill  ' Occupe tout l'espace restant
        dgvHistorique.EnableHeadersVisualStyles = False  ' Obligatoire pour appliquer la couleur d'en-tête
        dgvHistorique.GridColor = System.Drawing.Color.FromArgb(195, 208, 200)  ' Couleur du quadrillage
        dgvHistorique.Location = New System.Drawing.Point(0, 0)  ' Position (recalculée par Dock)
        dgvHistorique.MultiSelect = False  ' Une seule ligne sélectionnée
        dgvHistorique.ReadOnly = True  ' Lecture seule
        dgvHistorique.RowHeadersVisible = False  ' Cache la colonne grise de gauche
        dgvHistorique.RowTemplate.Height = 28  ' Hauteur des lignes : 28 px
        dgvHistorique.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect  ' Un clic sélectionne la ligne
        dgvHistorique.Size = New System.Drawing.Size(300, 200)  ' Taille (recalculée par Dock)
        dgvHistorique.TabIndex = 4  ' Ordre de tabulation
        '
        'colDate
        '
        colDate.DataPropertyName = "DateTexte"  ' Propriété de la ligne affichée
        colDate.FillWeight = 18.0!  ' Largeur relative
        colDate.HeaderText = "Date"  ' Titre de la colonne
        colDate.Name = "colDate"  ' Nom interne
        colDate.ReadOnly = True  ' Lecture seule
        colDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colMagasin
        '
        colMagasin.DataPropertyName = "Magasin"  ' Propriété de la ligne affichée
        colMagasin.FillWeight = 28.0!  ' Largeur relative
        colMagasin.HeaderText = "Magasin"  ' Titre de la colonne
        colMagasin.Name = "colMagasin"  ' Nom interne
        colMagasin.ReadOnly = True  ' Lecture seule
        colMagasin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colConditionnement
        '
        colConditionnement.DataPropertyName = "Conditionnement"  ' Propriété de la ligne affichée
        colConditionnement.FillWeight = 24.0!  ' Largeur relative
        colConditionnement.HeaderText = "Conditionnement"  ' Titre de la colonne
        colConditionnement.Name = "colConditionnement"  ' Nom interne
        colConditionnement.ReadOnly = True  ' Lecture seule
        colConditionnement.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colPrix
        '
        colPrix.DataPropertyName = "PrixTexte"  ' Propriété de la ligne affichée
        colPrix.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight}  ' Nombres alignés à droite
        colPrix.FillWeight = 30.0!  ' Largeur relative
        colPrix.HeaderText = "Prix unitaire"  ' Titre de la colonne
        colPrix.Name = "colPrix"  ' Nom interne
        colPrix.ReadOnly = True  ' Lecture seule
        colPrix.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'frmHistorique
        '
        AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)  ' Dimensions de référence (DPI)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font  ' S'adapte à la police de l'écran
        BackColor = System.Drawing.Color.FromArgb(244, 247, 245)  ' Fond gris très clair
        ClientSize = New System.Drawing.Size(1100, 590)  ' Taille intérieure
        Controls.Add(dgvHistorique)  ' Ajoute dgvHistorique (le contrôle Fill d'abord, puis les bords)
        Controls.Add(pnlGraphique)  ' Ajoute pnlGraphique (le contrôle Fill d'abord, puis les bords)
        Controls.Add(pnlBas)  ' Ajoute pnlBas (le contrôle Fill d'abord, puis les bords)
        Controls.Add(pnlFiltres)  ' Ajoute pnlFiltres (le contrôle Fill d'abord, puis les bords)
        Controls.Add(pnlTitre)  ' Ajoute pnlTitre (le contrôle Fill d'abord, puis les bords)
        MinimumSize = New System.Drawing.Size(1116, 420)  ' Taille minimale
        Name = "frmHistorique"  ' Nom interne
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent  ' Centrée sur la fenêtre parente
        Text = "Historique des prix"  ' Titre de la fenêtre
        pnlTitre.ResumeLayout(False)  ' Reprend la mise en page de pnlTitre
        pnlTitre.PerformLayout()  ' Applique la mise en page
        pnlFiltres.ResumeLayout(False)  ' Reprend la mise en page de pnlFiltres
        pnlFiltres.PerformLayout()  ' Applique la mise en page
        pnlBas.ResumeLayout(False)  ' Reprend la mise en page de pnlBas
        pnlBas.PerformLayout()  ' Applique la mise en page
        CType(dgvHistorique, System.ComponentModel.ISupportInitialize).EndInit()  ' Fin de l'initialisation de la grille
        ResumeLayout(False)  ' Reprend la mise en page du formulaire
    End Sub  ' Fin de InitializeComponent

    Friend WithEvents pnlTitre As System.Windows.Forms.Panel  ' Bandeau du titre
    Friend WithEvents lblTitre As System.Windows.Forms.Label  ' Texte « Historique des prix »
    Friend WithEvents pnlFiltres As System.Windows.Forms.Panel  ' Panneau blanc des filtres
    Friend WithEvents lblProduit As System.Windows.Forms.Label  ' Texte « Produit »
    Friend WithEvents cmbProduit As System.Windows.Forms.ComboBox  ' Choix du produit
    Friend WithEvents lblMagasin As System.Windows.Forms.Label  ' Texte « Magasin »
    Friend WithEvents cmbMagasin As System.Windows.Forms.ComboBox  ' Choix du magasin (ou Tous)
    Friend WithEvents lblAfficher As System.Windows.Forms.Label  ' Texte « Afficher »
    Friend WithEvents cmbAfficher As System.Windows.Forms.ComboBox  ' Période affichée
    Friend WithEvents btnActualiser As System.Windows.Forms.Button  ' Recharge l'historique
    Friend WithEvents pnlBas As System.Windows.Forms.Panel  ' Barre du bas : résumé chiffré
    Friend WithEvents lblResume As System.Windows.Forms.Label  ' Résumé : nombre, minimum, maximum, moyenne, variation
    Friend WithEvents pnlGraphique As PanneauGraphique  ' Zone du graphique dessiné par l'événement Paint
    Friend WithEvents dgvHistorique As System.Windows.Forms.DataGridView  ' Tableau des observations
    Friend WithEvents colDate As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Date »
    Friend WithEvents colMagasin As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Magasin »
    Friend WithEvents colConditionnement As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Conditionnement »
    Friend WithEvents colPrix As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Prix unitaire »
End Class
