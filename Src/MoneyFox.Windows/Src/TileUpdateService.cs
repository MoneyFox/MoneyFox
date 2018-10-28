using Windows.UI.Notifications;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using NotificationsExtensions;
using NotificationsExtensions.Tiles;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.UI.StartScreen;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.ApplicationModel.Background;

namespace MoneyFox.Windows.Business
{
    public sealed class TileUpdateService : ITileUpdateService
    {
        /// <summary>
        ///     Sets the MainTile with new Information
        /// </summary>
        /// <param name="income">Income of these month</param>
        /// <param name="spending">Expense of these month</param>
        /// <param name="earnings">Earnings of these month </param>
        public void UpdateMainTile(string income, string spending, string earnings)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();

            var content = new TileContent
            {
                Visual = new TileVisual
                {
                    TileMedium = GetBindingMediumContent(income, spending, earnings),
                    TileWide = GetBindingWideContent(income, spending, earnings),
                    TileLarge = GetBindingLargeContent(income, spending, earnings)
                } 
            };

            // Update Tile
            TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(content.GetXml()));
        }

        private static TileBinding GetBindingMediumContent(string income, string spending, string earnings)
        {
            var content = new TileBindingContentAdaptive
            {
                PeekImage = new TilePeekImage
                {
                    Source = "Assets/Square150x150Logo.png"
                },
                Children =
                {
                    new AdaptiveText
                    {
                        Text = income,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                        HintWrap = true
                    },
                    new AdaptiveText
                    {
                        Text = spending,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                        HintWrap = true
                    },
                    new AdaptiveText
                    {
                        Text = earnings,
                        HintWrap = true,
                        HintStyle = AdaptiveTextStyle.Caption
                    }
                }
            };

            return new TileBinding
            {
                Branding = TileBranding.NameAndLogo,
                DisplayName = Strings.ApplicationTitle,
                Content = content
            };
        }

        private static TileBinding GetBindingWideContent(string income, string spending, string earnings)
        {
            var content = new TileBindingContentAdaptive
            {
                PeekImage = new TilePeekImage
                {
                    Source = "Assets/Wide310x150Logo.png"
                },
                Children =
                {
                    new AdaptiveText
                    {
                        Text = income,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },
                    new AdaptiveText
                    {
                        Text = spending,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },
                    new AdaptiveText
                    {
                        Text = earnings,
                        HintWrap = true,
                        HintStyle = AdaptiveTextStyle.Body
                    }
                }
            };

            return new TileBinding
            {
                Branding = TileBranding.NameAndLogo,
                DisplayName = Strings.ApplicationTitle,
                Content = content
            };
        }

        private static TileBinding GetBindingLargeContent(string income, string spending, string earnings)
        {
            var content = new TileBindingContentAdaptive
            {
                PeekImage = new TilePeekImage
                {
                    Source = "Assets/Square310x310Logo.png"
                },
                Children =
                {
                    new AdaptiveText
                    {
                        Text = income,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },
                    new AdaptiveText
                    {
                        Text = spending,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },
                    new AdaptiveText
                    {
                        Text = earnings,
                        HintWrap = true,
                        HintStyle = AdaptiveTextStyle.Body
                    }
                }
            };

            return new TileBinding
            {
                Branding = TileBranding.NameAndLogo,
                DisplayName = Strings.ApplicationTitle,
                Content = content
            };
        }
    }
	public static class LiveTiles
	{
		public static bool PrimaryPinned;
		public static bool SecondaryPinned;
		public static void CheckPrimary()
		{
			AppListEntry entry = Package.Current.GetAppListEntriesAsync().GetResults()[0];
			PrimaryPinned = StartScreenManager.GetDefault().ContainsAppListEntryAsync(entry).GetResults();
		}
		public static void CheckSecondary()
		{
			var tiles = SecondaryTile.FindAllForPackageAsync().GetResults();
			if (tiles != null)
			{
				SecondaryPinned = true;
			}
			else
			{
				SecondaryPinned = false;
			}
		}
		public static void CheckorStartLiveTileService()
		{
			CheckPrimary();
			CheckSecondary();
			if (PrimaryPinned || SecondaryPinned)
			{
				BackgroundTaskHelper.Register(typeof(Tasks.UpdateliveandLockscreenTiles), new TimeTrigger(15, false));
			}
		}
	}
}