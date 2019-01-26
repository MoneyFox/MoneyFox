using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.DataLayer;
using MoneyFox.Foundation.Constants;
using MoneyFox.ServiceLayer.Facades;

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

            EfCoreContext.DbPath = DatabaseConstants.DB_NAME;
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            try
            {
                // TODO Reactivate
                //await new RecurringPaymentManager(
                //        new RecurringPaymentService(ambientDbContextLocator, dbContextScopeFactory),
                //        new PaymentService(ambientDbContextLocator, dbContextScopeFactory))
                //    .CreatePaymentsUpToRecur();
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