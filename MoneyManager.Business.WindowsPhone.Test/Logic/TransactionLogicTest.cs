using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Logic;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using SQLite.Net;

namespace MoneyManager.Business.WindowsPhone.Test.Logic {
    [TestClass]
    public class TransactionLogicTest {
        #region properties

        private Account _sampleAccount;

        private static AddTransactionViewModel addTransactionView {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        private static TransactionDataAccess transactionData {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        #endregion

        [TestInitialize]
        public void TestInit() {
            new ViewModelLocator();

            DatabaseLogic.CreateDatabase();

            _sampleAccount = new Account {
                Currency = "CHF",
                IsExchangeModeActive = false,
                CurrentBalance = 700,
                CurrentBalanceWithoutExchange = 700,
                ExchangeRatio = 1,
                Iban = "this is a iban",
                Name = "Jugendkonto",
                Note = "just a note"
            };

            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                db.Insert(_sampleAccount);
            }

            transactionData.AllTransactions = new ObservableCollection<FinancialTransaction>();
        }

        [TestMethod]
        public void GoToAddTransactionTest() {
            TransactionLogic.GoToAddTransaction(TransactionType.Income);

            Assert.IsFalse(addTransactionView.IsEdit);
            Assert.IsFalse(addTransactionView.IsTransfer);
            Assert.AreEqual((int) TransactionType.Income, addTransactionView.SelectedTransaction.Type);
            Assert.IsFalse(addTransactionView.SelectedTransaction.IsExchangeModeActive);

            TransactionLogic.GoToAddTransaction(TransactionType.Transfer);

            Assert.IsFalse(addTransactionView.IsEdit);
            Assert.IsTrue(addTransactionView.IsTransfer);
            Assert.AreEqual((int) TransactionType.Transfer, addTransactionView.SelectedTransaction.Type);
            Assert.IsFalse(addTransactionView.SelectedTransaction.IsExchangeModeActive);
        }

        [TestMethod]
        public void PrepareEditTest() {
            var transaction = new FinancialTransaction {
                Type = (int) TransactionType.Income
            };

            TransactionLogic.PrepareEdit(transaction);

            Assert.IsTrue(addTransactionView.IsEdit);
            Assert.IsFalse(addTransactionView.IsTransfer);

            transaction = new FinancialTransaction {
                Type = (int) TransactionType.Transfer
            };

            TransactionLogic.PrepareEdit(transaction);

            Assert.IsTrue(addTransactionView.IsEdit);
            Assert.IsTrue(addTransactionView.IsTransfer);
        }

        [TestMethod]
        public async Task CrudTransactionTest() {
            var transaction = new FinancialTransaction {
                Amount = 50,
                AmountWithoutExchange = 50,
                Type = (int) TransactionType.Income,
                Category = new Category {
                    Id = 1,
                    Name = "Einkaufen"
                },
                Cleared = true,
                Date = DateTime.Now.AddDays(-1)
            };

            await TransactionLogic.SaveTransaction(transaction);

            Assert.AreEqual(1, transactionData.AllTransactions.Count);
            Assert.AreEqual(transaction, transactionData.AllTransactions.First());

            transaction.Amount = 80;
            transaction.AmountWithoutExchange = 80;

            await TransactionLogic.UpdateTransaction(transaction);

            Assert.AreEqual(1, transactionData.AllTransactions.Count);
            Assert.AreEqual(transaction, transactionData.AllTransactions.First());

            await TransactionLogic.DeleteTransaction(transaction, true);

            Assert.IsFalse(transactionData.AllTransactions.Any());
        }

        [TestMethod]
        [Ignore]
        public void DeleteAssociatedTransactionsFromDatabaseTest() {
            Assert.IsTrue(false);
        }

        [TestMethod]
        [Ignore]
        public async Task ClearTransactionsTest() {
            Assert.IsTrue(false);
        }
    }
}