using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

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
        public async Task InitTests()
        {
            await DatabaseHelper.CreateDatabase();

            using (var dbConn = ConnectionFactory.GetDbConnection())
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