using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Widget;
using Cheesebaron.MvxPlugins.Settings.Droid;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.Droid.OneDriveAuth;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Resources;
using MoneyFox.Service;
using MvvmCross.Platform;
using MvvmCross.Plugins.File;

namespace MoneyFox.Droid.Jobs
{
    [Service(Exported = true, Permission = "android.permission.BIND_JOB_SERVICE")]
    public class SyncBackupJob : JobService
    {
        public override bool OnStartJob(JobParameters @params)
        {
            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            Toast.MakeText(this, Strings.BackupSyncedSuccessfullyMessage, ToastLength.Long);
            return true;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Task.Run(() => SyncBackups());
            return StartCommandResult.RedeliverIntent;
        }

        private async void SyncBackups()
        {
            try
            {
                DataAccess.ApplicationContext.DbPath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                                 DatabaseConstants.DB_NAME);

                await new BackupManager(new OneDriveService(new OneDriveAuthenticator()),
                                        Mvx.Resolve<IMvxFileStore>(),
                                        new SettingsManager(new Settings()),
                                        new Connectivity(),
                                        new DbFactory())
                    .DownloadBackup();
            }
            catch (Exception)
            {
                Toast.MakeText(this, Strings.BackupSyncFailedMessage, ToastLength.Long);
            }
        }
    }
}