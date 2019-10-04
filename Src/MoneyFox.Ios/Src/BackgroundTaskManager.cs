using MoneyFox.Presentation.Interfaces;

namespace MoneyFox.iOS
{
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        public void StartBackupSyncTask(int interval)
        {
            // Not needed on iOS
        }

        public void StopBackupSyncTask()
        {
            // Not needed on iOS
        }
    }
}
