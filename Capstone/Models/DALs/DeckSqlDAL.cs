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
        //private ICardDAL = new cardSqlDAL(connectionString);

        private const string sql_CreateDeck = @"insert into decks (name, user, description) values (@name, @user, @description);";
        private const string sql_GetDeckById = @"SELECT * FROM decks WHERE id = @id";
        //private const string sql_GetRandomDeck = "";
        //private const string sql_GetDecksbyUserId = "";


        public DeckSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //bool CreateDeck();
        public bool CreateDeck(Deck newDeck)
        {
            bool result = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_CreateDeck, conn);

                    cmd.Parameters.AddWithValue("@name", newDeck.Name);
                    cmd.Parameters.AddWithValue("@user", newDeck.UserId);
                    cmd.Parameters.AddWithValue("@description", newDeck.Description);

                    int alteredRows = cmd.ExecuteNonQuery();

                    if (alteredRows > 0)
                    {
                        result = true;
                    }
                }
            }
            catch
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
                Deck deck = new Deck();
            }
        }

        //Deck GetRandomDeck();
        //List<Deck> GetDecksbyUserId();
    }
}
