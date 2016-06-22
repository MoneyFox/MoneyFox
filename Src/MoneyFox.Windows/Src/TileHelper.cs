using MoneyFox.Shared.Constants;
using MoneyFox.Shared.ViewModels;

namespace MoneyFox.Windows {
    public class TileHelper : BaseViewModel {
        public bool DoNavigation(string tileId) {
            switch (tileId) {
                case Constants.ADD_INCOME_TILE_ID:
                    ShowViewModel<ModifyPaymentViewModel>(new {typeString = Constants.INCOME_TILE_ID});
                    return true;

                case Constants.ADD_EXPENSE_TILE_ID:
                    ShowViewModel<ModifyPaymentViewModel>(new {typeString = Constants.EXPENSE_TILE_ID});
                    return true;

                case Constants.ADD_TRANSFER_TILE_ID:
                    ShowViewModel<ModifyPaymentViewModel>(new {typeString = Constants.TRANSFER_TILE_ID});
                    return true;
                default:
                    return false;
            }
        }
    }
}