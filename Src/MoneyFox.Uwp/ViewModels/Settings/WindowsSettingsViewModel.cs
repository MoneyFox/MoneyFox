using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Ui.Shared.ViewModels.Settings;
using System.Windows.Input;

namespace MoneyFox.Uwp.ViewModels.Settings
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

        public string ElementTheme => themeSelectorAdapter.Theme;

        private ICommand switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                return switchThemeCommand ??= new RelayCommand<string>(param =>
                    {
                        themeSelectorAdapter.SetTheme(param);
                    });
            }
        }
    }
}
