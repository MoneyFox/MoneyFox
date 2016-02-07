using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.Fragging.Caching;

namespace MoneyManager.Droid.Activities
{
    [Activity(Label = "MoneyManager",
        Icon = "@drawable/icon",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop,
        Name = "moneyfox.droid.activities.MainActivity")]
    public class MainActivity : MvxFragmentCompatActivity<MainViewModel>
    {
        public DrawerLayout DrawerLayout;

        protected override void OnCreate(Bundle bundle)
        {
            if (bundle == null)
            {
                //ViewModel.GoToAboutCommand.Execute();
            }
            else
            {
                base.OnCreate(bundle);
            }

            SetContentView(Resource.Layout.activity_main);

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ShowHamburguerMenu();
        }

        private void CheckIfMenuIsNeeded(CustomFragmentInfo myCustomInfo)
        {
            //If not root, we will block the menu sliding gesture and show the back button on top
            if (myCustomInfo.IsRoot)
                ShowHamburguerMenu();
            else
                ShowBackButton();
        }

        private void ShowBackButton()
        {
            //TODO Tell the toggle to set the indicator off
            //this.DrawerToggle.DrawerIndicatorEnabled = false;

            //Block the menu slide gesture
            DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
        }

        private void ShowHamburguerMenu()
        {
            //TODO set toggle indicator as enabled 
            //this.DrawerToggle.DrawerIndicatorEnabled = true;

            //Unlock the menu sliding gesture
            DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home && !DrawerLayout.IsDrawerOpen(GravityCompat.Start))
            {
                DrawerLayout.OpenDrawer(GravityCompat.Start);
                return true;
            }

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

                default:
                    return base.OnOptionsItemSelected(item);
            }
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