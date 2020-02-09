using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using CommonServiceLocator;
using MediatR;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Payments.Commands.ClearPayments;
using NLog;

namespace MoneyFox.Uwp.BackgroundTasks
{
    /// <summary>
    ///     Background task to periodically clear payments.
    /// </summary>
    public sealed class ClearPaymentsTask : BackgroundTask
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
            if(taskInstance == null)
            {
                return Task.FromResult<Task>(null);
            }

            deferral = taskInstance.GetDeferral();

            return Task.Run(async () =>
                            {
                                this.taskInstance = taskInstance;
                                await ClearPaymentsAsync();
                            });
        }

        public override void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            logManager.Debug("ClearPayment Canceled");
        }

        private async Task ClearPaymentsAsync()
        {
            logManager.Debug("ClearPayment started");

            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            try
            {
                taskInstance.Progress = 10;

                var mediator = ServiceLocator.Current.GetInstance<IMediator>();
                await mediator.Send(new ClearPaymentsCommand());

                taskInstance.Progress = 100;
            }
            catch (Exception ex)
            {
                logManager.Error(ex, "ClearPaymentTask stopped due to an error.");
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampClearPayments = DateTime.Now;
                logManager.Debug("ClearPaymentTask finished.");
            }

            deferral?.Complete();
        }
    }
}
