using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.DALs
{
    public class DeckSqlDAL : IDeckDAL
    {
        private string connectionString;
        private ICardDAL cardSqlDAL;

        private const string sql_CreateDeck = @"INSERT INTO decks (name, users_id, description) VALUES (@name, @user, @description); SELECT CAST(SCOPE_IDENTITY() as int);";
        private const string sql_GetDeckById = @"SELECT * FROM decks WHERE id = @id";
        //private const string sql_GetRandomDeck = "";
        private const string sql_GetDecksbyUserId = @"SELECT * FROM decks WHERE users_id = @userId";
        private const string sql_UpdateDeck = @"UPDATE decks SET name = @name, description = @description WHERE id = @id";
        private const string sql_GetHighestOrderNumber = "SELECT TOP 1 cards.card_order FROM decks JOIN cards on decks.id = cards.deck_id WHERE decks.id = @id ORDER BY cards.card_order DESC";
        private const string sql_DeleteDeck = "DELETE t FROM tags t JOIN cards c ON t.card_id=c.id WHERE deck_id = @deckId; DELETE FROM cards WHERE deck_id = @deckId; DELETE FROM decks WHERE id = @deckId;";
        private const string sql_GetUserNameFromDeckId = "select display_name from users join decks on users.id = decks.users_id where decks.id = @deckId;";
        private const string sql_LazyLoadDecksByUserId = @"SELECT TOP 10 * FROM decks WHERE users_id = @userId AND id > @deckId";
        private const string sql_LazyLoadPublicDecks = @"SELECT TOP 10 * FROM decks WHERE is_public = 1 AND id > @deckId";
        private const string sql_SetDeckForReview = @"UPDATE decks SET for_review = @bit WHERE id = @deckId;";
        private const string sql_MakePrivate = @"UPDATE decks SET is_public = 0 WHERE id = @deckId;";
        private const string sql_GetAllDecksForReview = @"SELECT * FROM decks WHERE for_review = 1";
        private const string sql_GetAllAdminDecks = @"SELECT * FROM decks JOIN users ON decks.users_id = users.id WHERE users.is_admin = 1;";

        public DeckSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
            cardSqlDAL = new CardSqlDAL(connectionString);
        }

        //int CreateDeck();
        public int CreateDeck(Deck newDeck)
        {
            int result = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_CreateDeck, conn);

                    cmd.Parameters.AddWithValue("@name", newDeck.Name);
                    cmd.Parameters.AddWithValue("@user", newDeck.UserId);
                    cmd.Parameters.AddWithValue("@description", newDeck.Description);

                    newDeck.Id = (int)cmd.ExecuteScalar();

                    if (newDeck.Id > 0)
                    {
                        result = newDeck.Id;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        //Deck GetDeckById();
        public Deck GetDeckById(int deckId)
        {
            Deck result = new Deck();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetDeckById, conn);
                    cmd.Parameters.AddWithValue("@id", deckId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Deck deck = new Deck
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            DateCreated = Convert.ToDateTime(reader["date_created"]),
                            PublicDeck = Convert.ToBoolean(reader["is_public"]),
                            UserId = Convert.ToInt32(reader["users_id"]),
                            ForReview = Convert.ToBoolean(reader["for_review"]),
                            Description = Convert.ToString(reader["description"])
                        };

                        result = deck;
                    }
                }
                result.Cards = cardSqlDAL.GetCardsByDeckId(result.Id);
            }
            catch (SqlException ex)
            {
                //Deck deck = new Deck();
            }
            return result;
        }

        //Deck ViewAllAdminDecks();
        public List<Deck> GetAllAdminDecks()
        {
            List<Deck> result = new List<Deck>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetAllAdminDecks, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Deck deck = new Deck
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            DateCreated = Convert.ToDateTime(reader["date_created"]),
                            PublicDeck = Convert.ToBoolean(reader["is_public"]),
                            UserId = Convert.ToInt32(reader["users_id"]),
                            ForReview = Convert.ToBoolean(reader["for_review"]),
                            Description = Convert.ToString(reader["description"])
                        };
                        deck.Cards = cardSqlDAL.GetCardsByDeckId(deck.Id);
                        result.Add(deck);
                    }
                }
            }
            catch (SqlException ex)
            {
                //Deck deck = new Deck();
            }
            return result;
        }

        //List<Deck> GetDecksbyUserId();
        public List<Deck> GetDecksbyUserId(int userId) //todo userId
        {
            List<Deck> result = new List<Deck>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetDecksbyUserId, conn);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Deck deck = new Deck
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            DateCreated = Convert.ToDateTime(reader["date_created"]),
                            PublicDeck = Convert.ToBoolean(reader["is_public"]),
                            UserId = Convert.ToInt32(reader["users_id"]),
                            ForReview = Convert.ToBoolean(reader["for_review"]),
                            Description = Convert.ToString(reader["description"])
                        };

                        deck.Cards = cardSqlDAL.GetCardsByDeckId(deck.Id);

                        result.Add(deck);
                    }
                }
            }
            catch (SqlException ex)
            {
                List<Deck> deck = new List<Deck>();
            }
            return result;
        }

        //Modify an existing deck
        public Deck UpdateDeck(Deck updatedDeck)
        {
            Deck output;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_UpdateDeck, conn);
                    cmd.Parameters.AddWithValue("@id", updatedDeck.Id);
                    cmd.Parameters.AddWithValue("@name", updatedDeck.Name);
                    cmd.Parameters.AddWithValue("@description", updatedDeck.Description);

                    int numRowsChanged = cmd.ExecuteNonQuery();
                    if (numRowsChanged > 0)
                    {
                        output = updatedDeck;
                    }
                    else
                    {
                        output = null;
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return output;
        }

        //Deck GetRandomDeck();

        public int GetNextCardOrder(int deckId)
        {
            // 1 by default. If there are no cards in a deck the next card would be number 1
            int output = 1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetHighestOrderNumber, conn);
                    cmd.Parameters.AddWithValue("@id", deckId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output = Convert.ToInt32(reader["card_order"]);
                    }
                }
                // Add 1 to return the next card order
                output++;
            }
            catch
            {
                // A return of -1 indicates an error
                output = -1;
            }

            return output;
        }

        public bool DeleteDeck(int deckId)
        {
            bool output = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_DeleteDeck, conn);
                    cmd.Parameters.AddWithValue("@deckId", deckId);

                    int numRowsChanged = cmd.ExecuteNonQuery();
                    if (numRowsChanged > 0)
                    {
                        output = true;
                    }
                    else
                    {
                        output = false;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return output;
        }

        public List<SelectListItem> GetUserDecksSelectList(int userId)
        {
            List<SelectListItem> output = new List<SelectListItem>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetDecksbyUserId, conn);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        SelectListItem deck = new SelectListItem()
                        {
                            Value = Convert.ToString(reader["id"]),
                            Text = Convert.ToString(reader["name"])
                        };

                        output.Add(deck);
                    }
                }
            }
            catch
            {
                output = null;
            }
            return output;
        }

        public string GetUserNameFromDeckId(int deckId)
        {
            string name = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetUserNameFromDeckId, conn);
                    cmd.Parameters.AddWithValue("@deckId", deckId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        name = Convert.ToString(reader["display_name"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                name = null;
            }
            return name;
        }

        public List<Deck> LazyLoadDecks(int userId, int startId)
        {
            List<Deck> result = new List<Deck>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_LazyLoadDecksByUserId, conn);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@deckId", startId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Deck deck = new Deck
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            DateCreated = Convert.ToDateTime(reader["date_created"]),
                            PublicDeck = Convert.ToBoolean(reader["is_public"]),
                            UserId = Convert.ToInt32(reader["users_id"]),
                            ForReview = Convert.ToBoolean(reader["for_review"]),
                            Description = Convert.ToString(reader["description"])
                        };

                        deck.Cards = cardSqlDAL.GetCardsByDeckId(deck.Id);

                        result.Add(deck);
                    }
                }
            }
            catch (SqlException)
            {
                result = null;
            }
            return result;
        }

        public List<Deck> LazyLoadPublicDecks(int startId)
        {
            List<Deck> result = new List<Deck>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_LazyLoadPublicDecks, conn);
                    cmd.Parameters.AddWithValue("@deckId", startId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Deck deck = new Deck
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            DateCreated = Convert.ToDateTime(reader["date_created"]),
                            PublicDeck = Convert.ToBoolean(reader["is_public"]),
                            UserId = Convert.ToInt32(reader["users_id"]),
                            ForReview = Convert.ToBoolean(reader["for_review"]),
                            Description = Convert.ToString(reader["description"])
                        };

                        deck.Cards = cardSqlDAL.GetCardsByDeckId(deck.Id);

                        result.Add(deck);
                    }
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        //sets the selected deck's "For Review" to either 1 or 2
        public bool SetDeckForReferral(int deckId, bool bit)
        {
            bool output;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_SetDeckForReview, conn);
                    cmd.Parameters.AddWithValue("@deckId", deckId);
                    cmd.Parameters.AddWithValue("@bit", bit);

                    int numRowsChanged = cmd.ExecuteNonQuery();
                    if (numRowsChanged > 0)
                    {
                        output = true;
                    }
                    else
                    {
                        output = false;
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return output;
        }

        public bool MakePrivate(int deckId)
        {
            bool output = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_MakePrivate, conn);
                    cmd.Parameters.AddWithValue("@deckId", deckId);

                    int numRowsChanged = cmd.ExecuteNonQuery();
                    if (numRowsChanged > 0)
                    {
                        output = true;
                    }
                    else
                    {
                        output = false;
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return output;
        }

        //List<Deck> GetAllDecksForReview();
        public List<Deck> GetAllDecksForReview()
        {
            List<Deck> result = new List<Deck>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetAllDecksForReview, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Deck deck = new Deck
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            DateCreated = Convert.ToDateTime(reader["date_created"]),
                            PublicDeck = Convert.ToBoolean(reader["is_public"]),
                            UserId = Convert.ToInt32(reader["users_id"]),
                            ForReview = Convert.ToBoolean(reader["for_review"]),
                            Description = Convert.ToString(reader["description"])
                        };

                        deck.Cards = cardSqlDAL.GetCardsByDeckId(deck.Id);

                        result.Add(deck);
                    }
                }
            }
            catch (SqlException ex)
            {
                List<Deck> deck = new List<Deck>();
            }
            return result;
        }
    }
}
