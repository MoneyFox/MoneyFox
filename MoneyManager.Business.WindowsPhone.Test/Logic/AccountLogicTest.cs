#region

using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Logic;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;

#endregion

namespace MoneyManager.Business.WindowsPhone.Test.Logic
{
    [TestClass]
    public class AccountLogicTest
    {
        private Account _sampleAccount;

        private static AccountDataAccess accountData
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }


        [TestInitialize]
        public void TestInit()
        {
            new ViewModelLocator();

            _sampleAccount = new Account
            {
                Currency = "CHF",
                IsExchangeModeActive = false,
                CurrentBalance = 700,
                CurrentBalanceWithoutExchange = 700,
                ExchangeRatio = 1,
                Iban = "this is a iban",
                Name = "Sparkonto",
                Note = "just a note"
            };
        }

        [TestMethod]
        public void GoToAddAccountTest()
        {
            AccountLogic.PrepareAddAccount();

            Assert.AreEqual(ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultCurrency,
                accountData.SelectedAccount.Currency);
            Assert.AreEqual(false, accountData.SelectedAccount.IsExchangeModeActive);

            Assert.AreEqual(false, ServiceLocator.Current.GetInstance<AddAccountViewModel>().IsEdit);
        }

        [TestMethod]
        public async void AddTransactionAmount()
        {
            using (var db = SqlConnectionFactory.GetSqlConnection())
            {
                db.Insert(_sampleAccount);
            }

            var transaction = new FinancialTransaction
            {
                ChargedAccount = _sampleAccount,
                Currency = "CHF",
                Amount = 100
            };

            await AccountLogic.AddTransactionAmount(transaction);

            Assert.AreEqual(600, _sampleAccount.CurrentBalance);
            Assert.AreEqual(600, _sampleAccount.CurrentBalanceWithoutExchange);
        }

        [TestMethod]
        public async void RemoveTransactionAmountTest()
        {
            using (var db = SqlConnectionFactory.GetSqlConnection())
            {
                db.Insert(_sampleAccount);
            }

            var transaction = new FinancialTransaction
            {
                ChargedAccount = _sampleAccount,
                Currency = "CHF",
                Amount = 100
            };

            await AccountLogic.RemoveTransactionAmount(transaction);

            Assert.AreEqual(700, _sampleAccount.CurrentBalance);
            Assert.AreEqual(700, _sampleAccount.CurrentBalanceWithoutExchange);

        }
    }
}