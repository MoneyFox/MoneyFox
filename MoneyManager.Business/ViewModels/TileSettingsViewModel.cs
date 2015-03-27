using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Helper;
using MoneyManager.Business.Logic;
using MoneyManager.Business.Logic.Tile;
using MoneyManager.DataAccess.DataAccess;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class TileSettingsViewModel : ViewModelBase {

        private readonly LicenseManager _licenseManager;

        public TileSettingsViewModel(LicenseManager licenseManager) {
            _licenseManager = licenseManager;
        }

        public bool ShowInfoOnMainTile {
            get {
                if (_licenseManager.IsFeaturepackLicensed) {
                    return ServiceLocator.Current.GetInstance<SettingDataAccess>().ShowCashFlowOnMainTile;
                }
                return false;
            }
            set { SetValue(value); }
        }

        public IncomeTile IncomeTile {
            get { return new IncomeTile(); }
        }

        public SpendingTile SpendingTile {
            get { return new SpendingTile(); }
        }

        public TransferTile TransferTile {
            get { return new TransferTile(); }
        }

        private async void SetValue(bool value) {
            if (await CheckLicense()) {
                ServiceLocator.Current.GetInstance<SettingDataAccess>().ShowCashFlowOnMainTile = value;
            }
        }

        private async Task<bool> CheckLicense() {
            if (!_licenseManager.IsFeaturepackLicensed) {
                await ServiceLocator.Current.GetInstance<Utilities>().ShowFeatureNotLicensedMessage();
            }
            else {
                Tile.UpdateMainTile();
            }

            return _licenseManager.IsFeaturepackLicensed;
        }
    }
}