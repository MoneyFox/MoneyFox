using Windows.ApplicationModel.Background;
using MoneyFox.DataAccess;
using MoneyFox.Windows.Business;
using MvvmCross.Plugins.File.WindowsCommon;
using MoneyFox.Business.Manager;
using Cheesebaron.MvxPlugins.Connectivity.WindowsUWP;
using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.Service;

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
                
                var settingsManager = new SettingsManager(new WindowsUwpSettings());

                var backupManager = new BackupManager(new OneDriveService(new OneDriveAuthenticator()),
                        new MvxWindowsCommonFileStore(), 
                        settingsManager,
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