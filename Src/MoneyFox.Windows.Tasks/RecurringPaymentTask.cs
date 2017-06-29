using Windows.ApplicationModel.Background;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.Constants;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Windows.Tasks
{
    public sealed class RecurringPaymentTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            ApplicationContext.DbPath = DatabaseConstants.DB_NAME;

            try
            {
                var dbFactory = new DbFactory();

                new RecurringPaymentManager(
                    new RecurringPaymentService(new RecurringPaymentRepository(dbFactory)),
                        new PaymentService(new PaymentRepository(dbFactory), new UnitOfWork(dbFactory)))
                    .CreatePaymentsUpToRecur();
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}