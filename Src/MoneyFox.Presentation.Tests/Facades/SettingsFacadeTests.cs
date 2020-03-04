using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Facades;
using Moq;
using Should;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Presentation.Tests.Facades
{
    [ExcludeFromCodeCoverage]
    public class SettingsFacadeTests
    {
        private readonly ISettingsAdapter settingsAdapter;

        public SettingsFacadeTests()
        {
            var settingsAdapterMock = new Mock<ISettingsAdapter>();
            settingsAdapterMock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>()))
                               .Returns((string key, string defaultValue) => defaultValue);
            settingsAdapterMock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<int>()))
                               .Returns((string key, int defaultValue) => defaultValue);
            settingsAdapterMock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<bool>()))
                               .Returns((string key, bool defaultValue) => defaultValue);

            settingsAdapter = settingsAdapterMock.Object;
        }

        [Fact]
        public void Ctor_DefaultValues_IsBackupAutouploadEnabledFalse()
        {
            // Arrange

            // Act
            var settingsfacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsfacade.IsBackupAutouploadEnabled.ShouldBeFalse();
        }

        [Fact]
        public void Ctor_DefaultValues_LastDatabaseUpdateMinDate()
        {
            // Arrange

            // Act
            var settingsfacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsfacade.LastDatabaseUpdate.ShouldEqual(DateTime.MinValue);
        }

        [Fact]
        public void Ctor_DefaultValues_ThemeLight()
        {
            // Arrange

            // Act
            var settingsfacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsfacade.Theme.ShouldEqual(AppTheme.Light);
        }

        [Fact]
        public void Ctor_DefaultValues_IsLoggedInToBackupServiceFalse()
        {
            // Arrange

            // Act
            var settingsfacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsfacade.IsLoggedInToBackupService.ShouldBeFalse();
        }
    }
}
