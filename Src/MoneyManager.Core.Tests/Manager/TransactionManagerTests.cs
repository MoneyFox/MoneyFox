using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Manager;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;
using Moq;

namespace MoneyManager.Core.Tests.Manager
{
    [TestClass]
    public class TransactionManagerTests
    {
        [TestMethod]
        public void GoToAddTransaction_Income_CorrectPreparation()
        {
            var dbHelper = new Mock<IDbHelper>().Object;
            var accountRepository = new AccountRepository(new AccountDataAccess(dbHelper));
            var settings = new SettingDataAccess();
            var addTransactionViewModel =
                new ModifyTransactionViewModel(new TransactionRepository(new TransactionDataAccess(dbHelper)),
                    accountRepository,
                    new Mock<IDialogService>().Object);

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);

            transactionManager.PrepareCreation("Income");

            Assert.IsFalse(addTransactionViewModel.IsEdit);
            Assert.IsTrue(addTransactionViewModel.IsEndless);
            Assert.IsFalse(addTransactionViewModel.IsTransfer);
            Assert.AreEqual((int) TransactionType.Income, addTransactionViewModel.SelectedTransaction.Type);
        }

        [TestMethod]
        public void GoToAddTransaction_Spending_CorrectPreparation()
        {
            var dbHelper = new Mock<IDbHelper>().Object;
            var accountRepository = new AccountRepository(new AccountDataAccess(dbHelper));
            var settings = new SettingDataAccess();
            var addTransactionViewModel =
                new ModifyTransactionViewModel(new TransactionRepository(new TransactionDataAccess(dbHelper)),
                    accountRepository,
                    new Mock<IDialogService>().Object);

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);

            transactionManager.PrepareCreation("Spending");

            Assert.IsFalse(addTransactionViewModel.IsEdit);
            Assert.IsTrue(addTransactionViewModel.IsEndless);
            Assert.IsFalse(addTransactionViewModel.IsTransfer);
            Assert.AreEqual((int) TransactionType.Spending, addTransactionViewModel.SelectedTransaction.Type);
        }

        [TestMethod]
        public void GoToAddTransaction_Transfer_CorrectPreparation()
        {
            var dbHelper = new Mock<IDbHelper>().Object;
            var accountRepository = new AccountRepository(new AccountDataAccess(dbHelper));
            var settings = new SettingDataAccess();
            var addTransactionViewModel =
                new ModifyTransactionViewModel(new TransactionRepository(new TransactionDataAccess(dbHelper)),
                    accountRepository,
                    new Mock<IDialogService>().Object);

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);

            transactionManager.PrepareCreation("Transfer");

            Assert.IsFalse(addTransactionViewModel.IsEdit);
            Assert.IsTrue(addTransactionViewModel.IsEndless);
            Assert.IsTrue(addTransactionViewModel.IsTransfer);
            Assert.AreEqual((int) TransactionType.Transfer, addTransactionViewModel.SelectedTransaction.Type);
        }
    }
}