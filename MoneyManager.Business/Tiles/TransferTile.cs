using System;
using Windows.UI.StartScreen;
using MoneyManager.Foundation.OperationContracts;
using MoneyManager.OperationContracts;
using MoneyManager.Src;

namespace MoneyManager.Models.Tiles
{
    internal class TransferTile : Tile, ISecondTile
    {
        public const string Id = "AddTransferTile";

        public bool Exists
        {
            get { return Exists(Id); }
        }

        public async void Create()
        {
            await Create(new SecondaryTile(
                Id,
                Utilities.GetTranslation("AddTransferTileText"),
                "intake",
                new Uri("ms-appx:///Images/transferTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public async void Remove()
        {
            await Remove(new SecondaryTile(Id));
        }
    }
}
