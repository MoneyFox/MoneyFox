using GalaSoft.MvvmLight.Views;
using MockQueryable.Moq;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.ViewModels;
using Moq;
using Should;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Domain.Entities;
using Xunit;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AddPaymentViewModelTests
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IPaymentService> paymentServiceMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IDialogService> dialogServiceMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        public AddPaymentViewModelTests()
        {
            mediatorMock = new Mock<IMediator>();
            mapperMock = new Mock<IMapper>();
            paymentServiceMock = new Mock<IPaymentService>();
            settingsFacadeMock = new Mock<ISettingsFacade>();
            backupServiceMock = new Mock<IBackupService>();
            dialogServiceMock = new Mock<IDialogService>();
            navigationServiceMock = new Mock<INavigationService>();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountsQuery>(), default))
                        .ReturnsAsync(new List<Account>());

            mapperMock.Setup(x => x.Map<List<AccountViewModel>>(It.IsAny<List<Account>>()))
                      .Returns(new List<AccountViewModel>());
        }

        [Theory]
        [InlineData(PaymentType.Expense)]
        [InlineData(PaymentType.Income)]
        [InlineData(PaymentType.Transfer)]
        public async Task Initialize_PaymentHasCorrectType(PaymentType type)
        {
            // Arrange
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                .Returns(Task.CompletedTask);

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object, 
                                                       mapperMock.Object,
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
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                              .Returns(Task.CompletedTask);

            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountsQuery>(), default))
                        .ReturnsAsync(new List<Account> {new Account("dfasdf")});

            mapperMock.Setup(x => x.Map<List<AccountViewModel>>(It.IsAny<List<Account>>()))
                      .Returns(new List<AccountViewModel> {new AccountViewModel()});

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapperMock.Object, 
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
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                              .Returns(Task.CompletedTask);

            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountsQuery>(), default))
                        .ReturnsAsync(new List<Account> { new Account("dfasdf") });

            mapperMock.Setup(x => x.Map<List<AccountViewModel>>(It.IsAny<List<Account>>()))
                      .Returns(new List<AccountViewModel> { new AccountViewModel(), new AccountViewModel() });

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapperMock.Object,
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
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                              .Returns(Task.CompletedTask);

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapperMock.Object,
                                                       paymentServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();

            // Act
            await addPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            paymentServiceMock.Verify(x => x.SavePayment(It.IsAny<PaymentViewModel>()), Times.Never);
            dialogServiceMock.Verify(x => x.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.AccountRequiredMessage), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Never);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public async Task SavePayment_ResultSucceeded_CorrectMethodCalls()
        {
            // Arrange
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                              .Returns(Task.CompletedTask);

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapperMock.Object,
                                                       paymentServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            await addPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            paymentServiceMock.Verify(x => x.SavePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public async Task SavePayment_ResultSucceededWithBackup_CorrectMethodCalls()
        {
            // Arrange
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                              .Returns(Task.CompletedTask);

            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapperMock.Object,
                                                       paymentServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            await addPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            paymentServiceMock.Verify(x => x.SavePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Once);
        }

        [Fact]
        public async Task SavePayment_ResultFailed_CorrectMethodCalls()
        {
            // Arrange
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                              .Callback(() => throw new Exception());

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapperMock.Object,
                                                       paymentServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            await Assert.ThrowsAsync<Exception>(async () => await addPaymentVm.SaveCommand.ExecuteAsync());

            // Assert
            paymentServiceMock.Verify(x => x.SavePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public async Task SavePayment_ResultFailedWithBackup_CorrectMethodCalls()
        {
            // Arrange
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                              .Callback(() => throw new Exception());

            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);

            var addPaymentVm = new AddPaymentViewModel(mediatorMock.Object,
                                                       mapperMock.Object,
                                                       paymentServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            await addPaymentVm.InitializeCommand.ExecuteAsync();
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            await Assert.ThrowsAsync<Exception>(async () => await addPaymentVm.SaveCommand.ExecuteAsync());

            // Assert
            paymentServiceMock.Verify(x => x.SavePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }
    }
}