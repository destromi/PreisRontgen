Public Class Referentiels
    'Private referentiel As New ReferentielMySql
    Private prod As FrmProduits
    Private referentiel As New ReferentielService
    'Friend Class referentiele
    '    Public Property id As Integer
    '    Public Property nom As String
    'End Class
    Private Sub BtnEnregistrer_Click(sender As Object, e As EventArgs) Handles BtnEnregistrer.Click
        Dim id As Integer = referentiel.saveMagasins(TextBoxMagasin.Text)
        'TextBoxMagasin.Clear()
        ListBoxMagasins.DataSource = referentiel.GetMagasin
        ListBoxMagasins.SelectedValue = id
    End Sub

    Private Sub BtnNouveau_Click(sender As Object, e As EventArgs) Handles BtnNouveau.Click
        TextBoxMagasin.Clear()
        ListBoxMagasins.SelectedValue = Nothing
    End Sub

    Private Sub BtnNewCateg_Click(sender As Object, e As EventArgs) Handles BtnNewCateg.Click
        TextBoxCateg.Clear()
        'ListBoxCateg.SelectedValue = Nothing
    End Sub

    Private Sub BtnEnregistrerCateg_Click(sender As Object, e As EventArgs) Handles BtnEnregistrerCateg.Click
        Dim id As Integer = referentiel.saveCategorie(TextBoxCateg.Text)
        ListBoxCateg.DataSource = referentiel.GetCategorie
        'ListBoxCateg.SelectedIndex = ListBoxCateg.FindStringExact(TextBoxCateg.Text)
        ListBoxCateg.SelectedValue = id
    End Sub

    Private Sub Referentiels_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ListBoxMagasins.DataSource = referentiel.GetMagasin
        ListBoxCateg.DataSource = referentiel.GetCategorie
    End Sub


    Private Sub TabProduits_Selected(sender As Object, e As EventArgs) Handles TabControls1.SelectedIndexChanged
        prod = New FrmProduits
        prod.TopLevel = False
        prod.FormBorderStyle = FormBorderStyle.None
        prod.Dock = DockStyle.Fill
        TabProduits.Controls.Add(prod)
        prod.Show()
    End Sub

    Private Sub BtnDesactiver_Click(sender As Object, e As EventArgs) Handles BtnDesactiver.Click
        Dim reponse As DialogResult = MessageBox.Show("Voulez-vous vraiment désactiver ce magasin ? ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reponse = DialogResult.Yes Then
            Dim id As Integer = ListBoxMagasins.SelectedValue
            referentiel.DesactiverMagasin(id)
            ListBoxMagasins.DataSource = referentiel.GetMagasin
        End If
    End Sub

    Private Sub BtnDelCateg_Click(sender As Object, e As EventArgs) Handles BtnDelCateg.Click
        Dim reponse As DialogResult = MessageBox.Show("Voulez-vous vraiment désactiver cette catégorie ? ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reponse = DialogResult.Yes Then
            Dim id As Integer = ListBoxMagasins.SelectedValue
            referentiel.DesactiveCategorie(id)
            ListBoxCateg.DataSource = referentiel.GetCategorie
        End If
    End Sub
End Class