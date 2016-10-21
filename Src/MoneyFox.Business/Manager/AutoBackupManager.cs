using System;
using System.Threading.Tasks;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace MoneyFox.Business.Manager
{
    /// <summary>
    ///     Handles the automatich backup upload and download
    /// </summary>
    public class AutoBackupManager : IAutobackupManager
    {
        private readonly IBackupManager backupManager;
        private readonly GlobalBusyIndicatorState globalBusyIndicatorState;
        private readonly ISettingsManager settingsManager;

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="backupManager">An backup manager object that handles the restoring and creating of backups.</param>
        public AutoBackupManager(IBackupManager backupManager, GlobalBusyIndicatorState globalBusyIndicatorState,
            ISettingsManager settingsManager)
        {
            this.backupManager = backupManager;
            this.globalBusyIndicatorState = globalBusyIndicatorState;
            this.settingsManager = settingsManager;
        }

        /// <summary>
        ///     Creates a new backup from OneDrive when the last modification is newer then the last OneDrive backup.
        /// </summary>
        public async Task UploadBackupIfNewer()
        {
            try
            {
                if (!settingsManager.IsBackupAutouploadEnabled)
                {
                    return;
                }

                globalBusyIndicatorState.IsActive = true;

                if (await backupManager.GetBackupDate() < settingsManager.LastDatabaseUpdate)
                {
                    await backupManager.EnqueueBackupTask(0);
                }
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, ex.Message);
            }
            globalBusyIndicatorState.IsActive = false;
        }

        /// <summary>
        ///     Restores the backup from OneDrive when the backup is newer then the last modification.
        /// </summary>
        public async Task RestoreBackupIfNewer()
        {
            try
            {
                globalBusyIndicatorState.IsActive = true;
                if (!settingsManager.IsBackupAutouploadEnabled)
                {
                    globalBusyIndicatorState.IsActive = false;
                    return;
                }

                var date = await backupManager.GetBackupDate();
                if (date > settingsManager.LastDatabaseUpdate)
                {
                    await backupManager.RestoreBackup();
                }
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, ex.Message);
            }
            globalBusyIndicatorState.IsActive = false;
        }
    }
}