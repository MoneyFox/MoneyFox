using Windows.ApplicationModel.Background;
using Microsoft.Toolkit.Uwp.Helpers;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.Uwp.Tasks;

namespace MoneyFox.Uwp
{
    /// <inheritdoc />
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        /// <inheritdoc />
        public void StartBackupSyncTask(int interval)
        {
            BackgroundTaskHelper.Register(typeof(SyncBackupTask), new TimeTrigger((uint)interval, false));
        }

        /// <inheritdoc />
        public void StopBackupSyncTask()
        {
            BackgroundTaskHelper.Unregister(typeof(SyncBackupTask));
        }
    }
}