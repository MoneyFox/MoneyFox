using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Core.DataAccess;

namespace MoneyManager.Windows.Core.Tests.DataAccess
{
    [TestClass]
    public class SettingsDataAccessTest
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void SettingsDataAccess_CrudSettings()
        {
            var settings = new SettingDataAccess();
            Assert.AreEqual("USD", settings.DefaultCurrency);

            settings.DefaultCurrency = "CHF";
            Assert.AreEqual("CHF", settings.DefaultCurrency);

            var settings2 = new SettingDataAccess();
            Assert.AreEqual("CHF", settings2.DefaultCurrency);
        }
    }
}