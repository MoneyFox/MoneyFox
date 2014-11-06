using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;

namespace MoneyManager.DataAccess.WindowsPhone.Test.DataAccess
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

            transactionDataAccess.LoadList();
            ObservableCollection<FinancialTransaction> list = transactionDataAccess.AllTransactions;

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(firstAmount, list.First().Amount);

            transaction.Amount = secondAmount;

            transactionDataAccess.Update(transaction);

            transactionDataAccess.LoadList();
            list = transactionDataAccess.AllTransactions;

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(secondAmount, list.First().Amount);

            transactionDataAccess.Delete(transaction);

            transactionDataAccess.LoadList();
            list = transactionDataAccess.AllTransactions;
            Assert.IsFalse(list.Any());
        }
    }
}