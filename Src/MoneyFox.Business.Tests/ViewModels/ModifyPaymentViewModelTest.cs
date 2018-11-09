using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoneyFox.Business.Parameters;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Tests;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    public class ModifyPaymentViewModelTest : MvxIoCSupportingTest
    {
        public ModifyPaymentViewModelTest()
        {
            base.Setup();
        }

        [Theory]
        [InlineData(PaymentType.Income)]
        [InlineData(PaymentType.Expense)]
        public async void Init_IncomeNotEditing_PropertiesSetupCorrectly(PaymentType type)
        {
            // Arrange
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAllAccounts()).ReturnsAsync(new List<Account>());

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            var viewmodel = new ModifyPaymentViewModel(new Mock<IPaymentService>().Object,
                                                       accountServiceMock.Object,
                                                       new Mock<IDialogService>().Object,
                                                       settingsManagerMock.Object,
                                                       new Mock<IMvxMessenger>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object);

            viewmodel.Prepare(new ModifyPaymentParameter(type));
            await viewmodel.Initialize();

            // Act / Assert
            viewmodel.SelectedPayment.ShouldNotBeNull();
            viewmodel.SelectedPayment.Type.ShouldEqual(type);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeFalse();
        }

        [Theory]
        [InlineData(PaymentType.Income)]
        [InlineData(PaymentType.Expense)]
        public async void Init_IncomeEditing_PropertiesSetupCorrectly(PaymentType type)
        {
            // Arrange
            var testEndDate = new DateTime(2099, 1, 31);

            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Payment
            {
                Data =
                {
                    Type = type,
                    IsRecurring = true,
                    RecurringPayment = new RecurringPaymentEntity
                    {
                        EndDate = testEndDate
                    }
                }
            });

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAllAccounts()).ReturnsAsync(new List<Account>());

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            var viewmodel = new ModifyPaymentViewModel(paymentServiceMock.Object,
                                                       accountServiceMock.Object,
                                                       new Mock<IDialogService>().Object,
                                                       settingsManagerMock.Object,
                                                       new Mock<IMvxMessenger>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object);

            // Act
            viewmodel.Prepare(new ModifyPaymentParameter(type, 12));
            await viewmodel.Initialize();

            // Assert
            viewmodel.SelectedPayment.ShouldNotBeNull();
            viewmodel.SelectedPayment.Type.ShouldEqual(type);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeTrue();
            viewmodel.SelectedPayment.RecurringPayment.EndDate.ShouldEqual(testEndDate);
            viewmodel.SelectedPayment.RecurringPayment.IsEndless.ShouldBeFalse();
        }

        [Theory]
        [InlineData(PaymentRecurrence.Daily)]
        [InlineData(PaymentRecurrence.DailyWithoutWeekend)]
        [InlineData(PaymentRecurrence.Weekly)]
        [InlineData(PaymentRecurrence.Biweekly)]
        [InlineData(PaymentRecurrence.Monthly)]
        [InlineData(PaymentRecurrence.Yearly)]
        public async void SaveCommand_Recurrence_RecurrenceSetCorrectly(PaymentRecurrence recurrence)
        {
            // Arrange
            base.Setup();
            var testPayment = new Payment();

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.SavePayments(It.IsAny<Payment[]>()))
                              .Callback((Payment[] payment) => testPayment = payment.First())
                              .Returns(Task.CompletedTask);

            var viewmodel = new ModifyPaymentViewModel(paymentServiceMock.Object,
                                                       new Mock<IAccountService>().Object,
                                                       new Mock<IDialogService>().Object,
                                                       settingsManagerMock.Object,
                                                       new Mock<IMvxMessenger>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object);

            viewmodel.Prepare(new ModifyPaymentParameter(PaymentType.Income));
            await viewmodel.Initialize();
            viewmodel.SelectedPayment.ChargedAccount = new AccountViewModel(new Account());
            viewmodel.SelectedPayment.IsRecurring = true;
            viewmodel.Recurrence = recurrence;

            // Act
            viewmodel.SaveCommand.Execute();

            // Assert
            testPayment.Data.RecurringPayment.ShouldNotBeNull();
            testPayment.Data.RecurringPayment.Recurrence.ShouldEqual(recurrence);
        }

        [Theory]
        [InlineData("35", 35, "de-CH")]
        [InlineData("35.5", 35.5, "de-CH")]
        [InlineData("35,5", 35.5, "de-CH")]
        [InlineData("35.50", 35.5, "de-CH")]
        [InlineData("35,50", 35.5, "de-CH")]
        [InlineData("3,500.5", 3500.5, "de-CH")]
        [InlineData("3,500.50", 3500.5, "de-CH")]
        [InlineData("3.500,5", 3500.5, "de-CH")]
        [InlineData("3.500,50", 3500.5, "de-CH")]
        [InlineData("35", 35, "de-DE")]
        [InlineData("35,5", 35.5, "de-DE")]
        [InlineData("35,50", 35.5, "de-DE")]
        [InlineData("35.5", 35.5, "de-DE")]
        [InlineData("35.50", 35.5, "de-DE")]
        [InlineData("3,500.5", 3500.5, "de-DE")]
        [InlineData("3,500.50", 3500.5, "de-DE")]
        [InlineData("3.500,5", 3500.5, "de-DE")]
        [InlineData("3.500,50", 3500.5, "de-DE")]
        [InlineData("35", 35, "en-GB")]
        [InlineData("35,5", 35.5, "en-GB")]
        [InlineData("35,50", 35.5, "en-GB")]
        [InlineData("35.5", 35.5, "en-GB")]
        [InlineData("35.50", 35.5, "en-GB")]
        [InlineData("3,500.5", 3500.5, "en-GB")]
        [InlineData("3,500.50", 3500.5, "en-GB")]
        [InlineData("3.500,5", 3500.5, "en-GB")]
        [InlineData("3.500,50", 3500.5, "en-GB")]
        [InlineData("35", 35, "en-US")]
        [InlineData("35,5", 35.5, "en-US")]
        [InlineData("35,50", 35.5, "en-US")]
        [InlineData("35.5", 35.5, "en-US")]
        [InlineData("35.50", 35.5, "en-US")]
        [InlineData("3,500.5", 3500.5, "en-US")]
        [InlineData("3,500.50", 3500.5, "en-US")]
        [InlineData("3.500,5", 3500.5, "en-US")]
        [InlineData("3.500,50", 3500.5, "en-US")]
        [InlineData("35", 35, "it-IT")]
        [InlineData("35,5", 35.5, "it-IT")]
        [InlineData("35,50", 35.5, "it-IT")]
        [InlineData("35.5", 35.5, "it-IT")]
        [InlineData("35.50", 35.5, "it-IT")]
        [InlineData("3,500.5", 3500.5, "it-IT")]
        [InlineData("3,500.50", 3500.5, "it-IT")]
        [InlineData("3.500,5", 3500.5, "it-IT")]
        public void AmountString_CorrectConvertedAmount(string amount, double convertedAmount, string culture)
        {
            // Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture, false);

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            var testPayment = new PaymentViewModel(new Payment());

            var viewmodel = new ModifyPaymentViewModel(new Mock<IPaymentService>().Object,
                                                       new Mock<IAccountService>().Object,
                                                       new Mock<IDialogService>().Object,
                                                       settingsManagerMock.Object,
                                                       new Mock<IMvxMessenger>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object)
            {
                SelectedPayment = testPayment
            };

            // Act
            viewmodel.AmountString = amount;

            // Assert
            viewmodel.AmountString.ShouldEqual(convertedAmount.ToString("N", CultureInfo.CurrentCulture));
        }

        [Fact]
        public async void Init_TransferEditing_PropertiesSetupCorrectly()
        {
            // Arrange
            var testEndDate = new DateTime(2099, 1, 31);

            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Payment
            {
                Data =
                {
                    Type = PaymentType.Transfer,
                    IsRecurring = true,
                    RecurringPayment = new RecurringPaymentEntity
                    {
                        EndDate = testEndDate
                    }
                }
            });

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAllAccounts()).ReturnsAsync(new List<Account>());

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            var viewmodel = new ModifyPaymentViewModel(paymentServiceMock.Object,
                                                       accountServiceMock.Object,
                                                       new Mock<IDialogService>().Object,
                                                       settingsManagerMock.Object,
                                                       new Mock<IMvxMessenger>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object);

            // Act
            viewmodel.Prepare(new ModifyPaymentParameter(PaymentType.Income, 12));
            await viewmodel.Initialize();

            // Assert
            viewmodel.SelectedPayment.ShouldNotBeNull();
            viewmodel.SelectedPayment.Type.ShouldEqual(PaymentType.Transfer);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeTrue();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeTrue();
            viewmodel.SelectedPayment.RecurringPayment.EndDate.ShouldEqual(testEndDate);
            viewmodel.SelectedPayment.RecurringPayment.IsEndless.ShouldBeFalse();
        }

        [Fact]
        public async void Init_TransferNotEditing_PropertiesSetupCorrectly()
        {
            // Arrange
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAllAccounts())
                              .ReturnsAsync(new List<Account> {new Account {Data = {Id = 3}}});

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            var viewmodel = new ModifyPaymentViewModel(new Mock<IPaymentService>().Object,
                                                       accountServiceMock.Object,
                                                       new Mock<IDialogService>().Object,
                                                       settingsManagerMock.Object,
                                                       new Mock<IMvxMessenger>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object);

            // Act / Assert
            viewmodel.Prepare(new ModifyPaymentParameter(PaymentType.Transfer));
            await viewmodel.Initialize();
            viewmodel.SelectedPayment.Type.ShouldEqual(PaymentType.Transfer);
            viewmodel.SelectedPayment.IsTransfer.ShouldBeTrue();
            viewmodel.SelectedPayment.IsRecurring.ShouldBeFalse();
        }

        [Fact]
        public void Save_NoAccount_AccountRequiredInfoShown()
        {
            // Arrange
            base.Setup();
            bool dialogShown = false;

            var dialogServiceMock = new Mock<IDialogService>();
            dialogServiceMock.Setup(x => x.ShowMessage(It.Is<string>(s => s == Strings.MandatoryFieldEmptyTitle),
                                                       It.Is<string>(s => s == Strings.AccountRequiredMessage)))
                             .Callback(() => dialogShown = true)
                             .Returns(Task.CompletedTask);

            var viewmodel = new ModifyPaymentViewModel(new Mock<IPaymentService>().Object,
                                                       new Mock<IAccountService>().Object,
                                                       dialogServiceMock.Object,
                                                       new Mock<ISettingsManager>().Object,
                                                       new Mock<IMvxMessenger>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object);

            // Act
            viewmodel.SelectedPayment = new PaymentViewModel();
            viewmodel.SaveCommand.Execute();

            //Assert
            Assert.True(dialogShown);
        }

        [Fact]
        public void Save_OldDate_InvalidDateInfoShown()
        {
            // Arrange
            base.Setup();
            bool dialogShown = false;

            var dialogServiceMock = new Mock<IDialogService>();
            dialogServiceMock.Setup(x => x.ShowMessage(It.Is<string>(s => s == Strings.InvalidEnddateTitle),
                                                       It.Is<string>(s => s == Strings.InvalidEnddateMessage)))
                             .Callback(() => dialogShown = true)
                             .Returns(Task.CompletedTask);


            var payment = new PaymentViewModel
            {
                ChargedAccount = new AccountViewModel(new Account()) {Name = "Konto"},
                IsRecurring = true,
                Date = DateTime.Now.AddDays(-2)
            };

            var viewmodel = new ModifyPaymentViewModel(new Mock<IPaymentService>().Object,
                                                       new Mock<IAccountService>().Object,
                                                       dialogServiceMock.Object,
                                                       new Mock<ISettingsManager>().Object,
                                                       new Mock<IMvxMessenger>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object);
            // Act
            viewmodel.SelectedPayment = payment;
            viewmodel.SaveCommand.Execute();

            //Assert
            Assert.True(dialogShown);
        }

        [Fact]
        public void Save_UpdateTimeStamp()
        {
            // Arrange
            base.Setup();
            var account = new AccountEntity {Id = 3, Name = "3"};
            var selectedPayment = new Payment
            {
                Data =
                {
                    ChargedAccount = account
                }
            };

            var localDateSetting = DateTime.MinValue;

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupSet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>())
                               .Callback((DateTime x) => localDateSetting = x);

            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(selectedPayment);
            paymentServiceMock.Setup(x => x.SavePayments(selectedPayment)).Returns(Task.CompletedTask);

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAllAccounts())
                              .ReturnsAsync(new List<Account> {new Account {Data = {Id = 3, Name = "3"}}});

            var dialogService = new Mock<IDialogService>().Object;

            var viewmodel = new ModifyPaymentViewModel(paymentServiceMock.Object,
                                                       accountServiceMock.Object,
                                                       dialogService,
                                                       settingsManagerMock.Object,
                                                       new Mock<IMvxMessenger>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object)
            {
                SelectedPayment =
                    new PaymentViewModel(selectedPayment) {ChargedAccount = new AccountViewModel(new Account(account))}
            };

            // Act
            viewmodel.SaveCommand.Execute();

            // Assert
            localDateSetting.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            localDateSetting.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
        }
    }
}