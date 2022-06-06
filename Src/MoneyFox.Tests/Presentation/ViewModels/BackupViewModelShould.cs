namespace MoneyFox.Tests.Presentation.ViewModels
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MediatR;
    using MoneyFox.Core._Pending_.Common.Facades;
    using MoneyFox.Core.ApplicationCore.UseCases.DbBackup;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Core.Interfaces;
    using MoneyFox.ViewModels.DataBackup;
    using NSubstitute;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class BackupViewModelShould
    {
        private readonly IMediator mediator;
        private readonly IConnectivityAdapter connectivityAdapter;
        private readonly ISettingsFacade settingsManager;
        private readonly IBackupService backupService;
        private readonly IToastService toastService;
        private readonly IDialogService dialogService;

        protected BackupViewModelShould()
        {
            mediator = Substitute.For<IMediator>();
            connectivityAdapter = Substitute.For<IConnectivityAdapter>();
            settingsManager = Substitute.For<ISettingsFacade>();
            backupService = Substitute.For<IBackupService>();
            toastService = Substitute.For<IToastService>();
            dialogService = Substitute.For<IDialogService>();
        }

        public sealed class InitializeCommand : BackupViewModelShould
        {
            [Fact]
            public async Task CallNothing_OnInitialize_WhenDeviceIsDisconnected()
            {
                // Arrange
                connectivityAdapter.IsConnected.Returns(false);

                // Act
                var vm = new BackupViewModel(
                    mediator: mediator,
                    backupService: backupService,
                    dialogService: dialogService,
                    connectivity: connectivityAdapter,
                    settingsFacade: settingsManager,
                    toastService: toastService);

                vm.InitializeCommand.Execute(null);

                // Assert
                vm.IsLoadingBackupAvailability.Should().BeFalse();
                await backupService.Received(0).IsBackupExistingAsync();
                await backupService.Received(0).GetBackupDateAsync();
            }

            [Fact]
            public async Task CallNothing_OnInitialize_WhenNotLoggedIn()
            {
                // Arrange
                var connectivitySetup = Substitute.For<IConnectivityAdapter>();
                connectivitySetup.IsConnected.Returns(true);
                var settingsManagerMock = Substitute.For<ISettingsFacade>();
                var backupServiceMock = Substitute.For<IBackupService>();

                // Act
                var vm = new BackupViewModel(
                    mediator: mediator,
                    backupService: backupServiceMock,
                    dialogService: dialogService,
                    connectivity: connectivitySetup,
                    settingsFacade: settingsManagerMock,
                    toastService: toastService);

                vm.InitializeCommand.Execute(null);

                // Assert
                vm.IsLoadingBackupAvailability.Should().BeFalse();
                await backupServiceMock.Received(0).IsBackupExistingAsync();
                await backupServiceMock.Received(0).GetBackupDateAsync();
            }

            [Fact]
            public void CallInitializations_WhenConnectivitySet_AndUserLoggedIn()
            {
                // Arrange
                var connectivitySetup = Substitute.For<IConnectivityAdapter>();
                connectivitySetup.IsConnected.Returns(true);
                var settingsManagerMock = Substitute.For<ISettingsFacade>();
                settingsManagerMock.IsLoggedInToBackupService.Returns(true);
                var returnDate = DateTime.Today;
                var backupServiceMock = Substitute.For<IBackupService>();
                backupServiceMock.IsBackupExistingAsync().Returns(true);
                backupServiceMock.GetBackupDateAsync().Returns(returnDate);

                // Act
                var vm = new BackupViewModel(
                    mediator: mediator,
                    backupService: backupServiceMock,
                    dialogService: dialogService,
                    connectivity: connectivitySetup,
                    settingsFacade: settingsManagerMock,
                    toastService: toastService);

                vm.InitializeCommand.Execute(null);

                // Assert
                vm.IsLoadingBackupAvailability.Should().BeFalse();
                vm.BackupAvailable.Should().BeTrue();
                vm.BackupLastModified.Should().Be(returnDate);
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
                var vm = new BackupViewModel(
                    mediator: mediator,
                    backupService: backupService,
                    dialogService: dialogService,
                    connectivity: connectivityAdapter,
                    settingsFacade: settingsManager,
                    toastService: toastService);

                vm.LogoutCommand.Execute(null);

                // Assert
                logoutCommandCalled.Should().BeTrue();
                settingsManager.IsLoggedInToBackupService.Should().BeFalse();
            }
        }
    }

}
