using Capstone.Models;
using Capstone.Models.DALs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace Capstone.Test
{
    [TestClass]
    public class ITagDALTests
    {
        string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=FlashCards;Integrated Security=True";
        TransactionScope tran;
        ITagDAL dal;
        int testDeckId;
        int testCardId;


        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();
            dal = new TagSqlDAL(connectionString);

            //Add a test deck
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                Deck test = new Deck()
                {
                    Name = "Test Deck",
                    Description = "Test Decription",
                    DateCreated = DateTime.Now,
                    PublicDeck = false,
                    ForReview = false,
                    UserId = 1
                };

                SqlCommand cmd = new SqlCommand("INSERT INTO decks (name, description, date_created, is_public, for_review, users_id) VALUES (@name, @desc, @date_created, @is_public, @for_review, @user_id); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                cmd.Parameters.AddWithValue("@name", test.Name);
                cmd.Parameters.AddWithValue("@desc", test.Description);
                cmd.Parameters.AddWithValue("@date_created", test.DateCreated);
                cmd.Parameters.AddWithValue("@is_public", test.PublicDeck);
                cmd.Parameters.AddWithValue("@for_review", test.ForReview);
                cmd.Parameters.AddWithValue("@user_id", test.UserId);

                testDeckId = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("INSERT INTO cards (front, back, img, card_order, deck_id) VALUES (@front, @back, @img, @card_order, @deck_id); SELECT CAST(SCOPE_IDENTITY() AS INT);", conn);
                cmd.Parameters.AddWithValue("@front", "Test Front");
                cmd.Parameters.AddWithValue("@back", "Test Back");
                cmd.Parameters.AddWithValue("@img", "");
                cmd.Parameters.AddWithValue("@card_order", "1");
                cmd.Parameters.AddWithValue("@deck_id", testDeckId);

                testCardId = (int)cmd.ExecuteScalar();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void AddTagTests()
        {
            Tag tag = new Tag()
            {
                CardId = testCardId,
                Name = "Testing"
            };

            tag = dal.AddTag(tag);

            Assert.IsNotNull(tag.Id);

            Tag manualTag = new Tag();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM tags WHERE id = @id;", conn);
                cmd.Parameters.AddWithValue("@id", tag.Id);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    manualTag.Id = Convert.ToInt32(reader["id"]);
                    manualTag.Name = Convert.ToString(reader["tag"]);
                    manualTag.CardId = Convert.ToInt32(reader["card_id"]);
                }
            }

            Assert.AreEqual(tag.Id, manualTag.Id, "id");
            Assert.AreEqual(tag.Name, manualTag.Name, "name");
            Assert.AreEqual(tag.CardId, manualTag.CardId, "card id");
        }

        [TestMethod]
        public void GetTagsForDeckTests()
        {
            Tag tag = new Tag()
            {
                CardId = testCardId,
                Name = "Testing"
            };

            dal.AddTag(tag);
            List<Tag> results = dal.GetTagsForDeck(testDeckId);

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(tag.Name, results[0].Name);
            Assert.AreEqual(tag.CardId, results[0].CardId);
        }

        [TestMethod]
        public void GetTagsForCardTests()
        {
            Tag tag = new Tag()
            {
                CardId = testCardId,
                Name = "Testing"
            };

            dal.AddTag(tag);
            List<Tag> results = dal.GetTagsForCard(testCardId);

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(tag.Name, results[0].Name);
            Assert.AreEqual(tag.CardId, results[0].CardId);
        }

        [TestMethod]
        public void AddTagListTests()
        {
            List<Tag> tags = new List<Tag>();

            Tag tag = new Tag()
            {
                CardId = testCardId,
                Name = "Testing"
            };

            tags.Add(tag);

            tag = new Tag()
            {
                Name = "Testing 2",
                CardId = testCardId
            };

            tags.Add(tag);

            tags = dal.AddTagList(tags);

            List<Tag> manualTagList = new List<Tag>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM tags WHERE card_id = @id;", conn);
                cmd.Parameters.AddWithValue("@id", testCardId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Tag t = new Tag();
                    t.Id = Convert.ToInt32(reader["id"]);
                    t.Name = Convert.ToString(reader["tag"]);
                    t.CardId = Convert.ToInt32(reader["card_id"]);
                    manualTagList.Add(t);
                }
            }

            Assert.AreEqual(tags[0].Id, manualTagList[0].Id, "id");
            Assert.AreEqual(tags[0].Name, manualTagList[0].Name, "name");
            Assert.AreEqual(tags[0].CardId, manualTagList[0].CardId, "card id");

            Assert.AreEqual(tags[1].Id, manualTagList[1].Id, "id 2");
            Assert.AreEqual(tags[1].Name, manualTagList[1].Name, "name 2");
            Assert.AreEqual(tags[1].CardId, manualTagList[1].CardId, "card id 2");
        }
    }
}
