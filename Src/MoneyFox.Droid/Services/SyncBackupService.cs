using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Cheesebaron.MvxPlugins.Settings.Droid;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.Droid.OneDriveAuth;
using MoneyFox.Service;
using MvvmCross.Platform;
using MvvmCross.Plugins.File;

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
                                    Mvx.Resolve<IMvxFileStore>(),
                                    new SettingsManager(new Settings()),
                                    new Connectivity(),
                                    new DbFactory())
                .DownloadBackup();
        }
    }
}