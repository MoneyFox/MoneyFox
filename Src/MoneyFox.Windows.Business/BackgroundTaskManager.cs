using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using Microsoft.Toolkit.Uwp;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Windows.Business
{
    /// <inheritdoc />
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private const string TASK_NAMESPACE = "MoneyFox.Windows.Tasks";

        private const string SYNC_BACKUP_TASK = "SyncBackupTask";

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
            BackgroundTaskRegistration registered = BackgroundTaskHelper.Register(typeof(SyncBackupTask), new TimeTrigger(15, true));
        }

        /// <inheritdoc />
        public void StopBackupSyncTask()
        {
            if (BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == SYNC_BACKUP_TASK))
            {
                BackgroundTaskRegistration.AllTasks.First(task => task.Value.Name == SYNC_BACKUP_TASK).Value.Unregister(true);
            }
        }

        private void RegisterSyncBackupTask()
        {
            if (BackgroundTaskRegistration.AllTasks.All(task => task.Value.Name != SYNC_BACKUP_TASK))
            {
                var builder = new BackgroundTaskBuilder
                {
                    Name = SYNC_BACKUP_TASK,
                    TaskEntryPoint = string.Format("{0}.{1}", TASK_NAMESPACE, SYNC_BACKUP_TASK),
                    IsNetworkRequested = true
                };

                builder.SetTrigger(new TimeTrigger((uint) (settingsManager.BackupSyncRecurrence * 60), false));
                builder.Register();
            }
        }
    }
}