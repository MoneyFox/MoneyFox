using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Microsoft.Identity.Client;
using MoneyFox.Application.Common;
using MoneyFox.Droid.Jobs;
using MoneyFox.Presentation;
using Rg.Plugins.Popup;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XF.Material.Droid;
using Platform = Xamarin.Essentials.Platform;

#if !DEBUG
using PCLAppConfig;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif

namespace MoneyFox.Droid
{
    [Activity(Label = "MoneyFox", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        private const int MESSAGE_SERVICE_SYNC_BACKUP_JOB_HANDLE = 3;

        /// <summary>
        ///     Constant for the sync backup Service.
        /// </summary>
        public static int MessageServiceSyncBackup => MESSAGE_SERVICE_SYNC_BACKUP_JOB_HANDLE;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ParentActivityWrapper.ParentActivity = this;
            ExecutingPlatform.Current = AppPlatform.Android;

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
            Material.Init(this, savedInstanceState);

            LoadApplication(new App());
            Platform.Init(this, savedInstanceState);

            StartBackgroundServices();
        }

        private void StartBackgroundServices()
        {
            // Handler to create jobs.
            var handler = new Handler(msg =>
                                      {
                                          if (msg.What == MESSAGE_SERVICE_SYNC_BACKUP_JOB_HANDLE)
                                          {
                                              var clearPaymentsJob = (SyncBackupJob) msg.Obj;
                                              clearPaymentsJob.ScheduleTask();
                                          }
                                      });

            // Start services and provide it a way to communicate with us.
            var startServiceIntentSyncBackup = new Intent(this, typeof(SyncBackupJob));
            startServiceIntentSyncBackup.PutExtra("messenger", new Messenger(handler));
            StartService(startServiceIntentSyncBackup);
        }

        public override void OnBackPressed()
        {
            Material.HandleBackButton(base.OnBackPressed);
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
