using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace MoneyManager.DataAccess.WindowsPhone.Test {
    [TestClass]
    public class SqlConnectionFactoryTest {
        [TestMethod]
        public void SqlConnectionFactory_GetSqlConnectionWithouthParams() {
            SQLiteConnection connection = SqlConnectionFactory.GetSqlConnection();

            Assert.IsInstanceOfType(connection, typeof (SQLiteConnection));
        }

        [TestMethod]
        public void SqlConnectionFactory_GetSqlConnectionWithParams() {
            SQLiteConnection connection = SqlConnectionFactory.GetSqlConnection(new SQLitePlatformWinRT());

            Assert.IsInstanceOfType(connection, typeof (SQLiteConnection));
        }
    }
}