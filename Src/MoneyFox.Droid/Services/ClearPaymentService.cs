using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using MoneyFox.Shared;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Repositories;
using MvvmCross.Plugins.Sqlite.Droid;

namespace MoneyFox.Droid.Services
{
    [Service]
    public class ClearPaymentService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return new Binder();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Task.Run(() => ClearPayments());

            return base.OnStartCommand(intent, flags, startId);
        }

        public void ClearPayments()
        {
            var dbManager = new DatabaseManager(new DroidSqliteConnectionFactory());

            var paymentRepository = new PaymentRepository(new PaymentDataAccess(dbManager));

            var paymentManager = new PaymentManager(paymentRepository,
                new AccountRepository(new AccountDataAccess(dbManager)),
                new RecurringPaymentRepository(new RecurringPaymentDataAccess(dbManager)),
                null);

            paymentManager.ClearPayments();
        }
    }
}