using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using Moq;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.Repositories
{
    [TestClass]
    public class AccountRepositoryTests : MvxIoCSupportingTest
    {

        [TestInitialize]
        public void Init()
        {
            Setup();
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
            Assert.IsFalse(repository.GetList().Any());
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

            Assert.IsTrue(accountRepository.GetList(x => x.Id == 10).Any());
            Assert.IsTrue(accountRepository.GetList(x => x.Id == 15).Any());
        }

        [TestMethod]
        public void FindById_ReturnsAccount()
        {
            var accountDataAccessMock = new Mock<IDataAccess<Account>>();
            var testAccount = new Account {Id = 100, Name = "Test Account"};

            accountDataAccessMock.Setup(x => x.LoadList(null))
                .Returns(new List<Account> {testAccount});

            Assert.AreEqual(testAccount, new AccountRepository(accountDataAccessMock.Object).FindById(100));
        }

        [TestMethod]
        public void Delete_Failure_ReturnFalse()
        {
            var dataAccessSetup = new Mock<IDataAccess<Account>>();
            dataAccessSetup.Setup(x => x.DeleteItem(It.IsAny<Account>())).Returns(false);
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            new AccountRepository(dataAccessSetup.Object).Delete(new Account()).ShouldBeFalse();
        }

        [TestMethod]
        public void Save_Failure_ReturnFalse()
        {
            var dataAccessSetup = new Mock<IDataAccess<Account>>();
            dataAccessSetup.Setup(x => x.DeleteItem(It.IsAny<Account>())).Returns(false);
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            new AccountRepository(dataAccessSetup.Object).Save(new Account()).ShouldBeFalse();
        }
    }
}