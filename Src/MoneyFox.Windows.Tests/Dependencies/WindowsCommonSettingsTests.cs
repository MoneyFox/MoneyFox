using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MoneyFox.Windows.Tests.Dependencies
{
    [TestClass]
    public class WindowsCommonSettingsTests
    {
        [TestMethod]
        public void Ctor()
        {
            var settings = new WindowsUwpSettings();
            Assert.IsNotNull(settings);
        }

        [TestMethod]
        public void GetValues_NoValue_ReturnDefault()
        {
            Assert.AreEqual(true, new WindowsUwpSettings().GetValue("Fooo", true));
        }

        [TestMethod]
        public void GetValues_HasValue_ReturnValue()
        {
            var key = "Foo";
            var value = "abcd";

            var settings = new WindowsUwpSettings();
            settings.AddOrUpdateValue(key, value);

            Assert.AreEqual(value, settings.GetValue(key, "123"));
        }
    }
}