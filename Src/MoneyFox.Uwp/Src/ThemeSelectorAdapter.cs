using System;
using Windows.UI.Xaml;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Ui.Shared.Utilities;
using MoneyFox.Uwp.Services;

namespace MoneyFox.Uwp
{
    public class ThemeSelectorAdapter : IThemeSelectorAdapter
    {
        public string Theme => ThemeSelectorService.Theme.ToString();

        public void SetTheme(string theme)
        {
            if (Enum.TryParse(theme, out ElementTheme cacheTheme)) ThemeSelectorService.SetThemeAsync(cacheTheme).FireAndForgetSafeAsync();
        }
    }
}
