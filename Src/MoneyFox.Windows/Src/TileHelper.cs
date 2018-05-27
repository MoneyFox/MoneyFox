using System.Threading.Tasks;
using MoneyFox.Business.Parameters;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Constants;
using MvvmCross.Navigation;

namespace MoneyFox.Windows
{
    public class TileHelper
    {
        private IMvxNavigationService navigationService;

        public TileHelper(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public async Task<bool> DoNavigation(string tileId)
        {
            switch (tileId)
            {
                case Constants.ADD_INCOME_TILE_ID:
                    await navigationService.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Income));
                    return true;

                case Constants.ADD_EXPENSE_TILE_ID:
                    await navigationService.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Expense));
                    return true;

                case Constants.ADD_TRANSFER_TILE_ID:
                    await navigationService.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Transfer));
                    return true;
                default:
                    return false;
            }
        }
    }
}