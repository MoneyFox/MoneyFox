namespace MoneyFox.Tests.Infrastructure.DbBackup
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Exceptions;
    using MoneyFox.Core.Common.Facades;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Core.Interfaces;
    using MoneyFox.Infrastructure.DbBackup.Legacy;
    using Moq;
    using NSubstitute;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class BackupServiceTests
    {
        private readonly BackupService backupService;
        private readonly Mock<IOneDriveBackupService> cloudBackupServiceMock;
        private readonly Mock<IConnectivityAdapter> connectivityAdapterMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;

        public BackupServiceTests()
        {
            cloudBackupServiceMock = new Mock<IOneDriveBackupService>();
            settingsFacadeMock = new Mock<ISettingsFacade>();
            connectivityAdapterMock = new Mock<IConnectivityAdapter>();
            var dbPathProvider = Substitute.For<IDbPathProvider>();
            dbPathProvider.GetDbPath().Returns(Path.GetTempFileName());
            backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: new Mock<IFileStore>().Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                toastService: Substitute.For<IToastService>(),
                dbPathProvider: dbPathProvider);
        }

        [Fact]
        public async Task Login_NotConnected_ExceptionThrown()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(false);

            // Act / Assert
            await Assert.ThrowsAsync<NetworkConnectionException>(async () => await backupService.LoginAsync());
        }

        [Fact]
        public async Task Login_loginFailed_SettingsNotUpdated()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);
            cloudBackupServiceMock.Setup(x => x.LoginAsync()).Callback(() => throw new BackupException());
            settingsFacadeMock.SetupAllProperties();

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.LoginAsync());

            // Assert
            settingsFacadeMock.Object.IsBackupAutoUploadEnabled.Should().BeFalse();
            settingsFacadeMock.Object.IsLoggedInToBackupService.Should().BeFalse();
        }

        [Fact]
        public async Task Login_loginSuccess_SettingsUpdated()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);
            cloudBackupServiceMock.Setup(x => x.LoginAsync()).Returns(Task.CompletedTask);
            settingsFacadeMock.SetupAllProperties();
            var expectedAutoBackupFlag = settingsFacadeMock.Object.IsBackupAutoUploadEnabled;

            // Act
            await backupService.LoginAsync();

            // Assert
            settingsFacadeMock.Object.IsBackupAutoUploadEnabled.Should().Be(expectedAutoBackupFlag);
            settingsFacadeMock.Object.IsLoggedInToBackupService.Should().BeTrue();
        }

        [Fact]
        public async Task Logout_NotConnected_ExceptionThrown()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(false);

            // Act / Assert
            await Assert.ThrowsAsync<NetworkConnectionException>(async () => await backupService.LogoutAsync());
        }

        [Fact]
        public async Task Logout_loginFailed_SettingsNotUpdated()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);
            cloudBackupServiceMock.Setup(x => x.LogoutAsync()).Callback(() => throw new BackupException());
            settingsFacadeMock.SetupAllProperties();
            settingsFacadeMock.Object.IsBackupAutoUploadEnabled = true;
            settingsFacadeMock.Object.IsLoggedInToBackupService = true;

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.LogoutAsync());

            // Assert
            settingsFacadeMock.Object.IsBackupAutoUploadEnabled.Should().BeTrue();
            settingsFacadeMock.Object.IsLoggedInToBackupService.Should().BeTrue();
        }

        [Fact]
        public async Task Logout_loginSuccess_SettingsUpdated()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);
            cloudBackupServiceMock.Setup(x => x.LogoutAsync()).Returns(Task.CompletedTask);
            settingsFacadeMock.SetupAllProperties();

            // Act
            await backupService.LogoutAsync();

            // Assert
            settingsFacadeMock.Object.IsBackupAutoUploadEnabled.Should().BeFalse();
            settingsFacadeMock.Object.IsLoggedInToBackupService.Should().BeFalse();
        }

        [Fact]
        public async Task IsBackupExisting_NotConnected_ExceptionThrown()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(false);

            // Act
            var result = await backupService.IsBackupExistingAsync();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsBackupExisting_NoNamesFound()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);
            cloudBackupServiceMock.Setup(x => x.GetFileNamesAsync()).ReturnsAsync(new List<string>());

            // Act
            var result = await backupService.IsBackupExistingAsync();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsBackupExisting_NamesFound()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);
            cloudBackupServiceMock.Setup(x => x.GetFileNamesAsync()).ReturnsAsync(new List<string> { "asd" });

            // Act
            var result = await backupService.IsBackupExistingAsync();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetBackupDate_NotConnected_ExceptionThrown()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(false);

            // Act
            var result = await backupService.GetBackupDateAsync();

            // Assert
            result.Should().Be(DateTime.MinValue);
        }

        [Fact]
        public async Task GetBackupDate_CorrectCall()
        {
            // Arrange
            settingsFacadeMock.SetupGet(x => x.IsBackupAutoUploadEnabled).Returns(true);
            settingsFacadeMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);
            cloudBackupServiceMock.Setup(x => x.GetBackupDateAsync()).ReturnsAsync(DateTime.Today);

            // Act
            var result = await backupService.GetBackupDateAsync();

            // Assert
            result.Should().Be(DateTime.Today);
        }

        [Fact]
        public async Task RestoreBackupAsync_NotConnected_ExceptionThrown()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(false);
            settingsFacadeMock.SetupGet(x => x.IsBackupAutoUploadEnabled).Returns(true);
            settingsFacadeMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);

            // Act / Assert
            await Assert.ThrowsAsync<NetworkConnectionException>(async () => await backupService.RestoreBackupAsync());
        }

        [Fact]
        public async Task RestoreBackup_Success_LastBackupTimestampNotUpdated()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);
            var expectedPassedDate = DateTime.Now.AddDays(-3);
            settingsFacadeMock.SetupAllProperties();
            settingsFacadeMock.Object.LastDatabaseUpdate = expectedPassedDate;
            settingsFacadeMock.SetupGet(x => x.IsBackupAutoUploadEnabled).Returns(true);
            settingsFacadeMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);
            cloudBackupServiceMock.Setup(x => x.RestoreAsync()).ReturnsAsync(new Mock<Stream>().Object);
            cloudBackupServiceMock.Setup(x => x.GetFileNamesAsync()).ReturnsAsync(new List<string> { "asd" });

            // Act
            await backupService.RestoreBackupAsync();

            // Assert
            settingsFacadeMock.Object.LastDatabaseUpdate.Should().BeBefore(DateTime.Now.AddSeconds(-1));
        }

        [Fact]
        public async Task RestoreBackup_Failed_LastBackupTimestampNotUpdated()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(true);
            var expectedPassedDate = DateTime.Now.AddDays(-3);
            settingsFacadeMock.SetupAllProperties();
            settingsFacadeMock.Object.LastDatabaseUpdate = expectedPassedDate;
            settingsFacadeMock.SetupGet(x => x.IsBackupAutoUploadEnabled).Returns(true);
            settingsFacadeMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);
            cloudBackupServiceMock.Setup(x => x.RestoreAsync()).Callback(() => throw new BackupException());
            cloudBackupServiceMock.Setup(x => x.GetBackupDateAsync()).ReturnsAsync(DateTime.Now);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.RestoreBackupAsync());

            // Assert
            settingsFacadeMock.Object.LastDatabaseUpdate.Should().Be(expectedPassedDate);
        }
    }

}
