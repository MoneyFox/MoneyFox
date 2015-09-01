using System.Collections.ObjectModel;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Manager;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.Manager
{
    public class TransactionManagerTests
    {
        [Theory]
        [InlineData("Spending", 0)]
        [InlineData("Income", 1)]
        [InlineData("Transfer", 2)]
        public void GoToAddTransaction_TransactionTypeString_CorrectPreparation(string transactionTypeString, int transactionTypeInt)
        {
            var accountSetup = new Mock<IRepository<Account>>();
            accountSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var selectedTransaction = new FinancialTransaction();
            var transactionSetup = new Mock<ITransactionRepository>();
            transactionSetup.SetupSet(x => x.Selected = It.IsAny<FinancialTransaction>())
                .Callback<FinancialTransaction>(x => selectedTransaction = x);

            transactionSetup.SetupGet(x => x.Selected).Returns(selectedTransaction);

            var accountRepository = accountSetup.Object;
            var settings = new SettingDataAccess();
            var addTransactionViewModel =
                new ModifyTransactionViewModel(transactionSetup.Object,
                    accountRepository,
                    new Mock<IDialogService>().Object);

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);

            transactionManager.PrepareCreation(transactionTypeString);

            addTransactionViewModel.IsEdit.ShouldBeFalse();
            addTransactionViewModel.IsEndless.ShouldBeTrue();
            if (transactionTypeString == "Transfer")
            {
                addTransactionViewModel.IsTransfer.ShouldBeTrue();
            }
            else
            {
                addTransactionViewModel.IsTransfer.ShouldBeFalse();
            }
            selectedTransaction.Type.ShouldBe(transactionTypeInt);
        }
    }
}