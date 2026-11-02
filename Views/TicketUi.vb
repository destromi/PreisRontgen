Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Globalization
Imports System.Linq
Imports System.Windows.Forms

Public NotInheritable Partial Class MainForm
    Private ReadOnly cmbMagasinTicket As New ComboBox()
    Private ReadOnly dtpDateTicket As New DateTimePicker()
    Private ReadOnly cmbProduitTicket As New ComboBox()
    Private ReadOnly txtConditionnement As New TextBox()
    Private ReadOnly cmbUniteTicket As New ComboBox()
    Private ReadOnly txtPrixTicket As New TextBox()
    Private ReadOnly txtNombreTicket As New TextBox()
    Private ReadOnly txtTotalTicket As New TextBox()
    Private ReadOnly dgvLignesTicket As New DataGridView()
    Private ReadOnly btnEnregistrerTicket As Button = CreerBouton("Enregistrer le ticket", True)
    Private ReadOnly btnAjouterLigne As Button = CreerBouton("Ajouter une ligne", False)
    Private ReadOnly btnOcrTicket As Button = CreerBouton("Lire une photo (OCR)", False)
    Private _produitsTicket As New List(Of Produit)()

    Private Sub ConstruireOngletTickets()
        Dim page As New TabPage("Saisie ticket") With {.BackColor = ThemePricieux.FondPage, .Padding = New Padding(18)}
        _onglets.TabPages.Add(page)
        page.Controls.Add(New Label With {.Text = "Nouveau ticket", .Font = New Font("Segoe UI", 17, FontStyle.Bold), .AutoSize = True, .Location = New Point(20, 18), .ForeColor = ThemePricieux.Foret})
        Dim bandeau As New FlowLayoutPanel With {.Location = New Point(22, 67), .Size = New Size(1190, 112), .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right, .BackColor = Color.White, .Padding = New Padding(12), .WrapContents = True}
        page.Controls.Add(bandeau)

        cmbMagasinTicket.Name = "cmbMagasinTicket"
        cmbMagasinTicket.DropDownStyle = ComboBoxStyle.DropDownList
        cmbMagasinTicket.Width = 190
        cmbMagasinTicket.Margin = New Padding(8, 14, 8, 4)
        bandeau.Controls.Add(CreerEtiquette("Magasin", True))
        bandeau.Controls.Add(cmbMagasinTicket)
        dtpDateTicket.Name = "dtpDateTicket"
        dtpDateTicket.Format = DateTimePickerFormat.Short
        dtpDateTicket.Width = 145
        dtpDateTicket.Margin = New Padding(8, 14, 8, 4)
        bandeau.Controls.Add(CreerEtiquette("Date", True))
        bandeau.Controls.Add(dtpDateTicket)
        btnOcrTicket.Name = "btnOcrTicket"
        btnOcrTicket.Margin = New Padding(16, 11, 8, 4)
        AddHandler btnOcrTicket.Click, AddressOf ChargerOcrAsync
        bandeau.Controls.Add(btnOcrTicket)

        Dim panneauSaisie As New FlowLayoutPanel With {.Location = New Point(22, 194), .Size = New Size(1190, 138), .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right, .BackColor = Color.White, .Padding = New Padding(12), .WrapContents = True}
        page.Controls.Add(panneauSaisie)
        cmbProduitTicket.Name = "cmbProduitTicket"
        cmbProduitTicket.DropDownStyle = ComboBoxStyle.DropDown
        cmbProduitTicket.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbProduitTicket.AutoCompleteSource = AutoCompleteSource.CustomSource
        cmbProduitTicket.FormattingEnabled = True
        cmbProduitTicket.Width = 255
        cmbProduitTicket.Margin = New Padding(6, 13, 6, 5)
        AddHandler cmbProduitTicket.Format, AddressOf FormaterProduit
        AddHandler cmbProduitTicket.TextChanged, AddressOf RechercherProduitPendantFrappe
        AddHandler cmbProduitTicket.SelectedIndexChanged, AddressOf ProduitTicketSelectionne
        panneauSaisie.Controls.Add(CreerEtiquette("Produit", True))
        panneauSaisie.Controls.Add(cmbProduitTicket)
        AjouterChamp(panneauSaisie, "Conditionnement", txtConditionnement, 115)
        cmbUniteTicket.Name = "cmbUniteTicket"
        cmbUniteTicket.DropDownStyle = ComboBoxStyle.DropDownList
        cmbUniteTicket.Items.AddRange(New Object() {"g", "kg", "mg", "ml", "cl", "L", "pièce"})
        cmbUniteTicket.Width = 90
        cmbUniteTicket.Margin = New Padding(6, 13, 6, 5)
        panneauSaisie.Controls.Add(CreerEtiquette("Unité", True))
        panneauSaisie.Controls.Add(cmbUniteTicket)
        AjouterChamp(panneauSaisie, "Prix unité", txtPrixTicket, 110)
        AjouterChamp(panneauSaisie, "Nombre acheté", txtNombreTicket, 110)
        btnAjouterLigne.Name = "btnAjouterLigne"
        btnAjouterLigne.Margin = New Padding(8, 10, 6, 4)
        AddHandler btnAjouterLigne.Click, AddressOf AjouterLigneDepuisFormulaire
        panneauSaisie.Controls.Add(btnAjouterLigne)
        AddHandler txtNombreTicket.KeyDown, AddressOf AjouterLigneAvecEntree
        AddHandler txtPrixTicket.KeyDown, AddressOf AjouterLigneAvecEntree

        ConfigurerGrille(dgvLignesTicket)
        dgvLignesTicket.Name = "dgvLignesTicket"
        dgvLignesTicket.ReadOnly = False
        dgvLignesTicket.AllowUserToAddRows = False
        dgvLignesTicket.AllowUserToDeleteRows = True
        dgvLignesTicket.Location = New Point(22, 346)
        dgvLignesTicket.Size = New Size(1190, 330)
        dgvLignesTicket.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        page.Controls.Add(dgvLignesTicket)
        ConstruireColonnesTicket()
        AddHandler dgvLignesTicket.CellValidating, AddressOf ValiderCelluleTicket
        AddHandler dgvLignesTicket.CellEndEdit, AddressOf CelluleTicketModifiee
        AddHandler dgvLignesTicket.RowsRemoved, AddressOf LignesTicketModifiees
        AddHandler dgvLignesTicket.DataError, Sub(sender As Object, args As DataGridViewDataErrorEventArgs) args.ThrowException = False

        Dim lblTotal As New Label With {.Text = "Total du ticket (FCFA)", .AutoSize = True, .Anchor = AnchorStyles.Bottom Or AnchorStyles.Right, .Location = New Point(698, 696), .Font = New Font("Segoe UI", 10, FontStyle.Bold)}
        page.Controls.Add(lblTotal)
        txtTotalTicket.Name = "txtTotalTicket"
        txtTotalTicket.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        txtTotalTicket.Location = New Point(870, 690)
        txtTotalTicket.Size = New Size(150, 30)
        txtTotalTicket.TextAlign = HorizontalAlignment.Right
        page.Controls.Add(txtTotalTicket)
        btnEnregistrerTicket.Name = "btnEnregistrerTicket"
        btnEnregistrerTicket.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnEnregistrerTicket.Location = New Point(1035, 686)
        btnEnregistrerTicket.Size = New Size(177, 38)
        AddHandler btnEnregistrerTicket.Click, AddressOf EnregistrerTicketAsync
        page.Controls.Add(btnEnregistrerTicket)
    End Sub

    Private Shared Sub AjouterChamp(parent As Control, titre As String, champ As TextBox, largeur As Integer)
        parent.Controls.Add(CreerEtiquette(titre, True))
        champ.Name = If(titre = "Conditionnement", "txtConditionnement", If(titre = "Prix unité", "txtPrixTicket", "txtNombreTicket"))
        champ.Width = largeur
        champ.Margin = New Padding(6, 13, 6, 5)
        champ.TextAlign = HorizontalAlignment.Right
        parent.Controls.Add(champ)
    End Sub

    Private Sub ConstruireColonnesTicket()
        Dim colProduit As New DataGridViewComboBoxColumn With {.Name = "produitId", .HeaderText = "Produit", .DisplayMember = "Libelle", .ValueMember = "Id", .DataSource = _produitsTicket, .Width = 260}
        dgvLignesTicket.Columns.Add(colProduit)
        dgvLignesTicket.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "conditionnement", .HeaderText = "Conditionnement", .Width = 135})
        Dim colUnite As New DataGridViewComboBoxColumn With {.Name = "unite", .HeaderText = "Unité", .Width = 100}
        colUnite.Items.AddRange("g", "kg", "mg", "ml", "cl", "L", "pièce")
        dgvLignesTicket.Columns.Add(colUnite)
        dgvLignesTicket.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "prix", .HeaderText = "Prix / paquet", .Width = 140})
        dgvLignesTicket.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "nombre", .HeaderText = "Nombre", .Width = 110})
        dgvLignesTicket.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "total", .HeaderText = "Total ligne", .ReadOnly = True, .Width = 145})
        dgvLignesTicket.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "uniteBase", .HeaderText = "Unité de comparaison", .ReadOnly = True, .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
    End Sub

    Private Sub FormaterProduit(sender As Object, e As ListControlConvertEventArgs)
        Dim produit As Produit = TryCast(e.ListItem, Produit)
        If produit IsNot Nothing Then e.Value = produit.Libelle
    End Sub

    Private Sub RechercherProduitPendantFrappe(sender As Object, e As EventArgs)
        If cmbProduitTicket.Text.Length < 2 Then Return
        cmbProduitTicket.AutoCompleteCustomSource.Clear()
        For Each produit As Produit In _produitsTicket.Where(Function(item) item.Libelle.Contains(cmbProduitTicket.Text, StringComparison.CurrentCultureIgnoreCase)).Take(50)
            cmbProduitTicket.AutoCompleteCustomSource.Add(produit.Libelle)
        Next
    End Sub

    Private Sub ProduitTicketSelectionne(sender As Object, e As EventArgs)
        Dim produit As Produit = TryCast(cmbProduitTicket.SelectedItem, Produit)
        If produit Is Nothing Then Return
        Select Case produit.UniteBase
            Case UniteProduit.Kilogramme
                cmbUniteTicket.SelectedItem = "g"
                txtConditionnement.Text = "500"
            Case UniteProduit.Litre
                cmbUniteTicket.SelectedItem = "ml"
                txtConditionnement.Text = "500"
            Case Else
                cmbUniteTicket.SelectedItem = "pièce"
                txtConditionnement.Text = "1"
        End Select
        txtNombreTicket.Text = "1"
        txtPrixTicket.Focus()
    End Sub

    Private Sub AjouterLigneDepuisFormulaire(sender As Object, e As EventArgs)
        Try
            Dim produit As Produit = TryCast(cmbProduitTicket.SelectedItem, Produit)
            If produit Is Nothing Then Throw New InvalidOperationException("Sélectionnez un produit dans la liste.")
            Dim conditionnement As Decimal = LireDecimal(txtConditionnement.Text, "conditionnement")
            Dim prix As Decimal = LireDecimal(txtPrixTicket.Text, "prix")
            Dim nombre As Decimal = LireDecimal(txtNombreTicket.Text, "nombre acheté")
            If cmbUniteTicket.SelectedItem Is Nothing Then Throw New InvalidOperationException("Sélectionnez une unité.")
            Dim unite As String = Convert.ToString(cmbUniteTicket.SelectedItem, CultureInfo.CurrentCulture)
            Dim ligne As LigneTicket = _ticketService.PreparerLigne(produit, conditionnement, unite, prix, nombre)
            AjouterLigneGrille(produit, ligne.Conditionnement, ligne.Unite, ligne.Prix, ligne.NombreAchete, ligne.Total)
            txtPrixTicket.Clear()
            txtPrixTicket.Focus()
        Catch ex As Exception
            AfficherErreur("Ligne invalide", ex)
        End Try
    End Sub

    Private Sub AjouterLigneGrille(produit As Produit, conditionnement As Decimal, unite As String, prix As Decimal, nombre As Decimal, total As Decimal)
        dgvLignesTicket.Rows.Add(produit.Id, conditionnement.ToString(CultureInfo.CurrentCulture), unite, prix.ToString("0.00", CultureInfo.CurrentCulture), nombre.ToString(CultureInfo.CurrentCulture), total.ToString("0.00", CultureInfo.CurrentCulture), UniteEnTexte(produit.UniteBase))
        ActualiserTotalTicket()
    End Sub

    Private Sub ValiderCelluleTicket(sender As Object, e As DataGridViewCellValidatingEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex = 0 OrElse e.ColumnIndex = 2 OrElse e.ColumnIndex >= 5 Then Return
        Dim valeur As Decimal
        If Not Decimal.TryParse(Convert.ToString(e.FormattedValue, CultureInfo.CurrentCulture), NumberStyles.Number, CultureInfo.CurrentCulture, valeur) AndAlso Not Decimal.TryParse(Convert.ToString(e.FormattedValue, CultureInfo.InvariantCulture), NumberStyles.Number, CultureInfo.InvariantCulture, valeur) Then
            e.Cancel = True
            MessageBox.Show("Entrez une valeur numérique valide.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If (e.ColumnIndex = 1 OrElse e.ColumnIndex = 4) AndAlso valeur <= 0D Then
            e.Cancel = True
            MessageBox.Show("La quantité doit être supérieure à zéro.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf e.ColumnIndex = 3 AndAlso valeur < 0D Then
            e.Cancel = True
            MessageBox.Show("Le prix ne peut pas être négatif.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub CelluleTicketModifiee(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        Dim row As DataGridViewRow = dgvLignesTicket.Rows(e.RowIndex)
        Try
            Dim produitId As Long = Convert.ToInt64(row.Cells("produitId").Value)
            Dim produit As Produit = _produitsTicket.Find(Function(item) item.Id = produitId)
            If produit Is Nothing Then Return
            Dim ligne As LigneTicket = _ticketService.PreparerLigne(produit,
                LireDecimal(Convert.ToString(row.Cells("conditionnement").Value), "conditionnement"),
                Convert.ToString(row.Cells("unite").Value),
                LireDecimal(Convert.ToString(row.Cells("prix").Value), "prix"),
                LireDecimal(Convert.ToString(row.Cells("nombre").Value), "nombre"))
            row.Cells("total").Value = ligne.Total.ToString("0.00", CultureInfo.CurrentCulture)
            row.Cells("uniteBase").Value = UniteEnTexte(produit.UniteBase)
            row.ErrorText = String.Empty
            ActualiserTotalTicket()
        Catch
            row.ErrorText = "Vérifiez les valeurs de cette ligne."
        End Try
    End Sub

    Private Sub LignesTicketModifiees(sender As Object, e As DataGridViewRowsRemovedEventArgs)
        ActualiserTotalTicket()
    End Sub

    Private Sub ActualiserTotalTicket()
        Dim total As Decimal = 0D
        For Each row As DataGridViewRow In dgvLignesTicket.Rows
            Dim valeur As Decimal
            If Decimal.TryParse(Convert.ToString(row.Cells("total").Value), NumberStyles.Number, CultureInfo.CurrentCulture, valeur) Then total += valeur
        Next
        txtTotalTicket.Text = Decimal.Round(total, 2).ToString("0.00", CultureInfo.CurrentCulture)
    End Sub

    Private Async Sub EnregistrerTicketAsync(sender As Object, e As EventArgs)
        Try
            If cmbMagasinTicket.SelectedItem Is Nothing Then Throw New InvalidOperationException("Sélectionnez un magasin.")
            dgvLignesTicket.EndEdit()
            Dim lignes As New List(Of LigneTicket)()
            For Each row As DataGridViewRow In dgvLignesTicket.Rows
                Dim produitId As Long = Convert.ToInt64(row.Cells("produitId").Value)
                Dim produit As Produit = _produitsTicket.Find(Function(item) item.Id = produitId)
                If produit Is Nothing Then Throw New InvalidOperationException("Sélectionnez un produit valide pour chaque ligne.")
                lignes.Add(_ticketService.PreparerLigne(produit,
                    LireDecimal(Convert.ToString(row.Cells("conditionnement").Value), "conditionnement"),
                    Convert.ToString(row.Cells("unite").Value),
                    LireDecimal(Convert.ToString(row.Cells("prix").Value), "prix"),
                    LireDecimal(Convert.ToString(row.Cells("nombre").Value), "nombre acheté")))
            Next
            Dim magasin As Magasin = DirectCast(cmbMagasinTicket.SelectedItem, Magasin)
            Dim total As Decimal = LireDecimal(txtTotalTicket.Text, "total du ticket")
            btnEnregistrerTicket.Enabled = False
            Dim alertes As List(Of AlertePrix) = Await _ticketService.EnregistrerAsync(_utilisateur, magasin.Id, dtpDateTicket.Value.Date, total, lignes)
            dgvLignesTicket.Rows.Clear()
            txtTotalTicket.Clear()
            MessageBox.Show("Ticket enregistré." & If(alertes.Count > 0, Environment.NewLine & alertes.Count.ToString(CultureInfo.CurrentCulture) & " alerte(s) détectée(s).", String.Empty), "Pricieux", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Await RafraichirAccueilAsyncTask()
        Catch ex As Exception
            AfficherErreur("Le ticket n'a pas pu être enregistré", ex)
        Finally
            btnEnregistrerTicket.Enabled = True
        End Try
    End Sub

    Private Async Function RafraichirAccueilAsyncTask() As Task
        Dim depenses = Await _analysesDepot.DepensesParCategorieAsync(_utilisateur.Id, Date.Today.Year, Date.Today.Month)
        dgvDepensesCategorie.Rows.Clear()
        Dim total As Decimal = 0D
        For Each depense As DepenseCategorie In depenses
            dgvDepensesCategorie.Rows.Add(depense.CategorieNom, depense.Montant.ToString("N0") & " FCFA")
            total += depense.Montant
        Next
        lblDepenseMois.Text = total.ToString("N0") & " FCFA"
        Dim nonLues As Integer = Await _alertesDepot.CompterNonLuesAsync(_utilisateur.Id)
        lblAlertesNonLues.Text = nonLues.ToString(CultureInfo.CurrentCulture)
        lblAlertes.Text = "Alertes non lues : " & nonLues.ToString(CultureInfo.CurrentCulture)
    End Function

    Private Async Sub ChargerOcrAsync(sender As Object, e As EventArgs)
        If Not _utilisateur.EstAdministrateur AndAlso _produitsTicket.Count = 0 Then Return
        Using dialogue As New OpenFileDialog With {.Title = "Choisir une photo de ticket", .Filter = "Images|*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.bmp|Tous les fichiers|*.*"}
            If dialogue.ShowDialog() <> DialogResult.OK Then Return
            Try
                btnOcrTicket.Enabled = False
                Dim candidats = Await New TesseractOcrService().LireTicketAsync(dialogue.FileName, _produitsTicket)
                If candidats.Count = 0 Then
                    MessageBox.Show("Aucune ligne reconnue. Saisissez ou corrigez les informations manuellement.", "OCR", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If
                For Each candidat As LigneOcr In candidats
                    Dim uniteParDefaut As String = If(candidat.Produit.UniteBase = UniteProduit.Kilogramme, "g", If(candidat.Produit.UniteBase = UniteProduit.Litre, "ml", "pièce"))
                    Dim unite As String = If(String.IsNullOrWhiteSpace(candidat.UniteConditionnement), uniteParDefaut, candidat.UniteConditionnement)
                    Dim tailleParDefaut As Decimal = If(candidat.Produit.UniteBase = UniteProduit.Piece, 1D, 500D)
                    Dim taille As Decimal = If(candidat.QuantiteConditionnement.HasValue, candidat.QuantiteConditionnement.Value, tailleParDefaut)
                    Dim nombre As Decimal = If(candidat.NombreAchete.HasValue, candidat.NombreAchete.Value, 1D)
                    Dim indexLigne As Integer = dgvLignesTicket.Rows.Count
                    AjouterLigneGrille(candidat.Produit, taille, unite, candidat.PrixLu, nombre, candidat.PrixLu * nombre)
                    Dim remarques As New List(Of String)()
                    If candidat.Confiance < 0.8D Then remarques.Add("Produit reconnu approximativement")
                    If Not candidat.QuantiteConditionnement.HasValue Then remarques.Add("conditionnement non détecté")
                    If Not candidat.NombreAchete.HasValue Then remarques.Add("nombre acheté non détecté")
                    If remarques.Count > 0 Then dgvLignesTicket.Rows(indexLigne).ErrorText = String.Join(" ; ", remarques) & ". Vérifiez cette ligne."
                Next
                MessageBox.Show("Les lignes reconnues sont préremplies. Les montants et quantités détectés sont indiqués dans le tableau. Vérifiez chaque ligne avant l'enregistrement ; les icônes d'erreur signalent les champs que l'OCR n'a pas pu confirmer.", "Vérification manuelle requise", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                AfficherErreur("Lecture OCR impossible", ex)
            Finally
                btnOcrTicket.Enabled = True
            End Try
        End Using
    End Sub

    Private Sub AjouterLigneAvecEntree(sender As Object, e As KeyEventArgs)
        If e.KeyCode <> Keys.Enter Then Return
        e.SuppressKeyPress = True
        AjouterLigneDepuisFormulaire(sender, EventArgs.Empty)
    End Sub

    Private Shared Function LireDecimal(texte As String, nomChamp As String) As Decimal
        Dim valeur As Decimal
        If Decimal.TryParse(If(texte, String.Empty).Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, valeur) OrElse Decimal.TryParse(If(texte, String.Empty).Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, valeur) Then Return valeur
        Throw New InvalidOperationException("Saisissez une valeur numérique valide pour « " & nomChamp & " ».")
    End Function

    Private Shared Function UniteEnTexte(unite As UniteProduit) As String
        Select Case unite
            Case UniteProduit.Kilogramme
                Return "kg"
            Case UniteProduit.Litre
                Return "L"
            Case Else
                Return "pièce"
        End Select
    End Function
End Class
