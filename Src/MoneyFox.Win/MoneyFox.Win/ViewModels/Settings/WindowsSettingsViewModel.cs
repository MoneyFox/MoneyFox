namespace MoneyFox.Win.ViewModels.Settings;

using CommunityToolkit.Mvvm.Input;
using Core._Pending_.Common.Facades;
using Core.Common.Interfaces;

public class WindowsSettingsViewModel : SettingsViewModel, IWindowsSettingsViewModel
{
    public WindowsSettingsViewModel(
        ISettingsFacade settingsFacade,
        IDialogService dialogService)
        : base(settingsFacade, dialogService)
    {
    }

    public AsyncRelayCommand InitializeCommand => new(async () => await InitializeAsync());
}