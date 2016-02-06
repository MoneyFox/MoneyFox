using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using MoneyManager.Core.Manager;
using MoneyManager.Core.ViewModels;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Test.Core;
using Xunit;
using XunitShouldExtension;

namespace MoneyManager.Core.Tests.ViewModels
{
    public class ModifyPaymentViewModelTest : MvxIoCSupportingTest
    {
        [Fact]
        public void Init_SpendingNotEditing_PropertiesSetupCorrectly()
        {
            ClearAll();
            Setup();
            Mvx.RegisterSingleton(() => new Mock<IMvxMessenger>().Object);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupGet(x => x.Selected).Returns(new Payment {ChargedAccountId = 3});
            
            var paymentManager = new PaymentManager(paymentRepoSetup.Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object);

            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepoMock.SetupAllProperties();

            var accountRepo = accountRepoMock.Object;
            accountRepo.Data = new ObservableCollection<Account> {new Account {Id = 3}};

            var defaultManager = new DefaultManager(accountRepo,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object));

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepo,
                new Mock<IDialogService>().Object,
                paymentManager,
                defaultManager);


            //Execute and Assert
            viewmodel.Init("Income", true);
            viewmodel.SelectedPayment.Type.ShouldBe((int) PaymentType.Expense);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeFalse();
        }

        [Fact]
        public void Init_IncomeEditing_PropertiesSetupCorrectly()
        {
            ClearAll();
            Setup();
            Mvx.RegisterSingleton(() => new Mock<IMvxMessenger>().Object);

            var testEndDate = new DateTime(2099, 1, 31);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupGet(x => x.Selected).Returns(new Payment
            {
                Type = (int)PaymentType.Income,
                IsRecurring = true,
                RecurringPayment = new RecurringPayment
                {
                    EndDate = testEndDate
                }
            });

            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepoMock.SetupAllProperties();

            var accountRepo = accountRepoMock.Object;
            accountRepo.Data = new ObservableCollection<Account>();

            var paymentManager = new PaymentManager(paymentRepoSetup.Object,
                accountRepo,
                new Mock<IDialogService>().Object);


            var defaultManager = new DefaultManager(accountRepo,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object));

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepo,
                new Mock<IDialogService>().Object,
                paymentManager,
                defaultManager);

            //Execute and Assert
            viewmodel.SelectedPayment.ShouldNotBeNull();

            viewmodel.Init("Income", true);
            viewmodel.SelectedPayment.Type.ShouldBe((int)PaymentType.Income);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeTrue();
            viewmodel.SelectedPayment.RecurringPayment.EndDate.ShouldBe(testEndDate);
            viewmodel.SelectedPayment.RecurringPayment.IsEndless.ShouldBeFalse();
        }
    }
}