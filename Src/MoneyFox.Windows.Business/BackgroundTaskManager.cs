using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Windows.Business
{
    /// <inheritdoc />
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private const string TASK_NAMESPACE = "MoneyFox.Windows.Tasks";

        private const string CLEAR_PAYMENT_TASK = "ClearPaymentTask";
        private const string RECURRING_PAYMENT_TASK = "RecurringPaymentTask";
        private const string SYNC_BACKUP_TASK = "SyncBackupTask";

        private readonly ISettingsManager settingsManager;

        public BackgroundTaskManager(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        /// <inheritdoc />
        public async void StartBackgroundTasks()
        {
            var requestAccess = await BackgroundExecutionManager.RequestAccessAsync();

            if (requestAccess == BackgroundAccessStatus.DeniedByUser 
                || requestAccess == BackgroundAccessStatus.DeniedBySystemPolicy) return;

            RegisterClearPaymentTask();
            RegisterRecurringPaymentTask();
            if (settingsManager.IsBackupAutouploadEnabled)
            {
                RegisterSyncBackupTask();
            }
        }

        /// <inheritdoc />
        public void StopBackgroundTasks()
        {
            if (BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == CLEAR_PAYMENT_TASK))
            {
                BackgroundTaskRegistration.AllTasks.First(task => task.Value.Name == CLEAR_PAYMENT_TASK).Value.Unregister(true);
            }

            if (BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == RECURRING_PAYMENT_TASK))
            {
                BackgroundTaskRegistration.AllTasks.First(task => task.Value.Name == RECURRING_PAYMENT_TASK).Value.Unregister(true);
            }

            if (BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == SYNC_BACKUP_TASK))
            {
                BackgroundTaskRegistration.AllTasks.First(task => task.Value.Name == SYNC_BACKUP_TASK).Value.Unregister(true);
            }
        }

        /// <inheritdoc />
        public void StartBackupSyncTask()
        {
            RegisterSyncBackupTask();
        }

        /// <inheritdoc />
        public void StopBackupSyncTask()
        {
            if (BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == SYNC_BACKUP_TASK))
            {
                BackgroundTaskRegistration.AllTasks.First(task => task.Value.Name == SYNC_BACKUP_TASK).Value.Unregister(true);
            }
        }

        private void RegisterClearPaymentTask()
        {
            if (BackgroundTaskRegistration.AllTasks.All(task => task.Value.Name != CLEAR_PAYMENT_TASK))
            {
                var builder = new BackgroundTaskBuilder
                {
                    Name = CLEAR_PAYMENT_TASK,
                    TaskEntryPoint = string.Format("{0}.{1}", TASK_NAMESPACE, CLEAR_PAYMENT_TASK)
                };

                // Task will be executed all 1 hours
                builder.SetTrigger(new TimeTrigger(180, false));
                builder.Register();
            }
        }

        private void RegisterRecurringPaymentTask()
        {
            if (BackgroundTaskRegistration.AllTasks.All(task => task.Value.Name != RECURRING_PAYMENT_TASK))
            {
                var builder = new BackgroundTaskBuilder
                {
                    Name = RECURRING_PAYMENT_TASK,
                    TaskEntryPoint = string.Format("{0}.{1}", TASK_NAMESPACE, RECURRING_PAYMENT_TASK)
                };

                // Task will be executed all 1 hours
                builder.SetTrigger(new TimeTrigger(180, false));
                builder.Register();
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

                builder.SetTrigger(new TimeTrigger((uint)(settingsManager.BackupSyncRecurrence * 60), false));
                builder.Register();
            }
        }
    }
}