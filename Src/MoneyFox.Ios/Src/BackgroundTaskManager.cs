using System;
using MoneyFox.Presentation.Interfaces;

namespace MoneyFox.iOS
{
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        public void StartBackupSyncTask(int interval)
        {
            throw new NotSupportedException();
        }

        public void StopBackupSyncTask()
        {
            throw new NotSupportedException();
        }
    }
}
