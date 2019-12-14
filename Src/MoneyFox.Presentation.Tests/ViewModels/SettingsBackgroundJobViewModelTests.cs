using MoneyFox.Application.Facades;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.Presentation.ViewModels;
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

            // Act
            var vm = new SettingsBackgroundJobViewModel(settingsFacadeMock.Object);
            vm.BackupSyncRecurrence = passedValue;

            // Assert
            Assert.True(taskStarted);
            Assert.Equal(expectedValue, vm.BackupSyncRecurrence);
        }
    }
}
