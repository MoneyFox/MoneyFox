using System;
using Android.App;
using Android.Content;
using MoneyFox.Droid.Services;
using MoneyFox.Shared.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace MoneyFox.Droid
{
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        protected Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public void StartBackgroundTask()
        {
            StartClearingService();
            StartRecurringPaymentService();
            StartSyncBackupService();
        }

        private void StartClearingService()
        {
            var intent = new Intent(CurrentActivity, typeof(ClearPaymentService));
            var pendingIntent = PendingIntent.GetService(CurrentActivity, 0, intent, 0);

            var alarmmanager = (AlarmManager)CurrentActivity.GetSystemService(Context.AlarmService);

            // The task will be executed all 6 hours.
            alarmmanager.SetInexactRepeating(AlarmType.RtcWakeup, 21600000, 21600000, pendingIntent);
        }

        private void StartRecurringPaymentService()
        {
            var intent = new Intent(CurrentActivity, typeof(ClearPaymentService));
            var pendingIntent = PendingIntent.GetService(CurrentActivity, 0, intent, 0);

            var alarmmanager = (AlarmManager)CurrentActivity.GetSystemService(Context.AlarmService);

            alarmmanager.SetRepeating(AlarmType.RtcWakeup, DateTime.Today.AddHours(2).Millisecond, AlarmManager.IntervalDay, pendingIntent);
        }

        private void StartSyncBackupService() {
            var intent = new Intent(CurrentActivity, typeof(SyncBackupService));
            var pendingIntent = PendingIntent.GetService(CurrentActivity, 0, intent, 0);

            var alarmmanager = (AlarmManager)CurrentActivity.GetSystemService(Context.AlarmService);

            // The task will be executed all 6 hours.
            alarmmanager.SetInexactRepeating(AlarmType.RtcWakeup, 10800000, 10800000, pendingIntent);
        }
    }
}