using Windows.ApplicationModel.Background;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Windows.Tasks
{
    public sealed class RecurringPaymentTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            try
            {
                var dbFactory = new DbFactory();

                new RecurringPaymentManager(
                        new RecurringPaymentService(new UnitOfWork(dbFactory)),
                        new PaymentService(new UnitOfWork(dbFactory)))
                    .CreatePaymentsUpToRecur();
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}