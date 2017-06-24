using System;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Widget;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.Resources;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Droid.Services
{
    [Service(Exported = true, Permission = "android.permission.BIND_JOB_SERVICE")]
    public class ClearPaymentService : JobService
    { 
        public override bool OnStartJob(JobParameters @params)
        {
            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            Toast.MakeText(this, Strings.ClearPaymentFinishedMessage, ToastLength.Long);
            return true;
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