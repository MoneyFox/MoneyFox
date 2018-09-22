using MoneyFox.Business.Manager;
using MoneyFox.Business.Parameters;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Interfaces;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Tests;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    [Collection("MvxIocCollection")]
    public class PaymentListViewModelTests : MvxIoCSupportingTest
    {
        private readonly Mock<IAccountService> accountService;
        private readonly Mock<IPaymentService> paymentService;
        private readonly Mock<IDialogService> dialogService;
        private readonly Mock<ISettingsManager> settingsManager;
        private readonly Mock<IBalanceCalculationManager> balanceCalculatorManager;
        private readonly Mock<IBackupManager> backupManager;
        private readonly Mock<IMvxNavigationService> navigationService;
        private readonly Mock<IMvxMessenger> messenger;
        private readonly Mock<IMvxLogProvider> logProvider;

        public PaymentListViewModelTests()
        {
            accountService = new Mock<IAccountService>();
            paymentService = new Mock<IPaymentService>();
            dialogService = new Mock<IDialogService>();
            settingsManager = new Mock<ISettingsManager>();
            balanceCalculatorManager = new Mock<IBalanceCalculationManager>();
            backupManager = new Mock<IBackupManager>();
            navigationService = new Mock<IMvxNavigationService>();
            messenger = new Mock<IMvxMessenger>();
            logProvider = new Mock<IMvxLogProvider>();

            accountService.SetupAllProperties();
            paymentService.SetupAllProperties();
        }

        [Fact]
        public async void Init_PassAccountId_AccountIdSet()
        {
            // Arrange
            accountService.Setup(x => x.GetById(It.IsAny<int>()))
                          .ReturnsAsync(new Account());
            balanceCalculatorManager.Setup(x => x.GetEndOfMonthBalanceForAccount(It.IsAny<Account>()))
                                    .ReturnsAsync(0);

            var vm = new PaymentListViewModel(accountService.Object,
                                              paymentService.Object,
                                              dialogService.Object,
                                              settingsManager.Object,
                                              balanceCalculatorManager.Object,
                                              backupManager.Object,
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
        public async void Init_NullPassAccountId_AccountIdSet()
        {
            // Arrange
            accountService.Setup(x => x.GetById(It.IsAny<int>()))
                          .ReturnsAsync(new Account());
            balanceCalculatorManager.Setup(x => x.GetEndOfMonthBalanceForAccount(It.IsAny<Account>()))
                                    .ReturnsAsync(0);

            var vm = new PaymentListViewModel(accountService.Object,
                                              paymentService.Object,
                                              dialogService.Object,
                                              settingsManager.Object,
                                              balanceCalculatorManager.Object,
                                              backupManager.Object,
                                              navigationService.Object,
                                              messenger.Object,
                                              logProvider.Object);

            // Act
            vm.Prepare(new PaymentListParameter());
            await vm.Initialize();

            // Assert
            Assert.Equal(0, vm.AccountId);
        }
    }
}