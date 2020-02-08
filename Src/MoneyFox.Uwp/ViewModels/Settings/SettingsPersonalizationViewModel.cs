using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.Presentation.ViewModels.Settings
{
    public class SettingsPersonalizationViewModel : ViewModelBase, ISettingsPersonalizationViewModel
    {
        private readonly IThemeSelectorAdapter themeSelectorAdapter;

        public SettingsPersonalizationViewModel(IThemeSelectorAdapter themeSelectorAdapter)
        {
            this.themeSelectorAdapter = themeSelectorAdapter;
        }

        public string ElementTheme => themeSelectorAdapter.Theme;

        private ICommand switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                return switchThemeCommand
                       ?? (switchThemeCommand = new RelayCommand<string>(param => { themeSelectorAdapter.SetTheme(param); }));
            }
        }
    }
}
