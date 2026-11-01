
Imports System.Threading.Tasks

Public Class FrmProduits

    Private Async Sub btnRechercherCode_Click(sender As Object, e As EventArgs) Handles btnRechercher.Click

        ' 1. Récupérer le code-barres saisi
        Dim code As String = txtRechercheCode.Text.Trim()

        If String.IsNullOrWhiteSpace(code) Then
            MessageBox.Show("Veuillez saisir un code-barres.")
            txtRechercheCode.Focus()
            Return
        End If

        Try
            ' 2. Désactiver le bouton pendant la recherche
            btnRechercher.Enabled = False
            btnRechercher.Text = "Recherche..."

            ' 3. Appeler le service Open Food Facts
            Dim service As New OpenFoodFactsService()

            Dim produit As ProduitOFF =
                Await service.RechercherProduitAsync(code)

            ' 4. Vérifier si le produit existe
            If produit Is Nothing Then
                MessageBox.Show(
                    "Aucun produit trouvé pour ce code-barres.",
                    "Produit introuvable",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                )
                Return
            End If

            ' 5. Remplir les champs du formulaire
            txtCodeBarres.Text = produit.CodeBarres
            txtNom.Text = produit.Nom
            txtMarque.Text = produit.Marque

            MessageBox.Show(
                "Produit récupéré avec succès !" &
                vbCrLf & "Nom : " & produit.Nom &
                vbCrLf & "Marque : " & produit.Marque &
                vbCrLf & "Quantité : " & produit.Quantite &
                vbCrLf & vbCrLf &
                "Vérifiez les informations avant l'enregistrement.",
                "Recherche réussie",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

        Catch ex As Exception

            MessageBox.Show(
                "Une erreur est survenue pendant la recherche :" &
                vbCrLf & ex.Message,
                "Erreur",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        Finally

            btnRechercher.Enabled = True
            btnRechercher.Text = "Rechercher"

        End Try

    End Sub

End Class
