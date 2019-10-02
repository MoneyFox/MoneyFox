using System;
using System.Globalization;
using Windows.ApplicationModel;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using CommonServiceLocator;
using GenericServices;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Presentation.ViewModels.DesignTime;
using MoneyFox.Uwp.Business.Tiles;
using MoneyFox.Application.Resources;

namespace MoneyFox.Uwp.Views
{
    /// <summary>
    ///     View to display an list of accounts.
    /// </summary>
    public sealed partial class AccountListView
    {
        /// <summary>
        ///     Initialize View.
        /// </summary>
        public AccountListView()
		{
			InitializeComponent();

			if (DesignMode.DesignModeEnabled)
			{
				DataContext = new DesignTimeAccountListViewModel();
			}
		}

		private void AccountList_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			var senderElement = sender as FrameworkElement;
			var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

			flyoutBase?.ShowAt(senderElement, e.GetPosition(senderElement));
		}

		private void Edit_OnClick(object sender, RoutedEventArgs e)
		{
			var element = (FrameworkElement)sender;
		    if (!(element.DataContext is AccountViewModel account))
			{
				return;
			}

			(DataContext as AccountListViewModel)?.EditAccountCommand.Execute(account);
		}

		private void Delete_OnClick(object sender, RoutedEventArgs e)
		{
			//this has to be called before the dialog service since otherwise the data context is reseted and the account will be null
			var element = (FrameworkElement)sender;
		    if (!(element.DataContext is AccountViewModel account))
			{
				return;
			}

			(DataContext as AccountListViewModel)?.DeleteAccountCommand.ExecuteAsync(account).FireAndForgetSafeAsync();
		}

        private const string SMALL_150_TILE_ICON = "ms-appx:///Assets/SmallTile.scale-150.png";
        private const string SQUARE_310_TILE_ICON = "ms-appx:///Assets/Square310x310Logo.scale-100.png";
        private const string SQUARE_150_TILE_ICON = "ms-appx:///Assets/Square150x150Logo.scale-100.png";
        private const string WIDE_310_TILE_ICON = "ms-appx:///Assets/Wide310x150Logo.scale-100.png";
        private const string SQUARE_71_TILE_ICON = "ms-appx:///Assets/Square71x71Logo.scale-100.png";
        private async void AddToStartMenu_ClickAsync(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            if (!(element.DataContext is AccountViewModel account))
            {
                return;
            }

            var liveTileManager = new LiveTileManager(ServiceLocator.Current.GetInstance<ICrudServicesAsync>());

            int id = account.Id;
            bool isPinned = SecondaryTile.Exists(id.ToString(CultureInfo.InvariantCulture));
            if (!isPinned)
            {
                SecondaryTile tile = new SecondaryTile(id.ToString(CultureInfo.InvariantCulture),  Strings.ApplicationTitle, "a", new Uri(SMALL_150_TILE_ICON), TileSize.Default);
                tile.VisualElements.ShowNameOnSquare150x150Logo = false;
                tile.VisualElements.ShowNameOnSquare310x310Logo = true;
                tile.VisualElements.ShowNameOnWide310x150Logo = false;
                tile.VisualElements.Square310x310Logo = new Uri(SQUARE_310_TILE_ICON);
                tile.VisualElements.Square150x150Logo = new Uri(SQUARE_150_TILE_ICON);
                tile.VisualElements.Wide310x150Logo = new Uri(WIDE_310_TILE_ICON);
                tile.VisualElements.Square71x71Logo = new Uri(SQUARE_71_TILE_ICON);
                bool successfulPinned = await tile.RequestCreateAsync();
                if (successfulPinned)
                {
                    await liveTileManager.UpdateSecondaryLiveTiles();
                }
            }
            else
            {
                await liveTileManager.UpdateSecondaryLiveTiles();
                await liveTileManager.UpdatePrimaryLiveTile();
            }
        }
    }
}
