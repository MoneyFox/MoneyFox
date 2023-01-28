namespace MoneyFox.Ui.Tests.ViewModels;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.UseCases.DbBackup;
using Core.Common.Facades;
using Core.Common.Interfaces;
using Core.Interfaces;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Views.Backup;
using Xunit;


public class BackupViewModelShould
{
    private readonly IBackupService backupService;
    private readonly IConnectivityAdapter connectivityAdapter;
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;
    private readonly IOneDriveProfileService oneDriveProfileService;
    private readonly ISettingsFacade settingsManager;
    private readonly IToastService toastService;

    private readonly BackupViewModel viewModel;

    protected BackupViewModelShould()
    {
        backupService = Substitute.For<IBackupService>();
        oneDriveProfileService = Substitute.For<IOneDriveProfileService>();
        connectivityAdapter = Substitute.For<IConnectivityAdapter>();
        toastService = Substitute.For<IToastService>();
        mediator = Substitute.For<IMediator>();
        settingsManager = Substitute.For<ISettingsFacade>();
        dialogService = Substitute.For<IDialogService>();
        viewModel = new(
            mediator: mediator,
            backupService: backupService,
            dialogService: dialogService,
            connectivity: connectivityAdapter,
            settingsFacade: settingsManager,
            toastService: toastService,
            oneDriveProfileService: oneDriveProfileService);
    }

    public sealed class InitializeCommand : BackupViewModelShould
    {
        [Fact]
        public async Task CallNothing_OnInitialize_WhenDeviceIsDisconnected()
        {
            // Arrange
            _ = connectivityAdapter.IsConnected.Returns(false);

            // Act
            viewModel.InitializeCommand.Execute(null);

            // Assert
            viewModel.IsLoadingBackupAvailability.Should().BeFalse();
            _ = await backupService.Received(0).IsBackupExistingAsync();
            _ = await backupService.Received(0).GetBackupDateAsync();
        }

        [Fact]
        public async Task CallNothing_OnInitialize_WhenNotLoggedIn()
        {
            // Arrange
            _ = connectivityAdapter.IsConnected.Returns(true);

            // Act
            viewModel.InitializeCommand.Execute(null);

            // Assert
            viewModel.IsLoadingBackupAvailability.Should().BeFalse();
            _ = await backupService.Received(0).IsBackupExistingAsync();
            _ = await backupService.Received(0).GetBackupDateAsync();
        }

        [Fact]
        public void CallInitializations_WhenConnectivitySet_AndUserLoggedIn()
        {
            // Arrange
            _ = connectivityAdapter.IsConnected.Returns(true);
            _ = settingsManager.IsLoggedInToBackupService.Returns(true);
            var returnDate = DateTime.Today;
            _ = backupService.IsBackupExistingAsync().Returns(true);
            _ = backupService.GetBackupDateAsync().Returns(returnDate);

            // Act
            viewModel.InitializeCommand.Execute(null);

            // Assert
            viewModel.IsLoadingBackupAvailability.Should().BeFalse();
            viewModel.BackupAvailable.Should().BeTrue();
            viewModel.BackupLastModified.Should().Be(returnDate);
        }
    }

    public class LogoutCommand : BackupViewModelShould
    {
        [Fact]
        public void UpdateSettingsCorrectly_OnLogout()
        {
            // Arrange
            var logoutCommandCalled = false;
            backupService.When(x => x.LogoutAsync()).Do(x => logoutCommandCalled = true);

            // Act
            viewModel.LogoutCommand.Execute(null);

            // Assert
            _ = logoutCommandCalled.Should().BeTrue();
            _ = settingsManager.IsLoggedInToBackupService.Should().BeFalse();
        }
    }
}
