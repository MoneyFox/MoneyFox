using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Tests.Mocks;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Tests.Repositories
{
    [TestClass]
    public class AccountRepositoryTests
    {
        [TestMethod]
        public void AccountRepository_Save()
        {
            var accountDataAccessMock = new AccountDataAccessMock();
            var account = new Account
            {
                Name = "Sparkonto",
                CurrentBalanceWithoutExchange = 6034,
                CurrentBalance = 6034,
                Currency = "CHF"
            };

            accountDataAccessMock.Save(account);

            Assert.IsTrue(account == accountDataAccessMock.AccountTestList[0]);
        }

        [TestMethod]
        public void AccountRepository_SaveWithoutName()
        {
            var accountDataAccessMock = new AccountDataAccessMock();
            var repository = new AccountRepository(accountDataAccessMock);

            var account = new Account
            {
                CurrentBalanceWithoutExchange = 6034,
                CurrentBalance = 6034,
                Currency = "CHF"
            };

            repository.Save(account);

            Assert.AreSame(account, accountDataAccessMock.AccountTestList[0]);
            Assert.IsTrue(accountDataAccessMock.AccountTestList[0].Name == Strings.NoNamePlaceholderLabel);
        }

        [TestMethod]
        public void AccountRepository_AccessCache()
        {
            Assert.IsNotNull(new AccountRepository(new AccountDataAccessMock()).Data);
        }

        [TestMethod]
        public void AccountRepository_Delete()
        {
            var accountDataAccessMock = new AccountDataAccessMock();
            var repository = new AccountRepository(accountDataAccessMock);

            var account = new Account
            {
                Name = "Sparkonto",
                CurrentBalanceWithoutExchange = 6034,
                CurrentBalance = 6034,
                Currency = "CHF"
            };

            repository.Save(account);

            Assert.IsTrue(account == accountDataAccessMock.AccountTestList[0]);

            repository.Delete(account);

            Assert.IsFalse(accountDataAccessMock.AccountTestList.Any());
            Assert.IsFalse(repository.Data.Any());
        }
    }
}