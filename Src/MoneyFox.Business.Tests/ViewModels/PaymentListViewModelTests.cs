using MoneyFox.Business.Manager;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Service.DataServices;
using MoneyFox.Service.Pocos;
using Moq;
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

        public PaymentListViewModelTests()
        {
            accountService = new Mock<IAccountService>();
            paymentService = new Mock<IPaymentService>();
            dialogService = new Mock<IDialogService>();
            settingsManager = new Mock<ISettingsManager>();
            balanceCalculatorManager = new Mock<IBalanceCalculationManager>();
            backupManager = new Mock<IBackupManager>();
            modifyDialogService = new Mock<IModifyDialogService>();

            accountService.SetupAllProperties();
            paymentService.SetupAllProperties();
        }

        [Fact]
        public void Init_PassAccountId_AccountIdSet()
        {
            // Arrange
            var vm = new PaymentListViewModel(accountService.Object,
                                              paymentService.Object,
                                              dialogService.Object,
                                              settingsManager.Object,
                                              balanceCalculatorManager.Object,
                                              backupManager.Object,
                                              modifyDialogService.Object);

            // Act
            vm.Init(42);

            // Assert
            Assert.Equal(42, vm.AccountId);
        }

        [Fact]
        public void Init_NullPassAccountId_AccountIdSet()
        {
            // Arrange
            var vm = new PaymentListViewModel(accountService.Object,
                                              paymentService.Object,
                                              dialogService.Object,
                                              settingsManager.Object,
                                              balanceCalculatorManager.Object,
                                              backupManager.Object,
                                              modifyDialogService.Object);

            // Act
            vm.Init(0);

            // Assert
            Assert.Equal(0, vm.AccountId);
        }
    }
}