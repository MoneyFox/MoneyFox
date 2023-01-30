namespace MoneyFox.Infrastructure.Tests.DbBackup;

using Core.Common.Facades;
using Core.Common.Interfaces;
using Core.Interfaces;
using Domain.Exceptions;
using FluentAssertions;
using Infrastructure.DbBackup.Legacy;
using Moq;
using NSubstitute;

public class BackupServiceTests
{
    private readonly BackupService backupService;
    private readonly Mock<IOneDriveBackupService> cloudBackupServiceMock;
    private readonly Mock<IConnectivityAdapter> connectivityAdapterMock;
    private readonly Mock<ISettingsFacade> settingsFacadeMock;

    public BackupServiceTests()
    {
        cloudBackupServiceMock = new();
        settingsFacadeMock = new();
        connectivityAdapterMock = new();
        var appDbMock = Substitute.For<IAppDbContext>();
        var dbPathProvider = Substitute.For<IDbPathProvider>();
        dbPathProvider.GetDbPath().Returns(Path.GetTempFileName());
        backupService = new(
            oneDriveBackupService: cloudBackupServiceMock.Object,
            fileStore: new Mock<IFileStore>().Object,
            settingsFacade: settingsFacadeMock.Object,
            connectivity: connectivityAdapterMock.Object,
            dbPathProvider: dbPathProvider,
            appDbContext: appDbMock);
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
