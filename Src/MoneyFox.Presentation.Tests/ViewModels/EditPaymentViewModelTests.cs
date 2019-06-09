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
    public class EditPaymentViewModelTests
    {
        private readonly Mock<IPaymentService> paymentServiceMock;
        private readonly Mock<ICrudServicesAsync> crudServiceMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IDialogService> dialogServiceMock;
        private readonly Mock<IMvxNavigationService> navigationServiceMock;

        public EditPaymentViewModelTests()
        {
            paymentServiceMock = new Mock<IPaymentService>();
            crudServiceMock = new Mock<ICrudServicesAsync>();
            settingsFacadeMock = new Mock<ISettingsFacade>();
            backupServiceMock = new Mock<IBackupService>();
            dialogServiceMock = new Mock<IDialogService>();
            navigationServiceMock = new Mock<IMvxNavigationService>();
        }

        [Fact]
        public void Prepare_EndlessRecurringPayment_EndDateNotNull()
        {
            // Arrange
            const int paymentId = 99;

            crudServiceMock.Setup(x => x.ReadSingleAsync<PaymentViewModel>(It.IsAny<int>()))
                           .ReturnsAsync(new PaymentViewModel
                           {
                               IsRecurring = true,
                               RecurringPayment = new RecurringPaymentViewModel
                               {
                                   IsEndless = true, EndDate = null
                               }
                           });

            var editAccountVm = new EditPaymentViewModel(null, crudServiceMock.Object, null, null, new Mock<IMvxMessenger>().Object, null, null, null);

            // Act
            editAccountVm.Prepare(new ModifyPaymentParameter(paymentId));

            // Assert
            editAccountVm.SelectedPayment.RecurringPayment.IsEndless.ShouldBeTrue();
            editAccountVm.SelectedPayment.RecurringPayment.EndDate.HasValue.ShouldBeTrue();
        }

        [Fact]
        public void Prepare_RecurringPayment_EndDateCorrect()
        {
            // Arrange
            const int paymentId = 99;
            var endDate = DateTime.Today.AddDays(-7);
            crudServiceMock.Setup(x => x.ReadSingleAsync<PaymentViewModel>(It.IsAny<int>()))
                           .ReturnsAsync(new PaymentViewModel
                           {
                               IsRecurring = true,
                               RecurringPayment = new RecurringPaymentViewModel
                               {
                                   IsEndless = false, EndDate = endDate
                               }
                           });

            var editAccountVm = new EditPaymentViewModel(null, crudServiceMock.Object, null, null, new Mock<IMvxMessenger>().Object, null, null, null);

            // Act
            editAccountVm.Prepare(new ModifyPaymentParameter(paymentId));

            // Assert
            editAccountVm.SelectedPayment.RecurringPayment.IsEndless.ShouldBeFalse();
            editAccountVm.SelectedPayment.RecurringPayment.EndDate.ShouldEqual(endDate);
        }


        [Fact]
        public void SavePayment_NoAccount_DialogShown()
        {
            // Arrange
            const int paymentId = 99;

            crudServiceMock.Setup(x => x.ReadSingleAsync<PaymentViewModel>(It.IsAny<int>()))
                .ReturnsAsync(new PaymentViewModel
                {
                    IsRecurring = true,
                    RecurringPayment = new RecurringPaymentViewModel
                    {
                        IsEndless = true,
                        EndDate = null
                    }
                });

            var editPaymentVm = new EditPaymentViewModel(paymentServiceMock.Object,
                                                       crudServiceMock.Object,
                                                       dialogServiceMock.Object,
                                                       settingsFacadeMock.Object,
                                                       new Mock<IMvxMessenger>().Object,
                                                       backupServiceMock.Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       navigationServiceMock.Object);

            editPaymentVm.Prepare(new ModifyPaymentParameter(paymentId));

            // Act
            editPaymentVm.SaveCommand.Execute();

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Never);
            dialogServiceMock.Verify(x => x.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.AccountRequiredMessage), Times.Once);
            navigationServiceMock.Verify(x => x.Close(It.IsAny<MvxViewModel>(), CancellationToken.None), Times.Never);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Never);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public void SavePayment_ResultSucceeded_CorrectMethodCalls()
        {
            // Arrange
            const int paymentId = 99;

            paymentServiceMock.Setup(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()))
                .ReturnsAsync(OperationResult.Succeeded());

            crudServiceMock.Setup(x => x.ReadSingleAsync<PaymentViewModel>(It.IsAny<int>()))
                .ReturnsAsync(new PaymentViewModel
                {
                    IsRecurring = true,
                    RecurringPayment = new RecurringPaymentViewModel
                    {
                        IsEndless = true,
                        EndDate = null
                    }
                });

            var editPaymentVm = new EditPaymentViewModel(paymentServiceMock.Object,
                crudServiceMock.Object,
                dialogServiceMock.Object,
                settingsFacadeMock.Object,
                new Mock<IMvxMessenger>().Object,
                backupServiceMock.Object,
                new Mock<IMvxLogProvider>().Object,
                navigationServiceMock.Object);

            editPaymentVm.Prepare(new ModifyPaymentParameter(paymentId));
            editPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            editPaymentVm.SaveCommand.Execute();

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationServiceMock.Verify(x => x.Close(It.IsAny<MvxViewModel>(), CancellationToken.None), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public void SavePayment_ResultFailed_CorrectMethodCalls()
        {
            // Arrange
            const int paymentId = 99;

            paymentServiceMock.Setup(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()))
                .ReturnsAsync(OperationResult.Failed(""));

            crudServiceMock.Setup(x => x.ReadSingleAsync<PaymentViewModel>(It.IsAny<int>()))
                .ReturnsAsync(new PaymentViewModel
                {
                    IsRecurring = true,
                    RecurringPayment = new RecurringPaymentViewModel
                    {
                        IsEndless = true,
                        EndDate = null
                    }
                });

            var editPaymentVm = new EditPaymentViewModel(paymentServiceMock.Object,
                crudServiceMock.Object,
                dialogServiceMock.Object,
                settingsFacadeMock.Object,
                new Mock<IMvxMessenger>().Object,
                backupServiceMock.Object,
                new Mock<IMvxLogProvider>().Object,
                navigationServiceMock.Object);

            editPaymentVm.Prepare(new ModifyPaymentParameter(paymentId));
            editPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            editPaymentVm.SaveCommand.Execute();

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            navigationServiceMock.Verify(x => x.Close(It.IsAny<MvxViewModel>(), CancellationToken.None), Times.Never);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public void SavePayment_ResultSucceededWithBackup_CorrectMethodCalls()
        {
            // Arrange
            const int paymentId = 99;

            paymentServiceMock.Setup(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()))
                .ReturnsAsync(OperationResult.Succeeded());

            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);

            crudServiceMock.Setup(x => x.ReadSingleAsync<PaymentViewModel>(It.IsAny<int>()))
                .ReturnsAsync(new PaymentViewModel
                {
                    IsRecurring = true,
                    RecurringPayment = new RecurringPaymentViewModel
                    {
                        IsEndless = true,
                        EndDate = null
                    }
                });

            var editPaymentVm = new EditPaymentViewModel(paymentServiceMock.Object,
                crudServiceMock.Object,
                dialogServiceMock.Object,
                settingsFacadeMock.Object,
                new Mock<IMvxMessenger>().Object,
                backupServiceMock.Object,
                new Mock<IMvxLogProvider>().Object,
                navigationServiceMock.Object);

            editPaymentVm.Prepare(new ModifyPaymentParameter(paymentId));
            editPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            editPaymentVm.SaveCommand.Execute();

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationServiceMock.Verify(x => x.Close(It.IsAny<MvxViewModel>(), CancellationToken.None), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Once);
        }

        [Fact]
        public void SavePayment_ResultFailedWithBackup_CorrectMethodCalls()
        {
            // Arrange
            const int paymentId = 99;

            paymentServiceMock.Setup(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()))
                .ReturnsAsync(OperationResult.Failed(""));

            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);

            crudServiceMock.Setup(x => x.ReadSingleAsync<PaymentViewModel>(It.IsAny<int>()))
                .ReturnsAsync(new PaymentViewModel
                {
                    IsRecurring = true,
                    RecurringPayment = new RecurringPaymentViewModel
                    {
                        IsEndless = true,
                        EndDate = null
                    }
                });

            var editPaymentVm = new EditPaymentViewModel(paymentServiceMock.Object,
                crudServiceMock.Object,
                dialogServiceMock.Object,
                settingsFacadeMock.Object,
                new Mock<IMvxMessenger>().Object,
                backupServiceMock.Object,
                new Mock<IMvxLogProvider>().Object,
                navigationServiceMock.Object);

            editPaymentVm.Prepare(new ModifyPaymentParameter(paymentId));
            editPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            editPaymentVm.SaveCommand.Execute();

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            navigationServiceMock.Verify(x => x.Close(It.IsAny<MvxViewModel>(), CancellationToken.None), Times.Never);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Once);
        }
    }
}
