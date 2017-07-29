using Windows.ApplicationModel.Background;
using Microsoft.Toolkit.Uwp;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Windows.Tasks;

namespace MoneyFox.Windows
{
    /// <inheritdoc />
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private readonly ISettingsManager settingsManager;

        /// <summary>
        ///     Constructor
        /// </summary>
        public BackgroundTaskManager(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        /// <inheritdoc />
        public void StartBackupSyncTask()
        {
            BackgroundTaskRegistration registered =
                BackgroundTaskHelper.Register(typeof(SyncBackupTask),
                                              new TimeTrigger((uint) (settingsManager.BackupSyncRecurrence * 60),
                                                              true));
        }

        /// <inheritdoc />
        public void StopBackupSyncTask()
        {
            BackgroundTaskHelper.Unregister(typeof(SyncBackupTask));
        }
    }
}