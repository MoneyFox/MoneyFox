using Android.OS;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MoneyManager.Core.ViewModels;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MoneyManager.Droid.Activities;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;

namespace MoneyManager.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneymanager.droid.fragments.PaymentListFragment")]
    public class PaymentListFragment : MvxFragment<PaymentListViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_payment_list, null);

            ((MainActivity)Activity).SetSupportActionBar(view.FindViewById<Toolbar>(Resource.Id.toolbar));
            ((MainActivity)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            return view;
        }

        public override void OnResume ()
		{
			base.OnResume ();

			ViewModel.LoadCommand.Execute();
		}
    }
}

