using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Manager;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Tests.Stubs;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;

namespace MoneyManager.Core.Tests.ViewModels
{
    [TestClass]
    public class MainViewModelTests
    {
        //TODO:Refactor to check navigation with mock
        [TestMethod]
        public void GoToAddTransaction_Income_CorrectPreparation()
        {
            var dbHelper = new DbHelperStub();
            var accountRepository = new AccountRepository(new AccountDataAccess(dbHelper));
            var settings = new SettingDataAccess();
            var addTransactionViewModel =
                new AddTransactionViewModel(new TransactionRepository(new TransactionDataAccess(dbHelper)),
                    accountRepository,
                    settings,
                    new DialogServiceStub());

            var addAccountViewModel = new AddAccountViewModel(accountRepository);

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);
            var accountManager = new AccountManager(accountRepository, addAccountViewModel, settings);

            var mainViewModel = new MainViewModel(transactionManager, accountManager);
            mainViewModel.GoToAddTransactionCommand.Execute("Income");

            Assert.IsFalse(addTransactionViewModel.IsEdit);
            Assert.IsTrue(addTransactionViewModel.IsEndless);
            Assert.IsFalse(addTransactionViewModel.IsTransfer);
            Assert.AreEqual((int) TransactionType.Income, addTransactionViewModel.SelectedTransaction.Type);
            Assert.IsFalse(addTransactionViewModel.SelectedTransaction.IsExchangeModeActive);
        }

        //TODO:Refactor to check navigation with mock
        [TestMethod]
        public void GoToAddTransaction_Spending_CorrectPreparation()
        {
            var dbHelper = new DbHelperStub();
            var accountRepository = new AccountRepository(new AccountDataAccess(dbHelper));
            var settings = new SettingDataAccess();
            var addTransactionViewModel =
                new AddTransactionViewModel(new TransactionRepository(new TransactionDataAccess(dbHelper)),
                    accountRepository,
                    settings,
                    new DialogServiceStub());

            var addAccountViewModel = new AddAccountViewModel(accountRepository);

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);
            var accountManager = new AccountManager(accountRepository, addAccountViewModel, settings);

            var mainViewModel = new MainViewModel(transactionManager, accountManager);

            mainViewModel.GoToAddTransactionCommand.Execute("Spending");

            Assert.IsFalse(addTransactionViewModel.IsEdit);
            Assert.IsTrue(addTransactionViewModel.IsEndless);
            Assert.IsFalse(addTransactionViewModel.IsTransfer);
            Assert.AreEqual((int) TransactionType.Spending, addTransactionViewModel.SelectedTransaction.Type);
            Assert.IsFalse(addTransactionViewModel.SelectedTransaction.IsExchangeModeActive);
        }

        //TODO:Refactor to check navigation with mock
        [TestMethod]
        public void GoToAddTransaction_Transfer_CorrectPreparation()
        {
            var dbHelper = new DbHelperStub();
            var accountRepository = new AccountRepository(new AccountDataAccess(dbHelper));
            var settings = new SettingDataAccess();
            var addTransactionViewModel =
                new AddTransactionViewModel(new TransactionRepository(new TransactionDataAccess(dbHelper)),
                    accountRepository,
                    settings,
                    new DialogServiceStub());

            var addAccountViewModel = new AddAccountViewModel(accountRepository);

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);
            var accountManager = new AccountManager(accountRepository, addAccountViewModel, settings);

            var mainViewModel = new MainViewModel(transactionManager, accountManager);
            mainViewModel.GoToAddTransactionCommand.Execute("Transfer");

            Assert.IsFalse(addTransactionViewModel.IsEdit);
            Assert.IsTrue(addTransactionViewModel.IsEndless);
            Assert.IsTrue(addTransactionViewModel.IsTransfer);
            Assert.AreEqual((int) TransactionType.Transfer, addTransactionViewModel.SelectedTransaction.Type);
            Assert.IsFalse(addTransactionViewModel.SelectedTransaction.IsExchangeModeActive);
        }
    }
}