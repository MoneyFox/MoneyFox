using System;
using Android.App;
using Android.Content;
using MoneyFox.Droid.Services;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Platform.Droid.Platform;

namespace MoneyFox.Droid
{
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private readonly Activity currentActivity;
        private readonly ISettingsManager settingsManager;

        public BackgroundTaskManager(IMvxAndroidCurrentTopActivity currentActivity, ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
            this.currentActivity = currentActivity.Activity;
        }


        public void StartBackgroundTasks()
        {
            var pendingIntentClearPayments = PendingIntent.GetService(currentActivity, 0, new Intent(currentActivity, typeof(ClearPaymentService)), PendingIntentFlags.UpdateCurrent);
            var pendingIntentRecurringPayments = PendingIntent.GetService(currentActivity, 0, new Intent(currentActivity, typeof(RecurringPaymentService)), PendingIntentFlags.UpdateCurrent);
            var pendingIntentSyncBackups = PendingIntent.GetService(currentActivity, 0, new Intent(currentActivity, typeof(SyncBackupService)), PendingIntentFlags.UpdateCurrent);

            var alarmmanager = (AlarmManager)currentActivity.GetSystemService(Context.AlarmService);

            // The task will be executed all 1 hours.
            alarmmanager.SetInexactRepeating(AlarmType.RtcWakeup, 10000, 3600000, pendingIntentClearPayments);
            alarmmanager.SetInexactRepeating(AlarmType.RtcWakeup, 10000, 3600000, pendingIntentRecurringPayments);
            StartSyncBackupService();
        }

        public void StartBackupSyncTask()
        {
            StartSyncBackupService();
        }

        public void StopBackupSyncTask()
        {
            throw new NotImplementedException();
        }

        private void StartSyncBackupService()
        {
            var pendingIntentSyncBackups = PendingIntent.GetService(currentActivity, 0, new Intent(currentActivity, typeof(SyncBackupService)), PendingIntentFlags.UpdateCurrent);
            var alarmmanager = (AlarmManager)currentActivity.GetSystemService(Context.AlarmService);

            if (settingsManager.IsBackupAutouploadEnabled)
            {
                // Convert hours to milliseconds
                var interval = 60 * 60 * 1000 * settingsManager.BackupSyncRecurrence;
                alarmmanager.SetInexactRepeating(AlarmType.RtcWakeup, 10000, interval, pendingIntentSyncBackups);
            }
        }
    }
}