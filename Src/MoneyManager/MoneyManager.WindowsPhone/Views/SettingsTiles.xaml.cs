#region

using Windows.UI.Xaml.Navigation;
using MoneyManager.Common;

#endregion

namespace MoneyManager.Views
{
    public sealed partial class SettingsTiles
    {
        public SettingsTiles()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper { get; }

        public TileSettingsViewModel TileSettingsView
        {
            get { return ServiceLocator.Current.GetInstance<TileSettingsViewModel>(); }
        }

        private void CreateIncomeTile(object sender, RoutedEventArgs e)
        {
            TileSettingsView.IncomeTile.Create();
            ButtonRemoveIncomeTile.Visibility = Visibility.Visible;
        }

        private async void RemoveIncomeTile(object sender, RoutedEventArgs e)
        {
            TileSettingsView.IncomeTile.Remove();
            ButtonRemoveIncomeTile.Visibility = Visibility.Collapsed;
            await ShowUnpinnNotification();
        }

        private void CreateSpendingTile(object sender, RoutedEventArgs routedEventArgs)
        {
            TileSettingsView.SpendingTile.Create();
            ButtonRemoveSpendingTile.Visibility = Visibility.Visible;
        }

        private async void RemoveSpendingTile(object sender, RoutedEventArgs e)
        {
            TileSettingsView.SpendingTile.Remove();
            ButtonRemoveSpendingTile.Visibility = Visibility.Collapsed;
            await ShowUnpinnNotification();
        }

        private void CreateTransferTile(object sender, RoutedEventArgs e)
        {
            TileSettingsView.TransferTile.Create();
            ButtonRemoveTransferTile.Visibility = Visibility.Visible;
        }

        private async void RemoveTransferTile(object sender, RoutedEventArgs e)
        {
            TileSettingsView.TransferTile.Remove();
            ButtonRemoveTransferTile.Visibility = Visibility.Collapsed;
            await ShowUnpinnNotification();
        }

        private async Task ShowUnpinnNotification()
        {
            var dialog = new MessageDialog(Translation.GetTranslation("TileUnpinnedText"),
                Translation.GetTranslation("TileUnpinnedTitle"));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));
            dialog.DefaultCommandIndex = 1;

            await dialog.ShowAsync();
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