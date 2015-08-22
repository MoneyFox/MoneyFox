using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Repositories;
using MoneyManager.Business.WindowsPhone.Test.Mocks;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Business.WindowsPhone.Test.Repositories
{
    [TestClass]
    public class AccountRepositoryTest
    {
        private AccountDataAccessMock _accountDataAccessMock;

        [TestInitialize]
        public void Init()
        {
            _accountDataAccessMock = new AccountDataAccessMock();
        }

        [TestMethod]
        public void AccountRepository_Save()
        {
            var account = new Account
            {
                Name = "Sparkonto",
                CurrentBalanceWithoutExchange = 6034,
                CurrentBalance = 6034,
                Currency = "CHF"
            };

            _accountDataAccessMock.Save(account);

            Assert.IsTrue(account == _accountDataAccessMock.AccountTestList[0]);
        }

        [TestMethod]
        public void AccountRepository_SaveWithoutName()
        {
            var repository = new AccountRepository(_accountDataAccessMock);

            var account = new Account
            {
                CurrentBalanceWithoutExchange = 6034,
                CurrentBalance = 6034,
                Currency = "CHF"
            };

            repository.Save(account);

            Assert.AreSame(account, _accountDataAccessMock.AccountTestList[0]);
            Assert.IsTrue(_accountDataAccessMock.AccountTestList[0].Name ==
                          Translation.GetTranslation("NoNamePlaceholderLabel"));
        }

        [TestMethod]
        public void AccountRepository_AccessCache()
        {
            Assert.IsNotNull(new AccountRepository(_accountDataAccessMock).Data);
        }

        [TestMethod]
        public void AccountRepository_Delete()
        {
            var repository = new AccountRepository(_accountDataAccessMock);

            var account = new Account
            {
                Name = "Sparkonto",
                CurrentBalanceWithoutExchange = 6034,
                CurrentBalance = 6034,
                Currency = "CHF"
            };

            repository.Save(account);

            Assert.IsTrue(account == _accountDataAccessMock.AccountTestList[0]);

            repository.Delete(account);

            Assert.IsFalse(_accountDataAccessMock.AccountTestList.Any());
            Assert.IsFalse(repository.Data.Any());
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void AccountRepository_Update()
        {
            var repository = new AccountRepository(new AccountDataAccess());

            var account = new Account
            {
                Name = "Sparkonto",
                CurrentBalanceWithoutExchange = 6034,
                CurrentBalance = 6034,
                Currency = "CHF"
            };

            repository.Save(account);

            Assert.IsTrue(account == repository.Data[0]);

            account.Name = "newName";

            repository.Save(account);

            Assert.AreEqual(1, repository.Data.Count());
            Assert.AreEqual("newName", repository.Data[0].Name);
        }
    }
}