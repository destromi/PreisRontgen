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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        pnlTitre = New Panel()
        pnlRappel = New Panel()
        lblRappel = New Label()
        pnlForm = New Panel()
        lblFormTitre = New Label()
        lblNom = New Label()
        txtNom = New TextBox()
        lblMontant = New Label()
        txtMontant = New TextBox()
        lblPeriodicite = New Label()
        cmbPeriodicite = New ComboBox()
        lblEcheance = New Label()
        dtpEcheance = New DateTimePicker()
        btnEnregistrer = New Button()
        btnNouveau = New Button()
        btnEcheanceSuivante = New Button()
        btnDesactiver = New Button()
        lblNote = New Label()
        dgvDepenses = New DataGridView()
        pnlRappel.SuspendLayout()
        pnlForm.SuspendLayout()
        CType(dgvDepenses, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' pnlTitre
        ' 
        pnlTitre.BackColor = Color.FromArgb(CByte(244), CByte(247), CByte(245))
        pnlTitre.Dock = DockStyle.Top
        pnlTitre.Location = New Point(0, 0)
        pnlTitre.Margin = New Padding(3, 4, 3, 4)
        pnlTitre.Name = "pnlTitre"
        pnlTitre.Size = New Size(1415, 59)
        pnlTitre.TabIndex = 0
        ' 
        ' pnlRappel
        ' 
        pnlRappel.BackColor = Color.FromArgb(CByte(253), CByte(243), CByte(214))
        pnlRappel.Controls.Add(lblRappel)
        pnlRappel.Dock = DockStyle.Bottom
        pnlRappel.Location = New Point(0, 728)
        pnlRappel.Margin = New Padding(3, 4, 3, 4)
        pnlRappel.Name = "pnlRappel"
        pnlRappel.Size = New Size(1415, 59)
        pnlRappel.TabIndex = 1
        ' 
        ' lblRappel
        ' 
        lblRappel.Dock = DockStyle.Fill
        lblRappel.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblRappel.ForeColor = Color.FromArgb(CByte(122), CByte(51), CByte(0))
        lblRappel.Location = New Point(0, 0)
        lblRappel.Name = "lblRappel"
        lblRappel.Padding = New Padding(16, 0, 0, 0)
        lblRappel.Size = New Size(1415, 59)
        lblRappel.TabIndex = 0
        lblRappel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' pnlForm
        ' 
        pnlForm.BackColor = Color.White
        pnlForm.Controls.Add(lblFormTitre)
        pnlForm.Controls.Add(lblNom)
        pnlForm.Controls.Add(txtNom)
        pnlForm.Controls.Add(lblMontant)
        pnlForm.Controls.Add(txtMontant)
        pnlForm.Controls.Add(lblPeriodicite)
        pnlForm.Controls.Add(cmbPeriodicite)
        pnlForm.Controls.Add(lblEcheance)
        pnlForm.Controls.Add(dtpEcheance)
        pnlForm.Controls.Add(btnEnregistrer)
        pnlForm.Controls.Add(btnNouveau)
        pnlForm.Controls.Add(btnEcheanceSuivante)
        pnlForm.Controls.Add(btnDesactiver)
        pnlForm.Controls.Add(lblNote)
        pnlForm.Dock = DockStyle.Right
        pnlForm.Location = New Point(919, 59)
        pnlForm.Margin = New Padding(3, 4, 3, 4)
        pnlForm.Name = "pnlForm"
        pnlForm.Size = New Size(496, 669)
        pnlForm.TabIndex = 2
        ' 
        ' lblFormTitre
        ' 
        lblFormTitre.AutoSize = True
        lblFormTitre.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        lblFormTitre.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblFormTitre.Location = New Point(18, 19)
        lblFormTitre.Name = "lblFormTitre"
        lblFormTitre.Size = New Size(93, 28)
        lblFormTitre.TabIndex = 0
        lblFormTitre.Text = "Dépense"
        ' 
        ' lblNom
        ' 
        lblNom.AutoSize = True
        lblNom.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        lblNom.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblNom.Location = New Point(18, 64)
        lblNom.Name = "lblNom"
        lblNom.Size = New Size(49, 23)
        lblNom.TabIndex = 1
        lblNom.Text = "Nom"
        ' 
        ' txtNom
        ' 
        txtNom.Location = New Point(18, 91)
        txtNom.Margin = New Padding(3, 4, 3, 4)
        txtNom.MaxLength = 180
        txtNom.Name = "txtNom"
        txtNom.Size = New Size(436, 27)
        txtNom.TabIndex = 2
        ' 
        ' lblMontant
        ' 
        lblMontant.AutoSize = True
        lblMontant.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        lblMontant.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblMontant.Location = New Point(18, 136)
        lblMontant.Name = "lblMontant"
        lblMontant.Size = New Size(136, 23)
        lblMontant.TabIndex = 3
        lblMontant.Text = "Montant (FCFA)"
        ' 
        ' txtMontant
        ' 
        txtMontant.Location = New Point(18, 163)
        txtMontant.Margin = New Padding(3, 4, 3, 4)
        txtMontant.Name = "txtMontant"
        txtMontant.Size = New Size(436, 27)
        txtMontant.TabIndex = 4
        ' 
        ' lblPeriodicite
        ' 
        lblPeriodicite.AutoSize = True
        lblPeriodicite.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        lblPeriodicite.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblPeriodicite.Location = New Point(18, 208)
        lblPeriodicite.Name = "lblPeriodicite"
        lblPeriodicite.Size = New Size(95, 23)
        lblPeriodicite.TabIndex = 5
        lblPeriodicite.Text = "Périodicité"
        ' 
        ' cmbPeriodicite
        ' 
        cmbPeriodicite.DropDownStyle = ComboBoxStyle.DropDownList
        cmbPeriodicite.FormattingEnabled = True
        cmbPeriodicite.Location = New Point(18, 235)
        cmbPeriodicite.Margin = New Padding(3, 4, 3, 4)
        cmbPeriodicite.Name = "cmbPeriodicite"
        cmbPeriodicite.Size = New Size(436, 28)
        cmbPeriodicite.TabIndex = 6
        ' 
        ' lblEcheance
        ' 
        lblEcheance.AutoSize = True
        lblEcheance.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        lblEcheance.ForeColor = Color.FromArgb(CByte(23), CByte(38), CByte(31))
        lblEcheance.Location = New Point(18, 280)
        lblEcheance.Name = "lblEcheance"
        lblEcheance.Size = New Size(165, 23)
        lblEcheance.TabIndex = 7
        lblEcheance.Text = "Prochaine échéance"
        ' 
        ' dtpEcheance
        ' 
        dtpEcheance.Format = DateTimePickerFormat.Short
        dtpEcheance.Location = New Point(18, 307)
        dtpEcheance.Margin = New Padding(3, 4, 3, 4)
        dtpEcheance.Name = "dtpEcheance"
        dtpEcheance.Size = New Size(436, 27)
        dtpEcheance.TabIndex = 8
        ' 
        ' btnEnregistrer
        ' 
        btnEnregistrer.BackColor = Color.ForestGreen
        btnEnregistrer.FlatAppearance.BorderColor = Color.LimeGreen
        btnEnregistrer.FlatStyle = FlatStyle.Flat
        btnEnregistrer.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnEnregistrer.ForeColor = Color.White
        btnEnregistrer.Location = New Point(18, 371)
        btnEnregistrer.Margin = New Padding(3, 4, 3, 4)
        btnEnregistrer.Name = "btnEnregistrer"
        btnEnregistrer.Size = New Size(215, 40)
        btnEnregistrer.TabIndex = 9
        btnEnregistrer.Text = "Enregistrer"
        btnEnregistrer.UseVisualStyleBackColor = False
        ' 
        ' btnNouveau
        ' 
        btnNouveau.BackColor = Color.White
        btnNouveau.FlatAppearance.BorderColor = Color.ForestGreen
        btnNouveau.FlatStyle = FlatStyle.Flat
        btnNouveau.Font = New Font("Segoe UI", 8.5F, FontStyle.Bold)
        btnNouveau.ForeColor = Color.ForestGreen
        btnNouveau.Location = New Point(240, 371)
        btnNouveau.Margin = New Padding(3, 4, 3, 4)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(215, 40)
        btnNouveau.TabIndex = 10
        btnNouveau.Text = "Nouvelle dépense"
        btnNouveau.UseVisualStyleBackColor = False
        ' 
        ' btnEcheanceSuivante
        ' 
        btnEcheanceSuivante.BackColor = Color.White
        btnEcheanceSuivante.FlatAppearance.BorderColor = Color.ForestGreen
        btnEcheanceSuivante.FlatStyle = FlatStyle.Flat
        btnEcheanceSuivante.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnEcheanceSuivante.ForeColor = Color.ForestGreen
        btnEcheanceSuivante.Location = New Point(18, 419)
        btnEcheanceSuivante.Margin = New Padding(3, 4, 3, 4)
        btnEcheanceSuivante.Name = "btnEcheanceSuivante"
        btnEcheanceSuivante.Size = New Size(437, 40)
        btnEcheanceSuivante.TabIndex = 11
        btnEcheanceSuivante.Text = "Échéance suivante"
        btnEcheanceSuivante.UseVisualStyleBackColor = False
        ' 
        ' btnDesactiver
        ' 
        btnDesactiver.BackColor = Color.White
        btnDesactiver.FlatAppearance.BorderColor = Color.ForestGreen
        btnDesactiver.FlatStyle = FlatStyle.Flat
        btnDesactiver.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnDesactiver.ForeColor = Color.ForestGreen
        btnDesactiver.Location = New Point(18, 472)
        btnDesactiver.Margin = New Padding(3, 4, 3, 4)
        btnDesactiver.Name = "btnDesactiver"
        btnDesactiver.Size = New Size(437, 40)
        btnDesactiver.TabIndex = 12
        btnDesactiver.Text = "Désactiver"
        btnDesactiver.UseVisualStyleBackColor = False
        ' 
        ' lblNote
        ' 
        lblNote.Font = New Font("Segoe UI", 8F)
        lblNote.ForeColor = Color.FromArgb(CByte(90), CByte(106), CByte(98))
        lblNote.Location = New Point(18, 536)
        lblNote.Name = "lblNote"
        lblNote.Size = New Size(325, 80)
        lblNote.TabIndex = 13
        lblNote.Text = "Aucun paiement automatique : l'application rappelle seulement l'échéance."
        ' 
        ' dgvDepenses
        ' 
        dgvDepenses.AllowUserToAddRows = False
        dgvDepenses.AllowUserToDeleteRows = False
        dgvDepenses.AllowUserToResizeRows = False
        dgvDepenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvDepenses.BackgroundColor = Color.White
        dgvDepenses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        dgvDepenses.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        dgvDepenses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDepenses.Dock = DockStyle.Fill
        dgvDepenses.EnableHeadersVisualStyles = False
        dgvDepenses.GridColor = Color.FromArgb(CByte(195), CByte(208), CByte(200))
        dgvDepenses.Location = New Point(0, 59)
        dgvDepenses.Margin = New Padding(3, 4, 3, 4)
        dgvDepenses.MultiSelect = False
        dgvDepenses.Name = "dgvDepenses"
        dgvDepenses.ReadOnly = True
        dgvDepenses.RowHeadersVisible = False
        dgvDepenses.RowHeadersWidth = 51
        dgvDepenses.RowTemplate.Height = 28
        dgvDepenses.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvDepenses.Size = New Size(919, 669)
        dgvDepenses.TabIndex = 3
        ' 
        ' frmDepenses
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(244), CByte(247), CByte(245))
        ClientSize = New Size(1415, 787)
        Controls.Add(dgvDepenses)
        Controls.Add(pnlForm)
        Controls.Add(pnlRappel)
        Controls.Add(pnlTitre)
        Margin = New Padding(3, 4, 3, 4)
        MinimumSize = New Size(1273, 678)
        Name = "frmDepenses"
        StartPosition = FormStartPosition.CenterParent
        Text = "Dépenses récurrentes"
        pnlRappel.ResumeLayout(False)
        pnlForm.ResumeLayout(False)
        pnlForm.PerformLayout()
        CType(dgvDepenses, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
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
