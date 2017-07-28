using System;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    public class SettingsPersonalizationViewModel : MvxViewModel
    {
        private readonly IThemeService themeService;

        public SettingsPersonalizationViewModel(IThemeService themeService)
        {
            this.themeService = themeService;
        }

        public MvxCommand<int> SelectedTheme => new MvxCommand<int>(ThemeSelectionChanged);

        private void ThemeSelectionChanged(int index)
        {
            var theme = (AppTheme) Enum.ToObject(typeof(AppTheme), index);
            themeService.SetTheme(theme);
        }
    }
}