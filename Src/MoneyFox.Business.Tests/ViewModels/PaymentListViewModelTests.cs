using MoneyFox.Business.Manager;
using MoneyFox.Business.Parameters;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Service.DataServices;
using MoneyFox.Service.Pocos;
using Moq;
using MvvmCross.Core.Navigation;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Test.Core;
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
        private readonly Mock<IModifyDialogService> modifyDialogService;
        private readonly Mock<IMvxNavigationService> navigationService;
        private readonly Mock<IMvxMessenger> messenger;

        public PaymentListViewModelTests()
        {
            accountService = new Mock<IAccountService>();
            paymentService = new Mock<IPaymentService>();
            dialogService = new Mock<IDialogService>();
            settingsManager = new Mock<ISettingsManager>();
            balanceCalculatorManager = new Mock<IBalanceCalculationManager>();
            backupManager = new Mock<IBackupManager>();
            modifyDialogService = new Mock<IModifyDialogService>();
            navigationService = new Mock<IMvxNavigationService>();
            messenger = new Mock<IMvxMessenger>();

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
                                              modifyDialogService.Object,
                                              navigationService.Object,
                                              messenger.Object);

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
                                              modifyDialogService.Object,
                                              navigationService.Object,
                                              messenger.Object);

            // Act
            vm.Prepare(new PaymentListParameter());
            await vm.Initialize();

            // Assert
            Assert.Equal(0, vm.AccountId);
        }
    }
}