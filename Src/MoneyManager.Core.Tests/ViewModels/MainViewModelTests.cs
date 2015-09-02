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
            var modifyTransactionViewModel =
                new ModifyTransactionViewModel(new TransactionRepository(new TransactionDataAccess(dbHelper)),
                    accountRepository,
                    new Mock<IDialogService>().Object,
                    transactionManager);

            var modifyAccountViewModel = new ModifyAccountViewModel(accountRepository,
                new BalanceViewModel(accountRepository, new Mock<ITransactionRepository>().Object, settings));
            var mainViewModel = new MainViewModel(modifyAccountViewModel, modifyTransactionViewModel);

            mainViewModel.GoToAddTransactionCommand.Execute(typestring);

            Assert.False(modifyTransactionViewModel.IsEdit);
            Assert.True(modifyTransactionViewModel.IsEndless);
            if (type == TransactionType.Transfer)
            {
                Assert.True(modifyTransactionViewModel.IsTransfer);
            }
            else
            {
                Assert.False(modifyTransactionViewModel.IsTransfer);
            }
            Assert.Equal((int)type, modifyTransactionViewModel.SelectedTransaction.Type);
        }
    }
}