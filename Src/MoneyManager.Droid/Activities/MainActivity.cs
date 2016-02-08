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

namespace MoneyManager.Droid.Activities
{
    [Activity(Label = "MoneyManager",
        Icon = "@drawable/icon",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop,
        Name = "moneymanager.droid.activities.MainActivity")]
    public class MainActivity : MvxCachingFragmentCompatActivity<MainViewModel>
    {
        public DrawerLayout DrawerLayout;

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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override IFragmentCacheConfiguration BuildFragmentCacheConfiguration()
        {
            // custom FragmentCacheConfiguration is used because custom IMvxFragmentInfo is used -> CustomFragmentInfo
            return new FragmentCacheConfigurationCustomFragmentInfo();
        }

        public override void OnFragmentChanged(IMvxCachedFragmentInfo fragmentInfo)
        {
            var myCustomInfo = fragmentInfo as CustomFragmentInfo;
            CheckIfMenuIsNeeded(myCustomInfo);
        }

        private void CheckIfMenuIsNeeded(CustomFragmentInfo myCustomInfo)
        {
            //If not root, we will block the menu sliding gesture and show the back button on top
			if (myCustomInfo != null && myCustomInfo.IsRoot)
            {
                ShowHamburguerMenu();
            }
            else
            {
                ShowBackButton();
            }
        }

        private void ShowBackButton()
        {
            //Block the menu slide gesture
            DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
        }

        private void ShowHamburguerMenu()
        {
            //Unlock the menu sliding gesture
            DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    DrawerLayout.OpenDrawer(GravityCompat.Start);
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
                bool addToBackstack = false,
                bool isRoot = false)
                : base(tag, fragmentType, viewModelType, cacheFragment, addToBackstack)
            {
                IsRoot = isRoot;
            }

            public bool IsRoot { get; set; }
        }
    }
}