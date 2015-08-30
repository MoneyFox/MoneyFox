using MoneyManager.Core.Logic;
using MoneyManager.Foundation;
using MoneyManager.Windows.Tile;

namespace MoneyManager.Windows
{
    public static class TileHelper
    {
        public static void DoNavigation(string tileId)
        {
            switch (tileId)
            {
                case IncomeTile.ID:
                    TransactionLogic.GoToAddTransaction(TransactionType.Income);
                    //TODO:uncomment
                    //((Frame) Window.Current.Content).Navigate(typeof (ModifyTransactionView));
                    break;

                case SpendingTile.ID:
                    TransactionLogic.GoToAddTransaction(TransactionType.Spending);
                    //TODO:uncomment
                    //((Frame) Window.Current.Content).Navigate(typeof (ModifyTransactionView));
                    break;

                case TransferTile.ID:
                    TransactionLogic.GoToAddTransaction(TransactionType.Transfer);
                    //TODO:uncomment
                    //((Frame) Window.Current.Content).Navigate(typeof (ModifyTransactionView));
                    break;
            }
        }
    }
}