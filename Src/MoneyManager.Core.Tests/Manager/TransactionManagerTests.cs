using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Manager;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Tests.Stubs;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;

namespace MoneyManager.Core.Tests.Manager
{
    [TestClass]
    public class TransactionManagerTests
    {
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

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);

            transactionManager.PrepareCreation("Income");

            Assert.IsFalse(addTransactionViewModel.IsEdit);
            Assert.IsTrue(addTransactionViewModel.IsEndless);
            Assert.IsFalse(addTransactionViewModel.IsTransfer);
            Assert.AreEqual((int) TransactionType.Income, addTransactionViewModel.SelectedTransaction.Type);
            Assert.IsFalse(addTransactionViewModel.SelectedTransaction.IsExchangeModeActive);
        }

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

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);

            transactionManager.PrepareCreation("Spending");

            Assert.IsFalse(addTransactionViewModel.IsEdit);
            Assert.IsTrue(addTransactionViewModel.IsEndless);
            Assert.IsFalse(addTransactionViewModel.IsTransfer);
            Assert.AreEqual((int) TransactionType.Spending, addTransactionViewModel.SelectedTransaction.Type);
            Assert.IsFalse(addTransactionViewModel.SelectedTransaction.IsExchangeModeActive);
        }

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

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);

            transactionManager.PrepareCreation("Transfer");

            Assert.IsFalse(addTransactionViewModel.IsEdit);
            Assert.IsTrue(addTransactionViewModel.IsEndless);
            Assert.IsTrue(addTransactionViewModel.IsTransfer);
            Assert.AreEqual((int) TransactionType.Transfer, addTransactionViewModel.SelectedTransaction.Type);
            Assert.IsFalse(addTransactionViewModel.SelectedTransaction.IsExchangeModeActive);
        }
    }
}