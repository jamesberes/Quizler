using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using Capstone.Models.DALs;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.Test
{
    [TestClass]
    class ICardDALTests
    {
        string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=FlashCards;Integrated Security=True";
        TransactionScope tran;


        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();
            ICardDAL dal = new CardSqlDAL(connectionString);
            
            //Add a test deck
            using(SqlConnection conn = new SqlConnection(connectionString))
            {

            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void AddCardToDeckShouldReturnTrueIfSuccessful()
        {
            Card card = new Card()
            {
                Front = "testFront",
                Back = "testBack",
                CardOrder = 1,
                DeckID = 1,
            };
        }
    }
}
