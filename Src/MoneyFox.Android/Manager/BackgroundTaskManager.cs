using Android.App;
using Android.Content;
using Android.OS;
using MoneyFox.Droid.Jobs;
using MoneyFox.Foundation;
using MoneyFox.Presentation.Interfaces;
using JobSchedulerType = Android.App.Job.JobScheduler;

namespace MoneyFox.Droid.Manager
{
    /// <inheritdoc />
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private const int SYNC_BACK_JOB_ID = 30;

        private readonly Activity currentActivity;
        
        /// <summary>
        ///     Constructor
        /// </summary>
        public BackgroundTaskManager()
        {
            currentActivity = ParentActivityWrapper.ParentActivity as Activity;
        }

        /// <inheritdoc />
        public void StartBackupSyncTask(int interval)
        {
            var handler = new Handler(msg =>
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