using Android.OS;
using Android.Runtime;
using Android.Views;
using Clans.Fab;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Views.Attributes;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.AccountListFragment")]
    public class AccountListFragment : BaseFragment<AccountListViewModel>
    {
        private View view;
        protected override int FragmentId => Resource.Layout.fragment_account_list;
        protected override string Title => Strings.AccountsLabel;

        /// <inheritdoc />
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            LoadBalancePanel();

            view.FindViewById<FloatingActionMenu>(Resource.Id.fab_menu_add_element).SetClosedOnTouchOutside(true);

            return view;
        }

        /// <inheritdoc />
        public override async void OnStart()
        {
            base.OnStart();
            await ViewModel.LoadedCommand.ExecuteAsync();
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
    }
}