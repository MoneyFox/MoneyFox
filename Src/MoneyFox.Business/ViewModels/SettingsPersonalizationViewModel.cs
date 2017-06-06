using MoneyFox.Foundation.Interfaces;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    public class SettingsPersonalizationViewModel : MvxViewModel
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