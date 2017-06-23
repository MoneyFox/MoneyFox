using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Droid.Services
{
    [Service]
    public class RecurringPaymentService : Android.App.Service
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
            var unitOfWork = new UnitOfWork(new DbFactory());
            new RecurringPaymentManager(
                new Service.DataServices.RecurringPaymentService(unitOfWork),
                new PaymentService(unitOfWork))
                .CreatePaymentsUpToRecur();
        }
    }
}