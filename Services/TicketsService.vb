Imports System.IO
Imports PreisRontgen.LigneTicketSaisie
Imports Tesseract
'Imports TesseractOCR
'Imports TesseractOCR.Enums

Public Class TicketsService
    Public Async Function lectureTicket(CheminImageSelectionner As String, Produit As IList(Of Produit)) As Task(Of List(Of LigneOcr))
        If String.IsNullOrEmpty(CheminImageSelectionner) OrElse Not File.Exists(CheminImageSelectionner) Then
            Throw New Exception("Veuillez d'abord charger une image.")

        ElseIf Not Path.GetExtension(CheminImageSelectionner).Equals(".jpg", StringComparison.OrdinalIgnoreCase) AndAlso
               Not Path.GetExtension(CheminImageSelectionner).Equals(".jpeg", StringComparison.OrdinalIgnoreCase) AndAlso
               Not Path.GetExtension(CheminImageSelectionner).Equals(".png", StringComparison.OrdinalIgnoreCase) AndAlso
               Not Path.GetExtension(CheminImageSelectionner).Equals(".bmp", StringComparison.OrdinalIgnoreCase) Then
            Throw New Exception("Veuillez charger une image au format JPG, JPEG, PNG ou BMP.")

        End If
        Try
            Dim serviceOcr As New TesseractOcrService()
            Dim lignes As List(Of LigneOcr) = Await serviceOcr.LireTicketAsync(CheminImageSelectionner, Produit)
            Return lignes
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
End Class
