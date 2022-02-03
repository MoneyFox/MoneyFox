using CommunityToolkit.Mvvm.Input;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;

namespace MoneyFox.Win.ViewModels.Settings
{
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
}