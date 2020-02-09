using System.Windows.Input;

namespace MoneyFox.Uwp.ViewModels.Settings
{
    public interface ISettingsPersonalizationViewModel
    {
        string ElementTheme { get; }

        ICommand SwitchThemeCommand { get; }
    }
}
