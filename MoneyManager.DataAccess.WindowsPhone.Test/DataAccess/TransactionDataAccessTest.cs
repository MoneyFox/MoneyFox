#region

using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.Model;
using SQLite.Net;

#endregion

namespace MoneyManager.DataAccess.WindowsPhone.Test.DataAccess {
    [TestClass]
    public class TransactionDataAccessTest {
        [TestInitialize]
        public void InitTests() {
            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                db.CreateTable<FinancialTransaction>();
            }
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TransactionDataAccess_CrudTransaction() {
            var transactionDataAccess = new TransactionDataAccess();

            const double firstAmount = 76.30;
            const double secondAmount = 22.90;

            var transaction = new FinancialTransaction {
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
        public void TransactionDataAccess_CrudTransactionWithoutAccount() {
            var transactionDataAccess = new TransactionDataAccess();

            const double firstAmount = 76.30;
            const double secondAmount = 22.90;

            var transaction = new FinancialTransaction {
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