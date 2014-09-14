using System;
using Windows.UI.StartScreen;

namespace MoneyManager.Src
{
    public class TileHelper
    {
        public static string IntakeTileId = "AddIntakeTile";

        public async static void CreateSecondaryTile()
        {
            var secondaryTile = new SecondaryTile(
                IntakeTileId,
                "Add Intake", 
                "intake", 
                new Uri("ms-appx:///Assets/Logo.scale-240.png", UriKind.Absolute),
                TileSize.Square150x150);
            await secondaryTile.RequestCreateAsync();
        }

        public static void DoNavigation(string tileId)
        {
            if (tileId == IntakeTileId)
            {
                TransactionHelper.GoToTransaction(TransactionType.Spending.ToString());
            }
        }
    }
}
