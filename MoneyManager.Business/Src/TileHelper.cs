using MoneyManager.Business.Tiles;
using MoneyManager.Models.Tiles;

namespace MoneyManager.Src
{
    internal class TileHelper
    {
        public static void DoNavigation(string tileId)
        {
            if (tileId == IncomeTile.Id)
            {
                TransactionHelper.GoToAddTransaction(TransactionType.Income);
            }
            else if (tileId == SpendingTile.Id)
            {
                TransactionHelper.GoToAddTransaction(TransactionType.Spending);
            }
            else if (tileId == TransferTile.Id)
            {
                TransactionHelper.GoToAddTransaction(TransactionType.Transfer);
            }
        }
    }
}