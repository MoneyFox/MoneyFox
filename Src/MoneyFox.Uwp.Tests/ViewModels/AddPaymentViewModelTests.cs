using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountById;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Commands.CreatePayment;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Infrastructure.Tests.Collections;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    [Collection("AutoMapperCollection")]
    public class AddPaymentViewModelTests
    {
        private readonly IMapper mapper;
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IDialogService> dialogServiceMock;
        private readonly Mock<NavigationService> navigationServiceMock;

        public AddPaymentViewModelTests(MapperCollectionFixture fixture)
        {
            mediatorMock = new Mock<IMediator>();
            mapper = fixture.Mapper;
            settingsFacadeMock = new Mock<ISettingsFacade>();
            backupServiceMock = new Mock<IBackupService>();
            dialogServiceMock = new Mock<IDialogService>();
            navigationServiceMock = new Mock<NavigationService>();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountsQuery>(), default))
                        .ReturnsAsync(new List<Account>());

            mediatorMock.Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), default))
                        .ReturnsAsync(Unit.Value);
        }

        [Theory]
        [InlineData(PaymentType.Expense)]
        [InlineData(PaymentType.Income)]
        [InlineData(PaymentType.Transfer)]
        public async Task Initialize_PaymentHasCorrectType(PaymentType type)
        {
            // Arrange
            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            addPaymentVm.PaymentType = type;

            // Act
            await addPaymentVm.InitializeCommand.ExecuteAsync();

            // Assert
            addPaymentVm.SelectedPayment.Type.ShouldEqual(type);
        }

        [Theory]
        [InlineData(PaymentType.Expense)]
        [InlineData(PaymentType.Income)]
        public async Task Initialize_IncomeExpense_PaymentChargedAccountNotNull(PaymentType type)
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountsQuery>(), default))
                        .ReturnsAsync(new List<Account> {new Account("dfasdf")});

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            addPaymentVm.PaymentType = type;

            // Act
            await addPaymentVm.InitializeCommand.ExecuteAsync();

            // Assert
            addPaymentVm.SelectedPayment.ChargedAccount.ShouldNotBeNull();
            addPaymentVm.SelectedPayment.TargetAccount.ShouldBeNull();
        }

        [Fact]
        public async Task Initialize_Transfer_PaymentChargedAccountNotNull()
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountsQuery>(), default))
                        .ReturnsAsync(new List<Account> {new Account("dfasdf"), new Account("Foo")});

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            addPaymentVm.PaymentType = PaymentType.Transfer;

            // Act
            await addPaymentVm.InitializeCommand.ExecuteAsync();

            // Assert
            addPaymentVm.SelectedPayment.ChargedAccount.ShouldNotBeNull();
            addPaymentVm.SelectedPayment.TargetAccount.ShouldNotBeNull();
        }

        [Fact]
        public async Task AmountStringSetOnInit()
        {
            // Arrange
            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            // Act
            await addPaymentVm.InitializeCommand.ExecuteAsync();

            // Assert
            addPaymentVm.AmountString.ShouldEqual("0.00");
        }

        [Fact]
        public async Task SavePayment_NoAccount_DialogShown()
        {
            // Arrange
            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();

            // Act
            await addPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            dialogServiceMock.Verify(x => x.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.AccountRequiredMessage), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Never);
            backupServiceMock.Verify(x => x.UploadBackupAsync(BackupMode.Manual), Times.Never);
        }

        [Fact]
        public async Task ShowMessageIfAmountIsNegativeOnSave()
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountByIdQuery>(), default))
                        .ReturnsAsync(() => new Account("as"));

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel {Name = "asdf"};
            addPaymentVm.AmountString = "-2";

            // Act
            await addPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            dialogServiceMock.Verify(x => x.ShowMessageAsync(Strings.AmountMayNotBeNegativeTitle, Strings.AmountMayNotBeNegativeMessage),
                                     Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Never);
            backupServiceMock.Verify(x => x.UploadBackupAsync(BackupMode.Manual), Times.Never);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("2")]
        public async Task ShowNoMessageIfAmountIsPositiveOnSave(string amountString)
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountByIdQuery>(), default))
                        .ReturnsAsync(() => new Account("as"));

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel {Name = "asdf"};
            addPaymentVm.AmountString = amountString;

            // Act
            await addPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            dialogServiceMock.Verify(x => x.ShowMessageAsync(Strings.AmountMayNotBeNegativeTitle, Strings.AmountMayNotBeNegativeMessage),
                                     Times.Never);
        }

        [Fact]
        public async Task SavePayment_ResultSucceeded_CorrectMethodCalls()
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountByIdQuery>(), default))
                        .ReturnsAsync(() => new Account("as"));

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel {Name = "asdf"};

            // Act
            await addPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<CreatePaymentCommand>(), default), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.UploadBackupAsync(BackupMode.Manual), Times.Never);
        }

        [Fact]
        public async Task SavePayment_ResultSucceededWithBackup_CorrectMethodCalls()
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountByIdQuery>(), default))
                        .ReturnsAsync(() => new Account("as"));

            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel {Name = "asdf"};

            // Act
            await addPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<CreatePaymentCommand>(), default), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.UploadBackupAsync(BackupMode.Automatic), Times.Once);
        }

        [Fact]
        public async Task SavePayment_ResultFailed_CorrectMethodCalls()
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), default))
                        .Callback(() => throw new Exception());

            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountByIdQuery>(), default))
                        .ReturnsAsync(() => new Account("as"));

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel {Name = "asdf"};

            // Act
            await Assert.ThrowsAsync<Exception>(async () => await addPaymentVm.SaveCommand.ExecuteAsync());

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<CreatePaymentCommand>(), default), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            backupServiceMock.Verify(x => x.UploadBackupAsync(BackupMode.Manual), Times.Never);
        }

        [Fact]
        public async Task SavePayment_ResultFailedWithBackup_CorrectMethodCalls()
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), default))
                        .Callback(() => throw new Exception());

            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountByIdQuery>(), default))
                        .ReturnsAsync(() => new Account("as"));

            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel {Name = "asdf"};

            // Act
            await Assert.ThrowsAsync<Exception>(async () => await addPaymentVm.SaveCommand.ExecuteAsync());

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<CreatePaymentCommand>(), default), Times.Once);
            mediatorMock.Verify(x => x.Send(It.IsAny<GetAccountByIdQuery>(), default), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            backupServiceMock.Verify(x => x.UploadBackupAsync(BackupMode.Manual), Times.Never);
        }

        [Theory]
        [InlineData("de-CH", "12.20", 12.20)]
        [InlineData("de-DE", "12,20", 12.20)]
        [InlineData("en-US", "12.20", 12.20)]
        [InlineData("ru-RU", "12,20", 12.20)]
        [InlineData("de-CH", "-12.20", -12.20)]
        [InlineData("de-DE", "-12,20", -12.20)]
        [InlineData("en-US", "-12.20", -12.20)]
        [InlineData("ru-RU", "-12,20", -12.20)]
        public async Task AmountCorrectlyFormattedOnSave(string cultureString, string amountString, decimal expectedAmount)
        {
            // Arrange
            var cultureInfo = new CultureInfo(cultureString);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountByIdQuery>(), default))
                        .ReturnsAsync(() => new Account("as"));

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel {Name = "asdf"};

            // Act
            addPaymentVm.AmountString = amountString;
            await addPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            addPaymentVm.SelectedPayment.Amount.ShouldEqual(expectedAmount);
        }
    }
}
