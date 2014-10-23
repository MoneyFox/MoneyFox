using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;

namespace MoneyManager.DataAccess.WindowsPhone.Test
{
    [TestClass]
    public class TransactionDataAccessTest
    {
        [TestInitialize]
        public void InitTests()
        {
            using (var db = SqlConnectionFactory.GetSqlConnection())
            {
                db.CreateTable<FinancialTransaction>();
            }
        }

        [TestMethod]
        public void CrudTransactionTest()
        {
            var transactionDataAccess = new TransactionDataAccess();

            const double firstAmount = 76.30;
            const double secondAmount = 22.90;

            var transaction = new FinancialTransaction
            {
                ChargedAccountId = 4,
                Amount = firstAmount,
                Date = DateTime.Today,
                Note = "this is a note!!!",
                Cleared = false
            };

            transactionDataAccess.Save(transaction);

            var list = transactionDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(firstAmount, list.First().Amount);

            transaction.Amount = secondAmount;

            transactionDataAccess.Update(transaction);

            list = transactionDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(secondAmount, list.First().Amount);

            transactionDataAccess.Delete(transaction);

            list = transactionDataAccess.LoadList();
            Assert.IsFalse(list.Any());

        }
    }
}
