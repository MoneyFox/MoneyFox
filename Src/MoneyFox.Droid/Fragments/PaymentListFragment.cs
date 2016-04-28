using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MoneyFox.Droid.Activities;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof (MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.PaymentListFragment")]
    public class PaymentListFragment : BaseFragment<PaymentListViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_account_list;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            ((MainActivity) Activity).SetSupportActionBar(view.FindViewById<Toolbar>(Resource.Id.toolbar));
            ((MainActivity) Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            LoadBalancePanel();

            return view;
        }

        private void LoadBalancePanel()
        {
            var fragment = new BalanceFragment
            {
                ViewModel = (PaymentListBalanceViewModel) ViewModel.BalanceViewModel
            };

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.payment_list_balance_frame, fragment)
                .Commit();
        }

        public override void OnResume()
        {
            base.OnResume();

            ViewModel.LoadCommand.Execute();
        }
    }
}