namespace MoneyFox.Tests.Core._Pending_.CloudBackup
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core._Pending_.Common.Facades;
    using MoneyFox.Core.ApplicationCore.Domain.Exceptions;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Core.Interfaces;
    using MoneyFox.Infrastructure.DbBackup;
    using MoneyFox.Infrastructure.DbBackup.Legacy;
    using Moq;
    using NSubstitute;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class BackupServiceTests
    {
        private readonly Mock<IOneDriveBackupService> cloudBackupServiceMock;
        private readonly Mock<IFileStore> fileStoreMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IConnectivityAdapter> connectivityAdapterMock;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        private readonly IDbPathProvider dbPathProvider;
        private readonly IToastService toastService;

        public BackupServiceTests()
        {
            cloudBackupServiceMock = new Mock<IOneDriveBackupService>();
            fileStoreMock = new Mock<IFileStore>();
            settingsFacadeMock = new Mock<ISettingsFacade>();
            connectivityAdapterMock = new Mock<IConnectivityAdapter>();
            contextAdapterMock = new Mock<IContextAdapter>();
            toastService = Substitute.For<IToastService>();
            dbPathProvider = Substitute.For<IDbPathProvider>();
            dbPathProvider.GetDbPath().Returns(Path.GetTempFileName());
        }

        [Fact]
        public async Task Login_NotConnected_ExceptionThrown()
        {
            // Arrange
            connectivityAdapterMock.SetupGet(x => x.IsConnected).Returns(false);
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

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
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

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
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

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
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

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
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

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
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

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
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

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
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

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
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

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
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

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
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

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
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

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
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

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
            var backupService = new BackupService(
                oneDriveBackupService: cloudBackupServiceMock.Object,
                fileStore: fileStoreMock.Object,
                settingsFacade: settingsFacadeMock.Object,
                connectivity: connectivityAdapterMock.Object,
                contextAdapter: contextAdapterMock.Object,
                toastService: toastService,
                dbPathProvider: dbPathProvider);

            // Act
            await Assert.ThrowsAsync<BackupException>(async () => await backupService.RestoreBackupAsync());

            // Assert
            settingsFacadeMock.Object.LastDatabaseUpdate.Should().Be(expectedPassedDate);
        }
    }

}
