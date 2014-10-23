using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Microsoft.Practices.ServiceLocation;

namespace MoneyManager.UserControls
{
    public sealed partial class SettingsTilesUserControl
    {
        public SettingsTilesUserControl()
        {
            InitializeComponent();
        }

        public TileSettingsUserControlViewModel TileSettingsUserControlView
        {
            get { return ServiceLocator.Current.GetInstance<TileSettingsUserControlViewModel>(); }
        }

        private void CreateIncomeTile(object sender, RoutedEventArgs e)
        {
            TileSettingsUserControlView.IncomeTile.Create();
            ButtonRemoveIncomeTile.Visibility = Visibility.Visible;
        }

        private void RemoveIncomeTile(object sender, RoutedEventArgs e)
        {
            TileSettingsUserControlView.IncomeTile.Remove();
            ButtonRemoveIncomeTile.Visibility = Visibility.Collapsed;
            ShowUnpinnNotification();
        }

        private void CreateSpendingTile(object sender, RoutedEventArgs routedEventArgs)
        {
            TileSettingsUserControlView.SpendingTile.Create();
            ButtonRemoveSpendingTile.Visibility = Visibility.Visible;
        }

        private void RemoveSpendingTile(object sender, RoutedEventArgs e)
        {
            TileSettingsUserControlView.SpendingTile.Remove();
            ButtonRemoveSpendingTile.Visibility = Visibility.Collapsed;
            ShowUnpinnNotification();
        }

        private void CreateTransferTile(object sender, RoutedEventArgs e)
        {
            TileSettingsUserControlView.TransferTile.Create();
            ButtonRemoveTransferTile.Visibility = Visibility.Visible;
        }

        private void RemoveTransferTile(object sender, RoutedEventArgs e)
        {
            TileSettingsUserControlView.TransferTile.Remove();
            ButtonRemoveTransferTile.Visibility = Visibility.Collapsed;
            ShowUnpinnNotification();
        }

        private async void ShowUnpinnNotification()
        {
            var dialog = new MessageDialog(Utilities.GetTranslation("TileUnpinnedText"),
                Utilities.GetTranslation("TileUnpinnedTitle"));
            dialog.Commands.Add(new UICommand(Utilities.GetTranslation("OkLabel")));
            dialog.DefaultCommandIndex = 1;

            await dialog.ShowAsync();
        }
    }
}