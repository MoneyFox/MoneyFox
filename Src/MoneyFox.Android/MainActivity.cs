using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using MoneyFox.Droid.Jobs;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Views;
using Rg.Plugins.Popup;

#if !DEBUG
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif

namespace MoneyFox.Droid
{
    [Activity(Label = "MoneyFox", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : MvxFormsAppCompatActivity
    {        
        /// <summary>
        ///     Constant for the ClearPayment Service.
        /// </summary>
        public const int MESSAGE_SERVICE_CLEAR_PAYMENTS = 1;

        /// <summary>
        ///     Constant for the recurring payment Service.
        /// </summary>
        public const int MESSAGE_SERVICE_RECURRING_PAYMENTS = 2;

        /// <summary>
        ///     Constant for the sync backup Service.
        /// </summary>
        public const int MESSAGE_SERVICE_SYNC_BACKUP = 3;

        Handler handler;
        private ClearPaymentsJob clearPaymentsJob;
        private RecurringPaymentJob recurringPaymentJob;

        protected override void OnCreate(Bundle bundle)
        {
#if !DEBUG
            AppCenter.Start(ConfigurationManager.AppSettings["AndroidAppcenterSecret"],
                   typeof(Analytics), typeof(Crashes));
#endif
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Popup.Init(this, bundle);

            // Handler to create jobs.
            handler = new Handler(msg => {
                switch (msg.What)
                {
                    case MESSAGE_SERVICE_CLEAR_PAYMENTS:
                        clearPaymentsJob = (ClearPaymentsJob)msg.Obj;
                        clearPaymentsJob.ScheduleTask();
                        break;
                    case MESSAGE_SERVICE_RECURRING_PAYMENTS:
                        recurringPaymentJob = (RecurringPaymentJob)msg.Obj;
                        recurringPaymentJob.ScheduleTask();
                        break;
                }
            });

            // Start services and provide it a way to communicate with us.
            var startServiceIntentClearPayment = new Intent(this, typeof(ClearPaymentsJob));
            startServiceIntentClearPayment.PutExtra("messenger", new Messenger(handler));
            StartService(startServiceIntentClearPayment);

            var startServiceIntentRecurringPayment = new Intent(this, typeof(RecurringPaymentJob));
            startServiceIntentRecurringPayment.PutExtra("messenger", new Messenger(handler));
            StartService(startServiceIntentRecurringPayment);

            if (Mvx.IoCProvider.CanResolve<IBackgroundTaskManager>() && Mvx.IoCProvider.CanResolve<ISettingsFacade>())
            {
                Mvx.IoCProvider.Resolve<IBackgroundTaskManager>()
                   .StartBackupSyncTask(Mvx.IoCProvider.Resolve<ISettingsFacade>().BackupSyncRecurrence);
            }
        }

        public override void OnBackPressed()
        {
            Popup.SendBackPressed(base.OnBackPressed);
        }
    }
}

