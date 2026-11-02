Option Strict On
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public NotInheritable Partial Class MainForm
    Inherits Form

    Private ReadOnly _utilisateur As UtilisateurSession
    Private ReadOnly _referentiels As New ReferentielsDepot()
    Private ReadOnly _ticketDepot As New TicketDepot()
    Private ReadOnly _alertesDepot As New AlertesDepot()
    Private ReadOnly _analysesDepot As New AnalysesDepot()
    Private ReadOnly _depensesDepot As New DepensesDepot()
    Private ReadOnly _detecteur As New DetectionPrixService()
    Private ReadOnly _ticketService As TicketService
    Private ReadOnly _importExport As ImportExportService
    Private ReadOnly _layoutPrincipal As New TableLayoutPanel()
    Private ReadOnly _onglets As New TabControl()
    Private ReadOnly lblUtilisateur As New Label()
    Private ReadOnly lblAlertes As New Label()
    Private ReadOnly _timerRappels As New Timer()
    Private _dateDernierRappel As Date?
    Public Property DemandeDeconnexion As Boolean

    Private ReadOnly lblDepenseMois As New Label()
    Private ReadOnly lblAlertesNonLues As New Label()
    Private ReadOnly lblIndiceCourant As New Label()
    Private ReadOnly dgvDepensesCategorie As New DataGridView()
    Private ReadOnly dgvRappelsAccueil As New DataGridView()

    Public Sub New(utilisateur As UtilisateurSession)
        If utilisateur Is Nothing Then Throw New ArgumentNullException(NameOf(utilisateur))
        If utilisateur.Id <= 0 OrElse Not utilisateur.Actif Then Throw New UnauthorizedAccessException("Connectez-vous avec un compte actif avant d'ouvrir l'application.")
        _utilisateur = utilisateur
        _ticketService = New TicketService(_ticketDepot, _detecteur)
        _importExport = New ImportExportService(_referentiels, _ticketService, _ticketDepot)
        Text = "Pricieux — " & ThemePricieux.Slogan
        Name = "frmPrincipal"
        StartPosition = FormStartPosition.CenterScreen
        MinimumSize = New Size(1080, 720)
        Size = New Size(1360, 850)
        BackColor = ThemePricieux.FondPage
        Font = New Font("Segoe UI", 9)
        AllowDrop = True

        _layoutPrincipal.Name = "layoutPrincipal"
        _layoutPrincipal.Dock = DockStyle.Fill
        _layoutPrincipal.ColumnCount = 1
        _layoutPrincipal.RowCount = 2
        _layoutPrincipal.Margin = Padding.Empty
        _layoutPrincipal.Padding = Padding.Empty
        _layoutPrincipal.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        _layoutPrincipal.RowStyles.Add(New RowStyle(SizeType.Absolute, 100.0F))
        _layoutPrincipal.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Controls.Add(_layoutPrincipal)

        _onglets.Name = "tabsPrincipal"
        _onglets.Dock = DockStyle.Fill
        _onglets.Padding = New Point(16, 8)
        _onglets.DrawMode = TabDrawMode.OwnerDrawFixed
        _onglets.ItemSize = New Size(120, 30)
        AddHandler _onglets.DrawItem, AddressOf ThemePricieux.DessinerOnglet
        _layoutPrincipal.Controls.Add(_onglets, 0, 1)
        ConstruireOngletAccueil()
        ConstruireOngletTickets()
        ConstruireOngletHistorique()
        ConstruireOngletAlertes()
        ConstruireOngletAnalyses()
        ConstruireOngletReferentiels()
        ConstruireOngletImportEtDepenses()
        If _onglets.TabPages.Count > 0 Then _onglets.SelectedIndex = 0
        _layoutPrincipal.Controls.Add(ConstruireEntete(), 0, 0)

        AddHandler Load, AddressOf ChargerInitialAsync
        AddHandler FormClosed, Sub(sender As Object, args As FormClosedEventArgs) _timerRappels.Stop()
        AddHandler DragEnter, AddressOf FenetreDragEnter
        AddHandler DragDrop, AddressOf FenetreDragDrop
        AddHandler _detecteur.AlerteDetectee, AddressOf AlerteDetectee
        _timerRappels.Interval = CInt(TimeSpan.FromHours(6).TotalMilliseconds)
        AddHandler _timerRappels.Tick, Async Sub(sender As Object, args As EventArgs) Await VerifierRappelsAsync()
        _timerRappels.Start()
    End Sub

    Private Function ConstruireEntete() As Panel
        Dim pnlEntete As New Panel With {.Dock = DockStyle.Fill, .BackColor = ThemePricieux.Foret}
        Dim dispositionEntete As New TableLayoutPanel With {
            .Name = "layoutEntete",
            .Dock = DockStyle.Fill,
            .ColumnCount = 4,
            .RowCount = 1,
            .Padding = New Padding(26, 7, 26, 7),
            .Margin = Padding.Empty,
            .BackColor = ThemePricieux.Foret
        }
        dispositionEntete.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 45.0F))
        dispositionEntete.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 18.0F))
        dispositionEntete.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0F))
        dispositionEntete.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 17.0F))
        dispositionEntete.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        pnlEntete.Controls.Add(dispositionEntete)

        Dim panneauMarque As New Panel With {.Dock = DockStyle.Fill, .Margin = Padding.Empty, .BackColor = ThemePricieux.Foret}
        Dim lblTitre As New Label With {.Text = "Pricieux", .Font = New Font("Georgia", 29, FontStyle.Bold), .ForeColor = Color.White, .AutoSize = True, .Location = New Point(3, 0)}
        panneauMarque.Controls.Add(lblTitre)
        Dim lblSousTitre As New Label With {.Text = ThemePricieux.Slogan, .Font = New Font("Georgia", 10.5F, FontStyle.Regular), .ForeColor = Color.White, .AutoSize = True, .Location = New Point(2, 53)}
        panneauMarque.Controls.Add(lblSousTitre)
        dispositionEntete.Controls.Add(panneauMarque, 0, 0)

        lblUtilisateur.AutoSize = False
        lblUtilisateur.Dock = DockStyle.Fill
        lblUtilisateur.Margin = New Padding(4, 0, 4, 0)
        lblUtilisateur.ForeColor = Color.White
        lblUtilisateur.Font = New Font("Georgia", 11.0F, FontStyle.Regular)
        lblUtilisateur.AutoEllipsis = True
        lblUtilisateur.TextAlign = ContentAlignment.MiddleCenter
        lblUtilisateur.Text = _utilisateur.Login & "." & If(_utilisateur.EstAdministrateur, "Admin", "Utilisateur")
        dispositionEntete.Controls.Add(lblUtilisateur, 1, 0)

        lblAlertes.AutoSize = False
        lblAlertes.Dock = DockStyle.Fill
        lblAlertes.Margin = New Padding(4, 0, 4, 0)
        lblAlertes.ForeColor = Color.FromArgb(255, 232, 0)
        lblAlertes.Font = New Font("Georgia", 12.0F, FontStyle.Regular)
        lblAlertes.TextAlign = ContentAlignment.MiddleCenter
        lblAlertes.Text = "Alerte non lues : —"
        dispositionEntete.Controls.Add(lblAlertes, 2, 0)

        Dim btnDeconnexion As Button = CreerBouton("Se déconnecter", False)
        btnDeconnexion.Name = "btnDeconnexion"
        btnDeconnexion.Size = New Size(170, 40)
        btnDeconnexion.Anchor = AnchorStyles.Right
        btnDeconnexion.Margin = New Padding(0)
        btnDeconnexion.BackColor = Color.White
        btnDeconnexion.ForeColor = ThemePricieux.Foret
        btnDeconnexion.Font = New Font("Georgia", 13.0F, FontStyle.Regular)
        btnDeconnexion.FlatStyle = FlatStyle.Flat
        btnDeconnexion.FlatAppearance.BorderColor = Color.White
        AddHandler btnDeconnexion.Click, Sub(sender As Object, args As EventArgs)
                                             DemandeDeconnexion = True
                                             Close()
                                         End Sub
        dispositionEntete.Controls.Add(btnDeconnexion, 3, 0)
        Return pnlEntete
    End Function

    Private Sub ConstruireOngletAccueil()
        Dim page As New TabPage("Accueil") With {.BackColor = ThemePricieux.FondPage, .Padding = New Padding(18)}
        _onglets.TabPages.Add(page)
        Dim lblIntro As New Label With {.Text = "Tableau de bord", .Font = New Font("Segoe UI", 18, FontStyle.Bold), .ForeColor = ThemePricieux.Foret, .AutoSize = True, .Location = New Point(20, 20)}
        page.Controls.Add(lblIntro)
        Dim btnActualiser As Button = CreerBouton("Actualiser", True)
        btnActualiser.Location = New Point(1050, 18)
        btnActualiser.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        AddHandler btnActualiser.Click, Async Sub(sender As Object, args As EventArgs) Await RafraichirAccueilAsync()
        page.Controls.Add(btnActualiser)

        AjouterCarte(page, "Dépenses du mois", lblDepenseMois, "20 000 FCFA", 20)
        AjouterCarte(page, "Alertes non lues", lblAlertesNonLues, "0", 365)
        AjouterCarte(page, "Indice personnel", lblIndiceCourant, "100,00", 710)

        Dim lblCategories As New Label With {.Text = "Dépenses par catégorie — mois en cours", .Font = New Font("Segoe UI", 12, FontStyle.Bold), .AutoSize = True, .Location = New Point(26, 220)}
        page.Controls.Add(lblCategories)
        ConfigurerGrille(dgvDepensesCategorie)
        dgvDepensesCategorie.Location = New Point(24, 252)
        dgvDepensesCategorie.Size = New Size(600, 380)
        dgvDepensesCategorie.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        page.Controls.Add(dgvDepensesCategorie)
        dgvDepensesCategorie.Columns.Add("categorie", "Catégorie")
        dgvDepensesCategorie.Columns.Add("montant", "Montant")
        dgvDepensesCategorie.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvDepensesCategorie.Columns(1).Width = 150

        Dim lblRappels As New Label With {.Text = "Échéances à venir", .Font = New Font("Segoe UI", 12, FontStyle.Bold), .AutoSize = True, .Location = New Point(655, 220)}
        page.Controls.Add(lblRappels)
        ConfigurerGrille(dgvRappelsAccueil)
        dgvRappelsAccueil.Location = New Point(650, 252)
        dgvRappelsAccueil.Size = New Size(540, 380)
        dgvRappelsAccueil.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        page.Controls.Add(dgvRappelsAccueil)
        dgvRappelsAccueil.Columns.Add("libelle", "Dépense")
        dgvRappelsAccueil.Columns.Add("montant", "Montant")
        dgvRappelsAccueil.Columns.Add("echeance", "Échéance")
        dgvRappelsAccueil.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvRappelsAccueil.Columns(1).Width = 110
        dgvRappelsAccueil.Columns(2).Width = 120
    End Sub

    Private Shared Sub AjouterCarte(parent As Control, titre As String, valeur As Label, exemple As String, x As Integer)
        Dim carte As New Panel With {.Location = New Point(x, 76), .Size = New Size(320, 118), .BackColor = Color.White, .Padding = New Padding(16)}
        carte.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        parent.Controls.Add(carte)
        carte.Controls.Add(New Label With {.Text = titre, .Location = New Point(16, 16), .AutoSize = True, .ForeColor = Color.DimGray, .Font = New Font("Segoe UI", 10)})
        valeur.Text = exemple
        valeur.Location = New Point(16, 48)
        valeur.AutoSize = True
        valeur.ForeColor = ThemePricieux.Foret
        valeur.Font = New Font("Segoe UI", 23, FontStyle.Bold)
        carte.Controls.Add(valeur)
    End Sub

    Private Async Sub ChargerInitialAsync(sender As Object, e As EventArgs)
        Try
            Await RafraichirToutesLesDonneesAsync()
            Await ChargerAdministrationAsync()
            Await RafraichirAccueilAsync()
            Await ChargerDepensesRecurrentesAsyncTask()
            Await VerifierRappelsAsync()
        Catch ex As Exception
            AfficherErreur("Initialisation de l'application impossible", ex)
        End Try
    End Sub

    Private Async Function RafraichirAccueilAsync() As Task
        Try
            Dim maintenant As Date = Date.Today
            Dim depenses As List(Of DepenseCategorie) = Await _analysesDepot.DepensesParCategorieAsync(_utilisateur.Id, maintenant.Year, maintenant.Month)
            dgvDepensesCategorie.Rows.Clear()
            Dim total As Decimal = 0D
            For Each depense As DepenseCategorie In depenses
                dgvDepensesCategorie.Rows.Add(depense.CategorieNom, depense.Montant.ToString("N0") & " FCFA")
                total += depense.Montant
            Next
            lblDepenseMois.Text = total.ToString("N0") & " FCFA"
            Dim nonLues As Integer = Await _alertesDepot.CompterNonLuesAsync(_utilisateur.Id)
            lblAlertesNonLues.Text = nonLues.ToString(Globalization.CultureInfo.CurrentCulture)
            lblAlertes.Text = "Alerte non lues : " & nonLues.ToString(Globalization.CultureInfo.CurrentCulture)
            Dim debut As New Date(maintenant.Year, maintenant.Month, 1)
            If maintenant > debut Then
                Dim indice As ResultatIndice = Await _analysesDepot.CalculerIndiceAsync(_utilisateur.Id, debut, maintenant)
                lblIndiceCourant.Text = If(indice.Indice.HasValue, indice.Indice.Value.ToString("N2"), "—")
            Else
                lblIndiceCourant.Text = "—"
            End If
        Catch ex As Exception
            AfficherErreur("Impossible d'actualiser le tableau de bord", ex)
        End Try
    End Function

    Private Async Function RafraichirToutesLesDonneesAsync() As Task
        Try
            Dim magasins = Await _referentiels.ListerMagasinsAsync()
            Dim categories = Await _referentiels.ListerCategoriesAsync()
            Dim produits = Await _referentiels.ListerProduitsAsync()
            MettreAJourDonneesReferentiel(magasins, categories, produits)
        Catch ex As Exception
            AfficherErreur("Connexion à la base de données impossible", ex)
        End Try
    End Function

    Private Async Function VerifierRappelsAsync() As Task
        Try
            Dim depenses = Await _depensesDepot.ListerAsync(_utilisateur.Id)
            Dim dateAujourdhui As Date = Date.Today
            Dim imminentes As System.Collections.Generic.List(Of DepenseRecurrente) =
                depenses.FindAll(Function(item) item.ProchaineEcheance.Date <= dateAujourdhui.AddDays(2))
            If imminentes.Count > 0 AndAlso _timerRappels.Enabled AndAlso _dateDernierRappel <> dateAujourdhui Then
                _dateDernierRappel = dateAujourdhui
                Dim details As New System.Collections.Generic.List(Of String)()
                For index As Integer = 0 To Math.Min(4, imminentes.Count - 1)
                    Dim imminente As DepenseRecurrente = imminentes(index)
                    Dim typeRappel As String = If(imminente.ProchaineEcheance.Date < dateAujourdhui, "dépassée", If(imminente.ProchaineEcheance.Date = dateAujourdhui, "aujourd'hui", "proche"))
                    details.Add("• " & imminente.Libelle & " — " & imminente.ProchaineEcheance.ToString("dd/MM/yyyy") & " (" & typeRappel & ")")
                Next
                If imminentes.Count > details.Count Then details.Add("et " & (imminentes.Count - details.Count).ToString() & " autre(s)")
                MessageBox.Show(String.Join(Environment.NewLine, details) & Environment.NewLine & Environment.NewLine & "Les échéances dépassées sont avancées selon leur périodicité après ce rappel. Aucun paiement n'est déclenché.", "Rappel de dépense", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            Dim avancees As System.Collections.Generic.List(Of DepenseRecurrente) =
                Await _depensesDepot.AvancerEcheancesDepasseesAsync(_utilisateur.Id, dateAujourdhui)
            If avancees.Count > 0 Then
                ActualiserEcheancesDansGrilleDepenses(avancees)
                depenses = Await _depensesDepot.ListerAsync(_utilisateur.Id)
            End If

            dgvRappelsAccueil.Rows.Clear()
            Dim proches = depenses.FindAll(Function(item) item.ProchaineEcheance.Date <= dateAujourdhui.AddDays(7))
            For Each depense As DepenseRecurrente In proches
                dgvRappelsAccueil.Rows.Add(depense.Libelle, depense.Montant.ToString("N0") & " FCFA", depense.ProchaineEcheance.ToString("dd/MM/yyyy"))
            Next
        Catch ex As Exception
            If IsHandleCreated Then AfficherErreur("Impossible de vérifier les rappels", ex)
        End Try
    End Function

    Private Async Sub AlerteDetectee(sender As Object, e As AlerteDetecteeEventArgs)
        lblAlertes.Text = "Nouvelle alerte : " & e.Alerte.ProduitNom
        Try
            Dim nonLues As Integer = Await _alertesDepot.CompterNonLuesAsync(_utilisateur.Id)
            lblAlertes.Text = "Alerte non lues : " & nonLues.ToString(Globalization.CultureInfo.CurrentCulture)
            lblAlertesNonLues.Text = nonLues.ToString(Globalization.CultureInfo.CurrentCulture)
        Catch
        End Try
    End Sub

    Private Shared Sub ConfigurerGrille(grille As DataGridView)
        grille.BackgroundColor = Color.White
        grille.BorderStyle = BorderStyle.None
        grille.AllowUserToAddRows = False
        grille.AllowUserToDeleteRows = False
        grille.ReadOnly = True
        grille.RowHeadersVisible = False
        grille.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grille.MultiSelect = False
        grille.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        grille.ColumnHeadersDefaultCellStyle.BackColor = ThemePricieux.VertPale
        grille.ColumnHeadersDefaultCellStyle.ForeColor = ThemePricieux.Texte
        grille.EnableHeadersVisualStyles = False
        grille.DefaultCellStyle.SelectionBackColor = ThemePricieux.Selection
        grille.DefaultCellStyle.SelectionForeColor = ThemePricieux.Foret
    End Sub

    Private Shared Function CreerBouton(texte As String, principal As Boolean) As Button
        Return New Button With {
            .Text = texte,
            .AutoSize = True,
            .MinimumSize = New Size(110, 36),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = If(principal, ThemePricieux.Action, Color.White),
            .ForeColor = If(principal, Color.White, ThemePricieux.Foret),
            .Font = New Font("Segoe UI", 9, FontStyle.Bold),
            .Padding = New Padding(8, 3, 8, 3)
        }
    End Function

    Private Shared Function CreerEtiquette(texte As String, Optional gras As Boolean = False) As Label
        Return New Label With {.Text = texte, .AutoSize = True, .Font = New Font("Segoe UI", 9, If(gras, FontStyle.Bold, FontStyle.Regular)), .ForeColor = ThemePricieux.TexteSecondaire, .Margin = New Padding(7, 10, 5, 4)}
    End Function

    Private Shared Sub AfficherErreur(titre As String, exception As Exception)
        MessageBox.Show(titre & ":" & Environment.NewLine & exception.Message, "Pricieux", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub FenetreDragEnter(sender As Object, e As DragEventArgs)
        If e.Data IsNot Nothing AndAlso e.Data.GetDataPresent(DataFormats.FileDrop) Then e.Effect = DragDropEffects.Copy
    End Sub

    Private Async Sub FenetreDragDrop(sender As Object, e As DragEventArgs)
        If e.Data Is Nothing OrElse Not e.Data.GetDataPresent(DataFormats.FileDrop) Then Return
        Dim fichiers As String() = DirectCast(e.Data.GetData(DataFormats.FileDrop), String())
        If fichiers.Length = 0 OrElse Not String.Equals(IO.Path.GetExtension(fichiers(0)), ".csv", StringComparison.OrdinalIgnoreCase) Then Return
        Await ImporterFichierAsync(fichiers(0))
    End Sub
End Class
