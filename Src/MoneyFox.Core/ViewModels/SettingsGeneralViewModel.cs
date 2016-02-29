using GalaSoft.MvvmLight;
using MoneyFox.DataAccess;

namespace MoneyManager.Core.ViewModels
{
    public class SettingsGeneralViewModel : ViewModelBase
    {
        private readonly SettingDataRepository settingsDataRepository;

        public SettingsGeneralViewModel(SettingDataRepository settingsDataRepository)
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