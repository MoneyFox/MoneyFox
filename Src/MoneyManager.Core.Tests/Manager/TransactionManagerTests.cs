using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
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

        [Fact]
        public async void CheckForRecurringTransaction_IsRecurringFalse_ReturnFalse()
        {
            var result = await new TransactionManager(new Mock<ITransactionRepository>().Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object)
                .CheckForRecurringTransaction(new FinancialTransaction {IsRecurring = false});

            result.ShouldBeFalse();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void CheckForRecurringTransaction_IsRecurringTrue_ReturnUserInput(bool userAnswer)
        {
            var dialogService = new Mock<IDialogService>();
            dialogService.Setup(x => x.ShowConfirmMessage(It.Is<string>(y => y == Strings.ChangeSubsequentTransactionsTitle),
                It.Is<string>(y => y == Strings.ChangeSubsequentTransactionsMessage),
                It.Is<string>(y => y == Strings.RecurringLabel),
                It.Is<string>(y => y == Strings.JustThisLabel))).Returns(Task.FromResult(userAnswer));

            var result = await new TransactionManager(new Mock<ITransactionRepository>().Object,
                new Mock<IAccountRepository>().Object,
                dialogService.Object)
                .CheckForRecurringTransaction(new FinancialTransaction {IsRecurring = true});

            result.ShouldBe(userAnswer);
        }

        [Fact]
        public void RemoveRecurringForTransactions_RecTrans_TransactionPropertiesProperlyChanged()
        {
            var trans = new FinancialTransaction
            {
                Id = 2,
                ReccuringTransactionId = 3,
                RecurringTransaction = new RecurringTransaction {Id = 3},
                IsRecurring = true
            };

            var transRepoSetup = new Mock<ITransactionRepository>();
            transRepoSetup.SetupAllProperties();

            var transRepo = transRepoSetup.Object;
            transRepo.Data = new ObservableCollection<FinancialTransaction>(new List<FinancialTransaction> { trans});

            new TransactionManager(transRepo,
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object).RemoveRecurringForTransactions(trans.RecurringTransaction);

            trans.IsRecurring.ShouldBeFalse();
            trans.ReccuringTransactionId.ShouldBe(0);
        }
    }
}