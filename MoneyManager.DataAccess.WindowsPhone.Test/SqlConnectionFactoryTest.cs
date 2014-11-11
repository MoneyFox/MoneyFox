#region

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

#endregion

namespace MoneyManager.DataAccess.WindowsPhone.Test
{
    [TestClass]
    public class SqlConnectionFactoryTest
    {
        [TestMethod]
        public void GetSqlConnectionWithouthParamsTest()
        {
            var connection = SqlConnectionFactory.GetSqlConnection();

            Assert.IsInstanceOfType(connection, typeof (SQLiteConnection));
        }

        [TestMethod]
        public void GetSqlConnectionWithParamsTest()
        {
            var connection = SqlConnectionFactory.GetSqlConnection(new SQLitePlatformWinRT());

            Assert.IsInstanceOfType(connection, typeof (SQLiteConnection));
        }
    }
}