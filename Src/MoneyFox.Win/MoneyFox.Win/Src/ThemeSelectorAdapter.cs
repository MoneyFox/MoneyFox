using Microsoft.UI.Xaml;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Win.Services;
using System;

namespace MoneyFox.Win
{
    public class ThemeSelectorAdapter : IThemeSelectorAdapter
    {
        public string Theme => ThemeSelectorService.Theme.ToString();

        public void SetTheme(string theme)
        {
            if(Enum.TryParse(theme, out ElementTheme cacheTheme))
            {
                ThemeSelectorService.SetThemeAsync(cacheTheme).GetAwaiter().GetResult();
            }
        }
    }
}