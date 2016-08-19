using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [TestClass]
    public class AccountListViewModelTests : MvxIoCSupportingTest
    {
        private Mock<IAccountRepository> accountRepository;

        [TestInitialize]
        public void Init()
        {
            MvxSingleton.ClearAllSingletons();
            accountRepository = new Mock<IAccountRepository>();
            accountRepository.SetupAllProperties();
            Setup();
        }

        [TestMethod]
        public void DeleteAccountCommand_UserReturnTrue_ExecuteDeletion()
        {
            var deleteCalled = false;

            accountRepository.Setup(x => x.Delete(It.IsAny<Account>())).Callback(() => deleteCalled = true);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();
            paymentRepoSetup.Object.Data = new ObservableCollection<Payment>();

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessage(It.IsAny<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(true));

            var viewModel = new AccountListViewModel(accountRepository.Object, paymentRepoSetup.Object, dialogServiceSetup.Object);

            viewModel.DeleteAccountCommand.Execute(new Account {Id = 3});

            deleteCalled.ShouldBeTrue();
        }

        [TestMethod]
        public void DeleteAccountCommand_UserReturnFalse_SkipDeletion()
        {
            var deleteCalled = false;
            accountRepository.Setup(x => x.Delete(It.IsAny<Account>())).Callback(() => deleteCalled = true);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessage(It.IsAny<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(false));

            var viewModel = new AccountListViewModel(accountRepository.Object, paymentRepoSetup.Object,
                dialogServiceSetup.Object);

            viewModel.DeleteAccountCommand.Execute(new Account {Id = 3});

            deleteCalled.ShouldBeFalse();
        }

        [TestMethod]
        public void DeleteAccountCommand_AccountNull_DoNothing()
        {
            var deleteCalled = false;

            accountRepository.Setup(x => x.Delete(It.IsAny<Account>())).Callback(() => deleteCalled = true);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessage(It.IsAny<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(true));

            var viewModel = new AccountListViewModel(accountRepository.Object, paymentRepoSetup.Object, dialogServiceSetup.Object);

            viewModel.DeleteAccountCommand.Execute(null);

            deleteCalled.ShouldBeFalse();
        }

        [TestMethod]
        public void DeleteAccountCommand_CascadeDeletePayments()
        {
            var deleteCalled = false;

            accountRepository.Setup(x => x.Delete(It.IsAny<Account>())).Callback(() => deleteCalled = true);

            var accountData = new Account
            {
                CurrentBalance = 0,
                Id = 300,
                Name = "Test Account"
            };
            accountRepository.Setup(x => x.GetList(null)).Returns(new List<Account> { accountData });


            var mockPaymentRepo = new Mock<IPaymentRepository>();

            mockPaymentRepo.SetupAllProperties();
            mockPaymentRepo.Setup(c => c.Delete(It.IsAny<Payment>()))
                .Callback((Payment payment) => { mockPaymentRepo.Object.Data.Remove(payment); });


            var paymentRepo = mockPaymentRepo.Object;
            paymentRepo.Data = new ObservableCollection<Payment>
            {
                new Payment
                {
                    Id = 1,
                    Amount = 100,
                    ChargedAccountId = accountData.Id
                },
                new Payment
                {
                    Id = 2,
                    Amount = 200,
                    ChargedAccountId = accountData.Id
                }
            };

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessage(It.IsAny<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(true));

            var viewModel = new AccountListViewModel(accountRepository.Object, paymentRepo, dialogServiceSetup.Object);

            viewModel.DeleteAccountCommand.Execute(accountData);
            deleteCalled.ShouldBeTrue();
            Assert.AreEqual(null, paymentRepo.Data.FirstOrDefault(p => p.Id == 1));
        }

        [TestMethod]
        public void IsAllAccountsEmpty_AccountsEmpty_True()
        {
            accountRepository.Setup(x => x.GetList(null)).Returns(new List<Account>());
            new AccountListViewModel(accountRepository.Object,null,null).IsAllAccountsEmpty.ShouldBeTrue();
        }

        [TestMethod]
        public void IsAllAccountsEmpty_OneAccount_False()
        {
            accountRepository.Setup(x => x.GetList(null)).Returns(new List<Account>());
            new AccountListViewModel(accountRepository.Object,null,null).IsAllAccountsEmpty.ShouldBeFalse();
        }

        [TestMethod]
        public void IsAllAccountsEmpty_TwoAccount_False()
        {
            accountRepository.Setup(x => x.GetList(null)).Returns(new List<Account> {
                new Account(),
                new Account()
            });

            new AccountListViewModel(accountRepository.Object,null,null).IsAllAccountsEmpty.ShouldBeFalse();
        }

        [TestMethod]
        public void AllAccounts_AccountsAvailable_MatchesRepository()
        {
            accountRepository.Setup(x => x.GetList(null)).Returns(new List<Account> {
                new Account(),
                new Account()
            });
            new AccountListViewModel(accountRepository.Object, null, null).AllAccounts.ShouldBe(accountRepository.Object.GetList());
        }
        [TestMethod]
        public void AllAccounts_NoAccountsAvailable_MatchesRepository()
        {
            accountRepository.Setup(x => x.GetList(null)).Returns(new List<Account>());
            new AccountListViewModel(accountRepository.Object, null, null).AllAccounts.ShouldBe(accountRepository.Object.GetList());
        }
    }
}