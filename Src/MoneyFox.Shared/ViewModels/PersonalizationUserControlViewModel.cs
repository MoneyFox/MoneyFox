using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Shared.ViewModels
{
    public class PersonalizationUserControlViewModel : BaseViewModel
    {
        private readonly ISettingsManager settingsManager;

        public PersonalizationUserControlViewModel(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public bool IsDarkThemeEnabled
        {
            get { return settingsManager.IsDarkThemeSelected; }
            set { settingsManager.IsDarkThemeSelected = value; }
        }
    }
}