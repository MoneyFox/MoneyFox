namespace MoneyFox.Win.ViewModels.Settings;

using CommunityToolkit.Mvvm.Input;

public interface IWindowsSettingsViewModel : ISettingsViewModel
{
    AsyncRelayCommand InitializeCommand { get; }
}