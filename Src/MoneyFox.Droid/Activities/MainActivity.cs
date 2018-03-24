using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
#if !DEBUG
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif
using MoneyFox.Business.ViewModels;
using MoneyFox.Droid.Jobs;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "Money Fox",
        Icon = "@drawable/ic_launcher",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop,
        Name = "moneyfox.droid.activities.MainActivity")]
    public class MainActivity : MvxAppCompatActivity<MainViewModel>
    {
        public DrawerLayout DrawerLayout;
        public CoordinatorLayout MainFrame;

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

        /// <inheritdoc />
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

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

            SetContentView(Resource.Layout.activity_main);

            Xamarin.Forms.Forms.Init(this, null);

#if !DEBUG
            AppCenter.Start("6d9840ff-d832-4c1b-a2ee-bac7f15d89bd",
                               typeof(Analytics), typeof(Crashes));
#endif

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            MainFrame = FindViewById<CoordinatorLayout>(Resource.Id.main_frame);

            if (bundle == null)
            {
                await ViewModel.ShowMenuAndFirstDetail();
            }

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);

                var drawerToggle = new MvxActionBarDrawerToggle(
                    this, // host Activity
                    DrawerLayout, // DrawerLayout object
                    toolbar, // nav drawer icon to replace 'Up' caret
                    Resource.String.drawer_open, // "open drawer" description
                    Resource.String.drawer_close // "close drawer" description
                    );

                DrawerLayout.AddDrawerListener(drawerToggle);
                drawerToggle.SyncState();
            }

            // Start services and provide it a way to communicate with us.
            var startServiceIntentClearPayment = new Intent(this, typeof(ClearPaymentsJob));
            startServiceIntentClearPayment.PutExtra("messenger", new Messenger(handler));
            StartService(startServiceIntentClearPayment);

            var startServiceIntentRecurringPayment = new Intent(this, typeof(RecurringPaymentJob));
            startServiceIntentRecurringPayment.PutExtra("messenger", new Messenger(handler));
            StartService(startServiceIntentRecurringPayment);

            Mvx.Resolve<IBackgroundTaskManager>().StartBackupSyncTask(Mvx.Resolve<ISettingsManager>().BackupSyncRecurrence);
        }

        /// <inheritdoc />
        public override void OnBackPressed()
        {
            if (DrawerLayout != null && DrawerLayout.IsDrawerOpen(GravityCompat.Start))
            {
                DrawerLayout.CloseDrawers();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        /// <inheritdoc />
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    DrawerLayout.OpenDrawer(GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}