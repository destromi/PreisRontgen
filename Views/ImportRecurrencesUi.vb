Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms

Public NotInheritable Partial Class MainForm
    Private ReadOnly ongletsImportDepenses As New TabControl()
    Private ReadOnly lblResultatImport As New Label()
    Private ReadOnly cmbProduitExport As New ComboBox()
    Private ReadOnly cmbMagasinExport As New ComboBox()
    Private ReadOnly dgvDepensesRecurrentes As New DataGridView()
    Private ReadOnly txtLibelleRecurrent As New TextBox()
    Private ReadOnly txtMontantRecurrent As New TextBox()
    Private ReadOnly cmbPeriodiciteRecurrente As New ComboBox()
    Private ReadOnly dtpEcheanceRecurrente As New DateTimePicker()
    Private _depenseChoisieId As Long?

    Private Sub ConstruireOngletImportEtDepenses()
        Dim page As New TabPage("CSV et dépenses") With {.BackColor = ThemePricieux.FondPage, .Padding = New Padding(16), .AllowDrop = True}
        _onglets.TabPages.Add(page)
        page.Controls.Add(New Label With {.Text = "Import/export et dépenses récurrentes", .Font = New Font("Segoe UI", 17, FontStyle.Bold), .AutoSize = True, .Location = New Point(18, 16), .ForeColor = ThemePricieux.Foret})
        ongletsImportDepenses.Dock = DockStyle.Fill
        ongletsImportDepenses.Location = New Point(18, 60)
        ongletsImportDepenses.Size = New Size(1195, 628)
        ongletsImportDepenses.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        ongletsImportDepenses.DrawMode = TabDrawMode.OwnerDrawFixed
        ongletsImportDepenses.ItemSize = New Size(170, 28)
        AddHandler ongletsImportDepenses.DrawItem, AddressOf ThemePricieux.DessinerOnglet
        page.Controls.Add(ongletsImportDepenses)
        ConstruireOngletCsv()
        ConstruireOngletDepensesRecurrentes()
        AddHandler page.DragEnter, AddressOf FenetreDragEnter
        AddHandler page.DragDrop, AddressOf FenetreDragDrop
    End Sub

    Private Sub ConstruireOngletCsv()
        Dim page As New TabPage("Import et export CSV")
        ongletsImportDepenses.TabPages.Add(page)
        page.Controls.Add(New Label With {.Text = "Importez des relevés de prix au format CSV. Les tickets sont regroupés par date et magasin.", .Location = New Point(24, 24), .Size = New Size(980, 42), .Font = New Font("Segoe UI", 10)})
        Dim btnImporter As Button = CreerBouton("Importer un CSV", True)
        btnImporter.Location = New Point(28, 90)
        AddHandler btnImporter.Click, AddressOf ChoisirCsvAImporter
        page.Controls.Add(btnImporter)
        lblResultatImport.Name = "lblResultatImport"
        lblResultatImport.Location = New Point(28, 146)
        lblResultatImport.Size = New Size(1030, 92)
        lblResultatImport.ForeColor = ThemePricieux.TexteSecondaire
        page.Controls.Add(lblResultatImport)
        page.Controls.Add(New Label With {.Text = "Exporter l'historique d'un produit", .Location = New Point(28, 271), .AutoSize = True, .Font = New Font("Segoe UI", 11, FontStyle.Bold)})
        cmbProduitExport.Name = "cmbProduitExport"
        cmbProduitExport.DropDownStyle = ComboBoxStyle.DropDownList
        cmbProduitExport.Location = New Point(28, 310)
        cmbProduitExport.Size = New Size(350, 30)
        cmbProduitExport.FormattingEnabled = True
        AddHandler cmbProduitExport.Format, AddressOf FormaterProduit
        page.Controls.Add(cmbProduitExport)
        cmbMagasinExport.Name = "cmbMagasinExport"
        cmbMagasinExport.DropDownStyle = ComboBoxStyle.DropDownList
        cmbMagasinExport.Location = New Point(400, 310)
        cmbMagasinExport.Size = New Size(240, 30)
        page.Controls.Add(cmbMagasinExport)
        Dim btnExporter As Button = CreerBouton("Exporter en CSV", False)
        btnExporter.Location = New Point(665, 306)
        AddHandler btnExporter.Click, AddressOf ChoisirCsvAExporter
        page.Controls.Add(btnExporter)
        Dim lblFormat As New Label With {.Text = "En-tête d'import : date;magasin;produit;marque;categorie;unite_base;conditionnement;unite;prix;nombre. Le glisser-déposer d'un fichier CSV sur cet onglet lance son import.", .Location = New Point(28, 385), .Size = New Size(1080, 74), .ForeColor = Color.DimGray}
        page.Controls.Add(lblFormat)
    End Sub

    Private Sub ConstruireOngletDepensesRecurrentes()
        Dim page As New TabPage("Dépenses récurrentes")
        ongletsImportDepenses.TabPages.Add(page)
        ConfigurerGrille(dgvDepensesRecurrentes)
        dgvDepensesRecurrentes.Name = "dgvDepensesRecurrentes"
        dgvDepensesRecurrentes.Location = New Point(18, 18)
        dgvDepensesRecurrentes.Size = New Size(700, 520)
        dgvDepensesRecurrentes.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        page.Controls.Add(dgvDepensesRecurrentes)
        dgvDepensesRecurrentes.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "id", .HeaderText = "Id", .Visible = False})
        dgvDepensesRecurrentes.Columns.Add("libelle", "Libellé")
        dgvDepensesRecurrentes.Columns.Add("montant", "Montant")
        dgvDepensesRecurrentes.Columns.Add("periodicite", "Périodicité")
        dgvDepensesRecurrentes.Columns.Add("echeance", "Prochaine échéance")
        dgvDepensesRecurrentes.Columns("libelle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvDepensesRecurrentes.Columns("montant").Width = 130
        dgvDepensesRecurrentes.Columns("periodicite").Width = 140
        dgvDepensesRecurrentes.Columns("echeance").Width = 150
        AddHandler dgvDepensesRecurrentes.SelectionChanged, AddressOf DepenseRecurrenteSelectionnee
        Dim panneau As New Panel With {.Location = New Point(742, 18), .Size = New Size(390, 520), .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right}
        page.Controls.Add(panneau)
        AjouterControle(panneau, "Nom de la dépense", txtLibelleRecurrent, 10, 12, 350)
        AjouterControle(panneau, "Montant (FCFA)", txtMontantRecurrent, 10, 83, 350)
        panneau.Controls.Add(New Label With {.Text = "Périodicité", .Location = New Point(10, 154), .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold)})
        cmbPeriodiciteRecurrente.Name = "cmbPeriodiciteRecurrente"
        cmbPeriodiciteRecurrente.DropDownStyle = ComboBoxStyle.DropDownList
        cmbPeriodiciteRecurrente.Items.AddRange(New Object() {"HEBDOMADAIRE", "MENSUELLE", "TRIMESTRIELLE", "ANNUELLE"})
        cmbPeriodiciteRecurrente.SelectedIndex = 1
        cmbPeriodiciteRecurrente.Location = New Point(10, 181)
        cmbPeriodiciteRecurrente.Size = New Size(240, 30)
        panneau.Controls.Add(cmbPeriodiciteRecurrente)
        panneau.Controls.Add(New Label With {.Text = "Prochaine échéance", .Location = New Point(10, 231), .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold)})
        dtpEcheanceRecurrente.Name = "dtpEcheanceRecurrente"
        dtpEcheanceRecurrente.Format = DateTimePickerFormat.Short
        dtpEcheanceRecurrente.Location = New Point(10, 258)
        dtpEcheanceRecurrente.Size = New Size(160, 30)
        panneau.Controls.Add(dtpEcheanceRecurrente)
        Dim btnEnregistrer As Button = CreerBouton("Enregistrer", True)
        btnEnregistrer.Location = New Point(10, 320)
        AddHandler btnEnregistrer.Click, AddressOf EnregistrerDepenseRecurrenteAsync
        panneau.Controls.Add(btnEnregistrer)
        Dim btnNouvelle As Button = CreerBouton("Nouveau", False)
        btnNouvelle.Location = New Point(155, 320)
        AddHandler btnNouvelle.Click, Sub(sender As Object, args As EventArgs) ViderDepenseRecurrente()
        panneau.Controls.Add(btnNouvelle)
        Dim btnSupprimer As Button = CreerBouton("Désactiver", False)
        btnSupprimer.Location = New Point(10, 370)
        AddHandler btnSupprimer.Click, AddressOf SupprimerDepenseRecurrenteAsync
        panneau.Controls.Add(btnSupprimer)
        panneau.Controls.Add(New Label With {.Text = "Un rappel s'affiche avant l'échéance, sans déclencher de paiement automatique.", .Location = New Point(10, 425), .Size = New Size(340, 58), .ForeColor = Color.DimGray})
    End Sub

    Private Async Sub ChoisirCsvAImporter(sender As Object, e As EventArgs)
        Using dialogue As New OpenFileDialog With {.Title = "Importer des relevés CSV", .Filter = "Fichiers CSV|*.csv|Tous les fichiers|*.*"}
            If dialogue.ShowDialog() = DialogResult.OK Then Await ImporterFichierAsync(dialogue.FileName)
        End Using
    End Sub

    Private Async Function ImporterFichierAsync(chemin As String) As System.Threading.Tasks.Task
        Try
            lblResultatImport.Text = "Import en cours…"
            Dim resultat = Await _importExport.ImporterAsync(chemin, _utilisateur)
            lblResultatImport.Text = resultat.LignesImportees.ToString(CultureInfo.CurrentCulture) & " ligne(s) importée(s). " & resultat.Erreurs.Count.ToString(CultureInfo.CurrentCulture) & " erreur(s)."
            If resultat.Erreurs.Count > 0 Then
                Dim details As String = String.Join(Environment.NewLine, resultat.Erreurs.Take(12))
                If resultat.Erreurs.Count > 12 Then details &= Environment.NewLine & "… et " & (resultat.Erreurs.Count - 12).ToString(CultureInfo.CurrentCulture) & " autre(s)."
                MessageBox.Show(details, "Lignes rejetées", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
            Await RafraichirAccueilAsyncTask()
            Await ChargerHistoriqueDepuisImportAsync()
        Catch ex As Exception
            AfficherErreur("Import CSV impossible", ex)
        End Try
    End Function

    Private Async Function ChargerHistoriqueDepuisImportAsync() As System.Threading.Tasks.Task
        If cmbProduitHistorique.SelectedItem Is Nothing Then Return
        Dim produit As Produit = DirectCast(cmbProduitHistorique.SelectedItem, Produit)
        Dim magasin As Magasin = TryCast(cmbMagasinHistorique.SelectedItem, Magasin)
        _observationsAffichees = Await _ticketDepot.ListerHistoriqueAsync(_utilisateur.Id, produit.Id, If(magasin Is Nothing, CType(Nothing, Long?), magasin.Id))
        pnlGraphique.Invalidate()
    End Function

    Private Async Sub ChoisirCsvAExporter(sender As Object, e As EventArgs)
        Try
            Dim produit As Produit = TryCast(cmbProduitExport.SelectedItem, Produit)
            If produit Is Nothing Then Throw New InvalidOperationException("Sélectionnez un produit.")
            Dim magasin As Magasin = TryCast(cmbMagasinExport.SelectedItem, Magasin)
            Dim magasinId As Long? = If(magasin Is Nothing, CType(Nothing, Long?), magasin.Id)
            Using dialogue As New SaveFileDialog With {.Title = "Exporter l'historique", .Filter = "Fichier CSV|*.csv", .FileName = "historique-prix.csv"}
                If dialogue.ShowDialog() <> DialogResult.OK Then Return
                Await _importExport.ExporterHistoriqueAsync(dialogue.FileName, _utilisateur.Id, produit.Id, magasinId)
                MessageBox.Show("Historique exporté.", "Pricieux", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using
        Catch ex As Exception
            AfficherErreur("Export CSV impossible", ex)
        End Try
    End Sub

    Private Async Sub ChargerDepensesRecurrentesAsync(sender As Object, e As EventArgs)
        Try
            Dim depenses = Await _depensesDepot.ListerAsync(_utilisateur.Id)
            dgvDepensesRecurrentes.Rows.Clear()
            For Each depense As DepenseRecurrente In depenses
                dgvDepensesRecurrentes.Rows.Add(depense.Id, depense.Libelle, depense.Montant.ToString("N2"), depense.Periodicite, depense.ProchaineEcheance.ToString("dd/MM/yyyy"))
            Next
        Catch ex As Exception
            AfficherErreur("Liste des dépenses indisponible", ex)
        End Try
    End Sub

    Private Sub DepenseRecurrenteSelectionnee(sender As Object, e As EventArgs)
        If dgvDepensesRecurrentes.CurrentRow Is Nothing Then Return
        Dim idValeur As Object = dgvDepensesRecurrentes.CurrentRow.Cells("id").Value
        If idValeur Is Nothing Then Return
        _depenseChoisieId = Convert.ToInt64(idValeur)
        txtLibelleRecurrent.Text = Convert.ToString(dgvDepensesRecurrentes.CurrentRow.Cells("libelle").Value)
        txtMontantRecurrent.Text = Convert.ToString(dgvDepensesRecurrentes.CurrentRow.Cells("montant").Value).Replace(" ", String.Empty)
        cmbPeriodiciteRecurrente.SelectedItem = Convert.ToString(dgvDepensesRecurrentes.CurrentRow.Cells("periodicite").Value)
        Dim echeance As Date
        If Date.TryParse(Convert.ToString(dgvDepensesRecurrentes.CurrentRow.Cells("echeance").Value), echeance) Then dtpEcheanceRecurrente.Value = echeance
    End Sub

    Private Async Sub EnregistrerDepenseRecurrenteAsync(sender As Object, e As EventArgs)
        Try
            Dim id As Long = If(_depenseChoisieId.HasValue, _depenseChoisieId.Value, 0L)
            Dim montant As Decimal = LireDecimal(txtMontantRecurrent.Text, "montant")
            Dim periodicite As String = Convert.ToString(cmbPeriodiciteRecurrente.SelectedItem, CultureInfo.CurrentCulture)
            Await _depensesDepot.EnregistrerAsync(_utilisateur.Id, New DepenseRecurrente With {.Id = id, .Libelle = txtLibelleRecurrent.Text, .Montant = montant, .Periodicite = periodicite, .ProchaineEcheance = dtpEcheanceRecurrente.Value.Date})
            ViderDepenseRecurrente()
            Await ChargerDepensesRecurrentesAsyncTask()
            Await VerifierRappelsAsyncTask()
        Catch ex As Exception
            AfficherErreur("Enregistrement de la dépense impossible", ex)
        End Try
    End Sub

    Private Async Sub SupprimerDepenseRecurrenteAsync(sender As Object, e As EventArgs)
        Try
            If dgvDepensesRecurrentes.CurrentRow Is Nothing Then Throw New InvalidOperationException("Sélectionnez une dépense.")
            Dim id As Long = Convert.ToInt64(dgvDepensesRecurrentes.CurrentRow.Cells("id").Value)
            Await _depensesDepot.SupprimerAsync(_utilisateur.Id, id)
            ViderDepenseRecurrente()
            Await ChargerDepensesRecurrentesAsyncTask()
        Catch ex As Exception
            AfficherErreur("Désactivation impossible", ex)
        End Try
    End Sub

    Private Sub ViderDepenseRecurrente()
        dgvDepensesRecurrentes.ClearSelection()
        dgvDepensesRecurrentes.CurrentCell = Nothing
        _depenseChoisieId = Nothing
        txtLibelleRecurrent.Clear()
        txtMontantRecurrent.Clear()
        cmbPeriodiciteRecurrente.SelectedIndex = 1
        dtpEcheanceRecurrente.Value = Date.Today
    End Sub

    Private Async Function ChargerDepensesRecurrentesAsyncTask() As System.Threading.Tasks.Task
        Dim depenses = Await _depensesDepot.ListerAsync(_utilisateur.Id)
        _depenseChoisieId = Nothing
        dgvDepensesRecurrentes.Rows.Clear()
        For Each depense As DepenseRecurrente In depenses
            dgvDepensesRecurrentes.Rows.Add(depense.Id, depense.Libelle, depense.Montant.ToString("N2"), depense.Periodicite, depense.ProchaineEcheance.ToString("dd/MM/yyyy"))
        Next
    End Function

    Private Async Function VerifierRappelsAsyncTask() As System.Threading.Tasks.Task
        Await VerifierRappelsAsync()
    End Function

    Private Sub ActualiserEcheancesDansGrilleDepenses(depenses As IEnumerable(Of DepenseRecurrente))
        For Each depense As DepenseRecurrente In depenses
            For Each ligne As DataGridViewRow In dgvDepensesRecurrentes.Rows
                If ligne.IsNewRow OrElse ligne.Cells("id").Value Is Nothing Then Continue For
                Dim idLigne As Long
                If Not Long.TryParse(Convert.ToString(ligne.Cells("id").Value, CultureInfo.InvariantCulture), idLigne) OrElse idLigne <> depense.Id Then Continue For

                Dim ancienneEcheance As Date
                Dim ancienneEcheanceValide As Boolean = Date.TryParse(Convert.ToString(ligne.Cells("echeance").Value, CultureInfo.CurrentCulture), ancienneEcheance)
                Dim formulaireInchange As Boolean = _depenseChoisieId.HasValue AndAlso _depenseChoisieId.Value = depense.Id AndAlso ancienneEcheanceValide AndAlso dtpEcheanceRecurrente.Value.Date = ancienneEcheance.Date
                ligne.Cells("echeance").Value = depense.ProchaineEcheance.ToString("dd/MM/yyyy")
                If formulaireInchange Then dtpEcheanceRecurrente.Value = depense.ProchaineEcheance.Date
                Exit For
            Next
        Next
    End Sub
End Class
