using MoneyFox.Uwp.ViewModels.Settings;
using System.Windows.Input;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeSettingsPersonalizationViewModel : ISettingsPersonalizationViewModel
    {
        public string ElementTheme { get; }

        public ICommand SwitchThemeCommand { get; }
    }
}
