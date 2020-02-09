using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using CommonServiceLocator;
using MediatR;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Payments.Commands.CreateRecurringPayments;
using NLog;

namespace MoneyFox.Uwp.BackgroundTasks
{
    /// <summary>
    ///     Periodically tests if there are new recurring payments and creates these.
    /// </summary>
    public sealed class RecurringPaymentTask : BackgroundTask
    {
        private const int TASK_RECURRENCE_COUNT = 240;

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

            logManager.Debug("RecurringPaymentTask started.");
            return Task.Run(async () =>
                            {
                                this.taskInstance = taskInstance;
                                await CreateRecurringPaymentsAsync();
                            });
        }

        public override void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            logManager.Debug("RecurringPaymentTask Canceled.");
        }

        private async Task CreateRecurringPaymentsAsync()
        {
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            try
            {
                taskInstance.Progress = 10;

                var mediator = ServiceLocator.Current.GetInstance<IMediator>();
                await mediator.Send(new CreateRecurringPaymentsCommand());

                taskInstance.Progress = 100;
            }
            catch (Exception ex)
            {
                logManager.Error(ex, "RecurringPaymentTask stopped due to an error.");
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampRecurringPayments = DateTime.Now;
                logManager.Debug("RecurringPaymentTask finished.");
            }

            deferral?.Complete();
        }
    }
}
