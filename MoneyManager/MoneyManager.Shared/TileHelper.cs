#region

using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MoneyManager.Business.Logic;
using MoneyManager.Business.Logic.Tile;
using MoneyManager.Foundation;
using MoneyManager.Views;
using NotificationsExtensions.TileContent;

#endregion

namespace MoneyManager
{
    public class TileHelper
    {
        public static void SetMainTile()
        {
            ITileSquare310x310SmallImagesAndTextList04 tileContent = TileContentFactory.CreateTileSquare310x310SmallImagesAndTextList04();
            tileContent.Image1.Src = "ms-appx:///Images/logoWide.png";
            tileContent.TextHeading1.Text = "Cashflow";
            tileContent.TextWrap1.Text = "Income: 500";
            tileContent.TextWrap2.Text = "Spending: 400";
            tileContent.TextWrap3.Text = "Earnings: 200";

            // Create a notification for the Wide310x150 tile using one of the available templates for the size.
            ITileWide310x150SmallImageAndText02 wide310x150Content = TileContentFactory.CreateTileWide310x150SmallImageAndText02();
            wide310x150Content.Image.Src = "ms-appx:///Images/logoSmall.png";
            wide310x150Content.TextHeading.Text = "Cashflow";
            wide310x150Content.TextBody1.Text = "Income: 500";
            wide310x150Content.TextBody2.Text = "Spending: 400";
            wide310x150Content.TextBody3.Text = "Earnings: 200";

            // Create a notification for the Square150x150 tile using one of the available templates for the size.
            ITileSquare150x150PeekImageAndText01 square150x150Content = TileContentFactory.CreateTileSquare150x150PeekImageAndText01();
            square150x150Content.Image.Src = "ms-appx:///Images/logoNormal.png";
            square150x150Content.TextHeading.Text = "Cashflow";
            square150x150Content.TextBody1.Text = "Income: 500";
            square150x150Content.TextBody2.Text = "Spending: 300";
            square150x150Content.TextBody3.Text = "Earnings: 200";

            // Attach the Square150x150 template to the Wide310x150 template.
            wide310x150Content.Square150x150Content = square150x150Content;

            // Attach the Wide310x150 template to the Square310x310 template.
            tileContent.Wide310x150Content = wide310x150Content;

            // Send the notification to the application? tile.
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileContent.CreateNotification());
        }

        public static void DoNavigation(string tileId)
        {
            switch (tileId)
            {
                case IncomeTile.Id:
                    TransactionLogic.GoToAddTransaction(TransactionType.Income);
                    ((Frame) Window.Current.Content).Navigate(typeof (AddTransaction));
                    break;

                case SpendingTile.Id:
                    TransactionLogic.GoToAddTransaction(TransactionType.Spending);
                    ((Frame) Window.Current.Content).Navigate(typeof (AddTransaction));
                    break;

                case TransferTile.Id:
                    TransactionLogic.GoToAddTransaction(TransactionType.Transfer);
                    ((Frame) Window.Current.Content).Navigate(typeof (AddTransaction));
                    break;
            }
        }
    }
}