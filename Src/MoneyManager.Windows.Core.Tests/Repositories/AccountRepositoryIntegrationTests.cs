using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Core;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Repositories;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Core.Tests.Helper;
using SQLite.Net.Platform.WinRT;

namespace MoneyManager.Windows.Core.Tests.Repositories
{
    [TestClass]
    public class AccountRepositoryIntegrationTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void AccountRepository_Update()
        {
            var repository =
                new AccountRepository(
                    new AccountDataAccess(new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath())));

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