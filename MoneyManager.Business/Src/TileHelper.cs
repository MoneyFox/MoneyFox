using MoneyManager.Business.Tiles;
using MoneyManager.Foundation;

namespace MoneyManager.Business.Src
{
    internal class TileHelper
    {
        public static void DoNavigation(string tileId)
        {
            switch (tileId)
            {
                case IncomeTile.Id:
                    TransactionHelper.GoToAddTransaction(TransactionType.Income);
                    break;

                case SpendingTile.Id:
                    TransactionHelper.GoToAddTransaction(TransactionType.Spending);
                    break;

                case TransferTile.Id:
                    TransactionHelper.GoToAddTransaction(TransactionType.Transfer);
                    break;
            }
        }
    }
}