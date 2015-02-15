using Windows.UI.Notifications;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;
using NotificationsExtensions.TileContent;

namespace MoneyManager {
    public class UserNotification :IUserNotification  {

        /// <summary>
        /// Sets the MainTile with new Information
        /// </summary>
        /// <param name="income">Income of these month</param>
        /// <param name="spending">Spending of these month</param>
        /// <param name="earnings">Earnings of these month </param>
        public void UpdateMainTile(double income, double spending, double earnings) {

            TileUpdateManager.CreateTileUpdaterForApplication().Clear();

            if (ServiceLocator.Current.GetInstance<TileSettingsViewModel>().ShowInfoOnMainTile) {
                ITileSquare310x310SmallImagesAndTextList04 tileContent =
                    TileContentFactory.CreateTileSquare310x310SmallImagesAndTextList04();
                tileContent.Image1.Src = "ms-appx:///Assets/Logo.png";
                tileContent.TextHeading1.Text = Translation.GetTranslation("CashflowTileLabel");
                tileContent.TextWrap1.Text = Translation.GetTranslation("IncomeTileLabel") + income;
                tileContent.TextWrap2.Text = Translation.GetTranslation("SpendingTileLabel") + spending;
                tileContent.TextWrap3.Text = Translation.GetTranslation("EarningTileLabel") + earnings;

                // Create a notification for the Wide310x150 tile using one of the available templates for the size.
                ITileWide310x150SmallImageAndText02 wide310x150Content =
                    TileContentFactory.CreateTileWide310x150SmallImageAndText02();
                wide310x150Content.Image.Src = "ms-appx:///Assets/Logo.png";
                wide310x150Content.TextHeading.Text = Translation.GetTranslation("CashflowTileLabel");
                wide310x150Content.TextBody1.Text = Translation.GetTranslation("IncomeTileLabel") + income;
                wide310x150Content.TextBody2.Text = Translation.GetTranslation("SpendingTileLabel") + spending;
                wide310x150Content.TextBody3.Text = Translation.GetTranslation("EarningTileLabel") + earnings;

                // Create a notification for the Square150x150 tile using one of the available templates for the size.
                ITileSquare150x150PeekImageAndText01 square150x150Content =
                    TileContentFactory.CreateTileSquare150x150PeekImageAndText01();
                square150x150Content.Image.Src = "ms-appx:///Assets/Logo.png";
                square150x150Content.TextHeading.Text = Translation.GetTranslation("CashflowTileLabel");
                square150x150Content.TextBody1.Text = Translation.GetTranslation("IncomeTileLabel") + income;
                square150x150Content.TextBody2.Text = Translation.GetTranslation("SpendingTileLabel") + spending;
                square150x150Content.TextBody3.Text = Translation.GetTranslation("EarningTileLabel") + earnings;

                // Attach the Square150x150 template to the Wide310x150 template.
                wide310x150Content.Square150x150Content = square150x150Content;

                // Attach the Wide310x150 template to the Square310x310 template.
                tileContent.Wide310x150Content = wide310x150Content;

                // Send the notification to the application? tile.
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileContent.CreateNotification());
            }
        }
    }
}