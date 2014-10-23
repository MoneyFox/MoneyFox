using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MoneyManager.WindowsPhone.Test.Src
{
    [TestClass]
    internal class ConnectionFactoryTest
    {
        [TestMethod]
        public void GetDbConnectionTest()
        {
            var connection = ConnectionFactory.GetDbConnection();
            Assert.IsInstanceOfType(connection, typeof (SQLiteConnection));
            Assert.IsTrue(connection.DatabasePath.Contains("moneymanager.sqlite"));
        }

        [TestMethod]
        public void GetAsyncDbConnectionTest()
        {
            var connection = ConnectionFactory.GetAsyncDbConnection();
            Assert.IsInstanceOfType(connection, typeof (SQLiteAsyncConnection));
        }
    }
}