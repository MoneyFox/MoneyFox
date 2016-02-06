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
        public void GoToAddPayment_PaymentType_CorrectPreparation(string typestring, PaymentType type)
        {
            Setup();
            // for navigation parsing
            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var dbHelper = new Mock<ISqliteConnectionCreator>().Object;
            var accountRepository = new AccountRepository(new AccountDataAccess(dbHelper));
            var paymentRepository = new PaymentRepository(new PaymentDataAccess(dbHelper),
                new RecurringPaymentDataAccess(dbHelper),
                accountRepository,
                new CategoryRepository(new CategoryDataAccess(dbHelper)));
            var paymentManager = new PaymentManager(paymentRepository, accountRepository,
                new Mock<IDialogService>().Object);

            var defaultManager = new DefaultManager(accountRepository,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object));

            var modifyPaymentViewModel =
                new ModifyPaymentViewModel(paymentRepository,
                    accountRepository,
                    new Mock<IDialogService>().Object,
                    paymentManager,
                    defaultManager);

            var modifyAccountViewModel = new ModifyAccountViewModel(accountRepository,
                new BalanceViewModel(accountRepository, new Mock<IPaymentRepository>().Object));

            var mainViewModel = new MainViewModel(modifyAccountViewModel, modifyPaymentViewModel,
                new BalanceViewModel(accountRepository, paymentRepository));

            mainViewModel.GoToAddPaymentCommand.Execute(typestring);

            Assert.False(modifyPaymentViewModel.IsEdit);
            Assert.True(modifyPaymentViewModel.IsEndless);
            if (type == PaymentType.Transfer)
            {
                Assert.True(modifyPaymentViewModel.IsTransfer);
            }
            else
            {
                Assert.False(modifyPaymentViewModel.IsTransfer);
            }
            Assert.Equal((int) type, modifyPaymentViewModel.SelectedPayment.Type);
        }
    }
}