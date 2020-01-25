using System.Windows.Input;
using MoneyFox.Presentation.ViewModels.Settings;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeSettingsPersonalizationViewModel : ISettingsPersonalizationViewModel
    {
        public string ElementTheme { get; }
        public ICommand SwitchThemeCommand { get; }
    }
}
