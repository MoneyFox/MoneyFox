using System;
using System.Threading.Tasks;
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
        private readonly ISettingsManager settingsManager;

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="backupManager">An backup manager object that handles the restoring and creating of backups.</param>
        /// <param name="settingsManager">Settings manager to access the settings.</param>
        public AutoBackupManager(IBackupManager backupManager, ISettingsManager settingsManager)
        {
            this.backupManager = backupManager;
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


                if (await backupManager.GetBackupDate() < settingsManager.LastDatabaseUpdate)
                {
                    await backupManager.EnqueueBackupTask(0);
                }
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, ex.Message);
            }
        }

        /// <summary>
        ///     Restores the backup from OneDrive when the backup is newer then the last modification.
        /// </summary>
        public async Task RestoreBackupIfNewer()
        {
            try
            {
                if (!settingsManager.IsBackupAutouploadEnabled) return;

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
        }
    }
}