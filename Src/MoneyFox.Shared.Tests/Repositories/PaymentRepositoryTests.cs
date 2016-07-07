using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Exceptions;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.Tests.Mocks;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.Repositories
{
    [TestClass]
    public class PaymentRepositoryTests : MvxIoCSupportingTest
    {
        private DateTime localDateSetting;

        [TestInitialize]
        public void Init()
        {
            Setup();

            // We setup the static setting classes here for the general usage in the app
            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.SetupAllProperties();
            settingsMockSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<DateTime>(), false))
                .Callback((string key, DateTime date, bool roam) => localDateSetting = date);

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
            paymentDataAccessSetup.SetupAllProperties();

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
            Assert.AreSame(payment, repository.Data[0]);

            repository.Delete(payment);

            Assert.IsFalse(repository.Data.Any());
        }

        [TestMethod]
        public void PaymentRepository_AccessCache()
        {
            var paymentDataAccessSetup = new Mock<IDataAccess<Payment>>();
            paymentDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>());
            paymentDataAccessSetup.SetupAllProperties();

            var paymentRepo = new PaymentRepository(paymentDataAccessSetup.Object);

            Assert.IsFalse(paymentRepo.Data.Any());
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

            Assert.IsTrue(paymentRepository.Data.Any(x => x.Id == 10));
            Assert.IsTrue(paymentRepository.Data.Any(x => x.Id == 15));
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
        public void PaymentRepository_FindById_ReturnsPayment()
        {
            var paymentRepository = new Mock<IPaymentRepository>();
            var testPayment = new Payment {Id = 100};
            paymentRepository.SetupAllProperties();
            paymentRepository.Setup(x => x.FindById(It.IsAny<int>()))
                .Returns((int paymentId) => paymentRepository.Object.Data.FirstOrDefault(p => p.Id == paymentId));
            paymentRepository.Object.Data = new ObservableCollection<Payment>();
            paymentRepository.Object.Data.Add(testPayment);

            Assert.AreEqual(testPayment, paymentRepository.Object.FindById(100));
        }
    }
}