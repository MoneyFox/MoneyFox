using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using CommonServiceLocator;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using NLog;

namespace MoneyFox.Uwp.BackgroundTasks
{
    public class SyncBackupTask : BackgroundTask
    {
        private const int TASK_RECURRENCE_COUNT = 30;

        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private IBackgroundTaskInstance taskInstance;
        private BackgroundTaskDeferral deferral;


        public override void Register()
        {
            var taskName = GetType().Name;
            var taskRegistration = BackgroundTaskRegistration.AllTasks.FirstOrDefault(t => t.Value.Name == taskName).Value;

            if (taskRegistration == null)
            {
                var builder = new BackgroundTaskBuilder
                {
                    Name = taskName
                };

                builder.SetTrigger(new TimeTrigger(TASK_RECURRENCE_COUNT, false));
                builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));

                builder.Register();
            }
        }

        public override Task RunAsyncInternal(IBackgroundTaskInstance taskInstance)
        {
            if (taskInstance == null)
            {
                return Task.FromResult<Task>(null);
            }

            deferral = taskInstance.GetDeferral();

            return Task.Run(async () =>
                            {
                                this.taskInstance = taskInstance;
                                await SynBackupAsync();
                            });
        }

        public override void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            logManager.Debug("Sync Backup canceled.");
        }

        private async Task SynBackupAsync()
        {
            logManager.Debug("Sync Backup started.");

            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            if (!settingsFacade.IsBackupAutouploadEnabled || !settingsFacade.IsLoggedInToBackupService) return;

            try
            {
                taskInstance.Progress = 10;

                var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                await backupService.RestoreBackupAsync();

                taskInstance.Progress = 100;
            }
            catch (Exception ex)
            {
                logManager.Error(ex, "Sync Backup failed.");
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
                logManager.Debug("Sync Backup finished.");
            }
            deferral?.Complete();
        }
    }
}
