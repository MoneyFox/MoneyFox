using System;
using System.Diagnostics;
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
    ///     Periodically tests if there are new recurring payments and creates these.
    /// </summary>
    public sealed class RecurringPaymentTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            Debug.WriteLine("RecurringPaymentTask started.");

            ApplicationContext.DbPath = DatabaseConstants.DB_NAME;
            var settingsManager = new SettingsManager(new SettingsAdapter());

            try
            {
                var dbContextScopeFactory = new DbContextScopeFactory();
                var ambientDbContextLocator = new AmbientDbContextLocator();

                await new RecurringPaymentManager(
                        new RecurringPaymentService(ambientDbContextLocator, dbContextScopeFactory),
                        new PaymentService(ambientDbContextLocator, dbContextScopeFactory))
                    .CreatePaymentsUpToRecur();
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                Debug.WriteLine("RecurringPaymentTask stopped due to an error.");

            } 
            finally
            {
                settingsManager.LastExecutionTimeStampRecurringPayments = DateTime.Now;
                Debug.WriteLine("RecurringPaymentTask finished.");
                deferral.Complete();
            }
        }
    }
}