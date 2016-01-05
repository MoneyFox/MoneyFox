using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;

namespace MoneyManager.Droid.Fragments
{
    public class BalanceFragment : MvxFragment
    {
        public new BalanceViewModel ViewModel
        {
            get { return (BalanceViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.BalanceLayout, null);
        }
    }
}