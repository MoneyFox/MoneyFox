using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using MoneyManager.Core.Manager;
using MoneyManager.Core.Repositories;
using MoneyManager.DataAccess;
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
            new RepositoryManager(null, null,null).ShouldNotBeNull();
        }

        [Fact]
        public void Constructor_DefaultInstances_NoException()
        {
            var connectionCreatorMock = new Mock<ISqliteConnectionCreator>().Object;

            new RepositoryManager(new AccountRepository(new AccountDataAccess(connectionCreatorMock)),
                new TransactionRepository(new TransactionDataAccess(connectionCreatorMock), new RecurringTransactionDataAccess(connectionCreatorMock)),
                new CategoryRepository(new CategoryDataAccess(connectionCreatorMock))).ShouldNotBeNull();
        }

        [Fact]
        public void ReloadData_SelectedNotNull_SelectedSetToNull()
        {
            var accountRepoSetup = new Mock<IRepository<Account>>();
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

            new RepositoryManager(accountRepo, transactionRepo, categoryRepo).ReloadData();

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
            
            var accountRepoSetup = new Mock<IRepository<Account>>();
            accountRepoSetup.SetupAllProperties();
            accountRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>())).Callback(() => accountsLoaded = true);

            var transactionRepoSetup = new Mock<ITransactionRepository>();
            transactionRepoSetup.SetupAllProperties();
            transactionRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<FinancialTransaction, bool>>>())).Callback(() => transactionsLoaded = true);

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();
            categoryRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Category, bool>>>())).Callback(() => categoryLoaded = true);

            new RepositoryManager(accountRepoSetup.Object, transactionRepoSetup.Object, categoryRepoSetup.Object).ReloadData();

            accountsLoaded.ShouldBeTrue();
            transactionsLoaded.ShouldBeTrue();
            categoryLoaded.ShouldBeTrue();
        }
    }
}
