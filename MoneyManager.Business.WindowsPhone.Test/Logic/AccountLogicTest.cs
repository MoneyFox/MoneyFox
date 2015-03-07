#region

using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Logic;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using SQLite.Net;

#endregion

namespace MoneyManager.Business.WindowsPhone.Test.Logic {
    [TestClass]
    public class AccountLogicTest {
        private Account _sampleAccount;
        private Account _sampleAccount2;

        private static AccountDataAccess accountData {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

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
            _sampleAccount2 = new Account {
                Currency = "CHF",
                IsExchangeModeActive = false,
                CurrentBalance = 1200,
                CurrentBalanceWithoutExchange = 1200,
                ExchangeRatio = 1,
                Iban = "this is a second iban",
                Name = "Sparkonto",
                Note = "just a note"
            };
        }

        [TestMethod]
        public void GoToAddAccountTest() {
            AccountLogic.PrepareAddAccount();

            Assert.AreEqual(ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultCurrency,
                accountData.SelectedAccount.Currency);
            Assert.AreEqual(false, accountData.SelectedAccount.IsExchangeModeActive);

            Assert.AreEqual(false, ServiceLocator.Current.GetInstance<AddAccountViewModel>().IsEdit);
        }

        [TestMethod]
        public void DeleteAccountTest() {
            accountData.LoadList();

            var transaction = new FinancialTransaction {
                ChargedAccountId = _sampleAccount.Id,
                Type = (int) TransactionType.Income,
                Currency = "CHF",
                Amount = 100
            };

            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                db.Insert(_sampleAccount);
                db.Insert(transaction);
            }

            AccountLogic.DeleteAccount(_sampleAccount, true);

            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                Assert.IsFalse(db.Table<Account>().Any(x => x.Id == _sampleAccount.Id));
                Assert.IsFalse(db.Table<FinancialTransaction>().Any(x => x.ChargedAccountId == _sampleAccount.Id));
            }
        }

        [TestMethod]
        public async void AddSpendingTransactionAmount() {
            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                db.Insert(_sampleAccount);
            }

            var transaction = new FinancialTransaction {
                ChargedAccount = _sampleAccount,
                Type = (int) TransactionType.Spending,
                Currency = "CHF",
                Amount = 100
            };

            await AccountLogic.AddTransactionAmount(transaction);

            Assert.AreEqual(600, _sampleAccount.CurrentBalance);
            Assert.AreEqual(600, _sampleAccount.CurrentBalanceWithoutExchange);
        }

        [TestMethod]
        public async void AddIncomeTransactionAmount() {
            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                db.Insert(_sampleAccount);
            }

            var transaction = new FinancialTransaction {
                ChargedAccount = _sampleAccount,
                Type = (int) TransactionType.Income,
                Currency = "CHF",
                Amount = 100
            };

            await AccountLogic.AddTransactionAmount(transaction);

            Assert.AreEqual(800, _sampleAccount.CurrentBalance);
            Assert.AreEqual(800, _sampleAccount.CurrentBalanceWithoutExchange);
        }

        [TestMethod]
        public async void AddTransferTransactionAmount() {
            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                db.Insert(_sampleAccount);
                db.Insert(_sampleAccount2);
            }

            var transaction = new FinancialTransaction {
                ChargedAccount = _sampleAccount,
                TargetAccount = _sampleAccount2,
                Type = (int) TransactionType.Transfer,
                Currency = "CHF",
                Amount = 100
            };

            await AccountLogic.AddTransactionAmount(transaction);

            Assert.AreEqual(600, _sampleAccount.CurrentBalance);
            Assert.AreEqual(600, _sampleAccount.CurrentBalanceWithoutExchange);

            Assert.AreEqual(1300, _sampleAccount2.CurrentBalance);
            Assert.AreEqual(1300, _sampleAccount2.CurrentBalanceWithoutExchange);
        }

        [TestMethod]
        public async void RemoveSpendingTransactionAmountTest() {
            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                db.Insert(_sampleAccount);
            }

            var transaction = new FinancialTransaction {
                ChargedAccount = _sampleAccount,
                Type = (int) TransactionType.Spending,
                Currency = "CHF",
                Amount = 100
            };

            await AccountLogic.RemoveTransactionAmount(transaction);

            Assert.AreEqual(800, _sampleAccount.CurrentBalance);
            Assert.AreEqual(800, _sampleAccount.CurrentBalanceWithoutExchange);
        }

        [TestMethod]
        public async void RemoveIncomeTransactionAmountTest() {
            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                db.Insert(_sampleAccount);
            }

            var transaction = new FinancialTransaction {
                ChargedAccount = _sampleAccount,
                Type = (int) TransactionType.Income,
                Currency = "CHF",
                Amount = 100
            };

            await AccountLogic.RemoveTransactionAmount(transaction);

            Assert.AreEqual(600, _sampleAccount.CurrentBalance);
            Assert.AreEqual(600, _sampleAccount.CurrentBalanceWithoutExchange);
        }


        [TestMethod]
        public async void RemoveTransferTransactionAmount() {
            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                db.Insert(_sampleAccount);
                db.Insert(_sampleAccount2);
            }

            var transaction = new FinancialTransaction {
                ChargedAccount = _sampleAccount,
                TargetAccount = _sampleAccount2,
                Type = (int) TransactionType.Transfer,
                Currency = "CHF",
                Amount = 100
            };

            await AccountLogic.RemoveTransactionAmount(transaction);

            Assert.AreEqual(600, _sampleAccount.CurrentBalance);
            Assert.AreEqual(600, _sampleAccount.CurrentBalanceWithoutExchange);

            Assert.AreEqual(1300, _sampleAccount2.CurrentBalance);
            Assert.AreEqual(1300, _sampleAccount2.CurrentBalanceWithoutExchange);
        }

        [TestMethod]
        public async void AddTransactionAmountSaveToDb() {
            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                db.Insert(_sampleAccount);
            }

            var transaction = new FinancialTransaction {
                ChargedAccount = _sampleAccount,
                Type = (int) TransactionType.Income,
                Currency = "CHF",
                Amount = 100
            };

            await AccountLogic.AddTransactionAmount(transaction);

            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                Account loadedAccount = db.Table<Account>().First(x => x.Id == _sampleAccount.Id);

                Assert.AreEqual(800, loadedAccount.CurrentBalance);
                Assert.AreEqual(800, loadedAccount.CurrentBalanceWithoutExchange);
            }
        }

        [TestMethod]
        public async void RemoveTransactionAmountSaveToDb() {
            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                db.Insert(_sampleAccount);
            }

            var transaction = new FinancialTransaction {
                ChargedAccount = _sampleAccount,
                Type = (int) TransactionType.Income,
                Currency = "CHF",
                Amount = 100
            };

            await AccountLogic.RemoveTransactionAmount(transaction);

            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                Account loadedAccount = db.Table<Account>().First(x => x.Id == _sampleAccount.Id);

                Assert.AreEqual(600, loadedAccount.CurrentBalance);
                Assert.AreEqual(600, loadedAccount.CurrentBalanceWithoutExchange);
            }
        }
    }
}