using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using MoneyFox.Droid.Jobs;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Platform.Droid.Platform;
using JobSchedulerType = Android.App.Job.JobScheduler;

namespace MoneyFox.Droid
{
    /// <inheritdoc />
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private const int SYNC_BACK_JOB_ID = 30;

        private readonly Activity currentActivity;

        private Handler handler;

        /// <summary>
        ///     Constructor
        /// </summary>
        public BackgroundTaskManager(IMvxAndroidCurrentTopActivity currentActivity)
        {
            this.currentActivity = currentActivity.Activity;
        }

        /// <inheritdoc />
        public void StopBackgroundTasks()
        {
            var tm = (JobSchedulerType)currentActivity.GetSystemService(Context.JobSchedulerService);
            tm.CancelAll();
        }

        /// <inheritdoc />
        public void StartBackupSyncTask()
        {
            handler = new Handler((Message msg) =>
            {
                var syncBackupJob = (SyncBackupJob) msg.Obj;
                syncBackupJob.ScheduleTask();
            });

            var startServiceIntentSyncBackup = new Intent(currentActivity, typeof(SyncBackupJob));
            startServiceIntentSyncBackup.PutExtra("messenger", new Messenger(handler));
            currentActivity.StartService(startServiceIntentSyncBackup);
        }

        /// <inheritdoc />
        public void StopBackupSyncTask()
        {
            var tm = (JobSchedulerType)currentActivity.GetSystemService(Context.JobSchedulerService);
            tm.Cancel(SYNC_BACK_JOB_ID);
        }
    }
}