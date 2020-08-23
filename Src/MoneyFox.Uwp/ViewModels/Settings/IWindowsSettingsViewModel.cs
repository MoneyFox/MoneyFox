using GalaSoft.MvvmLight.Command;
using MoneyFox.Ui.Shared.ViewModels.Settings;
using System.Windows.Input;

namespace MoneyFox.Uwp.ViewModels.Settings
{
    public interface IWindowsSettingsViewModel : ISettingsViewModel
    {
        string ElementTheme { get; }
        ICommand SwitchThemeCommand { get; }
        RelayCommand InitializeCommand { get; }
    }
}
