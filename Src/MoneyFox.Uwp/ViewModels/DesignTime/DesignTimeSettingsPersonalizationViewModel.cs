using System.Windows.Input;
using MoneyFox.Uwp.ViewModels.Settings;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeSettingsPersonalizationViewModel : ISettingsPersonalizationViewModel
    {
        public string ElementTheme { get; }
        public ICommand SwitchThemeCommand { get; }
    }
}
