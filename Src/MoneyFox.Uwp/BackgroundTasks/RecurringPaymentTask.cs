using MoneyFox.Application;
using MoneyFox.Application.Adapters;
using MoneyFox.Application.Facades;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.BusinessLogic.PaymentActions;
using MoneyFox.Persistence;
using NLog;
using System;
using System.Diagnostics;
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

            ExecutingPlatform.Current = AppPlatform.UWP;
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            try
            {
                EfCoreContext context = EfCoreContextFactory.Create();
                await new RecurringPaymentAction(new RecurringPaymentDbAccess(context))
                    .CreatePaymentsUpToRecur();
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                logManager.Warn(ex, "RecurringPaymentTask stopped due to an error.");
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
