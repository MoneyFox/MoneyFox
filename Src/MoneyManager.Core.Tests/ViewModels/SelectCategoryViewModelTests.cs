using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Tests.Stubs;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Tests.ViewModels
{
    [TestClass]
    public class SelectCategoryViewModelTests
    {
        [TestMethod]
        public void ResetCategoryCommand_FilledProperty_PropertyIsNull()
        {
            var transactionRepository = new TransactionRepository(new TransactionDataAccess(new DbHelperStub()));
            var viewModel = new SelectCategoryViewModel(transactionRepository);

            transactionRepository.Selected = new FinancialTransaction
            {
                Category = new Category()
            };

            Assert.IsNotNull(transactionRepository.Selected.Category);

            viewModel.ResetCategoryCommand.Execute(null);

            Assert.IsNull(transactionRepository.Selected.Category);
        }
    }
}