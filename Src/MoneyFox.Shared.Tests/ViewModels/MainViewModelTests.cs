using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Core.Platform;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.ViewModels
{
    public class MainViewModelTests : MvxIoCSupportingTest
    {
        public void GoToAddPayment_Income_CorrectPreparation()
        {
            Setup();
            // for navigation parsing
            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var dbHelper = new Mock<IDatabaseManager>().Object;
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

            mainViewModel.GoToAddPaymentCommand.Execute(PaymentType.Income.ToString());

            modifyPaymentViewModel.IsEdit.ShouldBeFalse();
            modifyPaymentViewModel.IsEndless.ShouldBeTrue();
            modifyPaymentViewModel.IsTransfer.ShouldBeFalse();
            modifyPaymentViewModel.SelectedPayment.Type.ShouldBe((int)PaymentType.Income);
        }

        public void GoToAddPayment_Expense_CorrectPreparation()
        {
            Setup();
            // for navigation parsing
            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var dbHelper = new Mock<IDatabaseManager>().Object;
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

            mainViewModel.GoToAddPaymentCommand.Execute(PaymentType.Expense.ToString());

            modifyPaymentViewModel.IsEdit.ShouldBeFalse();
            modifyPaymentViewModel.IsEndless.ShouldBeTrue();
            modifyPaymentViewModel.IsTransfer.ShouldBeFalse();
            modifyPaymentViewModel.SelectedPayment.Type.ShouldBe((int)PaymentType.Expense);
        }

        public void GoToAddPayment_Transfer_CorrectPreparation()
        {
            Setup();
            // for navigation parsing
            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var dbHelper = new Mock<IDatabaseManager>().Object;
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

            mainViewModel.GoToAddPaymentCommand.Execute(PaymentType.Income.ToString());

            modifyPaymentViewModel.IsEdit.ShouldBeFalse();
            modifyPaymentViewModel.IsEndless.ShouldBeTrue();
            modifyPaymentViewModel.IsTransfer.ShouldBeTrue();
            modifyPaymentViewModel.SelectedPayment.Type.ShouldBe((int)PaymentType.Transfer);
        }
    }
}