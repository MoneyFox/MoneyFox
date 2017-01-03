using System;
using Cheesebaron.MvxPlugins.Settings.Wpf;
using MoneyFox.Business.Manager;
using Xunit;
using XunitShouldExtension;

namespace MoneyFox.Business.Tests.Manager
{
    public class SettingsManagerTests
    {
        [Fact]
        public void DefaultAccount_DefaultValue()
        {
            new SettingsManager(new Settings()).DefaultAccount.ShouldBe(-1);
        }

        [Fact]
        public void ShowCashFlowOnMainTile_DefaultValue()
        {
            new SettingsManager(new Settings()).ShowCashFlowOnMainTile.ShouldBeTrue();
        }

        [Fact]
        public void IsBackupAutouploadEnabled_DefaultValue()
        {
            new SettingsManager(new Settings()).IsBackupAutouploadEnabled.ShouldBeFalse();
        }

        [Fact]
        public void SessionTimestamp_DefaultValue()
        {
            new SettingsManager(new Settings()).SessionTimestamp.ShouldBe(string.Empty);
        }

        [Fact]
        public void LastDatabaseUpdate_DefaultValue()
        {
            new SettingsManager(new Settings()).LastDatabaseUpdate.ShouldBe(DateTime.MinValue);
        }

        [Fact]
        public void PasswordRequired_DefaultValue()
        {
            new SettingsManager(new Settings()).PasswordRequired.ShouldBeFalse();
        }

        [Fact]
        public void IsDarkThemeSelected_DefaultValue()
        {
            new SettingsManager(new Settings()).IsDarkThemeSelected.ShouldBeFalse();
        }

        [Fact]
        public void IsLoggedInToBackupService_DefaultValue()
        {
            new SettingsManager(new Settings()).IsLoggedInToBackupService.ShouldBeFalse();
        }
    }
}