using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.Foundation;
using MoneyFox.ServiceLayer.Facades;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.Facades
{
    [ExcludeFromCodeCoverage]
    public class SettingsFacadeTests {
        private ISettingsAdapter settingsAdapter;

        public SettingsFacadeTests() {
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
            var settingsFacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsFacade.IsBackupAutouploadEnabled.ShouldBeFalse();
        }

        [Fact]
        public void Ctor_DefaultValues_SessionTimestampEmpty()
        {
            // Arrange
            
            // Act
            var settingsFacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsFacade.SessionTimestamp.ShouldBeEmpty();
        }

        [Fact]
        public void Ctor_DefaultValues_PasswordRequiredFalse()
        {
            // Arrange
            
            // Act
            var settingsFacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsFacade.PasswordRequired.ShouldBeFalse();
        }

        [Fact]
        public void Ctor_DefaultValues_LastDatabaseUpdateMinDate()
        {
            // Arrange
            
            // Act
            var settingsFacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsFacade.LastDatabaseUpdate.ShouldEqual(DateTime.MinValue);
        }

        [Fact]
        public void Ctor_DefaultValues_ThemeLight()
        {
            // Arrange
            
            // Act
            var settingsFacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsFacade.Theme.ShouldEqual(AppTheme.Light);
        }

        [Fact]
        public void Ctor_DefaultValues_IsLoggedInToBackupServiceFalse()
        {
            // Arrange
            
            // Act
            var settingsFacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsFacade.IsLoggedInToBackupService.ShouldBeFalse();
        }

        [Fact]
        public void Ctor_DefaultValues_BackupSyncRecurrenceThreeHours()
        {
            // Arrange
            
            // Act
            var settingsFacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsFacade.BackupSyncRecurrence.ShouldEqual(3);
        }

        [Fact]
        public void Ctor_DefaultValues_LastExecutionTimeStampSyncBackupMinDate()
        {
            // Arrange
            
            // Act
            var settingsFacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsFacade.LastExecutionTimeStampSyncBackup.ShouldEqual(DateTime.MinValue);
        }

        [Fact]
        public void Ctor_DefaultValues_LastExecutionTimeStampClearPaymentsMinDate()
        {
            // Arrange
            
            // Act
            var settingsFacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsFacade.LastExecutionTimeStampClearPayments.ShouldEqual(DateTime.MinValue);
        }

        [Fact]
        public void Ctor_DefaultValues_LastExecutionTimeStampRecurringPaymentsMinDate()
        {
            // Arrange
            
            // Act
            var settingsFacade = new SettingsFacade(settingsAdapter);

            // Assert
            settingsFacade.LastExecutionTimeStampRecurringPayments.ShouldEqual(DateTime.MinValue);
        }
    }
}