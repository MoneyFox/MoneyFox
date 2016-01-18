using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Test.Core;
using MoneyManager.Core.Manager;
using MoneyManager.Core.ViewModels;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.TestFoundation;
using Moq;
using MvvmCross.Plugins.Messenger;
using Xunit;

namespace MoneyManager.Core.Tests.ViewModels
{
    public class ModifyPaymentViewModelTest : MvxIoCSupportingTest
    {
        public ModifyPaymentViewModelTest()
        {
            MvxSingleton.ClearAllSingletons();
            Setup();

            Mvx.RegisterSingleton(() => new Mock<IMvxMessenger>().Object);
        }

        [Fact]
        public void Init_SpendingNotEditing_PropertiesSetupCorrectly()
        {
            //Setup
            var transactionRepositorySetup = new Mock<IPaymentRepository>();
            transactionRepositorySetup.SetupProperty(x => x.Selected);

            var transactionManager = new PaymentManager(transactionRepositorySetup.Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object);

            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepoMock.SetupAllProperties();

            var accountRepo = accountRepoMock.Object;
            accountRepo.Data = new ObservableCollection<Account>();

            var defaultManager = new DefaultManager(accountRepo,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object));

            var viewmodel = new ModifyPaymentViewModel(transactionRepositorySetup.Object,
                accountRepo,
                new Mock<IDialogService>().Object,
                transactionManager,
                defaultManager);

            //Execute and Assert
            viewmodel.SelectedPayment.ShouldBeNull();

            viewmodel.Init("Income", true);
            viewmodel.SelectedPayment.Type.ShouldBe((int) PaymentType.Spending);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeFalse();
        }

        [Fact]
        public void Init_IncomeEditing_PropertiesSetupCorrectly()
        {
            //Setup
            var testEndDate = new DateTime(2099, 1, 31);

            var transactionRepositorySetup = new Mock<IPaymentRepository>();
            transactionRepositorySetup.SetupGet(x => x.Selected).Returns(new Payment
            {
                Type = (int) PaymentType.Income,
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

            var transactionManager = new PaymentManager(transactionRepositorySetup.Object,
                accountRepo,
                new Mock<IDialogService>().Object);


            var defaultManager = new DefaultManager(accountRepo,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object));

            var viewmodel = new ModifyPaymentViewModel(transactionRepositorySetup.Object,
                accountRepo,
                new Mock<IDialogService>().Object,
                transactionManager,
                defaultManager);

            //Execute and Assert
            viewmodel.SelectedPayment.ShouldNotBeNull();

            viewmodel.Init("Income", true);
            viewmodel.SelectedPayment.Type.ShouldBe((int) PaymentType.Income);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeTrue();
            viewmodel.SelectedPayment.RecurringPayment.EndDate.ShouldBe(testEndDate);
            viewmodel.SelectedPayment.RecurringPayment.IsEndless.ShouldBeFalse();
        }
    }
}