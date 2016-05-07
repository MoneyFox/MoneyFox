using Android.Runtime;
using MoneyFox.Shared.ViewModels;

namespace MoneyFox.Droid.Fragments
{
    [Register("moneyfox.droid.fragments.BalanceFragment")]
    public class BalanceFragment : BaseFragment<BalanceViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_balance;
        protected override string Title => string.Empty;

        public override void OnStart()
        {
            OnResume();

            ViewModel.UpdateBalanceCommand.Execute();
        }
    }
}