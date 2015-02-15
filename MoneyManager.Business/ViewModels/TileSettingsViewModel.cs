#region

using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Helper;
using MoneyManager.Business.Logic;
using MoneyManager.Business.Logic.Tile;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

#endregion

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class TileSettingsViewModel : ViewModelBase {

        public bool ShowInfoOnMainTile {
            get {
                if (LicenseHelper.IsFeaturepackLicensed) {
                    return ServiceLocator.Current.GetInstance<SettingDataAccess>().ShowCashFlowOnMainTile;
                }
                return false;
            }
            set {
                if (CheckLicense().Result) {
                    ServiceLocator.Current.GetInstance<SettingDataAccess>().ShowCashFlowOnMainTile = value;
                }
            }
        }

        private async static Task<bool> CheckLicense() {
            if (!LicenseHelper.IsFeaturepackLicensed) {
                await ServiceLocator.Current.GetInstance<Utilities>().ShowFeatureNotLicensedMessage();
            } else {
                Tile.UpdateMainTile();
            }

            return LicenseHelper.IsFeaturepackLicensed;
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
    }
}