using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Manager;
using MoneyManager.Business.Services;
using MoneyManager.Business.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.WindowsPhone.Test.ViewModels {
    [TestClass]
    public class AddTransactionViewModelTest {
        private ITransactionRepository _transactionRepository;

        [TestInitialize]
        public void Init() {
            new ViewModelLocator();

            _transactionRepository = ServiceLocator.Current.GetInstance<ITransactionRepository>();
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnEditSpendingTitle() {
            _transactionRepository.Selected = new FinancialTransaction {Type = (int) TransactionType.Spending};
            var viewModel = new AddTransactionViewModel(_transactionRepository, new CurrencyManager(new JsonService())) {
                IsEdit = true,
                IsTransfer = true
            };

            Assert.AreEqual("edit spending", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnEditIncomeTitle() {
            _transactionRepository.Selected = new FinancialTransaction {Type = (int) TransactionType.Income};
            var viewModel = new AddTransactionViewModel(_transactionRepository, new CurrencyManager(new JsonService())) {
                IsEdit = true,
                IsTransfer = true
            };

            Assert.AreEqual("edit income", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnEditTransferTitle() {
            _transactionRepository.Selected = new FinancialTransaction {Type = (int) TransactionType.Transfer};
            var viewModel = new AddTransactionViewModel(_transactionRepository, new CurrencyManager(new JsonService())) {
                IsEdit = true,
                IsTransfer = true
            };

            Assert.AreEqual("edit transfer", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnAddTransferTitle() {
            _transactionRepository.Selected = new FinancialTransaction {Type = (int) TransactionType.Transfer};
            var viewModel = new AddTransactionViewModel(_transactionRepository, new CurrencyManager(new JsonService())) {IsEdit = false};

            Assert.AreEqual("add transfer", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnSpendingDefaultTitle() {
            _transactionRepository.Selected = new FinancialTransaction {Type = (int) TransactionType.Spending};

            var viewModel = new AddTransactionViewModel(_transactionRepository, new CurrencyManager(new JsonService()));

            Assert.AreEqual("add spending", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnIncomeDefault() {
            _transactionRepository.Selected = new FinancialTransaction {Type = (int) TransactionType.Income};
            var viewModel = new AddTransactionViewModel(_transactionRepository, new CurrencyManager(new JsonService()));

            Assert.AreEqual("add income", viewModel.Title);
        }

        [TestMethod]
        public void AddTransactionViewModel_ReturnTransferDefault() {
            _transactionRepository.Selected = new FinancialTransaction {Type = (int) TransactionType.Transfer};
            var viewModel = new AddTransactionViewModel(_transactionRepository, new CurrencyManager(new JsonService()));

            Assert.AreEqual("add transfer", viewModel.Title);
        }
    }
}