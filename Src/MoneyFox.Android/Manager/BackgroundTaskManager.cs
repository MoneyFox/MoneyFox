using Android.App;
using Android.Content;
using Android.OS;
using MoneyFox.Droid.Jobs;
using MoneyFox.ServiceLayer.Interfaces;
using MvvmCross.Platforms.Android;
using JobSchedulerType = Android.App.Job.JobScheduler;

namespace MoneyFox.Droid.Manager
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
        public void StartBackupSyncTask(int interval)
        {
            handler = new Handler(msg =>
            {
                var syncBackupJob = (SyncBackupJob) msg.Obj;
                syncBackupJob.ScheduleTask(interval);
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