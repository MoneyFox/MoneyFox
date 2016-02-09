using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MoneyManager.Core.ViewModels;
using Android.Runtime;
using MoneyManager.Droid.Fragments;

namespace MoneyManager.Droid
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneymanager.droid.fragments.PaymentListFragment")]
    public class PaymentListFragment : BaseFragment<PaymentListViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_payment_list;        
    }
}

