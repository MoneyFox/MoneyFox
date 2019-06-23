using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Presentation.Interfaces;

namespace MoneyFox.Presentation.ViewModels
{
    public interface ISettingsPersonalizationViewModel : IBaseViewModel
    {
        string ElementTheme { get; }

        ICommand SwitchThemeCommand { get; }
    }

    public class SettingsPersonalizationViewModel : BaseViewModel, ISettingsPersonalizationViewModel
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
                       ?? (switchThemeCommand = new RelayCommand<string>(param => { themeSelectorAdapter.SetThemeAsync(param); }));
            }
        }
    }
}