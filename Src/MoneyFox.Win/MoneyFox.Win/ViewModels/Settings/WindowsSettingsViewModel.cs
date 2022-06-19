namespace MoneyFox.Win.ViewModels.Settings;

using CommunityToolkit.Mvvm.Input;
using Core.Common.Facades;
using Core.Common.Interfaces;

internal sealed class WindowsSettingsViewModel : SettingsViewModel, IWindowsSettingsViewModel
{
    public WindowsSettingsViewModel(ISettingsFacade settingsFacade, IDialogService dialogService) : base(
        settingsFacade: settingsFacade,
        dialogService: dialogService) { }

    public AsyncRelayCommand InitializeCommand => new(async () => await InitializeAsync());
}
