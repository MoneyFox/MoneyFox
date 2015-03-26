#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Helper;
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
        public void CrudTransactionTest() {
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
        [Ignore]
        public void GetUnclearedTransactionsTest() {
            var transactionDataAccess = new TransactionDataAccess();

            DateTime date = DateTime.Today.AddDays(-1);
            transactionDataAccess.Save(new FinancialTransaction {
                ChargedAccountId = 4,
                Amount = 55,
                Date = date,
                Note = "this is a note!!!",
                Cleared = false
            }
                );

            IEnumerable<FinancialTransaction> transactions = transactionDataAccess.GetUnclearedTransactions();

            Assert.AreEqual(1, transactions.Count());

            DateTime date2 = DateTime.Today.AddMonths(1);
            transactionDataAccess.Save(new FinancialTransaction {
                ChargedAccountId = 4,
                Amount = 55,
                Date = date2,
                Note = "this is a note!!!",
                Cleared = false
            }
                );

            transactions = transactionDataAccess.GetUnclearedTransactions();
            Assert.AreEqual(1, transactions.Count());

            transactions = transactionDataAccess.GetUnclearedTransactions(Utilities.GetEndOfMonth());
            Assert.AreEqual(1, transactions.Count());
        }
    }
}