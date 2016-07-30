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
    }
}
