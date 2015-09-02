using Cirrious.MvvmCross.Test.Core;
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
    public class MainViewModelTests : MvxIoCSupportingTest
    {
        public MainViewModelTests()
        {
            Setup();
        }

    public class MainViewModelTests
    {
        //TODO:Refactor to check navigation with mock
        [Theory]
        [InlineData("Income", TransactionType.Income)]
        [InlineData("Spending", TransactionType.Spending)]
        [InlineData("Transfer", TransactionType.Transfer)]
        public void GoToAddTransaction_Income_CorrectPreparation(string typestring, TransactionType type)
        {
            var dbHelper = new Mock<IDbHelper>().Object;
            var accountRepository = new AccountRepository(new AccountDataAccess(dbHelper));
            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper));
            var recurringTransactionRepository = new RecurringTransactionRepository(new RecurringTransactionDataAccess(dbHelper));
            var settings = new SettingDataAccess();
            var transactionManager = new TransactionManager(transactionRepository, accountRepository, recurringTransactionRepository, settings);
            var addTransactionViewModel =
                new ModifyTransactionViewModel(new TransactionRepository(new TransactionDataAccess(dbHelper)),
                    accountRepository,
                    new Mock<IDialogService>().Object,
                    transactionManager);

            var addAccountViewModel = new ModifyAccountViewModel(accountRepository,
                new BalanceViewModel(accountRepository, new Mock<ITransactionRepository>().Object, settings));
            var mainViewModel = new MainViewModel(transactionManager, addAccountViewModel);

            mainViewModel.GoToAddTransactionCommand.Execute(typestring);

            Assert.False(addTransactionViewModel.IsEdit);
            Assert.True(addTransactionViewModel.IsEndless);
            if (type == TransactionType.Transfer)
            {
                Assert.True(addTransactionViewModel.IsTransfer);
            }
            else
            {
                Assert.False(addTransactionViewModel.IsTransfer);
            }
            Assert.Equal((int)type, addTransactionViewModel.SelectedTransaction.Type);
        }
    }
}