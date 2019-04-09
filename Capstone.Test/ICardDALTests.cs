using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using Capstone.Models.DALs;
using Capstone.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

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
                Id = 0,
                Front = "testFront",
                Back = "testBack",
                CardOrder = 1,
                DeckId = testDeckId,
                ImageURL = ""
            };

            card = dal.AddCardToDeck(card);

            Assert.AreNotEqual(0, card.Id);
        }

        [TestMethod]
        public void UpdateCardShouldReturnUpdatedCard()
        {
            Card card = new Card()
            {
                Front = "testFront",
                Back = "testBack",
                CardOrder = 1,
                DeckId = testDeckId,
                ImageURL = "",
            };

            // Get new ID
            card = dal.AddCardToDeck(card);

            card.Front = "Updated Front";
            card.Back = "Updated Back";

            Card result = dal.UpdateCard(card);

            Assert.AreEqual("Updated Front", result.Front);
            Assert.AreEqual("Updated Back", result.Back);
        }

        [TestMethod]
        public void GetCardByIdShouldReturnCorrectCard()
        {
            Card card = new Card()
            {
                Front = "testFront",
                Back = "testBack",
                CardOrder = 1,
                DeckId = testDeckId,
                ImageURL = "",
            };

            // Get new ID
            card = dal.AddCardToDeck(card);

            Card result = dal.GetCardById(card.Id);

            Assert.AreEqual(card.Front, result.Front);
            Assert.AreEqual(card.Back, result.Back);
            Assert.AreEqual(card.Id, result.Id);
            Assert.AreEqual(card.DeckId, result.DeckId);

        }

        [TestMethod]
        public void GetCardByDeckIdShouldReturnCorrectCards()
        {
            List<Card> cards = new List<Card>();

            Card card = new Card()
            {
                Front = "testFront",
                Back = "testBack",
                CardOrder = 1,
                DeckId = testDeckId,
                ImageURL = "",
            };

            cards.Add(card);
            dal.AddCardToDeck(card);

            card = new Card()
            {
                Front = "testFront 2",
                Back = "testBack 2",
                CardOrder = 2,
                DeckId = testDeckId,
                ImageURL = "",
            };

            cards.Add(card);
            dal.AddCardToDeck(card);

            List<Card> result = dal.GetCardsByDeckId(testDeckId);

            Assert.AreEqual(cards[0].Front, result[0].Front);
            Assert.AreEqual(cards[0].Id, result[0].Id);
            Assert.AreEqual(cards[1].Front, result[1].Front);
            Assert.AreEqual(cards[1].Id, result[1].Id);
        }

        [TestMethod]
        public void DeleteCardTests()
        {
            Card card = new Card()
            {
                Front = "testFront",
                Back = "testBack",
                CardOrder = 1,
                DeckId = testDeckId,
                ImageURL = "",
            };

            card = dal.AddCardToDeck(card);

            Assert.AreEqual(false, dal.DeleteCard(card.Id + 1), "Result should be false because the test card should be the highest Id currently in the database");
            Assert.AreEqual(true, dal.DeleteCard(card.Id));
        }
    }
}
