using System;
using Windows.UI.StartScreen;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Windows.Tile
{
    public class IncomeTile : Tile, ISecondTile
    {
        public const string ID = "AddIncomeTile";

        public bool Exists => TileExists(ID);

        public async void Create()
        {
            await Create(new SecondaryTile(
                ID,
                Translation.GetTranslation("AddIncomeTileText"),
                "intake",
                new Uri("ms-appx:///Images/incomeTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public async void Remove()
        {
            await Remove(new SecondaryTile(ID));
        }
    }
}