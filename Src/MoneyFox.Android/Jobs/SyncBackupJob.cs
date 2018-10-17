using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using MoneyFox.Business;
using MoneyFox.Business.Adapter;
using MoneyFox.Business.Manager;
using MoneyFox.Business.Services;
using MoneyFox.Droid.OneDriveAuth;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Interfaces;
using MvvmCross;
using MvvmCross.Plugin.File;
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
            catch (RemoteException e)
            {
                Debug.WriteLine(e);
            }
            return StartCommandResult.NotSticky;
        }

        private async Task SyncBackups(JobParameters args)
        {
            if (!Mvx.IoCProvider.CanResolve<IMvxFileStore>()) return;

            try
            {
                DataAccess.ApplicationContext.DbPath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                                 DatabaseConstants.DB_NAME);

                await new BackupManager(new OneDriveService(new OneDriveAuthenticator()),
                                        Mvx.IoCProvider.Resolve<IMvxFileStore>(),
                                        new SettingsManager(new SettingsAdapter()),
                                        new ConnectivityImplementation())
                    .DownloadBackup();

                JobFinished(args, false);
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }

        /// <summary>
        ///     Schedules the task for execution.
        /// </summary>
        public void ScheduleTask(int interval)
        {
            if(!Mvx.IoCProvider.CanResolve<ISettingsManager>()) return;

            if (!Mvx.IoCProvider.Resolve<ISettingsManager>().IsBackupAutouploadEnabled) return;

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