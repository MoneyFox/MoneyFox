using System;
using Windows.UI.StartScreen;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Windows.Tile
{
    public class TransferTile : Windows.Tile.Tile, ISecondTile
    {
        public const string ID = "AddTransferTile";

        public bool Exists => TileExists(ID);

        public async void Create()
        {
            await Create(new SecondaryTile(
                ID,
                Translation.GetTranslation("AddTransferTileText"),
                "intake",
                new Uri("ms-appx:///Images/transferTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public async void Remove()
        {
            await Remove(new SecondaryTile(ID));
        }
    }
}