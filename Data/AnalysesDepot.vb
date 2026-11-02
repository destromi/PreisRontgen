Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports MySqlConnector

Public NotInheritable Class ResultatIndice
    Public Property Indice As Decimal?
    Public Property CoutDepart As Decimal
    Public Property CoutArrivee As Decimal
    Public Property ProduitsCompares As Integer
End Class

Public NotInheritable Class AnalysesDepot
    Public Async Function CalculerIndiceAsync(utilisateurId As Long, dateDepart As Date, dateArrivee As Date) As Task(Of ResultatIndice)
        If dateArrivee.Date <= dateDepart.Date Then Throw New ArgumentException("La date d'arrivée doit être postérieure à la date de départ.")
        Dim resultat As New ResultatIndice()
        Const sql As String = "SELECT lt.id_produit, SUM(lt.quantite_base_totale) AS quantite_reference, " &
            "(SELECT op.prix_unitaire FROM observation_prix op INNER JOIN ticket t1 ON t1.id=op.id_ticket WHERE t1.id_utilisateur=@utilisateur AND op.id_produit=lt.id_produit AND op.date_obs<=@depart ORDER BY op.date_obs DESC, op.id DESC LIMIT 1) AS prix_depart, " &
            "(SELECT op.prix_unitaire FROM observation_prix op INNER JOIN ticket t2 ON t2.id=op.id_ticket WHERE t2.id_utilisateur=@utilisateur AND op.id_produit=lt.id_produit AND op.date_obs<=@arrivee ORDER BY op.date_obs DESC, op.id DESC LIMIT 1) AS prix_arrivee " &
            "FROM ligne_ticket lt INNER JOIN ticket t0 ON t0.id=lt.id_ticket WHERE t0.id_utilisateur=@utilisateur AND t0.date_achat>=DATE_SUB(@depart, INTERVAL 30 DAY) AND t0.date_achat<=@depart GROUP BY lt.id_produit;"
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand(sql, connexion)
                commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                commande.Parameters.AddWithValue("@depart", dateDepart.Date)
                commande.Parameters.AddWithValue("@arrivee", dateArrivee.Date)
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    While Await lecteur.ReadAsync()
                        If lecteur.IsDBNull(2) OrElse lecteur.IsDBNull(3) Then Continue While
                        Dim quantite As Decimal = lecteur.GetDecimal("quantite_reference")
                        resultat.CoutDepart += quantite * lecteur.GetDecimal("prix_depart")
                        resultat.CoutArrivee += quantite * lecteur.GetDecimal("prix_arrivee")
                        resultat.ProduitsCompares += 1
                    End While
                End Using
            End Using
        End Using
        If resultat.CoutDepart > 0D Then resultat.Indice = Decimal.Round(resultat.CoutArrivee / resultat.CoutDepart * 100D, 2)
        Return resultat
    End Function

    Public Async Function DepensesParCategorieAsync(utilisateurId As Long, annee As Integer, mois As Integer) As Task(Of List(Of DepenseCategorie))
        Dim resultats As New List(Of DepenseCategorie)()
        Const sql As String = "SELECT c.nom, SUM(lt.prix * lt.nb_achete) AS depense FROM ticket t INNER JOIN ligne_ticket lt ON lt.id_ticket=t.id INNER JOIN produit p ON p.id=lt.id_produit INNER JOIN categorie c ON c.id=p.id_categorie WHERE t.id_utilisateur=@utilisateur AND YEAR(t.date_achat)=@annee AND MONTH(t.date_achat)=@mois GROUP BY c.id, c.nom ORDER BY depense DESC;"
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand(sql, connexion)
                commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                commande.Parameters.AddWithValue("@annee", annee)
                commande.Parameters.AddWithValue("@mois", mois)
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    While Await lecteur.ReadAsync()
                        resultats.Add(New DepenseCategorie With {.CategorieNom = lecteur.GetString("nom"), .Montant = lecteur.GetDecimal("depense")})
                    End While
                End Using
            End Using
        End Using
        Return resultats
    End Function

    Public Async Function ComparerMagasinsAsync(utilisateurId As Long, produitId As Long?, categorieId As Long?) As Task(Of List(Of PrixMagasin))
        Dim resultats As New List(Of PrixMagasin)()
        Dim sql As String = "SELECT m.nom AS magasin, p.nom AS produit, p.unite_base, o.prix_unitaire, o.date_obs FROM observation_prix o INNER JOIN ticket t ON t.id=o.id_ticket INNER JOIN magasin m ON m.id=o.id_magasin INNER JOIN produit p ON p.id=o.id_produit WHERE t.id_utilisateur=@utilisateur AND p.actif=TRUE AND NOT EXISTS (SELECT 1 FROM observation_prix o2 INNER JOIN ticket t2 ON t2.id=o2.id_ticket WHERE t2.id_utilisateur=@utilisateur AND o2.id_produit=o.id_produit AND o2.id_magasin=o.id_magasin AND (o2.date_obs>o.date_obs OR (o2.date_obs=o.date_obs AND o2.id>o.id)))"
        If produitId.HasValue Then sql &= " AND o.id_produit=@produit"
        If categorieId.HasValue Then sql &= " AND p.id_categorie=@categorie"
        sql &= " ORDER BY o.prix_unitaire, m.nom;"
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand(sql, connexion)
                commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                If produitId.HasValue Then commande.Parameters.AddWithValue("@produit", produitId.Value)
                If categorieId.HasValue Then commande.Parameters.AddWithValue("@categorie", categorieId.Value)
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    While Await lecteur.ReadAsync()
                        resultats.Add(New PrixMagasin With {.MagasinNom = lecteur.GetString("magasin"), .ProduitNom = lecteur.GetString("produit"), .UniteBase = lecteur.GetString("unite_base"), .PrixUnitaire = lecteur.GetDecimal("prix_unitaire"), .DateObservation = lecteur.GetDateTime("date_obs")})
                    End While
                End Using
            End Using
        End Using
        Return resultats
    End Function

    Public Async Function ProduitsMoinsChersAsync(utilisateurId As Long, produitId As Long) As Task(Of List(Of ProduitEquivalent))
        Dim resultats As New List(Of ProduitEquivalent)()
        Const sql As String = "SELECT p2.nom AS produit, p2.marque, m.nom AS magasin, p2.unite_base, o.prix_unitaire FROM produit source INNER JOIN produit p2 ON p2.id_categorie=source.id_categorie AND p2.unite_base=source.unite_base AND p2.id<>source.id AND p2.actif=TRUE INNER JOIN observation_prix o ON o.id_produit=p2.id INNER JOIN ticket t ON t.id=o.id_ticket INNER JOIN magasin m ON m.id=o.id_magasin WHERE source.id=@produit AND source.actif=TRUE AND t.id_utilisateur=@utilisateur AND NOT EXISTS (SELECT 1 FROM observation_prix o2 INNER JOIN ticket t2 ON t2.id=o2.id_ticket WHERE t2.id_utilisateur=@utilisateur AND o2.id_produit=o.id_produit AND o2.id_magasin=o.id_magasin AND (o2.date_obs>o.date_obs OR (o2.date_obs=o.date_obs AND o2.id>o.id))) AND o.prix_unitaire < (SELECT os.prix_unitaire FROM observation_prix os INNER JOIN ticket ts ON ts.id=os.id_ticket WHERE ts.id_utilisateur=@utilisateur AND os.id_produit=source.id ORDER BY os.date_obs DESC, os.id DESC LIMIT 1) ORDER BY o.prix_unitaire LIMIT 100;"
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand(sql, connexion)
                commande.Parameters.AddWithValue("@produit", produitId)
                commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    While Await lecteur.ReadAsync()
                        resultats.Add(New ProduitEquivalent With {
                            .ProduitNom = lecteur.GetString("produit"), .Marque = lecteur.GetString("marque"),
                            .MagasinNom = lecteur.GetString("magasin"), .UniteBase = lecteur.GetString("unite_base"),
                            .PrixUnitaire = lecteur.GetDecimal("prix_unitaire")
                        })
                    End While
                End Using
            End Using
        End Using
        Return resultats
    End Function
End Class
