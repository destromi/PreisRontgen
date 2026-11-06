Option Strict On
Option Explicit On

Imports MySql.Data.MySqlClient

Public Class TicketRepository

    Public Function ListerMagasins() As DataTable
        Using cn As MySqlConnection = ConnexionBD.CreerConnexion()
            Using cmd As New MySqlCommand("SELECT id, nom FROM magasin ORDER BY nom", cn)
                Using da As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    Return dt
                End Using
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Enregistre le ticket, ses lignes et les observations de prix dans UNE transaction :
    ''' tout est enregistré, ou rien. Renvoie l'id du ticket.
    ''' </summary>
    Public Function Enregistrer(idUtilisateur As Integer, idMagasin As Integer, dateAchat As Date,
                                totalSaisi As Decimal, lignes As IEnumerable(Of LigneTicketSaisie)) As Integer
        Using cn As MySqlConnection = ConnexionBD.CreerConnexion()
            Using tx As MySqlTransaction = cn.BeginTransaction()
                Try
                    Dim idTicket As Integer
                    Using cmd As New MySqlCommand(
                        "INSERT INTO ticket (id_utilisateur, id_magasin, date_achat, total_saisi) " &
                        "VALUES (@u, @m, @d, @t)", cn, tx)
                        cmd.Parameters.AddWithValue("@u", idUtilisateur)
                        cmd.Parameters.AddWithValue("@m", idMagasin)
                        cmd.Parameters.AddWithValue("@d", dateAchat.Date)
                        cmd.Parameters.AddWithValue("@t", totalSaisi)
                        cmd.ExecuteNonQuery()
                        idTicket = CInt(cmd.LastInsertedId)
                    End Using

                    For Each l As LigneTicketSaisie In lignes
                        Dim idLigne As Integer
                        Using cmd As New MySqlCommand(
                            "INSERT INTO ligne_ticket (id_ticket, id_produit, quantite_conditionnement, unite, prix, nb_achete) " &
                            "VALUES (@tk, @p, @q, @un, @pr, @nb)", cn, tx)
                            cmd.Parameters.AddWithValue("@tk", idTicket)
                            cmd.Parameters.AddWithValue("@p", l.IdProduit)
                            cmd.Parameters.AddWithValue("@q", l.Conditionnement)
                            cmd.Parameters.AddWithValue("@un", l.Unite)
                            cmd.Parameters.AddWithValue("@pr", l.Prix)
                            cmd.Parameters.AddWithValue("@nb", l.NbAchete)
                            cmd.ExecuteNonQuery()
                            idLigne = CInt(cmd.LastInsertedId)
                        End Using

                        ' Une observation de prix par ligne : base de l'historique et des alertes
                        Using cmd As New MySqlCommand(
                            "INSERT INTO observation_prix (id_utilisateur, id_produit, id_magasin, id_ligne_ticket, " &
                            "date_obs, prix_unitaire, quantite_base) " &
                            "VALUES (@u, @p, @m, @l, @d, @pu, @qb)", cn, tx)
                            cmd.Parameters.AddWithValue("@u", idUtilisateur)
                            cmd.Parameters.AddWithValue("@p", l.IdProduit)
                            cmd.Parameters.AddWithValue("@m", idMagasin)
                            cmd.Parameters.AddWithValue("@l", idLigne)
                            cmd.Parameters.AddWithValue("@d", dateAchat.Date)
                            cmd.Parameters.AddWithValue("@pu", Math.Round(l.PrixUnitaire, 4))
                            cmd.Parameters.AddWithValue("@qb", Math.Round(l.QuantiteBase, 3))
                            cmd.ExecuteNonQuery()
                        End Using
                    Next

                    tx.Commit()
                    Return idTicket
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function
End Class