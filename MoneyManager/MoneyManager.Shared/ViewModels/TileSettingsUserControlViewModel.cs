using GalaSoft.MvvmLight;
using MoneyManager.Models.Tiles;

namespace MoneyManager.ViewModels
{
    public class TileSettingsUserControlViewModel : ViewModelBase
    {
        public IncomeTile IncomeTile
        {
            get { return new IncomeTile(); }
        }

        public SpendingTile SpendingTile
        {
            get { return new SpendingTile(); }
        }

        public TransferTile TransferTile
        {
            get { return new TransferTile(); }
        }
    }
}