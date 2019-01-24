using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Business.Adapter;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Interfaces;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    [Collection("MvxIocCollection")]
    public class BackupViewModelTests : MvxIoCSupportingTest
    {
        [Fact]
        public async void Loaded_NoConnectivity_NothingCalled()
        {
            // Setup
            var connectivitySetup = new Mock<IConnectivtyAdapter>();
            connectivitySetup.Setup(x => x.IsConnected).Returns(false);

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            var checkBackupCalled = false;
            var getBackupDateCalled = false;

            var backupManagerSetup = new Mock<IBackupManager>();
            backupManagerSetup.Setup(x => x.IsBackupExisting()).Callback(() => checkBackupCalled = true);
            backupManagerSetup.Setup(x => x.GetBackupDate()).Callback(() => getBackupDateCalled = true);

            //execute
            var vm = new BackupViewModel(backupManagerSetup.Object, null, connectivitySetup.Object,
                                         settingsManagerMock.Object,
                                         new Mock<IMvxLogProvider>().Object,
                                         new Mock<IMvxNavigationService>().Object);
            await vm.Initialize();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            checkBackupCalled.ShouldBeFalse();
            getBackupDateCalled.ShouldBeFalse();
        }

        [Fact]
        public async void Loaded_ConnectivityNotLoggedIn_NothingCalled() {
            // Setup
            var connectivitySetup = new Mock<IConnectivtyAdapter>();
            connectivitySetup.Setup(x => x.IsConnected).Returns(true);

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            var checkBackupCalled = false;
            var getBackupDateCalled = false;

            var backupManagerSetup = new Mock<IBackupManager>();
            backupManagerSetup.Setup(x => x.IsBackupExisting()).Callback(() => checkBackupCalled = true);
            backupManagerSetup.Setup(x => x.GetBackupDate()).Callback(() => getBackupDateCalled = true);

            //execute
            var vm = new BackupViewModel(backupManagerSetup.Object, null, connectivitySetup.Object,
                                         settingsManagerMock.Object,
                                         new Mock<IMvxLogProvider>().Object,
                                         new Mock<IMvxNavigationService>().Object);
            await vm.Initialize();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            checkBackupCalled.ShouldBeFalse();
            getBackupDateCalled.ShouldBeFalse();
        }

        [Fact]
        public async void Loaded_ConnectivityLoggedIn_MethodsCalled() {
            // Setup
            var connectivitySetup = new Mock<IConnectivtyAdapter>();
            connectivitySetup.Setup(x => x.IsConnected).Returns(true);

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);

            var returnDate = DateTime.Today;

            var backupManagerSetup = new Mock<IBackupManager>();
            backupManagerSetup.Setup(x => x.IsBackupExisting()).Returns(Task.FromResult(true));
            backupManagerSetup.Setup(x => x.GetBackupDate()).Returns(Task.FromResult(returnDate));

            //execute
            var vm = new BackupViewModel(backupManagerSetup.Object, null, connectivitySetup.Object,
                                         settingsManagerMock.Object,
                                         new Mock<IMvxLogProvider>().Object,
                                         new Mock<IMvxNavigationService>().Object);
            await vm.Initialize();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            vm.BackupAvailable.ShouldBeTrue();
            vm.BackupLastModified.ShouldEqual(returnDate);
        }

        [Fact]
        public void Logout_PropertiesSet()
        {
            // Setup
            var connectivitySetup = new Mock<IConnectivtyAdapter>();

            var isLoggedIn = false;
            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupSet(x => x.IsLoggedInToBackupService = It.IsAny<bool>()).Callback((bool val) => isLoggedIn = val);

            var logoutCommandCalled = false;

            var backupManagerSetup = new Mock<IBackupManager>();
            backupManagerSetup.Setup(x => x.Logout()).Callback(() => logoutCommandCalled = true).Returns(Task.CompletedTask);

            //execute
            var vm = new BackupViewModel(backupManagerSetup.Object, null, connectivitySetup.Object,
                                         settingsManagerMock.Object,
                                         new Mock<IMvxLogProvider>().Object,
                                         new Mock<IMvxNavigationService>().Object);
            vm.LogoutCommand.Execute();

            //assert
            logoutCommandCalled.ShouldBeTrue();
            isLoggedIn.ShouldBeFalse();
        }
    }
}