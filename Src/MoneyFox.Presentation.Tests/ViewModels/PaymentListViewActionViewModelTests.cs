using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Foundation;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Presentation.ViewModels.Interfaces;
using Moq;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.Domain;
using Xunit;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class PaymentListViewActionViewModelTests
    {
        private readonly Mock<ICrudServicesAsync> crudServicesMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IDialogService> dialogServiceMock;
        private readonly Mock<IBalanceViewModel> balanceViewModelMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        public PaymentListViewActionViewModelTests()
        {
            crudServicesMock = new Mock<ICrudServicesAsync>();
            settingsFacadeMock = new Mock<ISettingsFacade>();
            dialogServiceMock = new Mock<IDialogService>();
            balanceViewModelMock = new Mock<IBalanceViewModel>();
            navigationServiceMock = new Mock<INavigationService>();
        }

        [Fact]
        public void GoToAddIncomeCommand_CorrectParametersPassed()
        {
            // Arrange
            var vm = new PaymentListViewActionViewModel(12,
                crudServicesMock.Object,
                settingsFacadeMock.Object,
                dialogServiceMock.Object,
                balanceViewModelMock.Object,
                navigationServiceMock.Object);

            // Act
            vm.GoToAddIncomeCommand.Execute(null);

            // Assert
            navigationServiceMock.Verify(x => x.NavigateTo(ViewModelLocator.AddPayment, PaymentType.Income), Times.Once);
        }

        [Fact]
        public void GoToAddExpenseCommand_CorrectParametersPassed()
        {
            // Arrange
            var vm = new PaymentListViewActionViewModel(12,
                crudServicesMock.Object,
                settingsFacadeMock.Object,
                dialogServiceMock.Object,
                balanceViewModelMock.Object,
                navigationServiceMock.Object);

            // Act
            vm.GoToAddExpenseCommand.Execute(null);

            // Assert
            navigationServiceMock.Verify(x => x.NavigateTo(ViewModelLocator.AddPayment, PaymentType.Expense), Times.Once);
        }

        [Fact]
        public void GoToAddTransferCommand_CorrectParametersPassed()
        {
            // Arrange
            var vm = new PaymentListViewActionViewModel(12,
                crudServicesMock.Object,
                settingsFacadeMock.Object,
                dialogServiceMock.Object,
                balanceViewModelMock.Object,
                navigationServiceMock.Object);

            // Act
            vm.GoToAddTransferCommand.Execute(null);

            // Assert
            navigationServiceMock.Verify(x => x.NavigateTo(ViewModelLocator.AddPayment, PaymentType.Transfer), Times.Once);
        }
    }
}
