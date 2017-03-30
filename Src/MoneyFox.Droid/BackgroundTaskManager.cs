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


        public void StartBackgroundTask()
        {
            var pendingIntentClearPayments = PendingIntent.GetService(currentActivity, 0, new Intent(currentActivity, typeof(ClearPaymentService)), PendingIntentFlags.UpdateCurrent);
            var pendingIntentRecurringPayments = PendingIntent.GetService(currentActivity, 0, new Intent(currentActivity, typeof(RecurringPaymentService)), PendingIntentFlags.UpdateCurrent);
            var pendingIntentSyncBackups = PendingIntent.GetService(currentActivity, 0, new Intent(currentActivity, typeof(SyncBackupService)), PendingIntentFlags.UpdateCurrent);

            var alarmmanager = (AlarmManager)currentActivity.GetSystemService(Context.AlarmService);

            // The task will be executed all 6 hours.
            alarmmanager.SetInexactRepeating(AlarmType.RtcWakeup, 10000, 10000, pendingIntentClearPayments);
            alarmmanager.SetInexactRepeating(AlarmType.RtcWakeup, 10000, 10000, pendingIntentRecurringPayments);

            if (settingsManager.IsBackupAutouploadEnabled)
            {
                alarmmanager.SetInexactRepeating(AlarmType.RtcWakeup, 10000, 10000, pendingIntentSyncBackups);
            }
        }

        private void StartClearingService()
        {
            var intent = new Intent(currentActivity, typeof(ClearPaymentService));
            var pendingIntent = PendingIntent.GetService(currentActivity, 0, intent, 0);

            var alarmmanager = (AlarmManager)currentActivity.GetSystemService(Context.AlarmService);

            // The task will be executed all 6 hours.
            alarmmanager.SetInexactRepeating(AlarmType.RtcWakeup, 21600000, 21600000, pendingIntent);
        }

        private void StartRecurringPaymentService()
        {
            var intent = new Intent(currentActivity, typeof(ClearPaymentService));
            var pendingIntent = PendingIntent.GetService(currentActivity, 0, intent, 0);

            var alarmmanager = (AlarmManager)currentActivity.GetSystemService(Context.AlarmService);

            alarmmanager.SetRepeating(AlarmType.RtcWakeup, DateTime.Today.AddHours(2).Millisecond, AlarmManager.IntervalDay, pendingIntent);
        }

        private void StartSyncBackupService() {
            var intent = new Intent(currentActivity, typeof(SyncBackupService));
            var pendingIntent = PendingIntent.GetService(currentActivity, 0, intent, 0);

            var alarmmanager = (AlarmManager)currentActivity.GetSystemService(Context.AlarmService);

            // The task will be executed all 6 hours.
            alarmmanager.SetInexactRepeating(AlarmType.RtcWakeup, 10800000, 10800000, pendingIntent);
        }
    }
}