using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using MoneyManager.ViewModels.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Windows.Test.ViewModels
{
    [TestClass]
    public class TransactionViewModelTest
    {
        private Account account;
        private FinancialTransaction transaction;

        private AccountViewModel accountViewModel
        {
            get { return new ViewModelLocator().AccountViewModel; }
        }

        private TransactionViewModel transactionViewModel
        {
            get { return new ViewModelLocator().TransactionViewModel; }
        }

        [TestInitialize]
        public async Task InitTests()
        {
            await DatabaseHelper.CreateDatabase();

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.DeleteAll<FinancialTransaction>();
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

            accountViewModel.Save(account);

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
            transactionViewModel.Save(transaction);

            using (var dbConn = ConnectionFactory.GetDbConnection())
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
            transactionViewModel.Save(transaction);
            transactionViewModel.Save(transaction);
            Assert.AreEqual(2, transactionViewModel.AllTransactions.Count);

            transactionViewModel.AllTransactions = null;
            accountViewModel.LoadList();
            Assert.AreEqual(2, transactionViewModel.AllTransactions.Count);
        }

        [TestMethod]
        public void UpdateFinancialTransactionTest()
        {
            transactionViewModel.Save(transaction);
            Assert.IsTrue(transactionViewModel.AllTransactions.Contains(transaction));

            double newAmount = 108.3;
            transaction.Amount = newAmount;
            transactionViewModel.Update(transaction);

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                Assert.AreEqual(newAmount, dbConn.Table<FinancialTransaction>().First().Amount);
            }
        }

        [TestMethod]
        public void DeleteFinancialTransactionTest()
        {
            transactionViewModel.Save(transaction);
            Assert.IsTrue(transactionViewModel.AllTransactions.Contains(transaction));

            transactionViewModel.Delete(transaction);
            Assert.IsFalse(transactionViewModel.AllTransactions.Contains(transaction));
        }

        [TestMethod]
        public void GetUnclearedTransactionsTest()
        {
            var firstTransaction = transaction;
            transactionViewModel.Save(firstTransaction);

            var secondTransaction = new FinancialTransaction { Amount = 80, Date = DateTime.Now.AddDays(1) };
            transactionViewModel.Save(secondTransaction);

            var unclearedList = transactionViewModel.GetUnclearedTransactions();
            Assert.AreEqual(unclearedList.Count, 1);
            var loadedTransaction = unclearedList.First();
            Assert.IsTrue(loadedTransaction.Id == firstTransaction.Id
                && loadedTransaction.Amount == firstTransaction.Amount
                && loadedTransaction.Date == firstTransaction.Date);
        }
    }
}