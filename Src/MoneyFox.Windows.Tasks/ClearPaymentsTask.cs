using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Background;
using EntityFramework.DbContextScope;
using MoneyFox.Business.Adapter;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation.Constants;

namespace MoneyFox.Windows.Tasks
{
    /// <summary>
    ///     Background task to periodically clear payments.
    /// </summary>
    public sealed class ClearPaymentsTask : IBackgroundTask
    {
        private IPaymentService paymentService;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            Debug.WriteLine("ClearPayment started");
            ApplicationContext.DbPath = DatabaseConstants.DB_NAME;
            var settingsManager = new SettingsManager(new SettingsAdapter());

            try
            {
                paymentService = new PaymentService(new AmbientDbContextLocator(), new DbContextScopeFactory());

                var payments = await paymentService.GetUnclearedPayments(DateTime.Now);
                var unclearedPayments = payments.ToList();
                if (unclearedPayments.Any())
                {
                    await paymentService.SavePayments(unclearedPayments.ToArray());
                }
            } 
            catch (Exception ex)
            {
                Debug.Write(ex);
                Debug.WriteLine("ClearPaymentTask stopped due to an error.");

            } finally
            {
                settingsManager.LastExecutionTimeStampClearPayments = DateTime.Now;
                Debug.WriteLine("ClearPaymentTask finished.");
                deferral.Complete();
            }
        }
    }
}