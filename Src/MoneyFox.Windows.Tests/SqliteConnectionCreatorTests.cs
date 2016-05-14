using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyFox.Shared;
using MoneyFox.Shared.Constants;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using SQLite.Net;
using SQLite.Net.Async;

namespace MoneyFox.Windows.Tests
{
    [TestClass]
    public class SqliteConnectionCreatorTests
    {
        [TestMethod]
        public void GetConnection_CorrectType()
        {
            Assert.IsInstanceOfType(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                new MvxWindowsCommonFileStore()).GetConnection(),
                typeof (SQLiteConnection));
        }

        [TestMethod]
        public void GetAsyncConnection_CorrectType()
        {
            Assert.IsInstanceOfType(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                new MvxWindowsCommonFileStore()).GetAsyncConnection(),
                typeof (SQLiteAsyncConnection));
        }

        [TestMethod]
        public void GetConnection_DbFileCreated()
        {
            new DatabaseManager(new WindowsSqliteConnectionFactory(), new MvxWindowsCommonFileStore());
            
            Assert.IsTrue(new MvxWindowsCommonFileStore().Exists(DatabaseConstants.DB_NAME));
        }
    }
}
