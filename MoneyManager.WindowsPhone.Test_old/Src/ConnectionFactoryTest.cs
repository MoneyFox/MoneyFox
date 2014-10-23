using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess;
using SQLite.Net;

namespace MoneyManager.WindowsPhone.Test.Src
{
    [TestClass]
    internal class ConnectionFactoryTest
    {
        [TestMethod]
        public void GetDbConnectionTest()
        {
            var connection = SqlConnectionFactory.GetSqlConnection();
            Assert.IsInstanceOfType(connection, typeof(SQLiteConnection));
            Assert.IsTrue(connection.DatabasePath.Contains("moneymanager.sqlite"));
        }
    }
}