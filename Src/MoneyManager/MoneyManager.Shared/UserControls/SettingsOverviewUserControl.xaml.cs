using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using MoneyManager.Views;

namespace MoneyManager.UserControls {
    public sealed partial class SettingsOverviewUserControl {
        public SettingsOverviewUserControl() {
            InitializeComponent();
        }

        private void NavigateToDefaultSettings(object sender, TappedRoutedEventArgs e) {
            ((Frame) Window.Current.Content).Navigate(typeof (SettingsDefaults));
        }

        private void NavigateToGeneralSettings(object sender, TappedRoutedEventArgs e) {
            ((Frame) Window.Current.Content).Navigate(typeof (SettingsRegion));
        }

        private void NavigateToBackupSettings(object sender, TappedRoutedEventArgs e) {
            ((Frame) Window.Current.Content).Navigate(typeof (SettingsBackup));
        }

        private void NavigateToCategorySettings(object sender, TappedRoutedEventArgs e) {
            ((Frame) Window.Current.Content).Navigate(typeof (SettingsCategory));
        }

        private void NavigateToTilesSettings(object sender, TappedRoutedEventArgs e) {
            ((Frame) Window.Current.Content).Navigate(typeof (SettingsTiles));
        }
    }
}