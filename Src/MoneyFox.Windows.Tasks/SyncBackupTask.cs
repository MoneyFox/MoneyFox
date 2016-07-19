using System;
using Windows.ApplicationModel.Background;
using Cheesebaron.MvxPlugins.Settings.WindowsCommon;
using MoneyFox.Shared;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.Services;
using MoneyFox.Windows.Business;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyFox.Windows.Tasks
{
    public sealed class SyncBackupTask : IBackgroundTask
    {
        private const string DATABASE_LAST_UPDATE_KEYNAME = "DatabaseLastUpdate";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var fileStore = new MvxWindowsCommonFileStore();
            var databaseManager = new DatabaseManager(new WindowsSqliteConnectionFactory(), fileStore);

            var unitOfWork = new UnitOfWork(databaseManager);

            var backupManager = new BackupManager(new RepositoryManager(unitOfWork, new PaymentManager(unitOfWork, null)),
                new OneDriveService(fileStore, new OneDriveAuthenticator()),
                fileStore, databaseManager);

            if (await backupManager.GetBackupDate() < new WindowsCommonSettings().GetValue(DATABASE_LAST_UPDATE_KEYNAME, DateTime.MinValue))
            {
                await backupManager.EnqueueBackupTask(0);
            }

            if (await backupManager.GetBackupDate() > SettingsHelper.LastDatabaseUpdate)
            {
                await backupManager.RestoreBackup();
            }
        }
    }
}
