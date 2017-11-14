using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using MoneyFox.Business.ViewModels;
using MoneyFox.Droid.Activities;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.navigation_frame)]
    [Register("moneyfox.droid.fragments.MenuFragment")]
    public class MenuFragment : MvxFragment<MenuViewModel>, NavigationView.IOnNavigationItemSelectedListener
    {
        private NavigationView navigationView;
        private IMenuItem previousMenuItem;

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            item.SetCheckable(true);
            item.SetChecked(true);
            previousMenuItem?.SetChecked(false);
            previousMenuItem = item;

            Navigate(item.ItemId);

            return true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_navigation, null);

            navigationView = view.FindViewById<NavigationView>(Resource.Id.navigation_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.Menu.FindItem(Resource.Id.nav_accounts).SetChecked(true);

            return view;
        }

        private async void Navigate(int itemId)
        {
            ((MainActivity) Activity).DrawerLayout.CloseDrawers();
            await Task.Delay(TimeSpan.FromMilliseconds(250));

            switch (itemId)
            {
                case Resource.Id.nav_accounts:
                    await ViewModel.ShowAccountListCommand.ExecuteAsync();
                    break;
                case Resource.Id.nav_statistics:
                    await ViewModel.ShowStatisticSelectorCommand.ExecuteAsync();
                    break;
                case Resource.Id.nav_categories:
                    await ViewModel.ShowCategoryListCommand.ExecuteAsync();
                    break;
                case Resource.Id.nav_backup:
                    await ViewModel.ShowBackupViewCommand.ExecuteAsync();
                    break;
                case Resource.Id.nav_settings:
                    await ViewModel.ShowSettingsCommand.ExecuteAsync();
                    break;
                case Resource.Id.nav_about:
                    await ViewModel.ShowAboutCommand.ExecuteAsync();
                    break;
            }
        }
    }
}