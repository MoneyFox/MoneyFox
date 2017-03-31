using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Platform;

namespace MoneyFox.Droid.Services
{
    [Service]
    public class ClearPaymentService : Service
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

        public void ClearPayments()
        {
            Mvx.Resolve<IPaymentManager>().ClearPayments();
        }
    }
}