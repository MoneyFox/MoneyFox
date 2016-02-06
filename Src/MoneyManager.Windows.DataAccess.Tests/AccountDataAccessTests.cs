using MoneyManager.DataAccess;
using MoneyManager.Foundation.Model;
using Xunit;
using XunitShouldExtension;

namespace MoneyManager.Windows.DataAccess.Tests
{
    public class AccountDataAccessTests
    {
        [Fact]
        public void SaveToDatabase_NewAccount_CorrectId()
        {
            var name = "Sparkonto";
            var balance = 456468;

            var account = new Account
            {
                Name = name,
                CurrentBalance = balance
            };

            new AccountDataAccess(new WindowsSqliteConnectionFactory()).SaveItem(account);

            account.Id.ShouldBeGreaterThanOrEqualTo(1);
            account.Name.ShouldBe(name);
            account.CurrentBalance.ShouldBe(balance);
        }

        [Fact]
        public void SaveToDatabase_ExistingAccount_CorrectId()
        {
            var balance = 456468;

            var account = new Account
            {
                CurrentBalance = balance
            };

            var dataAccess = new AccountDataAccess(new WindowsSqliteConnectionFactory());
            dataAccess.SaveItem(account);

            account.Name.ShouldBeNull();
            var id = account.Id;

            var name = "Sparkonto";
            account.Name = name;

            account.Id.ShouldBe(id);
            account.Name.ShouldBe(name);
            account.CurrentBalance.ShouldBe(balance);
        }
    }
}
