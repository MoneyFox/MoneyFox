using System.Linq;
using Windows.Globalization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Manager;
using MoneyManager.Business.Repositories;
using MoneyManager.Business.ViewModels;
using MoneyManager.Business.WindowsPhone.Test.Stubs;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;

namespace MoneyManager.Business.WindowsPhone.Test.ViewModels
{
    [TestClass]
    public class MainViewModelTests
    {
        //TODO:Refactor to check navigation with mock
        [TestMethod]
        public void GoToAddTransaction_Income_CorrectPreparation()
        {
            var accountRepository = new AccountRepository(new AccountDataAccess());
            var settings = new SettingDataAccess();
            var addTransactionViewModel =
                new AddTransactionViewModel(new TransactionRepository(new TransactionDataAccess()),
                    accountRepository,
                    settings,
                    new NavigationServiceStub());

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);

            var mainViewModel = new MainViewModel(transactionManager, new NavigationServiceStub());

            mainViewModel.GoToAddTransactionCommand.Execute("Income");

            Assert.IsFalse(addTransactionViewModel.IsEdit);
            Assert.IsTrue(addTransactionViewModel.IsEndless);
            Assert.IsFalse(addTransactionViewModel.IsTransfer);
            Assert.AreEqual((int)TransactionType.Income, addTransactionViewModel.SelectedTransaction.Type);
            Assert.IsFalse(addTransactionViewModel.SelectedTransaction.IsExchangeModeActive);
            Assert.AreEqual(new GeographicRegion().CurrenciesInUse.First(), addTransactionViewModel.SelectedTransaction.Currency);
        }

        //TODO:Refactor to check navigation with mock
        [TestMethod]
        public void GoToAddTransaction_Spending_CorrectPreparation()
        {
            var accountRepository = new AccountRepository(new AccountDataAccess());
            var settings = new SettingDataAccess();
            var addTransactionViewModel =
                new AddTransactionViewModel(new TransactionRepository(new TransactionDataAccess()),
                    accountRepository,
                    settings,
                    new NavigationServiceStub());

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);

            var mainViewModel = new MainViewModel(transactionManager, new NavigationServiceStub());

            mainViewModel.GoToAddTransactionCommand.Execute("Spending");

            Assert.IsFalse(addTransactionViewModel.IsEdit);
            Assert.IsTrue(addTransactionViewModel.IsEndless);
            Assert.IsFalse(addTransactionViewModel.IsTransfer);
            Assert.AreEqual((int)TransactionType.Spending, addTransactionViewModel.SelectedTransaction.Type);
            Assert.IsFalse(addTransactionViewModel.SelectedTransaction.IsExchangeModeActive);
            Assert.AreEqual(new GeographicRegion().CurrenciesInUse.First(), addTransactionViewModel.SelectedTransaction.Currency);
        }

        //TODO:Refactor to check navigation with mock
        [TestMethod]
        public void GoToAddTransaction_Transfer_CorrectPreparation()
        {
            var accountRepository = new AccountRepository(new AccountDataAccess());
            var settings = new SettingDataAccess();
            var addTransactionViewModel =
                new AddTransactionViewModel(new TransactionRepository(new TransactionDataAccess()),
                    accountRepository,
                    settings,
                    new NavigationServiceStub());

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);

            var mainViewModel = new MainViewModel(transactionManager, new NavigationServiceStub());

            mainViewModel.GoToAddTransactionCommand.Execute("Transfer");

            Assert.IsFalse(addTransactionViewModel.IsEdit);
            Assert.IsTrue(addTransactionViewModel.IsEndless);
            Assert.IsTrue(addTransactionViewModel.IsTransfer);
            Assert.AreEqual((int)TransactionType.Transfer, addTransactionViewModel.SelectedTransaction.Type);
            Assert.IsFalse(addTransactionViewModel.SelectedTransaction.IsExchangeModeActive);
            Assert.AreEqual(new GeographicRegion().CurrenciesInUse.First(), addTransactionViewModel.SelectedTransaction.Currency);
        }
    }
}
