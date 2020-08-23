using FluentAssertions;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Ui.Shared.ViewModels.Backup;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class BackupViewModelTests
    {
        [Fact]
        public async Task Initialize_NoConnectivity_NothingCalled()
        {
            // Setup
            var connectivitySetup = Substitute.For<IConnectivityAdapter>();
            connectivitySetup.IsConnected.Returns(false);

            var settingsManagerMock = Substitute.For<ISettingsFacade>();
            var backupServiceMock = Substitute.For<IBackupService>();

            //execute
            var vm = new BackupViewModel(backupServiceMock,
                                         null,
                                         connectivitySetup,
                                         settingsManagerMock,
                                         null);
            vm.InitializeCommand.Execute(null);

            //assert
            vm.IsLoadingBackupAvailability.Should().BeFalse();
            await backupServiceMock.Received(0).IsBackupExistingAsync();
            await backupServiceMock.Received(0).GetBackupDateAsync();
        }

        [Fact]
        public async Task Initialize_ConnectivityNotLoggedIn_NothingCalled()
        {
            // Setup
            var connectivitySetup = Substitute.For<IConnectivityAdapter>();
            connectivitySetup.IsConnected.Returns(true);

            var settingsManagerMock = Substitute.For<ISettingsFacade>();
            var backupServiceMock = Substitute.For<IBackupService>();

            //execute
            var vm = new BackupViewModel(backupServiceMock,
                                         null,
                                         connectivitySetup,
                                         settingsManagerMock,
                                         null);
            vm.InitializeCommand.Execute(null);

            //assert
            vm.IsLoadingBackupAvailability.Should().BeFalse();
            await backupServiceMock.Received(0).IsBackupExistingAsync();
            await backupServiceMock.Received(0).GetBackupDateAsync();
        }

        [Fact]
        public void Initialize_ConnectivityLoggedIn_MethodsCalled()
        {
            // Setup
            var connectivitySetup = Substitute.For<IConnectivityAdapter>();
            connectivitySetup.IsConnected.Returns(true);

            var settingsManagerMock = Substitute.For<ISettingsFacade>();
            settingsManagerMock.IsLoggedInToBackupService.Returns(true);

            DateTime returnDate = DateTime.Today;

            var backupServiceMock = Substitute.For<IBackupService>();
            backupServiceMock.IsBackupExistingAsync().Returns(true);
            backupServiceMock.GetBackupDateAsync().Returns(returnDate);

            //execute
            var vm = new BackupViewModel(backupServiceMock,
                                         null,
                                         connectivitySetup,
                                         settingsManagerMock,
                                         null);
            vm.InitializeCommand.Execute(null);

            //assert
            vm.IsLoadingBackupAvailability.Should().BeFalse();
            vm.BackupAvailable.Should().BeTrue();
            vm.BackupLastModified.Should().Be(returnDate);
        }

        [Fact]
        public void Logout_PropertiesSet()
        {
            // Setup
            var connectivitySetup = Substitute.For<IConnectivityAdapter>();
            var settingsManagerMock = Substitute.For<ISettingsFacade>();

            var logoutCommandCalled = false;

            var backupServiceMock = Substitute.For<IBackupService>();
            backupServiceMock.When(x => x.LogoutAsync())
                             .Do(x => logoutCommandCalled = true);

            //execute
            var vm = new BackupViewModel(backupServiceMock,
                                         null,
                                         connectivitySetup,
                                         settingsManagerMock,
                                         null);
            vm.LogoutCommand.Execute(null);

            //assert
            logoutCommandCalled.Should().BeTrue();
            settingsManagerMock.IsLoggedInToBackupService.Should().BeFalse();
        }
    }
}
