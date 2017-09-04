using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Cheesebaron.MvxPlugins.Connectivity.WindowsUWP;
using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.Foundation.Constants;
using MoneyFox.Service;
using MoneyFox.Windows.Business;
using MvvmCross.Plugins.File.Uwp;

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
                    new MvxWindowsCommonFileStore(),
                    new SettingsManager(new WindowsUwpSettings()),
                    new Connectivity(),
                    new DbFactory());

                await backupManager.DownloadBackup();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Sync Backup failed.");
                Debug.WriteLine(ex);

            } finally
            {
                Debug.WriteLine("Sync Backup stopped.");

                deferral.Complete();
            }
        }
    }
}