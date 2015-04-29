#region

using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Foundation;

#endregion

namespace MoneyManager.UserControls {
    public sealed partial class SettingsTilesUserControl {
        public SettingsTilesUserControl() {
            InitializeComponent();
        }

        public TileSettingsViewModel TileSettingsView {
            get { return ServiceLocator.Current.GetInstance<TileSettingsViewModel>(); }
        }

        private void CreateIncomeTile(object sender, RoutedEventArgs e) {
            TileSettingsView.IncomeTile.Create();
            ButtonRemoveIncomeTile.Visibility = Visibility.Visible;
        }

        private async void RemoveIncomeTile(object sender, RoutedEventArgs e) {
            TileSettingsView.IncomeTile.Remove();
            ButtonRemoveIncomeTile.Visibility = Visibility.Collapsed;
            await ShowUnpinnNotification();
        }

        private void CreateSpendingTile(object sender, RoutedEventArgs routedEventArgs) {
            TileSettingsView.SpendingTile.Create();
            ButtonRemoveSpendingTile.Visibility = Visibility.Visible;
        }

        private async void RemoveSpendingTile(object sender, RoutedEventArgs e) {
            TileSettingsView.SpendingTile.Remove();
            ButtonRemoveSpendingTile.Visibility = Visibility.Collapsed;
            await ShowUnpinnNotification();
        }

        private void CreateTransferTile(object sender, RoutedEventArgs e) {
            TileSettingsView.TransferTile.Create();
            ButtonRemoveTransferTile.Visibility = Visibility.Visible;
        }

        private async void RemoveTransferTile(object sender, RoutedEventArgs e) {
            TileSettingsView.TransferTile.Remove();
            ButtonRemoveTransferTile.Visibility = Visibility.Collapsed;
            await ShowUnpinnNotification();
        }

        private async Task ShowUnpinnNotification() {
            var dialog = new MessageDialog(Translation.GetTranslation("TileUnpinnedText"),
                Translation.GetTranslation("TileUnpinnedTitle"));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));
            dialog.DefaultCommandIndex = 1;

            await dialog.ShowAsync();
        }
    }
}