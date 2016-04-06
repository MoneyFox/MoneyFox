using Android.Runtime;
using MoneyFox.Droid;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;

namespace MoneyManager.Droid.Fragments
{
    [Register("moneymanager.droid.fragments.BalanceFragment")]
    public class BalanceFragment : BaseFragment<BalanceViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_balance;

        public override void OnStart()
        {
            base.OnResume();

            ViewModel.UpdateBalanceCommand.Execute();
        }
    }
}