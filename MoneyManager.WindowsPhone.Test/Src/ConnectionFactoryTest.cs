using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Src;
using SQLite;

namespace MoneyManager.WindowsPhone.Test.Src
{
    [TestClass]
    public class ConnectionFactoryTest
    {
        [TestMethod]
        public void GetDbConnectionTest()
        {
            var connection = ConnectionFactory.GetDbConnection();
            Assert.IsInstanceOfType(connection, typeof(SQLiteConnection));
            Assert.IsTrue(connection.DatabasePath.Contains("moneymanager.sqlite"));
        }

        [TestMethod]
        public void GetAsyncDbConnectionTest()
        {
            var connection = ConnectionFactory.GetAsyncDbConnection();
            Assert.IsInstanceOfType(connection, typeof(SQLiteAsyncConnection));
        }
    }
}