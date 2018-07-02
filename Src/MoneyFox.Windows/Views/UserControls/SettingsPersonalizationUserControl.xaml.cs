using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Windows.Views.UserControls
{
    public sealed partial class SettingsPersonalizationUserControl
    {
        public ToggleSwitch ToggleSwitch;
        public SettingsPersonalizationUserControl()
        {
            InitializeComponent();
            ToggleSwitch = ThemeSwitch;
        }

        private void ThemeSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleSwitch;
            if (toggle.IsOn)
            {
                toggle.OnContent = "On (Restart needed)";
            }
            else
            {
                toggle.OffContent = "Off (Restart needed)";
            }
        }
    }
}