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
        ITagDAL tagSqlDAL;

        private const string sql_AddCardToDeck = @"INSERT INTO cards (front, back, img, card_order, deck_id) VALUES (@front, @back, @img, @card_order, @deck_id); SELECT CAST(SCOPE_IDENTITY() AS INT);";
        private const string sql_GetCardsByDeckId = @"SELECT * FROM cards WHERE deck_id = @deckId ORDER BY card_order;";
        private const string sql_UpdateCard = @"UPDATE cards SET front = @front, back = @back, img = @img, card_order = @order WHERE id = @id;";
        private const string sql_GetCardById = @"SELECT * FROM cards WHERE id = @id;";
        private const string sql_DeleteCard = @"DELETE FROM tags where card_id = @id; DELETE FROM cards WHERE id = @id";
        private const string sql_SearchForCard = @"SELECT DISTINCT(cards.id) FROM cards JOIN tags ON cards.id = tags.card_id JOIN decks ON cards.deck_id = decks.id JOIN users ON users.id = decks.users_id WHERE (tags.tag LIKE '%' + @tag + '%' OR cards.front LIKE '%' + @tag + '%') AND (users.is_admin = 1 OR decks.is_public = 1);";
        private const string sql_ReorderDeck = "UPDATE cards SET card_order = @card_order WHERE id = @id;";

        public CardSqlDAL(string connectionString)
        {
            ConnectionString = connectionString;
            tagSqlDAL = new TagSqlDAL(connectionString);
        }

        public Card AddCardToDeck(Card card)
        {
            Card output = card;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql_AddCardToDeck, conn);
                cmd.Parameters.AddWithValue("@front", card.Front);
                cmd.Parameters.AddWithValue("@back", card.Back);
                cmd.Parameters.AddWithValue("@img", card.ImageURL);
                cmd.Parameters.AddWithValue("@card_order", card.CardOrder);
                cmd.Parameters.AddWithValue("@deck_id", card.DeckId);

                try
                {
                    output.Id = (int)cmd.ExecuteScalar();
                    if (output.Tags != null)
                    {
                        foreach (var tag in output.Tags)
                        {
                            tag.CardId = output.Id;
                        }
                    }
                    else
                    {
                        output.Tags = new List<Tag>();
                    }
                    tagSqlDAL.AddTagList(output.Tags);
                }
                catch (Exception e)
                {
                    output = new Card();
                }
            }
            return output;
        }

        public List<Card> AddCardListToDeck(List<Card> cards)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                foreach (Card card in cards)
                {
                    SqlCommand cmd = new SqlCommand(sql_AddCardToDeck, conn);
                    cmd.Parameters.AddWithValue("@front", card.Front);
                    cmd.Parameters.AddWithValue("@back", card.Back);
                    cmd.Parameters.AddWithValue("@img", card.ImageURL);
                    cmd.Parameters.AddWithValue("@card_order", card.CardOrder);
                    cmd.Parameters.AddWithValue("@deck_id", card.DeckId);

                    try
                    {
                        card.Id = (int)cmd.ExecuteScalar();
                        if (card.Tags != null)
                        {
                            foreach (var tag in card.Tags)
                            {
                                tag.CardId = card.Id;
                            }
                        }
                        else
                        {
                            card.Tags = new List<Tag>();
                        }
                        tagSqlDAL.AddTagList(card.Tags);
                    }
                    catch (Exception e)
                    {
                        cards = null;
                    }
                }
            }
            return cards;
        }

        public List<Card> GetCardsByDeckId(int deckId)
        {
            List<Card> result = new List<Card>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetCardsByDeckId, conn);
                    cmd.Parameters.AddWithValue("@deckId", deckId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Card card = new Card
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Front = Convert.ToString(reader["front"]),
                            Back = Convert.ToString(reader["back"]),
                            ImageURL = Convert.ToString(reader["img"]),
                            DeckId = Convert.ToInt32(reader["deck_id"]),
                            CardOrder = Convert.ToInt32(reader["card_order"])
                        };

                        card.Tags = tagSqlDAL.GetTagsForCard(card.Id);

                        result.Add(card);
                    }
                }
            }
            catch (SqlException ex)
            {
                result = new List<Card>();
            }

            return result;

        }

        public Card UpdateCard(Card updatedCard)
        {
            Card output;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                // Check if order has been changed. If it has, change the rest of the deck accordingly
                if (GetCardById(updatedCard.Id).CardOrder != updatedCard.CardOrder)
                {
                    try
                    {
                        List<Card> updatedDeck = ReorderDeck(GetCardsByDeckId(updatedCard.DeckId), updatedCard);

                        foreach (Card card in updatedDeck)
                        {
                            SqlCommand cmd = new SqlCommand(sql_ReorderDeck, conn);
                            cmd.Parameters.AddWithValue("@card_order", card.CardOrder);
                            cmd.Parameters.AddWithValue("@id", card.Id);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                }

                try
                {

                    SqlCommand cmd = new SqlCommand(sql_UpdateCard, conn);

                    if (String.IsNullOrEmpty(updatedCard.Front))
                    {
                        updatedCard.Front = "";
                    }
                    cmd.Parameters.AddWithValue("@front", updatedCard.Front);
                    cmd.Parameters.AddWithValue("@back", updatedCard.Back);
                    if (String.IsNullOrEmpty(updatedCard.ImageURL))
                    {
                        updatedCard.ImageURL = "";
                    }
                    cmd.Parameters.AddWithValue("@img", updatedCard.ImageURL);
                    cmd.Parameters.AddWithValue("@id", updatedCard.Id);
                    cmd.Parameters.AddWithValue("@order", updatedCard.CardOrder);

                    int numRowsChanged = cmd.ExecuteNonQuery();
                    if (numRowsChanged > 0)
                    {
                        output = updatedCard;
                    }
                    else
                    {
                        output = null;
                    }

                }
                catch (Exception e)
                {
                    output = null;
                }
            }
            return output;
        }

        public bool DeleteCard(int cardId)
        {
            bool output;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_DeleteCard, conn);
                    cmd.Parameters.AddWithValue("@id", cardId);

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
            catch
            {

                output = false;
            }

            return output;
        }

        public Card GetCardById(int cardId)
        {
            Card output = new Card();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_GetCardById, conn);
                    cmd.Parameters.AddWithValue("@id", cardId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output.Id = Convert.ToInt32(reader["id"]);
                        output.Front = Convert.ToString(reader["front"]);
                        output.Back = Convert.ToString(reader["back"]);
                        output.ImageURL = Convert.ToString(reader["img"]);
                        output.DeckId = Convert.ToInt32(reader["deck_id"]);
                        output.CardOrder = Convert.ToInt32(reader["card_order"]);
                    }
                    output.Tags = tagSqlDAL.GetTagsForCard(output.Id);
                }
            }
            catch
            {
                output = new Card();
            }
            return output;
        }

        public HashSet<int> SearchForCard(Tag tag)
        {
            HashSet<int> output = new HashSet<int>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql_SearchForCard, conn);
                    cmd.Parameters.AddWithValue("@tag", tag.Name);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Card card = new Card();
                        output.Add(card.Id = Convert.ToInt32(reader["id"]));
                    }
                }
            }
            catch
            {
                output = null;
            }
            return output;
        }

        private List<Card> ReorderDeck(List<Card> cards, Card updatedCard)
        {
            int originalIndex = cards.FindIndex(c => c.Id == updatedCard.Id);

            // Remove the updated card from the array
            cards.RemoveAt(originalIndex);

            // Find where the card should now go
            int updatedIndex = cards.FindIndex(c => c.CardOrder == updatedCard.CardOrder);

            // If updatedIndex cannot be found in the original cards, the new index must be higher than the highest card
            if (updatedIndex == -1)
            {
                cards.Add(updatedCard);
            }
            else if (originalIndex > updatedIndex)
            {
                cards.Insert(updatedIndex, updatedCard);

                for (int i = updatedIndex + 1; i <= originalIndex; i++)
                {
                    cards[i].CardOrder++;
                }
            }
            else if (updatedIndex > originalIndex)
            {
                for (int i = originalIndex; i < updatedIndex; i++)
                {
                    cards[i].CardOrder--;
                }
            }

            return cards;
        }
    }
}