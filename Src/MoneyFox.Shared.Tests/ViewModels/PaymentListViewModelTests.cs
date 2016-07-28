using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
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
        private Mock<IRepository<Account>> accountRepository;
        private Mock<IPaymentRepository> paymentRepository;
        private Mock<IRepository<RecurringPayment>> recPaymentRepository;
        private Mock<IPaymentManager> paymentManager;

        [TestInitialize]
        public void Init()
        {
            MvxSingleton.ClearAllSingletons();
            accountRepository = new Mock<IRepository<Account>>();
            paymentRepository = new Mock<IPaymentRepository>();
            recPaymentRepository = new Mock<IRepository<RecurringPayment>>();
            paymentManager = new Mock<IPaymentManager>();

            accountRepository.SetupAllProperties();
            paymentRepository.SetupAllProperties();
            Setup();
        }

        [TestMethod]
        public void RelatedPayments_PaymentsAvailable_MatchesRepository()
        {
            paymentRepository.SetupGet(x => x.Data).Returns(new ObservableCollection<Payment> {
                new Payment(),
                new Payment()
            });
            new PaymentListViewModel(accountRepository.Object,
                paymentRepository.Object,
                recPaymentRepository.Object, 
                paymentManager.Object,
                null).RelatedPayments.Count.ShouldBe(paymentRepository.Object.Data.Count);
        }

        [TestMethod]
        public void RelatedPayments_NoPaymentsAvailable_MatchesRepository()
        {
            paymentRepository.SetupGet(x => x.Data).Returns(new ObservableCollection<Payment>());
            new PaymentListViewModel(accountRepository.Object,
                paymentRepository.Object,
                recPaymentRepository.Object,
                paymentManager.Object,
                null).RelatedPayments.Count.ShouldBe(paymentRepository.Object.Data.Count);
        }
    }
}
