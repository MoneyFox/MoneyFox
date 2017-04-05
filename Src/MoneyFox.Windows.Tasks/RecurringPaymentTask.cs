using Windows.ApplicationModel.Background;
using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Repositories;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

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

                var dbManager = new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWindowsCommonFileStore());

                var paymentRepository = new PaymentRepository(dbManager);

                var paymentManager = new PaymentManager(paymentRepository,
                    new AccountRepository(dbManager),
                    new RecurringPaymentRepository(dbManager),
                    null);

                PaymentRepository.IsCacheMarkedForReload = true;

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