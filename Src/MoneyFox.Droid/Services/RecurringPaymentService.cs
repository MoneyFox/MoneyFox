using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Cheesebaron.MvxPlugins.Settings.Droid;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Repositories;
using MvvmCross.Plugins.File.Droid;
using MvvmCross.Plugins.Sqlite.Droid;

namespace MoneyFox.Droid.Services
{
    [Service]
    public class RecurringPaymentService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return new Binder();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Task.Run(() => CheckRecurringPayments());

            return base.OnStartCommand(intent, flags, startId);
        }

        private void CheckRecurringPayments()
        {
            var dbManager = new DatabaseManager(new DroidSqliteConnectionFactory(), new MvxAndroidFileStore());

            var paymentRepository = new PaymentRepository(new PaymentDataAccess(dbManager));

            var paymentManager = new PaymentManager(paymentRepository,
                new AccountRepository(new AccountDataAccess(dbManager)),
                new RecurringPaymentRepository(new RecurringPaymentDataAccess(dbManager)),
                null);

            new RecurringPaymentManager(paymentManager, paymentRepository, new SettingsManager(new Settings())).CheckRecurringPayments();
        }
    }
}