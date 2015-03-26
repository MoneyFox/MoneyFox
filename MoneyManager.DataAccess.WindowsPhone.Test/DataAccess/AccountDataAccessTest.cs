#region

using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.Model;
using SQLite.Net;

#endregion

namespace MoneyManager.DataAccess.WindowsPhone.Test.DataAccess {
    [TestClass]
    public class AccountDataAccessTest {
        [TestInitialize]
        public void InitTests() {
            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                db.CreateTable<Account>();
            }
        }

        [TestMethod]
        public void CrudAccountTest() {
            var accountDataAccess = new AccountDataAccess();

            const string firstName = "fooo Name";
            const string secondName = "new Foooo";

            var account = new Account {
                CurrentBalance = 20,
                Iban = "CHF20 0000 00000 000000",
                Name = firstName,
                Note = "this is a note"
            };

            accountDataAccess.Save(account);

            accountDataAccess.LoadList();
            ObservableCollection<Account> list = accountDataAccess.AllAccounts;

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(firstName, list.First().Name);

            account.Name = secondName;

            accountDataAccess.Update(account);

            accountDataAccess.LoadList();
            list = accountDataAccess.AllAccounts;

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(secondName, list.First().Name);

            accountDataAccess.Delete(account);

            accountDataAccess.LoadList();
            list = accountDataAccess.AllAccounts;

            Assert.IsFalse(list.Any());
        }
    }
}