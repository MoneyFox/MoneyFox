namespace MoneyFox.Tests.Presentation.ViewModels
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MediatR;
    using MoneyFox.Core.ApplicationCore.UseCases.DbBackup;
    using MoneyFox.Core.Common.Facades;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Core.Interfaces;
    using MoneyFox.ViewModels.DataBackup;
    using NSubstitute;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class BackupViewModelShould
    {
        private readonly IBackupService backupService;
        private readonly IOneDriveProfileService oneDriveProfileService;
        private readonly IConnectivityAdapter connectivityAdapter;
        private readonly IDialogService dialogService;
        private readonly IMediator mediator;
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

            viewModel = new BackupViewModel(
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
                connectivityAdapter.IsConnected.Returns(false);

                // Act
                viewModel.InitializeCommand.Execute(null);

                // Assert
                viewModel.IsLoadingBackupAvailability.Should().BeFalse();
                await backupService.Received(0).IsBackupExistingAsync();
                await backupService.Received(0).GetBackupDateAsync();
            }

            [Fact]
            public async Task CallNothing_OnInitialize_WhenNotLoggedIn()
            {
                // Arrange
                connectivityAdapter.IsConnected.Returns(true);

                // Act
                viewModel.InitializeCommand.Execute(null);

                // Assert
                viewModel.IsLoadingBackupAvailability.Should().BeFalse();
                await backupServiceMock.Received(0).IsBackupExistingAsync();
                await backupServiceMock.Received(0).GetBackupDateAsync();
            }

            [Fact]
            public void CallInitializations_WhenConnectivitySet_AndUserLoggedIn()
            {
                // Arrange
                connectivityAdapter.IsConnected.Returns(true);
                settingsManager.IsLoggedInToBackupService.Returns(true);

                var returnDate = DateTime.Today;
                backupService.IsBackupExistingAsync().Returns(true);
                backupService.GetBackupDateAsync().Returns(returnDate);

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
                logoutCommandCalled.Should().BeTrue();
                settingsManager.IsLoggedInToBackupService.Should().BeFalse();
            }
        }
    }

}
