using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Windows.Business
{
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private const string TASK_NAMESPACE = "MoneyFox.Windows.Tasks";

        private const string CLEAR_PAYMENT_TASK = "ClearPaymentTask";
        private const string RECURRING_PAYMENT_TASK = "RecurringPaymentTask";
        private const string SYNC_BACKUP_TASK = "SyncBackupTask";

        public async void StartBackgroundTask()
        {
            var requestAccess = await BackgroundExecutionManager.RequestAccessAsync();

            if (requestAccess == BackgroundAccessStatus.DeniedByUser 
                || requestAccess == BackgroundAccessStatus.DeniedBySystemPolicy) return;


            RegisterClearPaymentTask();
            RegisterRecurringPaymentTask();
            RegisterSyncBackupTask();
        }

        private static void RegisterClearPaymentTask()
        {
            if (BackgroundTaskRegistration.AllTasks.All(task => task.Value.Name != CLEAR_PAYMENT_TASK))
            {
                var builder = new BackgroundTaskBuilder
                {
                    Name = CLEAR_PAYMENT_TASK,
                    TaskEntryPoint = string.Format("{0}.{1}", TASK_NAMESPACE, CLEAR_PAYMENT_TASK)
                };
                // Task will be executed all 6 hours
                // 360 = 6 * 60 Minutes
                builder.SetTrigger(new TimeTrigger(360, false));
                builder.Register();
            }
        }

        private static void RegisterRecurringPaymentTask()
        {
            if (BackgroundTaskRegistration.AllTasks.All(task => task.Value.Name != RECURRING_PAYMENT_TASK))
            {
                var builder = new BackgroundTaskBuilder
                {
                    Name = RECURRING_PAYMENT_TASK,
                    TaskEntryPoint = string.Format("{0}.{1}", TASK_NAMESPACE, RECURRING_PAYMENT_TASK)
                };

                //  Task will be executed every day at midnight.
                builder.SetTrigger(new TimeTrigger(MinutesTilMidnight(), false));
                builder.Register();
            }
        }

        private static void RegisterSyncBackupTask()
        {
            if (BackgroundTaskRegistration.AllTasks.All(task => task.Value.Name != SYNC_BACKUP_TASK))
            {
                var builder = new BackgroundTaskBuilder
                {
                    Name = SYNC_BACKUP_TASK,
                    TaskEntryPoint = string.Format("{0}.{1}", TASK_NAMESPACE, SYNC_BACKUP_TASK)
                };
                // Task will be executed all 3 hours
                // 180 = 3 * 60 Minutes
                builder.SetTrigger(new TimeTrigger(180, false));
                builder.Register();
            }
        }

        /// <summary>
        ///     Returns the minutes to 5 minutes after midnight.
        /// </summary>
        private static uint MinutesTilMidnight()
        {
            var tommorowMidnight = DateTime.Today.AddDays(1);
            var timeTilMidnight = tommorowMidnight - DateTime.Now;
            return (uint)timeTilMidnight.TotalMinutes + 5;
        }
    }
}