using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Core.Platform;
using MvvmCross.Test.Core;
using Xunit;

namespace MoneyFox.Shared.Tests.ViewModels
{
    internal class MainViewModelTests : MvxIoCSupportingTest
    {
        [Theory]
        [InlineData("Income", PaymentType.Income)]
        [InlineData("Expense", PaymentType.Expense)]
        [InlineData("Transfer", PaymentType.Transfer)]
        public void GoToAddPayment_PaymentType_CorrectPreparation(string typestring, PaymentType type)
        {
            Setup();
            // for navigation parsing
            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var dbHelper = new Mock<ISqliteConnectionCreator>().Object;
            var accountRepository = new AccountRepository(new AccountDataAccess(dbHelper),
                new Mock<INotificationService>().Object);
            var paymentRepository = new PaymentRepository(new PaymentDataAccess(dbHelper),
                new RecurringPaymentDataAccess(dbHelper),
                accountRepository,
                new CategoryRepository(new CategoryDataAccess(dbHelper),
                new Mock<INotificationService>().Object),
                new Mock<INotificationService>().Object);
            var paymentManager = new PaymentManager(paymentRepository, accountRepository,
                new Mock<IDialogService>().Object);

            var defaultManager = new DefaultManager(accountRepository);

            var modifyPaymentViewModel =
                new ModifyPaymentViewModel(paymentRepository,
                    accountRepository,
                    new Mock<IDialogService>().Object,
                    paymentManager,
                    defaultManager);

            var mainViewModel = new MainViewModel();

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