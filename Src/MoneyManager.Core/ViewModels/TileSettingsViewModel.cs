using MoneyManager.Core.DataAccess;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    /// <summary>
    ///     Provides the information for the TileSettingsView
    /// </summary>
    [ImplementPropertyChanged]
    public class TileSettingsViewModel : BaseViewModel
    {
        private readonly SettingDataAccess settingDataAccess;

        /// <summary>
        ///     Creates a TileSettingsViewModel object
        /// </summary>
        /// <param name="settingDataAccess">Instance of settingDataAccess</param>
        public TileSettingsViewModel(SettingDataAccess settingDataAccess)
        {
            this.settingDataAccess = settingDataAccess;
        }

        /// <summary>
        ///     Returns the setting if the Cash Flow shall be displayed on the tile
        /// </summary>
        public bool ShowInfoOnMainTile
        {
            get { return settingDataAccess.ShowCashFlowOnMainTile; }
            set { SetValue(value); }
        }

        /// <summary>
        ///     Creates an IncomeTile object
        /// </summary>
        /// <summary>
        ///     Creates an SpendingTile object
        /// </summary>
        /// <summary>
        ///     Creates a TransferTile Object
        /// </summary>
        //TODO: Refactor this that it can be used for android aswell
        //public TransferTile TransferTile => new TransferTile();
        private void SetValue(bool value)
        {
            settingDataAccess.ShowCashFlowOnMainTile = value;
        }
    }
}