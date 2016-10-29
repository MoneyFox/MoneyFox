using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Clans.Fab;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Shared.Attributes;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.AccountListFragment")]
    public class AccountListFragment : BaseFragment<MainViewModel>
    {
        private readonly List<string> itemsForCreationList = new List<string>
        {
            Strings.AddAccountLabel,
            Strings.AddIncomeLabel,
            Strings.AddExpenseLabel,
            Strings.AddTransferLabel
        };

        private View view;
        protected override int FragmentId => Resource.Layout.fragment_account_list;
        protected override string Title => Strings.AccountsLabel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            LoadBalancePanel();

            var list = view.FindViewById<ListView>(Resource.Id.account_list);
            RegisterForContextMenu(list);

            view.FindViewById<FloatingActionMenu>(Resource.Id.fab_menu_add_element).SetClosedOnTouchOutside(true);

            return view;
        }

        public override void OnStart()
        {
            ViewModel.AccountListViewModel.LoadedCommand.Execute();
            base.OnStart();
        }

        private void LoadBalancePanel()
        {
            var fragment = new BalanceFragment
            {
                ViewModel = (BalanceViewModel)ViewModel.AccountListViewModel.BalanceViewModel
            };

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.account_list_balance_frame, fragment)
                .Commit();
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.account_list)
            {
                menu.SetHeaderTitle(Strings.SelectOperationLabel);
                menu.Add(0, 0, 0, Strings.EditLabel);
                menu.Add(0, 1, 1, Strings.DeleteLabel);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var selected = ViewModel.AccountListViewModel.AllAccounts[((AdapterView.AdapterContextMenuInfo) item.MenuInfo).Position];

            switch (item.ItemId)
            {
                case 0:
                    ViewModel.AccountListViewModel.EditAccountCommand.Execute(selected);
                    return true;

                case 1:
                    ViewModel.AccountListViewModel.DeleteAccountCommand.Execute(selected);
                    return true;

                default:
                    return false;
            }
        }
    }
}