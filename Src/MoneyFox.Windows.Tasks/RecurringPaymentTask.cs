using Windows.ApplicationModel.Background;
using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;

namespace MoneyFox.Windows.Tasks
{
    public sealed class RecurringPaymentTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            try
            {
                MapperConfiguration.Setup();

                var dbFactory = new DbFactory();
                var paymentRepository = new PaymentRepository(dbFactory);

                var paymentManager = new PaymentManager(paymentRepository,
                    new AccountRepository(dbFactory),
                    new RecurringPaymentRepository(dbFactory),
                    null);

                new RecurringPaymentManager(paymentManager, paymentRepository,
                    new SettingsManager(new WindowsUwpSettings())).CheckRecurringPayments();
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}