using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.ViewModels
{
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
        public void Title_EditAccount_CorrectTitle()
        {
            var accountname = "Sparkonto";

            var settingsManagerMock = new Mock<ISettingsManager>();

            var viewmodel = new ModifyAccountViewModel(new Mock<IAccountRepository>().Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
            {
                IsEdit = true,
                SelectedAccount = new Account {Id = 3, Name = accountname}
            };

            viewmodel.Title.ShouldBe(string.Format(Strings.EditAccountTitle, accountname));
        }

        [TestMethod]
        public void Title_AddAccount_CorrectTitle()
        {
            var viewmodel = new ModifyAccountViewModel(new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object,
                new Mock<ISettingsManager>().Object)
            { IsEdit = false};

            viewmodel.Title.ShouldBe(Strings.AddAccountTitle);
        }

        [TestMethod]
        public void SaveCommand_Does_Not_Allow_Duplicate_Names()
        {
            var accountList = new List<Account>();

            var settingsManagerMock = new Mock<ISettingsManager>();

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.Setup(c => c.GetList(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(accountList);
            accountRepositorySetup.Setup(c => c.Save(It.IsAny<Account>()))
                .Callback((Account acc) => { accountList.Add(acc); });

            var account = new Account
            {
                Id = 1,
                Name = "Test Account"
            };
            var newAccount = new Account
            {
                Name = "Test Account"
            };
            accountList.Add(account);
            
            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
            {
                IsEdit = false,
                SelectedAccount = newAccount
            };

            viewmodel.SaveCommand.Execute();
            Assert.AreEqual(1, accountList.Count());
        }

        [TestMethod]
        public void SaveCommand_Does_Not_Allow_Duplicate_Names2()
        {
            var accountList = new List<Account>();

            var settingsManagerMock = new Mock<ISettingsManager>();

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.Setup(c => c.GetList(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(accountList);
            accountRepositorySetup.Setup(c => c.Save(It.IsAny<Account>()))
                .Callback((Account acc) => { accountList.Add(acc); });

            var account = new Account
            {
                Id = 1,
                Name = "Test Account"
            };
            var newAccount = new Account
            {
                Name = "TESt Account"
            };
            accountList.Add(account);

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
            {
                IsEdit = false,
                SelectedAccount = newAccount
            };

            viewmodel.SaveCommand.Execute();
            Assert.AreEqual(1, accountList.Count);
        }

        [TestMethod]
        public void SaveCommand_SavesAccount()
        {
            var accountList = new List<Account>();

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.Setup(c => c.GetList(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(accountList);
            accountRepositorySetup.Setup(c => c.Save(It.IsAny<Account>()))
                .Callback((Account acc) => { accountList.Add(acc); });

            var settingsManagerMock = new Mock<ISettingsManager>();

            var account = new Account
            {
                Id = 1,
                Name = "Test Account"
            };

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
            {
                IsEdit = false,
                SelectedAccount = account
            };

            viewmodel.SaveCommand.Execute();
            Assert.AreEqual(1, accountList.Count);
        }

        [TestMethod]
        public void Save_UpdateTimeStamp()
        {
            var account = new Account {Id = 0, Name = "account"};

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.Setup(x => x.Save(account)).Returns(true);
            accountRepositorySetup.Setup(x => x.GetList(null)).Returns(() => new List<Account>());

            var localDateSetting = DateTime.MinValue;
            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupSet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>())
                .Callback((DateTime x) => localDateSetting = x);

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
            {
                IsEdit = false,
                SelectedAccount = account
            };

            viewmodel.SaveCommand.Execute();

            localDateSetting.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            localDateSetting.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
        }

        [TestMethod]
        public void Cancel_SelectedAccountReseted()
        {
            string name = "Account";
            var baseAccount = new Account { Id = 5, Name = name };
            var account = new Account { Id = 5, Name = name };

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.Setup(x => x.FindById(It.IsAny<int>())).Returns(baseAccount);

            var settingsManagerMock = new Mock<ISettingsManager>();

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object, new Mock<IDialogService>().Object, settingsManagerMock.Object)
            {
                IsEdit = true,
                SelectedAccount = account
            };

            viewmodel.SelectedAccount.Name = "foooo";
            viewmodel.CancelCommand.Execute();

            viewmodel.SelectedAccount.Name.ShouldBe(name);
        }
    }
}