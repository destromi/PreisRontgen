Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Windows.Forms

Friend NotInheritable Class ThemePricieux
    Private Sub New()
    End Sub

    Public Shared ReadOnly Foret As Color = Color.FromArgb(24, 83, 52)
    Public Shared ReadOnly Action As Color = Color.FromArgb(31, 111, 62)
    Public Shared ReadOnly Texte As Color = Color.FromArgb(31, 45, 36)
    Public Shared ReadOnly TexteSecondaire As Color = Color.FromArgb(75, 89, 79)
    Public Shared ReadOnly TexteEntete As Color = Color.FromArgb(224, 239, 227)
    Public Shared ReadOnly FondPage As Color = Color.FromArgb(244, 247, 244)
    Public Shared ReadOnly VertPale As Color = Color.FromArgb(228, 239, 231)
    Public Shared ReadOnly Selection As Color = Color.FromArgb(215, 234, 220)

    Public Const Slogan As String = "Suivi des prix et hausse des prix"

    Public Shared Sub DessinerOnglet(sender As Object, e As DrawItemEventArgs)
        Dim onglets As TabControl = TryCast(sender, TabControl)
        If onglets Is Nothing OrElse e.Index < 0 OrElse e.Index >= onglets.TabPages.Count Then Return

        Dim selectionne As Boolean = e.Index = onglets.SelectedIndex
        Dim couleurFond As Color = If(selectionne, Color.White, VertPale)
        Dim couleurTexte As Color = Foret
        Using pinceau As New SolidBrush(couleurFond)
            e.Graphics.FillRectangle(pinceau, e.Bounds)
        End Using
        TextRenderer.DrawText(e.Graphics, onglets.TabPages(e.Index).Text, onglets.Font, e.Bounds,
            couleurTexte, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.EndEllipsis)
    End Sub
End Class
