using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

#nullable enable
namespace MoneyFox.Win.ViewModels.Settings
{
    public interface IWindowsSettingsViewModel : ISettingsViewModel
    {
        string ElementTheme { get; }
        ICommand SwitchThemeCommand { get; }
        RelayCommand InitializeCommand { get; }
    }
}