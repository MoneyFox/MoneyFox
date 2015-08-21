using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Repositories;
using MoneyManager.Business.ViewModels;
using MoneyManager.Business.WindowsPhone.Test.Stubs;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Business.WindowsPhone.Test.ViewModels
{
    [TestClass]
    public class AddTransactionViewModelTest
    {
        [TestMethod]
        public void AddTransactionViewModel_ReturnEditSpendingTitle()
        {
            var transactionRepository = new TransactionRepository(new TransactionDataAccess())
            {
                Selected = new FinancialTransaction {Type = (int) TransactionType.Spending}
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess()), 
                new SettingDataAccess(), 
                new NavigationServiceStub())
            {
                IsEdit = true,
                IsTransfer = true
            };

            Assert.AreEqual("edit spending", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnEditIncomeTitle()
        {
            var transactionRepository = new TransactionRepository(new TransactionDataAccess())
            {
                Selected = new FinancialTransaction { Type = (int)TransactionType.Income }
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess()),
                new SettingDataAccess(),
                new NavigationServiceStub())
            {
                IsEdit = true,
                IsTransfer = true
            };

            Assert.AreEqual("edit income", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnEditTransferTitle()
        {
            var transactionRepository = new TransactionRepository(new TransactionDataAccess())
            {
                Selected = new FinancialTransaction { Type = (int)TransactionType.Transfer }
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess()),
                new SettingDataAccess(),
                new NavigationServiceStub())
            {
                IsEdit = true,
                IsTransfer = true
            };

            Assert.AreEqual("edit transfer", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnAddTransferTitle()
        {
            var transactionRepository = new TransactionRepository(new TransactionDataAccess())
            {
                Selected = new FinancialTransaction { Type = (int)TransactionType.Transfer }
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess()),
                new SettingDataAccess(),
                new NavigationServiceStub())
            { IsEdit = false};

            Assert.AreEqual("add transfer", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnSpendingDefaultTitle()
        {
            var transactionRepository = new TransactionRepository(new TransactionDataAccess())
            {
                Selected = new FinancialTransaction { Type = (int)TransactionType.Spending }
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess()),
                new SettingDataAccess(),
                new NavigationServiceStub());
            

            Assert.AreEqual("add spending", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnIncomeDefault()
        {
            var transactionRepository = new TransactionRepository(new TransactionDataAccess())
            {
                Selected = new FinancialTransaction { Type = (int)TransactionType.Income }
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess()),
                new SettingDataAccess(),
                new NavigationServiceStub());
            Assert.AreEqual("add income", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnTransferDefault()
        {
            var transactionRepository = new TransactionRepository(new TransactionDataAccess())
            {
                Selected = new FinancialTransaction { Type = (int)TransactionType.Transfer }
            };

            var viewModel = new AddTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess()),
                new SettingDataAccess(),
                new NavigationServiceStub());

            Assert.AreEqual("add transfer", viewModel.Title);
        }
    }
}