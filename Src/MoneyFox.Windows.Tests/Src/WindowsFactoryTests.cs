using System.IO;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyFox.Windows.Tests
{
    [TestClass]
    public class WindowsFactoryTests
    {
        [TestMethod]
        public void GetPlattformDatabasePath_Dbname_CorrectPath()
        {
            const string dbname = "test";
            Assert.AreEqual(Path.Combine(ApplicationData.Current.LocalFolder.Path, dbname),
                new WindowsSqliteConnectionFactory().GetPlattformDatabasePath(dbname));
        }
    }
}