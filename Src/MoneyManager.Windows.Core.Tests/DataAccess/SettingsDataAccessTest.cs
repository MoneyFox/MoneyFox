using MoneyManager.Core.DataAccess;
using MoneyManager.Windows.Core.Tests.Stubs;
using Xunit;

namespace MoneyManager.Windows.Core.Tests.DataAccess
{
    public class SettingsDataAccessTest
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void SettingsDataAccess_CrudSettings()
        {
            var settings = new SettingDataAccess(new RoamingSettingsStub());
            Assert.Equal(-1, settings.DefaultAccount);

            settings.DefaultAccount = 3;
            Assert.Equal(3, settings.DefaultAccount);

            var settings2 = new SettingDataAccess(new RoamingSettingsStub());
            Assert.Equal(-1, settings2.DefaultAccount);
        }
    }
}