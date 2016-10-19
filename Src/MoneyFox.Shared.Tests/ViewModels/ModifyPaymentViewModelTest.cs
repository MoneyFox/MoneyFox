using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
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
        [TestInitialize]
        public void Init()
        {
            ClearAll();
            Setup();

            Mvx.RegisterSingleton(() => new Mock<IMvxMessenger>().Object);
        }

        [TestMethod]
        public void Init_IncomeNotEditing_PropertiesSetupCorrectly()
        {
            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));
            accountRepoMock.Setup(x => x.GetList(null)).Returns(new List<AccountViewModel>());

            var paymentManager = new PaymentManager(new Mock<IPaymentRepository>().Object, 
                accountRepoMock.Object,
                new Mock<IRecurringPaymentRepository>().Object,
                new Mock<IDialogService>().Object);

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            Mvx.RegisterType(() => settingsManagerMock.Object);

            var viewmodel = new ModifyPaymentViewModel(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager,
                settingsManagerMock.Object);

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
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));
            accountRepoMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel> { new AccountViewModel { Id = 3 } });

            var paymentManager = new PaymentManager(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IRecurringPaymentRepository>().Object,
                new Mock<IDialogService>().Object);

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();
        
            Mvx.RegisterType(() => settingsManagerMock.Object);

            var viewmodel = new ModifyPaymentViewModel(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager,
                settingsManagerMock.Object);

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
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));
            accountRepoMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel> { new AccountViewModel { Id = 3 } });

            var paymentManager = new PaymentManager(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IRecurringPaymentRepository>().Object,
                new Mock<IDialogService>().Object);

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            Mvx.RegisterType(() => settingsManagerMock.Object);

            var viewmodel = new ModifyPaymentViewModel(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager,
                settingsManagerMock.Object);

            //Execute and Assert
            viewmodel.Init(PaymentType.Transfer);
            viewmodel.SelectedPayment.Type.ShouldBe((int)PaymentType.Transfer);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeTrue();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeFalse();
        }

        [TestMethod]
        public void Save_UpdateTimeStamp()
        {
            var selectedPayment = new PaymentViewModel
            {
                ChargedAccountId = 3,
                ChargedAccountViewModel = new AccountViewModel {Id = 3, Name = "3"}
            };

            var localDateSetting = DateTime.MinValue;

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupSet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>())
                .Callback((DateTime x) => localDateSetting = x);
            Mvx.RegisterType(() => settingsManagerMock.Object);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.GetList(null)).Returns(new List<PaymentViewModel>());
            paymentRepoSetup.Setup(x => x.FindById(It.IsAny<int>())).Returns(selectedPayment);
            paymentRepoSetup.Setup(x => x.Save(selectedPayment)).Returns(true);

            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));
            accountRepoMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel> { new AccountViewModel { Id = 3, Name = "3" } });

            var dialogService = new Mock<IDialogService>().Object;

            var paymentManagerSetup = new Mock<IPaymentManager>();
            paymentManagerSetup.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>())).Returns(true);
            paymentManagerSetup.Setup(x => x.AddPaymentAmount(It.IsAny<PaymentViewModel>())).Returns(true);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepoMock.Object,
                dialogService, 
                paymentManagerSetup.Object,
                settingsManagerMock.Object)
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
            paymentRepoSetup.Setup(x => x.FindById(It.IsAny<int>())).Returns(new PaymentViewModel
            {
                Type = (int) PaymentType.Income,
                IsRecurring = true,
                RecurringPayment = new RecurringPaymentViewModel
                {
                    EndDate = testEndDate
                }
            });

            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));
            accountRepoMock.Setup(x => x.GetList(null)).Returns(new List<AccountViewModel>());

            var paymentManager = new PaymentManager(paymentRepoSetup.Object,
                accountRepoMock.Object,
                new Mock<IRecurringPaymentRepository>().Object,
                new Mock<IDialogService>().Object);

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            Mvx.RegisterType(() => settingsManagerMock.Object);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager,
                settingsManagerMock.Object);

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
            paymentRepoSetup.Setup(x => x.FindById(It.IsAny<int>())).Returns(new PaymentViewModel {
                Type = (int)PaymentType.Expense,
                IsRecurring = true,
                RecurringPayment = new RecurringPaymentViewModel {
                    EndDate = testEndDate
                }
            });

            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));
            accountRepoMock.Setup(x => x.GetList(null)).Returns(new List<AccountViewModel>());

            var paymentManager = new PaymentManager(paymentRepoSetup.Object,
                            accountRepoMock.Object,
                            new Mock<IRecurringPaymentRepository>().Object,
                            new Mock<IDialogService>().Object);

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            Mvx.RegisterType(() => settingsManagerMock.Object);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager,
                settingsManagerMock.Object);

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
            paymentRepoSetup.Setup(x => x.FindById(It.IsAny<int>())).Returns(new PaymentViewModel {
                Type = (int)PaymentType.Transfer,
                IsRecurring = true,
                RecurringPayment = new RecurringPaymentViewModel {
                    EndDate = testEndDate
                }
            });

            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));
            accountRepoMock.Setup(x => x.GetList(null)).Returns(new List<AccountViewModel>());

            var paymentManager = new PaymentManager(paymentRepoSetup.Object,
                            accountRepoMock.Object,
                            new Mock<IRecurringPaymentRepository>().Object,
                            new Mock<IDialogService>().Object);

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            Mvx.RegisterType(() => settingsManagerMock.Object);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager,
                settingsManagerMock.Object);

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
            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            Mvx.RegisterType(() => settingsManagerMock.Object);

            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));
            accountRepoMock.Setup(x => x.GetList(null)).Returns(new List<AccountViewModel>());

            var paymentManager = new PaymentManager(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IRecurringPaymentRepository>().Object,
                new Mock<IDialogService>().Object);

            var viewmodel = new ModifyPaymentViewModel(new Mock<IPaymentRepository>().Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManager,
                settingsManagerMock.Object);

            viewmodel.Init(PaymentType.Income);

            AccountViewModel test1 = new AccountViewModel();//target AccountViewModel
            AccountViewModel test2 = new AccountViewModel();//charge AccountViewModel
            viewmodel.TargetAccounts.Add(test1);
            viewmodel.ChargedAccounts.Add(test1);
            viewmodel.TargetAccounts.Add(test2);
            viewmodel.ChargedAccounts.Add(test2);

            viewmodel.SelectedPayment.TargetAccountViewModel = test1;
            viewmodel.SelectedPayment.ChargedAccountViewModel = test2;

            viewmodel.SelectedItemChangedCommand.Execute();

            viewmodel.ChargedAccounts.Contains(viewmodel.SelectedPayment.ChargedAccountViewModel).ShouldBeTrue();
            viewmodel.TargetAccounts.Contains(viewmodel.SelectedPayment.TargetAccountViewModel).ShouldBeTrue();
            viewmodel.ChargedAccounts.Contains(viewmodel.SelectedPayment.TargetAccountViewModel).ShouldBeFalse();
            viewmodel.TargetAccounts.Contains(viewmodel.SelectedPayment.ChargedAccountViewModel).ShouldBeFalse();
        }

        [TestMethod]
        public void SaveCommand_RecurrenceStringDaily_RecurrenceSetCorrectly()
        {
            //setup
            var testPayment = new PaymentViewModel();

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            Mvx.RegisterType(() => settingsManagerMock.Object);

            var paymentManagerMock = new Mock<IPaymentManager>();
            paymentManagerMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>())).Callback((PaymentViewModel payment) => testPayment = payment);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManagerMock.Object,
                settingsManagerMock.Object);

            viewmodel.Init(PaymentType.Income);
            viewmodel.SelectedPayment.ChargedAccountViewModel = new AccountViewModel();
            viewmodel.SelectedPayment.IsRecurring = true;
            viewmodel.RecurrenceString = Strings.DailyLabel;

            // execute
            viewmodel.SaveCommand.Execute();

            //Assert
            testPayment.RecurringPayment.ShouldNotBeNull();
            testPayment.RecurringPayment.Recurrence.ShouldBe((int)PaymentRecurrence.Daily);
        }

        [TestMethod]
        public void SaveCommand_RecurrenceStringWeekly_RecurrenceSetCorrectly() {
            //setup
            var testPayment = new PaymentViewModel();

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            Mvx.RegisterType(() => settingsManagerMock.Object);

            var paymentManagerMock = new Mock<IPaymentManager>();
            paymentManagerMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>())).Callback((PaymentViewModel payment) => testPayment = payment);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManagerMock.Object,
                settingsManagerMock.Object);

            viewmodel.Init(PaymentType.Income);
            viewmodel.SelectedPayment.ChargedAccountViewModel = new AccountViewModel();
            viewmodel.SelectedPayment.IsRecurring = true;
            viewmodel.RecurrenceString = Strings.WeeklyLabel;

            // execute
            viewmodel.SaveCommand.Execute();

            //Assert
            testPayment.RecurringPayment.ShouldNotBeNull();
            testPayment.RecurringPayment.Recurrence.ShouldBe((int)PaymentRecurrence.Weekly);
        }

        [TestMethod]
        public void SaveCommand_RecurrenceStringMonthly_RecurrenceSetCorrectly() {
            //setup
            var testPayment = new PaymentViewModel();

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            Mvx.RegisterType(() => settingsManagerMock.Object);

            var paymentManagerMock = new Mock<IPaymentManager>();
            paymentManagerMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>())).Callback((PaymentViewModel payment) => testPayment = payment);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManagerMock.Object,
                settingsManagerMock.Object);

            viewmodel.Init(PaymentType.Income);
            viewmodel.SelectedPayment.ChargedAccountViewModel = new AccountViewModel();
            viewmodel.SelectedPayment.IsRecurring = true;
            viewmodel.RecurrenceString = Strings.MonthlyLabel;

            // execute
            viewmodel.SaveCommand.Execute();

            //Assert
            testPayment.RecurringPayment.ShouldNotBeNull();
            testPayment.RecurringPayment.Recurrence.ShouldBe((int)PaymentRecurrence.Monthly);
        }

        [TestMethod]
        public void SaveCommand_RecurrenceStringYearly_RecurrenceSetCorrectly() {
            //setup
            var testPayment = new PaymentViewModel();

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            Mvx.RegisterType(() => settingsManagerMock.Object);

            var paymentManagerMock = new Mock<IPaymentManager>();
            paymentManagerMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>())).Callback((PaymentViewModel payment) => testPayment = payment);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManagerMock.Object,
                settingsManagerMock.Object);

            viewmodel.Init(PaymentType.Income);
            viewmodel.SelectedPayment.ChargedAccountViewModel = new AccountViewModel();
            viewmodel.SelectedPayment.IsRecurring = true;
            viewmodel.RecurrenceString = Strings.YearlyLabel;

            // execute
            viewmodel.SaveCommand.Execute();

            //Assert
            testPayment.RecurringPayment.ShouldNotBeNull();
            testPayment.RecurringPayment.Recurrence.ShouldBe((int)PaymentRecurrence.Yearly);
        }

        [TestMethod]
        public void SaveCommand_RecurrenceStringBiweekly_RecurrenceSetCorrectly() {
            //setup
            var testPayment = new PaymentViewModel();

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            var accountRepoMock = new Mock<IAccountRepository>();
            accountRepoMock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            Mvx.RegisterType(() => settingsManagerMock.Object);

            var paymentManagerMock = new Mock<IPaymentManager>();
            paymentManagerMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>())).Callback((PaymentViewModel payment) => testPayment = payment);

            var viewmodel = new ModifyPaymentViewModel(paymentRepoSetup.Object,
                accountRepoMock.Object,
                new Mock<IDialogService>().Object,
                paymentManagerMock.Object,
                settingsManagerMock.Object);

            viewmodel.Init(PaymentType.Income);
            viewmodel.SelectedPayment.ChargedAccountViewModel = new AccountViewModel();
            viewmodel.SelectedPayment.IsRecurring = true;
            viewmodel.RecurrenceString = Strings.BiweeklyLabel;

            // execute
            viewmodel.SaveCommand.Execute();

            //Assert
            testPayment.RecurringPayment.ShouldNotBeNull();
            testPayment.RecurringPayment.Recurrence.ShouldBe((int)PaymentRecurrence.Biweekly);
        }

        [TestMethod]
        public void Cancel_SelectedPaymentReseted()
        {
            double amount = 99;
            var basePayment = new PaymentViewModel { Id = 5, Amount = amount };
            var payment = new PaymentViewModel { Id = 5, Amount = amount };

            var paymentRepositorySetup = new Mock<IPaymentRepository>();
            paymentRepositorySetup.Setup(x => x.FindById(It.IsAny<int>())).Returns(basePayment);

            var viewmodel = new ModifyPaymentViewModel(paymentRepositorySetup.Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object,
                new Mock<IPaymentManager>().Object,
                new Mock<ISettingsManager>().Object)
            {
                SelectedPayment = payment
            };

            viewmodel.SelectedPayment.Amount = 7777;
            viewmodel.CancelCommand.Execute();

            viewmodel.SelectedPayment.Amount.ShouldBe(amount);
        }
    }
}