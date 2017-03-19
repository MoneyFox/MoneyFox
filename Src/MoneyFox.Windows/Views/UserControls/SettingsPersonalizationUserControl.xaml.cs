using Windows.UI.Xaml;

namespace MoneyFox.Windows.Views.UserControls
{
    public sealed partial class SettingsPersonalizationUserControl
    {
        public SettingsPersonalizationUserControl()
        {
            InitializeComponent();

        }

        public void SystemThemeToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ThemeToggleSwitch.IsEnabled = !SystemThemeToggleSwitch.IsOn;
        }
    }
}