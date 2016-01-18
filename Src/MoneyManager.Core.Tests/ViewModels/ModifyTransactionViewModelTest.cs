using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Cirrious.CrossCore;
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
    public class ModifyTransactionViewModelTest : MvxIoCSupportingTest
    {
        [Fact]
        public void Init_SpendingNotEditing_PropertiesSetupCorrectly()
        {
            ClearAll();
            Setup();
            Mvx.RegisterSingleton(() => new Mock<IMvxMessenger>().Object);

            var transactionRepositorySetup = new Mock<ITransactionRepository>();
            transactionRepositorySetup.SetupGet(x => x.Selected).Returns(new Payment {ChargedAccountId = 3});
            
            var transactionManager = new TransactionManager(transactionRepositorySetup.Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object);

            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepoMock.SetupAllProperties();

            var accountRepo = accountRepoMock.Object;
            accountRepo.Data = new ObservableCollection<Account> {new Account {Id = 3}};

            var defaultManager = new DefaultManager(accountRepo,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object));

            var viewmodel = new ModifyTransactionViewModel(transactionRepositorySetup.Object,
                accountRepo,
                new Mock<IDialogService>().Object,
                transactionManager,
                defaultManager);


            //Execute and Assert
            viewmodel.Init("Income", true);
            viewmodel.SelectedTransaction.Type.ShouldBe((int) TransactionType.Spending);
            viewmodel.SelectedTransaction.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedTransaction.IsRecurring.ShouldBeFalse();
        }

        [Fact]
        public void Init_IncomeEditing_PropertiesSetupCorrectly()
        {
            ClearAll();
            Setup();
            Mvx.RegisterSingleton(() => new Mock<IMvxMessenger>().Object);

            var testEndDate = new DateTime(2099, 1, 31);

            var transactionRepositorySetup = new Mock<ITransactionRepository>();
            transactionRepositorySetup.SetupGet(x => x.Selected).Returns(new Payment
            {
                Type = (int) TransactionType.Income,
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

            var transactionManager = new TransactionManager(transactionRepositorySetup.Object,
                accountRepo,
                new Mock<IDialogService>().Object);


            var defaultManager = new DefaultManager(accountRepo,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object));

            var viewmodel = new ModifyTransactionViewModel(transactionRepositorySetup.Object,
                accountRepo,
                new Mock<IDialogService>().Object,
                transactionManager,
                defaultManager);

            //Execute and Assert
            viewmodel.SelectedTransaction.ShouldNotBeNull();

            viewmodel.Init("Income", true);
            viewmodel.SelectedTransaction.Type.ShouldBe((int) TransactionType.Income);
            viewmodel.SelectedTransaction.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedTransaction.IsRecurring.ShouldBeTrue();
            viewmodel.SelectedTransaction.RecurringPayment.EndDate.ShouldBe(testEndDate);
            viewmodel.SelectedTransaction.RecurringPayment.IsEndless.ShouldBeFalse();
        }
    }
}