using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Model;
using Moq;

namespace MoneyFox.Shared.Tests.Manager {
    [TestClass]
    public class RepositoryManagerTests {
        [TestMethod]
        public void Constructor_NullValues_NoException() {
            new RepositoryManager(null, null, null, null).ShouldNotBeNull();
        }

        [TestMethod]
        public void ReloadData_SelectedNotNull_SelectedSetToNull() {
            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<ICategoryRepository>();
            categoryRepoSetup.SetupAllProperties();

            var accountRepo = accountRepoSetup.Object;
            var paymentRepository = paymentRepoSetup.Object;
            var categoryRepo = categoryRepoSetup.Object;

            paymentRepository.Selected = new Payment();
            categoryRepo.Selected = new Category();

            new RepositoryManager(accountRepo, paymentRepository, categoryRepo,
                new PaymentManager(paymentRepository, accountRepo, new Mock<IDialogService>().Object)).ReloadData();

            Assert.IsNull(paymentRepository.Selected);
            Assert.IsNull(categoryRepo.Selected);
        }

        [TestMethod]
        public void ReloadData_CollectionNull_CollectionInstantiated() {
            var accountsLoaded = false;
            var paymentsLoaded = false;
            var categoryLoaded = false;

            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();
            accountRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()))
                .Callback(() => accountsLoaded = true);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();
            paymentRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Payment, bool>>>()))
                .Callback(() => paymentsLoaded = true);

            var categoryRepoSetup = new Mock<ICategoryRepository>();
            categoryRepoSetup.SetupAllProperties();
            categoryRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Category, bool>>>()))
                .Callback(() => categoryLoaded = true);

            var accountRepo = accountRepoSetup.Object;
            var paymentRepository = paymentRepoSetup.Object;

            new RepositoryManager(accountRepo, paymentRepository, categoryRepoSetup.Object,
                new PaymentManager(paymentRepository, accountRepo, new Mock<IDialogService>().Object))
                .ReloadData();

            Assert.IsTrue(accountsLoaded);
            Assert.IsTrue(paymentsLoaded);
            Assert.IsTrue(categoryLoaded);
        }

        [TestMethod]
        public void ReloadData_UnclearedPayment_Clear() {
            var account = new Account {Id = 1, CurrentBalance = 40};
            var payment = new Payment {
                ChargedAccount = account,
                ChargedAccountId = 1,
                IsCleared = false,
                Date = DateTime.Today.AddDays(-3)
            };

            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();
            paymentRepoSetup.Setup(x => x.GetUnclearedPayments())
                .Returns(() => new List<Payment> {payment});

            var categoryRepoSetup = new Mock<ICategoryRepository>();
            categoryRepoSetup.SetupAllProperties();

            var accountRepo = accountRepoSetup.Object;
            var paymentRepository = paymentRepoSetup.Object;

            accountRepo.Data = new ObservableCollection<Account>(new List<Account> {account});

            new RepositoryManager(accountRepo, paymentRepository, categoryRepoSetup.Object,
                new PaymentManager(paymentRepository, accountRepo, new Mock<IDialogService>().Object))
                .ReloadData();

            Assert.IsTrue(payment.IsCleared);
        }
    }
}