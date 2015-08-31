using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using Moq;

namespace MoneyManager.Core.Tests.ViewModels
{
    [TestClass]
    public class AddTransactionViewModelTest
    {
        [TestMethod]
        public void AddTransactionViewModel_ReturnEditSpendingTitle()
        {
            var dbHelper = new Mock<IDbHelper>().Object;
            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Spending}
            };

            var viewModel = new ModifyTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new Mock<IDialogService>().Object)
            {
                IsEdit = true,
                IsTransfer = true
            };

            Assert.AreEqual("edit spending", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnEditIncomeTitle()
        {
            var dbHelper = new Mock<IDbHelper>().Object;

            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Income}
            };

            var viewModel = new ModifyTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new Mock<IDialogService>().Object)
            {
                IsEdit = true,
                IsTransfer = true
            };

            Assert.AreEqual("edit income", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnEditTransferTitle()
        {
            var dbHelper = new Mock<IDbHelper>().Object;

            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Transfer}
            };

            var viewModel = new ModifyTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new Mock<IDialogService>().Object)
            {
                IsEdit = true,
                IsTransfer = true
            };

            Assert.AreEqual("edit transfer", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnAddTransferTitle()
        {
            var dbHelper = new Mock<IDbHelper>().Object;

            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Transfer}
            };

            var viewModel = new ModifyTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new Mock<IDialogService>().Object)
            {IsEdit = false};

            Assert.AreEqual("add transfer", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnSpendingDefaultTitle()
        {
            var dbHelper = new Mock<IDbHelper>().Object;

            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Spending}
            };

            var viewModel = new ModifyTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new Mock<IDialogService>().Object);

            Assert.AreEqual("add spending", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnIncomeDefault()
        {
            var dbHelper = new Mock<IDbHelper>().Object;

            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Income}
            };

            var viewModel = new ModifyTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new Mock<IDialogService>().Object);
            Assert.AreEqual("add income", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnTransferDefault()
        {
            var dbHelper = new Mock<IDbHelper>().Object;

            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Transfer}
            };

            var viewModel = new ModifyTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new Mock<IDialogService>().Object);

            Assert.AreEqual("add transfer", viewModel.Title);
        }
    }
}