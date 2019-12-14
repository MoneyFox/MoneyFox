using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using MoneyFox.Application;
using MoneyFox.Application.Adapters;
using MoneyFox.Application.Facades;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.BusinessLogic.PaymentActions;
using MoneyFox.Persistence;
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
            ExecutingPlatform.Current = AppPlatform.UWP;

            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            try
            {
                EfCoreContext context = EfCoreContextFactory.Create();
                await new ClearPaymentAction(new ClearPaymentDbAccess(context))
                    .ClearPaymentsAsync();
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logManager.Warn(ex, "ClearPaymentTask stopped due to an error.");
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
