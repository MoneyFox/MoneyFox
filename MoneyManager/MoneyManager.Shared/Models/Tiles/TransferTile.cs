using System;
using Windows.UI.StartScreen;
using MoneyManager.OperationContracts;
using MoneyManager.Src;

namespace MoneyManager.Models.Tiles
{
    public class TransferTile : Tile, ISecondTile
    {
        public const string TransferTileId = "AddTransferTile";

        public bool Exists
        {
            get { return Exists(TransferTileId); }
        }

        public async void Create()
        {
            await Create(new SecondaryTile(
                TransferTileId,
                Utilities.GetTranslation("AddTransferLabel"),
                "intake",
                new Uri("ms-appx:///Images/transferTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
