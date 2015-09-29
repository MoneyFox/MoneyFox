using Windows.UI.Notifications;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Localization;
using NotificationsExtensions.Tiles;

namespace MoneyManager.Windows
{
    public class UserNotification : IUserNotification
    {
        /// <summary>
        ///     Sets the MainTile with new Information
        /// </summary>
        /// <param name="income">Income of these month</param>
        /// <param name="spending">Spending of these month</param>
        /// <param name="earnings">Earnings of these month </param>
        public void UpdateMainTile(string income, string spending, string earnings)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();

            if (Mvx.Resolve<TileSettingsViewModel>().ShowInfoOnMainTile)
            {
                var bindingContent = new TileBindingContentAdaptive
                {
                    PeekImage = new TilePeekImage
                    {
                        Source = new TileImageSource("Assets/BadgeLogo.scale-400.png")
                    },
                    Children =
                    {
                        new TileText
                        {
                            Text = income,
                            Style = TileTextStyle.CaptionSubtle
                        },
                        new TileText
                        {
                            Text = spending,
                            Style = TileTextStyle.CaptionSubtle
                        },
                        new TileText
                        {
                            Text = earnings,
                            Wrap = true,
                            Style = TileTextStyle.Body
                        }
                    }
                };

                var binding = new TileBinding
                {
                    Branding = TileBranding.NameAndLogo,
                    DisplayName = Strings.ApplicationTitle,
                    Content = bindingContent
                };

                var content = new TileContent
                {
                    Visual = new TileVisual
                    {
                        TileMedium = binding,
                        TileWide = binding,
                        TileLarge = binding
                    }
                };
                // Update Tile
                TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(content.GetXml()));
            }
        }
    }
}