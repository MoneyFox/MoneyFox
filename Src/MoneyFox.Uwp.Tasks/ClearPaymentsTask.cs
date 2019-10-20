using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using MoneyFox.Application;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.BusinessLogic.PaymentActions;
using MoneyFox.Persistence;
using MoneyFox.Presentation.Facades;

namespace MoneyFox.Uwp.Tasks
{
    /// <summary>
    ///     Background task to periodically clear payments.
    /// </summary>
    public sealed class ClearPaymentsTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            Debug.WriteLine("ClearPayment started");
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
                Debug.Write(ex);
                Debug.WriteLine("ClearPaymentTask stopped due to an error.");
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampClearPayments = DateTime.Now;
                Debug.WriteLine("ClearPaymentTask finished.");
                deferral.Complete();
            }
        }
    }
}
