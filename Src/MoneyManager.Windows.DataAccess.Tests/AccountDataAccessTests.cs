using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyManager.Windows.DataAccess.Tests
{
    [TestClass]
    public class AccountDataAccessTests
    {
        private SqliteConnectionCreator connectionCreator;

        [TestInitialize]
        public void Init()
        {
            connectionCreator = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());
        }

        [TestMethod]
        public void SaveToDatabase_NewAccount_CorrectId()
        {
            var name = "Sparkonto";
            var balance = 456468;

            var account = new Account
            {
                Name = name,
                CurrentBalance = balance
            };

            new AccountDataAccess(connectionCreator).SaveItem(account);

            Assert.IsTrue(account.Id >= 1);
            Assert.AreEqual(name, account.Name);
            Assert.AreEqual(balance, account.CurrentBalance);
        }

        [TestMethod]
        public void SaveToDatabase_ExistingAccount_CorrectId()
        {
            var balance = 456468;

            var account = new Account
            {
                CurrentBalance = balance
            };

            var dataAccess = new AccountDataAccess(connectionCreator);
            dataAccess.SaveItem(account);

            Assert.IsNull(account.Name);

            var id = account.Id;

            var name = "Sparkonto";
            account.Name = name;

            Assert.AreEqual(id, account.Id);
            Assert.AreEqual(name, account.Name);
            Assert.AreEqual(balance, account.CurrentBalance);
        }

        [TestMethod]
        public void SaveToDatabase_MultipleAccounts_AllSaved()
        {
            var account1 = new Account
            {
                Name = "Account1",
                CurrentBalance = 1234
            };

            var account2 = new Account
            {
                Name = "Account2",
                CurrentBalance = 999
            };

            var account3 = new Account();
            var account4 = new Account();

            var dataAccess = new AccountDataAccess(connectionCreator);
            dataAccess.SaveItem(account1);
            dataAccess.SaveItem(account2);
            dataAccess.SaveItem(account3);
            dataAccess.SaveItem(account4);

            var resultList = dataAccess.LoadList();

            Assert.AreNotSame(account1, account2);
            Assert.IsTrue(resultList.Any(x => x.Id == account1.Id && x.Name == account1.Name));
            Assert.IsTrue(resultList.Any(x => x.Id == account2.Id && x.Name == account2.Name));
        }

        [TestMethod]
        public void SaveToDatabase_CreateAndUpdateAccount_CorrectlyUpdated()
        {
            var firstName = "old name";
            var secondName = "new name";

            var account = new Account
            {
                Name = firstName,
                CurrentBalance = 1234
            };

            var dataAccess = new AccountDataAccess(connectionCreator);
            dataAccess.SaveItem(account);

            Assert.AreEqual(firstName, dataAccess.LoadList().FirstOrDefault(x => x.Id == account.Id).Name);

            account.Name = secondName;
            dataAccess.SaveItem(account);

            var accounts = dataAccess.LoadList();
            Assert.IsFalse(accounts.Any(x => x.Name == firstName));
            Assert.AreEqual(secondName, accounts.First(x => x.Id == account.Id).Name);
        }

        [TestMethod]
        public void DeleteFromDatabase_AccountToDelete_CorrectlyDelete()
        {
            var account = new Account
            {
                Name = "accountToDelete",
                CurrentBalance = 1234
            };

            var dataAccess = new AccountDataAccess(connectionCreator);
            dataAccess.SaveItem(account);

            Assert.IsTrue(dataAccess.LoadList(x => x.Id == account.Id).Any());

            dataAccess.DeleteItem(account);
            Assert.IsFalse(dataAccess.LoadList(x => x.Id == account.Id).Any());
        }
    }
}
