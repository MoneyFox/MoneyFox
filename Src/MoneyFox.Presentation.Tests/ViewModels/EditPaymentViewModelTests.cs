using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.BusinessLogic;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Parameters;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.Facades;
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
using IDialogService = MoneyFox.ServiceLayer.Interfaces.IDialogService;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class EditPaymentViewModelTests
    {
        private readonly Mock<IPaymentService> paymentServiceMock;
        private readonly Mock<ICrudServicesAsync> crudServiceMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IDialogService> dialogServiceMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        public EditPaymentViewModelTests()
        {
            paymentServiceMock = new Mock<IPaymentService>();
            crudServiceMock = new Mock<ICrudServicesAsync>();
            settingsFacadeMock = new Mock<ISettingsFacade>();
            backupServiceMock = new Mock<IBackupService>();
            dialogServiceMock = new Mock<IDialogService>();
            navigationServiceMock = new Mock<INavigationService>();
        }

        [Fact]
        public void Initialize_EndlessRecurringPayment_EndDateNotNull()
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

            var editPaymentVm = new EditPaymentViewModel(null, crudServiceMock.Object, null, null, null, null);

            // Act
            editPaymentVm.PaymentId = paymentId;
            editPaymentVm.InitializeCommand.Execute(null);

            // Assert
            editPaymentVm.SelectedPayment.RecurringPayment.IsEndless.ShouldBeTrue();
            editPaymentVm.SelectedPayment.RecurringPayment.EndDate.HasValue.ShouldBeTrue();
        }

        [Fact]
        public void Initialize_RecurringPayment_EndDateCorrect()
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

            var editPaymentVm = new EditPaymentViewModel(null, crudServiceMock.Object, null, null, null, null);

            // Act
            editPaymentVm.PaymentId = paymentId;
            editPaymentVm.InitializeCommand.Execute(null);

            // Assert
            editPaymentVm.SelectedPayment.RecurringPayment.IsEndless.ShouldBeFalse();
            editPaymentVm.SelectedPayment.RecurringPayment.EndDate.ShouldEqual(endDate);
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
                                                       backupServiceMock.Object,
                                                       navigationServiceMock.Object);

            editPaymentVm.PaymentId = paymentId;
            editPaymentVm.InitializeCommand.Execute(null);

            // Act
            editPaymentVm.SaveCommand.Execute(null);

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Never);
            dialogServiceMock.Verify(x => x.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.AccountRequiredMessage), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
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
                backupServiceMock.Object,
                navigationServiceMock.Object);

            editPaymentVm.PaymentId = paymentId;
            editPaymentVm.InitializeCommand.Execute(null);
            editPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            editPaymentVm.SaveCommand.Execute(null);

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Once);
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
                backupServiceMock.Object,
                navigationServiceMock.Object);

            editPaymentVm.PaymentId = paymentId;
            editPaymentVm.InitializeCommand.Execute(null);
            editPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            editPaymentVm.SaveCommand.Execute(null);

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
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
                backupServiceMock.Object,
                navigationServiceMock.Object);

            editPaymentVm.PaymentId = paymentId;
            editPaymentVm.InitializeCommand.Execute(null);
            editPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            editPaymentVm.SaveCommand.Execute(null);

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Once);
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
                backupServiceMock.Object,
                navigationServiceMock.Object);

            editPaymentVm.PaymentId = paymentId;
            editPaymentVm.InitializeCommand.Execute(null);
            editPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            editPaymentVm.SaveCommand.Execute(null);

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Once);
        }
    }
}
