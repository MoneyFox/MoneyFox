using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Background;
using EntityFramework.DbContextScope;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.Constants;
using MoneyFox.Service.DataServices;

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

            try
            {
                var ambientDbContextLocator = new AmbientDbContextLocator();
                paymentService = new PaymentService(
                    new DbContextScopeFactory(), 
                    new PaymentRepository(ambientDbContextLocator),
                    new RecurringPaymentRepository(ambientDbContextLocator),
                    new AccountRepository(ambientDbContextLocator),
                    ambientDbContextLocator);

                var payments = await paymentService.GetUnclearedPayments(DateTime.Now);
                var unclearedPayments = payments.ToList();
                if (unclearedPayments.Any())
                {
                    await paymentService.SavePayments(unclearedPayments);
                }
            }
            finally
            {
                Debug.WriteLine("ClearPayment stopped.");
                deferral.Complete();
            }
        }
    }
}