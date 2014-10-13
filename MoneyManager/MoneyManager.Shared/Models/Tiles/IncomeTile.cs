using System;
using Windows.UI.StartScreen;
using MoneyManager.OperationContracts;
using MoneyManager.Src;

namespace MoneyManager.Models.Tiles
{
    public class IncomeTile : Tile, ISecondTile
    {
        public const string Id = "AddIncomeTile";

        public bool Exists
        {
            get { return Exists(Id); }
        }
        
        public async void Create()
        {
            await Create(new SecondaryTile(
                Id,
                Utilities.GetTranslation("AddIncomeTileText"),
                "intake",
                new Uri("ms-appx:///Images/incomeTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public async void Remove()
        {
            await Remove(new SecondaryTile(Id));
        }
    }
}
