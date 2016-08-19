using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [TestClass]
    public class ModifyPaymentViewModelTest : MvxIoCSupportingTest
    {
        private DateTime localDateSetting;

        [TestInitialize]
        public void Init()
        {
            ClearAll();
            Setup();

            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.SetupAllProperties();
            settingsMockSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<DateTime>(), false))
                .Callback((string key, DateTime date, bool roam) => localDateSetting = date);
            Mvx.RegisterType(() => settingsMockSetup.Object);
            Mvx.RegisterType(() => new Mock<IAutobackupManager>().Object);

            Mvx.RegisterSingleton(() => new Mock<IMvxMessenger>().Object);
        }

                [TestMethod]
        public void Init_IncomeNotEditing_PropertiesSetupCorrectly()
        {
            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepoMock.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var paymentManager = new PaymentManager(new Mock<IPaymentRepository>().Object, 
                accountRepoMock.Object,
                new Mock<IRepository<RecurringPayment>>().Object,
                new Mock<IDialogService>().Object);

            var viewmodel = new ModifyPaymentViewModel(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager);

            viewmodel.Init(PaymentType.Income);

            //Execute and Assert
            viewmodel.SelectedPayment.ShouldNotBeNull();
            viewmodel.SelectedPayment.Type.ShouldBe((int) PaymentType.Income);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeFalse();
        }

        [TestMethod]
        public void Init_ExpenseNotEditing_PropertiesSetupCorrectly()
        {
            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepoMock.SetupGet(x => x.Data)
                .Returns(new ObservableCollection<Account> { new Account { Id = 3 } });

            var paymentManager = new PaymentManager(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IRepository<RecurringPayment>>().Object,
                new Mock<IDialogService>().Object);

            var viewmodel = new ModifyPaymentViewModel(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager);

            //Execute and Assert
            viewmodel.Init(PaymentType.Expense);
            viewmodel.SelectedPayment.Type.ShouldBe((int) PaymentType.Expense);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeFalse();
        }

        [TestMethod]
        public void Init_TransferNotEditing_PropertiesSetupCorrectly() 
        {
            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepoMock.SetupGet(x => x.Data)
                .Returns(new ObservableCollection<Account> { new Account { Id = 3 } });

            var paymentManager = new PaymentManager(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IRepository<RecurringPayment>>().Object,
                new Mock<IDialogService>().Object);

            var viewmodel = new ModifyPaymentViewModel(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager);

            //Execute and Assert
            viewmodel.Init(PaymentType.Transfer);
            viewmodel.SelectedPayment.Type.ShouldBe((int)PaymentType.Transfer);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeTrue();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeFalse();
        }

        [TestMethod]
        public void Save_UpdateTimeStamp()
        {
            var selectedPayment = new Payment
            {
                ChargedAccountId = 3,
                ChargedAccount = new Account {Id = 3, Name = "3"}
            };

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Payment>());
            paymentRepoSetup.SetupGet(x => x.Selected).Returns(selectedPayment);
            paymentRepoSetup.Setup(x => x.Save(selectedPayment)).Returns(true);

            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepoMock.SetupGet(x => x.Data)
                .Returns(new ObservableCollection<Account> { new Account { Id = 3, Name = "3" } });

            var dialogService = new Mock<IDialogService>().Object;

            var paymentManagerSetup = new Mock<IPaymentManager>();
            paymentManagerSetup.Setup(x => x.SavePayment(It.IsAny<Payment>())).Returns(true);
            paymentManagerSetup.Setup(x => x.AddPaymentAmount(It.IsAny<Payment>())).Returns(true);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepoMock.Object,
                dialogService, 
                paymentManagerSetup.Object)
            {
                SelectedPayment = selectedPayment
            };
            viewmodel.SaveCommand.Execute();

            localDateSetting.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            localDateSetting.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
        }

        [TestMethod]
        public void Init_IncomeEditing_PropertiesSetupCorrectly()
        {
            var testEndDate = new DateTime(2099, 1, 31);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.FindById(It.IsAny<int>())).Returns(new Payment
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
            accountRepoMock.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var paymentManager = new PaymentManager(paymentRepoSetup.Object,
                accountRepoMock.Object,
                new Mock<IRepository<RecurringPayment>>().Object,
                new Mock<IDialogService>().Object);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager);

            viewmodel.Init(PaymentType.Income, 12);

            //Execute and Assert
            viewmodel.SelectedPayment.ShouldNotBeNull();
            viewmodel.SelectedPayment.Type.ShouldBe((int) PaymentType.Income);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeTrue();
            viewmodel.SelectedPayment.RecurringPayment.EndDate.ShouldBe(testEndDate);
            viewmodel.SelectedPayment.RecurringPayment.IsEndless.ShouldBeFalse();
        }

        [TestMethod]
        public void Init_ExpenseEditing_PropertiesSetupCorrectly() {
            var testEndDate = new DateTime(2099, 1, 31);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.FindById(It.IsAny<int>())).Returns(new Payment {
                Type = (int)PaymentType.Expense,
                IsRecurring = true,
                RecurringPayment = new RecurringPayment {
                    EndDate = testEndDate
                }
            });

            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepoMock.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var paymentManager = new PaymentManager(paymentRepoSetup.Object,
                            accountRepoMock.Object,
                            new Mock<IRepository<RecurringPayment>>().Object,
                            new Mock<IDialogService>().Object);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager);

            viewmodel.Init(PaymentType.Income, 12);

            //Execute and Assert
            viewmodel.SelectedPayment.ShouldNotBeNull();
            viewmodel.SelectedPayment.Type.ShouldBe((int)PaymentType.Expense);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeTrue();
            viewmodel.SelectedPayment.RecurringPayment.EndDate.ShouldBe(testEndDate);
            viewmodel.SelectedPayment.RecurringPayment.IsEndless.ShouldBeFalse();
        }

        [TestMethod]
        public void Init_TransferEditing_PropertiesSetupCorrectly() {
            var testEndDate = new DateTime(2099, 1, 31);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.FindById(It.IsAny<int>())).Returns(new Payment {
                Type = (int)PaymentType.Transfer,
                IsRecurring = true,
                RecurringPayment = new RecurringPayment {
                    EndDate = testEndDate
                }
            });

            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepoMock.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var paymentManager = new PaymentManager(paymentRepoSetup.Object,
                            accountRepoMock.Object,
                            new Mock<IRepository<RecurringPayment>>().Object,
                            new Mock<IDialogService>().Object);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager);

            viewmodel.Init(PaymentType.Income, 12);

            //Execute and Assert
            viewmodel.SelectedPayment.ShouldNotBeNull();
            viewmodel.SelectedPayment.Type.ShouldBe((int)PaymentType.Transfer);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeTrue();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeTrue();
            viewmodel.SelectedPayment.RecurringPayment.EndDate.ShouldBe(testEndDate);
            viewmodel.SelectedPayment.RecurringPayment.IsEndless.ShouldBeFalse();
        }
        [TestMethod]
        public void SelectedItemChangedCommand_UpdatesCorrectely()
        {
            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepoMock.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var paymentManager = new PaymentManager(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IRepository<RecurringPayment>>().Object,
                new Mock<IDialogService>().Object);

            var viewmodel = new ModifyPaymentViewModel(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager);

            viewmodel.Init(PaymentType.Income);

            Account Test1 = new Account();//target account
            Account Test2 = new Account();//charge account
            viewmodel.TargetAccounts.Add(Test1);
            viewmodel.ChargedAccounts.Add(Test1);
            viewmodel.TargetAccounts.Add(Test2);
            viewmodel.ChargedAccounts.Add(Test2);

            viewmodel.SelectedPayment.TargetAccount = Test1;
            viewmodel.SelectedPayment.ChargedAccount = Test2;

            viewmodel.SelectedItemChangedCommand.Execute();

            viewmodel.ChargedAccounts.Contains(viewmodel.SelectedPayment.ChargedAccount).ShouldBeTrue();
            viewmodel.TargetAccounts.Contains(viewmodel.SelectedPayment.TargetAccount).ShouldBeTrue();
            viewmodel.ChargedAccounts.Contains(viewmodel.SelectedPayment.TargetAccount).ShouldBeFalse();
            viewmodel.TargetAccounts.Contains(viewmodel.SelectedPayment.ChargedAccount).ShouldBeFalse();
        }
    }
}