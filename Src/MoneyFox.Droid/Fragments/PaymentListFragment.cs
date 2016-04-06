using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MoneyManager.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.PaymentListFragment")]
    public class PaymentListFragment : MvxFragment<PaymentListViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_payment_list, null);

            ((Activities.MainActivity)Activity).SetSupportActionBar(view.FindViewById<Toolbar>(Resource.Id.toolbar));
            ((Activities.MainActivity)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            HasOptionsMenu = true;

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

        public override void OnResume ()
		{
			base.OnResume ();

			ViewModel.LoadCommand.Execute();
		}

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.menu_main, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }
    }
}