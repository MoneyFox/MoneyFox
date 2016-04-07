using GalaSoft.MvvmLight;
using MoneyFox.Core.SettingAccess;

namespace MoneyFox.Core.ViewModels
{
    public class SettingsGeneralViewModel : ViewModelBase
    {
        public bool IsAutoBackupEnabled
        {
            get { return Settings.IsBackupAutouploadEnabled; }
            set
            {
                Settings.IsBackupAutouploadEnabled = value;
                RaisePropertyChanged();
            }
        }
    }
}