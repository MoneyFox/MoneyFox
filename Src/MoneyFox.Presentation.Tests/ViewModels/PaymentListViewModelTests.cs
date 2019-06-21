using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Services;
using Moq;
using Xunit;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class PaymentListViewModelTests
    {
        public PaymentListViewModelTests()
        {
            crudService = new Mock<ICrudServicesAsync>();
            paymentService = new Mock<IPaymentService>();
            dialogService = new Mock<IDialogService>();
            settingsFacade = new Mock<ISettingsFacade>();
            balanceCalculatorService = new Mock<IBalanceCalculationService>();
            backupService = new Mock<IBackupService>();
            navigationService = new Mock<INavigationService>();

            crudService.SetupAllProperties();
            paymentService.SetupAllProperties();
        }

        private readonly Mock<ICrudServicesAsync> crudService;
        private readonly Mock<IPaymentService> paymentService;
        private readonly Mock<IDialogService> dialogService;
        private readonly Mock<ISettingsFacade> settingsFacade;
        private readonly Mock<IBalanceCalculationService> balanceCalculatorService;
        private readonly Mock<IBackupService> backupService;
        private readonly Mock<INavigationService> navigationService;

        [Fact]
        public async Task Init_NullPassAccountId_AccountIdSet()
        {
            // Arrange
            crudService.Setup(x => x.ReadSingleAsync<AccountViewModel>(It.IsAny<int>()))
                .ReturnsAsync(new AccountViewModel());

            balanceCalculatorService.Setup(x => x.GetEndOfMonthBalanceForAccount(It.IsAny<AccountViewModel>()))
                .ReturnsAsync(0);

            var vm = new PaymentListViewModel(crudService.Object,
                paymentService.Object,
                dialogService.Object,
                settingsFacade.Object,
                balanceCalculatorService.Object,
                backupService.Object,
                navigationService.Object);

            // Act
            vm.Initialize();

            // Assert
            Assert.Equal(0, vm.AccountId);
        }

        [Fact]
        public async Task Init_PassAccountId_AccountIdSet()
        {
            // Arrange
            crudService.Setup(x => x.ReadSingleAsync<AccountViewModel>(It.IsAny<int>()))
                .ReturnsAsync(new AccountViewModel());

            balanceCalculatorService.Setup(x => x.GetEndOfMonthBalanceForAccount(It.IsAny<AccountViewModel>()))
                .ReturnsAsync(0);

            var vm = new PaymentListViewModel(crudService.Object,
                paymentService.Object,
                dialogService.Object,
                settingsFacade.Object,
                balanceCalculatorService.Object,
                backupService.Object,
                navigationService.Object);

            // Act
            vm.Initialize();

            // Assert
            Assert.Equal(42, vm.AccountId);
        }
    }
}