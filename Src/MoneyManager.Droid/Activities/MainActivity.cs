using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Support.Fragging;
using MoneyManager.Core.ViewModels;
using MoneyManager.Droid.Fragments;
using MoneyManager.Localization;

namespace MoneyManager.Droid.Activities
{
    [Activity(Label = "MoneyManager", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : MvxFragmentActivity
    {
        private readonly List<string> menuItems = new List<string>
        {
            Strings.AccountsLabel,
            Strings.StatisticTitle,
            Strings.BackupLabel,
            Strings.SettingsLabel,
            Strings.AboutLabel
        };

        private AboutFragment aboutFragment;

        private AccountListFragment accountListFragment;

        private string drawerTitle;
        private ListView menuListView;
        private SlidingPaneLayout slidingLayout;
        private StatisticFragment statisticFragment;
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
            SetContentView(Resource.Layout.MainLayout);

            slidingLayout = FindViewById<SlidingPaneLayout>(Resource.Id.main_layout);
            menuListView = FindViewById<ListView>(Resource.Id.left_pane);
            accountListFragment = new AccountListFragment
            {
                ViewModel = Mvx.Resolve<AccountListViewModel>()
            };

            statisticFragment = new StatisticFragment
            {
                ViewModel = Mvx.Resolve<StatisticViewModel>()
            };

            aboutFragment = new AboutFragment
            {
                ViewModel = Mvx.Resolve<AboutViewModel>()
            };

            slidingLayout.PanelOpened += (sender, e) =>
            {
                ActionBar.SetHomeButtonEnabled(false);
                ActionBar.SetDisplayHomeAsUpEnabled(false);
                ActionBar.Title = drawerTitle;
            };

            slidingLayout.PanelClosed += (sender, e) =>
            {
                ActionBar.SetDisplayHomeAsUpEnabled(true);
                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.Title = title;
            };

            menuListView.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, menuItems);
            menuListView.ItemClick += NavigationClick;

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            title = menuItems[0];
            drawerTitle = Strings.MenuTitle;

            slidingLayout.ViewTreeObserver.GlobalLayout += FirstLayoutListener;

            var fragmenTransaction = SupportFragmentManager.BeginTransaction();
            fragmenTransaction.Add(Resource.Id.content_pane, accountListFragment);
            fragmenTransaction.Commit();
        }

        private void NavigationClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            title = menuItems[e.Position];

            switch (e.Position)
            {
                case 0:
                    SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_pane, accountListFragment)
                        .AddToBackStack("AccountList")
                        .Commit();
                    break;

                case 1:
                    SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_pane, statisticFragment)
                        .AddToBackStack("Statistic")
                        .Commit();
                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_pane, aboutFragment)
                        .AddToBackStack("About")
                        .Commit();
                    break;
            }
            slidingLayout.ClosePane();
        }

        private void FirstLayoutListener(object sender, EventArgs e)
        {
            if (slidingLayout.IsSlideable && !slidingLayout.IsOpen)
            {
                ActionBar.SetDisplayHomeAsUpEnabled(true);
                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.Title = title;
            }
            else
            {
                ActionBar.SetDisplayHomeAsUpEnabled(false);
                ActionBar.SetHomeButtonEnabled(false);
                ActionBar.Title = drawerTitle;
            }

            slidingLayout.ViewTreeObserver.GlobalLayout -= FirstLayoutListener;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home && !slidingLayout.IsOpen)
            {
                slidingLayout.OpenPane();
                return true;
            }

            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    slidingLayout.OpenPane();
                    return true;

                case Resource.Id.action_add_income:
                    ViewModel.GoToAddTransactionCommand.Execute("Income");
                    return true;

                case Resource.Id.action_add_spending:
                    ViewModel.GoToAddTransactionCommand.Execute("Spending");
                    return true;

                case Resource.Id.action_add_transfer:
                    ViewModel.GoToAddTransactionCommand.Execute("Transfer");
                    return true;

                case Resource.Id.action_add_account:
                    ViewModel.GoToAddAccountCommand.Execute();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}