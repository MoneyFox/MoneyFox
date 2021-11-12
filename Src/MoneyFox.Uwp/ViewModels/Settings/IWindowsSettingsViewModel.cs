using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Settings
{
    public interface IWindowsSettingsViewModel : ISettingsViewModel
    {
        string ElementTheme { get; }
        ICommand SwitchThemeCommand { get; }
        RelayCommand InitializeCommand { get; }
    }
}