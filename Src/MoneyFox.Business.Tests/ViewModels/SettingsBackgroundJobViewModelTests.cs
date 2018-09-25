using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Interfaces;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
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
            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            var taskStarted = false;
            var backgroundTaskManager = new Mock<IBackgroundTaskManager>();
            backgroundTaskManager.Setup(x => x.StartBackupSyncTask(It.IsAny<int>())).Callback(() => taskStarted = true);

            // Act
            var vm = new SettingsBackgroundJobViewModel(settingsManagerMock.Object, backgroundTaskManager.Object,
                                                        new Mock<IMvxLogProvider>().Object,
                                                        new Mock<IMvxNavigationService>().Object);
            vm.BackupSyncRecurrence = passedValue;

            // Assert
            Assert.True(taskStarted);
            Assert.Equal(expectedValue, vm.BackupSyncRecurrence);
        }

        [Fact]
        public void IsAutoBackupEnabled_StartService()
        {
            // Arrange
            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(false);

            var taskStarted = false;
            var backgroundTaskManager = new Mock<IBackgroundTaskManager>();
            backgroundTaskManager.Setup(x => x.StartBackupSyncTask(It.IsAny<int>())).Callback(() => taskStarted = true);

            // Act
            var vm = new SettingsBackgroundJobViewModel(settingsManagerMock.Object, backgroundTaskManager.Object,
                                                        new Mock<IMvxLogProvider>().Object,
                                                        new Mock<IMvxNavigationService>().Object);
            vm.IsAutoBackupEnabled = true;

            // Assert
            Assert.True(taskStarted);
        }

        [Fact]
        public void IsAutoBackupEnabled_StopService()
        {
            // Arrange
            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);

            var taskStopped = false;
            var backgroundTaskManager = new Mock<IBackgroundTaskManager>();
            backgroundTaskManager.Setup(x => x.StopBackupSyncTask()).Callback(() => taskStopped = true);

            // Act
            var vm = new SettingsBackgroundJobViewModel(settingsManagerMock.Object, backgroundTaskManager.Object,
                                                        new Mock<IMvxLogProvider>().Object,
                                                        new Mock<IMvxNavigationService>().Object);
            vm.IsAutoBackupEnabled = false;

            // Assert
            Assert.True(taskStopped);
        }
    }
}
