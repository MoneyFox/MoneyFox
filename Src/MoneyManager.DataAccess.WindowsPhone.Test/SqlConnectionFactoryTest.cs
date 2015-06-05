using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace MoneyManager.DataAccess.WindowsPhone.Test
{
    [TestClass]
    public class SqlConnectionFactoryTest
    {
        [TestMethod]
        public void SqlConnectionFactory_GetSqlConnectionWithouthParams()
        {
            var connection = SqlConnectionFactory.GetSqlConnection();

            Assert.IsInstanceOfType(connection, typeof (SQLiteConnection));
        }

        [TestMethod]
        public void SqlConnectionFactory_GetSqlConnectionWithParams()
        {
            var connection = SqlConnectionFactory.GetSqlConnection(new SQLitePlatformWinRT());

            Assert.IsInstanceOfType(connection, typeof (SQLiteConnection));
        }
    }
}