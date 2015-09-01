using System.Linq;
using MoneyManager.Core;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Repositories;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Core.Tests.Helper;
using SQLite.Net.Platform.WinRT;
using SQLiteNetExtensions.Extensions;
using Xunit;

namespace MoneyManager.Windows.Core.Tests.Repositories
{
    public class TransactionRepositoryIntegrationTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void TransactionRepository_LoadDataFromDbThroughRepository()
        {
            var dbHelper = new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath());

            using (var db = dbHelper.GetSqlConnection())
            {
                db.DeleteAll<FinancialTransaction>();
                db.InsertWithChildren(new FinancialTransaction
                {
                    Amount = 999,
                    ChargedAccount = new Account
                    {
                        Name = "testAccount"
                    }
                });
            }

            var repository = new TransactionRepository(new TransactionDataAccess(dbHelper));

            repository.Data.Any().ShouldBeTrue();
            repository.Data[0].Amount.ShouldBe(999);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void TransactionRepository_Update()
        {
            var dbHelper = new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath());

            using (var db = dbHelper.GetSqlConnection())
            {
                db.DeleteAll<FinancialTransaction>();
            }

            var repository = new TransactionRepository(new TransactionDataAccess(dbHelper));
            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new FinancialTransaction
            {
                ChargedAccount = account,
                Amount = 20
            };

            repository.Save(transaction);
            repository.Data.Count.ShouldBe(1);
            repository.Data[0].ShouldBeSameAs(transaction);

            transaction.Amount = 30;

            repository.Save(transaction);

            repository.Data.Count.ShouldBe(1);
            repository.Data[0].Amount.ShouldBe(30);
        }
    }
}