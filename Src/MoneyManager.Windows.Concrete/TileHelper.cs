using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Windows.Concrete.Shortcut;

namespace MoneyManager.Windows.Concrete
{
    public class TileHelper : BaseViewModel
    {
        private readonly ModifyTransactionViewModel modifyTransactionViewModel;

        public TileHelper(ModifyTransactionViewModel modifyTransactionViewModel)
        {
            this.modifyTransactionViewModel = modifyTransactionViewModel;
        }

        public bool DoNavigation(string tileId)
        {
            switch (tileId)
            {
                case Constants.ADD_INCOME_TILE_ID:
                    ShowViewModel<ModifyTransactionViewModel>(new {typeString = Constants.INCOME_TILE_ID});
                    return true;

                case Constants.ADD_SPENDING_TILE_ID:
                    ShowViewModel<ModifyTransactionViewModel>(new {typeString = Constants.SPENDING_TILE_ID});
                    return true;

                case Constants.ADD_TRANSFER_TILE_ID:
                    ShowViewModel<ModifyTransactionViewModel>(new {typeString = Constants.TRANSFER_TILE_ID});
                    return true;
                default:
                    return false;
            }
        }
    }
}