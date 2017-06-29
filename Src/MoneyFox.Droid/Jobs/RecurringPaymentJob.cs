using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Widget;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Resources;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Droid.Jobs
{
    [Service(Exported = true, Permission = "android.permission.BIND_JOB_SERVICE")]
    public class RecurringPaymentJob : JobService
    {
        public override bool OnStartJob(JobParameters @params)
        {
            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            Toast.MakeText(this, Strings.RecurringPaymentsCreatedMessages, ToastLength.Long);
            return true;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Task.Run(() => CheckRecurringPayments());
            return StartCommandResult.RedeliverIntent;
        }

        private void CheckRecurringPayments()
        {
            DataAccess.ApplicationContext.DbPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseConstants.DB_NAME);

            var dbFactory = new DbFactory();
            var unitOfWork = new UnitOfWork(dbFactory);
            new RecurringPaymentManager(
                new Service.DataServices.RecurringPaymentService(new RecurringPaymentRepository(dbFactory)),
                new PaymentService(new PaymentRepository(dbFactory),  unitOfWork))
                .CreatePaymentsUpToRecur();
        }
    }
}