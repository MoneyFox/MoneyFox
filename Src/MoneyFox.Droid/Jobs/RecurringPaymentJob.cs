using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Widget;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Droid.Activities;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Resources;
using MoneyFox.Service.DataServices;
using Debug = System.Diagnostics.Debug;
using Environment = System.Environment;
using JobSchedulerType = Android.App.Job.JobScheduler;

namespace MoneyFox.Droid.Jobs
{
    [Service(Exported = true, Permission = "android.permission.BIND_JOB_SERVICE")]
    public class RecurringPaymentJob : JobService
    {
        private const int RECURRING_PAYMENT_JOB_ID = 20;

        public override bool OnStartJob(JobParameters args)
        {
            Task.Run(async () => await CheckRecurringPayments(args));
            return true;
        }

        public override bool OnStopJob(JobParameters args)
        {
            return true;
        }

        public override StartCommandResult OnStartCommand(Intent intent, Android.App.StartCommandFlags flags, int startId)
        {
            var callback = (Messenger)intent.GetParcelableExtra("messenger");
            var m = Message.Obtain();
            m.What = MainActivity.MESSAGE_SERVICE_RECURRING_PAYMENTS;
            m.Obj = this;
            try
            {
                callback.Send(m);
            } catch (RemoteException e)
            {
                Debug.WriteLine(e);
            }
            return StartCommandResult.NotSticky;
        }

        private async Task CheckRecurringPayments(JobParameters args)
        {
            Debug.WriteLine("RecurringPayment Job started.");
            DataAccess.ApplicationContext.DbPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseConstants.DB_NAME);

            var dbFactory = new DbFactory();
            var unitOfWork = new UnitOfWork(dbFactory);
            await new RecurringPaymentManager(
                new RecurringPaymentService(new RecurringPaymentRepository(dbFactory), new PaymentRepository(dbFactory)),
                new PaymentService(new PaymentRepository(dbFactory),  unitOfWork))
                .CreatePaymentsUpToRecur();

            Debug.WriteLine("RecurringPayment Job finished.");
            Toast.MakeText(this, Strings.RecurringPaymentsCreatedMessages, ToastLength.Long);
            JobFinished(args, false);
        }

        public void ScheduleTask()
        {
            var builder = new JobInfo.Builder(RECURRING_PAYMENT_JOB_ID,
                                              new ComponentName(
                                                  this, Java.Lang.Class.FromType(typeof(RecurringPaymentJob))));

            // Execute all 30 Minutes
            builder.SetPeriodic(30 * 60 * 1000);
            builder.SetPersisted(true);
            builder.SetRequiredNetworkType(NetworkType.None);
            builder.SetRequiresDeviceIdle(false);
            builder.SetRequiresCharging(false);
            
            var tm = (JobSchedulerType)GetSystemService(Context.JobSchedulerService);
            var status = tm.Schedule(builder.Build());
        }
    }
}