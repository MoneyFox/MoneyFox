using CommunityToolkit.Mvvm.Input;

namespace MoneyFox.Win.ViewModels.Settings
{
    public interface IWindowsSettingsViewModel : ISettingsViewModel
    {
        AsyncRelayCommand InitializeCommand { get; }
    }
}