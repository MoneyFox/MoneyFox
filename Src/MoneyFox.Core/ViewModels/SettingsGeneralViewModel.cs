using GalaSoft.MvvmLight;
using MoneyManager.DataAccess;

namespace MoneyManager.Core.ViewModels
{
    public class SettingsGeneralViewModel : ViewModelBase
    {
        private readonly SettingDataAccess settingsDataAccess;

        public SettingsGeneralViewModel(SettingDataAccess settingsDataAccess)
        {
            this.settingsDataAccess = settingsDataAccess;
        }

        public bool IsAutoBackupEnabled
        {
            get { return settingsDataAccess.IsBackupAutouploadEnabled; }
            set
            {
                settingsDataAccess.IsBackupAutouploadEnabled = value;
                RaisePropertyChanged();
            }
        }
    }
}