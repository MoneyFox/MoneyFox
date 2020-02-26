using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using CommonServiceLocator;
using MediatR;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Payments.Commands.ClearPayments;
using MoneyFox.Application.Payments.Commands.CreateRecurringPayments;
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
            string taskName = GetType().Name;
            IBackgroundTaskRegistration taskRegistration =
                BackgroundTaskRegistration.AllTasks.FirstOrDefault(t => t.Value.Name == taskName).Value;

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
                logManager.Info("Taskinstance was null.");
                return Task.FromResult<Task>(null);
            }

            deferral = taskInstance.GetDeferral();

            logManager.Info("Sync Backup started.");
            return Task.Run(async () =>
                            {
                                this.taskInstance = taskInstance;
                                await SynBackupAsync();
                            });
        }

        public override void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            logManager.Info("Sync Backup canceled.");
        }

        private async Task SynBackupAsync()
        {
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            var mediator = ServiceLocator.Current.GetInstance<IMediator>();

            if (!settingsFacade.IsBackupAutouploadEnabled || !settingsFacade.IsLoggedInToBackupService)
            {
                // if auto backup not enabled or not logged in, just execute the other tasks.
                await mediator.Send(new ClearPaymentsCommand());
                await mediator.Send(new CreateRecurringPaymentsCommand());
                return;
            }

            try
            {
                taskInstance.Progress = 10;

                var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                await backupService.RestoreBackupAsync();

                await mediator.Send(new ClearPaymentsCommand());
                await mediator.Send(new CreateRecurringPaymentsCommand());

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
