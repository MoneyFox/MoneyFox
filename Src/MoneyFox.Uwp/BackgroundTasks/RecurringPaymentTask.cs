using CommonServiceLocator;
using MediatR;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Payments.Commands.CreateRecurringPayments;
using NLog;
using System;
using Windows.ApplicationModel.Background;

namespace MoneyFox.Uwp.BackgroundTasks
{
    /// <summary>
    /// Periodically tests if there are new recurring payments and creates these.
    /// </summary>
    public sealed class RecurringPaymentTask : IBackgroundTask
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            logManager.Debug("RecurringPaymentTask started.");

            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            try
            {
                var mediator = ServiceLocator.Current.GetInstance<IMediator>();
                await mediator.Send(new CreateRecurringPaymentsCommand());
            }
            catch (Exception ex)
            {
                logManager.Error(ex, "RecurringPaymentTask stopped due to an error.");
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampRecurringPayments = DateTime.Now;
                logManager.Debug("RecurringPaymentTask finished.");
                deferral.Complete();
            }
        }
    }
}
