using Windows.ApplicationModel.Background;
using Microsoft.Toolkit.Uwp.Helpers;
using MoneyFox.Uwp.Tasks;

namespace MoneyFox.Uwp.Services
{
    internal static class BackgroundTaskService
    {
        private const int TASK_RECURRENCE_COUNT = 60;

        public static void RegisterBackgroundTasks()
        {
            BackgroundTaskHelper.Register(typeof(ClearPaymentsTask), new TimeTrigger(TASK_RECURRENCE_COUNT, false));
            BackgroundTaskHelper.Register(typeof(RecurringPaymentTask), new TimeTrigger(TASK_RECURRENCE_COUNT, false));
            BackgroundTaskHelper.Register(typeof(SyncBackupTask), new TimeTrigger(TASK_RECURRENCE_COUNT, false));
        }
    }
}
