using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.ViewModels {
    [TestClass]
    public class ModifyAccountViewModelTests : MvxIoCSupportingTest
    {
        [TestInitialize]
        public void Init()
        {
            MvxSingleton.ClearAllSingletons();
            Setup();
        }

        [TestMethod]
        public void Title_EditAccount_CorrectTitle() {
            var accountname = "Sparkonto";

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Selected).Returns(new Account {Id = 2, Name = accountname});

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object, new Mock<IDialogService>().Object)
            {IsEdit = true};

            viewmodel.Title.ShouldBe(string.Format(Strings.EditAccountTitle, accountname));
        }

        [TestMethod]
        public void Title_AddAccount_CorrectTitle() {
            var accountname = "Sparkonto";

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Selected).Returns(new Account {Id = 2, Name = accountname});

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object, new Mock<IDialogService>().Object)
            {IsEdit = false};

            viewmodel.Title.ShouldBe(Strings.AddAccountTitle);
        }

        [TestMethod]
        public void SaveCommand_Does_Not_Allow_Duplicate_Names()
        {
            var accountRepo = new Mock<IAccountRepository>();
            accountRepo.SetupAllProperties();
            accountRepo.Setup(c => c.Save(It.IsAny<Account>())).Callback((Account acc) =>
            {
                accountRepo.Object.Data.Add(acc);
            });
            accountRepo.Object.Data = new ObservableCollection<Account>();
            var account = new Account()
            {
                Id = 1,
                Name = "Test Account"
            };
            var newAccount = new Account()
            {
                Name = "Test Account"
            };
            accountRepo.Object.Data.Add(account);
            accountRepo.Object.Selected = newAccount;

            var viewmodel = new ModifyAccountViewModel(accountRepo.Object, new Mock<IDialogService>().Object)
            {
                IsEdit = false
            };

            viewmodel.SaveCommand.Execute();
            Assert.AreEqual(1, accountRepo.Object.Data.Count);

        }

        [TestMethod]
        public void SaveCommand_SavesAccount()
        {
            var accountRepo = new Mock<IAccountRepository>();
            accountRepo.SetupAllProperties();
            accountRepo.Setup(c => c.Save(It.IsAny<Account>())).Callback((Account acc) =>
            {
                accountRepo.Object.Data.Add(acc);
            });
            accountRepo.Object.Data = new ObservableCollection<Account>();
            var account = new Account()
            {
                Id = 1,
                Name = "Test Account"
            };
            accountRepo.Object.Selected = account;

            var viewmodel = new ModifyAccountViewModel(accountRepo.Object, new Mock<IDialogService>().Object)
            {
                IsEdit = false
            };

            viewmodel.SaveCommand.Execute();
            Assert.AreEqual(1, accountRepo.Object.Data.Count);
        }
    }
}