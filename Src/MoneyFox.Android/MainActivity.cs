using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using CommonServiceLocator;
using Microsoft.Identity.Client;
using MoneyFox.Droid.Jobs;
using MoneyFox.Foundation;
using MoneyFox.Presentation;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Interfaces;
using PCLAppConfig;
using Rg.Plugins.Popup;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

#if !DEBUG
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif

namespace MoneyFox.Droid
{
    [Activity(Label = "MoneyFox", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (ConfigurationManager.AppSettings == null)
            {
                ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            }

#if !DEBUG
            AppCenter.Start(ConfigurationManager.AppSettings["AndroidAppcenterSecret"],
                   typeof(Analytics), typeof(Crashes));
#endif

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Forms.Init(this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);

            LoadApplication(new App());
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Popup.Init(this, savedInstanceState);

            ParentActivityWrapper.ParentActivity = this;

            StartBackgroundServices();
        }

        private void StartBackgroundServices()
        {
            // Handler to create jobs.
            var handler = new Handler(msg => 
            {
                switch (msg.What)
                {
                    case MESSAGE_SERVICE_CLEAR_PAYMENTS:
                        var clearPaymentsJob = (ClearPaymentsJob) msg.Obj;
                        clearPaymentsJob.ScheduleTask();
                        break;
                    case MESSAGE_SERVICE_RECURRING_PAYMENTS:
                        var recurringPaymentJob = (RecurringPaymentJob) msg.Obj;
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

            var backgroundTaskManager = ServiceLocator.Current.GetInstance<IBackgroundTaskManager>();
            var settingsFacade = ServiceLocator.Current.GetInstance<ISettingsFacade>();

            if (backgroundTaskManager != null && settingsFacade != null)
            {
                backgroundTaskManager.StartBackupSyncTask(settingsFacade.BackupSyncRecurrence);
            }
        }

        public override void OnBackPressed()
        {
            Popup.SendBackPressed(base.OnBackPressed);
        }

        // Needed for auth, so that MSAL can intercept the response from the browser
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

