using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
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
        [Fact]
        public void Prepare_PaymentCreated()
        {
            // Arrange
            var crudServiceMock = new Mock<ICrudServicesAsync>();

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
            var crudServiceMock = new Mock<ICrudServicesAsync>();

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
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                .ReturnsAsync(OperationResult.Succeeded());

            var crudServiceMock = new Mock<ICrudServicesAsync>();
            var settingsFacadeMock = new Mock<ISettingsFacade>();
            var backupServiceMock = new Mock<IBackupService>();
            var dialogServiceMock = new Mock<IDialogService>();
            var navigationServiceMock = new Mock<IMvxNavigationService>();

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
        }

        [Fact]
        public void SavePayment_ResultSucceeded_CorrectMethodCalls()
        {
            // Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                .ReturnsAsync(OperationResult.Succeeded());

            var crudServiceMock = new Mock<ICrudServicesAsync>();
            var settingsFacadeMock = new Mock<ISettingsFacade>();
            var backupServiceMock = new Mock<IBackupService>();
            var dialogServiceMock = new Mock<IDialogService>();
            var navigationServiceMock = new Mock<IMvxNavigationService>();

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
        }

        [Fact]
        public void SavePayment_ResultFailed_CorrectMethodCalls()
        {
            // Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.SavePayment(It.IsAny<PaymentViewModel>()))
                .ReturnsAsync(OperationResult.Failed(""));

            var crudServiceMock = new Mock<ICrudServicesAsync>();
            var settingsFacadeMock = new Mock<ISettingsFacade>();
            var backupServiceMock = new Mock<IBackupService>();
            var dialogServiceMock = new Mock<IDialogService>();
            var navigationServiceMock = new Mock<IMvxNavigationService>();

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
        }
    }
}