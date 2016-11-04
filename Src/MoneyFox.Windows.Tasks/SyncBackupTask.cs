using Windows.ApplicationModel.Background;
using Cheesebaron.MvxPlugins.Settings.WindowsCommon;
using MoneyFox.DataAccess;
using MoneyFox.Windows.Business;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using MoneyFox.Business.Manager;
using MoneyFox.Business.Services;
using MoneyFox.DataAccess.Repositories;

namespace MoneyFox.Windows.Tasks
{
    public sealed class SyncBackupTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            try
            {
                var dbManager = new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWindowsCommonFileStore());

                var settingsManager = new SettingsManager(new WindowsCommonSettings());

                var backupManager = new BackupManager(new OneDriveService(new MvxWindowsCommonFileStore(), new OneDriveAuthenticator()),
                        new MvxWindowsCommonFileStore(), 
                        dbManager, settingsManager,
                        new PaymentRepository(dbManager));

                await backupManager.SyncBackup();
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}