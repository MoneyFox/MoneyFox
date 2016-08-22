using System;
using System.Collections.ObjectModel;
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
        private Mock<IRepository<RecurringPayment>> recPaymentRepository;
        private Mock<IPaymentManager> paymentManager;

        [TestInitialize]
        public void Init()
        {
            MvxSingleton.ClearAllSingletons();
            accountRepository = new Mock<IAccountRepository>();
            paymentRepository = new Mock<IPaymentRepository>();
            recPaymentRepository = new Mock<IRepository<RecurringPayment>>();
            paymentManager = new Mock<IPaymentManager>();

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
                null);

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
                null);

            vm.Init(0);
            vm.AccountId.ShouldBe(0);
        }
    }
}
