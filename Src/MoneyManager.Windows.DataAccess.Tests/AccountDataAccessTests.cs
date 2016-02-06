using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using NUnit.Framework;

namespace MoneyManager.Windows.DataAccess.Tests
{
    [TestFixture]
    public class AccountDataAccessTests
    {
        private SqliteConnectionCreator connectionCreator;

        [SetUp]
        public void Init()
        {
            connectionCreator = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());
        }

        [Test]
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

        [Test]
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
    }
}
