using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.BusinessLogic.PaymentActions;
using MoneyFox.DataLayer;
using MoneyFox.Domain;
using MoneyFox.Presentation.Facades;
using MoneyFox.Foundation;

namespace MoneyFox.Uwp.Tasks
{
    /// <summary>
    ///     Periodically tests if there are new recurring payments and creates these.
    /// </summary>
    public sealed class RecurringPaymentTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            Debug.WriteLine("RecurringPaymentTask started.");

            ExecutingPlatform.Current = AppPlatform.UWP;
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            try
            {
                var context = new EfCoreContext();
                await new RecurringPaymentAction(new RecurringPaymentDbAccess(context))
                    .CreatePaymentsUpToRecur()
                    ;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                Debug.WriteLine("RecurringPaymentTask stopped due to an error.");

            } 
            finally
            {
                settingsFacade.LastExecutionTimeStampRecurringPayments = DateTime.Now;
                Debug.WriteLine("RecurringPaymentTask finished.");
                deferral.Complete();
            }
        }
    }
}