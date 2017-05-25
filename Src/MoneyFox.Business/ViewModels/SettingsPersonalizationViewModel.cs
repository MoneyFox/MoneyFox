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
            get => settingsManager.IsDarkThemeSelected;
            set => settingsManager.IsDarkThemeSelected = value;
        }

        public bool UseSystemTheme
        {
            get => settingsManager.UseSystemTheme;
            set => settingsManager.UseSystemTheme = value;
        }

        public bool UseCustomTheme => !UseSystemTheme;
    }
}