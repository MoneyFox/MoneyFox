using System.Diagnostics;
using Windows.ApplicationModel.Background;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.Constants;
using MoneyFox.Service.DataServices;

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

            try
            {
                var dbFactory = new DbFactory();

                await new RecurringPaymentManager(
                    new RecurringPaymentService(new RecurringPaymentRepository(dbFactory)),
                        new PaymentService(new PaymentRepository(dbFactory), new UnitOfWork(dbFactory)))
                    .CreatePaymentsUpToRecur();
            }
            finally
            {
                Debug.WriteLine("RecurringPaymentTask stopped.");
                deferral.Complete();
            }
        }
    }
}