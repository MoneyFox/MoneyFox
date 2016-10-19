using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Exceptions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.ViewModels.Models;
using Moq;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.Repositories
{
    [TestClass]
    public class PaymentRepositoryTests : MvxIoCSupportingTest
    {
        [TestInitialize]
        public void Init()
        {
            Setup();
        }

        [TestMethod]
        [ExpectedException(typeof(AccountMissingException))]
        public void SaveWithouthAccount_NoAccount_InvalidDataException()
        {
            var paymentDataAccess = new Mock<IDataAccess<PaymentViewModel>>();
            paymentDataAccess.Setup(x => x.LoadList(null))
                .Returns(new List<PaymentViewModel>());

            var repository = new PaymentRepository(paymentDataAccess.Object);

            var payment = new PaymentViewModel
            {
                Amount = 20
            };

            repository.Save(payment);
        }

        [TestMethod]
        public void Save_IncomeType_CorrectlySaved()
        {
            PaymentViewModel savedPayment = null;

            var paymentDataAccessSetup = new Mock<IDataAccess<PaymentViewModel>>();
            paymentDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<PaymentViewModel>());
            paymentDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<PaymentViewModel>()))
                .Callback((PaymentViewModel p) => savedPayment = p);

            var repository = new PaymentRepository(paymentDataAccessSetup.Object);

            var account = new AccountViewModel
            {
                Id = 2,
                Name = "TestAccount"
            };

            var payment = new PaymentViewModel
            {
                ChargedAccount = account,
                TargetAccount = null,
                Amount = 20,
                Type = (int) PaymentType.Income
            };

            repository.Save(payment);

            savedPayment.ShouldBeSameAs(payment);
            savedPayment.Type.ShouldBe((int)PaymentType.Income);
        }

        [TestMethod]
        public void Save_ExpenseType_CorrectlySaved() {
            PaymentViewModel savedPayment = null;

            var paymentDataAccessSetup = new Mock<IDataAccess<PaymentViewModel>>();
            paymentDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<PaymentViewModel>());
            paymentDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<PaymentViewModel>()))
                .Callback((PaymentViewModel p) => savedPayment = p);

            var repository = new PaymentRepository(paymentDataAccessSetup.Object);

            var account = new AccountViewModel {
                Id = 2,
                Name = "TestAccount"
            };

            var payment = new PaymentViewModel {
                ChargedAccount = account,
                TargetAccount = null,
                Amount = 20,
                Type = (int)PaymentType.Expense
            };

            repository.Save(payment);

            savedPayment.ShouldBeSameAs(payment);
            savedPayment.Type.ShouldBe((int)PaymentType.Expense);
        }

        [TestMethod]
        public void Save_TransferType_CorrectlySaved()
        {
            PaymentViewModel savedPayment = null;

            var paymentDataAccessSetup = new Mock<IDataAccess<PaymentViewModel>>();
            paymentDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<PaymentViewModel>());
            paymentDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<PaymentViewModel>()))
                .Callback((PaymentViewModel p) => savedPayment = p);

            var repository = new PaymentRepository(paymentDataAccessSetup.Object);

            var account = new AccountViewModel {
                Id = 2,
                Name = "TestAccount"
            };

            var payment = new PaymentViewModel {
                ChargedAccount = account,
                TargetAccount = null,
                Amount = 20,
                Type = (int)PaymentType.Transfer
            };

            repository.Save(payment);

            savedPayment.ShouldBeSameAs(payment);
            savedPayment.Type.ShouldBe((int)PaymentType.Transfer);
        }

        [TestMethod]
        public void PaymentRepository_Delete()
        {
            var paymentDataAccessSetup = new Mock<IDataAccess<PaymentViewModel>>();
            paymentDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<PaymentViewModel>());

            var repository = new PaymentRepository(paymentDataAccessSetup.Object);

            var account = new AccountViewModel
            {
                Id = 2,
                Name = "TestAccount"
            };

            var payment = new PaymentViewModel
            {
                ChargedAccount = account,
                ChargedAccountId = 2,
                Amount = 20
            };

            repository.Save(payment);
            Assert.AreSame(payment, repository.GetList().ToList()[0]);

            repository.Delete(payment);

            Assert.IsFalse(repository.GetList().Any());
        }

        [TestMethod]
        public void Load_Payment_DataInitialized()
        {
            var dataAccessSetup = new Mock<IDataAccess<PaymentViewModel>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<PaymentViewModel>
            {
                new PaymentViewModel {Id = 10},
                new PaymentViewModel {Id = 15}
            });

            var paymentRepository = new PaymentRepository(dataAccessSetup.Object);
            paymentRepository.Load();

            paymentRepository.GetList(x => x.Id == 10).Any().ShouldBeTrue();
            paymentRepository.GetList(x => x.Id == 15).Any().ShouldBeTrue();
        }

        [TestMethod]
        [ExpectedException(typeof(AccountMissingException))]
        public void Save_NoChargedAccount()
        {
            var dataAccessSetup = new Mock<IDataAccess<PaymentViewModel>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<PaymentViewModel>());

            new PaymentRepository(dataAccessSetup.Object)
                .Save(new PaymentViewModel {ChargedAccountId = 0});
        }

        [TestMethod]
        public void FindById_ReturnsPayment() {
            var dataAccessMock = new Mock<IDataAccess<RecurringPaymentViewModel>>();
            var testPayment = new RecurringPaymentViewModel { Id = 100, Amount = 78 };

            dataAccessMock.Setup(x => x.LoadList(null))
                .Returns(new List<RecurringPaymentViewModel> { testPayment });

            Assert.AreEqual(testPayment, new RecurringPaymentRepository(dataAccessMock.Object).FindById(100));
        }
    }
}