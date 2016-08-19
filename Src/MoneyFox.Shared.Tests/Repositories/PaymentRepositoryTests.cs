using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Exceptions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using Moq;
using MvvmCross.Platform;
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

            // We setup the static setting classes here for the general usage in the app
            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.SetupAllProperties();

            Mvx.RegisterType(() => new Mock<IAutobackupManager>().Object);
            Mvx.RegisterType(() => settingsMockSetup.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(AccountMissingException))]
        public void SaveWithouthAccount_NoAccount_InvalidDataException()
        {
            var paymentDataAccess = new Mock<IDataAccess<Payment>>();
            paymentDataAccess.Setup(x => x.LoadList(null))
                .Returns(new List<Payment>());

            var repository = new PaymentRepository(paymentDataAccess.Object);

            var payment = new Payment
            {
                Amount = 20
            };

            repository.Save(payment);
        }

        [TestMethod]
        public void Save_IncomeType_CorrectlySaved()
        {
            Payment savedPayment = null;

            var paymentDataAccessSetup = new Mock<IDataAccess<Payment>>();
            paymentDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>());
            paymentDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Payment>()))
                .Callback((Payment p) => savedPayment = p);

            var repository = new PaymentRepository(paymentDataAccessSetup.Object);

            var account = new Account
            {
                Id = 2,
                Name = "TestAccount"
            };

            var payment = new Payment
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
            Payment savedPayment = null;

            var paymentDataAccessSetup = new Mock<IDataAccess<Payment>>();
            paymentDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>());
            paymentDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Payment>()))
                .Callback((Payment p) => savedPayment = p);

            var repository = new PaymentRepository(paymentDataAccessSetup.Object);

            var account = new Account {
                Id = 2,
                Name = "TestAccount"
            };

            var payment = new Payment {
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
            Payment savedPayment = null;

            var paymentDataAccessSetup = new Mock<IDataAccess<Payment>>();
            paymentDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>());
            paymentDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Payment>()))
                .Callback((Payment p) => savedPayment = p);

            var repository = new PaymentRepository(paymentDataAccessSetup.Object);

            var account = new Account {
                Id = 2,
                Name = "TestAccount"
            };

            var payment = new Payment {
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
            var paymentDataAccessSetup = new Mock<IDataAccess<Payment>>();
            paymentDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>());

            var repository = new PaymentRepository(paymentDataAccessSetup.Object);

            var account = new Account
            {
                Id = 2,
                Name = "TestAccount"
            };

            var payment = new Payment
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
        public void PaymentRepository_AccessCache()
        {
            var paymentDataAccessSetup = new Mock<IDataAccess<Payment>>();
            paymentDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>());

            var paymentRepo = new PaymentRepository(paymentDataAccessSetup.Object);

            Assert.IsFalse(paymentRepo.GetList().Any());
        }

        [TestMethod]
        public void Load_Payment_DataInitialized()
        {
            var dataAccessSetup = new Mock<IDataAccess<Payment>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>
            {
                new Payment {Id = 10},
                new Payment {Id = 15}
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
            var dataAccessSetup = new Mock<IDataAccess<Payment>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>());

            new PaymentRepository(dataAccessSetup.Object)
                .Save(new Payment {ChargedAccountId = 0});
        }

        [TestMethod]
        public void FindById_ReturnsPayment() {
            var dataAccessMock = new Mock<IDataAccess<RecurringPayment>>();
            var testPayment = new RecurringPayment { Id = 100, Amount = 78 };

            dataAccessMock.Setup(x => x.LoadList(null))
                .Returns(new List<RecurringPayment> { testPayment });

            Assert.AreEqual(testPayment, new RecurringPaymentRepository(dataAccessMock.Object).FindById(100));
        }
    }
}