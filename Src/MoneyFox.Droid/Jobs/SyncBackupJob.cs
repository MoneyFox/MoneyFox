using System;
using System.IO;
using Android.App;
using Android.App.Job;
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
        public override bool OnStartJob(JobParameters args)
        {
            SyncBackups(args);
            return true;
        }

        public override bool OnStopJob(JobParameters args)
        {
            return true;
        }

        private async void SyncBackups(JobParameters args)
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

                Toast.MakeText(this, Strings.BackupSyncedSuccessfullyMessage, ToastLength.Long);
                JobFinished(args, false);
            } catch (Exception)
            {
                Toast.MakeText(this, Strings.BackupSyncFailedMessage, ToastLength.Long);
            }
        }
    }
}