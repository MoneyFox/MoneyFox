using Windows.ApplicationModel.Background;

namespace MoneyFox.Uwp.Services
{
    internal static class BackgroundTaskService
    {
        private const int TASK_RECURRENCE_COUNT = 60;

        public static void RegisterBackgroundTasks()
        {
            var builder = new BackgroundTaskBuilder();
            builder.Name = "BackgroundTasks";
            builder.SetTrigger(new TimeTrigger(TASK_RECURRENCE_COUNT, true));

            builder.Register();
        }
    }
}
