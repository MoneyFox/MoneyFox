using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Settings
{
    public class DesignTimeWindowsSettingsViewModel : DesignTimeSettingsViewModel, IWindowsSettingsViewModel
    {
        public string ElementTheme => "";

        public ICommand SwitchThemeCommand { get; } = null!;

        public RelayCommand InitializeCommand { get; } = null!;
    }
}