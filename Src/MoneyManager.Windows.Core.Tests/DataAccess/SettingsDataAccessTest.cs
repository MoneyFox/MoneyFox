using MoneyManager.Core.DataAccess;
using Xunit;

namespace MoneyManager.Windows.Core.Tests.DataAccess
{
    public class SettingsDataAccessTest
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void SettingsDataAccess_CrudSettings()
        {
            var settings = new SettingDataAccess();
            Assert.Equal("USD", settings.DefaultCurrency);

            settings.DefaultCurrency = "CHF";
            Assert.Equal("CHF", settings.DefaultCurrency);

            var settings2 = new SettingDataAccess();
            Assert.Equal("CHF", settings2.DefaultCurrency);
        }
    }
}