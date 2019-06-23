using MoneyFox.Foundation;
using MoneyFox.Presentation.Interfaces;

namespace MoneyFox.iOS
{
    public class ThemeSelectorAdapter : IThemeSelectorAdapter
    {
        public string Theme { get; } = AppTheme.Light.ToString();

        public void SetThemeAsync(string theme)
        {
            throw new System.NotImplementedException();
        }
    }
}