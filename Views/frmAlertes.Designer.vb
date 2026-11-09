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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        pnlTitre = New Panel()
        lblTitre = New Label()
        pnlFiltres = New Panel()
        lblDu = New Label()
        dtpDebut = New DateTimePicker()
        lblAu = New Label()
        dtpFin = New DateTimePicker()
        lblMagasin = New Label()
        cmbMagasin = New ComboBox()
        lblType = New Label()
        cmbType = New ComboBox()
        chkNonLues = New CheckBox()
        btnFiltrer = New Button()
        btnMarquerLue = New Button()
        pnlDetail = New Panel()
        lblDetailTitre = New Label()
        lblDetail = New Label()
        pnlBas = New Panel()
        lblCompteur = New Label()
        dgvAlertes = New DataGridView()
        pnlTitre.SuspendLayout()
        pnlFiltres.SuspendLayout()
        pnlDetail.SuspendLayout()
        pnlBas.SuspendLayout()
        CType(dgvAlertes, ComponentModel.ISupportInitialize).BeginInit()
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
        lblTitre.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitre.ForeColor = Color.ForestGreen
        lblTitre.Location = New Point(16, 13)
        lblTitre.Name = "lblTitre"
        lblTitre.Size = New Size(226, 41)
        lblTitre.TabIndex = 0
        lblTitre.Text = "Alertes de prix"
        ' 
        ' pnlFiltres
        ' 
        pnlFiltres.BackColor = Color.White
        pnlFiltres.Controls.Add(lblDu)
        pnlFiltres.Controls.Add(dtpDebut)
        pnlFiltres.Controls.Add(lblAu)
        pnlFiltres.Controls.Add(dtpFin)
        pnlFiltres.Controls.Add(lblMagasin)
        pnlFiltres.Controls.Add(cmbMagasin)
        pnlFiltres.Controls.Add(lblType)
        pnlFiltres.Controls.Add(cmbType)
        pnlFiltres.Controls.Add(chkNonLues)
        pnlFiltres.Controls.Add(btnFiltrer)
        pnlFiltres.Controls.Add(btnMarquerLue)
        pnlFiltres.Dock = DockStyle.Top
        pnlFiltres.Location = New Point(0, 59)
        pnlFiltres.Margin = New Padding(3, 4, 3, 4)
        pnlFiltres.Name = "pnlFiltres"
        pnlFiltres.Size = New Size(1257, 99)
        pnlFiltres.TabIndex = 1
        ' 
        ' lblDu
        ' 
        lblDu.AutoSize = True
        lblDu.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        lblDu.Location = New Point(21, 13)
        lblDu.Name = "lblDu"
        lblDu.Size = New Size(27, 19)
        lblDu.TabIndex = 0
        lblDu.Text = "Du"
        ' 
        ' dtpDebut
        ' 
        dtpDebut.Format = DateTimePickerFormat.Short
        dtpDebut.Location = New Point(21, 40)
        dtpDebut.Margin = New Padding(3, 4, 3, 4)
        dtpDebut.Name = "dtpDebut"
        dtpDebut.Size = New Size(125, 27)
        dtpDebut.TabIndex = 1
        ' 
        ' lblAu
        ' 
        lblAu.AutoSize = True
        lblAu.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        lblAu.Location = New Point(169, 13)
        lblAu.Name = "lblAu"
        lblAu.Size = New Size(27, 19)
        lblAu.TabIndex = 2
        lblAu.Text = "Au"
        ' 
        ' dtpFin
        ' 
        dtpFin.Format = DateTimePickerFormat.Short
        dtpFin.Location = New Point(169, 40)
        dtpFin.Margin = New Padding(3, 4, 3, 4)
        dtpFin.Name = "dtpFin"
        dtpFin.Size = New Size(125, 27)
        dtpFin.TabIndex = 3
        ' 
        ' lblMagasin
        ' 
        lblMagasin.AutoSize = True
        lblMagasin.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        lblMagasin.Location = New Point(318, 13)
        lblMagasin.Name = "lblMagasin"
        lblMagasin.Size = New Size(65, 19)
        lblMagasin.TabIndex = 4
        lblMagasin.Text = "Magasin"
        ' 
        ' cmbMagasin
        ' 
        cmbMagasin.DropDownStyle = ComboBoxStyle.DropDownList
        cmbMagasin.Location = New Point(318, 40)
        cmbMagasin.Margin = New Padding(3, 4, 3, 4)
        cmbMagasin.Name = "cmbMagasin"
        cmbMagasin.Size = New Size(182, 28)
        cmbMagasin.TabIndex = 5
        ' 
        ' lblType
        ' 
        lblType.AutoSize = True
        lblType.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        lblType.Location = New Point(523, 13)
        lblType.Name = "lblType"
        lblType.Size = New Size(41, 19)
        lblType.TabIndex = 6
        lblType.Text = "Type"
        ' 
        ' cmbType
        ' 
        cmbType.DropDownStyle = ComboBoxStyle.DropDownList
        cmbType.Location = New Point(523, 40)
        cmbType.Margin = New Padding(3, 4, 3, 4)
        cmbType.Name = "cmbType"
        cmbType.Size = New Size(148, 28)
        cmbType.TabIndex = 7
        ' 
        ' chkNonLues
        ' 
        chkNonLues.AutoSize = True
        chkNonLues.Location = New Point(695, 43)
        chkNonLues.Margin = New Padding(3, 4, 3, 4)
        chkNonLues.Name = "chkNonLues"
        chkNonLues.Size = New Size(172, 24)
        chkNonLues.TabIndex = 8
        chkNonLues.Text = "Non lues uniquement"
        chkNonLues.UseVisualStyleBackColor = True
        ' 
        ' btnFiltrer
        ' 
        btnFiltrer.BackColor = Color.ForestGreen
        btnFiltrer.FlatAppearance.BorderColor = Color.FromArgb(CByte(46), CByte(107), CByte(74))
        btnFiltrer.FlatStyle = FlatStyle.Flat
        btnFiltrer.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        btnFiltrer.ForeColor = Color.White
        btnFiltrer.Location = New Point(903, 37)
        btnFiltrer.Margin = New Padding(3, 4, 3, 4)
        btnFiltrer.Name = "btnFiltrer"
        btnFiltrer.Size = New Size(103, 37)
        btnFiltrer.TabIndex = 9
        btnFiltrer.Text = "Filtrer"
        btnFiltrer.UseVisualStyleBackColor = False
        ' 
        ' btnMarquerLue
        ' 
        btnMarquerLue.BackColor = Color.White
        btnMarquerLue.FlatAppearance.BorderColor = Color.ForestGreen
        btnMarquerLue.FlatStyle = FlatStyle.Flat
        btnMarquerLue.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        btnMarquerLue.ForeColor = Color.ForestGreen
        btnMarquerLue.Location = New Point(1017, 37)
        btnMarquerLue.Margin = New Padding(3, 4, 3, 4)
        btnMarquerLue.Name = "btnMarquerLue"
        btnMarquerLue.Size = New Size(206, 37)
        btnMarquerLue.TabIndex = 10
        btnMarquerLue.Text = "Marquer comme lue"
        btnMarquerLue.UseVisualStyleBackColor = False
        ' 
        ' pnlDetail
        ' 
        pnlDetail.BackColor = Color.White
        pnlDetail.Controls.Add(lblDetailTitre)
        pnlDetail.Controls.Add(lblDetail)
        pnlDetail.Dock = DockStyle.Bottom
        pnlDetail.Location = New Point(0, 632)
        pnlDetail.Margin = New Padding(3, 4, 3, 4)
        pnlDetail.Name = "pnlDetail"
        pnlDetail.Size = New Size(1257, 115)
        pnlDetail.TabIndex = 3
        ' 
        ' lblDetailTitre
        ' 
        lblDetailTitre.AutoSize = True
        lblDetailTitre.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblDetailTitre.Location = New Point(21, 13)
        lblDetailTitre.Name = "lblDetailTitre"
        lblDetailTitre.Size = New Size(214, 20)
        lblDetailTitre.TabIndex = 0
        lblDetailTitre.Text = "Détail de l'alerte sélectionnée"
        ' 
        ' lblDetail
        ' 
        lblDetail.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblDetail.ForeColor = Color.FromArgb(CByte(51), CByte(71), CByte(61))
        lblDetail.Location = New Point(21, 45)
        lblDetail.Name = "lblDetail"
        lblDetail.Size = New Size(1216, 56)
        lblDetail.TabIndex = 1
        ' 
        ' pnlBas
        ' 
        pnlBas.BackColor = Color.FromArgb(CByte(227), CByte(237), CByte(230))
        pnlBas.Controls.Add(lblCompteur)
        pnlBas.Dock = DockStyle.Bottom
        pnlBas.Location = New Point(0, 747)
        pnlBas.Margin = New Padding(3, 4, 3, 4)
        pnlBas.Name = "pnlBas"
        pnlBas.Size = New Size(1257, 40)
        pnlBas.TabIndex = 4
        ' 
        ' lblCompteur
        ' 
        lblCompteur.AutoSize = True
        lblCompteur.Location = New Point(21, 9)
        lblCompteur.Name = "lblCompteur"
        lblCompteur.Size = New Size(0, 20)
        lblCompteur.TabIndex = 0
        ' 
        ' dgvAlertes
        ' 
        dgvAlertes.AllowUserToAddRows = False
        dgvAlertes.AllowUserToDeleteRows = False
        dgvAlertes.AllowUserToResizeRows = False
        dgvAlertes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvAlertes.BackgroundColor = Color.White
        dgvAlertes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        dgvAlertes.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        dgvAlertes.ColumnHeadersHeight = 29
        dgvAlertes.Dock = DockStyle.Fill
        dgvAlertes.EnableHeadersVisualStyles = False
        dgvAlertes.GridColor = Color.FromArgb(CByte(195), CByte(208), CByte(200))
        dgvAlertes.Location = New Point(0, 158)
        dgvAlertes.Margin = New Padding(3, 4, 3, 4)
        dgvAlertes.MultiSelect = False
        dgvAlertes.Name = "dgvAlertes"
        dgvAlertes.ReadOnly = True
        dgvAlertes.RowHeadersVisible = False
        dgvAlertes.RowHeadersWidth = 51
        dgvAlertes.RowTemplate.Height = 28
        dgvAlertes.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAlertes.Size = New Size(1257, 474)
        dgvAlertes.TabIndex = 2
        ' 
        ' frmAlertes
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(244), CByte(247), CByte(245))
        ClientSize = New Size(1257, 787)
        Controls.Add(dgvAlertes)
        Controls.Add(pnlDetail)
        Controls.Add(pnlBas)
        Controls.Add(pnlFiltres)
        Controls.Add(pnlTitre)
        Margin = New Padding(3, 4, 3, 4)
        MinimumSize = New Size(1273, 544)
        Name = "frmAlertes"
        StartPosition = FormStartPosition.CenterParent
        Text = "Alertes de prix"
        pnlTitre.ResumeLayout(False)
        pnlTitre.PerformLayout()
        pnlFiltres.ResumeLayout(False)
        pnlFiltres.PerformLayout()
        pnlDetail.ResumeLayout(False)
        pnlDetail.PerformLayout()
        pnlBas.ResumeLayout(False)
        pnlBas.PerformLayout()
        CType(dgvAlertes, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
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
