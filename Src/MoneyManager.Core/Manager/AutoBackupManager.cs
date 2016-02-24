using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyManager.Core.Helpers;
using MoneyManager.DataAccess;
using MoneyManager.Foundation.Interfaces;
using MvvmCross.Platform;
using Xamarin;

namespace MoneyManager.Core.Manager
{
    /// <summary>
    ///     Handles the automatich backup upload and download
    /// </summary>
    public class AutoBackupManager : IAutobackupManager
    {
        private readonly IBackupService backupService;
        private readonly SettingDataAccess roamingSettings;
        private readonly IRepositoryManager repositoryManager;

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="backupService">Helper for uploading and downloading to the respective backup service.</param>
        /// <param name="roamingSettings">Access to the roaming settings.</param>
        /// <param name="repositoryManager">An instance of the repository manager to reload data.</param>
        public AutoBackupManager(IBackupService backupService, SettingDataAccess roamingSettings, IRepositoryManager repositoryManager)
        {
            this.backupService = backupService;
            this.roamingSettings = roamingSettings;
            this.repositoryManager = repositoryManager;
        }

        /// <summary>
        ///     Creates a new backup from OneDrive when the last modification is newer then the last OneDrive backup.
        /// </summary>
        public async void UploadBackupIfNewwer()
        {
            if (!roamingSettings.IsBackupAutouploadEnabled) return;

            if (await backupService.GetBackupDate() < Settings.LastDatabaseUpdate)
            {
                await backupService.Upload();
            }
        }

        /// <summary>
        ///     Restores the backup from OneDrive when the backup is newer then the last modification.
        /// </summary>
        public async Task RestoreBackupIfNewer()
        {
            try
            {
                if (!roamingSettings.IsBackupAutouploadEnabled) return;

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
                Insights.Report(ex);
            }
        }
    }
}