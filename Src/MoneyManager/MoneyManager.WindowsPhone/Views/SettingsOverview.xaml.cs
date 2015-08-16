using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using MoneyManager.Common;

namespace MoneyManager.Views
{
    public sealed partial class SettingsOverview
    {
        public SettingsOverview()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
        }

        private NavigationHelper NavigationHelper { get; }

        private void NavigateToDefaultSettings(object sender, TappedRoutedEventArgs e)
        {
            ((Frame) Window.Current.Content).Navigate(typeof (SettingsDefaults));
        }

        private void NavigateToGeneralSettings(object sender, TappedRoutedEventArgs e)
        {
            ((Frame) Window.Current.Content).Navigate(typeof (SettingsRegion));
        }

        private void NavigateToBackupSettings(object sender, TappedRoutedEventArgs e)
        {
            ((Frame) Window.Current.Content).Navigate(typeof (SettingsBackup));
        }

        private void NavigateToCategorySettings(object sender, TappedRoutedEventArgs e)
        {
            ((Frame) Window.Current.Content).Navigate(typeof (SettingsCategory));
        }

        private void NavigateToTilesSettings(object sender, TappedRoutedEventArgs e)
        {
            ((Frame) Window.Current.Content).Navigate(typeof (SettingsTiles));
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration
    }
}