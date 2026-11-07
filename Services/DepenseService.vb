' ============================================================================
' FICHIER : Services/DepenseService.vb
' RÔLE    : EF-14, dépenses récurrentes (loyer, électricité, abonnements...) avec
'           périodicité et rappel avant l'échéance.
' RÈGLES  : - un rappel est émis pour toute dépense active dont l'échéance est dans
'             les 7 jours (ou déjà dépassée) ;
'           - aucun paiement automatique n'est déclenché (maquette) : l'application ne fait
'             que rappeler et calculer la prochaine échéance.
' NOTION  : l'événement EcheanceProche suit le même principe que AlerteDetectee :
'           le service « crie » un rappel, l'écran qui s'y est abonné l'affiche.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Linq            ' Pour Where, Select, OrderBy...
Imports PreisRontgen.Data      ' Pour IDepenseRepository
Imports PreisRontgen.Models    ' Pour DepenseRecurrente, Periodicite, RappelEcheance

Namespace Services             ' Espace de noms : PreisRontgen.Services

    ''' <summary>Paquet d'informations transmis avec l'événement EcheanceProche.</summary>
    Public Class EcheanceProcheEventArgs          ' Début de la classe
        Inherits EventArgs                        ' Convention .NET pour les paramètres d'événement

        Public ReadOnly Property Rappel As RappelEcheance   ' Le rappel qui vient d'être émis

        Public Sub New(rappel As RappelEcheance)  ' Constructeur
            Me.Rappel = rappel                    ' Range le rappel dans la propriété
        End Sub                                   ' Fin du constructeur
    End Class                                     ' Fin de EcheanceProcheEventArgs

    ''' <summary>Gestion des dépenses récurrentes (EF-14).</summary>
    Public Class DepenseService                   ' Début de la classe

        ''' <summary>Nombre de jours avant l'échéance à partir duquel un rappel est émis.</summary>
        Public Const JoursRappelParDefaut As Integer = 7   ' Une semaine

        ''' <summary>Montant maximal accepté (colonne DECIMAL(12,2) de la base).</summary>
        Public Const MontantMaximum As Decimal = 9999999999.99D   ' Au-delà, la base refuserait la valeur

        ''' <summary>Déclenché pour chaque dépense dont l'échéance est proche ou dépassée (voir VerifierRappels).</summary>
        Public Event EcheanceProche As EventHandler(Of EcheanceProcheEventArgs)   ' Événement personnalisé

        Private ReadOnly _depot As IDepenseRepository   ' Accès aux données (reçu à la création)

        Public Sub New(depot As IDepenseRepository)   ' Constructeur
            If depot Is Nothing Then Throw New ArgumentNullException(NameOf(depot))   ' Refuse un dépôt absent
            _depot = depot                            ' Mémorise le dépôt
        End Sub                                       ' Fin du constructeur

        ''' <summary>Dépenses actives, de l'échéance la plus proche à la plus lointaine.</summary>
        Public Function Lister() As List(Of DepenseRecurrente)   ' Pour la grille
            Return _depot.Lister()                    ' Lit les dépenses actives
        End Function                                  ' Fin de Lister

        ''' <summary>
        ''' Valide puis enregistre la dépense (création si Id = 0, modification sinon) ; retourne son identifiant.
        ''' Lève ArgumentException avec un message clair si une donnée est invalide.
        ''' </summary>
        Public Function Enregistrer(depense As DepenseRecurrente) As Integer   ' Bouton « Enregistrer »
            If depense Is Nothing Then Throw New ArgumentNullException(NameOf(depense))   ' Refuse une dépense absente
            Dim erreur As String = Valider(depense.Libelle, depense.Montant, depense.Periodicite)   ' Contrôle des données
            If erreur IsNot Nothing Then Throw New ArgumentException(erreur)   ' Message affiché par l'écran

            depense.Libelle = depense.Libelle.Trim()  ' Enlève les espaces inutiles
            If depense.Id = 0 Then                    ' Nouvelle dépense
                depense.Id = _depot.Ajouter(depense)  ' La base attribue l'identifiant
            Else                                      ' Dépense existante
                _depot.Modifier(depense)              ' Met à jour
            End If                                    ' Fin du choix
            Return depense.Id                         ' Rend l'identifiant
        End Function                                  ' Fin de Enregistrer

        ''' <summary>Désactive une dépense (elle disparaît de la liste sans être supprimée).</summary>
        Public Sub Desactiver(idDepense As Integer)   ' Bouton « Désactiver »
            _depot.Desactiver(idDepense)              ' Passe la dépense à « inactive »
        End Sub                                       ' Fin de Desactiver

        ''' <summary>
        ''' Avance l'échéance d'une dépense d'une période (à utiliser quand elle a été réglée par l'utilisateur).
        ''' Aucun paiement n'est déclenché : seule la date change.
        ''' </summary>
        Public Sub PasserALEcheanceSuivante(depense As DepenseRecurrente)   ' Bouton « Échéance suivante »
            If depense Is Nothing Then Throw New ArgumentNullException(NameOf(depense))   ' Refuse une dépense absente
            depense.ProchaineEcheance = CalculerEcheanceSuivante(depense.ProchaineEcheance, depense.Periodicite)   ' Nouvelle date
            _depot.Modifier(depense)                  ' Enregistre la nouvelle échéance
        End Sub                                       ' Fin de PasserALEcheanceSuivante

        ''' <summary>
        ''' Cherche les rappels à émettre à la date indiquée et déclenche l'événement EcheanceProche pour chacun.
        ''' Retourne aussi la liste des rappels, de l'échéance la plus proche à la plus lointaine.
        ''' </summary>
        Public Function VerifierRappels(aujourdhui As Date, Optional joursRappel As Integer = JoursRappelParDefaut) As List(Of RappelEcheance)   ' Appelé à l'ouverture de l'écran
            Dim rappels As List(Of RappelEcheance) = ListerRappels(_depot.Lister(), aujourdhui, joursRappel)   ' Calcule les rappels
            For Each r As RappelEcheance In rappels   ' Pour chaque rappel
                RaiseEvent EcheanceProche(Me, New EcheanceProcheEventArgs(r))   ' Prévient les abonnés
            Next                                      ' Fin de la boucle
            Return rappels                            ' Rend la liste
        End Function                                  ' Fin de VerifierRappels

        ''' <summary>
        ''' Contrôle des données saisies. Retourne un message d'erreur, ou Nothing si tout est correct.
        ''' </summary>
        Public Shared Function Valider(libelle As String, montant As Decimal, periodicite As Periodicite) As String   ' Messages clairs (cahier §5, ergonomie)
            If String.IsNullOrWhiteSpace(libelle) Then Return "Le nom de la dépense est obligatoire."   ' Nom vide
            If libelle.Trim().Length > 180 Then Return "Le nom de la dépense ne doit pas dépasser 180 caractères."   ' Colonne VARCHAR(180)
            If montant <= 0D Then Return "Le montant doit être strictement positif."   ' Montant nul ou négatif
            If montant > MontantMaximum Then Return "Le montant est trop élevé."        ' Dépasse la colonne
            If Not [Enum].IsDefined(GetType(Periodicite), periodicite) Then Return "La périodicité est invalide."   ' Valeur hors liste
            Return Nothing                            ' Tout est correct
        End Function                                  ' Fin de Valider

        ''' <summary>Date de l'échéance suivante : +7 jours, +1 mois, +3 mois ou +1 an.</summary>
        Public Shared Function CalculerEcheanceSuivante(echeance As Date, periodicite As Periodicite) As Date   ' Calcul de date
            Select Case periodicite                   ' Aiguillage selon la fréquence
                Case Periodicite.Hebdomadaire         ' Toutes les semaines
                    Return echeance.AddDays(7)        ' Sept jours plus tard
                Case Periodicite.Mensuelle            ' Tous les mois
                    Return echeance.AddMonths(1)      ' Un mois plus tard (31 janvier donne 28 ou 29 février)
                Case Periodicite.Trimestrielle        ' Tous les trois mois
                    Return echeance.AddMonths(3)      ' Trois mois plus tard
                Case Else                             ' Annuelle
                    Return echeance.AddYears(1)       ' Un an plus tard (29 février donne 28 février)
            End Select                                ' Fin de l'aiguillage
        End Function                                  ' Fin de CalculerEcheanceSuivante

        ''' <summary>
        ''' Dépenses actives dont l'échéance est dans les « joursRappel » prochains jours, ou déjà dépassée.
        ''' JoursRestants est négatif pour une échéance dépassée. Tri : la plus urgente d'abord.
        ''' </summary>
        Public Shared Function ListerRappels(depenses As IEnumerable(Of DepenseRecurrente),
                                             aujourdhui As Date,
                                             joursRappel As Integer) As List(Of RappelEcheance)   ' Règle de rappel, sans base de données
            If depenses Is Nothing Then Throw New ArgumentNullException(NameOf(depenses))   ' Refuse une liste absente
            Dim actives As IEnumerable(Of DepenseRecurrente) = depenses.Where(Function(d) d.Actif)   ' Dépenses actives seulement
            Dim rappels As IEnumerable(Of RappelEcheance) = actives.Select(Function(d) New RappelEcheance With {.Depense = d, .JoursRestants = CInt((d.ProchaineEcheance.Date - aujourdhui.Date).TotalDays)})   ' Jours restants de chacune
            Dim proches As IEnumerable(Of RappelEcheance) = rappels.Where(Function(r) r.JoursRestants <= joursRappel)   ' Échéance proche ou dépassée
            Return proches.OrderBy(Function(r) r.JoursRestants).ThenBy(Function(r) r.Depense.Libelle).ToList()   ' La plus urgente d'abord
        End Function                                  ' Fin de ListerRappels

    End Class                                         ' Fin de DepenseService

End Namespace                                         ' Fin de l'espace de noms Services
