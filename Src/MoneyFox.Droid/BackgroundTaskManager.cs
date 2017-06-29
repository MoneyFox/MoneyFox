using Android.App;
using Android.App.Job;
using Android.Content;
using MoneyFox.Droid.Jobs;
using MoneyFox.Droid.Services;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Platform.Droid.Platform;
using JobSchedulerType = Android.App.Job.JobScheduler;

namespace MoneyFox.Droid
{
    /// <inheritdoc />
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private const int CLEARPAYMENT_JOB_ID = 10;
        private const int RECURRING_PAYMENT_JOB_ID = 20;
        private const int SYNC_BACK_JOB_ID = 30;

        private readonly Activity currentActivity;
        private readonly ISettingsManager settingsManager;

        /// <summary>
        ///     Constructor
        /// </summary>
        public BackgroundTaskManager(IMvxAndroidCurrentTopActivity currentActivity, ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
            this.currentActivity = currentActivity.Activity;
        }

        /// <inheritdoc />
        public void StopBackgroundTasks()
        {
            var tm = (JobSchedulerType)currentActivity.GetSystemService(Context.JobSchedulerService);
            tm.CancelAll();
        }

        /// <inheritdoc />
        public void StartBackgroundTasks()
        {
            var tm = (JobSchedulerType)currentActivity.GetSystemService(Context.JobSchedulerService);
            tm.Schedule(GetClearPaymentJobInfo());
            tm.Schedule(GetRecurringPaymentJobInfo());
            if (settingsManager.IsBackupAutouploadEnabled)
            {
                tm.Schedule(GetSyncBackupJobInfo());
            }
            else
            {
                tm.Cancel(SYNC_BACK_JOB_ID);
            }
        }

        /// <inheritdoc />
        public void StartBackupSyncTask()
        {
            var tm = (JobSchedulerType)currentActivity.GetSystemService(Context.JobSchedulerService);
            tm.Schedule(GetSyncBackupJobInfo());
        }

        /// <inheritdoc />
        public void StopBackupSyncTask()
        {
            var tm = (JobSchedulerType)currentActivity.GetSystemService(Context.JobSchedulerService);
            tm.Cancel(SYNC_BACK_JOB_ID);
        }

        private JobInfo GetClearPaymentJobInfo()
        {
            var builder = new JobInfo.Builder(CLEARPAYMENT_JOB_ID,
                                              new ComponentName(currentActivity,
                                                                Java.Lang.Class.FromType(typeof(ClearPaymentsJob))));
           
            // Execute all 30 Minutes
            builder.SetPeriodic(30 * 60 * 1000);
            builder.SetPersisted(true);
            builder.SetRequiredNetworkType(NetworkType.None);
            builder.SetRequiresDeviceIdle(false);
            builder.SetRequiresCharging(false);
            return builder.Build();
        }

        private JobInfo GetRecurringPaymentJobInfo()
        {
            var builder = new JobInfo.Builder(RECURRING_PAYMENT_JOB_ID,
                                              new ComponentName(currentActivity,
                                                                Java.Lang.Class.FromType(typeof(RecurringPaymentJob))));

            // Execute all 30 Minutes
            builder.SetPeriodic(30 * 60 * 1000);
            builder.SetPersisted(true);
            builder.SetRequiredNetworkType(NetworkType.None);
            builder.SetRequiresDeviceIdle(false);
            builder.SetRequiresCharging(false);
            return builder.Build();
        }

        private JobInfo GetSyncBackupJobInfo()
        {
            var builder = new JobInfo.Builder(SYNC_BACK_JOB_ID,
                                              new ComponentName(currentActivity,
                                                                Java.Lang.Class.FromType(typeof(SyncBackupJob))));

            builder.SetPeriodic(60 * 60 * 1000 * settingsManager.BackupSyncRecurrence);
            builder.SetPersisted(true);
            builder.SetRequiredNetworkType(NetworkType.NotRoaming);
            builder.SetRequiresDeviceIdle(false);
            builder.SetRequiresCharging(false);
            return builder.Build();
        }
    }
}