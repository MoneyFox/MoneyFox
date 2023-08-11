namespace MoneyFox.Infrastructure.Tests.DbBackup;

using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Interfaces;
using Domain.Exceptions;
using FluentAssertions;
using Infrastructure.DbBackup.Legacy;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

public class BackupServiceTests
{
    private readonly BackupService backupService;
    private readonly IOneDriveBackupService cloudBackupService;
    private readonly IConnectivityAdapter connectivityAdapter;
    private readonly ISettingsFacade settingsFacade;

    public BackupServiceTests()
    {
        cloudBackupService = Substitute.For<IOneDriveBackupService>();
        settingsFacade = Substitute.For<ISettingsFacade>();
        connectivityAdapter = Substitute.For<IConnectivityAdapter>();
        var appDbMock = Substitute.For<IAppDbContext>();
        var dbPathProvider = Substitute.For<IDbPathProvider>();
        dbPathProvider.GetDbPath().Returns(Path.GetTempFileName());
        backupService = new(
            oneDriveBackupService: cloudBackupService,
            fileStore: Substitute.For<IFileStore>(),
            settingsFacade: settingsFacade,
            connectivity: connectivityAdapter,
            dbPathProvider: dbPathProvider,
            appDbContext: appDbMock);
    }

    [Fact]
    public async Task Login_NotConnected_ExceptionThrown()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(false);

        // Act / Assert
        await Assert.ThrowsAsync<NetworkConnectionException>(async () => await backupService.LoginAsync());
    }

    [Fact]
    public async Task Login_loginFailed_SettingsNotUpdated()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(true);
        cloudBackupService.LoginAsync().ThrowsAsync<BackupException>();

        // Act
        await Assert.ThrowsAsync<BackupException>(async () => await backupService.LoginAsync());

        // Assert
        settingsFacade.IsBackupAutoUploadEnabled.Should().BeFalse();
        settingsFacade.IsLoggedInToBackupService.Should().BeFalse();
    }

    [Fact]
    public async Task Login_loginSuccess_SettingsUpdated()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(true);
        var expectedAutoBackupFlag = settingsFacade.IsBackupAutoUploadEnabled;

        // Act
        await backupService.LoginAsync();

        // Assert
        settingsFacade.IsBackupAutoUploadEnabled.Should().Be(expectedAutoBackupFlag);
        settingsFacade.IsLoggedInToBackupService.Should().BeTrue();
    }

    [Fact]
    public async Task Logout_NotConnected_ExceptionThrown()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(false);

        // Act / Assert
        await Assert.ThrowsAsync<NetworkConnectionException>(async () => await backupService.LogoutAsync());
    }

    [Fact]
    public async Task Logout_loginFailed_SettingsNotUpdated()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(true);
        cloudBackupService.LogoutAsync().ThrowsAsync<BackupException>();
        settingsFacade.IsBackupAutoUploadEnabled = true;
        settingsFacade.IsLoggedInToBackupService = true;

        // Act
        await Assert.ThrowsAsync<BackupException>(async () => await backupService.LogoutAsync());

        // Assert
        settingsFacade.IsBackupAutoUploadEnabled.Should().BeTrue();
        settingsFacade.IsLoggedInToBackupService.Should().BeTrue();
    }

    [Fact]
    public async Task Logout_loginSuccess_SettingsUpdated()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(true);

        // Act
        await backupService.LogoutAsync();

        // Assert
        settingsFacade.IsBackupAutoUploadEnabled.Should().BeFalse();
        settingsFacade.IsLoggedInToBackupService.Should().BeFalse();
    }

    [Fact]
    public async Task IsBackupExisting_NotConnected_ExceptionThrown()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(false);

        // Act
        var result = await backupService.IsBackupExistingAsync();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsBackupExisting_NoNamesFound()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(true);
        cloudBackupService.GetFileNamesAsync().Returns(new List<string>());

        // Act
        var result = await backupService.IsBackupExistingAsync();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsBackupExisting_NamesFound()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(true);
        cloudBackupService.GetFileNamesAsync().Returns(new List<string> { "asd" });

        // Act
        var result = await backupService.IsBackupExistingAsync();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetBackupDate_NotConnected_ExceptionThrown()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(true);

        // Act
        var result = await backupService.GetBackupDateAsync();

        // Assert
        result.Should().Be(DateTime.MinValue);
    }

    [Fact]
    public async Task GetBackupDate_CorrectCall()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(true);
        settingsFacade.IsBackupAutoUploadEnabled.Returns(true);
        settingsFacade.IsLoggedInToBackupService.Returns(true);
        cloudBackupService.GetBackupDateAsync().Returns(DateTime.Today);

        // Act
        var result = await backupService.GetBackupDateAsync();

        // Assert
        result.Should().Be(DateTime.Today);
    }

    [Fact]
    public async Task RestoreBackupAsync_NotConnected_ExceptionThrown()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(false);
        settingsFacade.IsBackupAutoUploadEnabled.Returns(true);
        settingsFacade.IsLoggedInToBackupService.Returns(true);

        // Act / Assert
        await Assert.ThrowsAsync<NetworkConnectionException>(async () => await backupService.RestoreBackupAsync());
    }

    [Fact]
    public async Task RestoreBackup_Success_LastBackupTimestampNotUpdated()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(true);
        var expectedPassedDate = DateTime.Now.AddDays(-3);
        settingsFacade.LastDatabaseUpdate = expectedPassedDate;
        settingsFacade.IsBackupAutoUploadEnabled.Returns(true);
        settingsFacade.IsLoggedInToBackupService.Returns(true);
        cloudBackupService.RestoreAsync().Returns(Substitute.For<Stream>());
        cloudBackupService.GetFileNamesAsync().Returns(new List<string> { "asd" });

        // Act
        await backupService.RestoreBackupAsync();

        // Assert
        settingsFacade.LastDatabaseUpdate.Should().BeBefore(DateTime.Now.AddSeconds(-1));
    }

    [Fact]
    public async Task RestoreBackup_Failed_LastBackupTimestampNotUpdated()
    {
        // Arrange
        connectivityAdapter.IsConnected.Returns(true);
        var expectedPassedDate = DateTime.Now.AddDays(-3);
        settingsFacade.LastDatabaseUpdate = expectedPassedDate;
        settingsFacade.IsBackupAutoUploadEnabled.Returns(true);
        settingsFacade.IsLoggedInToBackupService.Returns(true);
        cloudBackupService.GetBackupDateAsync().Returns(DateTime.Today);
        cloudBackupService.RestoreAsync().ThrowsAsync<BackupException>();

        // Act
        await Assert.ThrowsAsync<BackupException>(async () => await backupService.RestoreBackupAsync());

        // Assert
        settingsFacade.LastDatabaseUpdate.Should().Be(expectedPassedDate);
    }
}
