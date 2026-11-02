
Imports System.Data

Public Class FrmTickets

        Private Sub FrmTickets_Load(sender As Object, e As EventArgs) Handles MyBase.Load

            Try
                ' Charger les magasins
                Dim ticketRepo As New TicketRepository()
                Dim magasins As DataTable = ticketRepo.ChargerMagasins()

                cmbMagasin.DataSource = magasins
                cmbMagasin.DisplayMember = "nom"
                cmbMagasin.ValueMember = "id"

                ' Charger les produits
                Dim produitRepo As New ProduitRepository()
                Dim produits As DataTable = produitRepo.ChargerProduits()

                cmbProduit.DataSource = produits
                cmbProduit.DisplayMember = "nom"
                cmbProduit.ValueMember = "id"

            Catch ex As Exception

                MessageBox.Show(
                "Erreur lors du chargement des données : " & ex.Message,
                "Erreur",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

            End Try

        End Sub
End Class