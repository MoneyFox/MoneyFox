using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Microsoft.AppCenter.Crashes;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.DataLayer;
using MoneyFox.Foundation.Constants;
using MoneyFox.ServiceLayer.Facades;
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
            }
            catch (RemoteException e)
            {
                Debug.WriteLine(e);
            }
            return StartCommandResult.NotSticky;
        }

        private async Task CheckRecurringPayments(JobParameters args)
        {
            var settingsManager = new SettingsFacade(new SettingsAdapter());

            try
            {
                Debug.WriteLine("RecurringPayment Job started.");
                EfCoreContext.DbPath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                                 DatabaseConstants.DB_NAME);

                //await new RecurringPaymentManager(
                //        new RecurringPaymentService(ambientDbContextLocator, dbContextScopeFactory),
                //        new PaymentService(ambientDbContextLocator, dbContextScopeFactory))
                //    .CreatePaymentsUpToRecur();

                Debug.WriteLine("RecurringPayment Job finished.");
                JobFinished(args, false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            } 
            finally
            {
                settingsManager.LastExecutionTimeStampRecurringPayments = DateTime.Now;
            }
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