using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Payments.Commands.CreatePayment;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.Tests.Collections;
using MoneyFox.Presentation.ViewModels;
using Moq;
using Should;
using Xunit;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    [Collection("AutoMapperCollection")]
    public class AddPaymentViewModelTests
    {
        private readonly IMapper mapper;

        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IPaymentService> paymentServiceMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IDialogService> dialogServiceMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        public AddPaymentViewModelTests(MapperCollectionFixture fixture)
        {
            mediatorMock = new Mock<IMediator>();
            mapper = fixture.Mapper;
            paymentServiceMock = new Mock<IPaymentService>();
            settingsFacadeMock = new Mock<ISettingsFacade>();
            backupServiceMock = new Mock<IBackupService>();
            dialogServiceMock = new Mock<IDialogService>();
            navigationServiceMock = new Mock<INavigationService>();

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
                                                       paymentServiceMock.Object,
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
                                                       paymentServiceMock.Object,
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
                        .ReturnsAsync(new List<Account> {new Account("dfasdf"), new Account("Foo") });
            
            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       paymentServiceMock.Object,
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
        public async Task SavePayment_NoAccount_DialogShown()
        {
            // Arrange
            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       paymentServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();

            // Act
            await addPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            dialogServiceMock.Verify(x => x.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.AccountRequiredMessage), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Never);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public async Task SavePayment_ResultSucceeded_CorrectMethodCalls()
        {
            // Arrange
            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       paymentServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel{Name = "asdf"};

            // Act
            await addPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<CreatePaymentCommand>(), default), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public async Task SavePayment_ResultSucceededWithBackup_CorrectMethodCalls()
        {
            // Arrange
            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       paymentServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel { Name = "asdf" };

            // Act
            await addPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<CreatePaymentCommand>(), default), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Once);
        }

        [Fact]
        public async Task SavePayment_ResultFailed_CorrectMethodCalls()
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), default))
                              .Callback(() => throw new Exception());

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       paymentServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel { Name = "asdf" };

            // Act
            await Assert.ThrowsAsync<Exception>(async () => await addPaymentVm.SaveCommand.ExecuteAsync());

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<CreatePaymentCommand>(), default), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public async Task SavePayment_ResultFailedWithBackup_CorrectMethodCalls()
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), default))
                        .Callback(() => throw new Exception());

            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapper,
                                                       paymentServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel { Name = "asdf" };

            // Act
            await Assert.ThrowsAsync<Exception>(async () => await addPaymentVm.SaveCommand.ExecuteAsync());

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<CreatePaymentCommand>(), default), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }
    }
}
