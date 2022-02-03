using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace MoneyFox.Win.ViewModels.Settings
{
    public class DesignTimeWindowsSettingsViewModel : DesignTimeSettingsViewModel, IWindowsSettingsViewModel
    {
        public string ElementTheme => "";

        public ICommand SwitchThemeCommand { get; } = null!;

        public AsyncRelayCommand InitializeCommand { get; } = null!;
    }
}