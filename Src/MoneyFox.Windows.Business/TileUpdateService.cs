using Windows.UI.Notifications;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using NotificationsExtensions;
using NotificationsExtensions.Tiles;

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
        public void UpdateMainTile(string balance, string income, string spending, string earnings)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();

            var content = new TileContent
            {
                Visual = new TileVisual
                {
                    TileMedium = GetBindingMediumContent(balance, income, spending, earnings),
                    TileWide = GetBindingWideContent(balance, income, spending, earnings),
                    TileLarge = GetBindingLargeContent(balance, income, spending, earnings)
                } 
            };

            // Update Tile
            TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(content.GetXml()));
        }

        private static TileBinding GetBindingMediumContent(string balance, string income, string spending, string earnings)
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
                        Text = balance,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                        HintWrap = true
                    },
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

        private static TileBinding GetBindingWideContent(string balance, string income, string spending, string earnings)
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
                        Text = balance,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },
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

        private static TileBinding GetBindingLargeContent(string balance, string income, string spending, string earnings)
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
                        Text = balance,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },
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
}