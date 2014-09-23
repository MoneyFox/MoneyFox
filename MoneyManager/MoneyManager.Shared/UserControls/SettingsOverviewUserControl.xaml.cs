using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using MoneyManager.Src;
using MoneyManager.Views;

namespace MoneyManager.UserControls
{
    public sealed partial class SettingsOverviewUserControl
    {
        public SettingsOverviewUserControl()
        {
            InitializeComponent();
        }

        private void NavigateToCategorySettings(object sender, TappedRoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(SettingsCategory));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            TileHelper.CreateSecondaryTile();
        }
    }
}