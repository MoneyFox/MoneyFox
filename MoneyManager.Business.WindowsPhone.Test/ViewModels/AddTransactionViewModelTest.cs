using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.ViewModels;

namespace MoneyManager.Business.WindowsPhone.Test.ViewModels
{
    [TestClass]
    public class AddTransactionViewModelTest
    {
        [TestMethod]
        public void ReturnEditTitle_Test()
        {
            var viewModel = new AddTransactionViewModel { IsEdit = true };

            Assert.AreEqual("edit", viewModel.Title);
        }

        [TestMethod]
        public void ReturnAddTitle_Test()
        {
            var viewModel = new AddTransactionViewModel { IsEdit = false };

            Assert.AreEqual("add", viewModel.Title);
        }

        [TestMethod]
        public void ReturnDefault_Title()
        {
            var viewModel = new AddTransactionViewModel();

            Assert.AreEqual("add", viewModel.Title);
        }

    }
}
