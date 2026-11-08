Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports MySqlConnector

' =====================================================================
' CSV_Depense.vb : code du formulaire.
' - Onglet "Dépenses récurrentes" : code existant, repris tel quel
'   (d'après votre capture : adaptez-le si vous avez ajouté autre chose).
' - Onglet "Import & Export CSV" : affichage seulement ; le traitement
'   est dans ImportExportService.vb.
' =====================================================================
Public Class CSV_Depense

    ' ----- Partie dépenses (existante, reprise telle quelle) -----
    Private depense As frmDepenses

    Private Sub CSV_Depense_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        depense = New frmDepenses()
        depense.TopLevel = False
        depense.FormBorderStyle = FormBorderStyle.None
        depense.Dock = DockStyle.Fill
        TabDepense.Controls.Add(depense)
        depense.Show()


        If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Return

        LabelResultat.Text = ""

        ' Glisser-déposer : la fenêtre, le TabControl et tout l'onglet d'import
        Brancher(Me)
        Brancher(TabControl1)
        ActiverGlisserDeposer(TabImportExport)

        Try
            ChargerListesExport()
        Catch ex As Exception
            MessageBox.Show("Impossible de charger les listes d'export : " & ex.Message,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    ' ================= Glisser-déposer =================

    Private Sub Brancher(ctrl As Control)
        ctrl.AllowDrop = True
        AddHandler ctrl.DragEnter, AddressOf Glisser_DragEnter
        AddHandler ctrl.DragDrop, AddressOf Glisser_DragDrop
    End Sub

    ' Un contrôle qui n'a pas AllowDrop refuse le dépôt : on branche donc aussi les enfants
    Private Sub ActiverGlisserDeposer(ctrl As Control)
        If TypeOf ctrl Is ComboBox Then Return
        Brancher(ctrl)
        For Each enfant As Control In ctrl.Controls
            ActiverGlisserDeposer(enfant)
        Next
    End Sub

    Private Sub Glisser_DragEnter(sender As Object, e As DragEventArgs)
        If e.Data IsNot Nothing AndAlso e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub Glisser_DragDrop(sender As Object, e As DragEventArgs)
        Dim fichiers As String() = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
        If fichiers Is Nothing OrElse fichiers.Length = 0 Then Return

        ' Un seul fichier à la fois : on prend le premier .csv
        Dim csv As String = fichiers.FirstOrDefault(
            Function(f) String.Equals(Path.GetExtension(f), ".csv", StringComparison.OrdinalIgnoreCase))
        If csv Is Nothing Then
            MessageBox.Show("Déposez un fichier .csv.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        TabControl1.SelectedTab = TabImportExport
        ' BeginInvoke : l'import démarre après la fin du dépôt (l'Explorateur ne reste pas figé)
        BeginInvoke(New Action(Sub() LancerImport(csv)))
    End Sub

    ' ================= Import =================

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles BtnImport.Click
        Using dlg As New OpenFileDialog()
            dlg.Title = "Choisir un fichier CSV"
            dlg.Filter = "Fichiers CSV (*.csv)|*.csv|Tous les fichiers (*.*)|*.*"
            If dlg.ShowDialog() <> DialogResult.OK Then Return
            LancerImport(dlg.FileName)
        End Using
    End Sub

    Private Sub LancerImport(chemin As String)
        If Session.user Is Nothing Then
            MessageBox.Show("Aucun utilisateur connecté.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim resultat As ResultatImport = Nothing
        Try
            Me.Cursor = Cursors.WaitCursor
            resultat = ImportExportService.ImporterReleves(chemin, Session.user.Id)
        Catch ex As Exception
            MessageBox.Show("Import impossible : " & ex.Message & vbCrLf & "Aucune donnée n'a été enregistrée.",
                            "Erreur d'import", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            Me.Cursor = Cursors.Default
        End Try

        LabelResultat.ForeColor = If(resultat.Erreurs.Count = 0, Color.SeaGreen, Color.Firebrick)
        LabelResultat.Text = resultat.Importees.ToString(CultureInfo.CurrentCulture) & " ligne(s) importée(s). " &
                             resultat.Erreurs.Count.ToString(CultureInfo.CurrentCulture) & " erreur(s)"

        If resultat.Erreurs.Count > 0 Then
            Dim details As String = String.Join(Environment.NewLine, resultat.Erreurs.Take(12))
            If resultat.Erreurs.Count > 12 Then details &= Environment.NewLine & "… et " & (resultat.Erreurs.Count - 12).ToString(CultureInfo.CurrentCulture) & " autre(s)."
            MessageBox.Show(details, "Lignes rejetées", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ' ================= Export =================

    Private Sub ChargerListesExport()
        Dim produits As DataTable = ImportExportService.ListerProduits()
        Dim magasinsBase As DataTable = ImportExportService.ListerMagasins()

        ' "Tous les magasins" = id 0
        Dim magasins As New DataTable()
        magasins.Columns.Add("id", GetType(Integer))
        magasins.Columns.Add("nom", GetType(String))
        magasins.Rows.Add(0, "Tous les magasins")
        For Each r As DataRow In magasinsBase.Rows
            magasins.Rows.Add(Convert.ToInt32(r("id")), Convert.ToString(r("nom")))
        Next

        CmBProduit.DropDownStyle = ComboBoxStyle.DropDownList
        CmBProduit.DisplayMember = "libelle"
        CmBProduit.ValueMember = "id"
        CmBProduit.DataSource = produits
        CmBProduit.SelectedIndex = -1

        CmBMagasins.Items.Clear()
        CmBMagasins.DropDownStyle = ComboBoxStyle.DropDownList
        CmBMagasins.DisplayMember = "nom"
        CmBMagasins.ValueMember = "id"
        CmBMagasins.DataSource = magasins
        CmBMagasins.SelectedIndex = 0
    End Sub

    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExport.Click
        If Session.user Is Nothing Then
            MessageBox.Show("Aucun utilisateur connecté.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        If CmBProduit.SelectedValue Is Nothing Then
            MessageBox.Show("Choisissez d'abord un produit.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim idProduit As Integer = Convert.ToInt32(CmBProduit.SelectedValue)
        Dim idMagasin As Integer = If(CmBMagasins.SelectedValue Is Nothing, 0, Convert.ToInt32(CmBMagasins.SelectedValue))

        Dim dt As DataTable
        Try
            dt = ImportExportService.LireHistorique(Session.user.Id, idProduit, idMagasin)
        Catch ex As MySqlException
            MessageBox.Show("Impossible de lire l'historique : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        If dt.Rows.Count = 0 Then
            MessageBox.Show("Aucun relevé de prix pour ce produit.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim nomFichier As String = "historique_" & CmBProduit.Text & "_" & Date.Today.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        For Each c As Char In Path.GetInvalidFileNameChars()
            nomFichier = nomFichier.Replace(c, "_"c)
        Next

        Using dlg As New SaveFileDialog()
            dlg.Title = "Enregistrer l'historique"
            dlg.Filter = "Fichier CSV (*.csv)|*.csv"
            dlg.DefaultExt = "csv"
            dlg.FileName = nomFichier
            If dlg.ShowDialog() <> DialogResult.OK Then Return

            Try
                ImportExportService.EcrireCsv(dt, dlg.FileName)
            Catch ex As IOException
                MessageBox.Show("Impossible d'écrire le fichier (est-il ouvert dans Excel ?) : " & ex.Message,
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
        End Using

        MessageBox.Show(dt.Rows.Count.ToString(CultureInfo.CurrentCulture) & " ligne(s) exportée(s).",
                        "Export terminé", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Class