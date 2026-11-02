Option Strict On
Option Explicit On
Imports System.ComponentModel
Imports System.Data
Imports System.Linq
Imports MySql.Data.MySqlClient

''' Entrée dans « Prix » ajoute la ligne, Suppr retire la ligne sélectionnée, Ctrl+S enregistre.
Public Class FrmTickets
    Private ReadOnly _lignes As New BindingList(Of LigneTicket)()
    Private ReadOnly _ep As New ErrorProvider()
    Private ReadOnly _produitRepo As New ProduitRepository()
    Private ReadOnly _ticketRepo As New TicketRepository()
    Private _uniteBaseCourante As String = ""
    Private _chargement As Boolean = False

    ' ---------- Chargement ----------
    Private Sub FrmTickets_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _chargement = True
        Try
            Me.KeyPreview = True
            _ep.BlinkStyle = ErrorBlinkStyle.NeverBlink

            ' Charger les magasins
            cmbMagasin.DropDownStyle = ComboBoxStyle.DropDownList
            cmbMagasin.DataSource = _ticketRepo.ChargerMagasins()
            cmbMagasin.DisplayMember = "nom"
            cmbMagasin.ValueMember = "id"
            cmbMagasin.SelectedIndex = -1

            ' Charger les produits (saisie semi-automatique pendant la frappe)
            cmbProduit.DropDownStyle = ComboBoxStyle.DropDown
            cmbProduit.DataSource = _produitRepo.ChargerProduits()
            cmbProduit.DisplayMember = "nom"
            cmbProduit.ValueMember = "id"
            cmbProduit.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbProduit.AutoCompleteSource = AutoCompleteSource.ListItems
            cmbProduit.SelectedIndex = -1

            cmbUnite.DropDownStyle = ComboBoxStyle.DropDownList

            nudNombre.Minimum = 1
            nudNombre.Maximum = 999
            nudNombre.Value = 1

            dtpDateAchat.MaxDate = Date.Today
            dtpDateAchat.Value = Date.Today

            ConfigurerGrille()
            dgvLignes.DataSource = _lignes
            MettreAJourSomme()
        Catch ex As Exception
            MessageBox.Show("Erreur lors du chargement des données : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _chargement = False
        End Try
    End Sub

    Private Sub ConfigurerGrille()
        With dgvLignes
            .AutoGenerateColumns = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .RowHeadersVisible = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .Columns.Clear()
            .Columns.Add(Colonne("LibelleProduit", "Produit", True, ""))
            .Columns.Add(Colonne("Nombre", "Nombre", False, "0"))
            .Columns.Add(Colonne("QuantiteConditionnement", "Quantité", False, "0.###"))
            .Columns.Add(Colonne("Unite", "Unité", True, ""))
            .Columns.Add(Colonne("Prix", "Prix", False, "N2"))
            .Columns.Add(Colonne("TotalLigne", "Total ligne", True, "N2"))
            .Columns.Add(Colonne("PrixUnitaire", "Prix unitaire (/kg, /L, /pièce)", True, "N2"))
        End With
    End Sub

    Private Shared Function Colonne(propriete As String, titre As String, lectureSeule As Boolean, format As String) As DataGridViewTextBoxColumn
        Dim c As New DataGridViewTextBoxColumn With {
            .DataPropertyName = propriete,
            .Name = "col" & propriete,
            .HeaderText = titre,
            .ReadOnly = lectureSeule
        }
        If format <> "" Then c.DefaultCellStyle.Format = format
        Return c
    End Function

    ' ---------- Choix du produit : n'afficher que les unités compatibles ----------
    Private Sub cmbProduit_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbProduit.SelectedIndexChanged
        If _chargement Then Return
        cmbUnite.Items.Clear()
        _uniteBaseCourante = ""

        Dim drv As DataRowView = TryCast(cmbProduit.SelectedItem, DataRowView)
        If drv Is Nothing Then Return

        Try
            _uniteBaseCourante = _produitRepo.ObtenirUniteBase(CInt(drv("id")))
        Catch ex As MySqlException
            MessageBox.Show("Erreur base de données : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        cmbUnite.Items.AddRange(UniteService.UnitesPour(_uniteBaseCourante).Cast(Of Object)().ToArray())
        If _uniteBaseCourante <> "" Then cmbUnite.SelectedItem = _uniteBaseCourante
    End Sub

    ' ---------- Ajout d'une ligne ----------
    Private Sub btnAjouterProduit_Click(sender As Object, e As EventArgs) Handles btnAjouterProduit.Click
        _ep.Clear()

        Dim drv As DataRowView = TryCast(cmbProduit.SelectedItem, DataRowView)
        If drv Is Nothing OrElse cmbProduit.Text <> Convert.ToString(drv("nom")) Then
            Signaler(cmbProduit, "Choisissez un produit existant dans la liste.")
            Return
        End If
        If _uniteBaseCourante = "" Then
            Signaler(cmbProduit, "Ce produit n'a pas d'unité de base : complétez sa fiche dans Gestion des produits.")
            Return
        End If

        Dim quantite As Decimal
        If Not Saisie.TryDecimal(txtQuantite.Text, quantite) OrElse quantite <= 0D Then
            Signaler(txtQuantite, "Saisissez une quantité numérique supérieure à 0.")
            Return
        End If

        If cmbUnite.SelectedItem Is Nothing Then
            Signaler(cmbUnite, "Choisissez l'unité.")
            Return
        End If
        Dim unite As String = CStr(cmbUnite.SelectedItem)
        If UniteService.UniteBaseDe(unite) <> _uniteBaseCourante Then
            Signaler(cmbUnite, "Unité incompatible avec l'unité de base du produit (" & _uniteBaseCourante & ").")
            Return
        End If

        Dim prix As Decimal
        If Not Saisie.TryDecimal(txtPrix.Text, prix) OrElse prix < 0D Then
            Signaler(txtPrix, "Saisissez un prix numérique supérieur ou égal à 0.")
            Return
        End If

        _lignes.Add(New LigneTicket With {
            .IdProduit = CInt(drv("id")),
            .LibelleProduit = Convert.ToString(drv("nom")),
            .Nombre = CInt(nudNombre.Value),
            .QuantiteConditionnement = quantite,
            .Unite = unite,
            .Prix = prix
        })

        ' Prêt pour la ligne suivante
        _chargement = True
        cmbProduit.SelectedIndex = -1
        cmbProduit.Text = ""
        cmbUnite.Items.Clear()
        _uniteBaseCourante = ""
        _chargement = False
        nudNombre.Value = 1
        txtQuantite.Clear()
        txtPrix.Clear()
        MettreAJourSomme()
        cmbProduit.Focus()
    End Sub

    Private Sub txtPrix_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPrix.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            btnAjouterProduit.PerformClick()
        End If
    End Sub

    ' ---------- Édition des lignes dans la grille ----------
    Private Sub dgvLignes_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvLignes.CellValidating
        Dim nom As String = dgvLignes.Columns(e.ColumnIndex).DataPropertyName
        If nom <> "Nombre" AndAlso nom <> "QuantiteConditionnement" AndAlso nom <> "Prix" Then Return

        Dim v As Decimal
        Dim invalide As Boolean = Not Saisie.TryDecimal(Convert.ToString(e.FormattedValue), v)
        If Not invalide Then
            If nom = "Prix" Then
                invalide = v < 0D
            ElseIf nom = "Nombre" Then
                invalide = v <= 0D OrElse v <> Math.Floor(v)
            Else
                invalide = v <= 0D
            End If
        End If

        If invalide Then
            e.Cancel = True
            dgvLignes.Rows(e.RowIndex).ErrorText = "Valeur numérique invalide."
        Else
            dgvLignes.Rows(e.RowIndex).ErrorText = ""
        End If
    End Sub

    Private Sub dgvLignes_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLignes.CellEndEdit
        dgvLignes.Rows(e.RowIndex).ErrorText = ""
        _lignes.ResetItem(e.RowIndex)
        MettreAJourSomme()
    End Sub

    Private Sub dgvLignes_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvLignes.DataError
        e.ThrowException = False
    End Sub

    Private Sub dgvLignes_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvLignes.KeyDown
        If e.KeyCode = Keys.Delete AndAlso Not dgvLignes.IsCurrentCellInEditMode AndAlso dgvLignes.CurrentRow IsNot Nothing Then
            _lignes.RemoveAt(dgvLignes.CurrentRow.Index)
            MettreAJourSomme()
            e.Handled = True
        End If
    End Sub

    ' ---------- Enregistrement du ticket ----------
    Private Sub btnEnregistrerTicket_Click(sender As Object, e As EventArgs) Handles btnEnregistrerTicket.Click
        If cmbMagasin.SelectedValue Is Nothing Then
            Avertir("Choisissez le magasin.", cmbMagasin)
            Return
        End If

        Dim numero As String = txtNumeroTicket.Text.Trim()
        If numero = "" OrElse numero.Length > 50 Then
            Avertir("Saisissez le numéro du ticket (50 caractères au maximum).", txtNumeroTicket)
            Return
        End If

        If dtpDateAchat.Value.Date > Date.Today Then
            Avertir("La date d'achat ne peut pas être dans le futur.", dtpDateAchat)
            Return
        End If

        If _lignes.Count = 0 Then
            Avertir("Ajoutez au moins une ligne au ticket.", cmbProduit)
            Return
        End If

        Dim totalSaisi As Decimal
        If Not Saisie.TryDecimal(txtTotalTicket.Text, totalSaisi) OrElse totalSaisi < 0D Then
            Avertir("Saisissez le total figurant sur le ticket (nombre supérieur ou égal à 0).", txtTotalTicket)
            Return
        End If

        ' Contrôle du total saisi par rapport à la somme des lignes (EF-04)
        Dim somme As Decimal = _lignes.Sum(Function(l) l.TotalLigne)
        If Math.Abs(somme - totalSaisi) > 0.01D Then
            Avertir("Le total saisi (" & totalSaisi.ToString("N2") & ") ne correspond pas à la somme des lignes (" &
                    somme.ToString("N2") & "). Vérifiez le ticket.", txtTotalTicket)
            Return
        End If

        Try
            _ticketRepo.EnregistrerTicket(Session.IdUtilisateur, CInt(cmbMagasin.SelectedValue), numero,
                                          dtpDateAchat.Value.Date, totalSaisi, _lignes.ToList())
            MessageBox.Show("Ticket enregistré.", "Pricieux", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ViderChamps()
        Catch ex As MySqlException When ex.Number = 1062
            MessageBox.Show("Un ticket portant ce numéro est déjà enregistré pour ce magasin.", "Doublon", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Catch ex As MySqlException
            MessageBox.Show("Échec de l'enregistrement, le ticket n'a pas été enregistré : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnViderFormulaire_Click(sender As Object, e As EventArgs) Handles btnViderFormulaire.Click
        If _lignes.Count > 0 AndAlso
           MessageBox.Show("Effacer le ticket en cours de saisie ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        ViderChamps()
    End Sub

    ' ---------- Raccourcis clavier et fermeture ----------
    Private Sub FrmTickets_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.S Then
            e.SuppressKeyPress = True
            btnEnregistrerTicket.PerformClick()
        End If
    End Sub

    Private Sub FrmTickets_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If _lignes.Count > 0 AndAlso
           MessageBox.Show("Le ticket en cours n'est pas enregistré. Fermer quand même ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            e.Cancel = True
        End If
    End Sub

    ' ---------- Utilitaires ----------
    Private Sub Signaler(ctrl As Control, message As String)
        _ep.SetError(ctrl, message)
        ctrl.Focus()
    End Sub

    Private Sub Avertir(message As String, ctrl As Control)
        MessageBox.Show(message, "Pricieux", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ctrl.Focus()
    End Sub

    Private Sub MettreAJourSomme()
        Dim somme As Decimal = _lignes.Sum(Function(l) l.TotalLigne)
        Me.Text = "Tickets – somme des lignes : " & somme.ToString("N2")
    End Sub

    Private Sub ViderChamps()
        _chargement = True
        _lignes.Clear()
        _ep.Clear()
        cmbMagasin.SelectedIndex = -1
        cmbProduit.SelectedIndex = -1
        cmbProduit.Text = ""
        cmbUnite.Items.Clear()
        _uniteBaseCourante = ""
        txtNumeroTicket.Clear()
        txtQuantite.Clear()
        txtPrix.Clear()
        txtTotalTicket.Clear()
        nudNombre.Value = 1
        dtpDateAchat.Value = Date.Today
        _chargement = False
        MettreAJourSomme()
        cmbMagasin.Focus()
    End Sub
End Class
