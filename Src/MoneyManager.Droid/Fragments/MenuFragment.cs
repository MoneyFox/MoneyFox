using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using MoneyManager.Core.ViewModels;
using MoneyManager.Droid.Activities;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;

namespace MoneyManager.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.navigation_frame)]
    [Register("moneymanager.droid.fragments.MenuFragment")]
    public class MenuFragment : MvxFragment<MenuViewModel>, NavigationView.IOnNavigationItemSelectedListener
    {
        private NavigationView navigationView;
        private IMenuItem previousMenuItem;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_navigation, null);

            navigationView = view.FindViewById<NavigationView>(Resource.Id.navigation_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.Menu.FindItem(Resource.Id.nav_accounts).SetChecked(true);

            return view;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            item.SetCheckable(true);
            item.SetChecked(true);
            previousMenuItem?.SetChecked(false);
            previousMenuItem = item;

            Navigate (item.ItemId);

            return true;
        }

        private async Task Navigate(int itemId)
        {
            ((MainActivity)Activity).DrawerLayout.CloseDrawers ();
            await Task.Delay (TimeSpan.FromMilliseconds (250));

            switch (itemId)
            {
                case Resource.Id.nav_accounts:
                    ViewModel.ShowViewModelByType(typeof(AccountListViewModel));
                    ViewModel.ShowViewModelByType(typeof(BalanceViewModel));
                    break;
                case Resource.Id.nav_statistics:
                    ViewModel.ShowViewModelByType(typeof(StatisticSelectorViewModel));
                    break;
                case Resource.Id.nav_backup:
                    ViewModel.ShowViewModelByType(typeof(BackupViewModel));
                    break;
                case Resource.Id.nav_settings:
                    //ViewModel.ShowViewModelByType(typeof());
                    break;
                case Resource.Id.nav_about:
                    ViewModel.ShowViewModelByType(typeof(AboutViewModel));
                    break;
            }
        }
    }
}