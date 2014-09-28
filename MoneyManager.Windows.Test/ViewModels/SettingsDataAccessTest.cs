using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess;
using MoneyManager.Src;
using System.Threading.Tasks;

namespace MoneyManager.Windows.Test.ViewModels
{
    [TestClass]
    public class SettingsDataAccessTest
    {
        private SettingDataAccess settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        [TestInitialize]
        public async Task InitTests()
        {
            await DatabaseHelper.CreateDatabase();
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