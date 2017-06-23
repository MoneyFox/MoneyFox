using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.Droid.OneDriveAuth;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Service;
using MvvmCross.Plugins.File;
using MvvmCross.Plugins.File.Droid;

namespace MoneyFox.Droid.Services
{
    [Service]
    public class SyncBackupService : Android.App.Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Task.Run(() => SyncBackups());
            return StartCommandResult.RedeliverIntent;
        }

        private async void SyncBackups()
        {
            await new BackupManager(new OneDriveService(new OneDriveAuthenticator()),
                                                  new MvxIoFileStoreBase(),
                                                  new SettingsManager(new DroidSettings),
                                                  new DroidConnectivity(),
                                                  new DbFactory())
                .DownloadBackup();
        }
    }
}