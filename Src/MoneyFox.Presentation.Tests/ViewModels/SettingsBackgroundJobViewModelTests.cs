using System.Diagnostics.CodeAnalysis;
using MoneyFox.Presentation.Interfaces;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.Facades;
using Moq;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class SettingsBackgroundJobViewModelTests
    {
        [Theory]
        [InlineData(5, 5)]
        [InlineData(1, 1)]
        [InlineData(-2, 1)]
        [InlineData(24, 24)]
        [InlineData(48, 48)]
        public void BackupSyncRecurrence(int passedValue, int expectedValue)
        {
            // Arrange
            var settingsFacadeMock = new Mock<ISettingsFacade>();
            settingsFacadeMock.SetupAllProperties();

            var taskStarted = false;
            var backgroundTaskManager = new Mock<IBackgroundTaskManager>();
            backgroundTaskManager.Setup(x => x.StartBackupSyncTask(It.IsAny<int>())).Callback(() => taskStarted = true);

            // Act
            var vm = new SettingsBackgroundJobViewModel(settingsFacadeMock.Object, backgroundTaskManager.Object);
            vm.BackupSyncRecurrence = passedValue;

            // Assert
            Assert.True(taskStarted);
            Assert.Equal(expectedValue, vm.BackupSyncRecurrence);
        }

        [Fact]
        public void IsAutoBackupEnabled_StartService()
        {
            // Arrange
            var settingsFacadeMock = new Mock<ISettingsFacade>();
            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(false);

            var taskStarted = false;
            var backgroundTaskManager = new Mock<IBackgroundTaskManager>();
            backgroundTaskManager.Setup(x => x.StartBackupSyncTask(It.IsAny<int>())).Callback(() => taskStarted = true);

            // Act
            var vm = new SettingsBackgroundJobViewModel(settingsFacadeMock.Object, backgroundTaskManager.Object);
            vm.IsAutoBackupEnabled = true;

            // Assert
            Assert.True(taskStarted);
        }

        [Fact]
        public void IsAutoBackupEnabled_StopService()
        {
            // Arrange
            var settingsFacadeMock = new Mock<ISettingsFacade>();
            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);

            var taskStopped = false;
            var backgroundTaskManager = new Mock<IBackgroundTaskManager>();
            backgroundTaskManager.Setup(x => x.StopBackupSyncTask()).Callback(() => taskStopped = true);

            // Act
            var vm = new SettingsBackgroundJobViewModel(settingsFacadeMock.Object, backgroundTaskManager.Object);
            vm.IsAutoBackupEnabled = false;

            // Assert
            Assert.True(taskStopped);
        }
    }
}
