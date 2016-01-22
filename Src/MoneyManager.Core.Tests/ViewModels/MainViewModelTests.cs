using MoneyManager.Core.Manager;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.ViewModels;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using Moq;
using MvvmCross.Core.Platform;
using MvvmCross.Test.Core;
using Xunit;

namespace MoneyManager.Core.Tests.ViewModels
{
    internal class MainViewModelTests : MvxIoCSupportingTest
    {
        [Theory]
        [InlineData("Income", PaymentType.Income)]
        [InlineData("Spending", PaymentType.Spending)]
        [InlineData("Transfer", PaymentType.Transfer)]
        public void GoToAddTransaction_Transactiontype_CorrectPreparation(string typestring, PaymentType type)
        {
            Setup();
            // for navigation parsing
            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var dbHelper = new Mock<ISqliteConnectionCreator>().Object;
            var accountRepository = new AccountRepository(new AccountDataAccess(dbHelper));
            var transactionRepository = new PaymentRepository(new TransactionDataAccess(dbHelper),
                new RecurringTransactionDataAccess(dbHelper));
            var transactionManager = new PaymentManager(transactionRepository, accountRepository,
                new Mock<IDialogService>().Object);

            var defaultManager = new DefaultManager(accountRepository,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object));

            var modifyTransactionViewModel =
                new ModifyPaymentViewModel(transactionRepository,
                    accountRepository,
                    new Mock<IDialogService>().Object,
                    transactionManager,
                    defaultManager);

            var modifyAccountViewModel = new ModifyAccountViewModel(accountRepository,
                new BalanceViewModel(accountRepository, new Mock<IPaymentRepository>().Object));

            var mainViewModel = new MainViewModel(modifyAccountViewModel, modifyTransactionViewModel,
                new BalanceViewModel(accountRepository, transactionRepository));

            mainViewModel.GoToAddTransactionCommand.Execute(typestring);

            Assert.False(modifyTransactionViewModel.IsEdit);
            Assert.True(modifyTransactionViewModel.IsEndless);
            if (type == PaymentType.Transfer)
            {
                Assert.True(modifyTransactionViewModel.IsTransfer);
            }
            else
            {
                Assert.False(modifyTransactionViewModel.IsTransfer);
            }
            Assert.Equal((int) type, modifyTransactionViewModel.SelectedPayment.Type);
        }
    }
}