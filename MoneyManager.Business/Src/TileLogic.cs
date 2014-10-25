using MoneyManager.Business.Tiles;
using MoneyManager.Foundation;

namespace MoneyManager.Business.Src
{
    internal class TileLogic
    {
        public static void DoNavigation(string tileId)
        {
            var transactionLogic = new TransactionLogic();

            switch (tileId)
            {
                case IncomeTile.Id:
                    transactionLogic.GoToAddTransaction(TransactionType.Income);
                    break;

                case SpendingTile.Id:
                    transactionLogic.GoToAddTransaction(TransactionType.Spending);
                    break;

                case TransferTile.Id:
                    transactionLogic.GoToAddTransaction(TransactionType.Transfer);
                    break;
            }
        }
    }
}