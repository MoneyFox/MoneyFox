using GalaSoft.MvvmLight;
using MoneyManager.Business.Tiles;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
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