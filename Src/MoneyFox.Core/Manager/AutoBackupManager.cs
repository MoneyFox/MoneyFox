using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.SettingAccess;

namespace MoneyFox.Core.Manager
{
    /// <summary>
    ///     Handles the automatich backup upload and download
    /// </summary>
    public class AutoBackupManager : IAutobackupManager
    {
        private readonly IBackupService backupService;
        private readonly IRepositoryManager repositoryManager;

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="backupService">Helper for uploading and downloading to the respective backup service.</param>
        /// <param name="repositoryManager">An instance of the repository manager to reload data.</param>
        public AutoBackupManager(IBackupService backupService,
            IRepositoryManager repositoryManager)
        {
            this.backupService = backupService;
            this.repositoryManager = repositoryManager;
        }

        /// <summary>
        ///     Creates a new backup from OneDrive when the last modification is newer then the last OneDrive backup.
        /// </summary>
        public async void UploadBackupIfNewwer()
        {
            try
            {
                if (!Settings.IsBackupAutouploadEnabled)
                {
                    return;
                }

                if (await backupService.GetBackupDate() < Settings.LastDatabaseUpdate)
                {
                    await backupService.Upload();
                }
            }
            catch (OneDriveException ex)
            {
                new TelemetryClient().TrackException(ex);
            }
        }

        /// <summary>
        ///     Restores the backup from OneDrive when the backup is newer then the last modification.
        /// </summary>
        public async Task RestoreBackupIfNewer()
        {
            try
            {
                if (!Settings.IsBackupAutouploadEnabled)
                {
                    return;
                }

                var backupDate = await backupService.GetBackupDate();
                if (backupDate > Settings.LastDatabaseUpdate)
                {
                    await backupService.Restore();
                    repositoryManager.ReloadData();
                    Settings.LastDatabaseUpdate = backupDate;
                }
            }
            catch (OneDriveException ex)
            {
                new TelemetryClient().TrackException(ex);
            }
        }
    }
}