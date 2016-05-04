using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using Xamarin;

namespace MoneyFox.Shared.Manager
{
    /// <summary>
    ///     Handles the automatich backup upload and download
    /// </summary>
    public class AutoBackupManager : IAutobackupManager
    {
        private readonly IBackupManager backupManager;
        private readonly SettingDataAccess roamingSettings;

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="backupService">Helper for uploading and downloading to the respective backup service.</param>
        /// <param name="roamingSettings">Access to the roaming settings.</param>
        /// <param name="repositoryManager">An instance of the repository manager to reload data.</param>
        public AutoBackupManager(IBackupManager backupManager, SettingDataAccess roamingSettings)
        {
            this.backupManager = backupManager;
            this.roamingSettings = roamingSettings;
        }

        /// <summary>
        ///     Creates a new backup from OneDrive when the last modification is newer then the last OneDrive backup.
        /// </summary>
        public async void UploadBackupIfNewwer()
        {
            try
            {
                if (!roamingSettings.IsBackupAutouploadEnabled)
                {
                    return;
                }

                if (await backupManager.GetBackupDate() < Settings.LastDatabaseUpdate)
                {
                    await backupManager.UploadNewBackup();
                }
            }
            catch (OneDriveException ex)
            {
                Insights.Report(ex);
            }
        }

        /// <summary>
        ///     Restores the backup from OneDrive when the backup is newer then the last modification.
        /// </summary>
        public async Task RestoreBackupIfNewer()
        {
            try
            {
                if (!roamingSettings.IsBackupAutouploadEnabled)
                {
                    return;
                }

                var backupDate = await backupManager.GetBackupDate();
                if (backupDate > Settings.LastDatabaseUpdate)
                {
                    await backupManager.RestoreBackup();
                }
            }
            catch (OneDriveException ex)
            {
                Insights.Report(ex);
            }
        }
    }
}