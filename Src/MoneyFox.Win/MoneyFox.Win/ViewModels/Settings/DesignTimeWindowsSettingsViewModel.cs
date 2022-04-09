namespace MoneyFox.Win.ViewModels.Settings;

using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

public class DesignTimeWindowsSettingsViewModel : DesignTimeSettingsViewModel, IWindowsSettingsViewModel
{
    public string ElementTheme => "";

    public ICommand SwitchThemeCommand { get; } = null!;

    public AsyncRelayCommand InitializeCommand { get; } = null!;
}
