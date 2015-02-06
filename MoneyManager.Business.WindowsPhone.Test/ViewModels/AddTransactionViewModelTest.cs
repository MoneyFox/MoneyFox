using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;

namespace MoneyManager.Business.WindowsPhone.Test.ViewModels
{
    [TestClass]
    public class AddTransactionViewModelTest
    {
        public FinancialTransaction SelectedTransaction
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction; }
            set { ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction = value; }
        }

        [TestInitialize]
        public void Init()
        {
            new ViewModelLocator();
        }

        [TestMethod]
        public void ReturnEditSpendingTitle_Test()
        {
            var viewModel = new AddTransactionViewModel { IsEdit = true, IsTransfer = true };
            SelectedTransaction.Type = (int)TransactionType.Spending;

            Assert.AreEqual("edit spending", viewModel.Title);
        }

        [TestMethod]
        public void ReturnEditIncomeTitle_Test()
        {
            var viewModel = new AddTransactionViewModel { IsEdit = true, IsTransfer = true };
            SelectedTransaction.Type = (int)TransactionType.Income;

            Assert.AreEqual("edit income", viewModel.Title);
        }


        [TestMethod]
        public void ReturnEditTransferTitle_Test()
        {
            var viewModel = new AddTransactionViewModel { IsEdit = true, IsTransfer = true};
            SelectedTransaction.Type = (int)TransactionType.Transfer;

            Assert.AreEqual("edit transfer", viewModel.Title);
        }

        [TestMethod]
        public void ReturnAddTransferTitle_Test()
        {
            var viewModel = new AddTransactionViewModel { IsEdit = false, IsTransfer = true };

            Assert.AreEqual("add transfer", viewModel.Title);
        }

        [TestMethod]
        public void ReturnSpendingDefault_Title()
        {
            var viewModel = new AddTransactionViewModel();

            SelectedTransaction.Type = (int) TransactionType.Spending;

            Assert.AreEqual("add spending", viewModel.Title);
        }
        [TestMethod]
        public void ReturnIncomeDefault_Title()
        {
            var viewModel = new AddTransactionViewModel();

            SelectedTransaction.Type = (int) TransactionType.Income;

            Assert.AreEqual("add income", viewModel.Title);
        }
        
        [TestMethod]
        public void ReturnTransferDefault_Title()
        {
            var viewModel = new AddTransactionViewModel();

            SelectedTransaction.Type = (int)TransactionType.Transfer;

            Assert.AreEqual("add transfer", viewModel.Title);
        }

    }
}
