using System;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.iOS.Src
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
