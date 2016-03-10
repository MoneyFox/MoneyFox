using GalaSoft.MvvmLight;
using MoneyManager.DataAccess;

namespace MoneyFox.Core.ViewModels
{
    public class SettingsGeneralViewModel : ViewModelBase
    {
        private readonly SettingDataAccess settingsDataRepository;

        public SettingsGeneralViewModel(SettingDataAccess settingsDataRepository)
        {
            this.settingsDataRepository = settingsDataRepository;
        }

        public bool IsAutoBackupEnabled
        {
            get { return settingsDataRepository.IsBackupAutouploadEnabled; }
            set
            {
                settingsDataRepository.IsBackupAutouploadEnabled = value;
                RaisePropertyChanged();
            }
        }
    }
}