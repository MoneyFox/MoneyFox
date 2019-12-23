using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Presentation;

namespace MoneyFox.Droid
{
    public class ThemeSelectorAdapter : IThemeSelectorAdapter
    {
        public string Theme => ThemeManager.CurrentTheme().ToString();

        public void SetTheme(string theme)
        {
            ThemeManager.ChangeTheme(theme == "Light" ? AppTheme.Light : AppTheme.Dark);
        }
    }
}
