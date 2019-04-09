using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using Capstone.Models.DALs;
using Capstone.Models;
using System.Data.SqlClient;
using System;

namespace Capstone.Test
{
    [TestClass]
    public class ICardDALTests
    {
        string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=FlashCards;Integrated Security=True";
        TransactionScope tran;
        ICardDAL dal;
        int testDeckId;


        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();
            dal = new CardSqlDAL(connectionString);
            
            //Add a test deck
            using(SqlConnection conn = new SqlConnection(connectionString))
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
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void AddCardToDeckShouldReturnCardWithID()
        {
            Card card = new Card()
            {
                ID = 0,
                Front = "testFront",
                Back = "testBack",
                CardOrder = 1,
                DeckID = testDeckId,
                ImageURL = ""
            };

            card = dal.AddCardToDeck(card);

            Assert.AreNotEqual(0, card.ID);
        }

        [TestMethod]
        public void UpdateCardShouldReturnTrueIfSuccessful()
        {
            Card card = new Card()
            {
                Front = "testFront",
                Back = "testBack",
                CardOrder = 1,
                DeckID = testDeckId,
                ImageURL = ""
            };

            bool result = dal.UpdateCard(card);
        }
    }
}
