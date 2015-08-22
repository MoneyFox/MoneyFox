using GalaSoft.MvvmLight;
using MoneyManager.Business.DataAccess;
using MoneyManager.Business.Logic.Tile;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    /// <summary>
    ///     Provides the information for the TileSettingsView
    /// </summary>
    [ImplementPropertyChanged]
    public class TileSettingsViewModel : ViewModelBase
    {
        private readonly SettingDataAccess _settingDataAccess;

        /// <summary>
        ///     Creates a TileSettingsViewModel object
        /// </summary>
        /// <param name="settingDataAccess">Instance of settingDataAccess</param>
        public TileSettingsViewModel(SettingDataAccess settingDataAccess)
        {
            _settingDataAccess = settingDataAccess;
        }

        /// <summary>
        ///     Returns the setting if the Cash Flow shall be displayed on the tile
        /// </summary>
        public bool ShowInfoOnMainTile
        {
            get { return _settingDataAccess.ShowCashFlowOnMainTile; }
            set { SetValue(value); }
        }

        /// <summary>
        ///     Creates an IncomeTile object
        /// </summary>
        public IncomeTile IncomeTile => new IncomeTile();

        /// <summary>
        ///     Creates an SpendingTile object
        /// </summary>
        public SpendingTile SpendingTile => new SpendingTile();

        /// <summary>
        ///     Creates a TransferTile Object
        /// </summary>
        public TransferTile TransferTile => new TransferTile();

        private void SetValue(bool value)
        {
            _settingDataAccess.ShowCashFlowOnMainTile = value;
        }
    }
}