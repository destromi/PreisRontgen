Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports MySqlConnector
Imports PreisRontgen.LigneTicketSaisie
Imports PreisRontgen.Models


Public Class FrmTickets

    '  Types internes 

    ' Une ligne saisie, avant l'enregistrement du ticket
    Public Class LigneSaisie
        Public Property IdProduit As Integer
        Public Property Produit As String
        Public Property Conditionnement As Decimal
        Public Property Unite As String
        Public Property Prix As Decimal
        Public Property Nombre As Integer
        Public Property QuantiteBase As Decimal   ' conditionnement converti dans l'unité de base du produit

        Public ReadOnly Property Total As Decimal
            Get
                Return Prix * Nombre
            End Get
        End Property
    End Class


    ' Un produit du référentiel
    Private Class ProduitConnu
        Public Property Id As Integer
        Public Property Libelle As String
        Public Property Categorie As String
        Public Property UniteBase As String
    End Class

    'Données du formulaire 

    Private ReadOnly _lignes As New BindingList(Of LigneSaisie)()
    Private ReadOnly _produits As New List(Of Produit)()
    Private _produitCourant As Produit
    Private CheminImageSelectionner As String = String.Empty

    Private Sub FrmTickets_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LblInfo.Text = String.Empty
        If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Return

        ConfigurerGrille()

        cmbUnite.Items.AddRange(New Object() {"kg", "g", "L", "cL", "mL", "piece"})
        cmbUnite.SelectedIndex = 0

        dtpDate.MaxDate = Date.Today
        dtpDate.Value = Date.Today

        Try
            ChargerMagasins()
            ChargerProduits()
        Catch ex As Exception
            MessageBox.Show("Impossible de charger les données : " & ex.Message,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        AddHandler _lignes.ListChanged, Sub(s, args) MettreAJourSomme()
        MettreAJourSomme()
    End Sub

    Private Sub ConfigurerGrille()
        dgvLignes.AutoGenerateColumns = False
        dgvLignes.ReadOnly = True
        dgvLignes.AllowUserToAddRows = False
        dgvLignes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvLignes.Columns.Clear()

        AjouterColonne("Produit", "Produit", 200)
        AjouterColonne("Conditionnement", "Conditionnement", 90, "0.###", True)
        AjouterColonne("Unite", "Unité", 50)
        AjouterColonne("Prix", "Prix du paquet (FCFA)", 100, "#,##0", True)
        AjouterColonne("Nombre", "Nombre", 60, "", True)
        AjouterColonne("Total", "Total (FCFA)", 100, "#,##0", True)

        dgvLignes.DataSource = _lignes
    End Sub

    Private Sub AjouterColonne(propriete As String, titre As String, poids As Single,
                               Optional format As String = "", Optional alignerDroite As Boolean = False)
        Dim style As New DataGridViewCellStyle()
        If format <> "" Then style.Format = format
        If alignerDroite Then style.Alignment = DataGridViewContentAlignment.MiddleRight

        dgvLignes.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "col" & propriete,
            .DataPropertyName = propriete,
            .HeaderText = titre,
            .FillWeight = poids,
            .DefaultCellStyle = style
        })
    End Sub

    Private Sub ChargerMagasins()
        Dim dt As New DataTable()
        Using cn As MySqlConnection = GetConnection()
            Using cmd As New MySqlCommand("SELECT id, nom FROM magasin ORDER BY nom", cn)
                Using da As New MySqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        cmbMagasin.DisplayMember = "nom"
        cmbMagasin.ValueMember = "id"
        cmbMagasin.DataSource = dt
        cmbMagasin.SelectedIndex = -1
    End Sub

    Private Sub ChargerProduits()
        _produits.Clear()
        Dim sql As String =
            "SELECT p.id, p.nom, p.marque, c.nom, p.unite_base " &
            "FROM produit p INNER JOIN categorie c ON c.id = p.id_categorie " &
            "ORDER BY p.nom"

        Using cn As MySqlConnection = GetConnection()
            cn.Open()
            Using cmd As New MySqlCommand(sql, cn)
                Using rd = cmd.ExecuteReader()
                    While rd.Read()
                        Dim nom As String = rd.GetString(1)
                        Dim marque As String = rd.GetString(2)
                        _produits.Add(New Produit With {
                            .Id = rd.GetInt32(0),
                            .Nom = rd.GetString(1),
                            .Marque = rd.GetString(2),
                            .CategorieNom = rd.GetString(3),
                            .UniteBase = VersUniteBase(rd.GetString(4))
})
                    End While
                End Using
            End Using
        End Using

        ' Suggestions automatiques dans la zone "Produit"
        Dim source As New AutoCompleteStringCollection()
        source.AddRange(_produits.Select(Function(p) p.Libelle).ToArray())
        txtProduit.AutoCompleteCustomSource = source
        txtProduit.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        txtProduit.AutoCompleteSource = AutoCompleteSource.CustomSource
    End Sub

    Private Shared Function VersUniteBase(texte As String) As UniteBase
        If texte = "kg" Then Return UniteBase.Kg
        If texte = "L" Then Return UniteBase.Litre
        Return UniteBase.Piece
    End Function

    Private Shared Function TexteUnite(u As UniteBase) As String
        If u = UniteBase.Kg Then Return "kg"
        If u = UniteBase.Litre Then Return "L"
        Return "piece"
    End Function

    'Produit saisi

    Private Sub txtProduit_TextChanged(sender As Object, e As EventArgs) Handles txtProduit.TextChanged
        Dim texte As String = txtProduit.Text.Trim()
        _produitCourant = _produits.FirstOrDefault(
            Function(p) String.Equals(p.Libelle, texte, StringComparison.CurrentCultureIgnoreCase))

        If texte = "" Then
            lblProduitInfo.Text = ""
        ElseIf _produitCourant Is Nothing Then
            lblProduitInfo.ForeColor = Color.Firebrick
            lblProduitInfo.Text = "Produit introuvable : ajoutez-le d'abord dans les référentiels."
        Else
            lblProduitInfo.ForeColor = Color.FromArgb(34, 85, 51)
            lblProduitInfo.Text = "Catégorie : " & _produitCourant.CategorieNom &
                                  "  |  Unité de base : " & TexteUnite(_produitCourant.UniteBase)
            ' Propose l'unité de base du produit
            If cmbUnite.Items.Contains(_produitCourant.UniteBase) Then
                cmbUnite.SelectedItem = _produitCourant.UniteBase
            End If
        End If
    End Sub

    'Ajouter / supprimer une ligne

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        errProvider.Clear()

        If _produitCourant Is Nothing Then
            errProvider.SetError(txtProduit, "Choisissez un produit existant dans la liste.")
            txtProduit.Focus()
            Return
        End If

        Dim conditionnement As Decimal
        If Not TryLireDecimal(txtConditionnement.Text, conditionnement) OrElse conditionnement <= 0D Then
            errProvider.SetError(txtConditionnement, "Saisissez un conditionnement supérieur à 0.")
            txtConditionnement.Focus()
            Return
        End If

        If cmbUnite.SelectedItem Is Nothing Then
            errProvider.SetError(cmbUnite, "Choisissez une unité.")
            Return
        End If
        Dim unite As String = cmbUnite.SelectedItem.ToString()

        Dim quantiteBase As Decimal? = ConvertirVersBase(conditionnement, unite, TexteUnite(_produitCourant.UniteBase))
        If Not quantiteBase.HasValue Then
            errProvider.SetError(cmbUnite, "Unité incompatible avec l'unité de base du produit (" &
                                 TexteUnite(_produitCourant.UniteBase) & ").")
            Return
        End If

        Dim prix As Decimal
        If Not TryLireDecimal(txtPrix.Text, prix) OrElse prix < 0D Then
            errProvider.SetError(txtPrix, "Saisissez un prix valide (0 ou plus).")
            txtPrix.Focus()
            Return
        End If

        Dim nombre As Integer
        If Not Integer.TryParse(txtNombre.Text.Trim(), nombre) OrElse nombre <= 0 Then
            errProvider.SetError(txtNombre, "Saisissez un nombre entier supérieur à 0.")
            txtNombre.Focus()
            Return
        End If

        _lignes.Add(New LigneSaisie With {
            .IdProduit = _produitCourant.Id,
            .Produit = _produitCourant.Libelle,
            .Conditionnement = conditionnement,
            .Unite = unite,
            .Prix = prix,
            .Nombre = nombre,
            .QuantiteBase = quantiteBase.Value
        })

        ' Prêt pour l'article suivant
        txtProduit.Clear()
        txtConditionnement.Clear()
        txtPrix.Clear()
        txtNombre.Text = "1"
        txtProduit.Focus()
    End Sub

    Private Sub btnSupprimerLigne_Click(sender As Object, e As EventArgs) Handles btnSupprimerLigne.Click
        If dgvLignes.CurrentRow Is Nothing Then Return
        Dim ligne = TryCast(dgvLignes.CurrentRow.DataBoundItem, LigneSaisie)
        If ligne IsNot Nothing Then _lignes.Remove(ligne)
    End Sub

    Private Sub MettreAJourSomme()
        Dim somme As Decimal = _lignes.Sum(Function(l) l.Total)
        lblSomme.Text = "Somme des lignes : " & somme.ToString("#,##0") & " FCFA"
    End Sub

    'Enregistrement du ticket

    Private Sub btnEnregistrer_Click(sender As Object, e As EventArgs) Handles btnEnregistrer.Click
        errProvider.Clear()

        If Session.user Is Nothing Then
            MessageBox.Show("Aucun utilisateur connecté.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If cmbMagasin.SelectedValue Is Nothing Then
            errProvider.SetError(cmbMagasin, "Choisissez un magasin.")
            Return
        End If

        If _lignes.Count = 0 Then
            MessageBox.Show("Ajoutez au moins une ligne au ticket.", "Ticket vide",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim totalSaisi As Decimal
        If Not TryLireDecimal(txtTotalSaisi.Text, totalSaisi) OrElse totalSaisi < 0D Then
            errProvider.SetError(txtTotalSaisi, "Saisissez le total imprimé sur le ticket.")
            txtTotalSaisi.Focus()
            Return
        End If

        Dim somme As Decimal = _lignes.Sum(Function(l) l.Total)
        If totalSaisi <> somme Then
            Dim reponse = MessageBox.Show(
                "Le total saisi (" & totalSaisi.ToString("#,##0") & " FCFA) est différent de la somme des lignes (" &
                somme.ToString("#,##0") & " FCFA)." & vbCrLf & "Enregistrer quand même ?",
                "Totaux différents", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If reponse = DialogResult.No Then Return
        End If

        Try
            EnregistrerTicket(Session.user.Id, Convert.ToInt32(cmbMagasin.SelectedValue),
                              dtpDate.Value.Date, totalSaisi)
        Catch ex As MySqlException
            MessageBox.Show("Erreur lors de l'enregistrement : " & ex.Message,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        MessageBox.Show("Ticket enregistré.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        _lignes.Clear()
        txtTotalSaisi.Clear()
        cmbMagasin.SelectedIndex = -1
    End Sub

    ' Tout est enregistré dans une transaction : si une requête échoue, rien n'est gardé.
    Private Sub EnregistrerTicket(idUtilisateur As Integer, idMagasin As Integer,
                                  dateAchat As Date, totalSaisi As Decimal)
        Using cn As MySqlConnection = GetConnection()
            cn.Open()
            Using tx = cn.BeginTransaction()
                Try
                    Dim idTicket As Long
                    Using cmd As New MySqlCommand(
                        "INSERT INTO ticket (id_utilisateur, id_magasin, date_achat, total_saisi) " &
                        "VALUES (@u, @m, @d, @t)", cn, tx)
                        cmd.Parameters.AddWithValue("@u", idUtilisateur)
                        cmd.Parameters.AddWithValue("@m", idMagasin)
                        cmd.Parameters.AddWithValue("@d", dateAchat)
                        cmd.Parameters.AddWithValue("@t", totalSaisi)
                        cmd.ExecuteNonQuery()
                        idTicket = cmd.LastInsertedId
                    End Using

                    For Each l In _lignes
                        Dim idLigne As Long
                        Using cmd As New MySqlCommand(
                            "INSERT INTO ligne_ticket (id_ticket, id_produit, quantite_conditionnement, unite, prix, nb_achete) " &
                            "VALUES (@t, @p, @q, @u, @prix, @nb)", cn, tx)
                            cmd.Parameters.AddWithValue("@t", idTicket)
                            cmd.Parameters.AddWithValue("@p", l.IdProduit)
                            cmd.Parameters.AddWithValue("@q", l.Conditionnement)
                            cmd.Parameters.AddWithValue("@u", l.Unite)
                            cmd.Parameters.AddWithValue("@prix", l.Prix)
                            cmd.Parameters.AddWithValue("@nb", l.Nombre)
                            cmd.ExecuteNonQuery()
                            idLigne = cmd.LastInsertedId
                        End Using

                        ' Observation de prix : prix par unité de base (kg, L ou pièce)
                        Using cmd As New MySqlCommand(
                            "INSERT INTO observation_prix (id_utilisateur, id_produit, id_magasin, id_ligne_ticket, date_obs, prix_unitaire, quantite_base) " &
                            "VALUES (@u, @p, @m, @l, @d, @pu, @qb)", cn, tx)
                            cmd.Parameters.AddWithValue("@u", idUtilisateur)
                            cmd.Parameters.AddWithValue("@p", l.IdProduit)
                            cmd.Parameters.AddWithValue("@m", idMagasin)
                            cmd.Parameters.AddWithValue("@l", idLigne)
                            cmd.Parameters.AddWithValue("@d", dateAchat)
                            cmd.Parameters.AddWithValue("@pu", Math.Round(l.Prix / l.QuantiteBase, 4))
                            cmd.Parameters.AddWithValue("@qb", l.QuantiteBase)
                            cmd.ExecuteNonQuery()
                        End Using
                    Next

                    tx.Commit()
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    '  Outils

    ' Accepte "1500", "1 500", "1500,50" ou "1500.50"
    Private Shared Function TryLireDecimal(texte As String, ByRef valeur As Decimal) As Boolean
        Dim t As String = texte.Trim().Replace(" ", "").Replace(ChrW(160), "").Replace(",", ".")
        Return Decimal.TryParse(t, NumberStyles.Number, CultureInfo.InvariantCulture, valeur)
    End Function

    ' Convertit une quantité vers l'unité de base du produit (Nothing si incompatible)
    Private Shared Function ConvertirVersBase(quantite As Decimal, unite As String, uniteBase As String) As Decimal?
        Select Case uniteBase
            Case "kg"
                If unite = "kg" Then Return quantite
                If unite = "g" Then Return quantite / 1000D
            Case "L"
                If unite = "L" Then Return quantite
                If unite = "cL" Then Return quantite / 100D
                If unite = "mL" Then Return quantite / 1000D
            Case "piece"
                If unite = "piece" Then Return quantite
        End Select
        Return Nothing
    End Function

    ' Renvoie False si la ligne est incomplète : l'utilisateur la saisit alors à la main
    Private Function AjouterLigneOcr(l As LigneOcr) As Boolean
        Dim produit As Produit = l.Produit
        Dim uniteBaseTexte As String = TexteUnite(produit.UniteBase)

        Dim lueConditionnement As Decimal? = l.QuantiteConditionnement
        Dim luePrix As Decimal? = l.PrixLu
        Dim lueNombre As Decimal? = l.NombreAchete

        Dim conditionnement As Decimal = lueConditionnement.GetValueOrDefault()
        Dim unite As String = If(l.UniteConditionnement, String.Empty)

        If conditionnement <= 0D Then
            ' Conditionnement non lu : acceptable seulement pour un produit vendu à la pièce
            If produit.UniteBase <> UniteBase.Piece Then Return False
            conditionnement = 1D
            unite = "piece"
        End If

        If unite = "ml" Then unite = "mL"
        If unite = "cl" Then unite = "cL"
        If String.IsNullOrEmpty(unite) Then unite = uniteBaseTexte

        Dim quantiteBase As Decimal? = ConvertirVersBase(conditionnement, unite, uniteBaseTexte)
        If Not quantiteBase.HasValue Then Return False       ' unité inconnue (ex. mg) ou incompatible

        Dim nombre As Integer = 1
        If lueNombre.GetValueOrDefault() >= 1D Then nombre = CInt(Math.Round(lueNombre.GetValueOrDefault()))

        _lignes.Add(New LigneSaisie With {
        .IdProduit = CInt(produit.Id),
        .Produit = produit.Libelle,
        .Conditionnement = conditionnement,
        .Unite = unite,
        .Prix = luePrix.GetValueOrDefault(),
        .Nombre = nombre,
        .QuantiteBase = quantiteBase.Value
    })
        Return True
    End Function

    Private Async Sub BtnOCR_Click(sender As Object, e As EventArgs) Handles BtnOCR.Click
        Using Ofd As New OpenFileDialog
            Ofd.Title = "Choisir une image pour la lecture OCR "
            Ofd.Filter = "Fichiers image (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Tous les fichiers (*.*)|*.*"
            'affichage de la boîte de dialogue
            If Ofd.ShowDialog() = DialogResult.OK Then
                'récupération du chemin du fichier sélectionné
                CheminImageSelectionner = Ofd.FileName
            End If
        End Using
        Cursor = Cursors.WaitCursor
        LblInfo.Text = "Lecture en cours..."
        Try
            Dim service As New TesseractOcrService()
            Dim lues As List(Of LigneOcr) = Await service.LireTicketAsync(CheminImageSelectionner, _produits)

            Dim ajoutees As Integer = 0
            For Each l As LigneOcr In lues
                If AjouterLigneOcr(l) Then ajoutees += 1
            Next

            LblInfo.Text = ajoutees.ToString() & " ligne(s) ajoutée(s) sur " & lues.Count.ToString() &
                           " reconnue(s). Vérifiez prix et quantités avant d'enregistrer."
        Catch ex As Exception
            LblInfo.Text = ex.Message
            'MessageBox.Show("Lecture impossible : " & ex.Message, "OCR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Cursor = Cursors.Default
            BtnOCR.Enabled = True
        End Try
    End Sub
End Class