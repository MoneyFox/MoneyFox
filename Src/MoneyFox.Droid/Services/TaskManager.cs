using Android.App;
using Android.Content;
using MoneyFox.Droid.Business.Services;
using MoneyFox.Shared.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace MoneyFox.Droid.Services
{
    public class TaskManager : IBackgroundTaskManager
    {
        protected Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public void StartBackgroundTask()
        {
            var intent = new Intent(CurrentActivity, typeof(ClearPaymentService));
            var pendingIntent = PendingIntent.GetService(CurrentActivity, 0, intent, 0);

            var alarmmanager = (AlarmManager)CurrentActivity.GetSystemService(Activity.AlarmService);

            // The task will be executed all 6 hours.
            alarmmanager.SetInexactRepeating(AlarmType.RtcWakeup, 21600000, 21600000, pendingIntent);
        }
    }
}