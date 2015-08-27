using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Tests.Stubs;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Tests.ViewModels
{
    [TestClass]
    public class AddTransactionViewModelTest
    {
        [TestMethod]
        public void AddTransactionViewModel_ReturnEditSpendingTitle()
        {
            var dbHelper = new DbHelperStub();
            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Spending}
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new SettingDataAccess(),
                new DialogServiceStub())
            {
                IsEdit = true,
                IsTransfer = true
            };

            Assert.AreEqual("edit spending", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnEditIncomeTitle()
        {
            var dbHelper = new DbHelperStub();

            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Income}
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new SettingDataAccess(),
                new DialogServiceStub())
            {
                IsEdit = true,
                IsTransfer = true
            };

            Assert.AreEqual("edit income", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnEditTransferTitle()
        {
            var dbHelper = new DbHelperStub();

            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Transfer}
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new SettingDataAccess(),
                new DialogServiceStub())
            {
                IsEdit = true,
                IsTransfer = true
            };

            Assert.AreEqual("edit transfer", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnAddTransferTitle()
        {
            var dbHelper = new DbHelperStub();

            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Transfer}
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new SettingDataAccess(),
                new DialogServiceStub())
            {IsEdit = false};

            Assert.AreEqual("add transfer", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnSpendingDefaultTitle()
        {
            var dbHelper = new DbHelperStub();

            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Spending}
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new SettingDataAccess(),
                new DialogServiceStub());

            Assert.AreEqual("add spending", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnIncomeDefault()
        {
            var dbHelper = new DbHelperStub();

            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Income}
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new SettingDataAccess(),
                new DialogServiceStub());
            Assert.AreEqual("add income", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnTransferDefault()
        {
            var dbHelper = new DbHelperStub();

            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Transfer}
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new SettingDataAccess(),
                new DialogServiceStub());

            Assert.AreEqual("add transfer", viewModel.Title);
        }
    }
}