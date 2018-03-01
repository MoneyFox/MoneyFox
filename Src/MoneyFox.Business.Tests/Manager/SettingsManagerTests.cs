using System;
using Cheesebaron.MvxPlugins.Settings.Wpf;
using MoneyFox.Business.Manager;
using MoneyFox.Foundation;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.Manager
{
    public class SettingsManagerTests
    {
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
            new SettingsManager(new Settings()).SessionTimestamp.ShouldEqual(string.Empty);
        }

        [Fact]
        public void LastDatabaseUpdate_DefaultValue()
        {
            new SettingsManager(new Settings()).LastDatabaseUpdate.ShouldEqual(DateTime.MinValue);
        }

        [Fact]
        public void PasswordRequired_DefaultValue()
        {
            new SettingsManager(new Settings()).PasswordRequired.ShouldBeFalse();
        }

        [Fact]
        public void Theme_DefaultValue()
        {
            Assert.Equal(AppTheme.Dark, new SettingsManager(new Settings()).Theme);
        }
    }
}