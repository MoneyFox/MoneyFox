using MoneyFox.Ui.Shared.ViewModels.Settings;
using System.Windows.Input;

namespace MoneyFox.Uwp.ViewModels.Settings
{
    public class DesignTimeWindowsSettingsViewModel : DesignTimeSettingsViewModel, IWindowsSettingsViewModel
    {
        public string ElementTheme => "";

        public ICommand SwitchThemeCommand { get; } = null!;
    }
}
