using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Microsoft.Identity.Client;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.BusinessLogic.Backup;
using MoneyFox.DataLayer;
using MoneyFox.Foundation.Constants;
using MoneyFox.ServiceLayer.Facades;
using System;
using System.IO;
using System.Threading.Tasks;
using CommonServiceLocator;
using MoneyFox.BusinessLogic.FileStore;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using JobSchedulerType = Android.App.Job.JobScheduler;
using Debug = System.Diagnostics.Debug;
using Environment = System.Environment;
using MoneyFox.Foundation;

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
        public override bool OnStopJob(JobParameters args) => true;

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
            } catch (RemoteException e)
            {
                Debug.WriteLine(e);
            }
            return StartCommandResult.NotSticky;
        }

        private async Task SyncBackups(JobParameters args)
        {
            var settingsFacade = new SettingsFacade(new SettingsAdapter());
            if (!settingsFacade.IsBackupAutouploadEnabled || !settingsFacade.IsLoggedInToBackupService) return;

            try
            {
                ExecutingPlatform.Current = AppPlatform.Android;

                var pca = PublicClientApplicationBuilder
                    .Create(ServiceConstants.MSAL_APPLICATION_ID)
                    .WithRedirectUri($"msal{ServiceConstants.MSAL_APPLICATION_ID}://auth")
                    .Build();

                var backupManager = new BackupManager(
                    new OneDriveService(pca),
                    ServiceLocator.Current.GetInstance<IFileStore>(),
                    new ConnectivityAdapter());

                var backupService = new BackupService(backupManager, settingsFacade);

                var backupDate = await backupService.GetBackupDate();
                if (settingsFacade.LastDatabaseUpdate > backupDate) return;

                await backupService.RestoreBackup();

                JobFinished(args, false);
            } catch (Exception ex)
            {
                Debug.Write(ex);
            } finally
            {
                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            }
        }

        /// <summary>
        ///     Schedules the task for execution.
        /// </summary>
        public void ScheduleTask(int interval)
        {
            if (!ServiceLocator.Current.GetInstance<ISettingsFacade>().IsBackupAutouploadEnabled) return;

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