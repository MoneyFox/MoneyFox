using MoneyFox.Shared.Helpers;

namespace MoneyFox.Shared.ViewModels
{
    public class PersonalizationUserControlViewModel : BaseViewModel
    {
        public bool IsDarkThemeEnabled
        {
            get { return Settings.SelectedTheme; }
            set { Settings.SelectedTheme = value; }
        }
    }
}
