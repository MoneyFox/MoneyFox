using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;
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
        private Mock<IRepository<RecurringPaymentViewModel>> recPaymentRepository;
        private Mock<IPaymentManager> paymentManager;
        private Mock<ISettingsManager> settingsManager;
        private Mock<IEndOfMonthManager> endOfMonthManager;

        [TestInitialize]
        public void Init()
        {
            MvxSingleton.ClearAllSingletons();
            accountRepository = new Mock<IAccountRepository>();
            paymentRepository = new Mock<IPaymentRepository>();
            recPaymentRepository = new Mock<IRepository<RecurringPaymentViewModel>>();
            paymentManager = new Mock<IPaymentManager>();
            settingsManager = new Mock<ISettingsManager>();
            endOfMonthManager = new Mock<IEndOfMonthManager>();

            accountRepository.SetupAllProperties();
            paymentRepository.SetupAllProperties();
            Setup();
        }

        [TestMethod]
        public void Init_PassAccountId_AccountIdSet()
        {
            var vm = new PaymentListViewModel(accountRepository.Object,
                paymentRepository.Object,
                recPaymentRepository.Object,
                paymentManager.Object, 
                null,
                settingsManager.Object,
                endOfMonthManager.Object);

            vm.Init(42);
            vm.AccountId.ShouldBe(42);
        }

        [TestMethod]
        public void Init_NullPassAccountId_AccountIdSet()
        {
            var vm = new PaymentListViewModel(accountRepository.Object,
                paymentRepository.Object,
                recPaymentRepository.Object,
                paymentManager.Object, 
                null,
                settingsManager.Object,
                endOfMonthManager.Object);

            vm.Init(0);
            vm.AccountId.ShouldBe(0);
        }
    }
}
