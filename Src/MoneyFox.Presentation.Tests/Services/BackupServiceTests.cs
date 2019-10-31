using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic.Backup;
using MoneyFox.Domain.Exceptions;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class BackupServiceTests
    {
        [Fact]
        public async Task Login_loginFailed_SettingsNotUpdated()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.LoginAsync())
                             .Callback(() => throw new BackupException());

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.LoginAsync());

            // Assert
            settingsFacade.Object.IsBackupAutouploadEnabled.ShouldBeFalse();
            settingsFacade.Object.IsLoggedInToBackupService.ShouldBeFalse();
        }

        [Fact]
        public async Task Login_loginSuccess_SettingsUpdated()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.LoginAsync())
                             .Returns(Task.CompletedTask);

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await backupService.LoginAsync();

            // Assert
            settingsFacade.Object.IsBackupAutouploadEnabled.ShouldBeTrue();
            settingsFacade.Object.IsLoggedInToBackupService.ShouldBeTrue();
        }

        [Fact]
        public async Task Logout_loginFailed_SettingsNotUpdated()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.LogoutAsync())
                             .Callback(() => throw new BackupException());

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();
            settingsFacade.Object.IsBackupAutouploadEnabled = true;
            settingsFacade.Object.IsLoggedInToBackupService = true;

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.LogoutAsync());

            // Assert
            settingsFacade.Object.IsBackupAutouploadEnabled.ShouldBeTrue();
            settingsFacade.Object.IsLoggedInToBackupService.ShouldBeTrue();
        }

        [Fact]
        public async Task Logout_loginSuccess_SettingsUpdated()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.LogoutAsync())
                             .Returns(Task.CompletedTask);

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await backupService.LogoutAsync();

            // Assert
            settingsFacade.Object.IsBackupAutouploadEnabled.ShouldBeFalse();
            settingsFacade.Object.IsLoggedInToBackupService.ShouldBeFalse();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task IsBackupExisting_CorrectCall(bool expectedResult)
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.IsBackupExistingAsync())
                             .ReturnsAsync(expectedResult);

            var backupService = new BackupService(backupManagerMock.Object, new Mock<ISettingsFacade>().Object);

            // Act
            bool result = await backupService.IsBackupExistingAsync();

            // Assert
            result.ShouldEqual(expectedResult);
        }

        [Fact]
        public async Task GetBackupDate_CorrectCall()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.GetBackupDateAsync())
                             .ReturnsAsync(DateTime.Today);

            var backupService = new BackupService(backupManagerMock.Object, new Mock<ISettingsFacade>().Object);

            // Act
            DateTime result = await backupService.GetBackupDateAsync();

            // Assert
            result.ShouldEqual(DateTime.Today);
        }

        [Fact]
        public async Task RestoreBackup_Success_LastBackupTimestampNotUpdated()
        {
            // Arrange
            DateTime expectedPassedDate = DateTime.Now.AddDays(-3);

            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.RestoreBackupAsync())
                             .Returns(Task.CompletedTask);

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();
            settingsFacade.Object.LastDatabaseUpdate = expectedPassedDate;

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await backupService.RestoreBackupAsync();

            // Assert
            settingsFacade.Object.LastDatabaseUpdate.ShouldBeInRange(DateTime.Now.AddMinutes(-2), DateTime.Now);
        }

        [Fact]
        public async Task RestoreBackup_Failed_LastBackupTimestampNotUpdated()
        {
            // Arrange
            DateTime expectedPassedDate = DateTime.Now.AddDays(-3);

            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.RestoreBackupAsync())
                             .Callback(() => throw new BackupException());

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();
            settingsFacade.Object.LastDatabaseUpdate = expectedPassedDate;

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.RestoreBackupAsync());

            // Assert
            settingsFacade.Object.LastDatabaseUpdate.ShouldEqual(expectedPassedDate);
        }

        [Fact]
        public async Task EnqueueBackupTask_NotLoggedIn_Login()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.EnqueueBackupTaskAsync(It.IsAny<int>()))
                             .Returns(Task.CompletedTask);
            backupManagerMock.Setup(x => x.LoginAsync())
                             .Returns(Task.CompletedTask);

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacade.SetupGet(x => x.IsLoggedInToBackupService).Returns(false);

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await backupService.EnqueueBackupTaskAsync();

            // Assert
            backupManagerMock.Verify(x => x.LoginAsync(), Times.Once);
            backupManagerMock.Verify(x => x.EnqueueBackupTaskAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task EnqueueBackupTask_NotLoggedIn_RepeatedLogin()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.EnqueueBackupTaskAsync(It.IsAny<int>()))
                             .Returns(Task.CompletedTask);
            backupManagerMock.Setup(x => x.LoginAsync())
                             .Callback(() => throw new BackupException());

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacade.SetupGet(x => x.IsLoggedInToBackupService).Returns(false);

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.EnqueueBackupTaskAsync());

            // Assert
            backupManagerMock.Verify(x => x.LoginAsync(), Times.Once);
            backupManagerMock.Verify(x => x.EnqueueBackupTaskAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task EnqueueBackupTask_LoggedIn_EnqueueCalled()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.EnqueueBackupTaskAsync(It.IsAny<int>()))
                             .Returns(Task.CompletedTask);

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacade.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await backupService.EnqueueBackupTaskAsync();

            // Assert
            backupManagerMock.Verify(x => x.LoginAsync(), Times.Never);
            backupManagerMock.Verify(x => x.EnqueueBackupTaskAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
