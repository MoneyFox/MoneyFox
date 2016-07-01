using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Test.Core;
using Cheesebaron.MvxPlugins.Settings.Interfaces;

namespace MoneyFox.Shared.Tests.ViewModels {
    [TestClass]
    public class ModifyPaymentViewModelTest : MvxIoCSupportingTest {
        [TestInitialize]
        public void Init() {
            ClearAll();
            Setup();

            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.SetupAllProperties();
            settingsMockSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<DateTime>(), false))
                .Callback((string key, DateTime date, bool roam) => localDateSetting = date);
            Mvx.RegisterType(() => settingsMockSetup.Object);
            Mvx.RegisterType(() => new Mock<IAutobackupManager>().Object);


        }

        private DateTime localDateSetting;

        [TestMethod]
        public void Init_SpendingNotEditing_PropertiesSetupCorrectly() {
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

            var defaultManager = new DefaultManager(accountRepo);

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


        [TestMethod]
        public void Save_UpdateTimeStamp()
        {
            Mvx.RegisterSingleton(() => new Mock<IMvxMessenger>().Object);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupGet(x => x.Selected).Returns(new Payment { ChargedAccountId = 3, ChargedAccount = new Account { Id = 3, Name = "3" }, });
            paymentRepoSetup.Setup(x => x.Save(paymentRepoSetup.Object.Selected)).Returns(true);

            var paymentManager = new PaymentManager(paymentRepoSetup.Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object);

            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepoMock.SetupAllProperties();
            accountRepoMock.Setup(x => x.AddPaymentAmount(paymentRepoSetup.Object.Selected)).Returns(true);

            var accountRepo = accountRepoMock.Object;
            accountRepo.Data = new ObservableCollection<Account> { new Account { Id = 3, Name = "3" } };

            var defaultManager = new DefaultManager(accountRepo);

        
            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepo,
                new Mock<IDialogService>().Object,
                paymentManager,
                defaultManager);

            viewmodel.SaveCommand.Execute();
            
            localDateSetting.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            localDateSetting.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
        }

        [TestMethod]
        public void Init_IncomeEditing_PropertiesSetupCorrectly() {
            Mvx.RegisterSingleton(() => new Mock<IMvxMessenger>().Object);

            var testEndDate = new DateTime(2099, 1, 31);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupGet(x => x.Selected).Returns(new Payment {
                Type = (int) PaymentType.Income,
                IsRecurring = true,
                RecurringPayment = new RecurringPayment {
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


            var defaultManager = new DefaultManager(accountRepo);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepo,
                new Mock<IDialogService>().Object,
                paymentManager,
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