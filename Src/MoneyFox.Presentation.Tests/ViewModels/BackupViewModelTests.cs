using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Services;
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
            backupServiceMock.Setup(x => x.IsBackupExisting()).Callback(() => checkBackupCalled = true);
            backupServiceMock.Setup(x => x.GetBackupDate()).Callback(() => getBackupDateCalled = true);

            //execute
            var vm = new BackupViewModel(backupServiceMock.Object, 
                                         null,
                                         connectivitySetup.Object,
                                         settingsManagerMock.Object);
            vm.Initialize();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            checkBackupCalled.ShouldBeFalse();
            getBackupDateCalled.ShouldBeFalse();
        }

        [Fact]
        public async Task Initialize_ConnectivityNotLoggedIn_NothingCalled() {
            // Setup
            var connectivitySetup = new Mock<IConnectivityAdapter>();
            connectivitySetup.Setup(x => x.IsConnected).Returns(true);

            var settingsManagerMock = new Mock<ISettingsFacade>();
            settingsManagerMock.SetupAllProperties();

            var checkBackupCalled = false;
            var getBackupDateCalled = false;

            var backupServiceMock = new Mock<IBackupService>();
            backupServiceMock.Setup(x => x.IsBackupExisting()).Callback(() => checkBackupCalled = true);
            backupServiceMock.Setup(x => x.GetBackupDate()).Callback(() => getBackupDateCalled = true);

            //execute
            var vm = new BackupViewModel(backupServiceMock.Object, 
                                         null, 
                                         connectivitySetup.Object,
                                         settingsManagerMock.Object);
            vm.Initialize();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            checkBackupCalled.ShouldBeFalse();
            getBackupDateCalled.ShouldBeFalse();
        }

        [Fact]
        public async Task Initialize_ConnectivityLoggedIn_MethodsCalled() {
            // Setup
            var connectivitySetup = new Mock<IConnectivityAdapter>();
            connectivitySetup.Setup(x => x.IsConnected).Returns(true);

            var settingsManagerMock = new Mock<ISettingsFacade>();
            settingsManagerMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);

            var returnDate = DateTime.Today;

            var backupServiceMock = new Mock<IBackupService>();
            backupServiceMock.Setup(x => x.IsBackupExisting()).Returns(Task.FromResult(true));
            backupServiceMock.Setup(x => x.GetBackupDate()).Returns(Task.FromResult(returnDate));

            //execute
            var vm = new BackupViewModel(backupServiceMock.Object, 
                                         null, 
                                         connectivitySetup.Object,
                                         settingsManagerMock.Object);
            vm.Initialize();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            vm.BackupAvailable.ShouldBeTrue();
            vm.BackupLastModified.ShouldEqual(returnDate);
        }

        [Fact]
        public void Logout_PropertiesSet()
        {
            // Setup
            var connectivitySetup = new Mock<IConnectivityAdapter>();

            var isLoggedIn = false;
            var settingsManagerMock = new Mock<ISettingsFacade>();
            settingsManagerMock.SetupSet(x => x.IsLoggedInToBackupService = It.IsAny<bool>()).Callback((bool val) => isLoggedIn = val);

            var logoutCommandCalled = false;

            var backupServiceMock = new Mock<IBackupService>();
            backupServiceMock.Setup(x => x.Logout()).Callback(() => logoutCommandCalled = true).ReturnsAsync(OperationResult.Succeeded());

            //execute
            var vm = new BackupViewModel(backupServiceMock.Object, 
                                         null, 
                                         connectivitySetup.Object,
                                         settingsManagerMock.Object);

            vm.LogoutCommand.Execute(null);

            //assert
            logoutCommandCalled.ShouldBeTrue();
            isLoggedIn.ShouldBeFalse();
        }
    }
}