using System.Windows.Input;

namespace MoneyFox.Presentation.ViewModels.Settings
{
    public interface ISettingsPersonalizationViewModel
    {
        string ElementTheme { get; }

        ICommand SwitchThemeCommand { get; }
    }
}