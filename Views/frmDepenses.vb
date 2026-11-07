' ============================================================================
' FICHIER : Views/frmDepenses.vb  (partie « code » du formulaire)
'           frmDepenses.Designer.vb contient la partie « dessin » ; les deux forment UNE classe.
' RÔLE    : EF-14, dépenses récurrentes (loyer, électricité, abonnements...) avec périodicité
'           et rappel avant l'échéance (7 jours). Sous-onglet « Dépenses récurrentes » de
'           l'onglet « CSV et dépenses » de la maquette. Aucun paiement automatique.
' NOTION  : le formulaire s'abonne à l'événement EcheanceProche de DepenseService
'           (« Handles _service.EcheanceProche ») : chaque rappel émis par le service
'           arrive dans la méthode ci-dessous, qui prépare le texte du bandeau.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Globalization       ' Pour NumberStyles et CultureInfo
Imports PreisRontgen.Data          ' Pour IDepenseRepository
Imports PreisRontgen.Models        ' Pour DepenseRecurrente, Periodicite, RappelEcheance
Imports PreisRontgen.Services      ' Pour DepenseService et FormatageAnalyse

''' <summary>EF-14 : liste, création, modification et rappels des dépenses récurrentes.</summary>
Public Class frmDepenses                           ' Début de la classe (Partial dans le fichier Designer)

    ''' <summary>Ligne affichée dans la grille (liaison aux colonnes par DataPropertyName).</summary>
    Friend Class LigneDepense                      ' Une ligne déjà mise en forme
        Public Property Source As DepenseRecurrente = New DepenseRecurrente()   ' Dépense d'origine (non affichée)
        Public Property Nom As String = String.Empty               ' Colonne « Dépense »
        Public Property MontantTexte As String = String.Empty      ' Colonne « Montant »
        Public Property PeriodiciteTexte As String = String.Empty  ' Colonne « Périodicité »
        Public Property EcheanceTexte As String = String.Empty     ' Colonne « Prochaine échéance »
        Public Property RappelTexte As String = String.Empty       ' Colonne « Rappel »
        Public Property EnAlerte As Boolean                        ' Vrai si l'échéance est proche ou dépassée
    End Class                                      ' Fin de LigneDepense

    Private WithEvents _service As DepenseService  ' Service (WithEvents : on écoute EcheanceProche)
    Private ReadOnly _textesRappel As New List(Of String)()   ' Rappels reçus pendant la vérification
    Private _courante As DepenseRecurrente         ' Dépense affichée dans le formulaire (Nothing = nouvelle)
    Private _chargement As Boolean                 ' Vrai pendant le remplissage (évite les réactions en chaîne)

    ''' <summary>Constructeur sans paramètre, réservé au concepteur de Visual Studio.</summary>
    Public Sub New()                               ' Constructeur du concepteur
        InitializeComponent()                      ' Construit les contrôles
    End Sub                                        ' Fin du constructeur

    Public Sub New(depot As IDepenseRepository)    ' Constructeur utilisé par l'application
        Me.New()                                   ' Construit l'écran
        If depot Is Nothing Then Throw New ArgumentNullException(NameOf(depot))   ' Refuse un dépôt absent
        _service = New DepenseService(depot)       ' Crée le service
    End Sub                                        ' Fin du constructeur

    Private Sub frmDepenses_Load(sender As Object, e As EventArgs) Handles MyBase.Load   ' À l'ouverture
        If DesignMode OrElse _service Is Nothing Then Return   ' Rien à faire dans le concepteur
        cmbPeriodicite.Items.AddRange(New Object() {"Hebdomadaire", "Mensuelle", "Trimestrielle", "Annuelle"})   ' Dans l'ordre de l'énumération
        ViderFormulaire()                          ' Formulaire prêt pour une nouvelle dépense
        Rafraichir()                               ' Affiche les dépenses
    End Sub                                        ' Fin de frmDepenses_Load

    ''' <summary>Recharge la liste et les rappels. Peut être appelée depuis un autre thread.</summary>
    Public Sub Rafraichir()                        ' Recharge l'écran
        If _service Is Nothing Then Return         ' Mode conception
        If InvokeRequired Then                     ' Appelé depuis un autre thread
            Invoke(New Action(AddressOf Rafraichir))   ' Se ré-exécute sur le thread de l'écran
            Return                                 ' La version exécutée sur le bon thread fait le travail
        End If                                     ' Fin du contrôle de thread

        _chargement = True                         ' Le remplissage ne doit pas déclencher SelectionChanged
        Try                                        ' Début du bloc surveillé
            Dim depenses As List(Of DepenseRecurrente) = _service.Lister()   ' Dépenses actives
            _textesRappel.Clear()                  ' Repart de zéro
            Dim rappels As List(Of RappelEcheance) = _service.VerifierRappels(Date.Today)   ' Déclenche EcheanceProche pour chaque rappel
            Dim jours As New Dictionary(Of Integer, Integer)()   ' Jours restants par dépense en alerte
            For Each r As RappelEcheance In rappels   ' Pour chaque rappel
                jours(r.Depense.Id) = r.JoursRestants   ' Mémorise
            Next                                   ' Fin de la boucle

            Dim lignes As New List(Of LigneDepense)(depenses.Count)   ' Lignes du tableau
            For Each d As DepenseRecurrente In depenses   ' Pour chaque dépense
                Dim restants As Integer = 0        ' Jours restants (si en alerte)
                Dim enAlerte As Boolean = jours.TryGetValue(d.Id, restants)   ' Dans la liste des rappels ?
                lignes.Add(New LigneDepense With {
                    .Source = d,                                                      ' Dépense d'origine
                    .Nom = d.Libelle,                                                 ' Nom
                    .MontantTexte = FormatageAnalyse.FormaterMontant(d.Montant) & " FCFA",   ' Montant
                    .PeriodiciteTexte = TexteDe(d.Periodicite),                       ' Fréquence
                    .EcheanceTexte = d.ProchaineEcheance.ToString("d"),               ' Date courte
                    .RappelTexte = If(enAlerte, TexteJours(restants), String.Empty),  ' « dans 3 jours »...
                    .EnAlerte = enAlerte                                              ' Pour la couleur
                })                                 ' Ajoute la ligne
            Next                                   ' Fin de la boucle
            dgvDepenses.DataSource = lignes        ' Affiche le tableau
            dgvDepenses.ClearSelection()           ' Pas de ligne sélectionnée d'office

            If _textesRappel.Count = 0 Then        ' Aucune échéance proche
                lblRappel.Text = "Aucune échéance dans les 7 prochains jours."   ' Message calme
                pnlRappel.BackColor = Color.FromArgb(227, 237, 230)   ' Fond vert clair
                lblRappel.ForeColor = Color.FromArgb(23, 38, 31)      ' Texte sombre
            Else                                   ' Au moins un rappel
                lblRappel.Text = "Rappel : " & String.Join("   ·   ", _textesRappel)   ' Liste des rappels
                pnlRappel.BackColor = Color.FromArgb(253, 243, 214)   ' Fond ambre
                lblRappel.ForeColor = Color.FromArgb(122, 51, 0)      ' Texte brun
            End If                                 ' Fin du choix
        Catch ex As InvalidOperationException      ' Erreur d'accès aux données
            MessageBox.Show(ex.Message, "Dépenses récurrentes", MessageBoxButtons.OK, MessageBoxIcon.Error)   ' Affiche l'erreur
        Finally                                    ' Toujours exécuté
            _chargement = False                    ' Fin du remplissage
        End Try                                    ' Fin du bloc surveillé
    End Sub                                        ' Fin de Rafraichir

    Private Sub Service_EcheanceProche(sender As Object, e As EcheanceProcheEventArgs) Handles _service.EcheanceProche   ' Déclenché par le service pour chaque rappel
        _textesRappel.Add(e.Rappel.Depense.Libelle & " " & TexteJours(e.Rappel.JoursRestants))   ' Ex. « Loyer dans 3 jours »
    End Sub                                        ' Fin du gestionnaire

    Private Shared Function TexteJours(jours As Integer) As String   ' Phrase pour un nombre de jours
        If jours < 0 Then Return $"en retard de {-jours} jour(s)"   ' Échéance dépassée
        If jours = 0 Then Return "aujourd'hui"     ' Le jour même
        If jours = 1 Then Return "demain"          ' Demain
        Return $"dans {jours} jours"               ' Cas général
    End Function                                   ' Fin de TexteJours

    Private Shared Function TexteDe(p As Periodicite) As String   ' Libellé d'une périodicité
        Select Case p                              ' Aiguillage
            Case Periodicite.Hebdomadaire : Return "Hebdomadaire"
            Case Periodicite.Trimestrielle : Return "Trimestrielle"
            Case Periodicite.Annuelle : Return "Annuelle"
            Case Else : Return "Mensuelle"
        End Select                                 ' Fin de l'aiguillage
    End Function                                   ' Fin de TexteDe

    ''' <summary>Lit un montant saisi : espaces ignorés, virgule ou point acceptés comme séparateur décimal.</summary>
    Public Shared Function TryLireMontant(texte As String, ByRef montant As Decimal) As Boolean   ' Lecture tolérante
        Dim propre As String = If(texte, String.Empty).Replace(" ", "").Replace(ChrW(160), "").Replace(ChrW(8239), "").Replace(",", ".")   ' Retire les espaces, virgule vers point
        Return Decimal.TryParse(propre, NumberStyles.Number, CultureInfo.InvariantCulture, montant)   ' Convertit
    End Function                                   ' Fin de TryLireMontant

    Private Sub ViderFormulaire()                  ' Prépare la saisie d'une nouvelle dépense
        _courante = Nothing                        ' Aucune dépense sélectionnée
        lblFormTitre.Text = "Nouvelle dépense"     ' Titre du formulaire
        txtNom.Clear()                             ' Vide le nom
        txtMontant.Clear()                         ' Vide le montant
        cmbPeriodicite.SelectedIndex = 1           ' Mensuelle par défaut
        dtpEcheance.Value = Date.Today.AddDays(7)  ' Échéance proposée : dans une semaine
        btnDesactiver.Enabled = False              ' Rien à désactiver
        btnEcheanceSuivante.Enabled = False        ' Rien à avancer
        dgvDepenses.ClearSelection()               ' Retire la sélection de la grille
        txtNom.Focus()                             ' Curseur sur le nom
    End Sub                                        ' Fin de ViderFormulaire

    ' ---------------------------------------------------------------- événements

    Private Sub dgvDepenses_SelectionChanged(sender As Object, e As EventArgs) Handles dgvDepenses.SelectionChanged   ' L'utilisateur choisit une ligne
        If _chargement OrElse dgvDepenses.CurrentRow Is Nothing OrElse Not dgvDepenses.CurrentRow.Selected Then Return   ' Remplissage en cours ou aucune ligne choisie
        Dim ligne As LigneDepense = TryCast(dgvDepenses.CurrentRow.DataBoundItem, LigneDepense)   ' Objet de la ligne
        If ligne Is Nothing Then Return            ' Pas d'objet lié
        _courante = ligne.Source                   ' Mémorise la dépense choisie
        lblFormTitre.Text = "Modifier la dépense"  ' Titre du formulaire
        txtNom.Text = _courante.Libelle            ' Nom
        txtMontant.Text = _courante.Montant.ToString("0.##", CultureInfo.CurrentCulture)   ' Montant
        cmbPeriodicite.SelectedIndex = CInt(_courante.Periodicite) - 1   ' Périodicité (1 à 4 vers 0 à 3)
        dtpEcheance.Value = _courante.ProchaineEcheance   ' Échéance
        btnDesactiver.Enabled = True               ' Peut désactiver
        btnEcheanceSuivante.Enabled = True         ' Peut avancer l'échéance
    End Sub                                        ' Fin du gestionnaire

    Private Sub dgvDepenses_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvDepenses.CellFormatting   ' Avant l'affichage de chaque cellule
        If e.RowIndex < 0 Then Return              ' En-tête : rien à faire
        Dim ligne As LigneDepense = TryCast(dgvDepenses.Rows(e.RowIndex).DataBoundItem, LigneDepense)   ' Objet affiché sur cette ligne
        If ligne IsNot Nothing AndAlso ligne.EnAlerte Then   ' Échéance proche ou dépassée
            e.CellStyle.BackColor = Color.FromArgb(253, 243, 214)   ' Fond ambre clair
        End If                                     ' Fin du contrôle
    End Sub                                        ' Fin du gestionnaire

    Private Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click   ' Clic sur « Nouvelle dépense »
        ViderFormulaire()                          ' Prépare la saisie
    End Sub                                        ' Fin du gestionnaire

    Private Sub btnEnregistrer_Click(sender As Object, e As EventArgs) Handles btnEnregistrer.Click   ' Clic sur « Enregistrer »
        Dim montant As Decimal                     ' Montant lu
        If Not TryLireMontant(txtMontant.Text, montant) Then   ' Texte qui n'est pas un nombre
            MessageBox.Show("Le montant doit être un nombre (exemple : 25000).", "Dépenses récurrentes",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)   ' Message clair
            txtMontant.Focus()                     ' Replace le curseur
            Return                                 ' On n'enregistre pas
        End If                                     ' Fin de la validation

        Dim depense As New DepenseRecurrente With {
            .Id = If(_courante IsNot Nothing, _courante.Id, 0),   ' 0 = nouvelle dépense
            .Libelle = txtNom.Text,                               ' Nom
            .Montant = montant,                                   ' Montant
            .Periodicite = CType(cmbPeriodicite.SelectedIndex + 1, Periodicite),   ' Périodicité (0 à 3 vers 1 à 4)
            .ProchaineEcheance = dtpEcheance.Value.Date,          ' Échéance
            .Actif = True                                         ' Active
        }                                          ' Fin de l'initialisation
        Try                                        ' Début du bloc surveillé
            _service.Enregistrer(depense)          ' Valide puis enregistre
        Catch ex As ArgumentException              ' Donnée invalide (message clair du service)
            MessageBox.Show(ex.Message, "Dépenses récurrentes", MessageBoxButtons.OK, MessageBoxIcon.Warning)   ' Affiche le message
            Return                                 ' On reste sur le formulaire
        Catch ex As InvalidOperationException      ' Erreur d'accès aux données
            MessageBox.Show(ex.Message, "Dépenses récurrentes", MessageBoxButtons.OK, MessageBoxIcon.Error)   ' Affiche l'erreur
            Return                                 ' On reste sur le formulaire
        End Try                                    ' Fin du bloc surveillé
        Rafraichir()                               ' Recharge la liste et les rappels
        ViderFormulaire()                          ' Prêt pour la suivante
    End Sub                                        ' Fin du gestionnaire

    Private Sub btnEcheanceSuivante_Click(sender As Object, e As EventArgs) Handles btnEcheanceSuivante.Click   ' Clic sur « Échéance suivante »
        If _courante Is Nothing Then Return        ' Aucune dépense choisie
        Try                                        ' Début du bloc surveillé
            _service.PasserALEcheanceSuivante(_courante)   ' Avance la date d'une période
        Catch ex As InvalidOperationException      ' Erreur d'accès aux données
            MessageBox.Show(ex.Message, "Dépenses récurrentes", MessageBoxButtons.OK, MessageBoxIcon.Error)   ' Affiche l'erreur
            Return                                 ' Rien d'autre à faire
        End Try                                    ' Fin du bloc surveillé
        Rafraichir()                               ' Recharge la liste et les rappels
        ViderFormulaire()                          ' Prêt pour la suivante
    End Sub                                        ' Fin du gestionnaire

    Private Sub btnDesactiver_Click(sender As Object, e As EventArgs) Handles btnDesactiver.Click   ' Clic sur « Désactiver »
        If _courante Is Nothing Then Return        ' Aucune dépense choisie
        If MessageBox.Show($"Désactiver la dépense « {_courante.Libelle} » ?", "Dépenses récurrentes",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return   ' L'utilisateur renonce
        Try                                        ' Début du bloc surveillé
            _service.Desactiver(_courante.Id)      ' Retire la dépense de la liste
        Catch ex As InvalidOperationException      ' Erreur d'accès aux données
            MessageBox.Show(ex.Message, "Dépenses récurrentes", MessageBoxButtons.OK, MessageBoxIcon.Error)   ' Affiche l'erreur
            Return                                 ' Rien d'autre à faire
        End Try                                    ' Fin du bloc surveillé
        Rafraichir()                               ' Recharge la liste et les rappels
        ViderFormulaire()                          ' Prêt pour la suivante
    End Sub                                        ' Fin du gestionnaire

End Class                                          ' Fin de frmDepenses
