using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.ViewModels
{
    public class SelectCategoryViewModelTests
    {
        [Fact]
        public void ResetCategoryCommand_FilledProperty_PropertyIsNull()
        {
            var transactionRepository = new TransactionRepository(new TransactionDataAccess(new Mock<IDbHelper>().Object));
            var viewModel = new SelectCategoryViewModel(transactionRepository);

            transactionRepository.Selected = new FinancialTransaction
            {
                Category = new Category()
            };

            Assert.NotNull(transactionRepository.Selected.Category);

            viewModel.ResetCategoryCommand.Execute(null);

            Assert.Null(transactionRepository.Selected.Category);
        }
    }
}