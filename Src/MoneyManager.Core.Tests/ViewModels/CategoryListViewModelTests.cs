using Cirrious.MvvmCross.Test.Core;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.ViewModels
{
    public class CategoryListViewModelTests : MvxIoCSupportingTest
    {
        public CategoryListViewModelTests()
        {
            Setup();
        }

        [Fact]
        public void ResetCategoryCommand_FilledProperty_PropertyIsNull()
        {
            var transactionSetup = new Mock<ITransactionRepository>();

            var transaction = new FinancialTransaction
            {
                Category = new Category()
            };

            transactionSetup.SetupGet(x => x.Selected).Returns(transaction);
            var transactionRepository = transactionSetup.Object;

            new SelectCategoryTextBoxViewModel(transactionRepository).ResetCategoryCommand.Execute();

            Assert.Null(transaction.Category);
        }
    }
}