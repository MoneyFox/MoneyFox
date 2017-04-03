using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Tests;
using Moq;
using MvvmCross.Test.Core;
using Xunit;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [Collection("MvxIocCollection")]
    public class PaymentListViewModelTests : MvxIoCSupportingTest
    {
        private Mock<IAccountRepository> accountRepository;
        private Mock<IPaymentRepository> paymentRepository;
        private Mock<IPaymentManager> paymentManager;
        private Mock<ISettingsManager> settingsManager;
        private Mock<IEndOfMonthManager> endOfMonthManager;
        private Mock<IBackupManager> backupManager;
        private Mock<IModifyDialogService> modifyDialogService;

        public PaymentListViewModelTests()
        {
            accountRepository = new Mock<IAccountRepository>();
            paymentRepository = new Mock<IPaymentRepository>();
            paymentManager = new Mock<IPaymentManager>();
            settingsManager = new Mock<ISettingsManager>();
            endOfMonthManager = new Mock<IEndOfMonthManager>();
            backupManager = new Mock<IBackupManager>();
            modifyDialogService = new Mock<IModifyDialogService>();

            accountRepository.SetupAllProperties();
            paymentRepository.SetupAllProperties();
        }

        [Fact]
        public void Init_PassAccountId_AccountIdSet()
        {
            var vm = new PaymentListViewModel(accountRepository.Object,
                paymentRepository.Object,
                paymentManager.Object, 
                null,
                settingsManager.Object,
                endOfMonthManager.Object,
                backupManager.Object,
                modifyDialogService.Object);

            vm.Init(42);
            vm.AccountId.ShouldBe(42);
        }

        [Fact]
        public void Init_NullPassAccountId_AccountIdSet()
        {
            var vm = new PaymentListViewModel(accountRepository.Object,
                paymentRepository.Object,
                paymentManager.Object, 
                null,
                settingsManager.Object,
                endOfMonthManager.Object,
                backupManager.Object,
                modifyDialogService.Object);

            vm.Init(0);
            vm.AccountId.ShouldBe(0);
        }
    }
}
