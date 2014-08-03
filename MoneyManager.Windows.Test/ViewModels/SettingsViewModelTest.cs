using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Src;
using MoneyManager.ViewModels.Data;
using System.Threading.Tasks;

namespace MoneyManager.Windows.Test.ViewModels
{
    [TestClass]
    public class SettingsViewModelTest
    {
        [TestInitialize]
        public async Task InitTests()
        {
            App.Settings = new SettingViewModel();
            await DatabaseHelper.CreateDatabase();
        }

        [TestMethod]
        public void GetDbVersionTest()
        {
            Assert.AreEqual(App.Settings.Dbversion, 1);
        }

        [TestMethod]
        public void UpdateDbVersion()
        {
            App.Settings.Dbversion = 5;
            Assert.AreEqual(App.Settings.Dbversion, 5);
        }
    }
}