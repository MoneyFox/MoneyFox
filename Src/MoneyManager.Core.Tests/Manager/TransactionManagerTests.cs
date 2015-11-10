using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.TestFoundation;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.Manager
{
    public class TransactionManagerTests
    {
        [Theory]
        [InlineData(TransactionType.Spending, true, 550)]
        [InlineData(TransactionType.Spending, false, 500)]
        [InlineData(TransactionType.Income, true, 450)]
        [InlineData(TransactionType.Income, false, 500)]
        public void DeleteTransaction_WithoutSpending_DeletedAccountBalanceSet(TransactionType type, bool cleared, int resultAmount)
        {
            var deletedId = 0;

            var account = new Account
            {
                Id = 3,
                Name = "just an account",
                CurrentBalance = 500
            };

            var transaction = new FinancialTransaction
            {
                Id = 10,
                ChargedAccountId = account.Id,
                ChargedAccount = account,
                Amount = 50,
                Type = (int) type,
                IsCleared = cleared
            };

            var transRepoSetup = new Mock<ITransactionRepository>();
            transRepoSetup.SetupAllProperties();
            transRepoSetup.Setup(x => x.Delete(It.IsAny<FinancialTransaction>()))
                .Callback((FinancialTransaction trans) => deletedId = trans.Id);

            var accountRepoSetup = new Mock<IRepository<Account>>();
            accountRepoSetup.SetupAllProperties();

            var accountRepo = accountRepoSetup.Object;
            accountRepo.Data = new ObservableCollection<Account>(new List<Account> {account});

            var transRepo = transRepoSetup.Object;
            transRepo.Data = new ObservableCollection<FinancialTransaction>(new List<FinancialTransaction> {transaction});

            new TransactionManager(transRepo, accountRepo, new Mock<IDialogService>().Object).DeleteTransaction(transaction);

            deletedId.ShouldBe(10);
            account.CurrentBalance.ShouldBe(resultAmount);
        }

        [Theory]
        [InlineData(true, 550, 850)]
        [InlineData(false, 500, 900)]
        public void DeleteTransaction_Transfer_Deleted(bool isCleared, int balanceAccount1, int balanceAccount2)
        {
            var deletedId = 0;

            var account1 = new Account
            {
                Id = 3,
                Name = "just an account",
                CurrentBalance = 500
            };
            var account2 = new Account
            {
                Id = 4,
                Name = "just an account",
                CurrentBalance = 900
            };

            var transaction = new FinancialTransaction
            {
                Id = 10,
                ChargedAccountId = account1.Id,
                ChargedAccount = account1,
                TargetAccountId = account2.Id,
                TargetAccount = account2,
                Amount = 50,
                Type = (int) TransactionType.Transfer,
                IsCleared = isCleared
            };

            var transRepoSetup = new Mock<ITransactionRepository>();
            transRepoSetup.SetupAllProperties();
            transRepoSetup.Setup(x => x.Delete(It.IsAny<FinancialTransaction>()))
                .Callback((FinancialTransaction trans) => deletedId = trans.Id);

            var accountRepoSetup = new Mock<IRepository<Account>>();
            accountRepoSetup.SetupAllProperties();

            var accountRepo = accountRepoSetup.Object;
            accountRepo.Data = new ObservableCollection<Account>(new List<Account> {account1, account2});

            var transRepo = transRepoSetup.Object;
            transRepo.Data = new ObservableCollection<FinancialTransaction>(new List<FinancialTransaction> {transaction});

            new TransactionManager(transRepo, accountRepo, new Mock<IDialogService>().Object).DeleteTransaction(transaction);

            deletedId.ShouldBe(10);
            account1.CurrentBalance.ShouldBe(balanceAccount1);
            account2.CurrentBalance.ShouldBe(balanceAccount2);
        }

        [Fact]
        public void DeleteAssociatedTransactionsFromDatabase_Account_DeleteRightTransactions()
        {
            var resultList = new List<int>();

            var account1 = new Account
            {
                Id = 3,
                Name = "just an account",
                CurrentBalance = 500
            };
            var account2 = new Account
            {
                Id = 4,
                Name = "just an account",
                CurrentBalance = 900
            };

            var trans1 = new FinancialTransaction
            {
                Id = 1,
                ChargedAccount = account1,
                ChargedAccountId = account1.Id
            };

            var transRepoSetup = new Mock<ITransactionRepository>();
            transRepoSetup.SetupAllProperties();
            transRepoSetup.Setup(x => x.Delete(It.IsAny<FinancialTransaction>()))
                .Callback((FinancialTransaction trans) => resultList.Add(trans.Id));
            transRepoSetup.Setup(x => x.GetRelatedTransactions(It.IsAny<Account>()))
                .Returns(new List<FinancialTransaction>
                {
                    trans1
                });

            var repo = transRepoSetup.Object;
            repo.Data = new ObservableCollection<FinancialTransaction>();

            new TransactionManager(repo, new Mock<IRepository<Account>>().Object, new Mock<IDialogService>().Object).DeleteAssociatedTransactionsFromDatabase(account1);

            resultList.Count.ShouldBe(1);
            resultList.First().ShouldBe(trans1.Id);
        }

        [Fact]
        public void DeleteAssociatedTransactionsFromDatabase_DataNull_DoNothing()
        {
            new TransactionManager(new Mock<ITransactionRepository>().Object,
                new Mock<IRepository<Account>>().Object,
                new Mock<IDialogService>().Object).DeleteAssociatedTransactionsFromDatabase(
                new Account {Id = 3});
        }
    }
}