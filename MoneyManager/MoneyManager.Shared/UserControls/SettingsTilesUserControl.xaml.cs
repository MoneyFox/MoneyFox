using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Src;
using MoneyManager.ViewModels;

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

        private async void ShowUnpinnNotification()
        {
            var dialog = new MessageDialog(Utilities.GetTranslation("TileUnpinnedText"),
                Utilities.GetTranslation("TileUnpinnedTitle"));
            dialog.Commands.Add(new UICommand(Utilities.GetTranslation("OkLabel")));
            dialog.DefaultCommandIndex = 1;

            await dialog.ShowAsync();
        }

        private void CreateIncomeTile(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RemoveIncomeTile(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}