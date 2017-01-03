using Cheesebaron.MvxPlugins.Settings.WindowsCommon;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MoneyFox.Windows.Tests.Dependencies
{
    [TestClass]
    public class WindowsCommonSettingsTests
    {
        [TestMethod]
        public void Ctor()
        {
            var settings = new WindowsCommonSettings();
            Assert.IsNotNull(settings);
        }

        [TestMethod]
        public void GetValues_NoValue_ReturnDefault()
        {
            Assert.AreEqual(true, new WindowsCommonSettings().GetValue("Fooo", true));
        }

        [TestMethod]
        public void GetValues_HasValue_ReturnValue()
        {
            var key = "Foo";
            var value = "abcd";

            var settings = new WindowsCommonSettings();
            settings.AddOrUpdateValue(key, value);

            Assert.AreEqual(value, settings.GetValue(key, "123"));
        }
    }
}