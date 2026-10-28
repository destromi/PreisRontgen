' ============================================================================
' FICHIER : Services/NormalisationService.vb
' RÔLE    : EF-05, prix unitaire normalisé. Permet de comparer des produits vendus
'           dans des conditionnements différents (500 g, 1 kg, 33 cl, 1 L...).
' PRINCIPE: tout est ramené à une unité de base (kg, L ou pièce), puis
'           prix unitaire = prix de l'article / quantité en unité de base.
'           Exemple du cahier des charges : 2,40 pour 500 g = 4,80 par kg.
' NOTION  : « Shared » = méthode appelée directement sur la classe
'           (NormalisationService.CalculerPrixUnitaire(...)) sans créer d'objet.
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Imports PreisRontgen.Models    ' Permet d'écrire Unite, UniteBase, Observation sans préfixe

Namespace Services             ' Espace de noms des classes « métier » : PreisRontgen.Services

    ''' <summary>
    ''' EF-05 : prix unitaire normalisé.
    ''' Convertit les quantités en unité de base (g vers kg, cl et ml vers L)
    ''' et calcule le prix par unité de base, pour comparer les produits à unité égale.
    ''' Tous les calculs utilisent Decimal pour éviter les erreurs d'arrondi.
    ''' </summary>
    Public NotInheritable Class NormalisationService   ' NotInheritable = aucune autre classe ne peut en hériter

        Private Sub New()      ' Constructeur privé : personne ne peut faire « New NormalisationService »
        End Sub                ' (la classe ne contient que des méthodes Shared, pas besoin d'objet)

        ''' <summary>Unité de base associée à une unité de saisie.</summary>
        Public Shared Function UniteBaseDe(uniteSaisie As Unite) As UniteBase   ' Reçoit une unité de ticket, rend sa famille
            Select Case uniteSaisie                                   ' Aiguillage selon la valeur de l'unité
                Case Unite.Gramme, Unite.Kilogramme                   ' Si c'est du poids...
                    Return UniteBase.Kg                               ' ...la famille est « kilogramme »
                Case Unite.Millilitre, Unite.Centilitre, Unite.Litre  ' Si c'est du volume...
                    Return UniteBase.Litre                            ' ...la famille est « litre »
                Case Unite.Piece                                      ' Si c'est compté à l'unité...
                    Return UniteBase.Piece                            ' ...la famille est « pièce »
                Case Else                                             ' Toute autre valeur (ne devrait jamais arriver)
                    Throw New ArgumentOutOfRangeException(NameOf(uniteSaisie), "Unité inconnue.")   ' On signale l'erreur par une exception
            End Select                                                ' Fin de l'aiguillage
        End Function                                                  ' Fin de UniteBaseDe

        ''' <summary>
        ''' Convertit une quantité dans l'unité de base correspondante.
        ''' Exemples : 500 g donne 0,5 kg ; 33 cl donne 0,33 L ; 6 pièces donne 6.
        ''' </summary>
        ''' <exception cref="ArgumentOutOfRangeException">La quantité n'est pas strictement positive.</exception>
        Public Shared Function ConvertirEnUniteBase(quantite As Decimal, uniteSaisie As Unite) As Decimal   ' Rend la quantité exprimée en kg, L ou pièce
            If quantite <= 0D Then      ' Une quantité nulle ou négative n'a pas de sens (et provoquerait une division par zéro plus loin)
                Throw New ArgumentOutOfRangeException(NameOf(quantite), "La quantité doit être strictement positive.")   ' On refuse avec un message clair
            End If                      ' Fin du contrôle

            Select Case uniteSaisie     ' Aiguillage selon l'unité saisie
                Case Unite.Gramme, Unite.Millilitre    ' 1000 g = 1 kg et 1000 ml = 1 L
                    Return quantite / 1000D            ' Divise par 1000 (ex. 500 g donne 0,5 kg)
                Case Unite.Centilitre                  ' 100 cl = 1 L
                    Return quantite / 100D             ' Divise par 100 (ex. 33 cl donne 0,33 L)
                Case Unite.Kilogramme, Unite.Litre, Unite.Piece   ' Ces unités sont déjà des unités de base
                    Return quantite                    ' Aucune conversion à faire
                Case Else                              ' Toute autre valeur (ne devrait jamais arriver)
                    Throw New ArgumentOutOfRangeException(NameOf(uniteSaisie), "Unité inconnue.")   ' Erreur explicite
            End Select                                 ' Fin de l'aiguillage
        End Function                                   ' Fin de ConvertirEnUniteBase

        ''' <summary>
        ''' Prix par unité de base : prix de l'article divisé par sa quantité exprimée en unité de base.
        ''' Exemple : 2,40 pour 500 g donne 4,80 par kg.
        ''' </summary>
        ''' <param name="prix">Prix d'un article (un conditionnement), positif ou nul.</param>
        ''' <param name="quantiteConditionnement">Contenu de l'article dans l'unité du ticket (ex. 500).</param>
        ''' <param name="uniteSaisie">Unité du conditionnement (ex. Gramme).</param>
        Public Shared Function CalculerPrixUnitaire(prix As Decimal,
                                                    quantiteConditionnement As Decimal,
                                                    uniteSaisie As Unite) As Decimal   ' Rend le prix par kg, L ou pièce
            If prix < 0D Then           ' Un prix négatif est impossible
                Throw New ArgumentOutOfRangeException(NameOf(prix), "Le prix ne peut pas être négatif.")   ' On refuse
            End If                      ' Fin du contrôle (un prix de 0 reste accepté : produit offert)
            Return prix / ConvertirEnUniteBase(quantiteConditionnement, uniteSaisie)   ' Prix divisé par la quantité en unité de base
        End Function                    ' Fin de CalculerPrixUnitaire (version simple)

        ''' <summary>
        ''' Idem, en vérifiant que l'unité saisie est compatible avec l'unité de base du produit
        ''' (on ne peut pas enregistrer des grammes pour un produit vendu au litre).
        ''' </summary>
        ''' <exception cref="ArgumentException">Unité incompatible avec l'unité de base du produit.</exception>
        Public Shared Function CalculerPrixUnitaire(prix As Decimal,
                                                    quantiteConditionnement As Decimal,
                                                    uniteSaisie As Unite,
                                                    uniteBaseProduit As UniteBase) As Decimal   ' Surcharge : même nom, un paramètre de plus
            VerifierCompatibilite(uniteSaisie, uniteBaseProduit)   ' Refuse (exception) si on mélange poids et volume par exemple
            Return CalculerPrixUnitaire(prix, quantiteConditionnement, uniteSaisie)   ' Réutilise la version simple ci-dessus
        End Function                    ' Fin de CalculerPrixUnitaire (version avec contrôle)

        ''' <summary>Vérifie la compatibilité entre l'unité saisie et l'unité de base du produit.</summary>
        Public Shared Sub VerifierCompatibilite(uniteSaisie As Unite, uniteBaseProduit As UniteBase)   ' Sub = ne renvoie rien, lève une exception si incompatible
            If UniteBaseDe(uniteSaisie) <> uniteBaseProduit Then   ' La famille de l'unité saisie diffère de celle du produit
                Throw New ArgumentException(
                    $"L'unité saisie ({uniteSaisie}) est incompatible avec l'unité de base du produit ({uniteBaseProduit}).",
                    NameOf(uniteSaisie))                           ' Message précis ($"..." insère les valeurs entre accolades)
            End If                                                 ' Fin du contrôle
        End Sub                                                    ' Fin de VerifierCompatibilite

        ''' <summary>
        ''' Construit l'observation de prix correspondant à une ligne de ticket.
        ''' À appeler par la saisie de ticket (T2) avant d'enregistrer l'observation et d'appeler la détection.
        ''' </summary>
        Public Shared Function CreerObservation(idProduit As Integer,
                                                idMagasin As Integer,
                                                dateObs As Date,
                                                prixLigne As Decimal,
                                                quantiteConditionnement As Decimal,
                                                uniteSaisie As Unite) As Observation   ' Rend une Observation prête à enregistrer
            Dim quantiteBase As Decimal = ConvertirEnUniteBase(quantiteConditionnement, uniteSaisie)   ' Variable locale : quantité en kg/L/pièce
            Return New Observation With {                                  ' Crée l'objet et remplit ses propriétés d'un coup
                .IdProduit = idProduit,                                    ' Produit observé
                .IdMagasin = idMagasin,                                    ' Magasin
                .DateObs = dateObs,                                        ' Date du ticket
                .PrixTotal = prixLigne,                                    ' Prix de l'article tel qu'écrit sur le ticket
                .QuantiteBase = quantiteBase,                              ' Quantité convertie en unité de base
                .PrixUnitaire = CalculerPrixUnitaire(prixLigne, quantiteConditionnement, uniteSaisie)   ' Prix par kg/L/pièce
            }                                                              ' Fin de l'initialisation de l'objet
        End Function                                                       ' Fin de CreerObservation

        ''' <summary>
        ''' Lit une unité écrite sur un ticket ou dans un fichier CSV : g, kg, ml, cl, l, pièce...
        ''' Retourne False si le texte n'est pas reconnu.
        ''' </summary>
        Public Shared Function TryParseUnite(texte As String, ByRef uniteSaisie As Unite) As Boolean   ' ByRef : le résultat revient aussi par le 2e paramètre
            If String.IsNullOrWhiteSpace(texte) Then    ' Texte absent, vide ou fait uniquement d'espaces
                Return False                            ' Rien à reconnaître : échec
            End If                                      ' Fin du contrôle

            Select Case texte.Trim().TrimEnd("."c).ToLowerInvariant()   ' Nettoie : retire espaces et point final, passe en minuscules
                Case "g", "gr", "gramme", "grammes"                      ' Écritures possibles du gramme
                    uniteSaisie = Unite.Gramme                           ' On renvoie Gramme par le paramètre ByRef
                Case "kg", "kilo", "kilos", "kilogramme", "kilogrammes"  ' Écritures possibles du kilogramme
                    uniteSaisie = Unite.Kilogramme                       ' On renvoie Kilogramme
                Case "ml", "millilitre", "millilitres"                   ' Écritures possibles du millilitre
                    uniteSaisie = Unite.Millilitre                       ' On renvoie Millilitre
                Case "cl", "centilitre", "centilitres"                   ' Écritures possibles du centilitre
                    uniteSaisie = Unite.Centilitre                       ' On renvoie Centilitre
                Case "l", "litre", "litres"                              ' Écritures possibles du litre (« L » est passé en minuscule)
                    uniteSaisie = Unite.Litre                            ' On renvoie Litre
                Case "piece", "pièce", "pieces", "pièces", "pc", "pcs", "u", "unite", "unité", "unités", "unites"   ' Écritures possibles de la pièce
                    uniteSaisie = Unite.Piece                            ' On renvoie Piece
                Case Else                                                ' Texte non reconnu
                    Return False                                         ' Échec : l'appelant affichera une erreur
            End Select                                                   ' Fin de l'aiguillage
            Return True                                                  ' Texte reconnu : succès
        End Function                                                     ' Fin de TryParseUnite

        ''' <summary>Arrondit un prix ou une variation à 4 décimales, pour l'affichage ou le stockage.</summary>
        Public Shared Function Arrondir(valeur As Decimal) As Decimal    ' Rend la valeur arrondie
            Return Math.Round(valeur, 4, MidpointRounding.AwayFromZero)  ' 4 décimales ; 0,5 s'arrondit « vers le haut » comme à l'école
        End Function                                                     ' Fin de Arrondir

    End Class                                                            ' Fin de la classe NormalisationService

End Namespace                                                            ' Fin de l'espace de noms Services
