using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.BusinessLogic.Backup;
using MoneyFox.DataLayer;
using MoneyFox.Foundation.Constants;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.Uwp.Business;
using MvvmCross.Plugin.File.Platforms.Uap;

namespace MoneyFox.Uwp.Tasks
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
            EfCoreContext.DbPath = DatabaseConstants.DB_NAME;
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            try
            {
                var backupManager = new BackupManager(
                    new OneDriveService(new OneDriveAuthenticator()),
                    new MvxWindowsFileStore(), 
                    new ConnectivityAdapter());

                var backupService = new BackupService(backupManager,settingsFacade);

                await backupService.RestoreBackup()
                                   .ConfigureAwait(true);
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