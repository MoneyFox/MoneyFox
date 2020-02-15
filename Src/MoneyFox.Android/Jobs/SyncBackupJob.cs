using System;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using CommonServiceLocator;
using Java.Lang;
using MediatR;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Payments.Commands.ClearPayments;
using MoneyFox.Application.Payments.Commands.CreateRecurringPayments;
using NLog;
using Exception = System.Exception;
using JobSchedulerType = Android.App.Job.JobScheduler;

#pragma warning disable S927 // parameter names should match base declaration and other partial definitions: Not possible since base uses reserver word.
namespace MoneyFox.Droid.Jobs
{
    /// <summary>
    ///     Job to periodically sync backup.
    /// </summary>
    [Service(Exported = true, Permission = "android.permission.BIND_JOB_SERVICE")]
    public class SyncBackupJob : JobService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const int SYNC_BACK_JOB_ID = 30;
        private const int JOB_INTERVAL = 30 * 60 * 1000;

        /// <inheritdoc />
        public override bool OnStartJob(JobParameters args)
        {
            Task.Run(async () => await SyncBackupsAsync(args));

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
            var callback = (Messenger) intent.GetParcelableExtra("messenger");
            Message m = Message.Obtain();
            m.What = MainActivity.MessageServiceSyncBackup;
            m.Obj = this;
            try
            {
                callback.Send(m);
            }
            catch (RemoteException e)
            {
                logger.Error(e, "OnStart Create SyncBackup Job.");
            }

            return StartCommandResult.NotSticky;
        }

        private async Task SyncBackupsAsync(JobParameters args)
        {
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            if (!settingsFacade.IsBackupAutouploadEnabled || !settingsFacade.IsLoggedInToBackupService) return;

            try
            {
                var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                await backupService.RestoreBackupAsync();

                var mediator = ServiceLocator.Current.GetInstance<IMediator>();
                await mediator.Send(new ClearPaymentsCommand());
                await mediator.Send(new CreateRecurringPaymentsCommand());

                logger.Info("Backup synced.");
                JobFinished(args, false);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
                throw;
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            }
        }

        /// <summary>
        ///     Schedules the task for execution.
        /// </summary>
        public void ScheduleTask()
        {
            if (!ServiceLocator.Current.GetInstance<ISettingsFacade>().IsBackupAutouploadEnabled) return;

            var builder = new JobInfo.Builder(SYNC_BACK_JOB_ID,
                                              new ComponentName(
                                                                this, Class.FromType(typeof(SyncBackupJob))));

            // convert hours into millisecond
            builder.SetPeriodic(JOB_INTERVAL);
            builder.SetPersisted(true);
            builder.SetRequiredNetworkType(NetworkType.NotRoaming);
            builder.SetRequiresDeviceIdle(false);
            builder.SetRequiresCharging(false);

            var tm = (JobSchedulerType) GetSystemService(JobSchedulerService);
            tm.Schedule(builder.Build());
        }
    }
}
