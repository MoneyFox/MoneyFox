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
        public AccountListViewModelTests()
        {
            MvxSingleton.ClearAllSingletons();
            Setup();
        }

        [TestMethod]
        public void OpenOverviewCommand_Account_SelectedSet()
        {
            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var accountRepo = accountRepoSetup.Object;

            var viewModel = new AccountListViewModel(accountRepo,
                paymentRepoSetup.Object,
                new Mock<IDialogService>().Object);

            viewModel.OpenOverviewCommand.Execute(new Account {Id = 42});

            accountRepo.Selected.Id.ShouldBe(42);
        }

        [TestMethod]
        public void OpenOverviewCommand_NullAccount_DoNothing()
        {
            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();

            var accountRepo = accountRepoSetup.Object;

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var viewModel = new AccountListViewModel(accountRepo,
                paymentRepoSetup.Object,
                new Mock<IDialogService>().Object);

            viewModel.OpenOverviewCommand.Execute(null);

            accountRepo.Selected.ShouldBeNull();
        }

        [TestMethod]
        public void DeleteAccountCommand_UserReturnTrue_ExecuteDeletion()
        {
            var deleteCalled = false;

            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();
            accountRepoSetup.Setup(x => x.Delete(It.IsAny<Account>())).Callback(() => deleteCalled = true);
            var accountRepo = accountRepoSetup.Object;

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessage(It.IsAny<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(true));

            var viewModel = new AccountListViewModel(accountRepo,
                paymentRepoSetup.Object,
                dialogServiceSetup.Object
                );

            viewModel.DeleteAccountCommand.Execute(new Account {Id = 3});

            deleteCalled.ShouldBeTrue();
        }

        [TestMethod]
        public void DeleteAccountCommand_UserReturnFalse_SkipDeletion()
        {
            var deleteCalled = false;

            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();
            accountRepoSetup.Setup(x => x.Delete(It.IsAny<Account>())).Callback(() => deleteCalled = true);
            var accountRepo = accountRepoSetup.Object;

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessage(It.IsAny<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(false));

            var viewModel = new AccountListViewModel(accountRepo,
                paymentRepoSetup.Object,
                dialogServiceSetup.Object
                );

            viewModel.DeleteAccountCommand.Execute(new Account {Id = 3});

            deleteCalled.ShouldBeFalse();
        }

        [TestMethod]
        public void DeleteAccountCommand_AccountNull_DoNothing()
        {
            var deleteCalled = false;

            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();
            accountRepoSetup.Setup(x => x.Delete(It.IsAny<Account>())).Callback(() => deleteCalled = true);
            var accountRepo = accountRepoSetup.Object;

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessage(It.IsAny<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(true));

            var viewModel = new AccountListViewModel(accountRepo,
                paymentRepoSetup.Object,
                dialogServiceSetup.Object
                );

            viewModel.DeleteAccountCommand.Execute(null);

            deleteCalled.ShouldBeFalse();
        }
    }
}