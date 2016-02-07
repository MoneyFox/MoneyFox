using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Interfaces.ViewModels;

namespace MoneyManager.Droid.ViewModels
{
    public class DroidMainViewModel : MainViewModel
    {
        public DroidMainViewModel(ModifyAccountViewModel modifyAccountViewModel,
            ModifyPaymentViewModel modifyPaymentViewModel, IBalanceViewModel balanceViewModel)
            : base(modifyAccountViewModel, modifyPaymentViewModel, balanceViewModel)
        {
        }

        public void ShowMenuAndFirstDetail()
        {
            ShowViewModel<MenuViewModel>();
            //ShowViewModel<AccountListViewModel>();
        }
    }
}