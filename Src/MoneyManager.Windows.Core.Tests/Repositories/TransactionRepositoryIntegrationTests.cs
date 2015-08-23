using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Core;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Repositories;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Core.Tests.Helper;
using SQLite.Net.Platform.WinRT;
using SQLiteNetExtensions.Extensions;

namespace MoneyManager.Windows.Core.Tests.Repositories
{
    [TestClass]
    public class TransactionRepositoryIntegrationTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void TransactionRepository_LoadDataFromDbThroughRepository()
        {
            var dbHelper = new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath());

            using (var db = dbHelper.GetSqlConnection())
            {
                db.DeleteAll<FinancialTransaction>();
                db.InsertWithChildren(new FinancialTransaction
                {
                    Amount = 999,
                    AmountWithoutExchange = 777,
                    ChargedAccount = new Account
                    {
                        Name = "testAccount"
                    }
                });
            }

            var repository = new TransactionRepository(new TransactionDataAccess(dbHelper));

            Assert.IsTrue(repository.Data.Any());
            Assert.AreEqual(999, repository.Data[0].Amount);
            Assert.AreEqual(777, repository.Data[0].AmountWithoutExchange);
        }

        [TestMethod]
        [TestCategory("Integration")]
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
                Amount = 20,
                AmountWithoutExchange = 20
            };

            repository.Save(transaction);
            Assert.AreEqual(1, repository.Data.Count);
            Assert.AreSame(transaction, repository.Data[0]);

            transaction.Amount = 30;

            repository.Save(transaction);

            Assert.AreEqual(1, repository.Data.Count);
            Assert.AreEqual(30, repository.Data[0].Amount);
        }
    }
}