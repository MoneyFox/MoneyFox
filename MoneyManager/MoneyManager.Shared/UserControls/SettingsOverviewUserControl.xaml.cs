using Windows.UI.Xaml;
using MoneyManager.Src;

namespace MoneyManager.UserControls
{
    public sealed partial class SettingsOverviewUserControl
    {
        public SettingsOverviewUserControl()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            TileHelper.CreateSecondaryTile();
        }
    }
}