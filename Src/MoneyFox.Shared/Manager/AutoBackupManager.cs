using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace MoneyFox.Shared.Manager {
    /// <summary>
    ///     Handles the automatich backup upload and download
    /// </summary>
    public class AutoBackupManager : IAutobackupManager {
        private readonly IBackupManager backupManager;
        private readonly GlobalBusyIndicatorState globalBusyIndicatorState;

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="backupManager">An backup manager object that handles the restoring and creating of backups.</param>
        public AutoBackupManager(IBackupManager backupManager, GlobalBusyIndicatorState globalBusyIndicatorState) {
            this.backupManager = backupManager;
            this.globalBusyIndicatorState = globalBusyIndicatorState;
        }

        /// <summary>
        ///     Creates a new backup from OneDrive when the last modification is newer then the last OneDrive backup.
        /// </summary>
        public async void UploadBackupIfNewwer() {
            try {
                if (!SettingsHelper.IsBackupAutouploadEnabled) {
                    return;
                }

                globalBusyIndicatorState.IsActive = true;

                if (await backupManager.GetBackupDate() < SettingsHelper.LastDatabaseUpdate) {
                    await backupManager.CreateNewBackup();
                }
            }
            catch (OneDriveException ex) {
                Mvx.Trace(MvxTraceLevel.Error, ex.Message);
            }
            globalBusyIndicatorState.IsActive = false;
        }

        /// <summary>
        ///     Restores the backup from OneDrive when the backup is newer then the last modification.
        /// </summary>
        public async Task RestoreBackupIfNewer() {
            try {
                globalBusyIndicatorState.IsActive = true;
                if (!SettingsHelper.IsBackupAutouploadEnabled) {
                    globalBusyIndicatorState.IsActive = false;
                    return;
                }

                var backupDate = await backupManager.GetBackupDate();
                if (backupDate > SettingsHelper.LastDatabaseUpdate) {
                    await backupManager.RestoreBackup();
                }
            }
            catch (OneDriveException ex) {
                Mvx.Trace(MvxTraceLevel.Error, ex.Message);
            }
            globalBusyIndicatorState.IsActive = false;
        }
    }
}