using MoneyManager.Business.Logic;
using MoneyManager.Business.Logic.Tile;
using MoneyManager.Foundation;

namespace MoneyManager.Windows
{
    public static class TileHelper
    {
        public static void DoNavigation(string tileId)
        {
            switch (tileId)
            {
                case IncomeTile.Id:
                    TransactionLogic.GoToAddTransaction(TransactionType.Income);
                    //TODO:uncomment
                    //((Frame) Window.Current.Content).Navigate(typeof (AddTransactionView));
                    break;

                case SpendingTile.Id:
                    TransactionLogic.GoToAddTransaction(TransactionType.Spending);
                    //TODO:uncomment
                    //((Frame) Window.Current.Content).Navigate(typeof (AddTransactionView));
                    break;

                case TransferTile.Id:
                    TransactionLogic.GoToAddTransaction(TransactionType.Transfer);
                    //TODO:uncomment
                    //((Frame) Window.Current.Content).Navigate(typeof (AddTransactionView));
                    break;
            }
        }
    }
}