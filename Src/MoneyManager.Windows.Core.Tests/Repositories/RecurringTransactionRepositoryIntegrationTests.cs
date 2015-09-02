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
    public class RecurringTransactionRepositoryIntegrationTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void RecurringTransactionRepository_LoadDataFromDbThroughRepository()
        {
            var dbHelper = new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath());

            using (var db = dbHelper.GetSqlConnection())
            {
                db.DeleteAll<RecurringTransaction>();
                db.InsertWithChildren(new RecurringTransaction
                {
                    Amount = 999,
                    ChargedAccount = new Account
                    {
                        Name = "testAccount"
                    }
                });
            }

            var repository = new RecurringTransactionRepository(new RecurringTransactionDataAccess(dbHelper));


            repository.Data.Any().ShouldBeTrue();
            repository.Data[0].Amount.ShouldBe(999);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void RecurringTransactionRepository_Update()
        {
            var dbHelper = new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath());


            using (var db = dbHelper.GetSqlConnection())
            {
                db.DeleteAll<RecurringTransaction>();
            }

            var repository = new RecurringTransactionRepository(new RecurringTransactionDataAccess(dbHelper));

            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new RecurringTransaction
            {
                ChargedAccount = account,
                Amount = 20
            };


            repository.Save(transaction);
            repository.Data.Count.ShouldBe(1);
            repository.Data[0].ShouldBeSameAs(transaction);

            transaction.Amount = 789;

            repository.Save(transaction);

            repository.Data.Any().ShouldBeTrue();
            repository.Data[0].Amount.ShouldBe(789);
        }
    }
}