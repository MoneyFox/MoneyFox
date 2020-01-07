using MoneyFox.Presentation.ViewModels.Settings;
using System.Windows.Input;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeSettingsPersonalizationViewModel : ISettingsPersonalizationViewModel
    {
        public string ElementTheme { get; }
        public ICommand SwitchThemeCommand { get; }
    }
}
