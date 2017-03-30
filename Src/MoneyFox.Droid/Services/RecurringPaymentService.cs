using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Cheesebaron.MvxPlugins.Settings.Droid;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Repositories;
using MvvmCross.Plugins.File.Droid;
using MvvmCross.Plugins.Sqlite.Droid;

namespace MoneyFox.Droid.Services
{
    [Service]
    public class RecurringPaymentService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Task.Run(() => CheckRecurringPayments());
            return StartCommandResult.RedeliverIntent;
        }

        private void CheckRecurringPayments()
        {
            var dbManager = new DatabaseManager(new DroidSqliteConnectionFactory(), new MvxAndroidFileStore());
            var paymentRepository = new PaymentRepository(dbManager);

            var paymentManager = new PaymentManager(paymentRepository,
                new AccountRepository(dbManager),
                new RecurringPaymentRepository(dbManager),
                null);

            new RecurringPaymentManager(paymentManager, paymentRepository, new SettingsManager(new Settings())).CheckRecurringPayments();
            paymentRepository.ReloadCache();
        }
    }
}