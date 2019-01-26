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
                //var payments = await paymentService.GetUnclearedPayments(DateTime.Now);
                //var unclearedPayments = payments.ToList();
                //if (unclearedPayments.Any())
                //{
                //    await paymentService.SavePayments(unclearedPayments.ToArray());
                //}
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