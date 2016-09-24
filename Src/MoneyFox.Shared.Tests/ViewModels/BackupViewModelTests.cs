using Cheesebaron.MvxPlugins.Connectivity;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [TestClass]
    public class BackupViewModelTests : MvxIoCSupportingTest
    {
        [TestInitialize]
        public void Init()
        {
            ClearAll();
            Setup();
        }

        [TestMethod]
        public void Loaded_NoConnectivity_NothingCalled()
        {
            // Setup
            var connectivitySetup = new Mock<IConnectivity>();
            connectivitySetup.Setup(x => x.IsConnected).Returns(false);

            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<bool>(), false)).Returns(true);
            Mvx.RegisterType(() => settingsMockSetup.Object);

            var checkBackupCalled = false;
            var getBackupDateCalled = false;

            var backupManagerSetup = new Mock<IBackupManager>();
            backupManagerSetup.Setup(x => x.IsBackupExisting()).Callback(() => checkBackupCalled = true);
            backupManagerSetup.Setup(x => x.GetBackupDate()).Callback(() => getBackupDateCalled = true);

            //execute
            var vm = new BackupViewModel(backupManagerSetup.Object, null, connectivitySetup.Object);
            vm.LoadedCommand.Execute();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            checkBackupCalled.ShouldBeFalse();
            getBackupDateCalled.ShouldBeFalse();
        }

        [TestMethod]
        public void Loaded_ConnectivityNotLoggedIn_NothingCalled() {
            // Setup
            var connectivitySetup = new Mock<IConnectivity>();
            connectivitySetup.Setup(x => x.IsConnected).Returns(true);

            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<bool>(), false)).Returns(false);
            Mvx.RegisterType(() => settingsMockSetup.Object);

            var checkBackupCalled = false;
            var getBackupDateCalled = false;

            var backupManagerSetup = new Mock<IBackupManager>();
            backupManagerSetup.Setup(x => x.IsBackupExisting()).Callback(() => checkBackupCalled = true);
            backupManagerSetup.Setup(x => x.GetBackupDate()).Callback(() => getBackupDateCalled = true);

            //execute
            var vm = new BackupViewModel(backupManagerSetup.Object, null, connectivitySetup.Object);
            vm.LoadedCommand.Execute();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            checkBackupCalled.ShouldBeFalse();
            getBackupDateCalled.ShouldBeFalse();
        }

        [TestMethod]
        [Ignore]
        public void Loaded_ConnectivityLoggedIn_MethodsCalled() {
            // Setup
            var connectivitySetup = new Mock<IConnectivity>();
            connectivitySetup.Setup(x => x.IsConnected).Returns(true);

            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.SetupAllProperties();
            settingsMockSetup.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<bool>(), false)).Returns(true);
            Mvx.RegisterType(() => settingsMockSetup.Object);

            var checkBackupCalled = false;
            var getBackupDateCalled = false;

            var backupManagerSetup = new Mock<IBackupManager>();
            backupManagerSetup.Setup(x => x.IsBackupExisting()).Callback(() => checkBackupCalled = true);
            backupManagerSetup.Setup(x => x.GetBackupDate()).Callback(() => getBackupDateCalled = true);

            //execute
            var vm = new BackupViewModel(backupManagerSetup.Object, null, connectivitySetup.Object);
            vm.LoadedCommand.Execute();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            checkBackupCalled.ShouldBeTrue();
            getBackupDateCalled.ShouldBeTrue();
        }
    }
}