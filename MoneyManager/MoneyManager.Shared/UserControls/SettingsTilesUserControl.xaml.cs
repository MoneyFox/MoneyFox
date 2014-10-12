using MoneyManager.Src;
using Windows.UI.Xaml.Input;

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