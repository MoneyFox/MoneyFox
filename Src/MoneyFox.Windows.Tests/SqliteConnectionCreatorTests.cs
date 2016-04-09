using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyFox.Shared;
using MoneyFox.Shared.Constants;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using SQLite.Net;

namespace MoneyFox.Windows.Tests
{
    [TestClass]
    public class SqliteConnectionCreatorTests
    {
        [TestMethod]
        public void GetConnection_CorrectType()
        {
            Assert.IsInstanceOfType(new SqliteConnectionCreator(new WindowsSqliteConnectionFactory()).GetConnection(),
                typeof (SQLiteConnection));
        }

        [TestMethod]
        public void GetConnection_DbFileCreated()
        {
            new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());
            
            Assert.IsTrue(new MvxWindowsCommonFileStore().Exists(OneDriveAuthenticationConstants.DB_NAME));
        }
    }
}
