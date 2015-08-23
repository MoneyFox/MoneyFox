using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Core;
using MoneyManager.Core.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Core.Tests.Helper;
using SQLite.Net.Platform.WinRT;

namespace MoneyManager.Windows.Core.Tests.DataAccess
{
    [TestClass]
    public class AccountDataAccessTest
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void AccountDataAccess_CrudAccount()
        {
            var accountDataAccess =
                new AccountDataAccess(new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath()));

            const string firstName = "fooo Name";
            const string secondName = "new Foooo";

            var account = new Account
            {
                CurrentBalance = 20,
                Iban = "CHF20 0000 00000 000000",
                Name = firstName,
                Note = "this is a note"
            };

            accountDataAccess.Save(account);

            accountDataAccess.LoadList();
            var list = accountDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(firstName, list.First().Name);

            account.Name = secondName;

            accountDataAccess.Save(account);

            list = accountDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(secondName, list.First().Name);

            accountDataAccess.Delete(account);

            list = accountDataAccess.LoadList();

            Assert.IsFalse(list.Any());
        }
    }
}