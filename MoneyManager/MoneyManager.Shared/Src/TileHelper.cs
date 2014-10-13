using MoneyManager.Models;
using MoneyManager.Models.Tiles;

namespace MoneyManager.Src
{
    public class TileHelper
    {
        public static void DoNavigation(string tileId)
        {
            if (tileId == IncomeTile.IncomeTileId)
            {
                TransactionHelper.GoToAddTransaction(TransactionType.Income);
            }
            else if (tileId == SpendingTile.SpendingTileId)
            {
                TransactionHelper.GoToAddTransaction(TransactionType.Spending);
            }
            else if(tileId == TransferTile.TransferTileId)
            {
                TransactionHelper.GoToAddTransaction(TransactionType.Transfer);
            }
        }
    }
}