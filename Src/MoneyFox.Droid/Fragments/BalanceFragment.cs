using Android.Runtime;
using MoneyManager.Core.ViewModels;

namespace MoneyFox.Droid.Fragments
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