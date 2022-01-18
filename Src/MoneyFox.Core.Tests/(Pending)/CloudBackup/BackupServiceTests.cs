using FluentAssertions;
using MoneyFox.Core._Pending_.Common.Constants;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.FileStore;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.DbBackup;
using MoneyFox.Core._Pending_.Exceptions;
using MoneyFox.Core.Interfaces;
using MoneyFox.Infrastructure.DbBackup;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests._Pending_.CloudBackup
{
    [ExcludeFromCodeCoverage]
    public class BackupServiceTests
    {
        private readonly Mock<ICloudBackupService> cloudBackupServiceMock;
        private readonly Mock<IFileStore> fileStoreMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IConnectivityAdapter> connectivityAdapterMock;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        private readonly IToastService toastService;

        public BackupServiceTests()
        {
            cloudBackupServiceMock = new Mock<ICloudBackupService>();
            cloudBackupServiceMock.SetupGet(x => x.UserAccount).Returns(new UserAccount());

            fileStoreMock = new Mock<IFileStore>();
            settingsFacadeMock = new Mock<ISettingsFacade>();
            connectivityAdapterMock = new Mock<IConnectivityAdapter>();
            contextAdapterMock = new Mock<IContextAdapter>();

            toastService = Substitute.For<IToastService>();
        }

        [Fact]
        public async Task Login_NotConnected_ExceptionThrown()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(false);

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act / Assert
            await Assert.ThrowsAsync<NetworkConnectionException>(async () => await backupService.LoginAsync());
        }

        [Fact]
        public async Task Login_loginFailed_SettingsNotUpdated()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);

            cloudBackupServiceMock.Setup(x => x.LoginAsync())
                                  .Callback(() => throw new BackupException());

            settingsFacadeMock.SetupAllProperties();

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.LoginAsync());

            // Assert
            settingsFacadeMock.Object.IsBackupAutouploadEnabled.Should().BeFalse();
            settingsFacadeMock.Object.IsLoggedInToBackupService.Should().BeFalse();
        }

        [Fact]
        public async Task Login_loginSuccess_SettingsUpdated()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);

            cloudBackupServiceMock.Setup(x => x.LoginAsync()).Returns(Task.CompletedTask);

            settingsFacadeMock.SetupAllProperties();

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            await backupService.LoginAsync();

            // Assert
            settingsFacadeMock.Object.IsBackupAutouploadEnabled.Should().BeTrue();
            settingsFacadeMock.Object.IsLoggedInToBackupService.Should().BeTrue();
        }

        [Fact]
        public async Task Logout_NotConnected_ExceptionThrown()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(false);

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act / Assert
            await Assert.ThrowsAsync<NetworkConnectionException>(async () => await backupService.LogoutAsync());
        }

        [Fact]
        public async Task Logout_loginFailed_SettingsNotUpdated()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);

            cloudBackupServiceMock.Setup(x => x.LogoutAsync())
                                  .Callback(() => throw new BackupException());

            settingsFacadeMock.SetupAllProperties();
            settingsFacadeMock.Object.IsBackupAutouploadEnabled = true;
            settingsFacadeMock.Object.IsLoggedInToBackupService = true;

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.LogoutAsync());

            // Assert
            settingsFacadeMock.Object.IsBackupAutouploadEnabled.Should().BeTrue();
            settingsFacadeMock.Object.IsLoggedInToBackupService.Should().BeTrue();
        }

        [Fact]
        public async Task Logout_loginSuccess_SettingsUpdated()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);

            cloudBackupServiceMock.Setup(x => x.LogoutAsync()).Returns(Task.CompletedTask);

            settingsFacadeMock.SetupAllProperties();

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            await backupService.LogoutAsync();

            // Assert
            settingsFacadeMock.Object.IsBackupAutouploadEnabled.Should().BeFalse();
            settingsFacadeMock.Object.IsLoggedInToBackupService.Should().BeFalse();
        }

        [Fact]
        public async Task IsBackupExisting_NotConnected_ExceptionThrown()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(false);

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            bool result = await backupService.IsBackupExistingAsync();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsBackupExisting_NoNamesFound()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);

            cloudBackupServiceMock.Setup(x => x.GetFileNamesAsync())
                                  .ReturnsAsync(new List<string>());

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            bool result = await backupService.IsBackupExistingAsync();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsBackupExisting_NamesFound()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);

            cloudBackupServiceMock.Setup(x => x.GetFileNamesAsync())
                                  .ReturnsAsync(new List<string> {"asd"});

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            bool result = await backupService.IsBackupExistingAsync();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetBackupDate_NotConnected_ExceptionThrown()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(false);

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            DateTime result = await backupService.GetBackupDateAsync();

            // Assert
            result.Should().Be(DateTime.MinValue);
        }

        [Fact]
        public async Task GetBackupDate_CorrectCall()
        {
            // Arrange
            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacadeMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);

            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);
            cloudBackupServiceMock.Setup(x => x.GetBackupDateAsync()).ReturnsAsync(DateTime.Today);

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            DateTime result = await backupService.GetBackupDateAsync();

            // Assert
            result.Should().Be(DateTime.Today);
        }

        [Fact]
        public async Task RestoreBackupAsync_NotConnected_ExceptionThrown()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(false);

            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacadeMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act / Assert
            await Assert.ThrowsAsync<NetworkConnectionException>(async () => await backupService.RestoreBackupAsync());
        }

        [Fact]
        public async Task RestoreBackup_Success_LastBackupTimestampNotUpdated()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);

            DateTime expectedPassedDate = DateTime.Now.AddDays(-3);

            settingsFacadeMock.SetupAllProperties();
            settingsFacadeMock.Object.LastDatabaseUpdate = expectedPassedDate;
            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacadeMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);

            cloudBackupServiceMock.Setup(x => x.RestoreAsync(It.IsAny<string>(), It.IsAny<string>()))
                                  .ReturnsAsync(new Mock<Stream>().Object);

            cloudBackupServiceMock.Setup(x => x.GetFileNamesAsync())
                                  .ReturnsAsync(new List<string> {"asd"});

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            await backupService.RestoreBackupAsync();

            // Assert
            settingsFacadeMock.Object.LastDatabaseUpdate
                              .Should()
                              .BeBefore(DateTime.Now.AddSeconds(-1));
        }

        [Fact]
        public async Task RestoreBackup_Failed_LastBackupTimestampNotUpdated()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);

            DateTime expectedPassedDate = DateTime.Now.AddDays(-3);
            settingsFacadeMock.SetupAllProperties();
            settingsFacadeMock.Object.LastDatabaseUpdate = expectedPassedDate;
            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacadeMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);

            cloudBackupServiceMock.Setup(x => x.GetFileNamesAsync())
                                  .ReturnsAsync(new List<string> {DatabaseConstants.BACKUP_NAME});

            cloudBackupServiceMock.Setup(x => x.RestoreAsync(It.IsAny<string>(), It.IsAny<string>()))
                                  .Callback(() => throw new BackupException());

            cloudBackupServiceMock.Setup(x => x.GetBackupDateAsync()).ReturnsAsync(DateTime.Now);

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.RestoreBackupAsync());

            // Assert
            settingsFacadeMock.Object.LastDatabaseUpdate.Should().Be(expectedPassedDate);
        }

        [Fact]
        public async Task UploadBackupAsync_NotLoggedIn_Login()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);

            cloudBackupServiceMock.Setup(x => x.UploadAsync(It.IsAny<Stream>())).ReturnsAsync(true);
            cloudBackupServiceMock.Setup(x => x.LoginAsync()).Returns(Task.CompletedTask);

            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacadeMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(false);

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            await backupService.UploadBackupAsync();

            // Assert
            cloudBackupServiceMock.Verify(x => x.LoginAsync(), Times.Once);
            cloudBackupServiceMock.Verify(x => x.UploadAsync(It.IsAny<Stream>()), Times.Once);
        }

        [Fact]
        public async Task UploadBackupAsync_NotLoggedIn_RepeatedLogin()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);

            cloudBackupServiceMock.Setup(x => x.UploadAsync(It.IsAny<Stream>())).ReturnsAsync(true);
            cloudBackupServiceMock.Setup(x => x.LoginAsync())
                                  .Callback(() => throw new BackupException());

            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacadeMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(false);

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.UploadBackupAsync());

            // Assert
            cloudBackupServiceMock.Verify(x => x.LoginAsync(), Times.Once);
            cloudBackupServiceMock.Verify(x => x.UploadAsync(It.IsAny<Stream>()), Times.Never);
        }

        [Fact]
        public async Task UploadBackupAsync_LoggedIn_EnqueueCalled()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);

            cloudBackupServiceMock.Setup(x => x.UploadAsync(It.IsAny<Stream>())).ReturnsAsync(true);

            settingsFacadeMock.SetupGet(x => x.IsBackupAutouploadEnabled).Returns(true);
            settingsFacadeMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);

            var backupService = new BackupService(
                cloudBackupServiceMock.Object,
                fileStoreMock.Object,
                settingsFacadeMock.Object,
                connectivityAdapterMock.Object,
                contextAdapterMock.Object,
                toastService);

            // Act
            await backupService.UploadBackupAsync();

            // Assert
            cloudBackupServiceMock.Verify(x => x.LoginAsync(), Times.Never);
            cloudBackupServiceMock.Verify(x => x.UploadAsync(It.IsAny<Stream>()), Times.Once);
        }
    }
}