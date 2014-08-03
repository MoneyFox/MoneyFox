using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using MoneyManager.ViewModels.Data;
using System.Threading.Tasks;

namespace MoneyManager.Windows.Test.ViewModels
{
    [TestClass]
    public class SettingsViewModelTest
    {
        private SettingViewModel settings
        {
            get { return new ViewModelLocator().Settings; }
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