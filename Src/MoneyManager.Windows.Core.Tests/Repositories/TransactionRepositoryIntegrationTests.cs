using System.Linq;
using MoneyManager.Core.Repositories;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.TestFoundation;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
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
            var dbHelper = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());

            using (var db = dbHelper.GetConnection())
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

            var repository = new TransactionRepository(new TransactionDataAccess(dbHelper),
                new RecurringTransactionDataAccess(dbHelper));

            repository.Data.Any().ShouldBeTrue();
            repository.Data[0].Amount.ShouldBe(999);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void TransactionRepository_Update()
        {
            var dbHelper = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());

            using (var db = dbHelper.GetConnection())
            {
                db.DeleteAll<FinancialTransaction>();
            }

            var repository = new TransactionRepository(new TransactionDataAccess(dbHelper),
                new RecurringTransactionDataAccess(dbHelper));
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

        [Fact]
        [Trait("Category", "Integration")]
        public void LoadRecurringList_ListWithRecurringTransaction()
        {
            var dbHelper = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());

            var transactionDataAccess =
                new TransactionDataAccess(dbHelper);
            var recTransactionDataAccess =
                new RecurringTransactionDataAccess(dbHelper);
            var repository = new TransactionRepository(transactionDataAccess, recTransactionDataAccess);

            transactionDataAccess.SaveItem(new FinancialTransaction {Amount = 999, IsRecurring = false});
            transactionDataAccess.SaveItem(new FinancialTransaction
            {
                Amount = 123,
                IsRecurring = true,
                RecurringTransaction = new RecurringTransaction {IsEndless = true}
            });

            repository.Load();
            var result = repository.LoadRecurringList().ToList();

            result.Count.ShouldBe(1);

            result.First().Id.ShouldBe(2);
            result.First().RecurringTransaction.Id.ShouldBe(1);
        }
    }
}