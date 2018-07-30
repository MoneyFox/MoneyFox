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
    }
}