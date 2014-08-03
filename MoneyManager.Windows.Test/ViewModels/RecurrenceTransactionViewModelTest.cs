using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels.Data;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MoneyManager.Windows.Test.ViewModels
{
    [TestClass]
    public class RecurrenceTransactionViewModelTest
    {
        private Account account;
        private FinancialTransaction transaction;
        private RecurringTransaction recurringTransaction;

        [TestInitialize]
        public async Task InitTest()
        {
            await DatabaseHelper.CreateDatabase();

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.DeleteAll<RecurringTransaction>();
            }

            App.RecurrenceTransactionViewModel = new RecurrenceTransactionViewModel();
            transactionViewModel = new TransactionViewModel
            {
                AllTransactions = new ObservableCollection<FinancialTransaction>()
            };

            CreateTestData();
        }

        private void CreateTestData()
        {
            account = new Account
            {
                CurrentBalance = 20,
                Iban = "CHF20 0000 00000 000000",
                Name = "Sparkonto",
                Note = "this is a note"
            };

            transaction = new FinancialTransaction
            {
                ChargedAccountId = account.Id,
                Amount = 22.50,
                Date = DateTime.Today,
                Note = "this is a note!!!"
            };

            recurringTransaction = new RecurringTransaction
            {
                TransactionId = transaction.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
            };
        }

        [TestMethod]
        public void SaveRecurringTransactionTest()
        {
            var recurringTransaction = new RecurringTransaction
            {
                TransactionId = transaction.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
            };

            App.RecurrenceTransactionViewModel.Save(recurringTransaction);

            Assert.IsTrue(App.RecurrenceTransactionViewModel.AllTransactions.Contains(recurringTransaction));
        }

        [TestMethod]
        public void LoadRecurrenceListTest()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                App.RecurrenceTransactionViewModel.Save(recurringTransaction);
                App.RecurrenceTransactionViewModel.Save(recurringTransaction);
                Assert.AreEqual(2, App.RecurrenceTransactionViewModel.AllTransactions.Count);

                App.RecurrenceTransactionViewModel.AllTransactions = null;
                App.RecurrenceTransactionViewModel.LoadList();
                Assert.AreEqual(2, App.RecurrenceTransactionViewModel.AllTransactions.Count);
            }
        }

        [TestMethod]
        public void UpdateRecurringTransactionTest()
        {
            App.RecurrenceTransactionViewModel.Save(recurringTransaction);
            Assert.IsTrue(App.RecurrenceTransactionViewModel.AllTransactions.Contains(recurringTransaction));

            var newStartdate = new DateTime(1992, 1, 31);
            recurringTransaction.StartDate = newStartdate;
            App.RecurrenceTransactionViewModel.Update(recurringTransaction);

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                Assert.AreEqual(newStartdate, dbConn.Table<RecurringTransaction>().First().StartDate);
            }
        }

        [TestMethod]
        public void DeleteRecurrenceTransactionTest()
        {
            App.RecurrenceTransactionViewModel.Save(recurringTransaction);
            Assert.IsTrue(App.RecurrenceTransactionViewModel.AllTransactions.Contains(recurringTransaction));

            App.RecurrenceTransactionViewModel.Delete(recurringTransaction);
            Assert.IsFalse(App.RecurrenceTransactionViewModel.AllTransactions.Contains(recurringTransaction));
        }
    }
}