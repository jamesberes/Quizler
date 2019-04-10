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
    public class IDeckDALTests
    {
        string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=FlashCards;Integrated Security=True";
        TransactionScope tran;
        IDeckDAL dal;
        ICardDAL cardDal;
        int testDeckId;
        Deck testDeck;
        int usersDeckCount;


        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();
            dal = new DeckSqlDAL(connectionString);
            cardDal = new CardSqlDAL(connectionString);
            usersDeckCount = dal.GetDecksbyUserId(1).Count;
            testDeck = new Deck()
            {
                Name = "Test Deck",
                Description = "Test Decription",
                DateCreated = DateTime.Now,
                PublicDeck = false,
                ForReview = false,
                UserId = 1
            };
            testDeckId = dal.CreateDeck(testDeck);
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod] //CreateDeck
        public void AddDeckReturnsDeckId()
        {
            //Assert
            Assert.AreNotEqual(0, testDeckId);
        }

        [TestMethod] //GetDeckById
        public void GetDeckByIdReturnsCorrectDeck()
        {
            //Act
            Deck result = dal.GetDeckById(testDeckId);

            //Assert
            Assert.AreEqual(result.Name, testDeck.Name);
            Assert.AreEqual(result.Description, testDeck.Description);
        }

        [TestMethod]
        public void GetDecksByUserIdReturnsListWithNewDeck()
        {
            //usersDeckCount was set before adding the test deck
            //result should be usersDeckCount + 1
            int newCount = dal.GetDecksbyUserId(1).Count;
            List<Deck> result = dal.GetDecksbyUserId(1);
            Assert.AreEqual(usersDeckCount + 1, newCount);
            Assert.AreEqual(result[result.Count - 1].Name, testDeck.Name);
        }

        [TestMethod] //Deck UpdateDeck(Deck updatedDeck)
        public void UpdateDeckShouldReturnUpdatedDeck()
        {
            // Get new ID
            testDeck.Name = "updated name";
            testDeck.Description = "updated description";

            Deck result = dal.UpdateDeck(testDeck);

            Assert.AreEqual("updated name", result.Name);
            Assert.AreEqual("updated description", result.Description);
        }

        [TestMethod] //bool DeleteDeck(int deckId);
        public void DeleteDeckTest()
        {
            Assert.AreEqual(false, dal.DeleteDeck(testDeckId + 1), "Result should be false because the test card should be the highest Id currently in the database");
            Assert.AreEqual(true, dal.DeleteDeck(testDeckId));
        }
    }
}