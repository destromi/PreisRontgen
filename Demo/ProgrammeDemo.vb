' ============================================================================
' FICHIER : Demo/ProgrammeDemo.vb
' RÔLE    : point de départ du programme de DÉMONSTRATION : ouvre frmAlertes
'           branché sur un faux dépôt en mémoire (DepotDemo).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Windows.Forms   ' Pour Application

''' <summary>Programme de démonstration de l'écran des alertes (EF-09).</summary>
Module ProgrammeDemo                    ' Un Module regroupe des méthodes partagées ; ici il contient Main

    <STAThread>                         ' Obligatoire pour une application Windows Forms (modèle de thread de l'interface)
    Public Sub Main()                   ' Point d'entrée : exécuté au lancement
        Application.EnableVisualStyles()                    ' Active l'apparence moderne des contrôles
        Application.SetCompatibleTextRenderingDefault(False)   ' Réglage standard du rendu du texte
        Application.Run(New PreisRontgen.frmAlertes(New DepotDemo()))   ' Ouvre l'écran, alimenté par le faux dépôt
    End Sub                             ' Fin de Main

End Module                              ' Fin du module
