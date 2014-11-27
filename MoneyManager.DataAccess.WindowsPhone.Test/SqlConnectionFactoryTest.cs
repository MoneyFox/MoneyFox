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
            SQLiteConnection connection = SqlConnectionFactory.GetSqlConnection();

            Assert.IsInstanceOfType(connection, typeof (SQLiteConnection));
        }

        [TestMethod]
        public void GetSqlConnectionWithParamsTest()
        {
            SQLiteConnection connection = SqlConnectionFactory.GetSqlConnection(new SQLitePlatformWinRT());

            Assert.IsInstanceOfType(connection, typeof (SQLiteConnection));
        }
    }
}