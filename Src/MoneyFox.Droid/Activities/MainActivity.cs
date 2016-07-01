using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using MoneyFox.Droid.Activities.Caching;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Droid.Shared.Caching;
using MvvmCross.Droid.Support.V7.AppCompat;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace MoneyFox.Droid.Activities {
    [Activity(Label = "Money Fox",
        Icon = "@drawable/ic_launcher",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop,
        Name = "moneyfox.droid.activities.MainActivity")]
    public class MainActivity : MvxCachingFragmentCompatActivity<MainViewModel> {
        private CustomFragmentInfo currentFragmentInfo;
        public DrawerLayout DrawerLayout;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_main);

#if !DEBUG
            CrashManager.Register(this, ServiceConstants.HOCKEY_APP_DROID_ID);
            MetricsManager.Register(this, Application, ServiceConstants.HOCKEY_APP_DROID_ID);
#endif

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            if (bundle == null) {
                ViewModel.ShowMenuAndFirstDetail();
            }

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            if (toolbar != null) {
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
        }

        // custom FragmentCacheConfiguration is used because custom IMvxFragmentInfo is used -> CustomFragmentInfo
        public override IFragmentCacheConfiguration BuildFragmentCacheConfiguration()
            => new FragmentCacheConfigurationCustomFragmentInfo();

        public override void OnBeforeFragmentChanging(IMvxCachedFragmentInfo fragmentInfo,
            FragmentTransaction transaction) {
            transaction.SetCustomAnimations(Resource.Animation.abc_fade_in,
                Resource.Animation.abc_fade_out);
            base.OnBeforeFragmentChanging(fragmentInfo, transaction);
        }

        public override void OnFragmentChanged(IMvxCachedFragmentInfo fragmentInfo) {
            currentFragmentInfo = fragmentInfo as CustomFragmentInfo;
        }

        public override void OnBackPressed() {
            if (DrawerLayout != null && DrawerLayout.IsDrawerOpen(GravityCompat.Start)) {
                DrawerLayout.CloseDrawers();
            }
            else {
                base.OnBackPressed();
            }
        }

        /// <summary>
        ///     Handle Clicks in the Toolbar
        /// </summary>
        /// <param name="item">Represents the clicked menu item.</param>
        /// <returns>Returns true if the operation was succesful and false if not.</returns>
        public override bool OnOptionsItemSelected(IMenuItem item) {
            switch (item.ItemId) {
                case Android.Resource.Id.Home:
                    return HandleHomeButton();
            }
            return base.OnOptionsItemSelected(item);
        }

        private bool HandleHomeButton() {
            if (currentFragmentInfo != null && currentFragmentInfo.IsRoot) {
                DrawerLayout.OpenDrawer(GravityCompat.Start);
            }
            else {
                SupportFragmentManager.PopBackStackImmediate();
            }
            return true;
        }

        public class CustomFragmentInfo : MvxCachedFragmentInfo {
            public CustomFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool cacheFragment = true,
                bool isRoot = false)
                : base(tag, fragmentType, viewModelType, cacheFragment, true) {
                IsRoot = isRoot;
            }

            public bool IsRoot { get; set; }
        }
    }
}