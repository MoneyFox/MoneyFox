using System;
using System.Collections.Generic;
using System.Linq;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.Tests.Mocks;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.Repositories {
    [TestClass]
    public class AccountRepositoryTests : MvxIoCSupportingTest {
        private DateTime localDateSetting;

        [TestInitialize]
        public void Init() {
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
        public void Save_InputName_CorrectNameAssigned() {
            var testList = new List<Account>();
            const string nameInput = "Sparkonto";

            var accountDataAccessSetup = new Mock<IDataAccess<Account>>();
            accountDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Account>()))
                .Callback((Account acc) => testList.Add(acc));

            accountDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var accountRepository = new AccountRepository(accountDataAccessSetup.Object,
                new Mock<INotificationService>().Object);

            var account = new Account {
                Name = nameInput,
                CurrentBalance = 6034
            };

            accountRepository.Save(account);

            Assert.AreSame(testList[0], account);
            Assert.AreSame(testList[0].Name, account.Name);
        }

        [TestMethod]
        public void Save_EmptyName_CorrectDefault() {
            var testList = new List<Account>();
            const string nameInput = "";

            var accountDataAccessSetup = new Mock<IDataAccess<Account>>();
            accountDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Account>()))
                .Callback((Account acc) => testList.Add(acc));

            accountDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var accountRepository = new AccountRepository(accountDataAccessSetup.Object,
                new Mock<INotificationService>().Object);

            var account = new Account {
                Name = nameInput,
                CurrentBalance = 6034
            };

            accountRepository.Save(account);

            Assert.AreSame(testList[0], account);
            Assert.AreSame(testList[0].Name, account.Name);
        }

        [TestMethod]
        public void AccessCache() {
            Assert.IsNotNull(new AccountRepository(new AccountDataAccessMock(),
                new Mock<INotificationService>().Object).Data);
        }

        [TestMethod]
        public void Delete_None_AccountDeleted() {
            var testList = new List<Account>();

            var accountDataAccessSetup = new Mock<IDataAccess<Account>>();
            accountDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Account>()))
                .Callback((Account acc) => testList.Add(acc));

            var account = new Account {
                Name = "Sparkonto",
                CurrentBalance = 6034
            };

            accountDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account> {
                account
            });

            var repository = new AccountRepository(accountDataAccessSetup.Object,
                new Mock<INotificationService>().Object);

            repository.Delete(account);

            Assert.IsFalse(testList.Any());
            Assert.IsFalse(repository.Data.Any());
        }

        [TestMethod]
        public void Load_AccountDataAccess_DataInitialized() {
            var accountDataAccessSetup = new Mock<IDataAccess<Account>>();
            accountDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account> {
                new Account {Id = 10},
                new Account {Id = 15}
            });

            var accountRepository = new AccountRepository(accountDataAccessSetup.Object,
                new Mock<INotificationService>().Object);
            accountRepository.Load();

            Assert.IsTrue(accountRepository.Data.Any(x => x.Id == 10));
            Assert.IsTrue(accountRepository.Data.Any(x => x.Id == 15));
        }

        [TestMethod]
        public void Save_NotifyUserOfFailure() {
            var isNotificationServiceCalled = false;

            var dataAccessSetup = new Mock<IDataAccess<Account>>();
            dataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Account>())).Returns(false);
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var notificationServiceSetup = new Mock<INotificationService>();
            notificationServiceSetup.Setup(x => x.SendBasicNotification(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string x, string y) => isNotificationServiceCalled = true);

            new AccountRepository(dataAccessSetup.Object,
                notificationServiceSetup.Object).Save(new Account());

            isNotificationServiceCalled.ShouldBeTrue();
        }

        [TestMethod]
        public void Delete_NotifyUserOfFailure() {
            var isNotificationServiceCalled = false;

            var dataAccessSetup = new Mock<IDataAccess<Account>>();
            dataAccessSetup.Setup(x => x.DeleteItem(It.IsAny<Account>())).Returns(false);
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var notificationServiceSetup = new Mock<INotificationService>();
            notificationServiceSetup.Setup(x => x.SendBasicNotification(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string x, string y) => isNotificationServiceCalled = true);

            new AccountRepository(dataAccessSetup.Object,
                notificationServiceSetup.Object).Delete(new Account());

            isNotificationServiceCalled.ShouldBeTrue();
        }
    }
}