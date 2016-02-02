using System.Threading.Tasks;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Interfaces.ViewModels;
using MoneyManager.Foundation.Model;
using Moq;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;
using Xunit;
using XunitShouldExtension;

namespace MoneyManager.Core.Tests.ViewModels
{
    public class AccountListViewModelTests : MvxIoCSupportingTest
    {
        public AccountListViewModelTests()
        {
            MvxSingleton.ClearAllSingletons();
            Setup();
        }

        [Fact]
        public void OpenOverviewCommand_Account_SelectedSet()
        {
            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();

            var accountRepo = accountRepoSetup.Object;

            var viewModel = new AccountListViewModel(accountRepo,
                new Mock<IBalanceViewModel>().Object,
                new Mock<IDialogService>().Object);

            viewModel.OpenOverviewCommand.Execute(new Account {Id = 42});

            accountRepo.Selected.Id.ShouldBe(42);
        }

        [Fact]
        public void OpenOverviewCommand_NullAccount_DoNothing()
        {
            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();
            var accountRepo = accountRepoSetup.Object;

            var viewModel = new AccountListViewModel(accountRepo,
                new Mock<IBalanceViewModel>().Object,
                new Mock<IDialogService>().Object);

            viewModel.OpenOverviewCommand.Execute(null);

            accountRepo.Selected.ShouldBeNull();
        }

        [Fact]
        public void DeleteAccountCommand_UserReturnTrue_ExecuteDeletion()
        {
            var deleteCalled = false;
            var updateBalanceCalled = false;

            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();
            accountRepoSetup.Setup(x => x.Delete(It.IsAny<Account>())).Callback(() => deleteCalled = true);
            var accountRepo = accountRepoSetup.Object;

            var balanceViewModelSetup = new Mock<IBalanceViewModel>();
            balanceViewModelSetup.Setup(x => x.UpdateBalance(false)).Callback(() => updateBalanceCalled = true);

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessage(It.IsAny<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(true));

            var viewModel = new AccountListViewModel(accountRepo,
                balanceViewModelSetup.Object,
                dialogServiceSetup.Object
                );

            viewModel.DeleteAccountCommand.Execute(new Account {Id = 3});

            deleteCalled.ShouldBeTrue();
            updateBalanceCalled.ShouldBeTrue();
        }

        [Fact]
        public void DeleteAccountCommand_UserReturnFalse_SkipDeletion()
        {
            var deleteCalled = false;
            var updateBalanceCalled = false;

            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();
            accountRepoSetup.Setup(x => x.Delete(It.IsAny<Account>())).Callback(() => deleteCalled = true);
            var accountRepo = accountRepoSetup.Object;

            var balanceViewModelSetup = new Mock<IBalanceViewModel>();
            balanceViewModelSetup.Setup(x => x.UpdateBalance(false)).Callback(() => updateBalanceCalled = true);

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessage(It.IsAny<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(false));

            var viewModel = new AccountListViewModel(accountRepo,
                balanceViewModelSetup.Object,
                dialogServiceSetup.Object
                );

            viewModel.DeleteAccountCommand.Execute(new Account {Id = 3});

            deleteCalled.ShouldBeFalse();
            updateBalanceCalled.ShouldBeFalse();
        }

        [Fact]
        public void DeleteAccountCommand_AccountNull_DoNothing()
        {
            var deleteCalled = false;
            var updateBalanceCalled = false;

            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();
            accountRepoSetup.Setup(x => x.Delete(It.IsAny<Account>())).Callback(() => deleteCalled = true);
            var accountRepo = accountRepoSetup.Object;

            var balanceViewModelSetup = new Mock<IBalanceViewModel>();
            balanceViewModelSetup.Setup(x => x.UpdateBalance(false)).Callback(() => updateBalanceCalled = true);

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessage(It.IsAny<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(true));

            var viewModel = new AccountListViewModel(accountRepo,
                balanceViewModelSetup.Object,
                dialogServiceSetup.Object
                );

            viewModel.DeleteAccountCommand.Execute(null);

            deleteCalled.ShouldBeFalse();
            updateBalanceCalled.ShouldBeFalse();
        }
    }
}