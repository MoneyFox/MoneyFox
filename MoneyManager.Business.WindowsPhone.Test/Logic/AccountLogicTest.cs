#region

using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Logic;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;

#endregion

namespace MoneyManager.Business.WindowsPhone.Test.Logic
{
    [TestClass]
    public class AccountLogicTest
    {
        private Account _sampleAccount = new Account
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

        private static AccountDataAccess accountData
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }


        [TestInitialize]
        public void TestInit()
        {
            new ViewModelLocator();
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
        [Ignore]
        public void RemoveTransactionAmountTest()
        {
        }

        [TestMethod]
        [Ignore]
        public void AddTransactionAmount()
        {
        }
    }
}