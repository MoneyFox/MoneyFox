using System;
using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.Connectivity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.ViewModels;
using Moq;
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

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            var checkBackupCalled = false;
            var getBackupDateCalled = false;

            var backupManagerSetup = new Mock<IBackupManager>();
            backupManagerSetup.Setup(x => x.IsBackupExisting()).Callback(() => checkBackupCalled = true);
            backupManagerSetup.Setup(x => x.GetBackupDate()).Callback(() => getBackupDateCalled = true);

            //execute
            var vm = new BackupViewModel(backupManagerSetup.Object, null, connectivitySetup.Object, settingsManagerMock.Object);
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

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupAllProperties();

            var checkBackupCalled = false;
            var getBackupDateCalled = false;

            var backupManagerSetup = new Mock<IBackupManager>();
            backupManagerSetup.Setup(x => x.IsBackupExisting()).Callback(() => checkBackupCalled = true);
            backupManagerSetup.Setup(x => x.GetBackupDate()).Callback(() => getBackupDateCalled = true);

            //execute
            var vm = new BackupViewModel(backupManagerSetup.Object, null, connectivitySetup.Object, settingsManagerMock.Object);
            vm.LoadedCommand.Execute();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            checkBackupCalled.ShouldBeFalse();
            getBackupDateCalled.ShouldBeFalse();
        }

        [TestMethod]
        public void Loaded_ConnectivityLoggedIn_MethodsCalled() {
            // Setup
            var connectivitySetup = new Mock<IConnectivity>();
            connectivitySetup.Setup(x => x.IsConnected).Returns(true);

            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupGet(x => x.IsLoggedInToBackupService).Returns(true);

            var returnDate = DateTime.Today;

            var backupManagerSetup = new Mock<IBackupManager>();
            backupManagerSetup.Setup(x => x.IsBackupExisting()).Returns(Task.FromResult(true));
            backupManagerSetup.Setup(x => x.GetBackupDate()).Returns(Task.FromResult(returnDate));

            //execute
            var vm = new BackupViewModel(backupManagerSetup.Object, null, connectivitySetup.Object, settingsManagerMock.Object);
            vm.LoadedCommand.Execute();

            //assert
            vm.IsLoadingBackupAvailability.ShouldBeFalse();
            vm.BackupAvailable.ShouldBeTrue();
            vm.BackupLastModified.ShouldBe(returnDate);
        }
    }
}