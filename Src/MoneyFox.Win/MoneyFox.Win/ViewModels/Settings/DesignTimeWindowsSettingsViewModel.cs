namespace MoneyFox.Win.ViewModels.Settings;

using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

public class DesignTimeWindowsSettingsViewModel : DesignTimeSettingsViewModel, IWindowsSettingsViewModel
{
    public string ElementTheme => "";

    public ICommand SwitchThemeCommand { get; } = null!;

    public AsyncRelayCommand InitializeCommand { get; } = null!;
}