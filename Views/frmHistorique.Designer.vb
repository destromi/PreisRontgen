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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        pnlTitre = New Panel()
        lblTitre = New Label()
        pnlFiltres = New Panel()
        lblProduit = New Label()
        cmbProduit = New ComboBox()
        lblMagasin = New Label()
        cmbMagasin = New ComboBox()
        lblAfficher = New Label()
        cmbAfficher = New ComboBox()
        btnActualiser = New Button()
        pnlBas = New Panel()
        lblResume = New Label()
        dgvHistorique = New DataGridView()
        pnlTitre.SuspendLayout()
        pnlFiltres.SuspendLayout()
        pnlBas.SuspendLayout()
        CType(dgvHistorique, ComponentModel.ISupportInitialize).BeginInit()
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
        lblTitre.Size = New Size(288, 41)
        lblTitre.TabIndex = 0
        lblTitre.Text = "Historique des prix"
        ' 
        ' pnlFiltres
        ' 
        pnlFiltres.BackColor = Color.White
        pnlFiltres.Controls.Add(lblProduit)
        pnlFiltres.Controls.Add(cmbProduit)
        pnlFiltres.Controls.Add(lblMagasin)
        pnlFiltres.Controls.Add(cmbMagasin)
        pnlFiltres.Controls.Add(lblAfficher)
        pnlFiltres.Controls.Add(cmbAfficher)
        pnlFiltres.Controls.Add(btnActualiser)
        pnlFiltres.Dock = DockStyle.Top
        pnlFiltres.Location = New Point(0, 59)
        pnlFiltres.Margin = New Padding(3, 4, 3, 4)
        pnlFiltres.Name = "pnlFiltres"
        pnlFiltres.Size = New Size(1257, 99)
        pnlFiltres.TabIndex = 1
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
        ' lblMagasin
        ' 
        lblMagasin.AutoSize = True
        lblMagasin.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        lblMagasin.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblMagasin.Location = New Point(329, 13)
        lblMagasin.Name = "lblMagasin"
        lblMagasin.Size = New Size(65, 19)
        lblMagasin.TabIndex = 2
        lblMagasin.Text = "Magasin"
        ' 
        ' cmbMagasin
        ' 
        cmbMagasin.DropDownStyle = ComboBoxStyle.DropDownList
        cmbMagasin.FormattingEnabled = True
        cmbMagasin.Location = New Point(329, 40)
        cmbMagasin.Margin = New Padding(3, 4, 3, 4)
        cmbMagasin.Name = "cmbMagasin"
        cmbMagasin.Size = New Size(194, 28)
        cmbMagasin.TabIndex = 3
        ' 
        ' lblAfficher
        ' 
        lblAfficher.AutoSize = True
        lblAfficher.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        lblAfficher.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblAfficher.Location = New Point(546, 13)
        lblAfficher.Name = "lblAfficher"
        lblAfficher.Size = New Size(62, 19)
        lblAfficher.TabIndex = 4
        lblAfficher.Text = "Afficher"
        ' 
        ' cmbAfficher
        ' 
        cmbAfficher.DropDownStyle = ComboBoxStyle.DropDownList
        cmbAfficher.FormattingEnabled = True
        cmbAfficher.Location = New Point(546, 40)
        cmbAfficher.Margin = New Padding(3, 4, 3, 4)
        cmbAfficher.Name = "cmbAfficher"
        cmbAfficher.Size = New Size(171, 28)
        cmbAfficher.TabIndex = 5
        ' 
        ' btnActualiser
        ' 
        btnActualiser.BackColor = Color.ForestGreen
        btnActualiser.FlatAppearance.BorderColor = Color.ForestGreen
        btnActualiser.FlatStyle = FlatStyle.Flat
        btnActualiser.Font = New Font("Segoe UI", 8.5F, FontStyle.Bold)
        btnActualiser.ForeColor = Color.White
        btnActualiser.Location = New Point(741, 37)
        btnActualiser.Margin = New Padding(3, 4, 3, 4)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(126, 40)
        btnActualiser.TabIndex = 6
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = False
        ' 
        ' pnlBas
        ' 
        pnlBas.BackColor = Color.FromArgb(CByte(244), CByte(247), CByte(245))
        pnlBas.Controls.Add(lblResume)
        pnlBas.Dock = DockStyle.Bottom
        pnlBas.Location = New Point(0, 734)
        pnlBas.Margin = New Padding(3, 4, 3, 4)
        pnlBas.Name = "pnlBas"
        pnlBas.Size = New Size(1257, 53)
        pnlBas.TabIndex = 2
        ' 
        ' lblResume
        ' 
        lblResume.Dock = DockStyle.Fill
        lblResume.Font = New Font("Segoe UI", 9F)
        lblResume.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblResume.Location = New Point(0, 0)
        lblResume.Name = "lblResume"
        lblResume.Padding = New Padding(16, 0, 0, 0)
        lblResume.Size = New Size(1257, 53)
        lblResume.TabIndex = 0
        lblResume.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' dgvHistorique
        ' 
        dgvHistorique.AllowUserToAddRows = False
        dgvHistorique.AllowUserToDeleteRows = False
        dgvHistorique.AllowUserToResizeRows = False
        dgvHistorique.BackgroundColor = Color.White
        dgvHistorique.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        dgvHistorique.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        dgvHistorique.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvHistorique.Dock = DockStyle.Fill
        dgvHistorique.EnableHeadersVisualStyles = False
        dgvHistorique.GridColor = Color.FromArgb(CByte(195), CByte(208), CByte(200))
        dgvHistorique.Location = New Point(0, 158)
        dgvHistorique.Margin = New Padding(3, 4, 3, 4)
        dgvHistorique.MultiSelect = False
        dgvHistorique.Name = "dgvHistorique"
        dgvHistorique.ReadOnly = True
        dgvHistorique.RowHeadersVisible = False
        dgvHistorique.RowHeadersWidth = 51
        dgvHistorique.RowTemplate.Height = 28
        dgvHistorique.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvHistorique.Size = New Size(1257, 576)
        dgvHistorique.TabIndex = 4
        ' 
        ' frmHistorique
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(244), CByte(247), CByte(245))
        ClientSize = New Size(1257, 787)
        Controls.Add(dgvHistorique)
        Controls.Add(pnlBas)
        Controls.Add(pnlFiltres)
        Controls.Add(pnlTitre)
        Margin = New Padding(3, 4, 3, 4)
        MinimumSize = New Size(1273, 544)
        Name = "frmHistorique"
        StartPosition = FormStartPosition.CenterParent
        Text = "Historique des prix"
        pnlTitre.ResumeLayout(False)
        pnlTitre.PerformLayout()
        pnlFiltres.ResumeLayout(False)
        pnlFiltres.PerformLayout()
        pnlBas.ResumeLayout(False)
        CType(dgvHistorique, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
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
