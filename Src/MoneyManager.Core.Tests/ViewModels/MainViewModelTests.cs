using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Test.Core;
using MoneyManager.Core.Manager;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.ViewModels;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.ViewModels
{
    internal class MainViewModelTests : MvxIoCSupportingTest
    {
        [Theory]
        [InlineData("Income", TransactionType.Income)]
        [InlineData("Spending", TransactionType.Spending)]
        [InlineData("Transfer", TransactionType.Transfer)]
        public void GoToAddTransaction_Transactiontype_CorrectPreparation(string typestring, TransactionType type)
        {
            Setup();
            // for navigation parsing
            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var dbHelper = new Mock<ISqliteConnectionCreator>().Object;
            var accountRepository = new AccountRepository(new AccountDataAccess(dbHelper));
            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper),
                new RecurringTransactionDataAccess(dbHelper));
            var transactionManager = new TransactionManager(transactionRepository, accountRepository,
                new Mock<IDialogService>().Object);

            var defaultManager = new DefaultManager(accountRepository,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object));

            var modifyTransactionViewModel =
                new ModifyTransactionViewModel(transactionRepository,
                    accountRepository,
                    new Mock<IDialogService>().Object,
                    transactionManager,
                    defaultManager);

            var modifyAccountViewModel = new ModifyAccountViewModel(accountRepository,
                new BalanceViewModel(accountRepository, new Mock<ITransactionRepository>().Object));

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
            Assert.Equal((int) type, modifyTransactionViewModel.SelectedTransaction.Type);
        }
    }
}