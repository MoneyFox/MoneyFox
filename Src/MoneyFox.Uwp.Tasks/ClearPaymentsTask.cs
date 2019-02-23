using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.BusinessLogic.PaymentActions;
using MoneyFox.DataLayer;
using MoneyFox.Foundation.Constants;
using MoneyFox.ServiceLayer.Facades;

namespace MoneyFox.Uwp.Tasks
{
    /// <summary>
    ///     Background task to periodically clear payments.
    /// </summary>
    public sealed class ClearPaymentsTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            Debug.WriteLine("ClearPayment started");
            EfCoreContext.DbPath = DatabaseConstants.DB_NAME;
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            try
            {
                var context = new EfCoreContext();
                await new ClearPaymentAction(new ClearPaymentDbAccess(context))
                    .ClearPayments()
                    .ConfigureAwait(true);
                context.SaveChanges();
            } 
            catch (Exception ex)
            {
                Debug.Write(ex);
                Debug.WriteLine("ClearPaymentTask stopped due to an error.");

            } finally
            {
                settingsFacade.LastExecutionTimeStampClearPayments = DateTime.Now;
                Debug.WriteLine("ClearPaymentTask finished.");
                deferral.Complete();
            }
        }
    }
}