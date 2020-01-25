using System;
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
    public sealed class ClearPaymentsTask : IBackgroundTask
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            logManager.Debug("ClearPayment started");

            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            try
            {
                var mediator = ServiceLocator.Current.GetInstance<IMediator>();
                await mediator.Send(new ClearPaymentsCommand());
            }
            catch (Exception ex)
            {
                logManager.Error(ex, "ClearPaymentTask stopped due to an error.");
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampClearPayments = DateTime.Now;
                logManager.Debug("ClearPaymentTask finished.");
                deferral.Complete();
            }
        }
    }
}
