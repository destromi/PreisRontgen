' ============================================================================
' FICHIER : Services/FormatageAnalyse.vb
' RÔLE    : petites fonctions d'affichage partagées par les écrans de T4 :
'           symbole d'une unité de base, prix « 1 250 / L », quantité « 1 L »,
'           lecture de la colonne unite_base de la table produit.
' NOTION  : une classe « NotInheritable » avec des membres « Shared » est une boîte
'           à outils : on l'utilise sans créer d'objet (FormatageAnalyse.SymboleUnite(...)).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports System.Globalization   ' Pour CultureInfo (séparateurs de milliers et de décimales)
Imports PreisRontgen.Models    ' Pour UniteBase

Namespace Services             ' Espace de noms : PreisRontgen.Services

    ''' <summary>Fonctions d'affichage communes aux écrans d'analyse.</summary>
    Public NotInheritable Class FormatageAnalyse     ' Début de la classe

        Private Sub New()                            ' Constructeur privé : on n'a jamais besoin de créer un objet
        End Sub                                      ' Fin du constructeur

        ''' <summary>Symbole d'une unité de base : « kg », « L » ou « pièce ».</summary>
        Public Shared Function SymboleUnite(unite As UniteBase) As String   ' Texte court de l'unité
            Select Case unite                        ' Aiguillage selon l'unité
                Case UniteBase.Kg                    ' Poids
                    Return "kg"                      ' Kilogramme
                Case UniteBase.Litre                 ' Volume
                    Return "L"                       ' Litre
                Case Else                            ' Produit compté
                    Return "pièce"                   ' À la pièce
            End Select                               ' Fin de l'aiguillage
        End Function                                 ' Fin de SymboleUnite

        ''' <summary>Convertit le texte de la colonne unite_base (« kg », « L », « pièce ») en UniteBase.</summary>
        Public Shared Function UniteDepuisBase(texte As String) As UniteBase   ' Lecture de la base de données
            Dim t As String = If(texte, String.Empty).Trim().ToLowerInvariant()   ' Texte nettoyé, en minuscules
            If t = "kg" Then Return UniteBase.Kg     ' Kilogramme
            If t = "l" Then Return UniteBase.Litre   ' Litre
            Return UniteBase.Piece                   ' « pièce » ou valeur inconnue : à la pièce
        End Function                                 ' Fin de UniteDepuisBase

        ''' <summary>Montant sans décimale à partir de 100 (1 250), avec deux décimales en dessous (4,80).</summary>
        Public Shared Function FormaterMontant(montant As Decimal, Optional culture As IFormatProvider = Nothing) As String   ' Texte d'un montant
            Dim fournisseur As IFormatProvider = If(culture, CultureInfo.CurrentCulture)   ' Culture par défaut : celle de Windows
            If Math.Abs(montant) >= 100D Then        ' Grand montant (FCFA)
                Return montant.ToString("N0", fournisseur)   ' Sans décimale
            End If                                   ' Fin du test
            Return montant.ToString("N2", fournisseur)       ' Petit montant : deux décimales
        End Function                                 ' Fin de FormaterMontant

        ''' <summary>Prix unitaire avec son unité : « 1 250 / L ».</summary>
        Public Shared Function FormaterPrix(prix As Decimal, unite As UniteBase, Optional culture As IFormatProvider = Nothing) As String   ' Texte d'un prix unitaire
            Return FormaterMontant(prix, culture) & " / " & SymboleUnite(unite)   ' Montant, barre oblique, unité
        End Function                                 ' Fin de FormaterPrix

        ''' <summary>Quantité avec son unité : « 1 L », « 0,5 kg ».</summary>
        Public Shared Function FormaterQuantite(quantite As Decimal, unite As UniteBase, Optional culture As IFormatProvider = Nothing) As String   ' Texte d'un conditionnement
            Dim fournisseur As IFormatProvider = If(culture, CultureInfo.CurrentCulture)   ' Culture par défaut : celle de Windows
            Return quantite.ToString("0.###", fournisseur) & " " & SymboleUnite(unite)     ' Jusqu'à trois décimales, puis l'unité
        End Function                                 ' Fin de FormaterQuantite

    End Class                                        ' Fin de FormatageAnalyse

End Namespace                                        ' Fin de l'espace de noms Services
