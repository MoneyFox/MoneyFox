using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using CommonServiceLocator;
using Java.Lang;
using MediatR;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Payments.Commands.ClearPayments;
using MoneyFox.Persistence;
using NLog;
using System;
using System.Threading.Tasks;
using JobSchedulerType = Android.App.Job.JobScheduler;
using Debug = System.Diagnostics.Debug;
using Exception = System.Exception;

#pragma warning disable S927 // parameter names should match base declaration and other partial definitions: Not possible since base uses reserver word.
namespace MoneyFox.Droid.Jobs
{
    /// <summary>
    ///     Job to clear payments on a regular basis.
    /// </summary>
    [Service(Exported = true, Permission = "android.permission.BIND_JOB_SERVICE")]
    public class ClearPaymentsJob : JobService
    {
        private const int CLEAR_PAYMENT_JOB_ID = 10;
        private const int JOB_INTERVAL = 60 * 60 * 1000;

        /// <inheritdoc />
        public override bool OnStartJob(JobParameters args)
        {
            Task.Run(async () => await ClearPaymentsAsync(args));

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
            m.What = MainActivity.MessageServiceClearPayments;
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

        private async Task ClearPaymentsAsync(JobParameters args)
        {
            var settingsManager = new SettingsFacade(new SettingsAdapter());
            try
            {
                ExecutingPlatform.Current = AppPlatform.Android;

                EfCoreContext context = EfCoreContextFactory.Create();

                var mediator = ServiceLocator.Current.GetInstance<IMediator>();
                await mediator.Send(new ClearPaymentsCommand());

                JobFinished(args, false);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Fatal(ex);
                throw;
            }
            finally
            {
                settingsManager.LastExecutionTimeStampClearPayments = DateTime.Now;
            }
        }

        /// <summary>
        ///     Schedules the task for execution.
        /// </summary>
        public void ScheduleTask()
        {
            var builder = new JobInfo.Builder(CLEAR_PAYMENT_JOB_ID,
                                              new ComponentName(
                                                  this, Class.FromType(typeof(ClearPaymentsJob))));
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
