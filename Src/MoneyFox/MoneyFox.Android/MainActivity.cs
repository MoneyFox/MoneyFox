using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using MoneyFox.Droid.Jobs;
using MvvmCross.Forms.Droid.Views;
using Naxam.Controls.Platform.Droid;

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
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            SetupBottomTabs();

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

            // Start services and provide it a way to communicate with us.
            /*var startServiceIntentClearPayment = new Intent(this, typeof(ClearPaymentsJob));
            startServiceIntentClearPayment.PutExtra("messenger", new Messenger(handler));
            StartService(startServiceIntentClearPayment);

            var startServiceIntentRecurringPayment = new Intent(this, typeof(RecurringPaymentJob));
            startServiceIntentRecurringPayment.PutExtra("messenger", new Messenger(handler));
            StartService(startServiceIntentRecurringPayment);

            Mvx.Resolve<IBackgroundTaskManager>().StartBackupSyncTask(Mvx.Resolve<ISettingsManager>().BackupSyncRecurrence);
            */
        }

        void SetupBottomTabs()
        {
            var stateList = new Android.Content.Res.ColorStateList(
                new int[][] {
                    new int[] { Android.Resource.Attribute.StateChecked
                    },
                    new int[] { Android.Resource.Attribute.StateEnabled
                    }
                },
                new int[] {
                    Color.Red, //Selected
                    Color.White //Normal
                });

            BottomTabbedRenderer.BackgroundColor = new Color(0x9C, 0x27, 0xB0);
            BottomTabbedRenderer.FontSize = 12f;
            BottomTabbedRenderer.IconSize = 16;
            BottomTabbedRenderer.ItemTextColor = stateList;
            BottomTabbedRenderer.ItemIconTintList = stateList;
            //BottomTabbedRenderer.Typeface = Typeface.CreateFromAsset(this.Assets, "architep.ttf");
            //BottomTabbedRenderer.ItemBackgroundResource = Resource.Drawable.bnv_selector;
            BottomTabbedRenderer.ItemSpacing = 4;
            //BottomTabbedRenderer.ItemPadding = new Xamarin.Forms.Thickness(6);
            BottomTabbedRenderer.BottomBarHeight = 56;
            BottomTabbedRenderer.ItemAlign = ItemAlignFlags.Center;
            BottomTabbedRenderer.ShouldUpdateSelectedIcon = true;
            //BottomTabbedRenderer.MenuItemIconSetter = (menuItem, iconSource, selected) => {
            //    var iconized = Iconize.FindIconForKey(iconSource.File);
            //    if (iconized == null)
            //    {
            //        BottomTabbedRenderer.DefaultMenuItemIconSetter.Invoke(menuItem, iconSource, selected);

            //        return;
            //    }

            //    var drawable = new IconDrawable(this, iconized).Color(selected ? Color.Red : Color.White).SizeDp(30);

            //    menuItem.SetIcon(drawable);
            //};
        }
    }
}

