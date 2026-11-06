' ============================================================================
' FICHIER : Models/Observation.vb
' RÔLE    : représente UNE observation de prix = « tel produit coûtait tel prix
'           unitaire dans tel magasin à telle date ». Elle correspond à une ligne
'           de la table observation_prix. C'est la donnée que compare la détection.
' NOTION  : une « Property » est un champ de la classe avec un nom lisible.
'           Elle se lit (x = obs.PrixUnitaire) et s'écrit (obs.PrixUnitaire = 4.8D).
' ============================================================================
Option Strict On      ' Conversions implicites dangereuses interdites
Option Explicit On    ' Variables obligatoirement déclarées

Namespace Models      ' Espace de noms : PreisRontgen.Models

    ''' <summary>
    ''' Observation d'un prix : un produit, dans un magasin, à une date donnée.
    ''' Correspond à une ligne de la table observation_prix.
    ''' </summary>
    Public Class Observation     ' Début de la classe ; Public = visible par tous les modules

        Public Property Id As Integer          ' Identifiant unique en base (0 tant que non enregistrée)

        Public Property IdProduit As Integer   ' Identifiant du produit observé (clé étrangère vers produit)

        Public Property IdMagasin As Integer   ' Identifiant du magasin où le prix a été relevé

        Public Property DateObs As Date        ' Date de l'achat (date du ticket)

        ''' <summary>Prix par unité de base (€/kg, €/L ou €/pièce).</summary>
        Public Property PrixUnitaire As Decimal   ' Decimal = nombre décimal exact, adapté à l'argent (pas d'erreur d'arrondi binaire)

        ''' <summary>Quantité du conditionnement, exprimée en unité de base (ex. 0,5 pour 500 g).</summary>
        Public Property QuantiteBase As Decimal   ' Sert à détecter la baisse de quantité (shrinkflation)

        ''' <summary>Prix payé pour un article (un conditionnement), tel qu'inscrit sur le ticket.</summary>
        Public Property PrixTotal As Decimal      ' Sert à vérifier que le prix de l'article n'a pas baissé (shrinkflation)

    End Class                    ' Fin de la classe Observation

End Namespace                    ' Fin de l'espace de noms Models
