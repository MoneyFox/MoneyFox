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
            backupManagerMock.Setup(x => x.Login())
                             .Callback(() => throw new BackupException());

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.Login());

            // Assert
            settingsFacade.Object.IsBackupAutouploadEnabled.ShouldBeFalse();
            settingsFacade.Object.IsLoggedInToBackupService.ShouldBeFalse();
        }

        [Fact]
        public async Task Login_loginSuccess_SettingsUpdated()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.Login())
                             .Returns(Task.CompletedTask);

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await backupService.Login();

            // Assert
            settingsFacade.Object.IsBackupAutouploadEnabled.ShouldBeTrue();
            settingsFacade.Object.IsLoggedInToBackupService.ShouldBeTrue();
        }

        [Fact]
        public async Task Logout_loginFailed_SettingsNotUpdated()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.Logout())
                             .Callback(() => throw new BackupException());

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();
            settingsFacade.Object.IsBackupAutouploadEnabled = true;
            settingsFacade.Object.IsLoggedInToBackupService = true;

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.Logout());

            // Assert
            settingsFacade.Object.IsBackupAutouploadEnabled.ShouldBeTrue();
            settingsFacade.Object.IsLoggedInToBackupService.ShouldBeTrue();
        }

        [Fact]
        public async Task Logout_loginSuccess_SettingsUpdated()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.Logout())
                             .Returns(Task.CompletedTask);

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await backupService.Logout();

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
            backupManagerMock.Setup(x => x.IsBackupExisting())
                             .ReturnsAsync(expectedResult);

            var backupService = new BackupService(backupManagerMock.Object, new Mock<ISettingsFacade>().Object);

            // Act
            bool result = await backupService.IsBackupExisting();

            // Assert
            result.ShouldEqual(expectedResult);
        }

        [Fact]
        public async Task GetBackupDate_CorrectCall()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.GetBackupDate())
                             .ReturnsAsync(DateTime.Today);

            var backupService = new BackupService(backupManagerMock.Object, new Mock<ISettingsFacade>().Object);

            // Act
            DateTime result = await backupService.GetBackupDate();

            // Assert
            result.ShouldEqual(DateTime.Today);
        }

        [Fact]
        public async Task RestoreBackup_Success_LastBackupTimestampNotUpdated()
        {
            // Arrange
            DateTime expectedPassedDate = DateTime.Now.AddDays(-3);

            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.RestoreBackup())
                             .Returns(Task.CompletedTask);

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();
            settingsFacade.Object.LastDatabaseUpdate = expectedPassedDate;

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await backupService.RestoreBackup();

            // Assert
            settingsFacade.Object.LastDatabaseUpdate.ShouldBeInRange(DateTime.Now.AddMinutes(-2), DateTime.Now);
        }

        [Fact]
        public async Task RestoreBackup_Failed_LastBackupTimestampNotUpdated()
        {
            // Arrange
            DateTime expectedPassedDate = DateTime.Now.AddDays(-3);

            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.RestoreBackup())
                             .Callback(() => throw new BackupException());

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();
            settingsFacade.Object.LastDatabaseUpdate = expectedPassedDate;

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.RestoreBackup());

            // Assert
            settingsFacade.Object.LastDatabaseUpdate.ShouldEqual(expectedPassedDate);
        }

        [Fact]
        public async Task EnqueueBackupTask_NotLoggedIn_Login()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.EnqueueBackupTask(It.IsAny<int>()))
                             .Returns(Task.CompletedTask);
            backupManagerMock.Setup(x => x.Login())
                             .Returns(Task.CompletedTask);

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacade.SetupGet(x => x.IsLoggedInToBackupService).Returns(false);

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await backupService.EnqueueBackupTask();

            // Assert
            backupManagerMock.Verify(x => x.Login(), Times.Once);
            backupManagerMock.Verify(x => x.EnqueueBackupTask(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task EnqueueBackupTask_NotLoggedIn_RepeatedLogin()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.EnqueueBackupTask(It.IsAny<int>()))
                             .Returns(Task.CompletedTask);
            backupManagerMock.Setup(x => x.Login())
                             .Callback(() => throw new BackupException());

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacade.SetupGet(x => x.IsLoggedInToBackupService).Returns(false);

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.EnqueueBackupTask());

            // Assert
            backupManagerMock.Verify(x => x.Login(), Times.Once);
            backupManagerMock.Verify(x => x.EnqueueBackupTask(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task EnqueueBackupTask_LoggedIn_EnqueueCalled()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.EnqueueBackupTask(It.IsAny<int>()))
                             .Returns(Task.CompletedTask);

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacade.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            await backupService.EnqueueBackupTask();

            // Assert
            backupManagerMock.Verify(x => x.Login(), Times.Never);
            backupManagerMock.Verify(x => x.EnqueueBackupTask(It.IsAny<int>()), Times.Once);
        }
    }
}
