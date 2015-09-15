using MoneyManager.Core.ViewModels;
using MoneyManager.Windows.Shortcut;

namespace MoneyManager.Windows
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
                case IncomeTile.ID:
                    modifyTransactionViewModel.IsEdit = false;
                    ShowViewModel<ModifyTransactionViewModel>(new {typeString = "Income"});
                    return true;

                case SpendingTile.ID:
                    modifyTransactionViewModel.IsEdit = false;
                    ShowViewModel<ModifyTransactionViewModel>(new {typeString = "Spending"});
                    return true;

                case TransferTile.ID:
                    modifyTransactionViewModel.IsEdit = false;
                    ShowViewModel<ModifyTransactionViewModel>(new {typeString = "Transfer"});
                    return true;
                default:
                    return false;
            }
        }
    }
}