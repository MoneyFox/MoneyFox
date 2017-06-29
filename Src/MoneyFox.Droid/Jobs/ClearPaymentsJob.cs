using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Widget;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Resources;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Droid.Jobs
{
    [Service(Exported = true, Permission = "android.permission.BIND_JOB_SERVICE")]
    public class ClearPaymentsJob : JobService
    { 
        public override bool OnStartJob(JobParameters args)
        {
            Task.Run(async() => await ClearPayments(args));
            return true;
        }

        public override bool OnStopJob(JobParameters args)
        {
            return true;
        }

        public async Task ClearPayments(JobParameters args)
        {
            DataAccess.ApplicationContext.DbPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseConstants.DB_NAME);
            var dbFactory = new DbFactory();
            var paymentService = new PaymentService(new PaymentRepository(dbFactory), new UnitOfWork(dbFactory));
            await paymentService.GetUnclearedPayments(DateTime.Now);

            Toast.MakeText(this, Strings.ClearPaymentFinishedMessage, ToastLength.Long);
            JobFinished(args, false);
        }
    }
}