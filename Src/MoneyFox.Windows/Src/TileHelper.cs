using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Constants;

namespace MoneyFox.Windows
{
    public class TileHelper : BaseViewModel
    {
        public bool DoNavigation(string tileId)
        {
            switch (tileId)
            {
                case Constants.ADD_INCOME_TILE_ID:
                    ShowViewModel<ModifyPaymentViewModel>(new {type = PaymentType.Income });
                    return true;

                case Constants.ADD_EXPENSE_TILE_ID:
                    ShowViewModel<ModifyPaymentViewModel>(new {type = PaymentType.Expense });
                    return true;

                case Constants.ADD_TRANSFER_TILE_ID:
                    ShowViewModel<ModifyPaymentViewModel>(new {type= PaymentType.Transfer });
                    return true;
                default:
                    return false;
            }
        }
    }
}