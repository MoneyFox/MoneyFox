using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Droid.Jobs;
using MoneyFox.Droid.Renderer;
using MoneyFox.Foundation.Interfaces;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Views;
using Rg.Plugins.Popup;

namespace MoneyFox.Droid
{
    [Activity(Label = "MoneyFox", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class 
        MainActivity : MvxFormsAppCompatActivity
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
            AppCenter.Start("6d9840ff-d832-4c1b-a2ee-bac7f15d89bd",
                   typeof(Analytics), typeof(Crashes));
#endif
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            SetupBottomTabs();

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

            Mvx.Resolve<IBackgroundTaskManager>().StartBackupSyncTask(Mvx.Resolve<ISettingsManager>().BackupSyncRecurrence);
        }

        public override void OnBackPressed()
        {
            Popup.SendBackPressed(base.OnBackPressed);
        }
    }
}

