using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using MoneyManager.Src;

namespace MoneyManager.UserControls
{
    public sealed partial class SettingsTilesUserControl
    {
        public SettingsTilesUserControl()
        {
            InitializeComponent();
        }

        private void CreateIntakeTile(object sender, TappedRoutedEventArgs e)
        {
            TileHelper.CreateSecondaryTile();
        }
    }
}
