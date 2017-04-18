using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;

namespace MoneyFox.Windows.Business
{
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private const string TASK_NAMESPACE = "MoneyFox.Windows.Tasks";

        private const string CLEAR_PAYMENT_TASK = "ClearPaymentTask";
        private const string RECURRING_PAYMENT_TASK = "RecurringPaymentTask";
        private const string SYNC_BACKUP_TASK = "SyncBackupTask";

        private readonly ISettingsManager settingsManager;
        private readonly ICategoryRepository categoryRepository;
        private readonly IStartAssistant startAssistant;

        public BackgroundTaskManager(ISettingsManager settingsManager, ICategoryRepository categoryRepository, IStartAssistant startAssistant)
        {
            this.settingsManager = settingsManager;
            this.categoryRepository = categoryRepository;
            this.startAssistant = startAssistant;
        }

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
            startAssistant.AddCategory();
        }

        /// <summary>
        ///     Registers the backup sync task.
        /// </summary>
        public void StartBackupSyncTask()
        {
            RegisterSyncBackupTask();
        }

        /// <summary>
        ///     Unregisters the backup sync task.
        /// </summary>
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
                builder.SetTrigger(new TimeTrigger(60, false));
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
                builder.SetTrigger(new TimeTrigger(60, false));
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
                    TaskEntryPoint = string.Format("{0}.{1}", TASK_NAMESPACE, SYNC_BACKUP_TASK)
                };

                builder.SetTrigger(new TimeTrigger((uint)settingsManager.BackupSyncRecurrence, false));
                builder.Register();
            }
        }
    }
}