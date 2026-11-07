' ============================================================================================
' FICHIER GÉNÉRÉ PAR LE CONCEPTEUR DE FORMULAIRES DE VISUAL STUDIO (partie « dessin » de frmDepenses).
' Il décrit les contrôles de la fenêtre, leur position et leurs propriétés ; le comportement est dans frmDepenses.vb.
' Mise en page conforme à la maquette du groupe, en vert forêt, avec des zones « Dock » (pas d'Anchor à droite :
' elles se déplacent mal quand l'écran est réglé à 125 % ou 150 %).
' ============================================================================================
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  ' Fichier généré par le concepteur
Partial Class frmDepenses  ' Partial : la classe est coupée en deux fichiers (apparence ici, comportement dans frmDepenses.vb)
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
        pnlRappel = New System.Windows.Forms.Panel()  ' Crée pnlRappel
        lblRappel = New System.Windows.Forms.Label()  ' Crée lblRappel
        pnlForm = New System.Windows.Forms.Panel()  ' Crée pnlForm
        lblFormTitre = New System.Windows.Forms.Label()  ' Crée lblFormTitre
        lblNom = New System.Windows.Forms.Label()  ' Crée lblNom
        txtNom = New System.Windows.Forms.TextBox()  ' Crée txtNom
        lblMontant = New System.Windows.Forms.Label()  ' Crée lblMontant
        txtMontant = New System.Windows.Forms.TextBox()  ' Crée txtMontant
        lblPeriodicite = New System.Windows.Forms.Label()  ' Crée lblPeriodicite
        cmbPeriodicite = New System.Windows.Forms.ComboBox()  ' Crée cmbPeriodicite
        lblEcheance = New System.Windows.Forms.Label()  ' Crée lblEcheance
        dtpEcheance = New System.Windows.Forms.DateTimePicker()  ' Crée dtpEcheance
        btnEnregistrer = New System.Windows.Forms.Button()  ' Crée btnEnregistrer
        btnNouveau = New System.Windows.Forms.Button()  ' Crée btnNouveau
        btnEcheanceSuivante = New System.Windows.Forms.Button()  ' Crée btnEcheanceSuivante
        btnDesactiver = New System.Windows.Forms.Button()  ' Crée btnDesactiver
        lblNote = New System.Windows.Forms.Label()  ' Crée lblNote
        dgvDepenses = New System.Windows.Forms.DataGridView()  ' Crée dgvDepenses
        colNom = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colNom
        colMontant = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colMontant
        colPeriodicite = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colPeriodicite
        colEcheance = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colEcheance
        colRappel = New System.Windows.Forms.DataGridViewTextBoxColumn()  ' Crée colRappel
        pnlTitre.SuspendLayout()  ' Suspend la mise en page de pnlTitre pendant le réglage
        pnlRappel.SuspendLayout()  ' Suspend la mise en page de pnlRappel pendant le réglage
        pnlForm.SuspendLayout()  ' Suspend la mise en page de pnlForm pendant le réglage
        CType(dgvDepenses, System.ComponentModel.ISupportInitialize).BeginInit()  ' Début de l'initialisation de la grille
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
        lblTitre.Text = "Dépenses récurrentes"
        lblTitre.Name = "lblTitre"  ' Nom interne
        lblTitre.TabIndex = 0  ' Ordre de tabulation
        '
        'pnlRappel
        '
        pnlRappel.Controls.Add(lblRappel)  ' Place lblRappel dans pnlRappel
        pnlRappel.BackColor = System.Drawing.Color.FromArgb(253, 243, 214)
        pnlRappel.Dock = System.Windows.Forms.DockStyle.Bottom
        pnlRappel.Size = New System.Drawing.Size(100, 44)
        pnlRappel.Name = "pnlRappel"  ' Nom interne
        pnlRappel.TabIndex = 1  ' Ordre de tabulation
        '
        'lblRappel
        '
        lblRappel.AutoSize = False
        lblRappel.Dock = System.Windows.Forms.DockStyle.Fill
        lblRappel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblRappel.ForeColor = System.Drawing.Color.FromArgb(122, 51, 0)
        lblRappel.Padding = New System.Windows.Forms.Padding(14, 0, 0, 0)
        lblRappel.Text = ""
        lblRappel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        lblRappel.Name = "lblRappel"  ' Nom interne
        lblRappel.TabIndex = 0  ' Ordre de tabulation
        '
        'pnlForm
        '
        pnlForm.Controls.Add(lblFormTitre)  ' Place lblFormTitre dans pnlForm
        pnlForm.Controls.Add(lblNom)  ' Place lblNom dans pnlForm
        pnlForm.Controls.Add(txtNom)  ' Place txtNom dans pnlForm
        pnlForm.Controls.Add(lblMontant)  ' Place lblMontant dans pnlForm
        pnlForm.Controls.Add(txtMontant)  ' Place txtMontant dans pnlForm
        pnlForm.Controls.Add(lblPeriodicite)  ' Place lblPeriodicite dans pnlForm
        pnlForm.Controls.Add(cmbPeriodicite)  ' Place cmbPeriodicite dans pnlForm
        pnlForm.Controls.Add(lblEcheance)  ' Place lblEcheance dans pnlForm
        pnlForm.Controls.Add(dtpEcheance)  ' Place dtpEcheance dans pnlForm
        pnlForm.Controls.Add(btnEnregistrer)  ' Place btnEnregistrer dans pnlForm
        pnlForm.Controls.Add(btnNouveau)  ' Place btnNouveau dans pnlForm
        pnlForm.Controls.Add(btnEcheanceSuivante)  ' Place btnEcheanceSuivante dans pnlForm
        pnlForm.Controls.Add(btnDesactiver)  ' Place btnDesactiver dans pnlForm
        pnlForm.Controls.Add(lblNote)  ' Place lblNote dans pnlForm
        pnlForm.BackColor = System.Drawing.Color.White
        pnlForm.Dock = System.Windows.Forms.DockStyle.Right
        pnlForm.Size = New System.Drawing.Size(320, 100)
        pnlForm.Name = "pnlForm"  ' Nom interne
        pnlForm.TabIndex = 2  ' Ordre de tabulation
        '
        'lblFormTitre
        '
        lblFormTitre.AutoSize = True
        lblFormTitre.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblFormTitre.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblFormTitre.Location = New System.Drawing.Point(16, 14)
        lblFormTitre.Text = "Dépense"
        lblFormTitre.Name = "lblFormTitre"  ' Nom interne
        lblFormTitre.TabIndex = 0  ' Ordre de tabulation
        '
        'lblNom
        '
        lblNom.AutoSize = True
        lblNom.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblNom.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblNom.Location = New System.Drawing.Point(16, 48)
        lblNom.Text = "Nom"
        lblNom.Name = "lblNom"  ' Nom interne
        lblNom.TabIndex = 1  ' Ordre de tabulation
        '
        'txtNom
        '
        txtNom.Location = New System.Drawing.Point(16, 68)
        txtNom.Size = New System.Drawing.Size(284, 23)
        txtNom.MaxLength = 180
        txtNom.Name = "txtNom"  ' Nom interne
        txtNom.TabIndex = 2  ' Ordre de tabulation
        '
        'lblMontant
        '
        lblMontant.AutoSize = True
        lblMontant.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblMontant.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblMontant.Location = New System.Drawing.Point(16, 102)
        lblMontant.Text = "Montant (FCFA)"
        lblMontant.Name = "lblMontant"  ' Nom interne
        lblMontant.TabIndex = 3  ' Ordre de tabulation
        '
        'txtMontant
        '
        txtMontant.Location = New System.Drawing.Point(16, 122)
        txtMontant.Size = New System.Drawing.Size(284, 23)
        txtMontant.Name = "txtMontant"  ' Nom interne
        txtMontant.TabIndex = 4  ' Ordre de tabulation
        '
        'lblPeriodicite
        '
        lblPeriodicite.AutoSize = True
        lblPeriodicite.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblPeriodicite.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblPeriodicite.Location = New System.Drawing.Point(16, 156)
        lblPeriodicite.Text = "Périodicité"
        lblPeriodicite.Name = "lblPeriodicite"  ' Nom interne
        lblPeriodicite.TabIndex = 5  ' Ordre de tabulation
        '
        'cmbPeriodicite
        '
        cmbPeriodicite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbPeriodicite.FormattingEnabled = True
        cmbPeriodicite.Location = New System.Drawing.Point(16, 176)
        cmbPeriodicite.Size = New System.Drawing.Size(284, 23)
        cmbPeriodicite.Name = "cmbPeriodicite"  ' Nom interne
        cmbPeriodicite.TabIndex = 6  ' Ordre de tabulation
        '
        'lblEcheance
        '
        lblEcheance.AutoSize = True
        lblEcheance.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblEcheance.ForeColor = System.Drawing.Color.FromArgb(23, 38, 31)
        lblEcheance.Location = New System.Drawing.Point(16, 210)
        lblEcheance.Text = "Prochaine échéance"
        lblEcheance.Name = "lblEcheance"  ' Nom interne
        lblEcheance.TabIndex = 7  ' Ordre de tabulation
        '
        'dtpEcheance
        '
        dtpEcheance.Format = System.Windows.Forms.DateTimePickerFormat.Short
        dtpEcheance.Location = New System.Drawing.Point(16, 230)
        dtpEcheance.Size = New System.Drawing.Size(284, 23)
        dtpEcheance.Name = "dtpEcheance"  ' Nom interne
        dtpEcheance.TabIndex = 8  ' Ordre de tabulation
        '
        'btnEnregistrer
        '
        btnEnregistrer.BackColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnEnregistrer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        btnEnregistrer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnEnregistrer.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        btnEnregistrer.ForeColor = System.Drawing.Color.White
        btnEnregistrer.Location = New System.Drawing.Point(16, 274)
        btnEnregistrer.Size = New System.Drawing.Size(130, 30)
        btnEnregistrer.Text = "Enregistrer"
        btnEnregistrer.UseVisualStyleBackColor = False
        btnEnregistrer.Name = "btnEnregistrer"  ' Nom interne
        btnEnregistrer.TabIndex = 9  ' Ordre de tabulation
        '
        'btnNouveau
        '
        btnNouveau.BackColor = System.Drawing.Color.White
        btnNouveau.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        btnNouveau.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnNouveau.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        btnNouveau.ForeColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnNouveau.Location = New System.Drawing.Point(154, 274)
        btnNouveau.Size = New System.Drawing.Size(146, 30)
        btnNouveau.Text = "Nouvelle dépense"
        btnNouveau.UseVisualStyleBackColor = False
        btnNouveau.Name = "btnNouveau"  ' Nom interne
        btnNouveau.TabIndex = 10  ' Ordre de tabulation
        '
        'btnEcheanceSuivante
        '
        btnEcheanceSuivante.BackColor = System.Drawing.Color.White
        btnEcheanceSuivante.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        btnEcheanceSuivante.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnEcheanceSuivante.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        btnEcheanceSuivante.ForeColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnEcheanceSuivante.Location = New System.Drawing.Point(16, 314)
        btnEcheanceSuivante.Size = New System.Drawing.Size(284, 30)
        btnEcheanceSuivante.Text = "Échéance suivante"
        btnEcheanceSuivante.UseVisualStyleBackColor = False
        btnEcheanceSuivante.Name = "btnEcheanceSuivante"  ' Nom interne
        btnEcheanceSuivante.TabIndex = 11  ' Ordre de tabulation
        '
        'btnDesactiver
        '
        btnDesactiver.BackColor = System.Drawing.Color.White
        btnDesactiver.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        btnDesactiver.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnDesactiver.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        btnDesactiver.ForeColor = System.Drawing.Color.FromArgb(30, 77, 58)
        btnDesactiver.Location = New System.Drawing.Point(16, 354)
        btnDesactiver.Size = New System.Drawing.Size(284, 30)
        btnDesactiver.Text = "Désactiver"
        btnDesactiver.UseVisualStyleBackColor = False
        btnDesactiver.Name = "btnDesactiver"  ' Nom interne
        btnDesactiver.TabIndex = 12  ' Ordre de tabulation
        '
        'lblNote
        '
        lblNote.AutoSize = False
        lblNote.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        lblNote.ForeColor = System.Drawing.Color.FromArgb(90, 106, 98)
        lblNote.Location = New System.Drawing.Point(16, 402)
        lblNote.Size = New System.Drawing.Size(284, 60)
        lblNote.Text = "Aucun paiement automatique : l'application rappelle seulement l'échéance."
        lblNote.Name = "lblNote"  ' Nom interne
        lblNote.TabIndex = 13  ' Ordre de tabulation
        '
        'dgvDepenses
        '
        dgvDepenses.AllowUserToAddRows = False  ' Lecture seule : pas d'ajout par l'utilisateur
        dgvDepenses.AllowUserToDeleteRows = False  ' Pas de suppression
        dgvDepenses.AllowUserToResizeRows = False  ' Hauteur des lignes fixe
        dgvDepenses.AutoGenerateColumns = False  ' Colonnes définies à la main ci-dessous
        dgvDepenses.BackgroundColor = System.Drawing.Color.White  ' Fond blanc
        dgvDepenses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle  ' Fine bordure
        dgvDepenses.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single  ' Quadrillage complet
        dgvDepenses.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single  ' En-têtes avec bordure simple
        dgvDepenses.ColumnHeadersDefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.BackColor = System.Drawing.Color.FromArgb(227, 237, 230), .ForeColor = System.Drawing.Color.FromArgb(23, 38, 31), .Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point), .SelectionBackColor = System.Drawing.Color.FromArgb(227, 237, 230), .SelectionForeColor = System.Drawing.Color.FromArgb(23, 38, 31)}  ' En-tête vert très clair
        dgvDepenses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize  ' Hauteur d'en-tête automatique
        dgvDepenses.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {colNom, colMontant, colPeriodicite, colEcheance, colRappel})  ' Ajoute les colonnes dans l'ordre de la maquette
        dgvDepenses.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.SelectionBackColor = System.Drawing.Color.FromArgb(221, 235, 226), .SelectionForeColor = System.Drawing.Color.FromArgb(23, 38, 31)}  ' Ligne sélectionnée : vert clair
        dgvDepenses.Dock = System.Windows.Forms.DockStyle.Fill  ' Occupe tout l'espace restant
        dgvDepenses.EnableHeadersVisualStyles = False  ' Obligatoire pour appliquer la couleur d'en-tête
        dgvDepenses.GridColor = System.Drawing.Color.FromArgb(195, 208, 200)  ' Couleur du quadrillage
        dgvDepenses.Location = New System.Drawing.Point(0, 0)  ' Position (recalculée par Dock)
        dgvDepenses.MultiSelect = False  ' Une seule ligne sélectionnée
        dgvDepenses.ReadOnly = True  ' Lecture seule
        dgvDepenses.RowHeadersVisible = False  ' Cache la colonne grise de gauche
        dgvDepenses.RowTemplate.Height = 28  ' Hauteur des lignes : 28 px
        dgvDepenses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect  ' Un clic sélectionne la ligne
        dgvDepenses.Size = New System.Drawing.Size(300, 200)  ' Taille (recalculée par Dock)
        dgvDepenses.TabIndex = 3  ' Ordre de tabulation
        '
        'colNom
        '
        colNom.DataPropertyName = "Nom"  ' Propriété de la ligne affichée
        colNom.FillWeight = 30.0!  ' Largeur relative
        colNom.HeaderText = "Dépense"  ' Titre de la colonne
        colNom.Name = "colNom"  ' Nom interne
        colNom.ReadOnly = True  ' Lecture seule
        colNom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colMontant
        '
        colMontant.DataPropertyName = "MontantTexte"  ' Propriété de la ligne affichée
        colMontant.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight}  ' Nombres alignés à droite
        colMontant.FillWeight = 18.0!  ' Largeur relative
        colMontant.HeaderText = "Montant"  ' Titre de la colonne
        colMontant.Name = "colMontant"  ' Nom interne
        colMontant.ReadOnly = True  ' Lecture seule
        colMontant.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colPeriodicite
        '
        colPeriodicite.DataPropertyName = "PeriodiciteTexte"  ' Propriété de la ligne affichée
        colPeriodicite.FillWeight = 18.0!  ' Largeur relative
        colPeriodicite.HeaderText = "Périodicité"  ' Titre de la colonne
        colPeriodicite.Name = "colPeriodicite"  ' Nom interne
        colPeriodicite.ReadOnly = True  ' Lecture seule
        colPeriodicite.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colEcheance
        '
        colEcheance.DataPropertyName = "EcheanceTexte"  ' Propriété de la ligne affichée
        colEcheance.FillWeight = 18.0!  ' Largeur relative
        colEcheance.HeaderText = "Prochaine échéance"  ' Titre de la colonne
        colEcheance.Name = "colEcheance"  ' Nom interne
        colEcheance.ReadOnly = True  ' Lecture seule
        colEcheance.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'colRappel
        '
        colRappel.DataPropertyName = "RappelTexte"  ' Propriété de la ligne affichée
        colRappel.FillWeight = 16.0!  ' Largeur relative
        colRappel.HeaderText = "Rappel"  ' Titre de la colonne
        colRappel.Name = "colRappel"  ' Nom interne
        colRappel.ReadOnly = True  ' Lecture seule
        colRappel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable  ' Pas de tri par clic (l'ordre est celui du service)
        '
        'frmDepenses
        '
        AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)  ' Dimensions de référence (DPI)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font  ' S'adapte à la police de l'écran
        BackColor = System.Drawing.Color.FromArgb(244, 247, 245)  ' Fond gris très clair
        ClientSize = New System.Drawing.Size(1100, 590)  ' Taille intérieure
        Controls.Add(dgvDepenses)  ' Ajoute dgvDepenses (le contrôle Fill d'abord, puis les bords)
        Controls.Add(pnlForm)  ' Ajoute pnlForm (le contrôle Fill d'abord, puis les bords)
        Controls.Add(pnlRappel)  ' Ajoute pnlRappel (le contrôle Fill d'abord, puis les bords)
        Controls.Add(pnlTitre)  ' Ajoute pnlTitre (le contrôle Fill d'abord, puis les bords)
        MinimumSize = New System.Drawing.Size(1116, 520)  ' Taille minimale
        Name = "frmDepenses"  ' Nom interne
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent  ' Centrée sur la fenêtre parente
        Text = "Dépenses récurrentes"  ' Titre de la fenêtre
        pnlTitre.ResumeLayout(False)  ' Reprend la mise en page de pnlTitre
        pnlTitre.PerformLayout()  ' Applique la mise en page
        pnlRappel.ResumeLayout(False)  ' Reprend la mise en page de pnlRappel
        pnlRappel.PerformLayout()  ' Applique la mise en page
        pnlForm.ResumeLayout(False)  ' Reprend la mise en page de pnlForm
        pnlForm.PerformLayout()  ' Applique la mise en page
        CType(dgvDepenses, System.ComponentModel.ISupportInitialize).EndInit()  ' Fin de l'initialisation de la grille
        ResumeLayout(False)  ' Reprend la mise en page du formulaire
    End Sub  ' Fin de InitializeComponent

    Friend WithEvents pnlTitre As System.Windows.Forms.Panel  ' Bandeau du titre
    Friend WithEvents lblTitre As System.Windows.Forms.Label  ' Texte « Dépenses récurrentes »
    Friend WithEvents pnlRappel As System.Windows.Forms.Panel  ' Bandeau des rappels d'échéance
    Friend WithEvents lblRappel As System.Windows.Forms.Label  ' Texte du rappel (échéances proches)
    Friend WithEvents pnlForm As System.Windows.Forms.Panel  ' Formulaire d'une dépense
    Friend WithEvents lblFormTitre As System.Windows.Forms.Label  ' Texte « Dépense »
    Friend WithEvents lblNom As System.Windows.Forms.Label  ' Texte « Nom »
    Friend WithEvents txtNom As System.Windows.Forms.TextBox  ' Nom de la dépense
    Friend WithEvents lblMontant As System.Windows.Forms.Label  ' Texte « Montant (FCFA) »
    Friend WithEvents txtMontant As System.Windows.Forms.TextBox  ' Montant
    Friend WithEvents lblPeriodicite As System.Windows.Forms.Label  ' Texte « Périodicité »
    Friend WithEvents cmbPeriodicite As System.Windows.Forms.ComboBox  ' Hebdomadaire, mensuelle, trimestrielle ou annuelle
    Friend WithEvents lblEcheance As System.Windows.Forms.Label  ' Texte « Prochaine échéance »
    Friend WithEvents dtpEcheance As System.Windows.Forms.DateTimePicker  ' Date de la prochaine échéance
    Friend WithEvents btnEnregistrer As System.Windows.Forms.Button  ' Enregistre la dépense
    Friend WithEvents btnNouveau As System.Windows.Forms.Button  ' Vide le formulaire
    Friend WithEvents btnEcheanceSuivante As System.Windows.Forms.Button  ' Avance la date d'une période (après règlement)
    Friend WithEvents btnDesactiver As System.Windows.Forms.Button  ' Retire la dépense de la liste
    Friend WithEvents lblNote As System.Windows.Forms.Label  ' Note sur l'absence de paiement automatique
    Friend WithEvents dgvDepenses As System.Windows.Forms.DataGridView  ' Liste des dépenses actives
    Friend WithEvents colNom As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Dépense »
    Friend WithEvents colMontant As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Montant »
    Friend WithEvents colPeriodicite As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Périodicité »
    Friend WithEvents colEcheance As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Prochaine échéance »
    Friend WithEvents colRappel As System.Windows.Forms.DataGridViewTextBoxColumn  ' Colonne « Rappel »
End Class
