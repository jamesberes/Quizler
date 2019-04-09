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

        private const string sql_CreateDeck = @"insert into decks (name, users_id, description) values (@name, @user, @description); SELECT CAST(SCOPE_IDENTITY() as int);";
        private const string sql_GetDeckById = @"SELECT * FROM decks WHERE id = @id";
        //private const string sql_GetRandomDeck = "";
        private const string sql_GetDecksbyUserId = @"SELECT * FROM decks WHERE users_id = @userId";
        private const string sql_GetHighestOrderNumber = "SELECT TOP 1 cards.card_order FROM decks JOIN cards on decks.id = cards.deck_id WHERE decks.id = @id ORDER BY cards.card_order DESC";


        public DeckSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
            cardSqlDAL = new CardSqlDAL(connectionString);
        }

        //bool CreateDeck();
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

        //List<Deck> GetDecksbyUserId();
        public List<Deck> GetDecksbyUserId(int userId)
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
    }
}
