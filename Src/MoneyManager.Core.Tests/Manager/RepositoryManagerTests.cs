using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.TestFoundation;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.Manager
{
    public class RepositoryManagerTests
    {
        [Fact]
        public void Constructor_NullValues_NoException()
        {
            new RepositoryManager(null, null, null, null).ShouldNotBeNull();
        }

        [Fact]
        public void ReloadData_SelectedNotNull_SelectedSetToNull()
        {
            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var accountRepo = accountRepoSetup.Object;
            var paymentRepository = paymentRepoSetup.Object;
            var categoryRepo = categoryRepoSetup.Object;

            accountRepo.Selected = new Account();
            paymentRepository.Selected = new Payment();
            categoryRepo.Selected = new Category();

            new RepositoryManager(accountRepo, paymentRepository, categoryRepo,
                new PaymentManager(paymentRepository, accountRepo, new Mock<IDialogService>().Object)).ReloadData();

            accountRepo.Selected.ShouldBeNull();
            paymentRepository.Selected.ShouldBeNull();
            categoryRepo.Selected.ShouldBeNull();
        }

        [Fact]
        public void ReloadData_CollectionNull_CollectionInstantiated()
        {
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

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();
            categoryRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Category, bool>>>()))
                .Callback(() => categoryLoaded = true);

            var accountRepo = accountRepoSetup.Object;
            var paymentRepository = paymentRepoSetup.Object;

            new RepositoryManager(accountRepo, paymentRepository, categoryRepoSetup.Object,
                new PaymentManager(paymentRepository, accountRepo, new Mock<IDialogService>().Object))
                .ReloadData();

            accountsLoaded.ShouldBeTrue();
            paymentsLoaded.ShouldBeTrue();
            categoryLoaded.ShouldBeTrue();
        }

        [Fact]
        public void ReloadData_UnclearedTransaction_Clear()
        {
            var account = new Account {Id = 1, CurrentBalance = 40};
            var payment = new Payment
            {
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

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var accountRepo = accountRepoSetup.Object;
            var paymentRepository = paymentRepoSetup.Object;

            accountRepo.Data = new ObservableCollection<Account>(new List<Account> {account});

            new RepositoryManager(accountRepo, paymentRepository, categoryRepoSetup.Object,
                new PaymentManager(paymentRepository, accountRepo, new Mock<IDialogService>().Object))
                .ReloadData();

            payment.IsCleared.ShouldBeTrue();
        }
    }
}