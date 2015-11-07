using Cirrious.MvvmCross.Test.Core;
using MoneyManager.Core.Manager;
using MoneyManager.Core.ViewModels;
using MoneyManager.DataAccess;
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
            var accountRepoMock = new Mock<IRepository<Account>>().Object;
            var dialogMock = new Mock<IDialogService>().Object;

            var transaction = new FinancialTransaction
            {
                Category = new Category()
            };

            transactionSetup.SetupGet(x => x.Selected).Returns(transaction);
            var transactionRepository = transactionSetup.Object;

            new SelectCategoryTextBoxViewModel(new ModifyTransactionViewModel(transactionRepository, accountRepoMock,
                dialogMock, new TransactionManager(transactionRepository, accountRepoMock, dialogMock),
                new DefaultManager(accountRepoMock, new SettingDataAccess(new Mock<IRoamingSettings>().Object))))
                .ResetCategoryCommand.Execute();

            Assert.Null(transaction.Category);
        }
    }
}