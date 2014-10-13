using System;
using Windows.Foundation;
using Windows.UI.StartScreen;
using MoneyManager.OperationContracts;
using MoneyManager.Src;

namespace MoneyManager.Models.Tiles
{
    public class SpendingTile : Tile, ISecondTile
    {
        public const string SpendingTileId = "AddSpendingTile";

        public bool Exists
        {
            get { return Exists(SpendingTileId); }
        }

        public async void Create()
        {
            await Create(new SecondaryTile(
                SpendingTileId,
                Utilities.GetTranslation("AddSpendingTileText"),
                "intake",
                new Uri("ms-appx:///Images/spendingTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public async void Remove()
        {
            await Remove(new SecondaryTile(SpendingTileId));
        }
    }
}
