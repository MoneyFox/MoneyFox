using System.Linq;
using MoneyManager.Core;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Core.Tests.Helper;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using Xunit;

namespace MoneyManager.Windows.Core.Tests
{
    public class DbHelperTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void DatabaseLogic_CreateDatabase()
        {
            var dbHelper = new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath());

            using (var dbConn = dbHelper.GetSqlConnection())
            {
                var temp1 = dbConn.Table<Account>().ToList();
                var temp2 = dbConn.Table<FinancialTransaction>().ToList();
                var temp3 = dbConn.Table<RecurringTransaction>().ToList();
                var temp4 = dbConn.Table<Category>().ToList();
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void SqlConnectionFactory_GetSqlConnectionWithouthParams()
        {
            Assert.IsType<SQLiteConnection>(
                new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath()).GetSqlConnection());
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void SqlConnectionFactory_GetSqlConnectionWithParams()
        {
            Assert.IsType<SQLiteConnection>(
                new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath()).GetSqlConnection());
        }
    }
}