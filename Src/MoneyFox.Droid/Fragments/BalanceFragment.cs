using Android.Runtime;
using MoneyFox.Business.ViewModels;

namespace MoneyFox.Droid.Fragments
{
    [Register("moneyfox.droid.fragments.BalanceFragment")]
    public class BalanceFragment : BaseFragment<BalanceViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_balance;
        protected override string Title => string.Empty;

        public override async void OnStart()
        {
            OnResume();
            RetainInstance = false;
            if (ViewModel != null)
            {
                await ViewModel.UpdateBalanceCommand.ExecuteAsync();
            }
        }
    }
}