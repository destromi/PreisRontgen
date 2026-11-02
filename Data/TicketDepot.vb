Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports MySqlConnector

Public NotInheritable Class TicketDepot
    Public Async Function EnregistrerAsync(utilisateurId As Long, magasinId As Long, dateAchat As Date, totalSaisi As Decimal, lignes As IList(Of LigneTicket), detecteur As DetectionPrixService) As Task(Of List(Of AlertePrix))
        If magasinId <= 0 Then Throw New ArgumentException("Sélectionnez un magasin.")
        If lignes Is Nothing OrElse lignes.Count = 0 Then Throw New ArgumentException("Ajoutez au moins une ligne au ticket.")
        If totalSaisi < 0D Then Throw New ArgumentOutOfRangeException(NameOf(totalSaisi))
        Dim totalCalcule As Decimal = Decimal.Round(lignes.Sum(Function(ligne) ligne.Total), 2)
        If Math.Abs(totalCalcule - totalSaisi) > 0.02D Then Throw New InvalidOperationException("Le total saisi ne correspond pas à la somme des lignes (" & totalCalcule.ToString("0.00") & ").")

        Dim alertes As New List(Of AlertePrix)()
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using transaction As MySqlTransaction = DirectCast(Await connexion.BeginTransactionAsync(), MySqlTransaction)
                Try
                    Dim ticketId As Long
                    Using commande As New MySqlCommand("INSERT INTO ticket (id_utilisateur, id_magasin, date_achat, total_saisi) VALUES (@utilisateur, @magasin, @date, @total);", connexion, transaction)
                        commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                        commande.Parameters.AddWithValue("@magasin", magasinId)
                        commande.Parameters.AddWithValue("@date", dateAchat.Date)
                        commande.Parameters.AddWithValue("@total", totalSaisi)
                        Await commande.ExecuteNonQueryAsync()
                        ticketId = commande.LastInsertedId
                    End Using

                    Dim magasinNom As String
                    Using commandeMagasin As New MySqlCommand("SELECT nom FROM magasin WHERE id=@magasin AND actif=TRUE;", connexion, transaction)
                        commandeMagasin.Parameters.AddWithValue("@magasin", magasinId)
                        magasinNom = Convert.ToString(Await commandeMagasin.ExecuteScalarAsync())
                    End Using
                    Dim produitsDejaAnalyses As New HashSet(Of Long)()

                    For Each ligne As LigneTicket In lignes
                        If ligne.ProduitId <= 0 OrElse ligne.QuantiteBaseTotale <= 0D OrElse ligne.PrixUnitaire < 0D Then Throw New InvalidOperationException("Une ligne du ticket contient des valeurs invalides.")
                        Using commandeLigne As New MySqlCommand("INSERT INTO ligne_ticket (id_ticket, id_produit, quantite_conditionnement, unite, prix, nb_achete, quantite_base_totale, prix_unitaire) VALUES (@ticket, @produit, @conditionnement, @unite, @prix, @nombre, @quantiteBase, @prixUnitaire);", connexion, transaction)
                            commandeLigne.Parameters.AddWithValue("@ticket", ticketId)
                            commandeLigne.Parameters.AddWithValue("@produit", ligne.ProduitId)
                            commandeLigne.Parameters.AddWithValue("@conditionnement", ligne.Conditionnement)
                            commandeLigne.Parameters.AddWithValue("@unite", ligne.Unite)
                            commandeLigne.Parameters.AddWithValue("@prix", ligne.Prix)
                            commandeLigne.Parameters.AddWithValue("@nombre", ligne.NombreAchete)
                            commandeLigne.Parameters.AddWithValue("@quantiteBase", ligne.QuantiteBaseTotale)
                            commandeLigne.Parameters.AddWithValue("@prixUnitaire", ligne.PrixUnitaire)
                            Await commandeLigne.ExecuteNonQueryAsync()
                        End Using

                        Dim precedente As ObservationPrix = Await LireObservationPrecedenteAsync(connexion, transaction, ligne.ProduitId, magasinId, ticketId, dateAchat.Date)
                        Dim actuelle As New ObservationPrix With {
                            .ProduitId = ligne.ProduitId,
                            .ProduitNom = ligne.ProduitNom,
                            .Marque = ligne.Marque,
                            .MagasinId = magasinId,
                            .TicketId = ticketId,
                            .DateObservation = dateAchat.Date,
                            .PrixUnitaire = ligne.PrixUnitaire,
                            .QuantiteBase = ligne.QuantiteBaseTotale / ligne.NombreAchete,
                            .PrixConditionnement = ligne.Prix
                        }
                        Using commandeObservation As New MySqlCommand("INSERT INTO observation_prix (id_produit, id_magasin, id_ticket, date_obs, prix_unitaire, quantite_base, prix_conditionnement) VALUES (@produit, @magasin, @ticket, @date, @prixUnitaire, @quantiteBase, @prixConditionnement);", connexion, transaction)
                            commandeObservation.Parameters.AddWithValue("@produit", ligne.ProduitId)
                            commandeObservation.Parameters.AddWithValue("@magasin", magasinId)
                            commandeObservation.Parameters.AddWithValue("@ticket", ticketId)
                            commandeObservation.Parameters.AddWithValue("@date", dateAchat.Date)
                            commandeObservation.Parameters.AddWithValue("@prixUnitaire", actuelle.PrixUnitaire)
                            commandeObservation.Parameters.AddWithValue("@quantiteBase", actuelle.QuantiteBase)
                            commandeObservation.Parameters.AddWithValue("@prixConditionnement", actuelle.PrixConditionnement)
                            Await commandeObservation.ExecuteNonQueryAsync()
                        End Using

                        If precedente IsNot Nothing AndAlso produitsDejaAnalyses.Add(ligne.ProduitId) Then
                            Dim alertesLigne As List(Of AlertePrix) = Await DetecterEtEnregistrerAlertesAsync(connexion, transaction, utilisateurId, magasinId, magasinNom, ligne, precedente, actuelle, detecteur)
                            alertes.AddRange(alertesLigne)
                        End If
                    Next
                    Await transaction.CommitAsync()
                Catch
                    transaction.Rollback()
                    Throw
                End Try
            End Using
        End Using
        Return alertes
    End Function

    Public Async Function ListerHistoriqueAsync(utilisateurId As Long, produitId As Long, magasinId As Long?) As Task(Of List(Of ObservationPrix))
        Dim resultats As New List(Of ObservationPrix)()
        Dim sql As String = "SELECT o.id, o.id_produit, p.nom AS produit, p.marque, c.nom AS categorie, p.unite_base, o.id_magasin, m.nom AS magasin, o.id_ticket, o.date_obs, o.prix_unitaire, o.quantite_base, o.prix_conditionnement FROM observation_prix o INNER JOIN produit p ON p.id=o.id_produit INNER JOIN categorie c ON c.id=p.id_categorie INNER JOIN magasin m ON m.id=o.id_magasin INNER JOIN ticket t ON t.id=o.id_ticket WHERE t.id_utilisateur=@utilisateur AND o.id_produit=@produit"
        If magasinId.HasValue Then sql &= " AND o.id_magasin=@magasin"
        sql &= " ORDER BY o.date_obs, o.id LIMIT 5000;"
        Using connexion As MySqlConnection = Configuration.CreerConnexion()
            Await connexion.OpenAsync()
            Using commande As New MySqlCommand(sql, connexion)
                commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                commande.Parameters.AddWithValue("@produit", produitId)
                If magasinId.HasValue Then commande.Parameters.AddWithValue("@magasin", magasinId.Value)
                Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                    While Await lecteur.ReadAsync()
                        resultats.Add(New ObservationPrix With {
                            .Id = lecteur.GetInt64("id"), .ProduitId = lecteur.GetInt64("id_produit"),
                            .ProduitNom = lecteur.GetString("produit"), .Marque = lecteur.GetString("marque"), .CategorieNom = lecteur.GetString("categorie"), .UniteBase = lecteur.GetString("unite_base"),
                            .MagasinId = lecteur.GetInt64("id_magasin"), .MagasinNom = lecteur.GetString("magasin"),
                            .TicketId = lecteur.GetInt64("id_ticket"), .DateObservation = lecteur.GetDateTime("date_obs"),
                            .PrixUnitaire = lecteur.GetDecimal("prix_unitaire"), .QuantiteBase = lecteur.GetDecimal("quantite_base"),
                            .PrixConditionnement = lecteur.GetDecimal("prix_conditionnement")
                        })
                    End While
                End Using
            End Using
        End Using
        Return resultats
    End Function

    Private Shared Async Function LireObservationPrecedenteAsync(connexion As MySqlConnection, transaction As MySqlTransaction, produitId As Long, magasinId As Long, ticketId As Long, dateObservation As Date) As Task(Of ObservationPrix)
        Const sql As String = "SELECT id, id_ticket, date_obs, prix_unitaire, quantite_base, prix_conditionnement FROM observation_prix WHERE id_produit=@produit AND id_magasin=@magasin AND id_ticket<>@ticket AND date_obs<=@date ORDER BY date_obs DESC, id DESC LIMIT 1 FOR UPDATE;"
        Using commande As New MySqlCommand(sql, connexion, transaction)
            commande.Parameters.AddWithValue("@produit", produitId)
            commande.Parameters.AddWithValue("@magasin", magasinId)
            commande.Parameters.AddWithValue("@ticket", ticketId)
            commande.Parameters.AddWithValue("@date", dateObservation)
            Using lecteur As MySqlDataReader = DirectCast(Await commande.ExecuteReaderAsync(), MySqlDataReader)
                If Not Await lecteur.ReadAsync() Then Return Nothing
                Return New ObservationPrix With {
                    .Id = lecteur.GetInt64("id"), .TicketId = lecteur.GetInt64("id_ticket"), .DateObservation = lecteur.GetDateTime("date_obs"),
                    .PrixUnitaire = lecteur.GetDecimal("prix_unitaire"), .QuantiteBase = lecteur.GetDecimal("quantite_base"),
                    .PrixConditionnement = lecteur.GetDecimal("prix_conditionnement")
                }
            End Using
        End Using
    End Function

    Private Shared Async Function DetecterEtEnregistrerAlertesAsync(connexion As MySqlConnection, transaction As MySqlTransaction, utilisateurId As Long, magasinId As Long, magasinNom As String, ligne As LigneTicket, precedente As ObservationPrix, actuelle As ObservationPrix, detecteur As DetectionPrixService) As Task(Of List(Of AlertePrix))
        Dim seuilHausse As Decimal = Await LireParametreDecimalAsync(connexion, transaction, "seuil_hausse_pct", 5D)
        Dim seuilShrink As Decimal = Await LireParametreDecimalAsync(connexion, transaction, "seuil_shrinkflation_pct", 3D)
        Dim alertes As List(Of AlertePrix) = detecteur.Analyser(precedente, actuelle, seuilHausse, seuilShrink, magasinNom, ligne.ProduitNom, ligne.Marque)
        For Each alerte As AlertePrix In alertes
            Using commande As New MySqlCommand("INSERT INTO alerte (id_utilisateur, id_produit, id_magasin, type, valeur, prix_precedent, prix_actuel) VALUES (@utilisateur, @produit, @magasin, @type, @valeur, @precedent, @actuel);", connexion, transaction)
                commande.Parameters.AddWithValue("@utilisateur", utilisateurId)
                commande.Parameters.AddWithValue("@produit", ligne.ProduitId)
                commande.Parameters.AddWithValue("@magasin", magasinId)
                commande.Parameters.AddWithValue("@type", alerte.Type)
                commande.Parameters.AddWithValue("@valeur", alerte.Valeur)
                commande.Parameters.AddWithValue("@precedent", alerte.PrixPrecedent)
                commande.Parameters.AddWithValue("@actuel", alerte.PrixActuel)
                Await commande.ExecuteNonQueryAsync()
            End Using
        Next
        Return alertes
    End Function

    Private Shared Async Function LireParametreDecimalAsync(connexion As MySqlConnection, transaction As MySqlTransaction, cle As String, valeurDefaut As Decimal) As Task(Of Decimal)
        Using commande As New MySqlCommand("SELECT valeur FROM parametre WHERE cle=@cle LIMIT 1;", connexion, transaction)
            commande.Parameters.AddWithValue("@cle", cle)
            Dim valeur As Object = Await commande.ExecuteScalarAsync()
            Dim resultat As Decimal
            If valeur Is Nothing OrElse valeur Is DBNull.Value OrElse Not Decimal.TryParse(Convert.ToString(valeur), Globalization.NumberStyles.Number, Globalization.CultureInfo.InvariantCulture, resultat) Then Return valeurDefaut
            Return resultat
        End Using
    End Function
End Class
