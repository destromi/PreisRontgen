Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Globalization
Imports System.Linq
Imports System.Windows.Forms

Public NotInheritable Partial Class MainForm
    Private ReadOnly cmbProduitHistorique As New ComboBox()
    Private ReadOnly cmbMagasinHistorique As New ComboBox()
    Private ReadOnly dgvHistorique As New DataGridView()
    Private ReadOnly pnlGraphique As New Panel()
    Private _observationsAffichees As New List(Of ObservationPrix)()
    Private _historiqueCharge As Boolean

    Private ReadOnly dtpDebutAlerte As New DateTimePicker()
    Private ReadOnly dtpFinAlerte As New DateTimePicker()
    Private ReadOnly cmbMagasinAlerte As New ComboBox()
    Private ReadOnly cmbTypeAlerte As New ComboBox()
    Private ReadOnly chkNonLues As New CheckBox()
    Private ReadOnly dgvAlertes As New DataGridView()
    Private _alertesAffichees As New List(Of AlertePrix)()

    Private Sub ConstruireOngletHistorique()
        Dim page As New TabPage("Historique") With {.BackColor = ThemePricieux.FondPage, .Padding = New Padding(12)}
        _onglets.TabPages.Add(page)
        Dim disposition As New TableLayoutPanel With {
            .Name = "layoutHistorique",
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 3,
            .Margin = Padding.Empty,
            .Padding = Padding.Empty
        }
        disposition.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        disposition.RowStyles.Add(New RowStyle(SizeType.Absolute, 42.0F))
        disposition.RowStyles.Add(New RowStyle(SizeType.Absolute, 62.0F))
        disposition.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        page.Controls.Add(disposition)

        Dim titre As New Label With {
            .Text = "Historique des prix unitaires",
            .Font = New Font("Segoe UI", 17, FontStyle.Bold),
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft,
            .ForeColor = ThemePricieux.Foret,
            .Margin = Padding.Empty
        }
        disposition.Controls.Add(titre, 0, 0)

        Dim barre As New FlowLayoutPanel With {
            .Name = "barreFiltresHistorique",
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .Padding = New Padding(5),
            .Margin = New Padding(0, 0, 0, 8),
            .WrapContents = True,
            .AutoScroll = True
        }
        disposition.Controls.Add(barre, 0, 1)
        cmbProduitHistorique.Name = "cmbProduitHistorique"
        cmbProduitHistorique.DropDownStyle = ComboBoxStyle.DropDownList
        cmbProduitHistorique.Width = 310
        cmbProduitHistorique.FormattingEnabled = True
        AddHandler cmbProduitHistorique.Format, AddressOf FormaterProduit
        AddHandler cmbProduitHistorique.SelectedIndexChanged, AddressOf HistoriqueSelectionModifiee
        barre.Controls.Add(CreerEtiquette("Produit", True))
        barre.Controls.Add(cmbProduitHistorique)
        cmbMagasinHistorique.Name = "cmbMagasinHistorique"
        cmbMagasinHistorique.DropDownStyle = ComboBoxStyle.DropDownList
        cmbMagasinHistorique.Width = 190
        AddHandler cmbMagasinHistorique.SelectedIndexChanged, AddressOf HistoriqueSelectionModifiee
        barre.Controls.Add(CreerEtiquette("Magasin", True))
        barre.Controls.Add(cmbMagasinHistorique)
        Dim btnAfficher As Button = CreerBouton("Afficher", True)
        AddHandler btnAfficher.Click, AddressOf ChargerHistoriqueAsync
        barre.Controls.Add(btnAfficher)

        ConfigurerGrille(dgvHistorique)
        dgvHistorique.Dock = DockStyle.Fill
        dgvHistorique.Margin = New Padding(0, 0, 6, 0)
        dgvHistorique.Columns.Add("date", "Date")
        dgvHistorique.Columns.Add("magasin", "Magasin")
        dgvHistorique.Columns.Add("conditionnement", "Conditionnement")
        dgvHistorique.Columns.Add("prix", "Prix unitaire")
        dgvHistorique.Columns(0).Width = 105
        dgvHistorique.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvHistorique.Columns(2).Width = 135
        dgvHistorique.Columns(3).Width = 125

        pnlGraphique.Name = "pnlGraphiqueHistorique"
        pnlGraphique.Dock = DockStyle.Fill
        pnlGraphique.Margin = New Padding(6, 0, 0, 0)
        pnlGraphique.BackColor = Color.White
        AddHandler pnlGraphique.Paint, AddressOf DessinerHistorique

        Dim contenu As New TableLayoutPanel With {
            .Name = "contenuHistorique",
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 1,
            .Margin = Padding.Empty,
            .Padding = Padding.Empty
        }
        contenu.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        contenu.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        contenu.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        contenu.Controls.Add(dgvHistorique, 0, 0)
        contenu.Controls.Add(pnlGraphique, 1, 0)
        disposition.Controls.Add(contenu, 0, 2)
    End Sub

    Private Sub HistoriqueSelectionModifiee(sender As Object, e As EventArgs)
        _historiqueCharge = False
        _observationsAffichees.Clear()
        dgvHistorique.Rows.Clear()
        pnlGraphique.Invalidate()
    End Sub

    Private Sub ConstruireOngletAlertes()
        Dim page As New TabPage("Alertes") With {.BackColor = ThemePricieux.FondPage, .Padding = New Padding(18)}
        _onglets.TabPages.Add(page)
        page.Controls.Add(New Label With {.Text = "Alertes de prix", .Font = New Font("Segoe UI", 17, FontStyle.Bold), .AutoSize = True, .Location = New Point(20, 18), .ForeColor = ThemePricieux.Foret})
        Dim barre As New FlowLayoutPanel With {.Location = New Point(22, 64), .Size = New Size(1190, 78), .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right, .BackColor = Color.White, .Padding = New Padding(8), .WrapContents = True}
        page.Controls.Add(barre)
        dtpDebutAlerte.Name = "dtpDebutAlerte"
        dtpDebutAlerte.Format = DateTimePickerFormat.Short
        dtpDebutAlerte.Value = Date.Today.AddMonths(-3)
        barre.Controls.Add(CreerEtiquette("Du", True))
        barre.Controls.Add(dtpDebutAlerte)
        dtpFinAlerte.Name = "dtpFinAlerte"
        dtpFinAlerte.Format = DateTimePickerFormat.Short
        dtpFinAlerte.Value = Date.Today
        barre.Controls.Add(CreerEtiquette("Au", True))
        barre.Controls.Add(dtpFinAlerte)
        cmbMagasinAlerte.Name = "cmbMagasinAlerte"
        cmbMagasinAlerte.DropDownStyle = ComboBoxStyle.DropDownList
        cmbMagasinAlerte.Width = 180
        barre.Controls.Add(cmbMagasinAlerte)
        cmbTypeAlerte.Name = "cmbTypeAlerte"
        cmbTypeAlerte.DropDownStyle = ComboBoxStyle.DropDownList
        cmbTypeAlerte.Items.AddRange(New Object() {"Tous", "HAUSSE", "SHRINKFLATION"})
        cmbTypeAlerte.SelectedIndex = 0
        barre.Controls.Add(cmbTypeAlerte)
        chkNonLues.Name = "chkNonLues"
        chkNonLues.Text = "Non lues uniquement"
        chkNonLues.AutoSize = True
        chkNonLues.Margin = New Padding(8, 12, 5, 4)
        barre.Controls.Add(chkNonLues)
        Dim btnFiltrer As Button = CreerBouton("Filtrer", True)
        AddHandler btnFiltrer.Click, AddressOf ChargerAlertesAsync
        barre.Controls.Add(btnFiltrer)
        Dim btnLue As Button = CreerBouton("Marquer comme lue", False)
        AddHandler btnLue.Click, AddressOf MarquerAlerteLueAsync
        barre.Controls.Add(btnLue)

        ConfigurerGrille(dgvAlertes)
        dgvAlertes.Location = New Point(22, 158)
        dgvAlertes.Size = New Size(1190, 510)
        dgvAlertes.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        page.Controls.Add(dgvAlertes)
        dgvAlertes.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "id", .HeaderText = "Id", .Visible = False})
        dgvAlertes.Columns.Add("date", "Date")
        dgvAlertes.Columns.Add("type", "Type")
        dgvAlertes.Columns.Add("produit", "Produit")
        dgvAlertes.Columns.Add("magasin", "Magasin")
        dgvAlertes.Columns.Add("variation", "Variation prix unitaire")
        dgvAlertes.Columns.Add("statut", "Statut")
        dgvAlertes.Columns("date").Width = 150
        dgvAlertes.Columns("type").Width = 150
        dgvAlertes.Columns("produit").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvAlertes.Columns("magasin").Width = 170
        dgvAlertes.Columns("variation").Width = 190
        dgvAlertes.Columns("statut").Width = 100
    End Sub

    Private Async Sub ChargerHistoriqueAsync(sender As Object, e As EventArgs)
        Try
            If cmbProduitHistorique.SelectedItem Is Nothing Then Throw New InvalidOperationException("Sélectionnez un produit.")
            Dim produit As Produit = DirectCast(cmbProduitHistorique.SelectedItem, Produit)
            Dim magasin As Magasin = TryCast(cmbMagasinHistorique.SelectedItem, Magasin)
            _observationsAffichees = Await _ticketDepot.ListerHistoriqueAsync(_utilisateur.Id, produit.Id, If(magasin Is Nothing, CType(Nothing, Long?), magasin.Id))
            _historiqueCharge = True
            dgvHistorique.Rows.Clear()
            For Each observation As ObservationPrix In _observationsAffichees
                dgvHistorique.Rows.Add(observation.DateObservation.ToString("dd/MM/yyyy"), observation.MagasinNom,
                    observation.QuantiteBase.ToString("0.####") & " " & UniteEnTexte(produit.UniteBase),
                    observation.PrixUnitaire.ToString("N2") & " / " & UniteEnTexte(produit.UniteBase))
            Next
            pnlGraphique.Invalidate()
        Catch ex As Exception
            AfficherErreur("Historique indisponible", ex)
        End Try
    End Sub

    Private Sub DessinerHistorique(sender As Object, e As PaintEventArgs)
        Dim graph As Graphics = e.Graphics
        graph.SmoothingMode = SmoothingMode.AntiAlias
        graph.Clear(Color.White)
        If _observationsAffichees.Count = 0 Then
            Dim message As String = If(_historiqueCharge,
                "Aucun achat enregistré pour cette sélection. Enregistrez un ticket contenant ce produit, puis cliquez sur Afficher.",
                "Choisissez un produit, puis cliquez sur Afficher. L’historique est alimenté par les tickets enregistrés.")
            Dim zoneMessage As Rectangle = Rectangle.Inflate(pnlGraphique.ClientRectangle, -20, -20)
            TextRenderer.DrawText(graph, message, Font, zoneMessage, Color.DimGray, TextFormatFlags.WordBreak Or TextFormatFlags.VerticalCenter)
            Return
        End If
        Dim margeGauche As Single = 72.0F
        Dim margeDroite As Single = 24.0F
        Dim margeHaut As Single = 36.0F
        Dim margeBas As Single = 52.0F
        Dim zone As New RectangleF(margeGauche, margeHaut, Math.Max(1.0F, pnlGraphique.ClientSize.Width - margeGauche - margeDroite), Math.Max(1.0F, pnlGraphique.ClientSize.Height - margeHaut - margeBas))
        Dim minimum As Decimal = _observationsAffichees.Min(Function(item) item.PrixUnitaire)
        Dim maximum As Decimal = _observationsAffichees.Max(Function(item) item.PrixUnitaire)
        If maximum = minimum Then maximum += 1D
        Dim dateMinimum As Date = _observationsAffichees.Min(Function(item) item.DateObservation)
        Dim dateMaximum As Date = _observationsAffichees.Max(Function(item) item.DateObservation)
        If dateMaximum = dateMinimum Then dateMaximum = dateMinimum.AddDays(1)
        Dim couleurs As Color() = {ThemePricieux.Foret, Color.FromArgb(218, 105, 50), Color.FromArgb(50, 144, 101), Color.FromArgb(137, 92, 175), Color.FromArgb(194, 154, 45)}
        Using crayonAxe As New Pen(ThemePricieux.VertPale, 1.0F),
              police As New Font("Segoe UI", 8)
            graph.DrawLine(crayonAxe, zone.Left, zone.Bottom, zone.Right, zone.Bottom)
            graph.DrawLine(crayonAxe, zone.Left, zone.Top, zone.Left, zone.Bottom)
            Dim indexGroupe As Integer = 0
            For Each groupe In _observationsAffichees.GroupBy(Function(item) item.MagasinNom).OrderBy(Function(item) item.Key)
                Dim couleur As Color = couleurs(indexGroupe Mod couleurs.Length)
                Using crayonLigne As New Pen(couleur, 2.5F), pinceauPoint As New SolidBrush(couleur)
                    Dim observations As List(Of ObservationPrix) = groupe.OrderBy(Function(item) item.DateObservation).ThenBy(Function(item) item.Id).ToList()
                    Dim points As New List(Of PointF)()
                    For Each observation As ObservationPrix In observations
                        Dim ratioX As Single = CSng((observation.DateObservation - dateMinimum).TotalSeconds / (dateMaximum - dateMinimum).TotalSeconds)
                        Dim ratioY As Single = CSng((observation.PrixUnitaire - minimum) / (maximum - minimum))
                        points.Add(New PointF(zone.Left + ratioX * zone.Width, zone.Bottom - ratioY * zone.Height))
                    Next
                    If points.Count > 1 Then graph.DrawLines(crayonLigne, points.ToArray())
                    For Each point As PointF In points
                        graph.FillEllipse(pinceauPoint, point.X - 3.5F, point.Y - 3.5F, 7.0F, 7.0F)
                    Next
                    Dim legende As String = groupe.Key
                    TextRenderer.DrawText(graph, legende, police, New Point(CInt(zone.Left + indexGroupe Mod 3 * 165), 8 + indexGroupe \ 3 * 17), couleur)
                End Using
                indexGroupe += 1
            Next
            TextRenderer.DrawText(graph, maximum.ToString("N2") & " / unité", police, New Point(CInt(zone.Left + 4), CInt(zone.Top)), Color.DimGray)
            TextRenderer.DrawText(graph, minimum.ToString("N2"), police, New Point(CInt(zone.Left + 4), CInt(zone.Bottom - 18)), Color.DimGray)
            TextRenderer.DrawText(graph, dateMinimum.ToString("dd/MM/yy"), police, New Point(CInt(zone.Left), CInt(zone.Bottom + 10)), Color.DimGray)
            TextRenderer.DrawText(graph, dateMaximum.ToString("dd/MM/yy"), police, New Point(CInt(zone.Right - 65), CInt(zone.Bottom + 10)), Color.DimGray)
        End Using
    End Sub

    Private Async Sub ChargerAlertesAsync(sender As Object, e As EventArgs)
        Try
            If dtpFinAlerte.Value.Date < dtpDebutAlerte.Value.Date Then Throw New InvalidOperationException("La période de fin précède la date de début.")
            Dim magasin As Magasin = TryCast(cmbMagasinAlerte.SelectedItem, Magasin)
            Dim magasinId As Long? = If(magasin Is Nothing, CType(Nothing, Long?), magasin.Id)
            Dim type As String = Convert.ToString(cmbTypeAlerte.SelectedItem, CultureInfo.CurrentCulture)
            _alertesAffichees = Await _alertesDepot.ListerAsync(_utilisateur.Id, dtpDebutAlerte.Value.Date, dtpFinAlerte.Value.Date, magasinId, type, chkNonLues.Checked)
            dgvAlertes.Rows.Clear()
            For Each alerte As AlertePrix In _alertesAffichees
                dgvAlertes.Rows.Add(alerte.Id, alerte.DateAlerte.ToString("dd/MM/yyyy HH:mm"), alerte.Type,
                    alerte.ProduitNom & If(String.IsNullOrWhiteSpace(alerte.Marque), String.Empty, " — " & alerte.Marque),
                    alerte.MagasinNom, (If(alerte.Valeur >= 0D, "+", String.Empty) & alerte.Valeur.ToString("N2") & " %"), If(alerte.Lue, "Lue", "Non lue"))
            Next
        Catch ex As Exception
            AfficherErreur("Alertes indisponibles", ex)
        End Try
    End Sub

    Private Async Sub MarquerAlerteLueAsync(sender As Object, e As EventArgs)
        Try
            If dgvAlertes.CurrentRow Is Nothing Then Throw New InvalidOperationException("Sélectionnez une alerte.")
            Dim id As Long = Convert.ToInt64(dgvAlertes.CurrentRow.Cells("id").Value)
            Await _alertesDepot.MarquerLueAsync(_utilisateur.Id, id)
            Await RafraichirAccueilAsyncTask()
            ChargerAlertesAsync(sender, e)
        Catch ex As Exception
            AfficherErreur("Modification impossible", ex)
        End Try
    End Sub
End Class
