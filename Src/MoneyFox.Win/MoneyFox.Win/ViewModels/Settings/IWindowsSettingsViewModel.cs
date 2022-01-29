using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace MoneyFox.Win.ViewModels.Settings
{
    public interface IWindowsSettingsViewModel : ISettingsViewModel
    {
        string ElementTheme { get; }
        ICommand SwitchThemeCommand { get; }
        AsyncRelayCommand InitializeCommand { get; }
    }
}