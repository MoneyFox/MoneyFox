using System.Linq;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using Xunit;

namespace MoneyManager.Windows.Core.Tests.DataAccess
{
    public class AccountDataAccessTest
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void AccountDataAccess_CrudAccount()
        {
            var accountDataAccess =
                new AccountDataAccess(new SqliteConnectionCreator(new WindowsSqliteConnectionFactory()));

            const string firstName = "fooo Name";
            const string secondName = "new Foooo";

            var account = new Account
            {
                CurrentBalance = 20,
                Iban = "CHF20 0000 00000 000000",
                Name = firstName,
                Note = "this is a note"
            };

            accountDataAccess.SaveItem(account);

            accountDataAccess.LoadList();
            var list = accountDataAccess.LoadList();

            Assert.Equal(1, list.Count);
            Assert.Equal(firstName, list.First().Name);

            account.Name = secondName;

            accountDataAccess.SaveItem(account);

            list = accountDataAccess.LoadList();

            Assert.Equal(1, list.Count);
            Assert.Equal(secondName, list.First().Name);

            accountDataAccess.DeleteItem(account);

            list = accountDataAccess.LoadList();

            Assert.False(list.Any());
        }
    }
}