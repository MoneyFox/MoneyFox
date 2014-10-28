using MoneyManager.Business.Src;
using MoneyManager.Foundation;

namespace MoneyManager.Business.Logic.Tile
{
    public class TileLogic
    {
        public static void DoNavigation(string tileId)
        {
            switch (tileId)
            {
                case IncomeTile.Id:
                    TransactionLogic.GoToAddTransaction(TransactionType.Income);
                    break;

                case SpendingTile.Id:
                    TransactionLogic.GoToAddTransaction(TransactionType.Spending);
                    break;

                case TransferTile.Id:
                    TransactionLogic.GoToAddTransaction(TransactionType.Transfer);
                    break;
            }
        }
    }
}