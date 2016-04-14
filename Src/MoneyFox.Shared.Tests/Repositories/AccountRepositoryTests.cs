using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.Tests.Mocks;
using Moq;
using MvvmCross.Test.Core;
using MvvmCross.Platform;

namespace MoneyFox.Shared.Tests.Repositories
{
    [TestClass]
    public class AccountRepositoryTests : MvxIoCSupportingTest
    {
        private DateTime localDateSetting;

        [TestInitialize]
        public void Init()
        {
            Setup();

            // We setup the static setting classes here for the general usage in the app
            var settingsMockSetup = new Mock<ILocalSettings>();
            settingsMockSetup.SetupAllProperties();
            settingsMockSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Callback((string key, DateTime date) => localDateSetting = date);

            var roamSettingsMockSetup = new Mock<IRoamingSettings>();
            roamSettingsMockSetup.SetupAllProperties();

            Mvx.RegisterType(() => settingsMockSetup.Object);
            Mvx.RegisterType(() => roamSettingsMockSetup.Object);
        }

        [TestMethod]
        public void Save_InputName_CorrectNameAssigned()
        {
            var testList = new List<Account>();
            const string nameInput = "Sparkonto";

            var accountDataAccessSetup = new Mock<IDataAccess<Account>>();
            accountDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Account>()))
                .Callback((Account acc) => testList.Add(acc));

            accountDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var accountRepository = new AccountRepository(accountDataAccessSetup.Object);

            var account = new Account
            {
                Name = nameInput,
                CurrentBalance = 6034
            };

            accountRepository.Save(account);

            Assert.AreSame(testList[0], account);
            Assert.AreSame(testList[0].Name, account.Name);
        }

        [TestMethod]
        public void Save_EmptyName_CorrectDefault()
        {
            var testList = new List<Account>();
            const string nameInput = "";

            var accountDataAccessSetup = new Mock<IDataAccess<Account>>();
            accountDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Account>()))
                .Callback((Account acc) => testList.Add(acc));

            accountDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var accountRepository = new AccountRepository(accountDataAccessSetup.Object);

            var account = new Account
            {
                Name = nameInput,
                CurrentBalance = 6034
            };

            accountRepository.Save(account);

            Assert.AreSame(testList[0], account);
            Assert.AreSame(testList[0].Name, account.Name);
        }

        [TestMethod]
        public void AccessCache()
        {
            Assert.IsNotNull(new AccountRepository(new AccountDataAccessMock()).Data);
        }

        [TestMethod]
        public void Delete_None_AccountDeleted()
        {
            var testList = new List<Account>();

            var accountDataAccessSetup = new Mock<IDataAccess<Account>>();
            accountDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Account>()))
                .Callback((Account acc) => testList.Add(acc));

            var account = new Account
            {
                Name = "Sparkonto",
                CurrentBalance = 6034
            };

            accountDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>
            {
                account
            });

            var repository = new AccountRepository(accountDataAccessSetup.Object);

            repository.Delete(account);

            Assert.IsFalse(testList.Any());
            Assert.IsFalse(repository.Data.Any());
        }

        [TestMethod]
        public void Load_AccountDataAccess_DataInitialized()
        {
            var accountDataAccessSetup = new Mock<IDataAccess<Account>>();
            accountDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>
            {
                new Account {Id = 10},
                new Account {Id = 15}
            });

            var accountRepository = new AccountRepository(accountDataAccessSetup.Object);
            accountRepository.Load();

            Assert.IsTrue(accountRepository.Data.Any(x => x.Id == 10));
            Assert.IsTrue(accountRepository.Data.Any(x => x.Id == 15));
        }

        [TestMethod]
        public void Save_UpdateTimeStamp()
        {
            var dataAccessSetup = new Mock<IDataAccess<Category>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Category>());

            new CategoryRepository(dataAccessSetup.Object).Save(new Category());
<<<<<<< HEAD
            localDateSetting.ShouldBeInRange(DateTime.Now.AddSeconds(-1), DateTime.Now.AddSeconds(1));
=======
            _localDateSetting.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            _localDateSetting.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
>>>>>>> unitTesting
        }
    }
}