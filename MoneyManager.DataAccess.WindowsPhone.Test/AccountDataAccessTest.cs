using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using System.Linq;

namespace MoneyManager.DataAccess.WindowsPhone.Test
{
    [TestClass]
    public class AccountDataAccessTest
    {
        [TestMethod]
        public void CrudAccountTest()
        {
            var accountDataAccess = new AccountDataAccess();

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

            var list = accountDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(firstName, list.First());

            account.Name = secondName;

            accountDataAccess.Update(account);

            list = accountDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(secondName, list.First());

            accountDataAccess.Delete(account);

            list = accountDataAccess.LoadList();
            Assert.IsFalse(list.Any());
        }
    }
}