using CommunityToolkit.Mvvm.Input;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using System.Windows.Input;

namespace MoneyFox.Win.ViewModels.Settings
{
    public class WindowsSettingsViewModel : SettingsViewModel, IWindowsSettingsViewModel
    {
        private readonly IThemeSelectorAdapter themeSelectorAdapter;

        public WindowsSettingsViewModel(ISettingsFacade settingsFacade,
            IDialogService dialogService,
            IThemeSelectorAdapter themeSelectorAdapter)
            : base(settingsFacade, dialogService)
        {
            this.themeSelectorAdapter = themeSelectorAdapter;
        }

        public AsyncRelayCommand InitializeCommand => new (async () => await InitializeAsync());

        public string ElementTheme => themeSelectorAdapter.Theme;

        public ICommand SwitchThemeCommand => new RelayCommand<string>(
            param =>
            {
                themeSelectorAdapter.SetTheme(param);
            });
    }
}