#region

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess.DataAccess;

#endregion

namespace MoneyManager.DataAccess.WindowsPhone.Test.DataAccess {
    [TestClass]
    public class SettingsDataAccessTest {
        [TestMethod]
        public void CrudSettingsTest() {
            var settings = new SettingDataAccess();
            Assert.AreEqual("USD", settings.DefaultCurrency);

            settings.DefaultCurrency = "CHF";
            Assert.AreEqual("CHF", settings.DefaultCurrency);

            var settings2 = new SettingDataAccess();
            Assert.AreEqual("CHF", settings2.DefaultCurrency);
        }
    }
}