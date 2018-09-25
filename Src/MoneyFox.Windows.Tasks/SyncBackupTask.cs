using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using MoneyFox.Business;
using MoneyFox.Business.Manager;
using MoneyFox.Business.Services;
using MoneyFox.DataAccess;
using MoneyFox.Foundation.Constants;
using MoneyFox.Windows.Business;
using MvvmCross.Plugin.File.Platforms.Uap;
using Plugin.Connectivity;

namespace MoneyFox.Windows.Tasks
{
    /// <inheritdoc />
    /// <summary>
    ///     Background task to sync the backup with OneDrive.
    /// </summary>
    public sealed class SyncBackupTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            Debug.WriteLine("Sync Backup started.");
            ApplicationContext.DbPath = DatabaseConstants.DB_NAME;

            try
            {
                var backupManager = new BackupManager(
                    new OneDriveService(new OneDriveAuthenticator(true)),
                    new MvxWindowsFileStore(), 
                    new SettingsManager(new SettingsAdapter()),
                    new ConnectivityImplementation());

                await backupManager.DownloadBackup();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine("Sync Backup failed.");
            }
            finally
            {
                Debug.WriteLine("Sync Backup finished.");
                deferral.Complete();
            }
        }
    }
}