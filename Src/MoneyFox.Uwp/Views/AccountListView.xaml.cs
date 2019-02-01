using System;
using System.Globalization;
using Windows.ApplicationModel;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using GenericServices;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.ServiceLayer.ViewModels.DesignTime;
using MoneyFox.Uwp.Business.Tiles;
using MvvmCross;

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
				ViewModel = new DesignTimeAccountListViewModel();
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

			(DataContext as AccountListViewModel)?.DeleteAccountCommand.Execute(account);
		}

        private async void AddToStartMenu_ClickAsync(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            if (!(element.DataContext is AccountViewModel account)) return;
            if (!Mvx.IoCProvider.CanResolve<ICrudServicesAsync>()) return;

            var liveTileManager = new LiveTileManager(Mvx.IoCProvider.Resolve<ICrudServicesAsync>());

            int id = account.Id;
            bool isPinned = SecondaryTile.Exists(id.ToString(CultureInfo.InvariantCulture));
            if (!isPinned)
            {

                SecondaryTile tile = new SecondaryTile(id.ToString(CultureInfo.InvariantCulture), "Money Fox", "a", new Uri("ms-appx:///Assets/SmallTile.scale-150.png"), TileSize.Default);
                tile.VisualElements.ShowNameOnSquare150x150Logo = false;
                tile.VisualElements.ShowNameOnSquare310x310Logo = true;
                tile.VisualElements.ShowNameOnWide310x150Logo = false;
                tile.VisualElements.Square310x310Logo = new Uri("ms-appx:///Assets/Square310x310Logo.scale-100.png");
                tile.VisualElements.Square150x150Logo = new Uri("ms-appx:///Assets/Square150x150Logo.scale-100.png");
                tile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.scale-100.png");
                tile.VisualElements.Square71x71Logo = new Uri("ms-appx:///Assets/Square71x71Logo.scale-100.png");
                bool ispinned = await tile.RequestCreateAsync();
                if (ispinned)
                {
                    await liveTileManager.UpdateSecondaryLiveTiles().ConfigureAwait(false);
                }
            } else
            {
                await liveTileManager.UpdateSecondaryLiveTiles().ConfigureAwait(false);
                await liveTileManager.UpdatePrimaryLiveTile().ConfigureAwait(false);
            }
        }
    }
}