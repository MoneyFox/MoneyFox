using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MockQueryable.Moq;
using MoneyFox.BusinessLogic;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.ViewModels;
using Moq;
using Should;
using Xunit;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AddPaymentViewModelTests 
    {
        private readonly Mock<IPaymentService> paymentServiceMock;
        private readonly Mock<ICrudServicesAsync> crudServiceMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IDialogService> dialogServiceMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        public AddPaymentViewModelTests()
        {
            paymentServiceMock = new Mock<IPaymentService>();
            crudServiceMock = new Mock<ICrudServicesAsync>();
            settingsFacadeMock = new Mock<ISettingsFacade>();
            backupServiceMock = new Mock<IBackupService>();
            dialogServiceMock = new Mock<IDialogService>();
            navigationServiceMock = new Mock<INavigationService>();

            crudServiceMock.Setup(x => x.ReadManyNoTracked<AccountViewModel>())
                           .Returns(new List<AccountViewModel>().AsQueryable().BuildMockDbQuery().Object);
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

            var addPaymentVm = new AddPaymentViewModel(paymentServiceMock.Object,
                                                       crudServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            addPaymentVm.SelectedPayment.Type = type;

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

            crudServiceMock.Setup(x => x.ReadManyNoTracked<AccountViewModel>())
                .Returns(new List<AccountViewModel> { new AccountViewModel() }.AsQueryable().BuildMock().Object);

            var addPaymentVm = new AddPaymentViewModel(paymentServiceMock.Object,
                                                       crudServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            addPaymentVm.SelectedPayment.Type = type;

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

            crudServiceMock.Setup(x => x.ReadManyNoTracked<AccountViewModel>())
                           .Returns(new List<AccountViewModel> { new AccountViewModel(), new AccountViewModel() }.AsQueryable().BuildMock().Object);

            var addPaymentVm = new AddPaymentViewModel(paymentServiceMock.Object,
                                                       crudServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            addPaymentVm.SelectedPayment.Type = PaymentType.Transfer;

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

            var addPaymentVm = new AddPaymentViewModel(paymentServiceMock.Object,
                                                       crudServiceMock.Object,
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

            var addPaymentVm = new AddPaymentViewModel(paymentServiceMock.Object,
                crudServiceMock.Object,
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

            var addPaymentVm = new AddPaymentViewModel(paymentServiceMock.Object,
                crudServiceMock.Object,
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

            var addPaymentVm = new AddPaymentViewModel(paymentServiceMock.Object,
                crudServiceMock.Object,
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
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public async Task SavePayment_ResultFailedWithBackup_CorrectMethodCalls()
        {
            // Arrange
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                              .Callback(() => throw new Exception());

            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);

            var addPaymentVm = new AddPaymentViewModel(paymentServiceMock.Object,
                crudServiceMock.Object,
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
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Once);
        }
    }
}