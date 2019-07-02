using Windows.ApplicationModel.Background;
using Microsoft.Toolkit.Uwp.Helpers;
using MoneyFox.Uwp.Tasks;

namespace MoneyFox.Uwp.Services
{
    internal static class BackgroundTaskService
    {
        public static void RegisterBackgroundTasks()
        {
            BackgroundTaskHelper.Register(typeof(ClearPaymentsTask), new TimeTrigger(60, false));
            BackgroundTaskHelper.Register(typeof(RecurringPaymentTask), new TimeTrigger(60, false));
            BackgroundTaskHelper.Register(typeof(LiveTiles), new TimeTrigger(15, false));
        }
    }
}
