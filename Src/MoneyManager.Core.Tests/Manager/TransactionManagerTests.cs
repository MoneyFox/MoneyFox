using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Core.Manager;
using MoneyManager.Core.Repositories;
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

            new TransactionManager(repo, new Mock<IAccountRepository>().Object, new Mock<IDialogService>().Object).DeleteAssociatedTransactionsFromDatabase(account1);

            resultList.Count.ShouldBe(1);
            resultList.First().ShouldBe(trans1.Id);
        }

        [Fact]
        public void DeleteAssociatedTransactionsFromDatabase_DataNull_DoNothing()
        {
            new TransactionManager(new Mock<ITransactionRepository>().Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object).DeleteAssociatedTransactionsFromDatabase(
                new Account {Id = 3});
        }
    }
}