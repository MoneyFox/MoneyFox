using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
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
    public class EditPaymentViewModelTests
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IPaymentService> paymentServiceMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IDialogService> dialogServiceMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        public EditPaymentViewModelTests()
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
        }

        [Fact]
        public async Task Initialize_EndlessRecurringPayment_EndDateNotNull()
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
            await editPaymentVm.InitializeCommand.ExecuteAsync();

            // Assert
            editPaymentVm.SelectedPayment.RecurringPayment.IsEndless.ShouldBeTrue();
            editPaymentVm.SelectedPayment.RecurringPayment.EndDate.HasValue.ShouldBeTrue();
        }

        [Fact]
        public async Task Initialize_RecurringPayment_EndDateCorrect()
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
            await editPaymentVm.InitializeCommand.ExecuteAsync();

            // Assert
            editPaymentVm.SelectedPayment.RecurringPayment.IsEndless.ShouldBeFalse();
            editPaymentVm.SelectedPayment.RecurringPayment.EndDate.ShouldEqual(endDate);
        }


        [Fact]
        public async Task SavePayment_NoAccount_DialogShown()
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
            await editPaymentVm.InitializeCommand.ExecuteAsync();

            // Act
            await editPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Never);
            dialogServiceMock.Verify(x => x.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.AccountRequiredMessage), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Never);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public async Task SavePayment_ResultSucceeded_CorrectMethodCalls()
        {
            // Arrange
            const int paymentId = 99;

            paymentServiceMock.Setup(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()))
                              .Returns(Task.CompletedTask);

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
            await editPaymentVm.InitializeCommand.ExecuteAsync();
            editPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            await editPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public async Task SavePayment_ResultFailed_CorrectMethodCalls()
        {
            // Arrange
            const int paymentId = 99;

            paymentServiceMock.Setup(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()))
                              .Callback(() => throw new Exception());

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
            await editPaymentVm.InitializeCommand.ExecuteAsync();
            editPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            await Assert.ThrowsAsync<Exception>(async () =>  await editPaymentVm.SaveCommand.ExecuteAsync());

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Never);
        }

        [Fact]
        public async Task SavePayment_ResultSucceededWithBackup_CorrectMethodCalls()
        {
            // Arrange
            const int paymentId = 99;

            paymentServiceMock.Setup(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()))
                              .Returns(Task.CompletedTask);

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
            await editPaymentVm.InitializeCommand.ExecuteAsync();
            editPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            await editPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            dialogServiceMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastExecutionTimeStampSyncBackup = It.IsAny<DateTime>(), Times.Once);
            backupServiceMock.Verify(x => x.EnqueueBackupTask(0), Times.Once);
        }

        [Fact]
        public async Task SavePayment_ResultFailedWithBackup_CorrectMethodCalls()
        {
            // Arrange
            const int paymentId = 99;

            paymentServiceMock.Setup(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()))
                              .Callback(() => throw new Exception());

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
            await editPaymentVm.InitializeCommand.ExecuteAsync();
            editPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel();

            // Act
            await Assert.ThrowsAsync<Exception>(async () => await editPaymentVm.SaveCommand.ExecuteAsync());

            // Assert
            paymentServiceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentViewModel>()), Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
        }
    }
}
