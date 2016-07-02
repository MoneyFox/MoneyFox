using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Test.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace MoneyFox.Shared.Tests.ViewModels {
    [TestClass]
    public class ModifyAccountViewModelTests : MvxIoCSupportingTest {

        private DateTime localDateSetting;

        [TestInitialize]
        public void Init()
        {
            ClearAll();
            Setup();

            // We setup the static setting classes here for the general usage in the app
            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.SetupAllProperties();
            settingsMockSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<DateTime>(), false))
                .Callback((string key, DateTime date, bool roam) => localDateSetting = date);

            Mvx.RegisterType(() => new Mock<IAutobackupManager>().Object);
            Mvx.RegisterType(() => settingsMockSetup.Object);
        }

        [TestMethod]
        public void Title_EditAccount_CorrectTitle() {
            var accountname = "Sparkonto";
            var accountRepositorySetup = new Mock<IAccountRepository>();

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object, new Mock<IDialogService>().Object)
            {
                IsEdit = true,
                SelectedAccount = new Account() { Id = 3, Name = accountname}
            };

            viewmodel.Title.ShouldBe(string.Format(Strings.EditAccountTitle, accountname));
        }

        [TestMethod]
        public void Title_AddAccount_CorrectTitle() {

            var viewmodel = new ModifyAccountViewModel(new Mock<IAccountRepository>().Object, new Mock<IDialogService>().Object)
            {IsEdit = false};

            viewmodel.Title.ShouldBe(Strings.AddAccountTitle);
        }

        [TestMethod]
        public void Save_UpdateTimeStamp()
        {
<<<<<<< HEAD
            var accountname = "Sparkonto";

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepositorySetup.SetupAllProperties();
           

            ObservableCollection<Account> accountData = new ObservableCollection<Account>();
            accountData.Add(new Account { Id = 2, Name = accountname });
            accountRepositorySetup.SetupGet(x => x.Data).Returns(accountData);
            accountRepositorySetup.Setup(x => x.AddPaymentAmount(new Payment())).Returns(true);
            accountRepositorySetup.SetupGet(x => x.Selected).Returns(new Account { Id = 2, Name = "Sparkonto2" });
            accountRepositorySetup.Setup(x => x.Save(accountRepositorySetup.Object.Selected)).Returns(true);

            var accountRepo = accountRepositorySetup.Object;
            var viewmodel = new ModifyAccountViewModel(accountRepo, new Mock<IDialogService>().Object)
            { IsEdit = false };
            viewmodel.SelectedAccount = new Account { Id = 2, Name = "Sparkonto2" };
=======
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

            var viewmodel = new ModifyAccountViewModel(accountRepo.Object, new Mock<IDialogService>().Object)
            {
                IsEdit = false
            };
            viewmodel.SelectedAccount = newAccount;

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

            var viewmodel = new ModifyAccountViewModel(accountRepo.Object, new Mock<IDialogService>().Object)
            {
                IsEdit = false
            };
            viewmodel.SelectedAccount = account;
>>>>>>> refs/remotes/MoneyFox/master

            viewmodel.SaveCommand.Execute();

            localDateSetting.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            localDateSetting.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
        }
    }
}