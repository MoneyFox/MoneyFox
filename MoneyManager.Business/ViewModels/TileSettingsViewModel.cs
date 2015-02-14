#region

using GalaSoft.MvvmLight;
using MoneyManager.Business.Logic.Tile;
using PropertyChanged;

#endregion

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class TileSettingsViewModel : ViewModelBase {
        public IncomeTile IncomeTile {
            get { return new IncomeTile(); }
        }

        public SpendingTile SpendingTile {
            get { return new SpendingTile(); }
        }

        public TransferTile TransferTile {
            get { return new TransferTile(); }
        }
    }
}