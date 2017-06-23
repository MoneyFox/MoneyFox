using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Droid.Services
{
    [Service]
    public class ClearPaymentService : Android.App.Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Task.Run(() => ClearPayments());
            return StartCommandResult.RedeliverIntent;
        }

        public async void ClearPayments()
        {
            var dbFactory = new DbFactory();
            var paymentService = new PaymentService(new PaymentRepository(dbFactory), new UnitOfWork(dbFactory));
            await paymentService.GetUnclearedPayments(DateTime.Now);
        }
    }
}