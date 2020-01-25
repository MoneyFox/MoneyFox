using System;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using CommonServiceLocator;
using Java.Lang;
using MediatR;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Payments.Commands.CreateRecurringPayments;
using NLog;
using Exception = System.Exception;
using JobSchedulerType = Android.App.Job.JobScheduler;

#pragma warning disable S927 // parameter names should match base declaration and other partial definitions: Not possible since base uses reserver word.
namespace MoneyFox.Droid.Jobs
{
    /// <summary>
    ///     Jobs to periodically create recurring payments.
    /// </summary>
    [Service(Exported = true, Permission = "android.permission.BIND_JOB_SERVICE")]
    public class RecurringPaymentJob : JobService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const int RECURRING_PAYMENT_JOB_ID = 20;
        private const int JOB_INTERVAL = 60 * 60 * 1000;

        /// <inheritdoc />
        public override bool OnStartJob(JobParameters args)
        {
            Task.Run(async () => await CheckRecurringPaymentsAsync(args));

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
            var callback = (Messenger) intent.GetParcelableExtra("messenger");
            Message m = Message.Obtain();
            m.What = MainActivity.MessageServiceRecurringPayments;
            m.Obj = this;
            try
            {
                callback.Send(m);
            }
            catch (RemoteException e)
            {
                logger.Error(e, "OnStart Create Recurring Pamyents Job.");
            }

            return StartCommandResult.NotSticky;
        }

        private async Task CheckRecurringPaymentsAsync(JobParameters args)
        {
            var settingsManager = new SettingsFacade(new SettingsAdapter());

            try
            {
                var mediator = ServiceLocator.Current.GetInstance<IMediator>();
                await mediator.Send(new CreateRecurringPaymentsCommand());

                logger.Info("Recurring Payments Created.");
                JobFinished(args, false);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);

                throw;
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
                                                                this, Class.FromType(typeof(RecurringPaymentJob))));

            // Execute all 60 Minutes
            builder.SetPeriodic(JOB_INTERVAL);
            builder.SetPersisted(true);
            builder.SetRequiredNetworkType(NetworkType.None);
            builder.SetRequiresDeviceIdle(false);
            builder.SetRequiresCharging(false);

            var tm = (JobSchedulerType) GetSystemService(JobSchedulerService);
            tm.Schedule(builder.Build());
        }
    }
}
