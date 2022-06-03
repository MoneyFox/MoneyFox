namespace MoneyFox.Tests.Presentation.ViewModels
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MediatR;
    using MoneyFox.Core._Pending_.Common.Facades;
    using MoneyFox.Core.ApplicationCore.UseCases.DbBackup;
    using MoneyFox.Core.Interfaces;
    using MoneyFox.ViewModels.DataBackup;
    using NSubstitute;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class BackupViewModelTests
    {
        private readonly IMediator mediator;
        private readonly IConnectivityAdapter connectivitySetup;
        private readonly ISettingsFacade? settingsManagerMock1;
        private readonly IBackupService? backupServiceMock1;

        public BackupViewModelTests()
        {
            mediator = Substitute.For<IMediator>();
            connectivitySetup = Substitute.For<IConnectivityAdapter>();
            settingsManagerMock1 = Substitute.For<ISettingsFacade>();
            backupServiceMock1 = Substitute.For<IBackupService>();
        }

        [Fact]
        public async Task CallsNothing_OnInitialize_WhenDeviceIsDisconnected()
        {
            // Setup
            connectivitySetup.IsConnected.Returns(false);

            //execute
            var vm = new BackupViewModel(mediator, backupService: backupServiceMock1, dialogService: null, connectivity: connectivitySetup,
                settingsFacade: settingsManagerMock1,
                toastService: null);

            vm.InitializeCommand.Execute(null);

            //assert
            vm.IsLoadingBackupAvailability.Should().BeFalse();
            await backupServiceMock1.Received(0).IsBackupExistingAsync();
            await backupServiceMock1.Received(0).GetBackupDateAsync();
        }

        [Fact]
        public async Task CallsNothing_OnInitialize_WhenNotLoggedIn)
        {
            // Setup
            var connectivitySetup = Substitute.For<IConnectivityAdapter>();
            connectivitySetup.IsConnected.Returns(true);
            var settingsManagerMock = Substitute.For<ISettingsFacade>();
            var backupServiceMock = Substitute.For<IBackupService>();

            //execute
            var vm = new BackupViewModel(mediator, backupService: backupServiceMock, dialogService: null, connectivity: connectivitySetup,
                settingsFacade: settingsManagerMock,
                toastService: null);

            vm.InitializeCommand.Execute(null);

            //assert
            vm.IsLoadingBackupAvailability.Should().BeFalse();
            await backupServiceMock.Received(0).IsBackupExistingAsync();
            await backupServiceMock.Received(0).GetBackupDateAsync();
        }

        [Fact]
        public void CallsInitializations_WhenConnectivitySet_AndUserLoggedIn()
        {
            // Setup
            var connectivitySetup = Substitute.For<IConnectivityAdapter>();
            connectivitySetup.IsConnected.Returns(true);
            var settingsManagerMock = Substitute.For<ISettingsFacade>();
            settingsManagerMock.IsLoggedInToBackupService.Returns(true);
            var returnDate = DateTime.Today;
            var backupServiceMock = Substitute.For<IBackupService>();
            backupServiceMock.IsBackupExistingAsync().Returns(true);
            backupServiceMock.GetBackupDateAsync().Returns(returnDate);

            //execute
            var vm = new BackupViewModel(mediator, backupService: backupServiceMock, dialogService: null, connectivity: connectivitySetup,
                settingsFacade: settingsManagerMock,
                toastService: null);

            vm.InitializeCommand.Execute(null);

            //assert
            vm.IsLoadingBackupAvailability.Should().BeFalse();
            vm.BackupAvailable.Should().BeTrue();
            vm.BackupLastModified.Should().Be(returnDate);
        }

        [Fact]
        public void UpdatesSettingsCorrectly_OnLogout()
        {
            // Setup
            var connectivitySetup = Substitute.For<IConnectivityAdapter>();
            var settingsManagerMock = Substitute.For<ISettingsFacade>();
            var logoutCommandCalled = false;
            var backupServiceMock = Substitute.For<IBackupService>();
            backupServiceMock.When(x => x.LogoutAsync()).Do(x => logoutCommandCalled = true);

            //execute
            var vm = new BackupViewModel(mediator, backupService: backupServiceMock, dialogService: null, connectivity: connectivitySetup,
                settingsFacade: settingsManagerMock,
                toastService: null);

            vm.LogoutCommand.Execute(null);

            //assert
            logoutCommandCalled.Should().BeTrue();
            settingsManagerMock.IsLoggedInToBackupService.Should().BeFalse();
        }
    }

}
