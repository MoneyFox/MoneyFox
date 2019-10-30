using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Adapters;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.ViewModels;
using Moq;
using Should;
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
            var connectivitySetup = new Mock<IConnectivityAdapter>();
            connectivitySetup.Setup(x => x.IsConnected).Returns(false);

            var settingsManagerMock = new Mock<ISettingsFacade>();
            settingsManagerMock.SetupAllProperties();

            var checkBackupCalled = false;
            var getBackupDateCalled = false;

            var backupServiceMock = new Mock<IBackupService>();
            backupServiceMock.Setup(x => x.IsBackupExistingAsync()).Callback(() => checkBackupCalled = true);
            backupServiceMock.Setup(x => x.GetBackupDateAsync()).Callback(() => getBackupDateCalled = true);

            //execute
            var vm = new BackupViewModel(backupServiceMock.Object,
                                         null,
                                         connectivitySetup.Object,
                                         settingsManagerMock.Object);
            await vm.InitializeCommand.ExecuteAsync();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            checkBackupCalled.ShouldBeFalse();
            getBackupDateCalled.ShouldBeFalse();
        }

        [Fact]
        public async Task Initialize_ConnectivityNotLoggedIn_NothingCalled()
        {
            // Setup
            var connectivitySetup = new Mock<IConnectivityAdapter>();
            connectivitySetup.Setup(x => x.IsConnected).Returns(true);

            var settingsManagerMock = new Mock<ISettingsFacade>();
            settingsManagerMock.SetupAllProperties();

            var checkBackupCalled = false;
            var getBackupDateCalled = false;

            var backupServiceMock = new Mock<IBackupService>();
            backupServiceMock.Setup(x => x.IsBackupExistingAsync()).Callback(() => checkBackupCalled = true);
            backupServiceMock.Setup(x => x.GetBackupDateAsync()).Callback(() => getBackupDateCalled = true);

            //execute
            var vm = new BackupViewModel(backupServiceMock.Object,
                                         null,
                                         connectivitySetup.Object,
                                         settingsManagerMock.Object);
            await vm.InitializeCommand.ExecuteAsync();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            checkBackupCalled.ShouldBeFalse();
            getBackupDateCalled.ShouldBeFalse();
        }

        [Fact]
        public async Task Initialize_ConnectivityLoggedIn_MethodsCalled()
        {
            // Setup
            var connectivitySetup = new Mock<IConnectivityAdapter>();
            connectivitySetup.Setup(x => x.IsConnected).Returns(true);

            var settingsManagerMock = new Mock<ISettingsFacade>();
            settingsManagerMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);

            DateTime returnDate = DateTime.Today;

            var backupServiceMock = new Mock<IBackupService>();
            backupServiceMock.Setup(x => x.IsBackupExistingAsync()).Returns(Task.FromResult(true));
            backupServiceMock.Setup(x => x.GetBackupDateAsync()).Returns(Task.FromResult(returnDate));

            //execute
            var vm = new BackupViewModel(backupServiceMock.Object,
                                         null,
                                         connectivitySetup.Object,
                                         settingsManagerMock.Object);
            await vm.InitializeCommand.ExecuteAsync();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            vm.BackupAvailable.ShouldBeTrue();
            vm.BackupLastModified.ShouldEqual(returnDate);
        }

        [Fact]
        public async Task Logout_PropertiesSet()
        {
            // Setup
            var connectivitySetup = new Mock<IConnectivityAdapter>();

            var isLoggedIn = false;
            var settingsManagerMock = new Mock<ISettingsFacade>();
            settingsManagerMock.SetupSet(x => x.IsLoggedInToBackupService = It.IsAny<bool>()).Callback((bool val) => isLoggedIn = val);

            var logoutCommandCalled = false;

            var backupServiceMock = new Mock<IBackupService>();
            backupServiceMock.Setup(x => x.LogoutAsync()).Callback(() => logoutCommandCalled = true)
                             .Returns(Task.CompletedTask);

            //execute
            var vm = new BackupViewModel(backupServiceMock.Object,
                                         null,
                                         connectivitySetup.Object,
                                         settingsManagerMock.Object);

            await vm.LogoutCommand.ExecuteAsync();

            //assert
            logoutCommandCalled.ShouldBeTrue();
            isLoggedIn.ShouldBeFalse();
        }
    }
}
