using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Widget;
using EntityFramework.DbContextScope;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Droid.Activities;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Resources;
using Debug = System.Diagnostics.Debug;
using Environment = System.Environment;
using JobSchedulerType = Android.App.Job.JobScheduler;

namespace MoneyFox.Droid.Jobs
{
    /// <summary>
    ///     Job to clear payments on a regular basis.
    /// </summary>
    [Service(Exported = true, Permission = "android.permission.BIND_JOB_SERVICE")]
    public class ClearPaymentsJob : JobService
    {
        private const int CLEARPAYMENT_JOB_ID = 10;

        /// <inheritdoc />
        public override bool OnStartJob(JobParameters args)
        {
            Task.Run(async () => await ClearPayments(args));
            return true;
        }

        /// <inheritdoc />
        public override bool OnStopJob(JobParameters args)
        {
            return true;
        }

        /// <inheritdoc />
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var callback = (Messenger)intent.GetParcelableExtra("messenger");
            var m = Message.Obtain();
            m.What = MainActivity.MESSAGE_SERVICE_CLEAR_PAYMENTS;
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

        private async Task ClearPayments(JobParameters args)
        {
            Debug.WriteLine("ClearPayments Job started");
            DataAccess.ApplicationContext.DbPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseConstants.DB_NAME);

            var paymentService = new PaymentService(new DbContextScopeFactory(),
                                                    new AmbientDbContextLocator());

            var payments = await paymentService.GetUnclearedPayments(DateTime.Now);
            var unclearedPayments = payments.ToList();

            if (unclearedPayments.Any())
            {
                Debug.WriteLine("Payments for clearing found.");
                await paymentService.SavePayments(unclearedPayments.ToArray());
            }

            Toast.MakeText(this, Strings.ClearPaymentFinishedMessage, ToastLength.Long);
            Debug.WriteLine("ClearPayments Job finished.");
            JobFinished(args, false);
        }

        /// <summary>
        ///     Schedules the task for execution.
        /// </summary>
        public void ScheduleTask()
        {
            var builder = new JobInfo.Builder(CLEARPAYMENT_JOB_ID,
                                              new ComponentName(
                                                  this, Java.Lang.Class.FromType(typeof(ClearPaymentsJob))));
            // Execute all 60 Minutes
            builder.SetPeriodic(60 * 60 * 1000);
            builder.SetPersisted(true);
            builder.SetRequiredNetworkType(NetworkType.None);
            builder.SetRequiresDeviceIdle(false);
            builder.SetRequiresCharging(false);

            var tm = (JobSchedulerType)GetSystemService(JobSchedulerService);
            var status = tm.Schedule(builder.Build());
        }
    }
}