using System.Linq;
using Windows.ApplicationModel.Background;

namespace BackgroundTask
{
    public sealed class TransactionTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            if (IsTaskExisting()) return;

        }

        private bool IsTaskExisting()
        {
            return BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == name);
        }
    }
}