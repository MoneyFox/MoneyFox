using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using Moq;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [TestClass]
    public class PaymentListViewModelTests : MvxIoCSupportingTest
    {
        private Mock<IAccountRepository> accountRepository;
        private Mock<IPaymentRepository> paymentRepository;
        private Mock<IPaymentManager> paymentManager;
        private Mock<ISettingsManager> settingsManager;
        private Mock<IEndOfMonthManager> endOfMonthManager;
        private Mock<IBackupManager> backupManager;
        private Mock<IModifyDialogService> modifyDialogService;

        [TestInitialize]
        public void Init()
        {
            MvxSingleton.ClearAllSingletons();
            accountRepository = new Mock<IAccountRepository>();
            paymentRepository = new Mock<IPaymentRepository>();
            paymentManager = new Mock<IPaymentManager>();
            settingsManager = new Mock<ISettingsManager>();
            endOfMonthManager = new Mock<IEndOfMonthManager>();
            backupManager = new Mock<IBackupManager>();
            modifyDialogService = new Mock<IModifyDialogService>();

            accountRepository.SetupAllProperties();
            paymentRepository.SetupAllProperties();
            Setup();
        }

        [TestMethod]
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

        [TestMethod]
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
