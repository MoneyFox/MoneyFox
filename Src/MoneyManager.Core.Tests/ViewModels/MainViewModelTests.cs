using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Manager;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.ViewModels
{
    public class MainViewModelTests
    {
        //TODO:Refactor to check navigation with mock
        [Theory]
        [InlineData(TransactionType.Spending)]
        [InlineData(TransactionType.Income)]
        [InlineData(TransactionType.Transfer)]
        public void GoToAddTransaction_Type_CorrectPreparation(TransactionType type)
        {
            var dbHelper = new Mock<IDbHelper>().Object;
            var accountRepository = new AccountRepository(new AccountDataAccess(dbHelper));
            var settings = new SettingDataAccess();
            var addTransactionViewModel =
                new ModifyTransactionViewModel(new TransactionRepository(new TransactionDataAccess(dbHelper)),
                    accountRepository,
                    new Mock<IDialogService>().Object);

            var addAccountViewModel = new ModifyAccountViewModel(accountRepository,
                new BalanceViewModel(accountRepository, new Mock<ITransactionRepository>().Object, settings));

            var transactionManager = new TransactionManager(addTransactionViewModel, accountRepository, settings);
            var accountManager = new AccountManager(accountRepository, addAccountViewModel, settings);

            var mainViewModel = new MainViewModel(transactionManager, accountManager);
            mainViewModel.GoToAddTransactionCommand.Execute(type.ToString());

            addTransactionViewModel.IsEdit.ShouldBeFalse();
            addTransactionViewModel.IsEndless.ShouldBeTrue();

            if (type == TransactionType.Transfer)
            {
                addTransactionViewModel.IsTransfer.ShouldBeTrue();
            }
            else
            {
                addTransactionViewModel.IsTransfer.ShouldBeFalse();
            }
        }
    }
}