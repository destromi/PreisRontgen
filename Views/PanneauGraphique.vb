' ============================================================================
' FICHIER : Views/PanneauGraphique.vb
' RÔLE    : zone de dessin du graphique de l'historique (EF-06). C'est un Panel
'           ordinaire, mais « double tamponné » : le dessin est préparé en mémoire puis
'           affiché d'un coup, ce qui évite le scintillement quand on redimensionne.
'           Le dessin lui-même est fait dans l'événement Paint de frmHistorique.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

''' <summary>Panel sans scintillement, destiné à être dessiné par l'événement Paint.</summary>
Public Class PanneauGraphique                      ' Début de la classe
    Inherits Panel                                 ' C'est un Panel avec quelques réglages en plus

    Public Sub New()                               ' Constructeur
        MyBase.New()                               ' Construit d'abord le Panel
        DoubleBuffered = True                      ' Dessin en mémoire puis affichage d'un coup
        ResizeRedraw = True                        ' Redessine quand la taille change
    End Sub                                        ' Fin du constructeur

End Class                                          ' Fin de PanneauGraphique
