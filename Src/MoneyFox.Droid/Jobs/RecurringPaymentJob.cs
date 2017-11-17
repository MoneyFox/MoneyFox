using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Widget;
using EntityFramework.DbContextScope;
using MoneyFox.Business.Manager;
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
    /// <summary>
    ///     Jobs to periodically create recurring payments.
    /// </summary>
    [Service(Exported = true, Permission = "android.permission.BIND_JOB_SERVICE")]
    public class RecurringPaymentJob : JobService
    {
        private const int RECURRING_PAYMENT_JOB_ID = 20;

        /// <inheritdoc />
        public override bool OnStartJob(JobParameters args)
        {
            Task.Run(async () => await CheckRecurringPayments(args));
            return true;
        }

        /// <inheritdoc />
        public override bool OnStopJob(JobParameters args)
        {
            return true;
        }

        /// <inheritdoc />
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

            var ambientDbContextLocator = new AmbientDbContextLocator();
            var dbContextScopeFactory = new DbContextScopeFactory();

            await new RecurringPaymentManager(
                    new RecurringPaymentService(dbContextScopeFactory,
                                                new RecurringPaymentRepository(ambientDbContextLocator),
                                                new PaymentRepository(ambientDbContextLocator)),
                    new PaymentService(dbContextScopeFactory, new PaymentRepository(ambientDbContextLocator),
                                       new RecurringPaymentRepository(ambientDbContextLocator),
                                       new AccountRepository(ambientDbContextLocator),
                                       ambientDbContextLocator))
                .CreatePaymentsUpToRecur();

            Debug.WriteLine("RecurringPayment Job finished.");
            Toast.MakeText(this, Strings.RecurringPaymentsCreatedMessages, ToastLength.Long);
            JobFinished(args, false);
        }

        /// <summary>
        ///     Schedules the task for execution.
        /// </summary>
        public void ScheduleTask()
        {
            var builder = new JobInfo.Builder(RECURRING_PAYMENT_JOB_ID,
                                              new ComponentName(
                                                  this, Java.Lang.Class.FromType(typeof(RecurringPaymentJob))));

            // Execute all 60 Minutes
            builder.SetPeriodic(60 * 60 * 1000);
            builder.SetPersisted(true);
            builder.SetRequiredNetworkType(NetworkType.None);
            builder.SetRequiresDeviceIdle(false);
            builder.SetRequiresCharging(false);
            
            var tm = (JobSchedulerType)GetSystemService(Context.JobSchedulerService);
            var status = tm.Schedule(builder.Build());
        }
    }
}