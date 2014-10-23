using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using System;
using System.Linq;

namespace MoneyManager.WindowsPhone.Test.ViewModels
{
    [TestClass]
    internal class TransactionDataAccessTest
    {
        private Account account;
        private FinancialTransaction transaction;

        private AccountDataAccess AccountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        private TransactionDataAccess transactionDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        [TestInitialize]
        public void InitTests()
        {
            DatabaseLogic.CreateDatabase();

            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                dbConn.DeleteAll<FinancialTransaction>();
                transactionDataAccess.AllTransactions.Clear();
            }

            CreateTestData();
        }

        private void CreateTestData()
        {
            account = new Account
            {
                CurrentBalance = 20,
                Iban = "CHF20 0000 00000 000000",
                Name = "Testkonto",
                Note = "this is a note"
            };

            AccountDataAccess.Save(account);

            transaction = new FinancialTransaction
            {
                ChargedAccountId = account.Id,
                Amount = 22.50,
                Date = DateTime.Today,
                Note = "this is a note!!!",
                Cleared = false
            };
        }

        [TestMethod]
        public void SaveTransactionTest()
        {
            transactionDataAccess.Save(transaction);

            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                var saved = dbConn.Table<FinancialTransaction>().ToList().First();
                Assert.IsTrue(saved.ChargedAccountId == transaction.ChargedAccountId
                              && saved.Amount == transaction.Amount && saved.Date == transaction.Date
                              && saved.Note == transaction.Note);
            }
        }

        [TestMethod]
        public void LoadTransactionListTest()
        {
            transactionDataAccess.Save(transaction);
            transactionDataAccess.Save(transaction);
            Assert.AreEqual(2, transactionDataAccess.AllTransactions.Count);

            transactionDataAccess.AllTransactions = null;
            transactionDataAccess.LoadList();
            Assert.AreEqual(2, transactionDataAccess.AllTransactions.Count);
        }

        [TestMethod]
        public void UpdateFinancialTransactionTest()
        {
            transactionDataAccess.Save(transaction);
            Assert.IsTrue(transactionDataAccess.AllTransactions.Contains(transaction));

            double newAmount = 108.3;
            transaction.Amount = newAmount;
            transactionDataAccess.Update(transaction);

            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                Assert.AreEqual(newAmount, dbConn.Table<FinancialTransaction>().First().Amount);
            }
        }

        [TestMethod]
        public void DeleteFinancialTransactionTest()
        {
            transactionDataAccess.Save(transaction);
            Assert.IsTrue(transactionDataAccess.AllTransactions.Contains(transaction));

            transactionDataAccess.Delete(transaction, true);
            Assert.IsFalse(transactionDataAccess.AllTransactions.Contains(transaction));
        }

        [TestMethod]
        public void GetUnclearedTransactionsTest()
        {
            var firstTransaction = transaction;
            transactionDataAccess.Save(firstTransaction);

            var secondTransaction = new FinancialTransaction { Amount = 80, Date = DateTime.Now, Cleared = false };
            using (var db = SqlConnectionFactory.GetSqlConnection())
            {
                db.Insert(secondTransaction);
            }

            var unclearedList = transactionDataAccess.GetUnclearedTransactions();
            Assert.AreEqual(1, unclearedList.Count());
            var loadedTransaction = unclearedList.First();
            Assert.IsTrue(loadedTransaction.Id == secondTransaction.Id
                          && loadedTransaction.Amount == secondTransaction.Amount
                          && loadedTransaction.Date.Date == secondTransaction.Date.Date);
        }
    }
}