using Windows.UI.Notifications;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;
using NotificationsExtensions.TileContent;

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
                var tileContent =
                    TileContentFactory.CreateTileSquare310x310SmallImagesAndTextList04();
                tileContent.Image1.Src = "ms-appx:///Assets/Logo.png";
                tileContent.TextHeading1.Text = Strings.CashflowLabel;
                tileContent.TextWrap1.Text = income;
                tileContent.TextWrap2.Text = spending;
                tileContent.TextWrap3.Text = earnings;

                // Create a notification for the Wide310x150 tile using one of the available templates for the size.
                var wide310x150Content =
                    TileContentFactory.CreateTileWide310x150SmallImageAndText02();
                wide310x150Content.Image.Src = "ms-appx:///Assets/Logo.png";
                wide310x150Content.TextHeading.Text = Strings.CashflowLabel;
                wide310x150Content.TextBody1.Text = income;
                wide310x150Content.TextBody2.Text = spending;
                wide310x150Content.TextBody3.Text = earnings;

                // Create a notification for the Square150x150 tile using one of the available templates for the size.
                var square150x150Content =
                    TileContentFactory.CreateTileSquare150x150PeekImageAndText01();
                square150x150Content.Image.Src = "ms-appx:///Assets/Logo.png";
                square150x150Content.TextHeading.Text = Strings.CashflowLabel;
                square150x150Content.TextBody1.Text = income;
                square150x150Content.TextBody2.Text = spending;
                square150x150Content.TextBody3.Text = earnings;

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