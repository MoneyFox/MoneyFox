using System.Diagnostics.CodeAnalysis;
using System.Threading;
using GenericServices;
using MoneyFox.Foundation;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AccountListViewActionModelTests
    {
        [Fact]
        public void GoToAddPayment_IncomeNoEdit_CorrectParameterPassed()
        {
            // Arrange
            ModifyPaymentParameter parameter = null;

            var navigationService = new Mock<IMvxNavigationService>();
            navigationService
                .Setup(x => x.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(It.IsAny<ModifyPaymentParameter>(), null, CancellationToken.None))
                .Callback((ModifyPaymentParameter param, IMvxBundle bundle, CancellationToken t) => parameter = param)
                .ReturnsAsync(true);
            
            // Act
            new AccountListViewActionViewModel(new Mock<ICrudServicesAsync>().Object, new Mock<IMvxLogProvider>().Object, navigationService.Object)
                .GoToAddIncomeCommand.Execute();

            // Assert
            Assert.NotNull(parameter);
            Assert.Equal(PaymentType.Income, parameter.PaymentType);
            Assert.Equal(0, parameter.PaymentId);
        }

        [Fact]
        public void GoToAddPayment_ExpenseNoEdit_CorrectParameterPassed()
        {
            // Arrange
            ModifyPaymentParameter parameter = null;

            var navigationService = new Mock<IMvxNavigationService>();
            navigationService
                .Setup(x => x.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(It.IsAny<ModifyPaymentParameter>(), null, CancellationToken.None))
                .Callback((ModifyPaymentParameter param, IMvxBundle bundle, CancellationToken t) => parameter = param)
                .ReturnsAsync(true);
            
            // Act
            new AccountListViewActionViewModel(new Mock<ICrudServicesAsync>().Object, new Mock<IMvxLogProvider>().Object, navigationService.Object)
                .GoToAddExpenseCommand.Execute();

            // Assert
            Assert.NotNull(parameter);
            Assert.Equal(PaymentType.Expense, parameter.PaymentType);
            Assert.Equal(0, parameter.PaymentId);
        }

        [Fact]
        public void GoToAddPayment_TransferNoEdit_CorrectParameterPassed()
        {
            // Arrange
            ModifyPaymentParameter parameter = null;

            var navigationService = new Mock<IMvxNavigationService>();
            navigationService
                .Setup(x => x.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(
                           It.IsAny<ModifyPaymentParameter>(), null, CancellationToken.None))
                .Callback((ModifyPaymentParameter param, IMvxBundle bundle, CancellationToken t) => parameter = param)
                .ReturnsAsync(true);
            
            // Act
            new AccountListViewActionViewModel(new Mock<ICrudServicesAsync>().Object, new Mock<IMvxLogProvider>().Object, navigationService.Object)
                .GoToAddTransferCommand.Execute();

            // Assert
            Assert.NotNull(parameter);
            Assert.Equal(PaymentType.Transfer, parameter.PaymentType);
            Assert.Equal(0, parameter.PaymentId);
        }

        [Fact]
        public void GoToAddAccount_NoEdit_CorrectParameterPassed()
        {
            // Arrange
            ModifyPaymentParameter parameter = null;

            var navigationService = new Mock<IMvxNavigationService>();
            navigationService
                .Setup(x => x.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(
                           It.IsAny<ModifyPaymentParameter>(), null, CancellationToken.None))
                .Callback((ModifyPaymentParameter param, IMvxBundle bundle, CancellationToken t) => parameter = param)
                .ReturnsAsync(true);
            
            // Act
            new AccountListViewActionViewModel(new Mock<ICrudServicesAsync>().Object, new Mock<IMvxLogProvider>().Object, navigationService.Object)
                .GoToAddTransferCommand.Execute();

            // Assert
            Assert.NotNull(parameter);
            Assert.Equal(PaymentType.Transfer, parameter.PaymentType);
            Assert.Equal(0, parameter.PaymentId);
        }
    }
}