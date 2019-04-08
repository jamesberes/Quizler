using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.DALs
{
    public class CardSqlDAL : ICardDAL
    {
        public string ConnectionString { get; }
        private string SQL_AddCardToDeck = "INSERT INTO cards (front, back, img, card_order, deck_id) VALUES (@front, @back, @img, @card_order, @deck_id);";

        public CardSqlDAL(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool AddCardToDeck(int deckID, Card card)
        {
            bool output;

            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(SQL_AddCardToDeck, conn);
                cmd.Parameters.AddWithValue("@front", card.Front);
                cmd.Parameters.AddWithValue("@back", card.Back);
                cmd.Parameters.AddWithValue("@img", card.ImageURL);
                cmd.Parameters.AddWithValue("@card_order", card.CardOrder);
                cmd.Parameters.AddWithValue("@deck_id", deckID);

                try
                {
                    cmd.ExecuteNonQuery();
                    output = true;
                }
                catch(Exception e)
                {
                    output = false;
                }
            }
            return output;
        }
    }
}
