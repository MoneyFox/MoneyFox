using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using SQLite.Net;

namespace MoneyManager.WindowsPhone.Test.ViewModels
{
    [TestClass]
    internal class SettingsDataAccessTest
    {
        private SettingDataAccess settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        [TestInitialize]
        public void InitTests()
        {
            DatabaseLogic.CreateDatabase();

            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                dbConn.DeleteAll<Setting>();
                settings.Dbversion = 1;
            }
        }

        [TestMethod]
        public void GetDbVersionTest()
        {
            Assert.AreEqual(settings.Dbversion, 1);
        }

        [TestMethod]
        public void UpdateDbVersion()
        {
            settings.Dbversion = 5;
            Assert.AreEqual(settings.Dbversion, 5);
        }
    }
}