Option Strict On
Option Explicit On

''' <summary>Couleurs et style communs : vert forêt, pages gris clair.</summary>
Public Module ThemePricieux
    Public ReadOnly VertForet As Color = Color.FromArgb(34, 85, 51)      ' bandeau, boutons
    Public ReadOnly VertForetClair As Color = Color.FromArgb(46, 125, 50) ' survol, accents
    Public ReadOnly FondPage As Color = Color.FromArgb(242, 242, 242)    ' pages gris clair
    Public ReadOnly EnteteGrille As Color = Color.FromArgb(225, 228, 225) ' en-tête DataGridView

    Public Sub Appliquer(racine As Control)
        If TypeOf racine Is Form Then racine.BackColor = FondPage
        For Each c As Control In racine.Controls
            Select Case True
                Case TypeOf c Is Button
                    Dim b As Button = DirectCast(c, Button)
                    b.FlatStyle = FlatStyle.Flat
                    b.FlatAppearance.BorderSize = 0
                    b.BackColor = VertForet
                    b.ForeColor = Color.White
                    b.FlatAppearance.MouseOverBackColor = VertForetClair
                    b.Cursor = Cursors.Hand
                Case TypeOf c Is DataGridView
                    Dim g As DataGridView = DirectCast(c, DataGridView)
                    g.EnableHeadersVisualStyles = False
                    g.ColumnHeadersDefaultCellStyle.BackColor = EnteteGrille
                    g.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
                    g.BackgroundColor = Color.White
                    g.BorderStyle = BorderStyle.None
                    g.GridColor = Color.Gainsboro
                    g.DefaultCellStyle.SelectionBackColor = VertForetClair
                    g.DefaultCellStyle.SelectionForeColor = Color.White
                Case TypeOf c Is TabControl
                    ' Les pages d'onglets gardent le fond gris clair
                    For Each page As TabPage In DirectCast(c, TabControl).TabPages
                        page.BackColor = FondPage
                    Next
            End Select
            Appliquer(c)   ' récursif : panneaux, groupes, onglets
        Next
    End Sub
End Module