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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        pnlTitre = New Panel()
        lblTitre = New Label()
        pnlIndice = New Panel()
        lblIndiceTitre = New Label()
        lblDepart = New Label()
        dtpDepart = New DateTimePicker()
        lblArrivee = New Label()
        dtpArrivee = New DateTimePicker()
        btnCalculer = New Button()
        lblIndiceValeur = New Label()
        lblIndiceTexte = New Label()
        lblIndiceNote = New Label()
        pnlBas = New Panel()
        lblAvertissement = New Label()
        pnlComparaison = New Panel()
        tlpGrilles = New TableLayoutPanel()
        lblTitreMagasins = New Label()
        lblTitreEquivalents = New Label()
        dgvMagasins = New DataGridView()
        dgvEquivalents = New DataGridView()
        pnlCompFiltres = New Panel()
        lblProduit = New Label()
        cmbProduit = New ComboBox()
        lblCategorie = New Label()
        cmbCategorie = New ComboBox()
        btnComparer = New Button()
        btnEquivalents = New Button()
        lblMessage = New Label()
        pnlTitre.SuspendLayout()
        pnlIndice.SuspendLayout()
        pnlBas.SuspendLayout()
        pnlComparaison.SuspendLayout()
        tlpGrilles.SuspendLayout()
        CType(dgvMagasins, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvEquivalents, ComponentModel.ISupportInitialize).BeginInit()
        pnlCompFiltres.SuspendLayout()
        SuspendLayout()
        ' 
        ' pnlTitre
        ' 
        pnlTitre.BackColor = Color.FromArgb(CByte(244), CByte(247), CByte(245))
        pnlTitre.Controls.Add(lblTitre)
        pnlTitre.Dock = DockStyle.Top
        pnlTitre.Location = New Point(0, 0)
        pnlTitre.Margin = New Padding(3, 4, 3, 4)
        pnlTitre.Name = "pnlTitre"
        pnlTitre.Size = New Size(1257, 59)
        pnlTitre.TabIndex = 0
        ' 
        ' lblTitre
        ' 
        lblTitre.AutoSize = True
        lblTitre.Font = New Font("Segoe UI", 18F, FontStyle.Bold)
        lblTitre.ForeColor = Color.ForestGreen
        lblTitre.Location = New Point(16, 13)
        lblTitre.Name = "lblTitre"
        lblTitre.Size = New Size(377, 41)
        lblTitre.TabIndex = 0
        lblTitre.Text = "Analyses et comparaisons"
        ' 
        ' pnlIndice
        ' 
        pnlIndice.BackColor = Color.White
        pnlIndice.Controls.Add(lblIndiceTitre)
        pnlIndice.Controls.Add(lblDepart)
        pnlIndice.Controls.Add(dtpDepart)
        pnlIndice.Controls.Add(lblArrivee)
        pnlIndice.Controls.Add(dtpArrivee)
        pnlIndice.Controls.Add(btnCalculer)
        pnlIndice.Controls.Add(lblIndiceValeur)
        pnlIndice.Controls.Add(lblIndiceTexte)
        pnlIndice.Controls.Add(lblIndiceNote)
        pnlIndice.Dock = DockStyle.Top
        pnlIndice.Location = New Point(0, 59)
        pnlIndice.Margin = New Padding(3, 4, 3, 4)
        pnlIndice.Name = "pnlIndice"
        pnlIndice.Size = New Size(1257, 227)
        pnlIndice.TabIndex = 1
        ' 
        ' lblIndiceTitre
        ' 
        lblIndiceTitre.AutoSize = True
        lblIndiceTitre.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        lblIndiceTitre.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblIndiceTitre.Location = New Point(21, 13)
        lblIndiceTitre.Name = "lblIndiceTitre"
        lblIndiceTitre.Size = New Size(208, 23)
        lblIndiceTitre.TabIndex = 0
        lblIndiceTitre.Text = "Indice de prix personnel "
        ' 
        ' lblDepart
        ' 
        lblDepart.AutoSize = True
        lblDepart.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        lblDepart.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblDepart.Location = New Point(21, 59)
        lblDepart.Name = "lblDepart"
        lblDepart.Size = New Size(110, 19)
        lblDepart.TabIndex = 1
        lblDepart.Text = "Date de départ"
        ' 
        ' dtpDepart
        ' 
        dtpDepart.Format = DateTimePickerFormat.Short
        dtpDepart.Location = New Point(21, 85)
        dtpDepart.Margin = New Padding(3, 4, 3, 4)
        dtpDepart.Name = "dtpDepart"
        dtpDepart.Size = New Size(137, 27)
        dtpDepart.TabIndex = 2
        ' 
        ' lblArrivee
        ' 
        lblArrivee.AutoSize = True
        lblArrivee.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        lblArrivee.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblArrivee.Location = New Point(181, 59)
        lblArrivee.Name = "lblArrivee"
        lblArrivee.Size = New Size(105, 19)
        lblArrivee.TabIndex = 3
        lblArrivee.Text = "Date d'arrivée"
        ' 
        ' dtpArrivee
        ' 
        dtpArrivee.Format = DateTimePickerFormat.Short
        dtpArrivee.Location = New Point(181, 85)
        dtpArrivee.Margin = New Padding(3, 4, 3, 4)
        dtpArrivee.Name = "dtpArrivee"
        dtpArrivee.Size = New Size(137, 27)
        dtpArrivee.TabIndex = 4
        ' 
        ' btnCalculer
        ' 
        btnCalculer.BackColor = Color.ForestGreen
        btnCalculer.FlatAppearance.BorderColor = Color.LimeGreen
        btnCalculer.FlatStyle = FlatStyle.Flat
        btnCalculer.Font = New Font("Segoe UI", 8.5F, FontStyle.Bold)
        btnCalculer.ForeColor = Color.White
        btnCalculer.Location = New Point(341, 83)
        btnCalculer.Margin = New Padding(3, 4, 3, 4)
        btnCalculer.Name = "btnCalculer"
        btnCalculer.Size = New Size(126, 40)
        btnCalculer.TabIndex = 5
        btnCalculer.Text = "Calculer"
        btnCalculer.UseVisualStyleBackColor = False
        ' 
        ' lblIndiceValeur
        ' 
        lblIndiceValeur.AutoSize = True
        lblIndiceValeur.Font = New Font("Segoe UI", 26F, FontStyle.Bold)
        lblIndiceValeur.ForeColor = Color.ForestGreen
        lblIndiceValeur.Location = New Point(503, 45)
        lblIndiceValeur.Name = "lblIndiceValeur"
        lblIndiceValeur.Size = New Size(69, 60)
        lblIndiceValeur.TabIndex = 6
        lblIndiceValeur.Text = "—"
        ' 
        ' lblIndiceTexte
        ' 
        lblIndiceTexte.Font = New Font("Segoe UI", 9F)
        lblIndiceTexte.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblIndiceTexte.Location = New Point(503, 123)
        lblIndiceTexte.Name = "lblIndiceTexte"
        lblIndiceTexte.Size = New Size(686, 53)
        lblIndiceTexte.TabIndex = 7
        ' 
        ' lblIndiceNote
        ' 
        lblIndiceNote.Font = New Font("Segoe UI", 8F)
        lblIndiceNote.ForeColor = Color.FromArgb(CByte(90), CByte(106), CByte(98))
        lblIndiceNote.Location = New Point(21, 184)
        lblIndiceNote.Name = "lblIndiceNote"
        lblIndiceNote.Size = New Size(1143, 27)
        lblIndiceNote.TabIndex = 8
        ' 
        ' pnlBas
        ' 
        pnlBas.BackColor = Color.FromArgb(CByte(244), CByte(247), CByte(245))
        pnlBas.Controls.Add(lblAvertissement)
        pnlBas.Dock = DockStyle.Bottom
        pnlBas.Location = New Point(0, 808)
        pnlBas.Margin = New Padding(3, 4, 3, 4)
        pnlBas.Name = "pnlBas"
        pnlBas.Size = New Size(1257, 45)
        pnlBas.TabIndex = 2
        ' 
        ' lblAvertissement
        ' 
        lblAvertissement.Dock = DockStyle.Fill
        lblAvertissement.Font = New Font("Segoe UI", 8F)
        lblAvertissement.ForeColor = Color.FromArgb(CByte(90), CByte(106), CByte(98))
        lblAvertissement.Location = New Point(0, 0)
        lblAvertissement.Name = "lblAvertissement"
        lblAvertissement.Padding = New Padding(16, 0, 0, 0)
        lblAvertissement.Size = New Size(1257, 45)
        lblAvertissement.TabIndex = 0
        lblAvertissement.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' pnlComparaison
        ' 
        pnlComparaison.BackColor = Color.FromArgb(CByte(244), CByte(247), CByte(245))
        pnlComparaison.Controls.Add(tlpGrilles)
        pnlComparaison.Controls.Add(pnlCompFiltres)
        pnlComparaison.Dock = DockStyle.Fill
        pnlComparaison.Location = New Point(0, 286)
        pnlComparaison.Margin = New Padding(3, 4, 3, 4)
        pnlComparaison.Name = "pnlComparaison"
        pnlComparaison.Size = New Size(1257, 522)
        pnlComparaison.TabIndex = 3
        ' 
        ' tlpGrilles
        ' 
        tlpGrilles.ColumnCount = 2
        tlpGrilles.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        tlpGrilles.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        tlpGrilles.Controls.Add(lblTitreMagasins, 0, 0)
        tlpGrilles.Controls.Add(lblTitreEquivalents, 1, 0)
        tlpGrilles.Controls.Add(dgvMagasins, 0, 1)
        tlpGrilles.Controls.Add(dgvEquivalents, 1, 1)
        tlpGrilles.Dock = DockStyle.Fill
        tlpGrilles.Location = New Point(0, 99)
        tlpGrilles.Margin = New Padding(3, 4, 3, 4)
        tlpGrilles.Name = "tlpGrilles"
        tlpGrilles.RowCount = 2
        tlpGrilles.RowStyles.Add(New RowStyle(SizeType.Absolute, 35F))
        tlpGrilles.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        tlpGrilles.Size = New Size(1257, 423)
        tlpGrilles.TabIndex = 0
        ' 
        ' lblTitreMagasins
        ' 
        lblTitreMagasins.AutoSize = True
        lblTitreMagasins.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblTitreMagasins.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblTitreMagasins.Location = New Point(16, 8)
        lblTitreMagasins.Margin = New Padding(16, 8, 3, 0)
        lblTitreMagasins.Name = "lblTitreMagasins"
        lblTitreMagasins.Size = New Size(187, 20)
        lblTitreMagasins.TabIndex = 0
        lblTitreMagasins.Text = "Classement des magasins"
        ' 
        ' lblTitreEquivalents
        ' 
        lblTitreEquivalents.AutoSize = True
        lblTitreEquivalents.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblTitreEquivalents.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblTitreEquivalents.Location = New Point(644, 8)
        lblTitreEquivalents.Margin = New Padding(16, 8, 3, 0)
        lblTitreEquivalents.Name = "lblTitreEquivalents"
        lblTitreEquivalents.Size = New Size(240, 20)
        lblTitreEquivalents.TabIndex = 1
        lblTitreEquivalents.Text = "Produits équivalents moins chers"
        ' 
        ' dgvMagasins
        ' 
        dgvMagasins.AllowUserToAddRows = False
        dgvMagasins.AllowUserToDeleteRows = False
        dgvMagasins.AllowUserToResizeRows = False
        dgvMagasins.BackgroundColor = Color.White
        dgvMagasins.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        dgvMagasins.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        dgvMagasins.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvMagasins.Dock = DockStyle.Fill
        dgvMagasins.EnableHeadersVisualStyles = False
        dgvMagasins.GridColor = Color.FromArgb(CByte(195), CByte(208), CByte(200))
        dgvMagasins.Location = New Point(3, 39)
        dgvMagasins.Margin = New Padding(3, 4, 3, 4)
        dgvMagasins.MultiSelect = False
        dgvMagasins.Name = "dgvMagasins"
        dgvMagasins.ReadOnly = True
        dgvMagasins.RowHeadersVisible = False
        dgvMagasins.RowHeadersWidth = 51
        dgvMagasins.RowTemplate.Height = 28
        dgvMagasins.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvMagasins.Size = New Size(622, 380)
        dgvMagasins.TabIndex = 2
        ' 
        ' dgvEquivalents
        ' 
        dgvEquivalents.AllowUserToAddRows = False
        dgvEquivalents.AllowUserToDeleteRows = False
        dgvEquivalents.AllowUserToResizeRows = False
        dgvEquivalents.BackgroundColor = Color.White
        dgvEquivalents.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        dgvEquivalents.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        dgvEquivalents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvEquivalents.Dock = DockStyle.Fill
        dgvEquivalents.EnableHeadersVisualStyles = False
        dgvEquivalents.GridColor = Color.FromArgb(CByte(195), CByte(208), CByte(200))
        dgvEquivalents.Location = New Point(631, 39)
        dgvEquivalents.Margin = New Padding(3, 4, 3, 4)
        dgvEquivalents.MultiSelect = False
        dgvEquivalents.Name = "dgvEquivalents"
        dgvEquivalents.ReadOnly = True
        dgvEquivalents.RowHeadersVisible = False
        dgvEquivalents.RowHeadersWidth = 51
        dgvEquivalents.RowTemplate.Height = 28
        dgvEquivalents.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvEquivalents.Size = New Size(623, 380)
        dgvEquivalents.TabIndex = 3
        ' 
        ' pnlCompFiltres
        ' 
        pnlCompFiltres.BackColor = Color.White
        pnlCompFiltres.Controls.Add(lblProduit)
        pnlCompFiltres.Controls.Add(cmbProduit)
        pnlCompFiltres.Controls.Add(lblCategorie)
        pnlCompFiltres.Controls.Add(cmbCategorie)
        pnlCompFiltres.Controls.Add(btnComparer)
        pnlCompFiltres.Controls.Add(btnEquivalents)
        pnlCompFiltres.Controls.Add(lblMessage)
        pnlCompFiltres.Dock = DockStyle.Top
        pnlCompFiltres.Location = New Point(0, 0)
        pnlCompFiltres.Margin = New Padding(3, 4, 3, 4)
        pnlCompFiltres.Name = "pnlCompFiltres"
        pnlCompFiltres.Size = New Size(1257, 99)
        pnlCompFiltres.TabIndex = 1
        ' 
        ' lblProduit
        ' 
        lblProduit.AutoSize = True
        lblProduit.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        lblProduit.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblProduit.Location = New Point(21, 13)
        lblProduit.Name = "lblProduit"
        lblProduit.Size = New Size(59, 19)
        lblProduit.TabIndex = 0
        lblProduit.Text = "Produit"
        ' 
        ' cmbProduit
        ' 
        cmbProduit.DropDownStyle = ComboBoxStyle.DropDownList
        cmbProduit.FormattingEnabled = True
        cmbProduit.Location = New Point(21, 40)
        cmbProduit.Margin = New Padding(3, 4, 3, 4)
        cmbProduit.Name = "cmbProduit"
        cmbProduit.Size = New Size(285, 28)
        cmbProduit.TabIndex = 1
        ' 
        ' lblCategorie
        ' 
        lblCategorie.AutoSize = True
        lblCategorie.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        lblCategorie.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblCategorie.Location = New Point(329, 13)
        lblCategorie.Name = "lblCategorie"
        lblCategorie.Size = New Size(94, 19)
        lblCategorie.TabIndex = 2
        lblCategorie.Text = "ou catégorie"
        ' 
        ' cmbCategorie
        ' 
        cmbCategorie.DropDownStyle = ComboBoxStyle.DropDownList
        cmbCategorie.FormattingEnabled = True
        cmbCategorie.Location = New Point(329, 40)
        cmbCategorie.Margin = New Padding(3, 4, 3, 4)
        cmbCategorie.Name = "cmbCategorie"
        cmbCategorie.Size = New Size(228, 28)
        cmbCategorie.TabIndex = 3
        ' 
        ' btnComparer
        ' 
        btnComparer.BackColor = Color.ForestGreen
        btnComparer.FlatAppearance.BorderColor = Color.ForestGreen
        btnComparer.FlatStyle = FlatStyle.Flat
        btnComparer.Font = New Font("Segoe UI", 8.5F, FontStyle.Bold)
        btnComparer.ForeColor = Color.White
        btnComparer.Location = New Point(581, 37)
        btnComparer.Margin = New Padding(3, 4, 3, 4)
        btnComparer.Name = "btnComparer"
        btnComparer.Size = New Size(171, 40)
        btnComparer.TabIndex = 4
        btnComparer.Text = "Comparer magasins"
        btnComparer.UseVisualStyleBackColor = False
        ' 
        ' btnEquivalents
        ' 
        btnEquivalents.BackColor = Color.White
        btnEquivalents.FlatAppearance.BorderColor = Color.ForestGreen
        btnEquivalents.FlatStyle = FlatStyle.Flat
        btnEquivalents.Font = New Font("Segoe UI", 8.5F, FontStyle.Bold)
        btnEquivalents.ForeColor = Color.ForestGreen
        btnEquivalents.Location = New Point(763, 37)
        btnEquivalents.Margin = New Padding(3, 4, 3, 4)
        btnEquivalents.Name = "btnEquivalents"
        btnEquivalents.Size = New Size(229, 40)
        btnEquivalents.TabIndex = 5
        btnEquivalents.Text = "Voir équivalents moins chers"
        btnEquivalents.UseVisualStyleBackColor = False
        ' 
        ' lblMessage
        ' 
        lblMessage.AutoSize = True
        lblMessage.Font = New Font("Segoe UI", 8F)
        lblMessage.ForeColor = Color.FromArgb(CByte(90), CByte(106), CByte(98))
        lblMessage.Location = New Point(21, 75)
        lblMessage.Name = "lblMessage"
        lblMessage.Size = New Size(0, 19)
        lblMessage.TabIndex = 6
        ' 
        ' frmAnalyses
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(244), CByte(247), CByte(245))
        ClientSize = New Size(1257, 853)
        Controls.Add(pnlComparaison)
        Controls.Add(pnlBas)
        Controls.Add(pnlIndice)
        Controls.Add(pnlTitre)
        Margin = New Padding(3, 4, 3, 4)
        MinimumSize = New Size(1273, 678)
        Name = "frmAnalyses"
        StartPosition = FormStartPosition.CenterParent
        Text = "Analyses et comparaisons"
        pnlTitre.ResumeLayout(False)
        pnlTitre.PerformLayout()
        pnlIndice.ResumeLayout(False)
        pnlIndice.PerformLayout()
        pnlBas.ResumeLayout(False)
        pnlComparaison.ResumeLayout(False)
        tlpGrilles.ResumeLayout(False)
        tlpGrilles.PerformLayout()
        CType(dgvMagasins, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvEquivalents, ComponentModel.ISupportInitialize).EndInit()
        pnlCompFiltres.ResumeLayout(False)
        pnlCompFiltres.PerformLayout()
        ResumeLayout(False)
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
