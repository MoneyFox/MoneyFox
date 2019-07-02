using MoneyFox.Foundation;
using MoneyFox.Presentation;
using MoneyFox.Presentation.Interfaces;

namespace MoneyFox.iOS
{
    public class ThemeSelectorAdapter : IThemeSelectorAdapter
    {
        public string Theme => ThemeManager.CurrentTheme().ToString();

        public void SetThemeAsync(string theme)
        {
            ThemeManager.ChangeTheme(theme == "Light" ? AppTheme.Light : AppTheme.Dark);
        }
    }
}