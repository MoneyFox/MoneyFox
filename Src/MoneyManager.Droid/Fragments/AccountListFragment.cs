using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MoneyManager.Core.ViewModels;
using MoneyManager.Localization;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MoneyManager.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneymanager.droid.fragments.AccountListFragment")]
    public class AccountListFragment : BaseFragment<AccountListViewModel>
    {
		protected override int FragmentId => Resource.Layout.fragment_account_list;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view =  base.OnCreateView (inflater, container, savedInstanceState);

			var list = view.FindViewById<ListView>(Resource.Id.account_list);
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