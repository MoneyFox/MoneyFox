using Windows.ApplicationModel.Background;
using MoneyFox.DataAccess;
using MoneyFox.Windows.Business;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using MoneyFox.Business.Manager;
using MoneyFox.Business.Services;
using Cheesebaron.MvxPlugins.Connectivity.WindowsUWP;
using Cheesebaron.MvxPlugins.Settings.WindowsUWP;

namespace MoneyFox.Windows.Tasks
{
    public sealed class SyncBackupTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            try
            {
                MapperConfiguration.Setup();

                var dbManager = new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWindowsCommonFileStore());

                var settingsManager = new SettingsManager(new WindowsUwpSettings());

                var backupManager = new BackupManager(new OneDriveService(new MvxWindowsCommonFileStore(), new OneDriveAuthenticator()),
                        new MvxWindowsCommonFileStore(), 
                        dbManager, settingsManager,
                        new Connectivity());

                await backupManager.DownloadBackup();
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}