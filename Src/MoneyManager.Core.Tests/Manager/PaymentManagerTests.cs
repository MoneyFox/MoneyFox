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
    public class PaymentManagerTests
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

            var trans1 = new Payment
            {
                Id = 1,
                ChargedAccount = account1,
                ChargedAccountId = account1.Id
            };

            var transRepoSetup = new Mock<IPaymentRepository>();
            transRepoSetup.SetupAllProperties();
            transRepoSetup.Setup(x => x.Delete(It.IsAny<Payment>()))
                .Callback((Payment trans) => resultList.Add(trans.Id));
            transRepoSetup.Setup(x => x.GetRelatedPayments(It.IsAny<Account>()))
                .Returns(new List<Payment>
                {
                    trans1
                });

            var repo = transRepoSetup.Object;
            repo.Data = new ObservableCollection<Payment>();

            new PaymentManager(repo, new Mock<IAccountRepository>().Object, new Mock<IDialogService>().Object)
                .DeleteAssociatedPaymentsFromDatabase(account1);

            resultList.Count.ShouldBe(1);
            resultList.First().ShouldBe(trans1.Id);
        }

        [Fact]
        public void DeleteAssociatedTransactionsFromDatabase_DataNull_DoNothing()
        {
            new PaymentManager(new Mock<IPaymentRepository>().Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object).DeleteAssociatedPaymentsFromDatabase(
                    new Account {Id = 3});
        }

        [Fact]
        public async void CheckForRecurringTransaction_IsRecurringFalse_ReturnFalse()
        {
            var result = await new PaymentManager(new Mock<IPaymentRepository>().Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object)
                .CheckForRecurringPayment(new Payment {IsRecurring = false});

            result.ShouldBeFalse();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void CheckForRecurringTransaction_IsRecurringTrue_ReturnUserInput(bool userAnswer)
        {
            var dialogService = new Mock<IDialogService>();
            dialogService.Setup(
                x => x.ShowConfirmMessage(It.Is<string>(y => y == Strings.ChangeSubsequentPaymentTitle),
                    It.Is<string>(y => y == Strings.ChangeSubsequentPaymentMessage),
                    It.Is<string>(y => y == Strings.RecurringLabel),
                    It.Is<string>(y => y == Strings.JustThisLabel))).Returns(Task.FromResult(userAnswer));

            var result = await new PaymentManager(new Mock<IPaymentRepository>().Object,
                new Mock<IAccountRepository>().Object,
                dialogService.Object)
                .CheckForRecurringPayment(new Payment {IsRecurring = true});

            result.ShouldBe(userAnswer);
        }

        [Fact]
        public void RemoveRecurringForTransactions_RecTrans_TransactionPropertiesProperlyChanged()
        {
            var trans = new Payment
            {
                Id = 2,
                ReccuringPaymentId = 3,
                RecurringPayment = new RecurringPayment {Id = 3},
                IsRecurring = true
            };

            var transRepoSetup = new Mock<IPaymentRepository>();
            transRepoSetup.SetupAllProperties();

            var transRepo = transRepoSetup.Object;
            transRepo.Data = new ObservableCollection<Payment>(new List<Payment> {trans});

            new PaymentManager(transRepo,
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object).RemoveRecurringForPayments(trans.RecurringPayment);

            trans.IsRecurring.ShouldBeFalse();
            trans.ReccuringPaymentId.ShouldBe(0);
        }
    }
}