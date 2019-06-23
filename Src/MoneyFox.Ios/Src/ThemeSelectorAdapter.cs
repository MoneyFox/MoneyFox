using MoneyFox.Foundation;
using MoneyFox.Presentation;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Interfaces;

namespace MoneyFox.iOS
{
    public class ThemeSelectorAdapter : IThemeSelectorAdapter
    {
        private readonly ISettingsFacade settingsFacade;

        public ThemeSelectorAdapter(ISettingsFacade settingsFacade)
        {
            this.settingsFacade = settingsFacade;
        }

        public string Theme => settingsFacade.Theme.ToString();

        public void SetThemeAsync(string theme)
        {
            settingsFacade.Theme = theme == "Light" ? AppTheme.Light : AppTheme.Dark;
            StyleHelper.Init();
        }
    }
}