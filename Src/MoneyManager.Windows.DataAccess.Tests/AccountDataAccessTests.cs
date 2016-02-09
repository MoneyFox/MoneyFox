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

        [TestCleanup]
        public void Cleanup()
        {
            var dataAccess = new AccountDataAccess(connectionCreator);
            var list = dataAccess.LoadList();

            foreach (var account in list)
            {
                dataAccess.DeleteItem(account);
            }
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

            Assert.AreEqual(1, account.Id);
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
                Name = "Sparkonto",
                CurrentBalance = 1234
            };

            var account2 = new Account
            {
                Name = "Jugenkonto",
                CurrentBalance = 999
            };

            var dataAccess = new AccountDataAccess(connectionCreator);
            dataAccess.SaveItem(account1);
            dataAccess.SaveItem(account2);

            var resultList = dataAccess.LoadList();

            Assert.AreEqual(2, resultList.Count);
        }
    }
}
