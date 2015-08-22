using System;
using Windows.UI.StartScreen;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Windows.Tile
{
    public class SpendingTile : Tile, ISecondTile
    {
        public const string ID = "AddSpendingTile";

        public bool Exists => TileExists(ID);

        public async void Create()
        {
            await Create(new SecondaryTile(
                ID,
                Translation.GetTranslation("AddSpendingTileText"),
                "intake",
                new Uri("ms-appx:///Images/spendingTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public async void Remove()
        {
            await Remove(new SecondaryTile(ID));
        }
    }
}