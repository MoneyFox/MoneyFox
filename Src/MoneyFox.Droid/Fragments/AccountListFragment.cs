using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Clans.Fab;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Shared.Attributes;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.AccountListFragment")]
    public class AccountListFragment : BaseFragment<AccountListViewModel>
    {
        private const int GROUP_ID_INCLUDED_ACCOUNTS = 1;
        private const int GROUP_ID_EXCLUDED_ACCOUNTS = 2;

        private View view;
        protected override int FragmentId => Resource.Layout.fragment_account_list;
        protected override string Title => Strings.AccountsLabel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            LoadBalancePanel();

            //var includedAccountsList = view.FindViewById<ListView>(Resource.Id.included_account_list);
            //RegisterForContextMenu(includedAccountsList);
            //var excludedAccountList = view.FindViewById<ListView>(Resource.Id.excluded_account_list);
            //RegisterForContextMenu(excludedAccountList);

            view.FindViewById<FloatingActionMenu>(Resource.Id.fab_menu_add_element).SetClosedOnTouchOutside(true);

            return view;
        }

        public override void OnStart()
        {
            ViewModel.LoadedCommand.Execute();
            base.OnStart();
        }

        private void LoadBalancePanel()
        {
            var fragment = new BalanceFragment
            {
                ViewModel = (BalanceViewModel)ViewModel.BalanceViewModel
            };

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.account_list_balance_frame, fragment)
                .Commit();
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.included_account_list || v.Id == Resource.Id.excluded_account_list)
            {
                var groupId = v.Id == Resource.Id.included_account_list ? GROUP_ID_INCLUDED_ACCOUNTS : GROUP_ID_EXCLUDED_ACCOUNTS;

                menu.SetHeaderTitle(Strings.SelectOperationLabel);
                menu.Add(groupId, 0, 0, Strings.EditLabel);
                menu.Add(groupId, 1, 1, Strings.DeleteLabel);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var accounts = new List<AccountViewModel>();
            var menuInfos = (AdapterView.AdapterContextMenuInfo) item.MenuInfo;

            // Determine which listview was clicked
            switch (item.GroupId)
            {
                case GROUP_ID_INCLUDED_ACCOUNTS:
                    accounts = ViewModel.IncludedAccounts.ToList();
                    break;
                case GROUP_ID_EXCLUDED_ACCOUNTS:
                    accounts = ViewModel.ExcludedAccounts.ToList();
                    break;
            }

            var selected = accounts[menuInfos.Position];

            switch (item.ItemId)
            {
                case 0:
                    ViewModel.EditAccountCommand.Execute(selected);
                    return true;

                case 1:
                    ViewModel.DeleteAccountCommand.Execute(selected);
                    return true;

                default:
                    return false;
            }
        }
    }
}