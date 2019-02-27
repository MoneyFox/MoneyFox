using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using GenericServices;
using MoneyFox.BusinessLogic;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Should;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    [Collection("MvxIocCollection")]
    public class AddPaymentViewModelTests
    {
        private readonly Mock<IPaymentService> paymentServiceMock;
        private readonly Mock<ICrudServicesAsync> crudServiceMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IDialogService> dialogServiceMock;
        private readonly Mock<IMvxNavigationService> navigationServiceMock;

        public AddPaymentViewModelTests()
        {
            paymentServiceMock = new Mock<IPaymentService>();
            crudServiceMock = new Mock<ICrudServicesAsync>();
            settingsFacadeMock = new Mock<ISettingsFacade>();
            backupServiceMock = new Mock<IBackupService>();
            dialogServiceMock = new Mock<IDialogService>();
            navigationServiceMock = new Mock<IMvxNavigationService>();
        }

        [Fact]
        public void Prepare_PaymentCreated()
        {
            // Arrange
            var addPaymentVm = new AddPaymentViewModel(null,
                                                       crudServiceMock.Object,
                                                       null, null,
                                                       new Mock<IMvxMessenger>().Object,
                                                       null, null, null);

            // Act
            addPaymentVm.Prepare(new ModifyPaymentParameter());

            // Assert
            addPaymentVm.SelectedPayment.ShouldNotBeNull();
        }

        [Fact]
        public void Prepare_Title_Set()
        {
            // Arrange
            var addPaymentVm = new AddPaymentViewModel(null,
                                                       crudServiceMock.Object,
                                                       null, null,
                                                       new Mock<IMvxMessenger>().Object,
                                                       null, null, null);
            // Act
            addPaymentVm.Prepare(new ModifyPaymentParameter());

            // Assert
            addPaymentVm.Title.Contains(Strings.AddTitle);
        }

        [Fact]
        public void SavePayment_NoAccount_DialogShown()
        {
            // Arrange
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                .ReturnsAsync(OperationResult.Succeeded());

            var addPaymentVm = new AddPaymentViewModel(paymentServiceMock.Object,
                                                       crudServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       new Mock<IMvxMessenger>().Object,
                                                       backupServiceMock.Object, 
                                                       new Mock<IMvxLogProvider>().Object,
                                                       navigationServiceMock.Object);

            addPaymentVm.Prepare(new ModifyPaymentParameter());

            // Act
            addPaymentVm.SaveCommand.Execute();

            // Assert
            paymentServiceMock.Verify(x => x.SavePayment(It.IsAny<PaymentViewModel>()), Times.Never);
            dialogServiceMock.Verify(x => x.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.AccountRequiredMessage), Times.Once);
            navigationServiceMock.Verify(x => x.Close(It.IsAny<MvxViewModel>(), CancellationToken.None), Times.Never);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Never);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public void SavePayment_ResultSucceeded_CorrectMethodCalls()
        {
            // Arrange
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                .ReturnsAsync(OperationResult.Succeeded());

            var addPaymentVm = new AddPaymentViewModel(paymentServiceMock.Object,
                crudServiceMock.Object,
                dialogServiceMock.Object,
                settingsFacadeMock.Object,
                new Mock<IMvxMessenger>().Object,
                backupServiceMock.Object,
                new Mock<IMvxLogProvider>().Object,
                navigationServiceMock.Object);

            addPaymentVm.Prepare(new ModifyPaymentParameter());
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            addPaymentVm.SaveCommand.Execute();

            // Assert
            paymentServiceMock.Verify(x => x.SavePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationServiceMock.Verify(x => x.Close(It.IsAny<MvxViewModel>(), CancellationToken.None), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Once);
        }

        [Fact]
        public void SavePayment_ResultFailed_CorrectMethodCalls()
        {
            // Arrange
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                .ReturnsAsync(OperationResult.Failed(""));

            var addPaymentVm = new AddPaymentViewModel(paymentServiceMock.Object,
                crudServiceMock.Object,
                dialogServiceMock.Object,
                settingsFacadeMock.Object,
                new Mock<IMvxMessenger>().Object,
                backupServiceMock.Object,
                new Mock<IMvxLogProvider>().Object,
                navigationServiceMock.Object);

            addPaymentVm.Prepare(new ModifyPaymentParameter());
            addPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            addPaymentVm.SaveCommand.Execute();

            // Assert
            paymentServiceMock.Verify(x => x.SavePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            navigationServiceMock.Verify(x => x.Close(It.IsAny<MvxViewModel>(), CancellationToken.None), Times.Never);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Once);
        }
    }
}