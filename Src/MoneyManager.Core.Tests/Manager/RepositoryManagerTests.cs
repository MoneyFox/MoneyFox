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

            var transactionRepoSetup = new Mock<ITransactionRepository>();
            transactionRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var accountRepo = accountRepoSetup.Object;
            var transactionRepo = transactionRepoSetup.Object;
            var categoryRepo = categoryRepoSetup.Object;

            accountRepo.Selected = new Account();
            transactionRepo.Selected = new FinancialTransaction();
            categoryRepo.Selected = new Category();

            new RepositoryManager(accountRepo, transactionRepo, categoryRepo,
                new TransactionManager(transactionRepo, accountRepo, new Mock<IDialogService>().Object)).ReloadData();

            accountRepo.Selected.ShouldBeNull();
            transactionRepo.Selected.ShouldBeNull();
            categoryRepo.Selected.ShouldBeNull();
        }

        [Fact]
        public void ReloadData_CollectionNull_CollectionInstantiated()
        {
            var accountsLoaded = false;
            var transactionsLoaded = false;
            var categoryLoaded = false;

            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();
            accountRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()))
                .Callback(() => accountsLoaded = true);

            var transactionRepoSetup = new Mock<ITransactionRepository>();
            transactionRepoSetup.SetupAllProperties();
            transactionRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<FinancialTransaction, bool>>>()))
                .Callback(() => transactionsLoaded = true);

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();
            categoryRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Category, bool>>>()))
                .Callback(() => categoryLoaded = true);

            var accountRepo = accountRepoSetup.Object;
            var transactionRepo = transactionRepoSetup.Object;

            new RepositoryManager(accountRepo, transactionRepo, categoryRepoSetup.Object,
                new TransactionManager(transactionRepo, accountRepo, new Mock<IDialogService>().Object))
                .ReloadData();

            accountsLoaded.ShouldBeTrue();
            transactionsLoaded.ShouldBeTrue();
            categoryLoaded.ShouldBeTrue();
        }

        [Fact]
        public void ReloadData_UnclearedTransaction_Clear()
        {
            var account = new Account {Id = 1, CurrentBalance = 40};
            var transaction = new FinancialTransaction
            {
                ChargedAccount = account,
                ChargedAccountId = 1,
                IsCleared = false,
                Date = DateTime.Today.AddDays(-3)
            };

            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();

            var transactionRepoSetup = new Mock<ITransactionRepository>();
            transactionRepoSetup.SetupAllProperties();
            transactionRepoSetup.Setup(x => x.GetUnclearedTransactions())
                .Returns(() => new List<FinancialTransaction> {transaction});

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var accountRepo = accountRepoSetup.Object;
            var transactionRepo = transactionRepoSetup.Object;

            accountRepo.Data = new ObservableCollection<Account>(new List<Account> {account});

            new RepositoryManager(accountRepo, transactionRepo, categoryRepoSetup.Object,
                new TransactionManager(transactionRepo, accountRepo, new Mock<IDialogService>().Object))
                .ReloadData();

            transaction.IsCleared.ShouldBeTrue();
        }
    }
}