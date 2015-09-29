using System.Linq;
using MoneyManager.Core.Repositories;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using Xunit;

namespace MoneyManager.Windows.Core.Tests.Repositories
{
    public class AccountRepositoryIntegrationTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void AccountRepository_Update()
        {
            const string newName = "newName";

            var repository =
                new AccountRepository(
                    new AccountDataAccess(new SqliteConnectionCreator(new WindowsSqliteConnectionFactory())));

            var account = new Account
            {
                Name = "Sparkonto",
                CurrentBalance = 6034
            };

            repository.Save(account);

            repository.Data[0].ShouldBeSameAs(account);

            account.Name = newName;

            repository.Save(account);

            repository.Data.Count().ShouldBe(1);
            repository.Data[0].Name.ShouldBe(newName);
        }
    }
}