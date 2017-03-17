using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Business.ViewModels
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

        public bool UseSystemTheme
        {
            get { return settingsManager.UseSystemTheme; }
            set { settingsManager.UseSystemTheme = value; }
        }

        
    }
}