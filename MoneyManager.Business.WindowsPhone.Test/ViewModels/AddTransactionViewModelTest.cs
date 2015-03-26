using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
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
        public void ReturnEditSpendingTitle_Test() {
            _transactionRepository.Selected = new FinancialTransaction { Type = (int)TransactionType.Spending };
            var viewModel = new AddTransactionViewModel(_transactionRepository) { IsEdit = true, IsTransfer = true };

            Assert.AreEqual("edit spending", viewModel.Title);
        }

        [TestMethod]
        public void ReturnEditIncomeTitle_Test() {
            _transactionRepository.Selected = new FinancialTransaction { Type = (int)TransactionType.Income };
            var viewModel = new AddTransactionViewModel(_transactionRepository) { IsEdit = true, IsTransfer = true };

            Assert.AreEqual("edit income", viewModel.Title);
        }


        [TestMethod]
        public void ReturnEditTransferTitle_Test() {
            _transactionRepository.Selected = new FinancialTransaction { Type = (int)TransactionType.Transfer };
            var viewModel = new AddTransactionViewModel(_transactionRepository) { IsEdit = true, IsTransfer = true };

            Assert.AreEqual("edit transfer", viewModel.Title);
        }

        [TestMethod]
        public void ReturnAddTransferTitle_Test() {
            _transactionRepository.Selected = new FinancialTransaction { Type = (int)TransactionType.Transfer };
            var viewModel = new AddTransactionViewModel(_transactionRepository) { IsEdit = false };

            Assert.AreEqual("add transfer", viewModel.Title);
        }

        [TestMethod]
        public void ReturnSpendingDefault_Title() {
            _transactionRepository.Selected = new FinancialTransaction { Type = (int)TransactionType.Spending };

            var viewModel = new AddTransactionViewModel(_transactionRepository);

            Assert.AreEqual("add spending", viewModel.Title);
        }

        [TestMethod]
        public void ReturnIncomeDefault_Title() {
            _transactionRepository.Selected = new FinancialTransaction { Type = (int)TransactionType.Income };
            var viewModel = new AddTransactionViewModel(_transactionRepository);

            Assert.AreEqual("add income", viewModel.Title);
        }

        [TestMethod]
        [Ignore]
        public void ReturnTransferDefault_Title() {
            _transactionRepository.Selected = new FinancialTransaction { Type = (int)TransactionType.Transfer };
            var viewModel = new AddTransactionViewModel(_transactionRepository);

            Assert.AreEqual("add transfer", viewModel.Title);
        }
    }
}