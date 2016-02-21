using MoneyManager.DataAccess;

namespace MoneyManager.Core.ViewModels
{
    public class SettingsGeneralViewModel : BaseViewModel
    {
        private readonly SettingDataAccess settingsDataAccess;

        public SettingsGeneralViewModel(SettingDataAccess settingsDataAccess)
        {
            this.settingsDataAccess = settingsDataAccess;
        }
    }
}
