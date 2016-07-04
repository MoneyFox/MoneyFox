using System;
using System.Collections.ObjectModel;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [TestClass]
    public class ModifyAccountViewModelTests : MvxIoCSupportingTest {
        private DateTime localDateSetting;

        [TestInitialize]
        public void Init() {
            MvxSingleton.ClearAllSingletons();
            Setup();

            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.SetupAllProperties();
            settingsMockSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<DateTime>(), false))
                .Callback((string key, DateTime date, bool roam) => localDateSetting = date);

            Mvx.RegisterType(() => new Mock<IAutobackupManager>().Object);
            Mvx.RegisterType(() => settingsMockSetup.Object);
        }

        [TestMethod]
        public void Title_EditAccount_CorrectTitle()
        {
            var accountname = "Sparkonto";

            var accountRepositorySetup = new Mock<IAccountRepository>();

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object, new Mock<IDialogService>().Object) {
                IsEdit = true,
                SelectedAccount = new Account() { Id = 3, Name = accountname }
            };

            viewmodel.Title.ShouldBe(string.Format(Strings.EditAccountTitle, accountname));
        }

        [TestMethod]
        public void Title_AddAccount_CorrectTitle()
        {

            var viewmodel = new ModifyAccountViewModel(new Mock<IAccountRepository>().Object, new Mock<IDialogService>().Object)
            { IsEdit = false };

            viewmodel.Title.ShouldBe(Strings.AddAccountTitle);
        }

        [TestMethod]
        public void SaveCommand_Does_Not_Allow_Duplicate_Names() {
            var accountRepo = new Mock<IAccountRepository>();
            accountRepo.SetupAllProperties();
            accountRepo.Setup(c => c.Save(It.IsAny<Account>()))
                .Callback((Account acc) => { accountRepo.Object.Data.Add(acc); });
            accountRepo.Object.Data = new ObservableCollection<Account>();
            var account = new Account {
                Id = 1,
                Name = "Test Account"
            };
            var newAccount = new Account {
                Name = "Test Account"
            };
            accountRepo.Object.Data.Add(account);

            var viewmodel = new ModifyAccountViewModel(accountRepo.Object, new Mock<IDialogService>().Object) {
                IsEdit = false,
                SelectedAccount = newAccount
            };

            viewmodel.SaveCommand.Execute();
            Assert.AreEqual(1, accountRepo.Object.Data.Count);
        }

        [TestMethod]
        public void SaveCommand_Does_Not_Allow_Duplicate_Names2() {
            var accountRepo = new Mock<IAccountRepository>();
            accountRepo.SetupAllProperties();
            accountRepo.Setup(c => c.Save(It.IsAny<Account>()))
                .Callback((Account acc) => { accountRepo.Object.Data.Add(acc); });
            accountRepo.Object.Data = new ObservableCollection<Account>();
            var account = new Account {
                Id = 1,
                Name = "Test Account"
            };
            var newAccount = new Account {
                Name = "TESt Account"
            };
            accountRepo.Object.Data.Add(account);

            var viewmodel = new ModifyAccountViewModel(accountRepo.Object, new Mock<IDialogService>().Object) {
                IsEdit = false,
                SelectedAccount = newAccount
            };

            viewmodel.SaveCommand.Execute();
            Assert.AreEqual(1, accountRepo.Object.Data.Count);
        }

        [TestMethod]
        public void SaveCommand_SavesAccount() {
            var accountRepo = new Mock<IAccountRepository>();
            accountRepo.SetupAllProperties();
            accountRepo.Setup(c => c.Save(It.IsAny<Account>()))
                .Callback((Account acc) => { accountRepo.Object.Data.Add(acc); });
            accountRepo.Object.Data = new ObservableCollection<Account>();
            var account = new Account {
                Id = 1,
                Name = "Test Account"
            };

            var viewmodel = new ModifyAccountViewModel(accountRepo.Object, new Mock<IDialogService>().Object) {
                IsEdit = false,
                SelectedAccount = account
            };

            viewmodel.SaveCommand.Execute();
            Assert.AreEqual(1, accountRepo.Object.Data.Count);
        }

        [TestMethod]
        public void Save_UpdateTimeStamp()
        {
            var account = new Account { Id = 0, Name = "account" };

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupAllProperties();
            accountRepositorySetup.Setup(x => x.AddPaymentAmount(new Payment())).Returns(true);
            accountRepositorySetup.Setup(x => x.Save(account)).Returns(true);
            accountRepositorySetup.Setup(x => x.Data).Returns(() => new ObservableCollection<Account>());
            var accountRepo = accountRepositorySetup.Object;

            var viewmodel = new ModifyAccountViewModel(accountRepo, new Mock<IDialogService>().Object) {
                IsEdit = false,
                SelectedAccount = account
            };

            viewmodel.SaveCommand.Execute();

            localDateSetting.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            localDateSetting.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
        }
    }
}