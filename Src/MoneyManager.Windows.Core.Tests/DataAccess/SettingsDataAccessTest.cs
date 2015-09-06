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
            Assert.Equal(-1, settings.DefaultAccount);

            settings.DefaultAccount = 3;
            Assert.Equal(3, settings.DefaultAccount);

            var settings2 = new SettingDataAccess();
            Assert.Equal(-1, settings2.DefaultAccount);
        }
    }
}