using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Widget;
using Cheesebaron.MvxPlugins.Settings.Droid;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Business.Manager;
using MoneyFox.Business.Services;
using MoneyFox.Droid.Activities;
using MoneyFox.Droid.OneDriveAuth;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Platform;
using MvvmCross.Plugins.File;
using Plugin.Connectivity;
using Debug = System.Diagnostics.Debug;
using Environment = System.Environment;
using JobSchedulerType = Android.App.Job.JobScheduler;

namespace MoneyFox.Droid.Jobs
{
    /// <summary>
    ///     Job to periodically sync backup.
    /// </summary>
    [Service(Exported = true, Permission = "android.permission.BIND_JOB_SERVICE")]
    public class SyncBackupJob : JobService
    {
        private const int SYNC_BACK_JOB_ID = 30;

        /// <inheritdoc />
        public override bool OnStartJob(JobParameters args)
        {
            Task.Run(async () => await SyncBackups(args));
            return true;
        }

        /// <inheritdoc />
        public override bool OnStopJob(JobParameters args)
        {
            return true;
        }

        /// <inheritdoc />
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var callback = (Messenger)intent.GetParcelableExtra("messenger");
            var m = Message.Obtain();
            m.What = MainActivity.MESSAGE_SERVICE_SYNC_BACKUP;
            m.Obj = this;
            try
            {
                callback.Send(m);
            }
            catch (RemoteException ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex);
            }
            return StartCommandResult.NotSticky;
        }

        private async Task SyncBackups(JobParameters args)
        {
            try
            {
                DataAccess.ApplicationContext.DbPath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                                 DatabaseConstants.DB_NAME);

                await new BackupManager(new OneDriveService(new OneDriveAuthenticator()),
                                        Mvx.Resolve<IMvxFileStore>(),
                                        new SettingsManager(new Settings()),
                                        new ConnectivityImplementation())
                    .DownloadBackup();

                JobFinished(args, false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.Write(ex);
            }
        }

        /// <summary>
        ///     Schedules the task for execution.
        /// </summary>
        public void ScheduleTask(int interval)
        {
            if (!Mvx.Resolve<ISettingsManager>().IsBackupAutouploadEnabled) return;

            var builder = new JobInfo.Builder(SYNC_BACK_JOB_ID,
                                              new ComponentName(
                                                  this, Java.Lang.Class.FromType(typeof(SyncBackupJob))));

            // convert hours into millisecond
            builder.SetPeriodic(60 * 60 * 1000 * interval);
            builder.SetPersisted(true);
            builder.SetRequiredNetworkType(NetworkType.NotRoaming);
            builder.SetRequiresDeviceIdle(false);
            builder.SetRequiresCharging(false);

            var tm = (JobSchedulerType)GetSystemService(JobSchedulerService);
            var status = tm.Schedule(builder.Build());
        }
    }
}