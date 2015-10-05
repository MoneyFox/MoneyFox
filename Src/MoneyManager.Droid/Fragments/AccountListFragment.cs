using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using MoneyManager.Core.ViewModels;
using MoneyManager.Localization;

namespace MoneyManager.Droid.Fragments
{
    public class AccountListFragment : MvxFragment
    {
        public new AccountListViewModel ViewModel
        {
            get { return (AccountListViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.AccountListLayout, null);

            if (savedInstanceState == null)
            {
                var fragment = new BalanceFragment
                {
                    ViewModel = Mvx.Resolve<BalanceViewModel>()
                };

                FragmentManager.BeginTransaction()
                        .Replace(Resource.Id.balance_pane, fragment)
                        .Commit();
            }

            var list = view.FindViewById<ListView>(Resource.Id.accountList);
            RegisterForContextMenu(list);

            return view;
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.accountList)
            {
                menu.SetHeaderTitle(Strings.SelectOperationLabel);
                menu.Add(Strings.EditLabel);
                menu.Add(Strings.DeleteLabel);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var selected = ViewModel.AllAccounts[((AdapterView.AdapterContextMenuInfo) item.MenuInfo).Position];

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