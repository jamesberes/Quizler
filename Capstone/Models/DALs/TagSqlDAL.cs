using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.DALs
{
    public class TagSqlDAL : ITagDAL
    {
        private string connectionString;

        public TagSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private readonly string sql_AddTag = "INSERT INTO tags (tag, card_id) VALUES (@tag, @card_id); SELECT CAST(SCOPE_IDENTITY() AS INT);";
        private readonly string sql_GetTagsForCard = "SELECT * FROM tags WHERE card_id = @card_id;";
        private readonly string sql_GetTagsForDeck = "SELECT tags.id, tags.tag, tags.card_id from tags JOIN cards ON tags.card_id = cards.id JOIN decks ON cards.deck_id = decks.id WHERE decks.id = @id;";

        public Tag AddTag(Tag input)
        {
            Tag output = null;

            if (!String.IsNullOrWhiteSpace(input.Name))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand(sql_AddTag, conn);
                        cmd.Parameters.AddWithValue("@tag", input.Name);
                        cmd.Parameters.AddWithValue("@card_id", input.CardId);

                        input.Id = (int)cmd.ExecuteScalar();
                        output = input;
                    }
                }
                catch
                {
                    output = null;
                }

            }
            return output;
        }



        public List<Tag> AddTagList(List<Tag> input)
        {
            List<Tag> output = new List<Tag>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (Tag tag in input)
                    {
                        if (!String.IsNullOrWhiteSpace(tag.Name))
                        {
                            SqlCommand cmd = new SqlCommand(sql_AddTag, conn);
                            cmd.Parameters.AddWithValue("@tag", tag.Name);
                            cmd.Parameters.AddWithValue("@card_id", tag.CardId);

                            tag.Id = (int)cmd.ExecuteScalar();
                            output.Add(tag);
                        }
                    }
                }
            }
            catch
            {
                output = null;
            }

            return output;
        }

        public List<Tag> GetTagsForCard(int cardId)
        {
            List<Tag> output = new List<Tag>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetTagsForCard, conn);
                    cmd.Parameters.AddWithValue("@card_id", cardId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Tag t = new Tag()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["tag"]),
                            CardId = Convert.ToInt32(reader["card_id"])
                        };
                        output.Add(t);
                    }
                }
            }
            catch
            {
                output = null;
            }

            return output;
        }

        public List<Tag> GetTagsForDeck(int deckId)
        {
            List<Tag> output = new List<Tag>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetTagsForDeck, conn);
                    cmd.Parameters.AddWithValue("@id", deckId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Tag t = new Tag()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["tag"]),
                            CardId = Convert.ToInt32(reader["card_id"])
                        };
                        output.Add(t);
                    }
                }
            }
            catch
            {
                output = null;
            }

            return output;
        }
    }
}
