using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using MoneyManager.Core.ViewModels;
using MoneyManager.Droid.Activities.Caching;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.Fragging.Caching;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "MoneyManager",
        Icon = "@drawable/icon",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop,
        Name = "moneyfox.droid.activities.MainActivity")]
    public class MainActivity : MvxCachingFragmentCompatActivity<MainViewModel>
    {
        public DrawerLayout DrawerLayout;
        private CustomFragmentInfo currentFragmentInfo;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_main);

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            if (bundle == null)
            {
                ViewModel.ShowMenuAndFirstDetail();
            }
        }

        public override IFragmentCacheConfiguration BuildFragmentCacheConfiguration()
        {
            // custom FragmentCacheConfiguration is used because custom IMvxFragmentInfo is used -> CustomFragmentInfo
            return new FragmentCacheConfigurationCustomFragmentInfo();
        }

        public override void OnBeforeFragmentChanging(IMvxCachedFragmentInfo fragmentInfo, FragmentTransaction transaction)
        {
            transaction.SetCustomAnimations(Resource.Animation.abc_grow_fade_in_from_bottom, Resource.Animation.abc_fade_out);
            base.OnBeforeFragmentChanging(fragmentInfo, transaction);
        }

        public override void OnFragmentChanged(IMvxCachedFragmentInfo fragmentInfo)
        {
            currentFragmentInfo = fragmentInfo as CustomFragmentInfo;
        }

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

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    if (currentFragmentInfo != null && currentFragmentInfo.IsRoot)
                    {
                        DrawerLayout.OpenDrawer(GravityCompat.Start);
                    }
                    else
                    {
                        SupportFragmentManager.PopBackStackImmediate();
                    }    
                    return true;

                case Resource.Id.action_add_income:
                    ViewModel.GoToAddPaymentCommand.Execute("Income");
                    return true;

                case Resource.Id.action_add_spending:
                    ViewModel.GoToAddPaymentCommand.Execute("Expense");
                    return true;

                case Resource.Id.action_add_transfer:
                    ViewModel.GoToAddPaymentCommand.Execute("Transfer");
                    return true;

                case Resource.Id.action_add_account:
                    ViewModel.GoToAddAccountCommand.Execute();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public class CustomFragmentInfo : MvxCachedFragmentInfo
        {
            public CustomFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool cacheFragment = true,
                bool isRoot = false)
                : base(tag, fragmentType, viewModelType, cacheFragment, true)
            {
                IsRoot = isRoot;
            }

            public bool IsRoot { get; set; }
        }
    }
}