using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using MoneyManager.ViewModels.Data;
using System;
using System.Threading.Tasks;

namespace MoneyManager.Windows.Test.ViewModels
{
    [TestClass]
    public class RecurrenceTransactionViewModelTest
    {
        private Account account;
        private FinancialTransaction transaction;
        private RecurringTransaction recurringTransaction;

        private RecurrenceTransactionViewModel recurrenceTransactionViewModel
        {
            get { return new ViewModelLocator().RecurrenceTransactionViewModel; }
        }

        [TestInitialize]
        public async Task InitTest()
        {
            await DatabaseHelper.CreateDatabase();

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.DeleteAll<RecurringTransaction>();
            }

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

            recurrenceTransactionViewModel.Save(recurringTransaction);

            Assert.IsTrue(recurrenceTransactionViewModel.AllTransactions.Contains(recurringTransaction));
        }

        [TestMethod]
        public void LoadRecurrenceListTest()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                recurrenceTransactionViewModel.Save(recurringTransaction);
                recurrenceTransactionViewModel.Save(recurringTransaction);
                Assert.AreEqual(2, recurrenceTransactionViewModel.AllTransactions.Count);

                recurrenceTransactionViewModel.AllTransactions = null;
                recurrenceTransactionViewModel.LoadList();
                Assert.AreEqual(2, recurrenceTransactionViewModel.AllTransactions.Count);
            }
        }

        [TestMethod]
        public void UpdateRecurringTransactionTest()
        {
            recurrenceTransactionViewModel.Save(recurringTransaction);
            Assert.IsTrue(recurrenceTransactionViewModel.AllTransactions.Contains(recurringTransaction));

            var newStartdate = new DateTime(1992, 1, 31);
            recurringTransaction.StartDate = newStartdate;
            recurrenceTransactionViewModel.Update(recurringTransaction);

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                Assert.AreEqual(newStartdate, dbConn.Table<RecurringTransaction>().First().StartDate);
            }
        }

        [TestMethod]
        public void DeleteRecurrenceTransactionTest()
        {
            recurrenceTransactionViewModel.Save(recurringTransaction);
            Assert.IsTrue(recurrenceTransactionViewModel.AllTransactions.Contains(recurringTransaction));

            recurrenceTransactionViewModel.Delete(recurringTransaction);
            Assert.IsFalse(recurrenceTransactionViewModel.AllTransactions.Contains(recurringTransaction));
        }
    }
}