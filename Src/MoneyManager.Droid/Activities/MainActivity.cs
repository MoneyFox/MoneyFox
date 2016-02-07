using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using MoneyManager.Core.ViewModels;
using MoneyManager.Droid.Fragments;
using MoneyManager.Localization;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.Fragging.Caching;
using MvvmCross.Platform;

namespace MoneyManager.Droid.Activities
{
    [Activity(Label = "MoneyManager", 
        MainLauncher = true,
        Icon = "@drawable/icon", 
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : MvxCachingFragmentCompatActivity<MainViewModel>
    {
        private AboutFragment aboutFragment;

        private AccountListFragment accountListFragment;
        private BackupFragment backupFragment;

        private string drawerTitle;
        private DrawerLayout drawerLayout;
        private StatisticSelectorFragment statisticSelectorFragment;
        private string title;

        public new MainViewModel ViewModel
        {
            get { return (MainViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.main_layout);
            accountListFragment = new AccountListFragment
            {
                ViewModel = Mvx.Resolve<AccountListViewModel>()
            };

            statisticSelectorFragment = new StatisticSelectorFragment();
            backupFragment = new BackupFragment
            {
                ViewModel = Mvx.Resolve<BackupViewModel>()
            };

            aboutFragment = new AboutFragment
            {
                ViewModel = Mvx.Resolve<AboutViewModel>()
            };

            drawerLayout.DrawerOpened += (sender, e) =>
            {
                ActionBar.SetHomeButtonEnabled(false);
                ActionBar.SetDisplayHomeAsUpEnabled(false);
                ActionBar.Title = drawerTitle;
            };

            drawerLayout.DrawerClosed += (sender, e) =>
            {
                ActionBar.SetDisplayHomeAsUpEnabled(true);
                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.Title = title;
            };

            //ActionBar.SetDisplayHomeAsUpEnabled(true);
            //ActionBar.SetHomeButtonEnabled(true);

            drawerTitle = Strings.MenuTitle;

            //drawerLayout.ViewTreeObserver.GlobalLayout += FirstLayoutListener;

            var fragmentTransaction = FragmentManager.BeginTransaction();
            fragmentTransaction.Add(Resource.Id.content_frame, accountListFragment);
            fragmentTransaction.Commit();
        }

        public override void OnFragmentChanged(IMvxCachedFragmentInfo fragmentInfo)
        {
            var myCustomInfo = fragmentInfo as CustomFragmentInfo;
            CheckIfMenuIsNeeded(myCustomInfo);
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
            drawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
        }

        private void ShowHamburguerMenu()
        {
            //TODO set toggle indicator as enabled 
            //this.DrawerToggle.DrawerIndicatorEnabled = true;

            //Unlock the menu sliding gesture
            drawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
        }

        private void NavigationClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            switch (e.Position)
            {
                case 0:
                    FragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_frame, accountListFragment)
                        .AddToBackStack("AccountList")
                        .Commit();
                    break;

                case 1:
                    FragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_frame, statisticSelectorFragment)
                        .AddToBackStack("Statistic Selector")
                        .Commit();
                    break;

                case 2:
                    FragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_frame, backupFragment)
                        .AddToBackStack("Backup")
                        .Commit();
                    break;

                case 3:
                    break;

                case 4:
                    FragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_frame, aboutFragment)
                        .AddToBackStack("About")
                        .Commit();
                    break;
            }
            drawerLayout.CloseDrawer(GravityCompat.End);
        }

        //private void FirstLayoutListener(object sender, EventArgs e)
        //{
        //    if (drawerLayout != null && !drawerLayout.IsDrawerOpen(GravityCompat.Start))
        //    {
        //        ActionBar.SetDisplayHomeAsUpEnabled(true);
        //        ActionBar.SetHomeButtonEnabled(true);
        //        ActionBar.Title = title;
        //    }
        //    else
        //    {
        //        ActionBar.SetDisplayHomeAsUpEnabled(false);
        //        ActionBar.SetHomeButtonEnabled(false);
        //        ActionBar.Title = drawerTitle;
        //    }

        //    drawerLayout.ViewTreeObserver.GlobalLayout -= FirstLayoutListener;
        //}

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home && !drawerLayout.IsDrawerOpen(GravityCompat.Start))
            {
                drawerLayout.OpenDrawer(GravityCompat.Start);
                return true;
            }

            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(GravityCompat.Start);
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
            public CustomFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool cacheFragment = true, bool addToBackstack = false,
                bool isRoot = false)
                : base(tag, fragmentType, viewModelType, cacheFragment, addToBackstack)
            {
                IsRoot = isRoot;
            }

            public bool IsRoot { get; set; }
        }
    }
}