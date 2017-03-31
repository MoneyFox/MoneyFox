using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Cheesebaron.MvxPlugins.Settings.Droid;
using MoneyFox.Business.Manager;
using MoneyFox.Business.Services;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Droid.OneDriveAuth;
using MvvmCross.Plugins.File.Droid;
using MvvmCross.Plugins.Sqlite.Droid;

namespace MoneyFox.Droid.Services
{
    [Service]
    public class SyncBackupService : Service
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
            var dbManager = new DatabaseManager(new DroidSqliteConnectionFactory(), new MvxAndroidFileStore());

            var settings = new SettingsManager(new Settings());

            var backupManager =
                new BackupManager(new OneDriveService(new MvxAndroidFileStore(), new OneDriveAuthenticator()),
                    new MvxAndroidFileStore(),
                    dbManager, settings,
                    new Connectivity());

            await backupManager.DownloadBackup();

            PaymentRepository.IsCacheMarkedForReload = true;
        }
    }
}