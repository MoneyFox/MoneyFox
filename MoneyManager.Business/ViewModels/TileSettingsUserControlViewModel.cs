using GalaSoft.MvvmLight;
using MoneyManager.Business.Tiles;
using MoneyManager.Models.Tiles;

namespace MoneyManager.Business.ViewModels
{
    internal class TileSettingsUserControlViewModel : ViewModelBase
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