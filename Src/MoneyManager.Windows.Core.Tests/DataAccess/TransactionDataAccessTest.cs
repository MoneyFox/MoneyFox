using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Core;
using MoneyManager.Core.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Core.Tests.Helper;
using SQLite.Net.Platform.WinRT;

namespace MoneyManager.Windows.Core.Tests.DataAccess
{
    [TestClass]
    public class TransactionDataAccessTest
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void TransactionDataAccess_CrudTransaction()
        {
            var transactionDataAccess =
                new TransactionDataAccess(new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath()));

            const double firstAmount = 76.30;
            const double secondAmount = 22.90;

            var transaction = new FinancialTransaction
            {
                ChargedAccount = new Account {Id = 4},
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

            transactionDataAccess.Save(transaction);

            transactionDataAccess.LoadList();
            list = transactionDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(secondAmount, list.First().Amount);

            transactionDataAccess.Delete(transaction);

            transactionDataAccess.LoadList();
            list = transactionDataAccess.LoadList();
            Assert.IsFalse(list.Any());
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TransactionDataAccess_CrudTransactionWithoutAccount()
        {
            var transactionDataAccess =
                new TransactionDataAccess(new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath()));

            const double firstAmount = 76.30;
            const double secondAmount = 22.90;

            var transaction = new FinancialTransaction
            {
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

            transactionDataAccess.Save(transaction);

            transactionDataAccess.LoadList();
            list = transactionDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(secondAmount, list.First().Amount);

            transactionDataAccess.Delete(transaction);

            transactionDataAccess.LoadList();
            list = transactionDataAccess.LoadList();
            Assert.IsFalse(list.Any());
        }
    }
}