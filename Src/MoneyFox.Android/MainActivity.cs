using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using CommonServiceLocator;
using Microsoft.Identity.Client;
using MoneyFox.Application.Common;
using MoneyFox.Droid.Jobs;
using MoneyFox.Presentation;
using MoneyFox.Presentation.Interfaces;
using PCLAppConfig;
using PCLAppConfig.FileSystemStream;
using Rg.Plugins.Popup;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Platform = Xamarin.Essentials.Platform;

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
        private const int MESSAGE_SERVICE_CLEAR_PAYMENT_JOB_HANDLE = 1;
        private const int MESSAGE_SERVICE_RECURRING_PAYMENT_JOB_HANDLE = 2;
        private const int MESSAGE_SERVICE_SYNC_BACKUP_JOB_HANDLE = 3;

        /// <summary>
        ///     Constant for the ClearPayment Service.
        /// </summary>
        public static int MessageServiceClearPayments => MESSAGE_SERVICE_CLEAR_PAYMENT_JOB_HANDLE;

        /// <summary>
        ///     Constant for the recurring payment Service.
        /// </summary>
        public static int MessageServiceRecurringPayments => MESSAGE_SERVICE_RECURRING_PAYMENT_JOB_HANDLE;

        /// <summary>
        ///     Constant for the sync backup Service.
        /// </summary>
        public static int MessageServiceSyncBackup => MESSAGE_SERVICE_SYNC_BACKUP_JOB_HANDLE;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ParentActivityWrapper.ParentActivity = this;
            ExecutingPlatform.Current = AppPlatform.Android;

            if (ConfigurationManager.AppSettings == null) ConfigurationManager.Initialise(PortableStream.Current);

#if !DEBUG
            AppCenter.Start(ConfigurationManager.AppSettings["AndroidAppcenterSecret"],
                   typeof(Analytics), typeof(Crashes));
#endif

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Popup.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);
            XF.Material.Droid.Material.Init(this, savedInstanceState);

            LoadApplication(new App());
            Platform.Init(this, savedInstanceState);

            StartBackgroundServices();
        }

        private void StartBackgroundServices()
        {
            // Handler to create jobs.
            var handler = new Handler(msg =>
                                      {
                                          if (msg.What == MESSAGE_SERVICE_CLEAR_PAYMENT_JOB_HANDLE)
                                          {
                                              var clearPaymentsJob = (ClearPaymentsJob) msg.Obj;
                                              clearPaymentsJob.ScheduleTask();
                                          }
                                          else if (msg.What == MESSAGE_SERVICE_RECURRING_PAYMENT_JOB_HANDLE)
                                          {
                                              var recurringPaymentJob = (RecurringPaymentJob) msg.Obj;
                                              recurringPaymentJob.ScheduleTask();
                                          }
                                      });

            // Start services and provide it a way to communicate with us.
            var startServiceIntentClearPayment = new Intent(this, typeof(ClearPaymentsJob));
            startServiceIntentClearPayment.PutExtra("messenger", new Messenger(handler));
            StartService(startServiceIntentClearPayment);

            var startServiceIntentRecurringPayment = new Intent(this, typeof(RecurringPaymentJob));
            startServiceIntentRecurringPayment.PutExtra("messenger", new Messenger(handler));
            StartService(startServiceIntentRecurringPayment);

            var startServiceIntentSyncBackup = new Intent(this, typeof(SyncBackupJob));
            startServiceIntentSyncBackup.PutExtra("messenger", new Messenger(handler));
            StartService(startServiceIntentSyncBackup);
        }

        public override void OnBackPressed()
        {
            XF.Material.Droid.Material.HandleBackButton(base.OnBackPressed);
        }

        // Needed for auth, so that MSAL can intercept the response from the browser
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
