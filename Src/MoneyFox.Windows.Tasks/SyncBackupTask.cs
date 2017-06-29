using Windows.ApplicationModel.Background;
using MoneyFox.Windows.Business;
using MoneyFox.Business.Manager;
using Cheesebaron.MvxPlugins.Connectivity.WindowsUWP;
using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.Foundation.Constants;
using MoneyFox.Service;
using MvvmCross.Plugins.File.Uwp;

namespace MoneyFox.Windows.Tasks
{
    public sealed class SyncBackupTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            ApplicationContext.DbPath = DatabaseConstants.DB_NAME;

            try
            {
                var backupManager = new BackupManager(
                    new OneDriveService(new OneDriveAuthenticator()),
                    new MvxWindowsCommonFileStore(),
                    new SettingsManager(new WindowsUwpSettings()),
                    new Connectivity(),
                    new DbFactory());

                await backupManager.DownloadBackup();
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}