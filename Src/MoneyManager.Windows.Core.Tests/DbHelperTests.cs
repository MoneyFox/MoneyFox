using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Core;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Core.Tests.Helper;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace MoneyManager.Windows.Core.Tests
{
    [TestClass]
    public class DbHelperTests
    {
        [TestMethod]
        [TestCategory("Integration")]
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

        [TestMethod]
        [TestCategory("Integration")]
        public void SqlConnectionFactory_GetSqlConnectionWithouthParams()
        {
            var connection = new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath()).GetSqlConnection();
            Assert.IsInstanceOfType(connection, typeof (SQLiteConnection));
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void SqlConnectionFactory_GetSqlConnectionWithParams()
        {
            var connection = new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath()).GetSqlConnection();

            Assert.IsInstanceOfType(connection, typeof (SQLiteConnection));
        }
    }
}