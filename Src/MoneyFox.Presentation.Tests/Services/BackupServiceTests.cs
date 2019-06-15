using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic;
using MoneyFox.BusinessLogic.Backup;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Services;
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
                .ReturnsAsync(OperationResult.Failed(""));

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            var result = await backupService.Login();

            // Assert
            result.Success.ShouldBeFalse();
            settingsFacade.Object.IsBackupAutouploadEnabled.ShouldBeFalse();
            settingsFacade.Object.IsLoggedInToBackupService.ShouldBeFalse();
        }

        [Fact]
        public async Task Login_loginSuccess_SettingsUpdated()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.Login())
                .ReturnsAsync(OperationResult.Succeeded());

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            var result = await backupService.Login();

            // Assert
            result.Success.ShouldBeTrue();
            settingsFacade.Object.IsBackupAutouploadEnabled.ShouldBeTrue();
            settingsFacade.Object.IsLoggedInToBackupService.ShouldBeTrue();
        }

        [Fact]
        public async Task Logout_loginFailed_SettingsNotUpdated()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.Logout())
                .ReturnsAsync(OperationResult.Failed(""));

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();
            settingsFacade.Object.IsBackupAutouploadEnabled = true;
            settingsFacade.Object.IsLoggedInToBackupService = true;

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            var result = await backupService.Logout();

            // Assert
            result.Success.ShouldBeFalse();
            settingsFacade.Object.IsBackupAutouploadEnabled.ShouldBeTrue();
            settingsFacade.Object.IsLoggedInToBackupService.ShouldBeTrue();
        }

        [Fact]
        public async Task Logout_loginSuccess_SettingsUpdated()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.Logout())
                .ReturnsAsync(OperationResult.Succeeded());

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            var result = await backupService.Logout();

            // Assert
            result.Success.ShouldBeTrue();
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
            var result = await backupService.IsBackupExisting();

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
            var result = await backupService.GetBackupDate();

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
                             .ReturnsAsync(OperationResult.Succeeded);

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();
            settingsFacade.Object.LastDatabaseUpdate = expectedPassedDate;

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            OperationResult result = await backupService.RestoreBackup();

            // Assert
            result.Success.ShouldBeTrue();
            settingsFacade.Object.LastDatabaseUpdate.ShouldBeInRange(DateTime.Now.AddMinutes(-2), DateTime.Now);
        }

        [Fact]
        public async Task RestoreBackup_Failed_LastBackupTimestampNotUpdated()
        {
            // Arrange
            DateTime expectedPassedDate = DateTime.Now.AddDays(-3);

            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.RestoreBackup())
                             .ReturnsAsync(OperationResult.Failed(""));

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();
            settingsFacade.Object.LastDatabaseUpdate = expectedPassedDate;

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            OperationResult result = await backupService.RestoreBackup();

            // Assert
            result.Success.ShouldBeFalse();
            settingsFacade.Object.LastDatabaseUpdate.ShouldEqual(expectedPassedDate);
        }

        [Fact]
        public async Task EnqueueBackupTask_NotLoggedIn_Login()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.EnqueueBackupTask(It.IsAny<int>()))
                             .ReturnsAsync(OperationResult.Succeeded);
            backupManagerMock.Setup(x => x.Login())
                             .ReturnsAsync(OperationResult.Succeeded);

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacade.SetupGet(x => x.IsLoggedInToBackupService).Returns(false);

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            OperationResult result = await backupService.EnqueueBackupTask();

            // Assert
            backupManagerMock.Verify(x => x.Login(), Times.Once);
            backupManagerMock.Verify(x => x.EnqueueBackupTask(It.IsAny<int>()), Times.Once);
            result.Success.ShouldBeTrue();
        }

        [Fact]
        public async Task EnqueueBackupTask_NotLoggedIn_RepeatedLogin()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.EnqueueBackupTask(It.IsAny<int>()))
                             .ReturnsAsync(OperationResult.Succeeded);
            backupManagerMock.Setup(x => x.Login())
                             .ReturnsAsync(OperationResult.Failed(""));

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacade.SetupGet(x => x.IsLoggedInToBackupService).Returns(false);

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            OperationResult result = await backupService.EnqueueBackupTask();

            // Assert
            backupManagerMock.Verify(x => x.Login(), Times.Once);
            backupManagerMock.Verify(x => x.EnqueueBackupTask(It.IsAny<int>()), Times.Never);
            result.Success.ShouldBeFalse();
        }

        [Fact]
        public async Task EnqueueBackupTask_LoggedIn_EnqueueCalled()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.EnqueueBackupTask(It.IsAny<int>()))
                             .ReturnsAsync(OperationResult.Succeeded);

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacade.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            OperationResult result = await backupService.EnqueueBackupTask();

            // Assert
            backupManagerMock.Verify(x => x.Login(), Times.Never);
            backupManagerMock.Verify(x => x.EnqueueBackupTask(It.IsAny<int>()), Times.Once);
            result.Success.ShouldBeTrue();
        }
    }
}
