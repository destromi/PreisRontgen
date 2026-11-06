Public Class Referentiels
    Private referentiel As New ReferentielMySql
    Private Sub BtnEnregistrer_Click(sender As Object, e As EventArgs) Handles BtnEnregistrer.Click
        referentiel.saveMagasins(TextBoxMagasin.Text)
        'TextBoxMagasin.Clear()
        ListBoxMagasins.DataSource = referentiel.GetMagasin
    End Sub

    Private Sub BtnNouveau_Click(sender As Object, e As EventArgs) Handles BtnNouveau.Click
        TextBoxMagasin.Clear()
    End Sub

    Private Sub BtnNewCateg_Click(sender As Object, e As EventArgs) Handles BtnNewCateg.Click
        TextBoxCateg.Clear()
    End Sub

    Private Sub BtnEnregistrerCateg_Click(sender As Object, e As EventArgs) Handles BtnEnregistrerCateg.Click
        referentiel.saveCategorie(TextBoxCateg.Text)
        ListBoxCateg.DataSource = referentiel.GetCategorie
    End Sub

    Private Sub Referentiels_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListBoxMagasins.DataSource = referentiel.GetMagasin
        ListBoxCateg.DataSource = referentiel.GetCategorie
    End Sub
End Class