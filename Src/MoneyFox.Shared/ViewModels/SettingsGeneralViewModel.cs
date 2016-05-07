using MoneyFox.Shared.Helpers;

namespace MoneyFox.Shared.ViewModels
{
    public class SettingsGeneralViewModel : BaseViewModel
    {
        public bool IsAutoBackupEnabled
        {
            get { return SettingsHelper.IsBackupAutouploadEnabled; }
            set
            {
                SettingsHelper.IsBackupAutouploadEnabled = value;
                RaisePropertyChanged();
            }
        }
    }
}