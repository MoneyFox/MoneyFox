using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using Xunit;

namespace MoneyFox.Windows.Tests.Dependencies
{
    public class WindowsCommonSettingsTests
    {
        [Fact]
        public void Ctor()
        {
            var settings = new WindowsUwpSettings();
            Assert.NotNull(settings);
        }

        [Fact]
        public void GetValues_NoValue_ReturnDefault()
        {
            Assert.True(new WindowsUwpSettings().GetValue("Fooo", true));
        }

        [Fact]
        public void GetValues_HasValue_ReturnValue()
        {
            var key = "Foo";
            var value = "abcd";

            var settings = new WindowsUwpSettings();
            settings.AddOrUpdateValue(key, value);

            Assert.Equal(value, settings.GetValue(key, "123"));
        }
    }
}