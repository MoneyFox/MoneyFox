using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using GenericServices;
using MockQueryable.Moq;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Tests;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    [Collection("MvxIocCollection")]
    public class PaymentListViewModelTests : MvxIoCSupportingTest
    {
        public PaymentListViewModelTests()
        {
            crudService = new Mock<ICrudServicesAsync>();
            paymentService = new Mock<IPaymentService>();
            dialogService = new Mock<IDialogService>();
            settingsFacade = new Mock<ISettingsFacade>();
            balanceCalculatorService = new Mock<IBalanceCalculationService>();
            backupService = new Mock<IBackupService>();
            navigationService = new Mock<IMvxNavigationService>();
            messenger = new Mock<IMvxMessenger>();
            logProvider = new Mock<IMvxLogProvider>();

            crudService.SetupAllProperties();
            paymentService.SetupAllProperties();
        }

        private readonly Mock<ICrudServicesAsync> crudService;
        private readonly Mock<IPaymentService> paymentService;
        private readonly Mock<IDialogService> dialogService;
        private readonly Mock<ISettingsFacade> settingsFacade;
        private readonly Mock<IBalanceCalculationService> balanceCalculatorService;
        private readonly Mock<IBackupService> backupService;
        private readonly Mock<IMvxNavigationService> navigationService;
        private readonly Mock<IMvxMessenger> messenger;
        private readonly Mock<IMvxLogProvider> logProvider;

        [Fact]
        public async Task Init_NullPassAccountId_AccountIdSet()
        {
            // Arrange
            crudService.Setup(x => x.ReadSingleAsync<AccountViewModel>(It.IsAny<int>()))
                .ReturnsAsync(new AccountViewModel());

            balanceCalculatorService.Setup(x => x.GetEndOfMonthBalanceForAccount(It.IsAny<AccountViewModel>()))
                .Returns(0);

            var vm = new PaymentListViewModel(crudService.Object,
                paymentService.Object,
                dialogService.Object,
                settingsFacade.Object,
                balanceCalculatorService.Object,
                backupService.Object,
                navigationService.Object,
                messenger.Object,
                logProvider.Object);

            // Act
            vm.Prepare(new PaymentListParameter());
            await vm.Initialize();

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
                .Returns(0);

            var vm = new PaymentListViewModel(crudService.Object,
                paymentService.Object,
                dialogService.Object,
                settingsFacade.Object,
                balanceCalculatorService.Object,
                backupService.Object,
                navigationService.Object,
                messenger.Object,
                logProvider.Object);

            // Act
            vm.Prepare(new PaymentListParameter(42));
            await vm.Initialize();

            // Assert
            Assert.Equal(42, vm.AccountId);
        }

        [Fact]
        public void ViewAppearing_DialogShown()
        {
            // Arrange
            dialogService.Setup(x => x.ShowLoadingDialog(It.IsAny<string>()));
            dialogService.Setup(x => x.HideLoadingDialog());

            crudService.Setup(x => x.ReadManyNoTracked<AccountViewModel>())
                .Returns(new List<AccountViewModel>()
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var vm = new PaymentListViewModel(crudService.Object,
                paymentService.Object,
                dialogService.Object,
                settingsFacade.Object,
                balanceCalculatorService.Object,
                backupService.Object,
                navigationService.Object,
                messenger.Object,
                logProvider.Object);

            // Act
            vm.ViewAppearing();

            // Assert
            dialogService.Verify(x => x.ShowLoadingDialog(It.IsAny<string>()), Times.Once);
            dialogService.Verify(x => x.HideLoadingDialog(), Times.Once);
        }
    }
}