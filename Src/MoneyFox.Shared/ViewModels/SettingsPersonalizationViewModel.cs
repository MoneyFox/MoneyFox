using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Shared.ViewModels
{
    public class SettingsPersonalizationViewModel : BaseViewModel
    {
        private readonly ISettingsManager settingsManager;

        public SettingsPersonalizationViewModel(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public bool IsDarkThemeEnabled
        {
            get { return settingsManager.IsDarkThemeSelected; }
            set { settingsManager.IsDarkThemeSelected = value; }
        }
    }
}