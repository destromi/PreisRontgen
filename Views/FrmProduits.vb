Option Strict On

Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
'Imports MySql.Data.MySqlClient
Imports MySqlConnector

''' <summary>
''' Formulaire de gestion des produits.
''' Permet d'ajouter, modifier et rechercher des produits dans le catalogue.
''' </summary>
Partial Public Class FrmProduits

    Private idProduitSelectionne As Integer = 0

    ' =====================================================
    ' INITIALISATION DU FORMULAIRE
    ' =====================================================

    Private Sub FrmProduits_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ConfigurerInterface()
        ChargerUnites()
        ChargerCategories()
        ChargerProduits()

        EffacerFormulaire()

    End Sub

    ' =====================================================
    ' CONFIGURATION DE L'INTERFACE
    ' =====================================================

    Private Sub ConfigurerInterface()

        dgvProduits.AutoGenerateColumns = True
        dgvProduits.ReadOnly = True
        dgvProduits.AllowUserToAddRows = False
        dgvProduits.AllowUserToDeleteRows = False
        dgvProduits.MultiSelect = False
        dgvProduits.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvProduits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvProduits.BackgroundColor = Color.White
        dgvProduits.BorderStyle = BorderStyle.None

        dgvProduits.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(34, 85, 51)

        dgvProduits.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvProduits.ColumnHeadersDefaultCellStyle.Font =
            New Font("Segoe UI", 9, FontStyle.Bold)

        dgvProduits.EnableHeadersVisualStyles = False
        dgvProduits.RowTemplate.Height = 30

        btnEnregistrer.BackColor = Color.FromArgb(34, 85, 51)
        btnEnregistrer.ForeColor = Color.White
        btnEnregistrer.FlatStyle = FlatStyle.Flat

        btnNouveau.FlatStyle = FlatStyle.Flat
        btnDesactiver.FlatStyle = FlatStyle.Flat

    End Sub

    ' =====================================================
    ' CHARGEMENT DES UNITÉS
    ' =====================================================

    Private Sub ChargerUnites()

        cmbUnite.Items.Clear()

        cmbUnite.Items.Add("kg")
        cmbUnite.Items.Add("L")
        cmbUnite.Items.Add("piece")

        cmbUnite.SelectedIndex = 0

    End Sub

    ' =====================================================
    ' CHARGEMENT DES CATÉGORIES
    ' =====================================================

    Private Sub ChargerCategories()

        Try

            Using cn As MySqlConnection = ConnexionBD.CreerConnexion()

                If cn.State <> ConnectionState.Open Then
                    cn.Open()
                End If

                Dim sql As String =
                    "SELECT id, nom FROM categorie ORDER BY nom"

                Using cmd As New MySqlCommand(sql, cn)

                    Using da As New MySqlDataAdapter(cmd)

                        Dim table As New DataTable()
                        da.Fill(table)

                        cmbCategorie.DataSource = Nothing
                        cmbCategorie.DisplayMember = "nom"
                        cmbCategorie.ValueMember = "id"
                        cmbCategorie.DataSource = table
                        cmbCategorie.SelectedIndex = -1

                    End Using

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Impossible de charger les catégories." &
                Environment.NewLine & ex.Message,
                "Erreur",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub

    ' =====================================================
    ' CHARGEMENT DES PRODUITS
    ' =====================================================

    Private Sub ChargerProduits(Optional recherche As String = "")

        Try

            Using cn As MySqlConnection = ConnexionBD.CreerConnexion()

                If cn.State <> ConnectionState.Open Then
                    cn.Open()
                End If

                Dim sql As String =
                    "SELECT p.id, p.nom AS Produit, " &
                    "p.marque AS Marque, c.nom AS Categorie, " &
                    "p.unite_base AS Unite, " &
                    "p.code_barres AS CodeBarres " &
                    "FROM produit p " &
                    "INNER JOIN categorie c ON p.id_categorie = c.id " &
                    "WHERE p.nom LIKE @recherche " &
                    "OR p.marque LIKE @recherche " &
                    "OR c.nom LIKE @recherche " &
                    "OR p.code_barres LIKE @recherche " &
                    "ORDER BY p.nom"

                Using cmd As New MySqlCommand(sql, cn)

                    cmd.Parameters.AddWithValue(
                        "@recherche",
                        "%" & recherche.Trim() & "%")

                    Using da As New MySqlDataAdapter(cmd)

                        Dim table As New DataTable()
                        da.Fill(table)

                        dgvProduits.DataSource = table

                        If dgvProduits.Columns.Contains("id") Then
                            dgvProduits.Columns("id").Visible = False
                        End If

                        If dgvProduits.Columns.Contains("Produit") Then
                            dgvProduits.Columns("Produit").HeaderText = "Produit"
                        End If

                        If dgvProduits.Columns.Contains("Categorie") Then
                            dgvProduits.Columns("Categorie").HeaderText = "Catégorie"
                        End If

                        If dgvProduits.Columns.Contains("CodeBarres") Then
                            dgvProduits.Columns("CodeBarres").HeaderText = "Code-barres"
                        End If

                        If dgvProduits.Columns.Contains("Unite") Then
                            dgvProduits.Columns("Unite").HeaderText = "Unité"
                        End If

                    End Using

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Erreur lors du chargement des produits." &
                Environment.NewLine & ex.Message,
                "Erreur",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub

    ' =====================================================
    ' ENREGISTREMENT D'UN PRODUIT
    ' =====================================================

    Private Sub btnEnregistrer_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnEnregistrer.Click

        If Not ValiderFormulaire() Then
            Return
        End If

        Try

            Using cn As MySqlConnection = ConnexionBD.CreerConnexion()

                If cn.State <> ConnectionState.Open Then
                    cn.Open()
                End If

                Dim sql As String =
                    "INSERT INTO produit " &
                    "(nom, marque, id_categorie, unite_base, code_barres) " &
                    "VALUES " &
                    "(@nom, @marque, @categorie, @unite, @codebarres)"

                Using cmd As New MySqlCommand(sql, cn)

                    cmd.Parameters.AddWithValue("@nom", txtNom.Text.Trim())

                    cmd.Parameters.AddWithValue(
                        "@marque",
                        txtMarque.Text.Trim())

                    cmd.Parameters.AddWithValue(
                        "@categorie",
                        Convert.ToInt32(cmbCategorie.SelectedValue))

                    cmd.Parameters.AddWithValue(
                        "@unite",
                        cmbUnite.SelectedItem.ToString())

                    If String.IsNullOrWhiteSpace(txtCodeBarres.Text) Then

                        cmd.Parameters.AddWithValue(
                            "@codebarres",
                            DBNull.Value)

                    Else

                        cmd.Parameters.AddWithValue(
                            "@codebarres",
                            txtCodeBarres.Text.Trim())

                    End If

                    cmd.ExecuteNonQuery()

                End Using

            End Using

            MessageBox.Show(
                "Le produit a été enregistré avec succès.",
                "PRICIEUX",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

            ChargerProduits(txtRecherche.Text)
            EffacerFormulaire()

        Catch ex As MySqlException

            If ex.Number = 1062 Then

                MessageBox.Show(
                    "Ce produit existe déjà ou ce code-barres est déjà utilisé.",
                    "Doublon",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning)

            Else

                MessageBox.Show(
                    "Erreur MySQL : " & ex.Message,
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error)

            End If

        Catch ex As Exception

            MessageBox.Show(
                "Impossible d'enregistrer le produit." &
                Environment.NewLine & ex.Message,
                "Erreur",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub

    ' =====================================================
    ' VALIDATION DES CHAMPS
    ' =====================================================

    Private Function ValiderFormulaire() As Boolean

        errProvider.Clear()

        Dim valide As Boolean = True

        If String.IsNullOrWhiteSpace(txtNom.Text) Then

            errProvider.SetError(txtNom, "Le nom du produit est obligatoire.")
            valide = False

        End If

        If cmbCategorie.SelectedIndex = -1 Then

            errProvider.SetError(
                cmbCategorie,
                "Veuillez sélectionner une catégorie.")

            valide = False

        End If

        If cmbUnite.SelectedIndex = -1 Then

            errProvider.SetError(
                cmbUnite,
                "Veuillez sélectionner une unité.")

            valide = False

        End If

        If txtCodeBarres.Text.Trim().Length > 50 Then

            errProvider.SetError(
                txtCodeBarres,
                "Le code-barres ne doit pas dépasser 50 caractères.")

            valide = False

        End If

        If Not valide Then

            MessageBox.Show(
                "Veuillez corriger les champs signalés.",
                "Vérification",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)

        End If

        Return valide

    End Function

    ' =====================================================
    ' BOUTON NOUVEAU
    ' =====================================================

    Private Sub btnNouveau_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnNouveau.Click

        EffacerFormulaire()

    End Sub

    Private Sub EffacerFormulaire()

        idProduitSelectionne = 0

        txtNom.Clear()
        txtMarque.Clear()
        txtCodeBarres.Clear()

        If cmbCategorie.Items.Count > 0 Then
            cmbCategorie.SelectedIndex = -1
        End If

        If cmbUnite.Items.Count > 0 Then
            cmbUnite.SelectedIndex = 0
        End If

        errProvider.Clear()

        dgvProduits.ClearSelection()

        txtNom.Focus()

        btnEnregistrer.Text = "Enregistrer le produit"

    End Sub

    ' =====================================================
    ' RECHERCHE DANS LE CATALOGUE
    ' =====================================================

    Private Sub txtRecherche_TextChanged(
        sender As Object,
        e As EventArgs
    ) Handles txtRecherche.TextChanged

        ChargerProduits(txtRecherche.Text)

    End Sub

    ' =====================================================
    ' SÉLECTION D'UN PRODUIT DANS LE TABLEAU
    ' =====================================================

    Private Sub dgvProduits_CellClick(
        sender As Object,
        e As DataGridViewCellEventArgs
    ) Handles dgvProduits.CellClick

        If e.RowIndex < 0 Then
            Return
        End If

        Dim ligne As DataGridViewRow =
            dgvProduits.Rows(e.RowIndex)

        idProduitSelectionne =
            Convert.ToInt32(ligne.Cells("id").Value)

        txtNom.Text =
            Convert.ToString(ligne.Cells("Produit").Value)

        txtMarque.Text =
            Convert.ToString(ligne.Cells("Marque").Value)

        txtCodeBarres.Text =
            Convert.ToString(ligne.Cells("CodeBarres").Value)

        cmbCategorie.Text =
            Convert.ToString(ligne.Cells("Categorie").Value)

        cmbUnite.Text =
            Convert.ToString(ligne.Cells("Unite").Value)

        btnEnregistrer.Text = "Enregistrer le produit"

    End Sub

    ' =====================================================
    ' RECHERCHE OPEN FOOD FACTS
    ' =====================================================

    Private Sub btnRechercherOFF_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnRechercherOFF.Click

        If String.IsNullOrWhiteSpace(txtCodeBarres.Text) Then

            MessageBox.Show(
                "Veuillez saisir un code-barres.",
                "Open Food Facts",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)

            txtCodeBarres.Focus()
            Return

        End If

        lblInfoOFF.Text =
            "La recherche Open Food Facts doit être reliée à " &
            "OpenFoodFactsService.vb."

        MessageBox.Show(
            "Le bouton est prêt, mais la recherche doit être connectée " &
            "à ton service Open Food Facts existant.",
            "Open Food Facts",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

    End Sub

    ' DÉSACTIVATION NON DISPONIBLE DANS LE SCHÉMA ACTUEL
    Private Sub btnDesactiver_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnDesactiver.Click

        MessageBox.Show(
            "La désactivation n'est pas disponible dans la structure " &
            "actuelle de la table produit." &
            Environment.NewLine & Environment.NewLine &
            "La table ne possède pas de colonne actif ou statut. " &
            "Aucune donnée n'a été supprimée.",
            "Fonction non disponible",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

    End Sub

End Class