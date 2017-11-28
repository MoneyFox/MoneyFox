using System.Threading.Tasks;
using MoneyFox.Business.Parameters;
using MoneyFox.Business.Tests.Fixtures;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation;
using Moq;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    [Collection("MvxIocCollection")]
    public class AccountListViewActionModelTests : MvxIocFixture
    {
        [Fact]
        public void GoToAddPayment_IncomeNoEdit_CorrectParameterPassed()
        {
            // Arrange
            ModifyPaymentParameter parameter = null;

            var navigationService = new Mock<IMvxNavigationService>();
            navigationService
                .Setup(x => x.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(It.IsAny<ModifyPaymentParameter>(), null))
                .Callback((ModifyPaymentParameter param, IMvxBundle bundle) => parameter = param)
                .Returns(Task.CompletedTask);
            
            // Act
            new AccountListViewActionViewModel(new Mock<IAccountService>().Object, navigationService.Object)
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
                .Setup(x => x.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(It.IsAny<ModifyPaymentParameter>(), null))
                .Callback((ModifyPaymentParameter param, IMvxBundle bundle) => parameter = param)
                .Returns(Task.CompletedTask);
            
            // Act
            new AccountListViewActionViewModel(new Mock<IAccountService>().Object, navigationService.Object)
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
                .Setup(x => x.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(It.IsAny<ModifyPaymentParameter>(), null))
                .Callback((ModifyPaymentParameter param, IMvxBundle bundle) => parameter = param)
                .Returns(Task.CompletedTask);
            
            // Act
            new AccountListViewActionViewModel(new Mock<IAccountService>().Object, navigationService.Object)
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
                .Setup(x => x.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(It.IsAny<ModifyPaymentParameter>(), null))
                .Callback((ModifyPaymentParameter param, IMvxBundle bundle) => parameter = param)
                .Returns(Task.CompletedTask);
            
            // Act
            new AccountListViewActionViewModel(new Mock<IAccountService>().Object, navigationService.Object)
                .GoToAddTransferCommand.Execute();

            // Assert
            Assert.NotNull(parameter);
            Assert.Equal(PaymentType.Transfer, parameter.PaymentType);
            Assert.Equal(0, parameter.PaymentId);
        }

        [Fact]
        public void IsAddIncomeEnabled_EmptyData_NotAvailable()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(0);

            new AccountListViewActionViewModel(accountServiceMock.Object, new Mock<IMvxNavigationService>().Object).IsAddIncomeAvailable.ShouldBeFalse();
        }

        [Fact]
        public void IsAddIncomeEnabled_OneAccountInData_Available()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(1);

            new AccountListViewActionViewModel(accountServiceMock.Object, new Mock<IMvxNavigationService>().Object).IsAddIncomeAvailable.ShouldBeTrue();
        }

        [Fact]
        public void IsAddExpenseEnabled_EmptyData_NotAvailable()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(0);

            new AccountListViewActionViewModel(accountServiceMock.Object, new Mock<IMvxNavigationService>().Object).IsAddExpenseAvailable.ShouldBeFalse();
        }

        [Fact]
        public void IsAddExpenseEnabled_OneAccountInData_Available()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(1);

            new AccountListViewActionViewModel(accountServiceMock.Object, new Mock<IMvxNavigationService>().Object).IsAddExpenseAvailable.ShouldBeTrue();
        }

        [Fact]
        public void IsTransferAvailable_EmptyData_NotAvailable()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(0);

            new AccountListViewActionViewModel(accountServiceMock.Object, new Mock<IMvxNavigationService>().Object).IsTransferAvailable.ShouldBeFalse();
        }

        [Fact]
        public void IsTransferAvailable_OneAccountInData_NotAvailable()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(1);

            new AccountListViewActionViewModel(accountServiceMock.Object, new Mock<IMvxNavigationService>().Object).IsTransferAvailable.ShouldBeFalse();
        }

        [Fact]
        public void IsTransferAvailable_TwoAccountInData_Available()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(2);

            new AccountListViewActionViewModel(accountServiceMock.Object, new Mock<IMvxNavigationService>().Object).IsTransferAvailable.ShouldBeTrue();
        }
    }
}